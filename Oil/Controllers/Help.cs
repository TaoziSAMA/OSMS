using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Oil.AppCode;
using Oil.Models;

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

        /// <summary>
        /// 获取最新审批状态
        /// </summary>
        /// <param name="refOrderId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetDiscrible(Guid refOrderId,Models.OSMS db)
        {
            string Discribe = "";
            List<Models.ProcessStepRecord> currentApply = db.ProcessStepRecord.Where(x => x.RefOrderId == refOrderId).ToList();
            Models.ProcessStepRecord newApply = currentApply.OrderByDescending(x => x.StepOrder).FirstOrDefault(x => x.WhetherToExecute == true);
            if(newApply!=null)
                Discribe = newApply.Discrible;
            return Discribe;
        }

        /// <summary>
        /// 获取单号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="fix">类型识别</param>
        /// <param name="table">查询的表</param>
        /// <returns></returns>
        public static string GetOrderNumber<T>(Models.OSMS db, string fix, string table)
        {
            string pirfix = fix + DateTime.Now.ToString("yyyyMMdd");
            //if (context.OilMaterialOrder.Any(x => x.No.StartsWith(pirfix)))
            //{
            //    string maxNumber = context.OilMaterialOrder.Where(x => x.No.StartsWith(pirfix)).Max(x => x.No);
            //    string endStr = maxNumber.Substring(pirfix.Length, maxNumber.Length - pirfix.Length); //12  //0000012/
            //    return pirfix + (int.Parse(endStr) + 1).ToString("000000");
            //}
            //else
            //{
            //    return pirfix + "000001";
            //}

            //对比是否存在相同数据
            List<T> count = db.Database.SqlQuery<T>(string.Format("select top 1.* from {0} where No like'{1}%' order by No desc", table, pirfix)).ToList();
            if (count.Any())
            {
                //若存在找出最大的单号
                string maxNumber = db.Database.SqlQuery<string>(string.Format("select top 1.No from {0} where No like'{1}%' order by No desc", table, pirfix)).Max();
                //截取后六位区别码
                string endStr = maxNumber.Substring(pirfix.Length, maxNumber.Length - pirfix.Length);
                //区别码加一
                return pirfix + (int.Parse(endStr) + 1).ToString("000000");
            }
            else
            {
                //若不存在默认为 000001
                return pirfix + "000001";
            }
        }

        /// <summary>
        /// 发起流程视图
        /// </summary>
        /// <param name="proCode">流程类型</param>
        /// <param name="ApplyPersonId">申请用户id</param>
        /// <param name="No">申请单号</param>
        /// <param name="RefOrderId">申请单据id</param>
        /// <param name="db"></param>
        public static void SetProcess(string proCode, Guid? ApplyPersonId, string No, Guid RefOrderId, OSMS db)
        {
            ProcessItem Prodata = db.ProcessItem.FirstOrDefault(x => x.Code == proCode);//获取当前流程类型
            List<Models.Approver> Appdata = db.Approver.Where(x => x.ProcessItemId == Prodata.Id).OrderBy(x => x.Order).ToList();//获取当前流程设定 详细
            Models.Staff Stadata = db.Staff.FirstOrDefault(x => x.Id == ApplyPersonId);//获取当前用户信息
            List<Models.OrganizationStructure> SaveOrg = new List<OrganizationStructure>();//保存应有的机构
            Models.OrganizationStructure OrgOne = db.OrganizationStructure.FirstOrDefault(x => x.Id == Stadata.OrgID);//用户当前机构信息
            SaveOrg.Add(OrgOne);
            int leg = OrgOne.Leve;
            //将用户上级机构加入集合中
            for (int i = 0; i < leg; i++)
            {
                Models.OrganizationStructure add = db.OrganizationStructure.FirstOrDefault(x => x.Id == OrgOne.ParentId);
                SaveOrg.Add(add);
                OrgOne = add;
            }

            int StepOrder = 0;
            DateTime CreateTime = DateTime.Now; //创建时间
            foreach (Approver Appitem in Appdata)
            {
                Job JobCurrent = db.Job.FirstOrDefault(x => x.Code == Appitem.JobCode);//当前审批人职位
                OrganizationStructure OrgCurrent = SaveOrg.FirstOrDefault(x => x.Leve == Appitem.AreaLeve);//当前审批人机构
                Staff StaCurrent = db.Staff.FirstOrDefault(x => x.JobId == JobCurrent.Id && x.OrgID == OrgCurrent.Id);//当前审批人个人信息
                ProcessStepRecord ProCurrent = new ProcessStepRecord();
                ProCurrent.Id = Guid.NewGuid();//设置ID
                ProCurrent.Type = proCode;//流程类型
                ProCurrent.StepOrder = StepOrder;//执行顺序
                ProCurrent.WaitForExecutionStaffId = StaCurrent.Id;//待执行操作人
                ProCurrent.CreateTime = CreateTime;//创建时间
                ProCurrent.UpdateTime = CreateTime;//修改时间
                ProCurrent.WhetherToExecute = false;//是否已执行
                ProCurrent.No = No;//关联编号
                ProCurrent.RefOrderId = RefOrderId;//关联主据单ID
                ProCurrent.Result = false;//默认不通过
                ProCurrent.ExecuteMethod = Appitem.ExecuteMethod;//执行函数
                ProCurrent.Discrible = "[" + OrgCurrent.Name + "]" + " " + JobCurrent.Name + " " + StaCurrent.Name + "" + (Stadata.Id == StaCurrent.Id ? "=>已发起" : "");//描述
                StepOrder++;//增加顺序
                if (StaCurrent.Id == ApplyPersonId)//审批人发起则默认通过
                {
                    ProCurrent.WhetherToExecute = true;//默认执行
                    ProCurrent.Result = true;//默认通过
                }
                db.ProcessStepRecord.Add(ProCurrent);
            }
            db.SaveChanges();//保存修改
        }

        //获取是否需要执行
        public static string GetExec(Guid staffId, Guid refOrderId,OSMS db)
        {
            string NeedExec = "";
            List<ProcessStepRecord> currentApply = db.ProcessStepRecord.Where(x => x.RefOrderId == refOrderId).ToList();//当前订单流程
            ProcessStepRecord currentUser = currentApply.FirstOrDefault(x => x.WaitForExecutionStaffId == staffId);//当前审批人的状态
            if (currentUser.StepOrder == 0)
            {
                if (currentUser.WhetherToExecute == false)
                {
                    NeedExec = "true";
                }
            }
            else
            {
                ProcessStepRecord currentUpUser = currentApply.FirstOrDefault(x => x.StepOrder == currentUser.StepOrder - 1);//上一个审批人
                if (currentUpUser.WhetherToExecute == true && currentUser.WhetherToExecute == false & currentUpUser.Result == true)
                {
                    NeedExec = "true";
                }
            }
            return NeedExec;
        }
    }
}