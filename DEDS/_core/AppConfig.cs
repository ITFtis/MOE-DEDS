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

        private static string _ePAAPI;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _ePAAPI = ConfigurationManager.AppSettings["EPAAPI"] == null ? "": ConfigurationManager.AppSettings["EPAAPI"].ToString();
        }

        #endregion

        #region 公用屬性      

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