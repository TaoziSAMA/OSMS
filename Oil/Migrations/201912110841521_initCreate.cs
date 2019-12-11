namespace Oil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approver",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        JobCode = c.String(nullable: false, maxLength: 100, unicode: false),
                        AreaLeve = c.Int(nullable: false),
                        Discrible = c.String(maxLength: 100),
                        Order = c.Int(nullable: false),
                        ProcessItemId = c.Guid(),
                        ExecuteMethod = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entry",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StaffName = c.String(nullable: false, maxLength: 500),
                        Sex = c.Boolean(nullable: false),
                        BirthDay = c.DateTime(storeType: "date"),
                        MaritalStatus = c.Boolean(),
                        Height = c.Decimal(precision: 5, scale: 2),
                        HighestEducation = c.Int(),
                        Major = c.String(maxLength: 500),
                        ForeginLanguageAptitude = c.String(maxLength: 500),
                        IDNumber = c.String(maxLength: 20, unicode: false),
                        NativePlace = c.String(maxLength: 500),
                        Address = c.String(maxLength: 500),
                        Email = c.String(maxLength: 500),
                        Tel = c.String(maxLength: 20, fixedLength: true, unicode: false),
                        HaseMedicalHistory = c.Boolean(),
                        MedicalHistoryComment = c.String(maxLength: 500),
                        HobbiesAndSpeciality = c.String(maxLength: 500),
                        EducationalExperience1StartDate = c.DateTime(storeType: "date"),
                        EducationalExperience1EndDate = c.DateTime(storeType: "date"),
                        EducationalExperience1SchoolName = c.String(maxLength: 500),
                        EducationalExperience1Major = c.String(maxLength: 500),
                        EducationalExperience1Certificate = c.String(maxLength: 500),
                        EducationalExperience2StartDate = c.DateTime(storeType: "date"),
                        EducationalExperience2EndDate = c.DateTime(storeType: "date"),
                        EducationalExperience2SchoolName = c.String(maxLength: 500),
                        EducationalExperience2Major = c.String(maxLength: 500),
                        EducationalExperience2Certificate = c.String(maxLength: 500),
                        EducationalExperience3StartDate = c.DateTime(storeType: "date"),
                        EducationalExperience3EndDate = c.DateTime(storeType: "date"),
                        EducationalExperience3SchoolName = c.String(maxLength: 500),
                        EducationalExperience3Major = c.String(maxLength: 500),
                        EducationalExperience3Certificate = c.String(maxLength: 500),
                        EducationalExperience4StartDate = c.DateTime(storeType: "date"),
                        EducationalExperience4EndDate = c.DateTime(storeType: "date"),
                        EducationalExperience4SchoolName = c.String(maxLength: 500),
                        EducationalExperience4Major = c.String(maxLength: 500),
                        EducationalExperience4Certificate = c.String(maxLength: 500),
                        FamilyStatus1Name = c.String(maxLength: 500),
                        FamilyStatus1Appellation = c.String(maxLength: 500),
                        FamilyStatus1Company = c.String(maxLength: 500),
                        FamilyStatus1Tel = c.String(maxLength: 20, unicode: false),
                        FamilyStatus2Name = c.String(maxLength: 500),
                        FamilyStatus2Appellation = c.String(maxLength: 500),
                        FamilyStatus2Company = c.String(maxLength: 500),
                        FamilyStatus2Tel = c.String(maxLength: 20, unicode: false),
                        EmergencyContactName = c.String(maxLength: 500),
                        EmergencyContactTel = c.String(maxLength: 20, unicode: false),
                        EmergencyContactEelationShipWithMe = c.String(maxLength: 500),
                        WorkExperience1CompanyName = c.String(maxLength: 500),
                        WorkExperience1Job = c.String(maxLength: 500),
                        WorkExperience1StartDate = c.DateTime(storeType: "date"),
                        WorkExperience1EndDate = c.DateTime(storeType: "date"),
                        WorkExperience1Pay = c.String(maxLength: 50),
                        WorkExperience1LeavingReasons = c.String(maxLength: 100),
                        WorkExperience2CompanyName = c.String(maxLength: 500),
                        WorkExperience2Job = c.String(maxLength: 500),
                        WorkExperience2StartDate = c.DateTime(storeType: "date"),
                        WorkExperience2EndDate = c.DateTime(storeType: "date"),
                        WorkExperience2Pay = c.String(maxLength: 50),
                        WorkExperience2LeavingReasons = c.String(maxLength: 100),
                        WorkExperience3CompanyName = c.String(maxLength: 500),
                        WorkExperience3Job = c.String(maxLength: 500),
                        WorkExperience3StartDate = c.DateTime(storeType: "date"),
                        WorkExperience3EndDate = c.DateTime(storeType: "date"),
                        WorkExperience3Pay = c.String(maxLength: 50),
                        WorkExperience3LeavingReasons = c.String(maxLength: 100),
                        WorkExperience4CompanyName = c.String(maxLength: 500),
                        WorkExperience4Job = c.String(maxLength: 500),
                        WorkExperience4StartDate = c.DateTime(storeType: "date"),
                        WorkExperience4EndDate = c.DateTime(storeType: "date"),
                        WorkExperience4Pay = c.String(maxLength: 50),
                        WorkExperience4LeavingReasons = c.String(maxLength: 100),
                        JobId = c.Guid(),
                        Title = c.String(maxLength: 50),
                        Organization_Id = c.Guid(),
                        SupervisorComments = c.String(maxLength: 500),
                        ProbationarySalary = c.String(maxLength: 50),
                        CorrectSalary = c.String(maxLength: 50),
                        WorkNumber = c.String(maxLength: 50),
                        EntryDate = c.DateTime(storeType: "date"),
                        BirthCertificatePhoto = c.String(maxLength: 200),
                        RegistrationPhoto = c.String(maxLength: 200),
                        BankCardNumber = c.String(maxLength: 200),
                        CreateStaffeId = c.Guid(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        No = c.String(maxLength: 50, unicode: false),
                        StaffNo = c.String(maxLength: 100),
                        IsDel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Code = c.String(maxLength: 100),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                        IsDel = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveOffice",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        No = c.String(maxLength: 100),
                        StaffName = c.String(maxLength: 100),
                        JobId = c.Guid(nullable: false),
                        LeaveType = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        ApplyDate = c.DateTime(storeType: "date"),
                        Reason = c.String(maxLength: 500),
                        ExplanationHandover = c.String(maxLength: 500),
                        HandoverSatffId = c.Guid(),
                        ApplyPersonId = c.Guid(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        IsDel = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StaffId = c.Guid(nullable: false),
                        LoginTime = c.DateTime(precision: 0, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Staff", t => t.StaffId)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        No = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                        Sex = c.Boolean(),
                        BirthDay = c.DateTime(storeType: "date"),
                        NativePlace = c.String(maxLength: 500),
                        Address = c.String(maxLength: 500),
                        Password = c.String(maxLength: 1000),
                        Email = c.String(maxLength: 50),
                        Tel = c.String(maxLength: 20),
                        Status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        JobId = c.Guid(),
                        OrgID = c.Guid(),
                        IsHSEGroup = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OilMaterialOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        No = c.String(nullable: false, maxLength: 100),
                        ApplyPersonId = c.Guid(nullable: false),
                        ApplyDate = c.DateTime(storeType: "date"),
                        Remark = c.String(maxLength: 500),
                        IsDel = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OilMaterialOrderDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        OilSpec = c.String(maxLength: 100),
                        Volume = c.Decimal(precision: 18, scale: 2),
                        Surplus = c.Decimal(precision: 18, scale: 2),
                        DayAvg = c.Decimal(precision: 18, scale: 2),
                        NeedAmount = c.Decimal(precision: 18, scale: 2),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        IsDel = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrganizationStructure",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        Leve = c.Int(nullable: false),
                        ParentId = c.Guid(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        IsDel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProcessItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 100, unicode: false),
                        Discrible = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProcessStepRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(nullable: false, maxLength: 50, unicode: false),
                        RecordRemarks = c.String(maxLength: 500),
                        StepOrder = c.Int(nullable: false),
                        WaitForExecutionStaffId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        WhetherToExecute = c.Boolean(nullable: false),
                        No = c.String(nullable: false, maxLength: 50),
                        RefOrderId = c.Guid(nullable: false),
                        Result = c.Boolean(nullable: false),
                        ExecuteMethod = c.String(maxLength: 4000),
                        Discrible = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        Code = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleResourceModule",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ResourceModuleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StaffRole",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StaffId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemResourceModule",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 500),
                        Code = c.String(nullable: false, maxLength: 500),
                        Url = c.String(maxLength: 1000),
                        Type = c.Int(nullable: false),
                        ParentId = c.Guid(),
                        OrderNo = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.View_StaffJ",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        No = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                        Sex = c.Boolean(),
                        BirthDay = c.DateTime(storeType: "date"),
                        NativePlace = c.String(maxLength: 500),
                        Address = c.String(maxLength: 500),
                        Password = c.String(maxLength: 1000),
                        Email = c.String(maxLength: 50),
                        Tel = c.String(maxLength: 20),
                        Status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        JobId = c.Guid(),
                        OrgID = c.Guid(),
                        IsHSEGroup = c.Boolean(),
                        JobName = c.String(maxLength: 100),
                        JobCode = c.String(maxLength: 100),
                        JobUpdateTime = c.DateTime(),
                        JobCreateTime = c.DateTime(),
                        JobIsDel = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginInfo", "StaffId", "dbo.Staff");
            DropIndex("dbo.LoginInfo", new[] { "StaffId" });
            DropTable("dbo.View_StaffJ");
            DropTable("dbo.SystemResourceModule");
            DropTable("dbo.StaffRole");
            DropTable("dbo.RoleResourceModule");
            DropTable("dbo.Role");
            DropTable("dbo.ProcessStepRecord");
            DropTable("dbo.ProcessItem");
            DropTable("dbo.OrganizationStructure");
            DropTable("dbo.OilMaterialOrderDetail");
            DropTable("dbo.OilMaterialOrder");
            DropTable("dbo.Staff");
            DropTable("dbo.LoginInfo");
            DropTable("dbo.LeaveOffice");
            DropTable("dbo.Job");
            DropTable("dbo.Entry");
            DropTable("dbo.Approver");
        }
    }
}
