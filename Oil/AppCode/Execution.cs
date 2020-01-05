using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oil.Models;

namespace Oil.AppCode
{
    public class Execution
    {
        OSMS db = new OSMS();
        //入职
        public void InputStaffInfo(ProcessStepRecord info)
        {
            ProcessStepRecord datapro = db.ProcessStepRecord.Where(x => x.Id == info.Id).FirstOrDefault();
            Entry dataent = db.Entry.Where(x => x.Id == datapro.RefOrderId).FirstOrDefault();
            Staff add = new Staff();
            add.No = dataent.WorkNumber;
            add.Name = dataent.StaffName;
            add.Sex = dataent.Sex;
            add.BirthDay = dataent.BirthDay;
            add.NativePlace = dataent.NativePlace;
            add.Address = dataent.Address;
            add.Email = dataent.Email;
            add.Tel = dataent.Tel;
            add.Status = "1";
            add.JobId = dataent.JobId;
            add.OrgID = dataent.Organization_Id;
            add.Id = Guid.NewGuid();
            add.Password = Controllers.Help.Encode("1");//默认密码
            add.CreateTime = DateTime.Now;
            add.IsHSEGroup = false;
            db.Staff.Add(add);
            db.SaveChanges();
        }

        //分配工号
        public void FillEntryInfo(ProcessStepRecord info)
        {
            ProcessStepRecord datapro = db.ProcessStepRecord.Where(x => x.Id == info.Id).FirstOrDefault();//流程记录信息
            Entry dataent = db.Entry.Where(x => x.Id == datapro.RefOrderId).FirstOrDefault();//找到入职申请记录信息
            Staff stadata = db.Staff.Where(x => true).OrderByDescending(x => x.No).FirstOrDefault();//找到最新工号
            dataent.WorkNumber = Convert.ToInt32(stadata.No) + 1 + "";//创建新工号

            //更改记录中的工号
            Entry data = db.Entry.FirstOrDefault(x => x.Id == dataent.Id);
            data.StaffName = dataent.StaffName;  //员工姓名
            data.Sex = dataent.Sex;  //性别
            data.BirthDay = dataent.BirthDay;  //出生日期
            data.MaritalStatus = dataent.MaritalStatus;  //婚姻状况
            data.Height = dataent.Height;  //身高
            data.HighestEducation = dataent.HighestEducation;  //最高学历
            data.Major = dataent.Major;  //专业
            data.ForeginLanguageAptitude = dataent.ForeginLanguageAptitude;  //外语能力
            data.IDNumber = dataent.IDNumber;  //生份证号
            data.NativePlace = dataent.NativePlace;  //籍贯
            data.Address = dataent.Address;  //详细地址
            data.Email = dataent.Email;  //邮件
            data.Tel = dataent.Tel;  //联系电话
            data.HaseMedicalHistory = dataent.HaseMedicalHistory;  //是否有传染病史
            data.MedicalHistoryComment = dataent.MedicalHistoryComment;  //传染病史备注
            data.HobbiesAndSpeciality = dataent.HobbiesAndSpeciality;  //兴趣爱好及特长
            data.EducationalExperience1StartDate = dataent.EducationalExperience1StartDate;  //教育?经历（高中/中专）开始日期
            data.EducationalExperience1EndDate = dataent.EducationalExperience1EndDate;  //教育?经历（高中/中专）结束日期
            data.EducationalExperience1SchoolName = dataent.EducationalExperience1SchoolName;  //教育?经历（高中/中专）学校名
            data.EducationalExperience1Major = dataent.EducationalExperience1Major;  //教育?经历（高中/中专）学历
            data.EducationalExperience1Certificate = dataent.EducationalExperience1Certificate;  //教育?经历（高中/中专）证书
            data.EducationalExperience2StartDate = dataent.EducationalExperience2StartDate;  //教育?经历（大专/本科）开始日期
            data.EducationalExperience2EndDate = dataent.EducationalExperience2EndDate;  //教育?经历（大专/本科）结束日期
            data.EducationalExperience2SchoolName = dataent.EducationalExperience2SchoolName;  //教育?经历（大专/本科）学校名
            data.EducationalExperience2Major = dataent.EducationalExperience2Major;  //教育?经历（大专/本科）专业
            data.EducationalExperience2Certificate = dataent.EducationalExperience2Certificate;  //教育?经历（大专/本科）证书
            data.EducationalExperience3StartDate = dataent.EducationalExperience3StartDate;  //教育?经历（研究生）开始日期
            data.EducationalExperience3EndDate = dataent.EducationalExperience3EndDate;  //教育?经历（研究生）开始结束日期
            data.EducationalExperience3SchoolName = dataent.EducationalExperience3SchoolName;  //教育?经历（研究生）学校名
            data.EducationalExperience3Major = dataent.EducationalExperience3Major;  //教育?经历（研究生）专业
            data.EducationalExperience3Certificate = dataent.EducationalExperience3Certificate;  //教育?经历（研究生）证书
            data.EducationalExperience4StartDate = dataent.EducationalExperience4StartDate;  //教育?经历（其它）开始日期
            data.EducationalExperience4EndDate = dataent.EducationalExperience4EndDate;  //教育?经历（其它）结束日期
            data.EducationalExperience4SchoolName = dataent.EducationalExperience4SchoolName;  //教育?经历（其它）结束学校名
            data.EducationalExperience4Major = dataent.EducationalExperience4Major;  //教育?经历（其它）专业
            data.EducationalExperience4Certificate = dataent.EducationalExperience4Certificate;  //教育?经历（其它）证书
            data.FamilyStatus1Name = dataent.FamilyStatus1Name;  //家庭成员1 名称
            data.FamilyStatus1Appellation = dataent.FamilyStatus1Appellation;  //家庭成员1 称谓
            data.FamilyStatus1Company = dataent.FamilyStatus1Company;  //家庭成员1 工作单位
            data.FamilyStatus1Tel = dataent.FamilyStatus1Tel;  //家庭成员1 联系电话
            data.FamilyStatus2Name = dataent.FamilyStatus2Name;  //家庭成员2 名称
            data.FamilyStatus2Appellation = dataent.FamilyStatus2Appellation;  //家庭成员2 称谓
            data.FamilyStatus2Company = dataent.FamilyStatus2Company;  //家庭成员2 工作单位
            data.FamilyStatus2Tel = dataent.FamilyStatus2Tel;  //家庭成员2 电话
            data.EmergencyContactName = dataent.EmergencyContactName;  //紧急联系人名字
            data.EmergencyContactTel = dataent.EmergencyContactTel;  //紧急联系人电话
            data.EmergencyContactEelationShipWithMe = dataent.EmergencyContactEelationShipWithMe;  //紧急联系人关系
            data.JobId = dataent.JobId;  //职位Id 与职位表对应
            data.Title = dataent.Title;  //职称
            data.Organization_Id = dataent.Organization_Id;  //机构部门 与组织机构表对应
            data.SupervisorComments = dataent.SupervisorComments;  //主管评语
            data.ProbationarySalary = dataent.ProbationarySalary;  //试用期薪水
            data.CorrectSalary = dataent.CorrectSalary;  //转正薪水
            data.EntryDate = dataent.EntryDate;  //入职日期
            data.BirthCertificatePhoto = dataent.BirthCertificatePhoto;  //身份证拍照
            data.RegistrationPhoto = dataent.RegistrationPhoto;  //登记照
            data.BankCardNumber = dataent.BankCardNumber;  //银行卡号
            data.WorkNumber = dataent.WorkNumber;  //创建员工工号
            data.CreateStaffeId = dataent.CreateStaffeId;  //创建员工Id
            data.UpdateTime = DateTime.Now; //创建时间
            db.SaveChanges();
        }

        //离职
        public void UpdateStaffStatus(ProcessStepRecord info)
        {
            ProcessStepRecord datapro = db.ProcessStepRecord.Where(x => x.Id == info.Id).FirstOrDefault();
            LeaveOffice del = db.LeaveOffice.Where(x => x.Id == info.RefOrderId).FirstOrDefault();
            Staff stadata = new Staff();
            stadata.Id = del.ApplyPersonId;

            Staff data = db.Staff.FirstOrDefault(x => x.Id == info.Id);
            data.Status = "0";
            data.UpdateTime = DateTime.Now; //创建时间
            db.SaveChanges();//保存
        }
    }
}