﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;

namespace DEDS
{
    public class CommonFunc
    {
        /// <summary>
        /// 測試目的端是否正常連線
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool TestUrl(string url)
        {
            bool result = false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                HttpStatusCode statusCode = response.StatusCode;

                result = statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error(string.Format("url 無法正常連線：{0}", url));
                Logger.Log.For(null).Error("錯誤：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);

                return false;
            }

            return result;
        }

        /// <summary>
        /// 測試目的端是否正常連線
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool TestMailServerSmtpIp(string ip, int port)
        {
            bool result = false;

            System.Net.Sockets.TcpClient client = new TcpClient();
            try
            {
                client.Connect(ip, port);
                result = true;
            }
            catch (SocketException ex)
            {
                Logger.Log.For(null).Error(string.Format("SMTP 無法正常連線：{0}, {1}", ip, port.ToString()));
                Logger.Log.For(null).Error("錯誤：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);
            }

            return result;
        }
    }
}