namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class email_to_org : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizers", "Email", c => c.String());
            AddColumn("dbo.Organizers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizers", "Phone");
            DropColumn("dbo.Organizers", "Email");
        }
    }
}
