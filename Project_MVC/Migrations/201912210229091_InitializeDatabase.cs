namespace Project_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        ProductCode = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        OrderId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.ProductCode, t.OrderId })
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Products", t => t.ProductCode, cascadeDelete: true)
                .Index(t => t.ProductCode)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        PaymentTypeId = c.Int(nullable: false),
                        ShipName = c.String(unicode: false),
                        ShipAddress = c.String(unicode: false),
                        ShipPhone = c.String(unicode: false),
                        TotalPrice = c.Double(nullable: false),
                        CreatedAt = c.DateTime(precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, unicode: false),
                        Price = c.Double(nullable: false),
                        Description = c.String(unicode: false),
                        CreatedAt = c.DateTime(precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                        ProductCategoryCode = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryCode)
                .Index(t => t.ProductCategoryCode);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, unicode: false),
                        Description = c.String(unicode: false),
                        CreatedAt = c.DateTime(precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductCode", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryCode", "dbo.ProductCategories");
            DropForeignKey("dbo.OrderDetails", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "ProductCategoryCode" });
            DropIndex("dbo.OrderDetails", new[] { "Order_Id" });
            DropIndex("dbo.OrderDetails", new[] { "ProductCode" });
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
