using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class Help
    {
        //主页echart数据
        public string GetY(Guid StaffId)
        {
            using (Models.OSMS db =new Models.OSMS())
            {
                string result = null;
                DateTime Date =DateTime.Now.Date; //当前时间
                for (int i = 7; i > 0; i--)
                {
                    DateTime beginTime = Date.AddDays(-i);
                    DateTime endTime = Date.AddDays(-i+1);

                    List<Models.LoginInfo> data = db.LoginInfo.Where(x => x.StaffId == StaffId & (x.LoginTime >= beginTime) & (x.LoginTime < endTime)).ToList();
                    if (data.Count > 0)
                    {
                        if (i == 1)
                        {
                            result += data.Count;
                        }
                        else
                        {
                            result += data.Count + ", ";
                        }

                    }
                    else
                    {
                        if (i == 1)
                        {
                            result += 0;
                        }
                        else
                        {
                            result += 0 + ", ";
                        }
                    }
                }
                return result;
            }
        }
        public string GetX()
        {
            string result = null;
            DateTime Date = DateTime.Now; //当前时间
            for (int i = 7; i > 0; i--)
            {
                // ViewBag.BirthDay = BirthDay.ToString();
                string day = Date.AddDays(-i).ToString("MM月dd日", DateTimeFormatInfo.InvariantInfo);
                if (i == 1)
                {
                    result += day;
                }
                else
                {
                    result += day + ",";
                }
            }
            return result;
        }


        /// <summary>
        /// 分页数据处理，支持模糊查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="list">模糊查询后获取的数据</param>
        /// <returns>返回一个分页数据泛型</returns>
        public static PageItem<T> Page<T>(int index, int pageSize, IQueryable<T> list)
        {
            PageItem<T> querylist = new PageItem<T>();
            querylist.data = list.Skip((index - 1) * pageSize).Take(pageSize).ToList();
            querylist.count = list.Count();
            return querylist;
        }

        /// <summary>
        /// 作用：将字符串内容转化为16进制数据编码，其逆过程是Decode
        /// 参数说明：
        /// strEncode 需要转化的原始字符串
        /// 转换的过程是直接把字符转换成Unicode字符,比如数字"3"-->0033,汉字"我"-->U+6211
        /// 函数decode的过程是encode的逆过程.
        /// </summary>
        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            try
            {
                foreach (short shortx in strEncode.ToCharArray())
                {
                    strReturn += shortx.ToString("X4");
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// 作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        public static string Decode(string strDecode)
        {
            string sResult = "";
            try
            {
                for (int i = 0; i < strDecode.Length / 4; i++)
                {
                    sResult += (char)short.Parse(strDecode.Substring(i * 4, 4),
                        global::System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch { }
            return sResult;
        }
    }
}