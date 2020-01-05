namespace Oil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeaveOffice_ApplyPersonId_allowNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeaveOffice", "ApplyPersonId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeaveOffice", "ApplyPersonId", c => c.Guid());
        }
    }
}
