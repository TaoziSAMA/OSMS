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
    }
}