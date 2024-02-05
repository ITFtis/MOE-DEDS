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
            var TabulationList = Db.Tabulation.Where(q => q.Act == true).ToList();
            ////var TabulationList = Db.Tabulation.Where(q => q.Act == true).Take(30).ToList();

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


            string[] dotNum = {
                "...........................................",
                ".......................................",
                "...........................................",
                ".............................",
                ".....................................",
                ".....................................",
                "...........................",
                "...........................",
                "..",
                ".............................................",
                "..........................................",
                "..........................................",
                "..........................................",
                "........................ ..................",
                "........................ ..................",
                "........................ ..................",
                "........................ ..................",
                "........................ ..................",
                "........................ ..................",
                "........................ ..................",
                "................ .......................",
                "................. .......................",
                ".................... ...................",
                ".................... ...................",
                ".................... ...................",
                ".................... ...................",
                ".................... ...................",
                ".................... ...................",
                ".................... ...................",
                "................... ......................",
                "................. ......................"
            };
            int[] page = { 4, 4, 4, 4, 4, 5, 5, 5, 6, 9, 11, 12, 14, 16, 18, 19, 21, 23, 25, 26, 28, 30, 32, 33, 35, 37, 38, 40, 42, 44, 45 };
            int index = 0;
            foreach (var item in CategoryIdList)
            {
                if (item.Max == "-")
                {
                    blank = newDoc.CreateParagraph();
                    title = blank.CreateRun();
                    title.FontFamily = "標楷體";
                    title.FontSize = 12;
                    title.SetText(item.Name + dotNum[index] + page[index]);
                    index++;
                }
            }
            title.AddBreak();

            //硬刻 表頭表格換頁
            int[] breaktablenum = { 5, 9, 14, 21, 28, 34, 41, 55, 75, 82, 89, 96, 103, 110, 117, 124, 131, 138, 145, 151, 158, 165, 172, 179, 186, 193, 200, 207, 214, 221, 228, 240, 247, 262, 270 };
            for (int sheetNum = 1; sheetNum <= CategoryIdList.Count; sheetNum++)
            {
                var tquery = TabulationList.Where(w => w.CategoryId == CategoryIdList[sheetNum - 1].CategoryId).OrderBy(x => x.Sort).ToList();
                // 表格標題
                var AA = newDoc.CreateParagraph();
                //需要加入目錄的表格標題才設定樣式
                if (CategoryIdList[sheetNum - 1].Max == "-")
                {
                    AA.Style = "Heading1";
                }
                var newRun = AA.CreateRun();
                newRun.AppendText(CategoryIdList[sheetNum - 1].Name);
                newRun.FontFamily = "標楷體";
                newRun.FontSize = 12;
                // 創建一個Table
                XWPFTable targetTable = newDoc.CreateTable(tquery.Count() + 1, 7);
                targetTable.Width = 5000; //設定表格寬度
                for (int MemberNum = 0; MemberNum < (tquery.Count() + 1); MemberNum++)
                {
                    XWPFTableRow newRow = targetTable.GetRow(MemberNum);
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
                                        if (bquery.Note.Count() >= 5 && bquery.Note.Count() < 10)
                                        {
                                            fontrun.SetText(bquery.Note.Substring(0, 5));
                                            var newrun = newCell.AddParagraph().CreateRun();
                                            newrun.FontFamily = "標楷體";
                                            newrun.FontSize = 12;
                                            newrun.SetText(bquery.Note.Substring(5));
                                        }
                                        else if (bquery.Note.Count() >= 10 && bquery.Note.Count() < 15)
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
                                        else if (bquery.Note.Count() >= 15 && bquery.Note.Count() < 20)
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
                                        else if (bquery.Note.Count() >= 20 && bquery.Note.Count() < 25)
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
                                        else if (bquery.Note.Count() >= 25 && bquery.Note.Count() < 30)
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
                                        else if (bquery.Note.Count() >= 30)
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
                //targetTable.RemoveRow(0);
                if (breaktablenum.Contains(sheetNum))
                {
                    newDoc.CreateParagraph().CreateRun().AddBreak();
                }
                else
                {
                    newDoc.CreateParagraph();
                }

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
            doc.Close();

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
            foreach (var item in UnitList)
            {
                foreach (var eachitem in item.Category)
                {
                    Result.Add(new Category
                    {
                        CategoryId = eachitem.CategoryId,
                        Name = eachitem.Name,
                        Max = eachitem.Max
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
        }

    }
}