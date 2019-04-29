namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class principalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "PrincipalID", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "PrincipalID");
        }
    }
}
