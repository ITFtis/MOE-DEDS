using DEDS.FHYResultOfFHYFloodSensorStation;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using DouHelper;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DEDS.Controllers.Prj
{
    [MenuDef(Id = "RtHydro", Name = "決策支援圖台", MenuPath = "災情決策", Index = 1, Action = "Index", AllowAnonymous = false, Func = FuncEnum.None)]
    public class RtHydroController : Dou.Controllers.AGenericModelController<Object>
    {
        // GET: RtHydro
        public ActionResult Index()
        {
            ViewBag.HasGis = true;
            ViewBag.Leaflet = true;
            ViewBag.ToolNearCctv = true;
            return View();
        }

        protected override IModelEntity<object> GetModelEntity()
        {
            throw new NotImplementedException();
        }

        //取得單筆user (第1種回傳格式)
        public ActionResult MidProxy(string urlAPI)
        {
            //string api = "https://www.dprcflood.org.tw/SGDS/WS/FloodComputeWS.asmx/Countys";

            if (urlAPI == null)
                return null;

            string text = "";
            WebClient webClient = new WebClient();
            //其他清運機具資料介接規格書 websrv_ar4.asmx
            try
            {
                // 指定 WebClient 的編碼
                webClient.Encoding = Encoding.UTF8;
                text = webClient.DownloadString(urlAPI);
            }
            catch (WebException we)
            {
                using (StreamReader sr =
                    new StreamReader(we.Response.GetResponseStream()))
                {
                    //實務上可將錯誤資訊網頁寫入Log檔備查，
                    //此處只將錯誤訊息完整傳回當示範
                    string error = sr.ReadToEnd();

                    Logger.Log.For(this).Error(string.Format("api 介接錯誤：{0}{1}", urlAPI, error));
                }
            }

            //xml轉換，不可預期字串
            text = text.Replace("xmlns=\"http://tempuri.org/\"", "");
            text = text.Replace("xmlns=\"https://tempuri.org/\"", "");                      
            XDocument doc = XDocument.Parse(text);

            string result = "";

            try
            {
                if (urlAPI.IndexOf("WS/FloodComputeWS.asmx/Countys") != -1)
                {                    
                    var d = XmlHelper.Deserialize<ArrayOfCountyXY.ArrayOfCountyXY>(doc);
                    var jstr = JsonConvert.SerializeObject(d, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    result = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
                }                
                else if (urlAPI.IndexOf("WS/FloodComputeWS.asmx/FloodEvents") != -1)
                {
                    var d = XmlHelper.Deserialize<ArrayOfFloodEvent.ArrayOfFloodEvent>(doc);
                    var jstr = JsonConvert.SerializeObject(d, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    result = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
                }                              
                else
                {
                    Logger.Log.For(this).Error(string.Format("api 轉換Model物件(無Api對應Model)：{0}", urlAPI));
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.For(this).Error(string.Format("api 轉換Model物件錯誤：{0}{1}", urlAPI, ex.Message));
                Logger.Log.For(this).Error("內容(text)：" + text);
                Logger.Log.For(this).Error(ex.StackTrace);
                return null;
            }
            
            return Content(result, "application/json");
        }

        //取得單筆user (第2種回傳格式)
        public ActionResult MidProxyF2(string urlAPI)
        {
            //string api = "https://www.dprcflood.org.tw/SGDS/WS/FloodComputeWS.asmx/Countys";

            if (urlAPI == null)
                return null;

            string text = "";
            WebClient webClient = new WebClient();
            //其他清運機具資料介接規格書 websrv_ar4.asmx
            try
            {
                // 指定 WebClient 的編碼
                webClient.Encoding = Encoding.UTF8;
                text = webClient.DownloadString(urlAPI);
            }
            catch (WebException we)
            {
                using (StreamReader sr =
                    new StreamReader(we.Response.GetResponseStream()))
                {
                    //實務上可將錯誤資訊網頁寫入Log檔備查，
                    //此處只將錯誤訊息完整傳回當示範
                    string error = sr.ReadToEnd();

                    Logger.Log.For(this).Error(string.Format("api 介接錯誤：{0}{1}", urlAPI, error));
                }
            }

            //xml轉換，不可預期字串
            text = text.Replace("xmlns=\"http://tempuri.org/\"", "");
            text = text.Replace("xmlns=\"https://tempuri.org/\"", "");
                       
            XDocument doc = XDocument.Parse(text);

            string result = "";

            try
            {
                if (urlAPI.IndexOf("WS/FHYBrokerWS.asmx/GetFHYCity") != -1)
                {
                    var d = XmlHelper.Deserialize<FHYResultOfFHYCityData.FHYResultOfFHYCityData>(doc);
                    //var jstr = JsonConvert.SerializeObject(d, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    //result = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");

                    return Json(new { d = d }, JsonRequestBehavior.AllowGet);
                }
                else if (urlAPI.IndexOf("WS/FHYBrokerWS.asmx/GetFHYFloodSensorInfoRt") != -1)
                {
                    var d = XmlHelper.Deserialize<FHYResultOfFHYFloodSensorInfo.FHYResultOfFHYFloodSensorInfo>(doc);
                    //var jstr = JsonConvert.SerializeObject(d, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    //result = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");

                    var output = new FHYResultOfFHYFloodSensorInfo.Output();
                    output.UpdataTime = d.UpdataTime;
                    output.Data = d.Data.FHYFloodSensorInfo;
                    return Json(new { d = output }, JsonRequestBehavior.AllowGet);
                }
                else if (urlAPI.IndexOf("WS/FHYBrokerWS.asmx/GetFHYFloodSensorStation") != -1)
                {
                    var d = XmlHelper.Deserialize<FHYResultOfFHYFloodSensorStation.FHYResultOfFHYFloodSensorStation>(doc);
                    //var jstr = JsonConvert.SerializeObject(d, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    //result = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");

                    var output = new FHYResultOfFHYFloodSensorStation.Output();
                    output.UpdataTime = d.UpdataTime;
                    output.Data = d.Data.FHYFloodSensorStation;
                    return Json(new { d = output }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Logger.Log.For(this).Error(string.Format("api 轉換Model物件(無Api對應Model)：{0}", urlAPI));
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.For(this).Error(string.Format("api 轉換Model物件錯誤：{0}{1}", urlAPI, ex.Message));
                Logger.Log.For(this).Error("內容(text)：" + text);
                Logger.Log.For(this).Error(ex.StackTrace);
                return null;
            }
            
            //return Json(new { d = result }, JsonRequestBehavior.AllowGet);
        }        
    }
}