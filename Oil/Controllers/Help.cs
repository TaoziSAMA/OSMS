using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Oil.Controllers
{
    public class Help
    {
        public string GetY(Guid StaffId)
        {
            using (Models.OSMS db =new Models.OSMS())
            {
                string result = null;
                DateTime Date = DateTime.Now; //当前时间
                for (int i = 7; i > 0; i--)
                {
                    DateTime beginTime = Date.AddDays(-i - 1);
                    DateTime endTime = Date.AddDays(-i);

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
    }
}