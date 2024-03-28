using DEDS.Models.Comm;
using DEDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEDS
{
    public class Code
    {
        /// <summary>
        /// 身分類別(緊急聯繫窗口)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetConType()
        {
            IEnumerable<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();
            result = result.Append(new KeyValuePair<string, object>("1", "聯繫窗口"));
            result = result.Append(new KeyValuePair<string, object>("2", "幕僚人員"));            

            return result;
        }
    }

    #region Code下拉集合

    /// <summary>
    /// 身分類別(緊急聯繫窗口)
    /// </summary>
    public class CodeByGetConTypeItems : Dou.Misc.Attr.SelectItemsClass
    {        
        public const string AssemblyQualifiedName = "DEDS.CodeByGetConTypeItems, DEDS";
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Code.GetConType().Select(s => new KeyValuePair<string, object>(s.Key, s.Value));
        }
    }

    #endregion
}