namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_Entry
    {
        [Key]
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string StaffName { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDay { get; set; }

        public bool? MaritalStatus { get; set; }

        public decimal? Height { get; set; }

        public int? HighestEducation { get; set; }

        [StringLength(500)]
        public string Major { get; set; }

        [StringLength(500)]
        public string ForeginLanguageAptitude { get; set; }

        [StringLength(20)]
        public string IDNumber { get; set; }

        [StringLength(500)]
        public string NativePlace { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        public bool? HaseMedicalHistory { get; set; }

        [StringLength(500)]
        public string MedicalHistoryComment { get; set; }

        [StringLength(500)]
        public string HobbiesAndSpeciality { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience1StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience1EndDate { get; set; }

        [StringLength(500)]
        public string EducationalExperience1SchoolName { get; set; }

        [StringLength(500)]
        public string EducationalExperience1Major { get; set; }

        [StringLength(500)]
        public string EducationalExperience1Certificate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience2StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience2EndDate { get; set; }

        [StringLength(500)]
        public string EducationalExperience2SchoolName { get; set; }

        [StringLength(500)]
        public string EducationalExperience2Major { get; set; }

        [StringLength(500)]
        public string EducationalExperience2Certificate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience3StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience3EndDate { get; set; }

        [StringLength(500)]
        public string EducationalExperience3SchoolName { get; set; }

        [StringLength(500)]
        public string EducationalExperience3Major { get; set; }

        [StringLength(500)]
        public string EducationalExperience3Certificate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience4StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EducationalExperience4EndDate { get; set; }

        [StringLength(500)]
        public string EducationalExperience4SchoolName { get; set; }

        [StringLength(500)]
        public string EducationalExperience4Major { get; set; }

        [StringLength(500)]
        public string EducationalExperience4Certificate { get; set; }

        [StringLength(500)]
        public string FamilyStatus1Name { get; set; }

        [StringLength(500)]
        public string FamilyStatus1Appellation { get; set; }

        [StringLength(500)]
        public string FamilyStatus1Company { get; set; }

        [StringLength(20)]
        public string FamilyStatus1Tel { get; set; }

        [StringLength(500)]
        public string FamilyStatus2Name { get; set; }

        [StringLength(500)]
        public string FamilyStatus2Appellation { get; set; }

        [StringLength(500)]
        public string FamilyStatus2Company { get; set; }

        [StringLength(20)]
        public string FamilyStatus2Tel { get; set; }

        [StringLength(500)]
        public string EmergencyContactName { get; set; }

        [StringLength(20)]
        public string EmergencyContactTel { get; set; }

        [StringLength(500)]
        public string EmergencyContactEelationShipWithMe { get; set; }

        [StringLength(500)]
        public string WorkExperience1CompanyName { get; set; }

        [StringLength(500)]
        public string WorkExperience1Job { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience1StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience1EndDate { get; set; }

        [StringLength(50)]
        public string WorkExperience1Pay { get; set; }

        [StringLength(100)]
        public string WorkExperience1LeavingReasons { get; set; }

        [StringLength(500)]
        public string WorkExperience2CompanyName { get; set; }

        [StringLength(500)]
        public string WorkExperience2Job { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience2StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience2EndDate { get; set; }

        [StringLength(50)]
        public string WorkExperience2Pay { get; set; }

        [StringLength(100)]
        public string WorkExperience2LeavingReasons { get; set; }

        [StringLength(500)]
        public string WorkExperience3CompanyName { get; set; }

        [StringLength(500)]
        public string WorkExperience3Job { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience3StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience3EndDate { get; set; }

        [StringLength(50)]
        public string WorkExperience3Pay { get; set; }

        [StringLength(100)]
        public string WorkExperience3LeavingReasons { get; set; }

        [StringLength(500)]
        public string WorkExperience4CompanyName { get; set; }

        [StringLength(500)]
        public string WorkExperience4Job { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience4StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkExperience4EndDate { get; set; }

        [StringLength(50)]
        public string WorkExperience4Pay { get; set; }

        [StringLength(100)]
        public string WorkExperience4LeavingReasons { get; set; }

        public Guid? JobId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public Guid? Organization_Id { get; set; }

        [StringLength(500)]
        public string SupervisorComments { get; set; }

        [StringLength(50)]
        public string ProbationarySalary { get; set; }

        [StringLength(50)]
        public string CorrectSalary { get; set; }

        [StringLength(50)]
        public string WorkNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EntryDate { get; set; }

        [StringLength(200)]
        public string BirthCertificatePhoto { get; set; }

        [StringLength(200)]
        public string RegistrationPhoto { get; set; }

        [StringLength(200)]
        public string BankCardNumber { get; set; }

        public Guid? CreateStaffeId { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(50)]
        public string No { get; set; }

        [StringLength(100)]
        public string StaffNo { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsDel { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string NeedExec { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string Discrible { get; set; }
    }
}
