namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_user_about : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "About", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "About");
        }
    }
}
