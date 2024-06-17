using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DEDS
{
    public static class AppConfig
    {
        #region 私有變數

        private static string _rootPath;
        private static string _ePAAPI;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _rootPath = ConfigurationManager.AppSettings["RootPath"].ToString();
            //實體路徑(解決開發者專案於不同目錄)
            _rootPath = _rootPath.Replace("~\\", HttpContext.Current.Server.MapPath("~\\"));

            _ePAAPI = ConfigurationManager.AppSettings["EPAAPI"] == null ? "": ConfigurationManager.AppSettings["EPAAPI"].ToString();
        }

        #endregion

        #region 公用屬性      

        /// <summary>
        /// 檔案存放跟目錄
        /// </summary>
        public static string RootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        /// API連結(舊站)
        /// </summary>
        public static string EPAAPI
        {
            get { return _ePAAPI; }
        }

        #endregion
    }
}