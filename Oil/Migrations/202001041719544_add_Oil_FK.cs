namespace Oil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Oil_FK : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OilMaterialOrderDetail", "OrderId");
            AddForeignKey("dbo.OilMaterialOrderDetail", "OrderId", "dbo.OilMaterialOrder", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OilMaterialOrderDetail", "OrderId", "dbo.OilMaterialOrder");
            DropIndex("dbo.OilMaterialOrderDetail", new[] { "OrderId" });
        }
    }
}
