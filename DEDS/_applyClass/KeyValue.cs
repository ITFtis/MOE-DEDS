using Dou.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DEDS
{
    public class KeyValue
    {
        public string key { get; set; }

        public string value { get; set; }

        public static string GetValue(KeyValue[] paras, string key)
        {
            ////單筆
            //KeyValue para = paras.FirstOrDefault(s => s.key == key);
            //if (para != null && !string.IsNullOrEmpty(para.value?.ToString() ?? ""))
            //{
            //    return para.value?.ToString() ?? "";
            //}

            //多筆
            var keyValueParams2 = paras.Where(s => s.key == key).ToList();
            if (keyValueParams2 != null)
            {
                if (keyValueParams2.Count > 0)
                {
                    //多筆','區隔
                    var strs = keyValueParams2.Select(a => a.value ?? "");
                    return string.Join(",", strs);
                }
            }

            return null;
        }

        /// <summary>
        /// Dou.Controllers.KeyValueParams取value(value可為object)
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFilterParaValue(KeyValueParams[] paras, string key)
        {
            if (paras == null)
                return null;

            KeyValueParams keyValueParams = paras.FirstOrDefault((KeyValueParams s) => s.key == "filter");

            if (keyValueParams != null)
            {
                if (keyValueParams.value.GetType() == typeof(string[]))
                {
                    //調整寫法(vale:obejct)
                    string text = new JavaScriptSerializer().Serialize(keyValueParams.value);
                    if (text == "")
                        return "";

                    List<string> list = JsonConvert.DeserializeObject<List<string>>(text);
                    var source = JsonConvert.DeserializeObject<KeyValue[]>(list[0]);

                    string str = GetValue(source, key);
                    if (str == null)
                        return null;

                    //過濾空值
                    string fstr = string.Join(",", str.Split(',').Where(a => a != ""));

                    return fstr;
                }
                else
                {
                    //dou寫法(value:字串)
                    KeyValueParams[] source = JsonConvert.DeserializeObject<KeyValueParams[]>(keyValueParams.value?.ToString() ?? "");

                    //單筆
                    //KeyValueParams keyValueParams2 = source.FirstOrDefault((KeyValueParams s) => s.key == key);
                    //if (keyValueParams2 != null && !string.IsNullOrEmpty(keyValueParams2.value?.ToString() ?? ""))
                    //{
                    //    return keyValueParams2.value?.ToString() ?? "";
                    //}

                    //多筆
                    var keyValueParams2 = source.Where(s => s.key == key).ToList();
                    if (keyValueParams2.Count > 0)
                    {
                        //多筆','區隔
                        var strs = keyValueParams2.Select(a => a.value ?? "");
                        string str = string.Join(",", strs);

                        //過濾空值
                        string fstr = string.Join(",", str.Split(',').Where(a => a != ""));

                        return fstr;
                    }
                }
            }

            return null;
        }
    }
}