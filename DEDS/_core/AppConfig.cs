using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace DEDS
{
    public static class AppConfig
    {
        #region 私有變數

        private static string _rootPath;
        private static string _ePAAPI;

        private static string _htmlTemplatePath;
        private static string _emailAddressCC;
        private static string _emailAddressResp;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _rootPath = ConfigurationManager.AppSettings["RootPath"].ToString();
            //實體路徑(解決開發者專案於不同目錄)
            _rootPath = _rootPath.Replace("~\\", HttpContext.Current.Server.MapPath("~\\"));

            _ePAAPI = ConfigurationManager.AppSettings["EPAAPI"] == null ? "": ConfigurationManager.AppSettings["EPAAPI"].ToString();
            _htmlTemplatePath = ConfigurationManager.AppSettings["HtmlTemplatePath"] == null ? "" : ConfigurationManager.AppSettings["HtmlTemplatePath"].ToString();

            _emailAddressCC = ConfigurationManager.AppSettings["EmailAddressCC"] == null ? "" : ConfigurationManager.AppSettings["EmailAddressCC"].ToString();
            _emailAddressResp = ConfigurationManager.AppSettings["EmailAddressResp"] == null ? "" : ConfigurationManager.AppSettings["EmailAddressResp"].ToString();
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

        /// <summary>
        ///Html樣式路徑
        /// </summary>
        public static string HtmlTemplatePath
        {
            get 
            {
                _htmlTemplatePath = _htmlTemplatePath;
                return _htmlTemplatePath; 
            }
        }

        /// <summary>
        /// Email副本 
        /// </summary>
        public static string EmailAddressCC
        {
            get { return _emailAddressCC; }
        }

        /// <summary>
        /// Email負責人(淑俐) 
        /// </summary>
        public static string EmailAddressResp
        {
            get { return _emailAddressResp; }
        }

        #endregion
    }
}