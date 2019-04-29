namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSP : DbMigration
    {
        public override void Up()
        {
            DropStoredProcedure("dbo.Department_Insert");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Delete");
        }
        
        public override void Down()
        {
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
