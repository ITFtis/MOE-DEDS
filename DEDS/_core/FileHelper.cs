using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DEDS
{
    public class FileHelper
    {
        public static string GetTempleteFolder(string id1 = "")
        {
            string result = HttpContext.Current.Server.MapPath("~\\") + "DocsWeb\\";

            return result;
        }

        public static string GetFileFolder(Code.TempUploadFile en, string id1 = "")
        {
            string result = "";

            if (id1 != "")
            {
                result = AppConfig.RootPath + "Temp\\" + en.ToString() + "\\" + id1 + "\\";
            }
            else
            {
                result = AppConfig.RootPath + "Temp\\" + en.ToString() + "\\";
            }

            return result;
        }

        public static string GetFileFolder(Code.UploadFile en, string id1 = "")
        {
            string result = "";

            if (id1 != "")
            {
                result = AppConfig.RootPath + en.ToString() + "\\" + id1 + "\\";
            }
            else
            {
                result = AppConfig.RootPath + en.ToString() + "\\";
            }

            return result;
        }

        /// <summary>
        /// (F1檔案處理)移動檔案 瀏覽檔案送出,從Temp移動至對應路徑(2層,'_')
        /// </summary>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="from">來源Temp</param>
        /// <param name="to">目的</param>
        public static void MoveFileF1(string fileName, Code.TempUploadFile from, Code.UploadFile to)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string fromPath = FileHelper.GetFileFolder(from) + fileName;
            if (System.IO.File.Exists(fromPath))
            {
                string[] strs = fileName.Split('_');

                string toFolder = "";
                string toName = "";
                if (strs.Length == 2)
                {
                    //2層
                    string key1 = fileName.Split('_')[0];  //資料夾(ex.guid, id)
                    toFolder = FileHelper.GetFileFolder(to) + key1;
                    toName = fileName.Split('_')[1];  //檔名
                }
                else
                {
                    //1層
                    toFolder = FileHelper.GetFileFolder(to);
                    toName = fileName;
                }

                if (!Directory.Exists(toFolder))
                {
                    Directory.CreateDirectory(toFolder);
                }

                string toPath = toFolder + "/" + toName;
                System.IO.File.Move(fromPath, toPath);
            }
        }

        /// <summary>
        /// (F1檔案處理)刪除檔案
        /// </summary>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="from">來源Temp</param>        
        public static void DeleteFileF1(string fileName, Code.UploadFile type)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string[] strs = fileName.Split('_');

            string folder = "";
            string name = "";
            if (strs.Length == 2)
            {
                //2層
                string key1 = fileName.Split('_')[0];  //資料夾(ex.guid, id)
                folder = FileHelper.GetFileFolder(type) + key1;
                name = fileName.Split('_')[1];  //檔名
            }
            else
            {
                //1層
                folder = FileHelper.GetFileFolder(type);
                name = fileName;
            }

            string path = folder + "/" + name;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                //folder沒檔案，刪除
                string[] files = Directory.GetFiles(folder);
                if (files.Count() == 0)
                    Directory.Delete(folder, true);
            }
        }
    }
}