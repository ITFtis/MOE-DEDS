using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEDS
{
    public class Constant
    {
        public static int cacheTime = 60 * 60 * 1000;                        //60分
        public static int cacheReportTime = 24 * 60 * 60 * 1000;            //24小時
        public static int cacheBigReportTime = 7 * 24 * 60 * 60 * 1000;     //7天
    }
}