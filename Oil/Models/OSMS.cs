namespace Oil.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OSMS : DbContext
    {
        public OSMS()
            : base("name=OSMS")
        {
        }

        public virtual DbSet<Approver> Approver { get; set; }
        public virtual DbSet<Entry> Entry { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<LeaveOffice> LeaveOffice { get; set; }
        public virtual DbSet<LoginInfo> LoginInfo { get; set; }
        public virtual DbSet<OilMaterialOrder> OilMaterialOrder { get; set; }
        public virtual DbSet<OilMaterialOrderDetail> OilMaterialOrderDetail { get; set; }
        public virtual DbSet<OrganizationStructure> OrganizationStructure { get; set; }
        public virtual DbSet<ProcessItem> ProcessItem { get; set; }
        public virtual DbSet<ProcessStepRecord> ProcessStepRecord { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleResourceModule> RoleResourceModule { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffRole> StaffRole { get; set; }
        public virtual DbSet<SystemResourceModule> SystemResourceModule { get; set; }
        public virtual DbSet<View_Entry> View_Entry { get; set; }
        public virtual DbSet<View_LeaveOfficeJ> View_LeaveOfficeJ { get; set; }
        public virtual DbSet<View_OilMaterialOrderSP> View_OilMaterialOrderSP { get; set; }
        public virtual DbSet<View_StaffJ> View_StaffJ { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Approver>()
                .Property(e => e.JobCode)
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.Height)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Entry>()
                .Property(e => e.IDNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.Tel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.FamilyStatus1Tel)
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.FamilyStatus2Tel)
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.EmergencyContactTel)
                .IsUnicode(false);

            modelBuilder.Entity<Entry>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveOffice>()
                .Property(e => e.LeaveType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LoginInfo>()
                .Property(e => e.LoginTime)
                .HasPrecision(0);

            modelBuilder.Entity<ProcessItem>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<ProcessStepRecord>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.LoginInfo)
                .WithRequired(e => e.Staff)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.Height)
                .HasPrecision(5, 2);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.IDNumber)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.Tel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.FamilyStatus1Tel)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.FamilyStatus2Tel)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.EmergencyContactTel)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.NeedExec)
                .IsUnicode(false);

            modelBuilder.Entity<View_Entry>()
                .Property(e => e.Discrible)
                .IsUnicode(false);

            modelBuilder.Entity<View_LeaveOfficeJ>()
                .Property(e => e.LeaveType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<View_LeaveOfficeJ>()
                .Property(e => e.NeedExec)
                .IsUnicode(false);

            modelBuilder.Entity<View_LeaveOfficeJ>()
                .Property(e => e.Discrible)
                .IsUnicode(false);

            modelBuilder.Entity<View_OilMaterialOrderSP>()
                .Property(e => e.NeedExec)
                .IsUnicode(false);

            modelBuilder.Entity<View_OilMaterialOrderSP>()
                .Property(e => e.Discrible)
                .IsUnicode(false);

            modelBuilder.Entity<View_StaffJ>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
