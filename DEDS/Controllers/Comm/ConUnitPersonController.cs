using Antlr.Runtime.Misc;
using DEDS.Models;
using DEDS.Models.Comm;
using DEDS.Models.Manager;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using EnumsNET;
using MathNet.Numerics.Statistics.Mcmc;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.SS.UserModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.Office.Interop.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Office.Interop.Word;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dou.Misc.Attr;
using System.Diagnostics;
using System.ComponentModel;
using NPOI.SS.Formula.Functions;


namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ConUnitPerson", Name = "應變人員清冊", MenuPath = "幕僚/窗口", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ConUnitPersonController : Dou.Controllers.AGenericModelController<ConUnitPerson>
    {
        // GET: ConUnitPerson
        public ActionResult Index()
        {
            var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();

            //環境部(23)檢視所有單位資料，但只能修改自己
            string unit = Dou.Context.CurrentUser<DEDS.Models.Manager.User>().Unit;            
            if (unit == "23")
            {
                //全部
                ViewBag.IsView = false;
            }
            else
            {
                ViewBag.IsView = true;
            }

            //admin最大權限
            if (!user.IsManager)
            {
                string conUnit = user.ConUnit;
                var v = DEDS.Models.Comm.ConUnitCodeItems.ConUnitCodes.Where(a => a.Code == conUnit).FirstOrDefault();
                if (v != null)
                {
                    ViewBag.LoginConUnitName = v.Name;
                }
                else
                {
                    //User沒設定應變單位，預設xx，view控制不顯示編輯功能
                    ViewBag.LoginConUnitName = "xx";
                }

                //IsOrgStaff 幕僚人員                
                ViewBag.IsOrgStaff = IsOrgStaffByName(user.Name);
            }

            ViewBag.LoginIsManager = user.IsManager;
            ViewBag.LoginConUnit = user.ConUnit;

            return View();
        }

        protected override IModelEntity<ConUnitPerson> GetModelEntity()
        {
            return new ModelEntity<ConUnitPerson>(new DouModelContextExt());
        }

        protected override IEnumerable<ConUnitPerson> GetDataDBObject(IModelEntity<ConUnitPerson> dbEntity, params KeyValueParams[] paras)
        {
            var iquery = base.GetDataDBObject(dbEntity, paras);
            iquery = GetOutputData(iquery, paras);

            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序(備註：應變單位(縣市)，因特殊處理造成ConUnit變中文，目前使用可接受)                
                iquery = iquery.OrderBy(a => a.EditSort)
                            .ThenBy(a => a.ConUnitSort).ThenBy(a => a.PSort);                
            }

            return iquery;
        }

        protected override void AddDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BId = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);

            //寄發承辦
            string diffNote = "人員新增";
            ToSend("Add", f, diffNote);
        }

        protected override void UpdateDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            //原資料
            var before = GetModelEntity().GetAll().Where(a => a.Id == f.Id).First();

            f.UDate = DateTime.Now;
            f.UId = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;
            f.ConfirmDate = f.UDate;

            base.UpdateDBObject(dbEntity, objs);

            //寄發承辦
            List<string> diffNames = new List<string>();
            PropertyInfo[] properties = before.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty);
            ConUnitPerson menuDefAttribute = (ConUnitPerson)before.GetType().GetConstructor(new Type[0]).Invoke(new object[0]);
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                if (propertyInfo.Name == "UDate" || propertyInfo.Name == "UId" || propertyInfo.Name == "UName")
                    continue;

                try
                {
                    if (propertyInfo.CanWrite)
                    {
                        string diffName = "";
                        var a = propertyInfo.GetValue(before);
                        var b = propertyInfo.GetValue(f);

                        var attr = propertyInfo?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                        diffName = attr?.GetName() ?? propertyInfo.Name;

                        string s1 = a == null ? "" : a.ToString();
                        string s2 = b == null ? "" : b.ToString();

                        if (s1 != s2)
                            diffNames.Add(diffName);
                    }
                }
                catch (Exception value)
                {
                    Debug.WriteLine(value, ToString());
                }
            }

            string diffNote = diffNames.Count == 0 ? "無" : "修改欄位(" + string.Join(", ", diffNames) + ")";
            ToSend("Update", f, diffNote);
        }

        protected override void DeleteDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            base.DeleteDBObject(dbEntity, objs);

            //寄發承辦
            string diffNote = "人員刪除";
            ToSend("Delete", f, diffNote);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.sortable = true;
                field.align = "left";
            }

            //IsOrgStaff 幕僚人員
            var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();
            bool isOrgStaff = IsOrgStaffByName(user.Name);
            if (isOrgStaff)
            {
                opts.addable = false;
            }

            opts.GetFiled("StrConfirmDate").visible = true;

            return opts;
        }

        public ActionResult UpdateConfirm(List<int> Ids)
        {
            try
            {
                if (Ids == null)
                    return Json(new { result = false, errorMessage = "Ids：不可為Null" });

                //確認日期更新
                var f = GetModelEntity();
                var iquery = f.GetAll().Where(a => Ids.Any(b => b == a.Id));
                
                ////iquery = iquery.Take(47); /////////////////
                ////int n = iquery.Count(); /////////////////
                ////var test = iquery.ToList();

                if (iquery.Count() > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var i in iquery)
                    {
                        i.ConfirmDate = now;
                    }

                    f.Update(iquery);

                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false, errorMessage = "查無對應Id：" + string.Join(",", Ids) });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMessage = ex.Message });
            }
        }

        private IEnumerable<ConUnitPerson> GetOutputData(IEnumerable<ConUnitPerson> iquery, params KeyValueParams[] paras)
        {
            //預設條件
            iquery = iquery.Where(a => ConUnitCodeItems.ConUnitCodes.Any(b => b.Code == a.ConUnit));

            //var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();
            //bool IsOrgCity = user.Unit != null && user.Unit != "23";     //環境部(23)
            //if (IsOrgCity)
            //{
            //    iquery = iquery.Where(a => a.ConType == 1);
            //}

            //---1.查詢---
            var CusOrg1 = KeyValue.GetFilterParaValue(paras, "CusOrg1");
            var ConUnit = KeyValue.GetFilterParaValue(paras, "ConUnit");
            var ConType = KeyValue.GetFilterParaValue(paras, "ConType");
            var Name = KeyValue.GetFilterParaValue(paras, "Name");

            if (!string.IsNullOrEmpty(CusOrg1))
            {
                int num = int.Parse(CusOrg1);
                var list = ConUnitCode.GetAllDatas().Where(a => a.CusOrg1 == num);               
                iquery = iquery.Where(a => list.Any(b => b.Code == a.ConUnit));
            }

            if (!string.IsNullOrEmpty(ConUnit))
            {
                iquery = iquery.Where(a => a.ConUnit == ConUnit);
            }

            if (!string.IsNullOrEmpty(ConType))
            {
                int num = int.Parse(ConType);
                iquery = iquery.Where(a => a.ConType == num);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                iquery = iquery.Where(a => a.Name == Name);
            }

            return iquery;
        }

        private bool IsOrgStaffByName(string name) {

            bool result = false;

            //幕僚人員，若同時聯繫窗口與幕僚人員(此非幕僚人員)
            var models = GetModelEntity().GetAll();
            var names = models.Where(a => a.ConType == 1).Select(a => a.Name);

            result = models.Where(a => !names.Any(b =>b == a.Name))
                        .Any(a => a.ConType == 2 && a.Name == name);

            return result;
        }

        public ActionResult ExportPDF(params KeyValueParams[] paras)
        {
            var IModel = new ModelEntity<ConUnitPerson>(new DouModelContextExt());
            var iquery = base.GetDataDBObject(IModel, paras);
            iquery = GetOutputData(iquery, paras);

            //預設排序(備註：應變單位(縣市)，因特殊處理造成ConUnit變中文，目前使用可接受)                
            iquery = iquery.OrderBy(a => a.EditSort)
                        .ThenBy(a => a.ConUnitSort).ThenBy(a => a.PSort);

            var datas = iquery.ToList();

            //查無符合資料表數
            if (datas.Count == 0)
            {
                return Json(new { result = false, errorMessage = "查無符合資料表數" }, JsonRequestBehavior.AllowGet);
            }

            //產出PDF
            string url = "";
            try
            {
                //1.匯出Excel
                string fileTitle = "窗口_幕僚";
                string folder = DEDS.FileHelper.GetFileFolder(Code.TempUploadFile.窗口_幕僚);

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;

                    var ConUnits = ConUnitCode.GetAllDatas().Where(a => a.Code == data.ConUnit);
                    f.應變單位 = ConUnits.Count() == 0 ? data.ConUnit : ConUnits.First().Name;
                    var ConTypes = Code.GetConType().Where(a => a.Key == data.ConType.ToString());
                    f.身分 = ConTypes.Count() == 0 ? data.ConType : ConTypes.First().Value;
                    f.姓名 = data.Name;
                    f.職稱 = data.Position;
                    f.總機分機 = data.Tel;
                    f.行動電話 = data.Mobile;
                    f.住家電話 = data.HTel;
                    f.EMail = data.EMail;
                    f.確認日期 = data.ConfirmDate == null ? "" : DEDS.DateFormat.ToDate7((DateTime)data.ConfirmDate);
                    f.備註 = data.Remark;

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //特殊儲存格位置Top (會外評選=政府採購網公開評選)
                List<string> topContents = new List<string>() {};

                //產出excel
                List<string> titles = new List<string>();
                //"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)
                int autoSizeColumn = 3;

                Dictionary<string, int> dicWidth = new Dictionary<string, int>()
                {
                    { "應變單位", 2600 },//2229
                    { "總機分機", 2600 },//2485
                    { "行動電話", 2600 },//2229
                    { "住家電話", 2600 },//2229
                    { "EMail", 2700 },//2485
                    { "確認日期", 2800 },//2485
                };

                string fileName = DEDS.ExcelSpecHelper.GenerateExcelByLinqF2(fileTitle, titles, list, folder, autoSizeColumn, true, topContents, dicWidth);
                string path = folder + fileName;                //.xlsx
                //End Step 1

                //2.轉換PDF
                string toPdfName = System.IO.Path.GetFileNameWithoutExtension(fileName) + ".pdf";
                string toPdfPath = folder + toPdfName;           //.pdf
                //移除(範本)文字
                toPdfPath = toPdfPath.Replace("(範本)", "");

                // 轉換成pdf
                var app = new Microsoft.Office.Interop.Excel.Application();
                //開啟 Excel 檔案
                var xlsxDocument = app.Workbooks.Open(path);
                // 轉換為 PDF
                try
                {                    
                    xlsxDocument.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, toPdfPath);                    
                }
                catch (Exception ex)
                {                    
                    Logger.Log.For(this).Error("PDF轉換失敗:" + ex.Message);
                    Logger.Log.For(this).Error(ex.StackTrace);
                    return Json(new { result = false, errorMessage = "PDF轉換失敗："}, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    xlsxDocument.Close();
                    app.Quit();
                }
                path = toPdfPath;
                //End Step 2

                //3.新增浮水印
                string text = Dou.Context.CurrentUser<User>().Id + " 僅限公務使用";
                string waterPath = folder + "/Contact_Watermark.pdf";

                FileStream outStream = new FileStream(waterPath, FileMode.Create);

                PdfReader pdfReader = new PdfReader(path);

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

                Stream streamResult = new FileStream(waterPath, FileMode.Open);

                Stream result = new MemoryStream();

                streamResult.CopyTo(result);

                streamResult.Close();

                result.Position = 0;

                path = waterPath;
                //End Step 3

                url = DEDS.Cm.PhysicalToUrl(path);
            }
            catch (Exception ex)
            {
                Logger.Log.For(this).Error("匯出專家清單失敗:" + ex.Message);
                Logger.Log.For(this).Error(ex.StackTrace);
                
                return Json(new { result = false, errorMessage = "匯出專家清單失敗：" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

            if (url == "")
            {
                return Json(new { result = false, errorMessage = "匯出專家清單失敗：無產出連結" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, url = url }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportExcel(params KeyValueParams[] paras)
        {
            var IModel = new ModelEntity<ConUnitPerson>(new DouModelContextExt());
            var iquery = base.GetDataDBObject(IModel, paras);
            iquery = GetOutputData(iquery, paras);

            //預設排序(備註：應變單位(縣市)，因特殊處理造成ConUnit變中文，目前使用可接受)                
            iquery = iquery.OrderBy(a => a.EditSort)
                        .ThenBy(a => a.ConUnitSort).ThenBy(a => a.PSort);

            var datas = iquery.ToList();

            //查無符合資料表數
            if (datas.Count == 0)
            {
                return Json(new { result = false, errorMessage = "查無符合資料表數" }, JsonRequestBehavior.AllowGet);
            }

            //產出PDF
            string url = "";
            try
            {
                //1.匯出Excel
                string fileTitle = "窗口_幕僚";
                string folder = DEDS.FileHelper.GetFileFolder(Code.TempUploadFile.窗口_幕僚);

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;

                    var ConUnits = ConUnitCode.GetAllDatas().Where(a => a.Code == data.ConUnit);
                    f.應變單位 = ConUnits.Count() == 0 ? data.ConUnit : ConUnits.First().Name;
                    var ConTypes = Code.GetConType().Where(a => a.Key == data.ConType.ToString());
                    f.身分 = ConTypes.Count() == 0 ? data.ConType : ConTypes.First().Value;
                    f.姓名 = data.Name;
                    f.職稱 = data.Position;
                    f.總機分機 = data.Tel;
                    f.行動電話 = data.Mobile;
                    f.住家電話 = data.HTel;
                    f.EMail = data.EMail;
                    f.確認日期 = data.ConfirmDate == null ? "" : DEDS.DateFormat.ToDate7((DateTime)data.ConfirmDate);
                    f.備註 = data.Remark;

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //特殊儲存格位置Top (會外評選=政府採購網公開評選)
                List<string> topContents = new List<string>() { };

                //產出excel
                List<string> titles = new List<string>();
                //"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)
                int autoSizeColumn = 3;

                Dictionary<string, int> dicWidth = new Dictionary<string, int>()
                {
                    { "應變單位", 2600 },//2229
                    { "總機分機", 2600 },//2485
                    { "行動電話", 2600 },//2229
                    { "住家電話", 2600 },//2229
                    { "EMail", 2700 },//2485
                    { "確認日期", 2800 },//2485
                };

                string fileName = DEDS.ExcelSpecHelper.GenerateExcelByLinqF2(fileTitle, titles, list, folder, autoSizeColumn, true, topContents, dicWidth);
                string path = folder + fileName;                //.xlsx
                //End Step 1

                url = DEDS.Cm.PhysicalToUrl(path);
            }
            catch (Exception ex)
            {
                Logger.Log.For(this).Error("匯出專家清單失敗:" + ex.Message);
                Logger.Log.For(this).Error(ex.StackTrace);

                return Json(new { result = false, errorMessage = "匯出專家清單失敗：" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

            if (url == "")
            {
                return Json(new { result = false, errorMessage = "匯出專家清單失敗：無產出連結" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, url = url }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool ToSend(string act, ConUnitPerson f, string diffNote)
        {
            bool result = false;

            try
            {
                string actName = "";

                var con = ConUnitCode.GetAllDatas().Where(a => a.Code == f.ConUnit).FirstOrDefault();

                string content = string.Format(@"
您好，DEDS【幕僚/窗口】人員異動通知
<br/><br/>

異動幕僚/窗口人員：{0}({1})<br/>
異動項目：{2}<br/>
異動時間：{3}
<br/><br/>
處理人員帳號：{4}({5})<br/>
災情預警應變決策支援系統(DEDS)連結如下<br/>
<a href='https://newemis.moenv.gov.tw/DEDS/User/Login'>https://newemis.moenv.gov.tw/DEDS/User/Login</a>
", 
con == null ? "" : con.Name,
f.Name,
diffNote,
DateFormat.ToDate7(DateTime.Now),
Dou.Context.CurrentUser<User>().Name,
Dou.Context.CurrentUser<User>().Id
);

                if (act == "Add")
                {
                    actName = "新增";
                }
                else if (act == "Update")
                {
                    actName = "修改";
                }
                else if (act == "Delete")
                {
                    actName = "刪除";
                }
                else
                {
                    Logger.Log.For(null).Error("尚未設定行為(act)：" + act);
                }

                EmailHelper emailHelper = new EmailHelper();
                MailParam p = new MailParam();
                p.iniParam();
                emailHelper.MailFrom = p.MailFrom;
                emailHelper.MailFromName = p.MailFromName;
                emailHelper.Account = p.Account;
                emailHelper.Password = p.Password;
                emailHelper.MailServer = p.MailServer;
                emailHelper.MailPort = p.MailPort;
                emailHelper.EnableSSL = p.EnableSSL;

                emailHelper.Subject = "幕僚/窗口異動：" + actName;
                emailHelper.Body = content;
                foreach (string addr in AppConfig.EmailAddressResp.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddTo(addr, "");
                    }
                }

                foreach (string addr in AppConfig.EmailAddressCC.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddCC(addr, "");
                    }
                }

                emailHelper.IsSendEmail = true;
                bool success = emailHelper.SendBySmtp();

                if (!success)
                {
                    Logger.Log.For(null).Error("ToSend - 信件寄發失敗:" + emailHelper.ToMails);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("信件寄發錯誤：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);

                return false;
            }

            return result;
        }
    }
}