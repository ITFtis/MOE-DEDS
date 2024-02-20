using DEDS.Models;
using DEDS.Models.Comm;
using DEDS.Models.Manager;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NPOI.XWPF.UserModel;
using Microsoft.Office.Interop.Word;
using Document = Microsoft.Office.Interop.Word.Document;
using System.IO;
using NPOI.OpenXmlFormats.Wordprocessing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Dou.Misc;
using Dou.Controllers;
using System.Web.UI.WebControls.WebParts;
using NPOI.Util;
using System.Data.Entity.Infrastructure;
using Google.Protobuf.WellKnownTypes;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ContactExport", Name = "通聯造冊匯出", MenuPath = "通聯資料", Action = "Index", Index = 4, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class ContactExportController : Dou.Controllers.AGenericModelController<object>
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        // GET: ContactExport
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<object> GetModelEntity()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("ContactExport/ExportPDF")]
        public ActionResult ExportPDF()
        {
            // 準備資料進行核對
            var CategoryIdList = GetCategoryIdList();
            var BaseList = Db.UserBasic.ToList(); // 基本資料表            
            var TabulationList = Db.Tabulation.Where(q => q.Act == true)
                                    .AsEnumerable().OrderBy(b => b.Order).ToList();
            ////var TabulationList = Db.Tabulation.Where(q => q.Act == true)
            ////                        .AsEnumerable().OrderBy(b => b.Order)                                    
            ////                        .ToList().Take(300);

            var PositionList = fun.GetPosition();



            XWPFDocument doc1 = ReadDocx(Server.MapPath("../") + "Data/Comm/Contact_local.docx");
            // 複製表格
            XWPFTable Table = doc1.Tables[0];

            // 創建新的 Word 文件
            XWPFDocument newDoc = new XWPFDocument();

            //設定邊界
            newDoc.Document.body.sectPr = new CT_SectPr();
            CT_SectPr m_SectPr = newDoc.Document.body.sectPr;
            m_SectPr.pgSz.h = (ulong)16838;
            m_SectPr.pgSz.w = (ulong)11906;

            m_SectPr.pgMar.left = (ulong)800;
            m_SectPr.pgMar.right = (ulong)800;
            m_SectPr.pgMar.top = (ulong)800;
            m_SectPr.pgMar.left = (ulong)800;

            //新增時間
            var header = newDoc.CreateParagraph();
            header.Alignment = ParagraphAlignment.RIGHT;
            var headerrun = header.CreateRun();
            headerrun.FontFamily = "標楷體";
            headerrun.FontSize = 10;
            headerrun.SetText("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

            var blank = newDoc.CreateParagraph();
            var title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 12;
            title.SetText("◆ 公務用資料，限相關人員使用，嚴禁其他用途。");

            blank = newDoc.CreateParagraph();
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 12;
            title.SetText("◆ 請遵循「個人資料保護法」相關規定，限公務使用，禁止對外洩漏及公開個人資料。");

            blank = newDoc.CreateParagraph();
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 12;
            title.SetText("◆ 保管人遇職務異動時列入移交。");
            for (int r = 0; r < 12; r++) { newDoc.CreateParagraph(); }
            //標題
            blank = newDoc.CreateParagraph();
            blank.Alignment = ParagraphAlignment.CENTER;
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 24;
            title.IsBold = true;
            title.SetText("環境部" + (DateTime.Now.Year - 1911) + "年度環境污染事故");

            blank = newDoc.CreateParagraph();
            blank.Alignment = ParagraphAlignment.CENTER;
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 24;
            title.IsBold = true;
            title.SetText("(含春節期間)緊急應變通聯手冊");

            for (int r = 0; r < 18; r++) { newDoc.CreateParagraph(); }
            blank = newDoc.CreateParagraph();
            blank.Alignment = ParagraphAlignment.CENTER;
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 24;
            title.IsBold = true;
            title.SetText("環境部彙編");
            //blank = newDoc.CreateParagraph();
            //blank.Alignment = ParagraphAlignment.CENTER;
            //title = blank.CreateRun();
            //title.FontFamily = "標楷體";
            //title.FontSize = 24;
            //title.IsBold = true;
            //title.SetText((DateTime.Now.Year - 1911) + "年" + DateTime.Now.Month + "月");
            title.AddBreak();

            //目錄
            blank = newDoc.CreateParagraph();
            blank.Alignment = ParagraphAlignment.CENTER;
            title = blank.CreateRun();
            title.FontFamily = "標楷體";
            title.FontSize = 28;
            title.IsBold = true;
            title.SetText("目    錄");

            //頁數影響目錄換行，用Json控制(\Data\Comm\DocDot.json)
            string[] dotNum = Function.GetDocDot().Where(a => a.Id == "1").FirstOrDefault().Dot;
            //string[] dotNum = {
            //    "...........................................",
            //    ".......................................",
            //    "...........................................",
            //    ".............................",
            //    ".....................................",
            //    ".....................................",
            //    "...........................",
            //    ".........................",
            //    ".",
            //    "............................................",
            //    "..........................................",
            //    "..........................................",
            //    "..........................................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "........................ ..................",
            //    "................ .......................",
            //    "................. .......................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    ".................... ...................",
            //    "................... ......................",
            //    "................. ......................"
            //};            
            ////int[] page = { 4, 4, 4, 4, 4, 5, 5, 5, 6, 9, 10, 12, 14, 16, 17, 19, 21, 23, 24, 26, 28, 30, 31, 33, 35, 36, 38, 40, 42, 44, 45 };
            int index = 0;
            foreach (var item in CategoryIdList)
            {
                if (item.Max == "-")
                {
                    blank = newDoc.CreateParagraph();
                    title = blank.CreateRun();
                    title.FontFamily = "標楷體";
                    title.FontSize = 12;
                    //title.SetText(item.Name + dotNum[index] + string.Format("[{0}]", item.CategoryId));
                    title.SetText(item.Name + dotNum[index] + string.Format("[{0}]", item.CategoryId));
                    index++;
                }
            }
            title.AddBreak();

            //硬刻 表頭表格換頁            
            ////int[] breaktablenum = { 5, 9, 16, 23, 30, 36, 43, 57, 64, 70, 77, 84, 91, 98, 105, 112, 119, 126, 133, 140, 147, 153, 160, 167, 174, 181, 188, 195, 202, 209, 216, 223, 229, 249, 264, 272 };            
            int pageNumber = 4;//目錄後的page編號
            int pagePreNumberAdd = 0;//前一個Table換頁數量
            int countBG = 0; //大標題總計
            int countRow = 0;//當下頁(Row總計)
            int[] logRowLists;//紀錄Row有幾列

            ////int preHight = 0;//前一個高度 xxxxxxxxx                            
            for (int sheetNum = 1; sheetNum <= CategoryIdList.Count; sheetNum++)
            {
                var tquery = TabulationList.Where(w => w.CategoryId == CategoryIdList[sheetNum - 1].CategoryId).OrderBy(x => x.Sort).ToList();

                #region  自動換列：Table高度
                int tabelRow = 0;//該筆Table(Row總計)
                logRowLists = new int[(tquery.Count() + 1)];
                for (int MemberNum = 0; MemberNum < (tquery.Count() + 1); MemberNum++)
                {
                    if (MemberNum == 0)
                    {
                        logRowLists[MemberNum] = 0;
                    }
                    else
                    {
                        var bquery = BaseList.Where(w => w.UID == tquery[MemberNum - 1].UID).FirstOrDefault();
                        var PositionName = fun.GetPositionName(PositionList, bquery.PositionId);

                        if (PositionName == null)
                        {
                            Logger.Log.For(this).Error("PositionName：職稱為Null：");
                            continue;
                        }

                        int maxRow = 0;    //cell內最多的row數量
                        int cRow = 0;       //目前cell的row數量
                        for (int i = 0; i < 7; i++)
                        {
                            cRow = 1;
                            switch (i)
                            {
                                case 0:
                                    if (PositionName.Count() > 5 && PositionName.Count() <= 10)
                                    {
                                        cRow = 2;
                                    }
                                    else if (PositionName.Count() > 10 && PositionName.Count() <= 15)
                                    {
                                        cRow = 3;
                                    }
                                    else if (PositionName.Count() > 15 && PositionName.Count() <= 20)
                                    {
                                        cRow = 4;
                                    }
                                    else if (PositionName.Count() > 20 && PositionName.Count() <= 25)
                                    {
                                        cRow = 5;
                                    }
                                    else if (PositionName.Count() > 25 && PositionName.Count() <= 30)
                                    {
                                        cRow = 6;
                                    }
                                    else if (PositionName.Count() > 30)
                                    {
                                        cRow = 7;
                                    }
                                    else
                                    {                                        
                                        //fontrun.SetText(PositionName.PadRight(5, ' '));
                                        cRow = 1;
                                    }
                                    break;                                
                                case 6:
                                    if (bquery.Note != null)
                                    {
                                        if (bquery.Note.Count() >= 5 && bquery.Note.Count() <= 10)
                                        {
                                            cRow = 2;
                                        }
                                        else if (bquery.Note.Count() >= 10 && bquery.Note.Count() <= 15)
                                        {
                                            cRow = 3;
                                        }
                                        else if (bquery.Note.Count() >= 15 && bquery.Note.Count() <= 20)
                                        {
                                            cRow = 4;
                                        }
                                        else if (bquery.Note.Count() >= 20 && bquery.Note.Count() <= 25)
                                        {
                                            cRow = 5;
                                        }
                                        else if (bquery.Note.Count() >= 25 && bquery.Note.Count() <= 30)
                                        {
                                            cRow = 6;
                                        }
                                        else if (bquery.Note.Count() > 30)
                                        {
                                            cRow = 7;
                                        }
                                        else
                                        {
                                            //fontrun.SetText(bquery.Note);
                                            cRow = 1;
                                        }
                                    }
                                    break;
                            }

                            if (maxRow < cRow)
                            {
                                maxRow = cRow;
                            }
                        }

                        logRowLists[MemberNum] = maxRow;
                        tabelRow += maxRow; //資料表數量(資料列)                                                
                        maxRow = 0;
                    }
                }

                //有資料列
                int xxxxxxx = sheetNum;
                if (tabelRow > 0)
                {
                    tabelRow = tabelRow + 1; //資料表數量(欄位列 + 資料列)
                    countBG++;
                    countRow = countRow + tabelRow;
                    
                    int BGHight = 405;
                    int relHeight = (countBG * BGHight) + (countRow * 250) + ((countBG - 1) * BGHight); //大標題高度 + (資料表高度) + 結尾列

                    ////正祥換頁判斷式
                    ////A4（橫向）：W = 11906 H = 16838
                    ////A4（縱向）：W = 16838 H = 11906
                    ////A5 ： W = 8390 H = 11906
                    ////A6 ： W = 5953 H = 8390
                    int wordHeight = 11906;                    
                    if (wordHeight - relHeight < BGHight)  //融下大標題高度(BGHight)
                    {                        
                        newDoc.CreateParagraph().CreateRun().AddBreak();
                        ////單一資料表數量超過1頁(注意：單一列row data的cell有跨頁，如標題、備註)
                        ////tempHeight:第1頁要扣掉大標題高度 - 標題高度
                        int th = 250;
                        //int brth = 200;  //換行高度
                        int tempHeight = (wordHeight - BGHight - th);// 
                        if ((tabelRow - 1) * th > tempHeight)           //tableRow含th欄位
                        {
                            pageNumber++;

                            //int sumH = -th;
                            int sumH = 0;
                            int presumH = 0;
                            for (int MemberNum = 0; MemberNum < logRowLists.Count(); MemberNum++)
                            {
                                if (MemberNum == 0)
                                {
                                }
                                else
                                {                                    
                                    //驗證是否能存入下下一列資料
                                    int h = presumH + ((logRowLists[MemberNum - 1] + logRowLists[MemberNum]) * th);
                                    if(h >= tempHeight)
                                    {
                                        //上一位user
                                        var bquery = BaseList.Where(w => w.UID == tquery[MemberNum - 2].UID).FirstOrDefault();

                                        //換列數量(1個"[", 表示1列)
                                        string brNum = "";
                                        for (int ss = 0; ss < (logRowLists[MemberNum - 1]); ss++)
                                        {
                                            brNum += "[";
                                            sumH += th;//brth;
                                        }
                                        //sumH = sumH + (logRowLists[MemberNum - 1] * th);

                                        int lin = 0;
                                        for (; (sumH <= tempHeight); lin++)
                                        {
                                            //提供cell換列次數，方能換頁
                                            brNum += "[";

                                            sumH = sumH + th;// brth;
                                        }                                        

                                        if(lin == 0 || lin >= (logRowLists[MemberNum]))
                                        {
                                            //剛好塞滿，不用多標頭
                                            tempHeight = wordHeight;
                                            brNum += "{";

                                            //tempHeight = wordHeight - th;    //多一行標頭
                                            sumH = 0;
                                        }
                                        else
                                        {
                                            tempHeight = wordHeight - th;    //多一行標頭
                                            //sumH = (logRowLists[MemberNum - 1] * th);
                                            
                                            //reset 仿製logRowLists[0]
                                            logRowLists[MemberNum - 2] = 0;
                                            MemberNum--;
                                            sumH = 0;
                                        }

                                        bquery.Name = bquery.Name + "\n" + brNum + "a";  //test 資料呈現(才執行換行)                                            

                                        //ini
                                        ////if (lin >= (logRowLists[MemberNum]))
                                        ////{
                                        ////    //////剛好塞滿，不用換列
                                        ////    ////tempHeight = wordHeight;

                                        ////    sumH = 0;
                                        ////}
                                        ////else
                                        ////{
                                        ////    sumH = (logRowLists[MemberNum] * th);                                            
                                        ////}

                                        presumH = sumH;

                                        pagePreNumberAdd++;
                                    }
                                    else
                                    {
                                        sumH = sumH + (logRowLists[MemberNum - 1] * th);
                                        presumH = sumH;
                                    }

                                    
                                }
                            }
                        }
                        else
                        {
                            ////defalut
                            pageNumber++;
                        }

                        countRow = tabelRow;
                        countBG = 1;
                    }

                    ////preHight = (countBG * BGHight) + (countRow * 250) + ((countBG - 1) * BGHight); //大標題高度 + (資料表高度) + 結尾列  xxxxxxxxx                    
                    newDoc.FindAndReplaceText(string.Format("[{0}]", CategoryIdList[sheetNum - 1].CategoryId), pageNumber.ToString());
                    pageNumber = pageNumber + pagePreNumberAdd;
                    pagePreNumberAdd = 0;
                }
                
                #endregion

                // 表格標題
                var AA = newDoc.CreateParagraph();
                //需要加入目錄的表格標題才設定樣式
                if (CategoryIdList[sheetNum - 1].Max == "-")
                {
                    AA.Style = "Heading1";
                }
                var newRun = AA.CreateRun();
                newRun.AppendText(CategoryIdList[sheetNum - 1].Name);
                //newRun.AppendText(CategoryIdList[sheetNum - 1].Name + "(" + sheetNum.ToString() + ")");
                newRun.FontFamily = "標楷體";
                newRun.FontSize = 12;
                // 創建一個Table
                XWPFTable targetTable = newDoc.CreateTable(tquery.Count() + 10, 7);
                targetTable.Width = 5000; //設定表格寬度
                int tnum = 0;//儲存格插入row
                for (int MemberNum = 0; MemberNum < (tquery.Count() + 1); MemberNum++)
                {
                    XWPFTableRow newRow = targetTable.GetRow(MemberNum + tnum);
                    if (MemberNum == 0)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            XWPFTableCell newCell = newRow.GetCell(i);
                            var fontP = newCell.AddParagraph();
                            fontP.IsWordWrapped = true;
                            fontP.Alignment = ParagraphAlignment.CENTER;
                            var fontrun = fontP.CreateRun();
                            fontrun.FontFamily = "標楷體";
                            fontrun.FontSize = 12;

                            if (i == 0)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "1300";
                            }
                            else if (i == 1)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "850";
                            }
                            else if (i == 2)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "1400";
                            }
                            else if (i == 3)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "700";
                                //fontrun.SetText("姓名");
                            }
                            else if (i == 4)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "1400";
                            }
                            else if (i == 5)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "3200";
                            }
                            else if (i == 6)
                            {
                                CT_TblWidth cellWidth = newCell.GetCTTc().AddNewTcPr().AddNewTcW();
                                cellWidth.type = ST_TblWidth.dxa;
                                cellWidth.w = "1300";
                            }

                            fontrun.SetText(Table.Rows[0].GetTableCells()[i].GetText());

                            newCell.RemoveParagraph(0);
                        }
                    }
                    else
                    {
                        var bquery = BaseList.Where(w => w.UID == tquery[MemberNum - 1].UID).FirstOrDefault();
                        var PositionName = fun.GetPositionName(PositionList, bquery.PositionId);
                                                
                        newRow.Height = 250;
                        for (int i = 0; i < 7; i++)
                        {
                            if (i == 0)
                            {
                                //找不到對應職稱，跳下一個cell
                                if (PositionName == null)
                                    continue;

                                if (bquery.Name.IndexOf("[") > -1)
                                {
                                    //儲存格多資料，換列

                                    //(a)換列
                                    var pretb = targetTable.GetRow(MemberNum + tnum - 1);
                                    int n = bquery.Name.Where(a => a == '[').Count();
                                    for (int ss = 0; ss < n; ss++)
                                    {
                                        //pretb.GetCell(1).AddParagraph().CreateRun().SetText("辦理");
                                        //pretb.GetCell(1).AddParagraph().CreateRun().AddBreak(BreakType.PAGE);
                                        
                                        XWPFTableCell pgCell = pretb.GetCell(1);
                                        var pgFont = pgCell.AddParagraph();
                                        pgFont.IsWordWrapped = true;
                                        pgFont.Alignment = ParagraphAlignment.CENTER;
                                        var pgRun = pgFont.CreateRun();
                                        pgRun.FontFamily = "標楷體";
                                        pgRun.FontSize = 12;

                                        pgRun.SetText("辦理");
                                    }

                                    //(b)標題
                                    if (bquery.Name.IndexOf("{") == -1)
                                    {
                                        var tb = targetTable.GetRow(0);
                                        XWPFTableRow trow = new XWPFTableRow(tb.GetCTRow().Copy(), targetTable);
                                        newRow.Height = newRow.Height;
                                        newRow.GetCell(0).SetParagraph(trow.GetCell(0).Paragraphs[0]);
                                        newRow.GetCell(1).SetParagraph(trow.GetCell(1).Paragraphs[0]);
                                        newRow.GetCell(2).SetParagraph(trow.GetCell(2).Paragraphs[0]);
                                        newRow.GetCell(3).SetParagraph(trow.GetCell(3).Paragraphs[0]);
                                        newRow.GetCell(4).SetParagraph(trow.GetCell(4).Paragraphs[0]);
                                        newRow.GetCell(5).SetParagraph(trow.GetCell(5).Paragraphs[0]);
                                        newRow.GetCell(6).SetParagraph(trow.GetCell(6).Paragraphs[0]);

                                        tnum++;
                                        newRow = targetTable.GetRow(MemberNum + tnum);
                                    }

                                    bquery.Name = bquery.Name.Replace("[", "").Replace("{", "");
                                }
                            }

                            XWPFTableCell newCell = newRow.GetCell(i);
                            var fontP = newCell.AddParagraph();
                            fontP.IsWordWrapped = true;
                            var fontrun = fontP.CreateRun();
                            fontrun.FontFamily = "標楷體";
                            fontrun.FontSize = 12;
                            switch (i)
                            {
                                case 0:
                                    if (PositionName.Count() > 5 && PositionName.Count() <= 10)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5));
                                    }
                                    else if (PositionName.Count() > 10 && PositionName.Count() <= 15)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(10));
                                    }
                                    else if (PositionName.Count() > 15 && PositionName.Count() <= 20)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(10, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(15));
                                    }
                                    else if (PositionName.Count() > 20 && PositionName.Count() <= 25)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(10, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(15, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(20));
                                    }
                                    else if (PositionName.Count() > 25 && PositionName.Count() <= 30)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(10, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(15, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(20, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(25));
                                    }
                                    else if (PositionName.Count() > 30)
                                    {
                                        fontrun.SetText(PositionName.Substring(0, 5));
                                        var newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(5, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(10, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(15, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(20, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(25, 5));
                                        newrun = newCell.AddParagraph().CreateRun();
                                        newrun.FontFamily = "標楷體";
                                        newrun.FontSize = 12;
                                        newrun.SetText(PositionName.Substring(30));
                                    }
                                    else
                                    {
                                        ////fontrun.SetText(PositionName);
                                        fontrun.SetText(PositionName.PadRight(5, ' '));
                                    }
                                    break;
                                case 1:
                                    fontrun.SetText(bquery.Name);
                                    break;
                                case 2:
                                    fontP.Alignment = ParagraphAlignment.CENTER;
                                    fontrun.SetText(bquery.OfficePhone.Split('#')[0]);
                                    break;
                                case 3:
                                    fontP.Alignment = ParagraphAlignment.CENTER;
                                    fontrun.SetText(bquery.OfficePhone.Split('#')[1]);
                                    break;
                                case 4:
                                    fontP.Alignment = ParagraphAlignment.CENTER;
                                    fontrun.SetText(bquery.MobilePhone);
                                    break;
                                case 5:
                                    fontP.Alignment = ParagraphAlignment.LEFT;
                                    fontrun.SetText(bquery.Email);
                                    break;
                                case 6:
                                    if (bquery.Note != null)
                                    {
                                        if (bquery.Note.Count() >= 5 && bquery.Note.Count() <= 10)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5));
                                        }
                                        else if (bquery.Note.Count() >= 10 && bquery.Note.Count() <= 15)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(10));
                                        }
                                        else if (bquery.Note.Count() >= 15 && bquery.Note.Count() <= 20)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(10, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(15));
                                        }
                                        else if (bquery.Note.Count() >= 20 && bquery.Note.Count() <= 25)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(10, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(15, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(20));
                                        }
                                        else if (bquery.Note.Count() >= 25 && bquery.Note.Count() <= 30)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(10, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(15, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(20, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(25));
                                        }
                                        else if (bquery.Note.Count() > 30)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(10, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(15, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(20, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(25, 5));
                                            newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(30));
                                        }
                                        else
                                        {
                                            fontrun.SetText(bquery.Note);
                                        }
                                    }
                                    break;
                            }
                            newCell.RemoveParagraph(0);
                        }
                    }
                    //newRow.RemoveCell(0);
                }

                //刪除多餘row
                for (int i = targetTable.Rows.Count - 1; i >= 0; i--)
                {
                    int MemberNum = tquery.Count() + 1;
                    if (i >= (MemberNum + tnum))
                    targetTable.RemoveRow(i);
                }

                //targetTable.RemoveRow(0);

                ////承億換頁判斷式
                ////if (breaktablenum.Contains(sheetNum))
                ////{
                ////    newDoc.CreateParagraph().CreateRun().AddBreak();
                ////}
                ////else
                ////{
                ////    newDoc.CreateParagraph();
                ////}

                newDoc.CreateParagraph();

            }

            XWPFStyles styleF = newDoc.CreateStyles();

            styleF.SetEastAsia("Biaukai");
            styleF.SetSpellingLanguage("English");

            //var footer = newDoc.CreateHeaderFooterPolicy().CreateFooter(ST_HdrFtr.@default).CreateParagraph();
            //var footerRun = footer.CreateRun();
            //int pages = newDoc.GetProperties().ExtendedProperties.GetUnderlyingProperties().Pages;
            //footerRun.AppendText(pages.ToString());

            // 創建頁尾
            var footer = newDoc.CreateHeaderFooterPolicy().CreateFooter(ST_HdrFtr.@default);
            // 在頁尾添加頁碼
            var paragraph = footer.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;
            paragraph.GetCTP().AddNewR().AddNewFldChar().fldCharType = ST_FldCharType.begin;
            paragraph.GetCTP().AddNewR().AddNewInstrText().Value = " PAGE ";
            paragraph.GetCTP().AddNewR().AddNewFldChar().fldCharType = ST_FldCharType.separate;
            paragraph.GetCTP().AddNewR().AddNewFldChar().fldCharType = ST_FldCharType.end;



            string text = Dou.Context.CurrentUser<User>().Id + " 僅限公務使用";
            string filepath = Server.MapPath("../") + "Data/Comm/Export/" + Dou.Context.CurrentUser<User>().Id;
            //新增文字浮水印
            //var hfPolicy = newDoc.CreateHeaderFooterPolicy();
            //hfPolicy.CreateWatermark(text);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }



            FileStream file = new FileStream(filepath + "/Contact.docx", FileMode.Create, FileAccess.Write);
            newDoc.Write(file);
            file.Close();

            // 轉換成pdf
            Application app = new Application();
            // 開啟 Word 文件
            Document doc = app.Documents.Open(filepath + "/Contact.docx");
            // 轉換為 PDF
            doc.ExportAsFixedFormat(filepath + "/Contact.pdf", WdExportFormat.wdExportFormatPDF);
            //doc.Close();
            ((Microsoft.Office.Interop.Word._Document)doc).Close(false);
            ((Microsoft.Office.Interop.Word._Application)app).Quit(false);

            //新增浮水印
            string outFilePath = filepath + "/Contact_Watermark.pdf";

            FileStream outStream = new FileStream(outFilePath, FileMode.Create);

            PdfReader pdfReader = new PdfReader(filepath + "/Contact.pdf");

            PdfStamper pdfStamper = new PdfStamper(pdfReader, outStream);

            int total = pdfReader.NumberOfPages + 1;
            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            PdfContentByte waterMarkContent;
            BaseFont font = BaseFont.CreateFont("C:\\Windows\\fonts\\mingliu.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            PdfGState gs = new PdfGState();

            for (int i = 1; i < total; i++)
            {
                //在內容上方加水印（下方加水印參考上面圖片程式碼做法）
                waterMarkContent = pdfStamper.GetOverContent(i);
                //透明度
                gs.FillOpacity = 0.1f;
                waterMarkContent.SetGState(gs);
                //寫入文字
                waterMarkContent.BeginText();
                waterMarkContent.SetColorFill(Color.GRAY);
                waterMarkContent.SetFontAndSize(font, 50);
                waterMarkContent.SetTextMatrix(0, 0);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 200, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 200, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 400, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 400, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 600, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 600, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 800, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 800, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 1000, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 1000, 45);

                waterMarkContent.EndText();
            }
            pdfStamper.Close();
            pdfReader.Close();

            Stream streamResult = new FileStream(outFilePath, FileMode.Open);

            Stream result = new MemoryStream();

            streamResult.CopyTo(result);

            streamResult.Close();

            result.Position = 0;

            return Json(new { success = true, Account = Dou.Context.CurrentUser<User>().Id }, JsonRequestBehavior.AllowGet);
        }

        public List<Category> GetCategoryIdList()
        {
            var UnitList = fun.GetUnit();
            List<Category> Result = new List<Category>();
            int order = 0;

            foreach (var item in UnitList)
            {
                foreach (var eachitem in item.Category)
                {
                    order++;
                    Result.Add(new Category
                    {
                        CategoryId = eachitem.CategoryId,
                        Name = eachitem.Name,
                        Max = eachitem.Max,
                        Order = order,
                    });
                }

            }
            return Result;
        }

        static XWPFDocument ReadDocx(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return new XWPFDocument(fs);
            }
        }

        public class Category
        {
            public string CategoryId { get; set; }
            public string Name { get; set; }

            public string Max { get; set; }

            public int Order { get; set; }
        }

    }
}