namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class codeforfiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Code");
        }
    }
}
