namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OrganizerId = c.Int(nullable: false),
                        AgeFrom = c.Int(nullable: false),
                        AgeTo = c.Int(nullable: false),
                        Phones = c.String(),
                        Address = c.String(),
                        Prices = c.String(),
                        Mentor = c.String(),
                        Description = c.String(),
                        InterestId = c.Int(),
                        IsChecked = c.Boolean(nullable: false),
                        Free = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.InterestId)
                .ForeignKey("dbo.Organizers", t => t.OrganizerId, cascadeDelete: true)
                .Index(t => t.OrganizerId)
                .Index(t => t.InterestId);
            
            CreateTable(
                "dbo.ActivityUserVoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserEmail = c.String(maxLength: 255),
                        ActivityId = c.Int(nullable: false),
                        VoiceValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserEmail)
                .Index(t => t.UserEmail)
                .Index(t => t.ActivityId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 255),
                        Id = c.Int(nullable: false),
                        AuthToken = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(maxLength: 255, unicode: false),
                        Phone = c.String(maxLength: 255, unicode: false),
                        PictureId = c.Int(),
                        Role = c.Int(nullable: false),
                        CityId = c.Int(),
                        DateRegistered = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Email)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .Index(t => t.AuthToken)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.PictureId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Filename = c.String(nullable: false, maxLength: 100),
                        IsMain = c.Boolean(nullable: false),
                        LinkedObjectId = c.Int(nullable: false),
                        LinkedObjectType = c.Int(nullable: false),
                        Data = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        CityId = c.Int(nullable: false),
                        Sobriety = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.Name)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        UserEmail = c.String(nullable: false, maxLength: 255),
                        Name = c.String(maxLength: 255, unicode: false),
                        Phone = c.String(maxLength: 255, unicode: false),
                        Comment = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserEmail, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.UserEmail)
                .Index(t => t.Name)
                .Index(t => t.Phone);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserEmail = c.String(nullable: false, maxLength: 255),
                        ActivityId = c.Int(nullable: false),
                        Text = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        ReplyToReviewId = c.Int(),
                        IsChecked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Reviews", t => t.ReplyToReviewId)
                .ForeignKey("dbo.Users", t => t.UserEmail, cascadeDelete: true)
                .Index(t => t.UserEmail)
                .Index(t => t.ActivityId)
                .Index(t => t.ReplyToReviewId);
            
            CreateTable(
                "dbo.TempFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.String(),
                        Filename = c.String(nullable: false),
                        Data = c.Binary(nullable: false),
                        IsMain = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserEmail", "dbo.Users");
            DropForeignKey("dbo.Reviews", "ReplyToReviewId", "dbo.Reviews");
            DropForeignKey("dbo.Reviews", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Reservations", "UserEmail", "dbo.Users");
            DropForeignKey("dbo.Reservations", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Activities", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.Organizers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Activities", "InterestId", "dbo.Interests");
            DropForeignKey("dbo.ActivityUserVoices", "UserEmail", "dbo.Users");
            DropForeignKey("dbo.Users", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Users", "CityId", "dbo.Cities");
            DropForeignKey("dbo.ActivityUserVoices", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Reviews", new[] { "ReplyToReviewId" });
            DropIndex("dbo.Reviews", new[] { "ActivityId" });
            DropIndex("dbo.Reviews", new[] { "UserEmail" });
            DropIndex("dbo.Reservations", new[] { "Phone" });
            DropIndex("dbo.Reservations", new[] { "Name" });
            DropIndex("dbo.Reservations", new[] { "UserEmail" });
            DropIndex("dbo.Reservations", new[] { "ActivityId" });
            DropIndex("dbo.Organizers", new[] { "CityId" });
            DropIndex("dbo.Organizers", new[] { "Name" });
            DropIndex("dbo.Users", new[] { "CityId" });
            DropIndex("dbo.Users", new[] { "PictureId" });
            DropIndex("dbo.Users", new[] { "Phone" });
            DropIndex("dbo.Users", new[] { "Name" });
            DropIndex("dbo.Users", new[] { "AuthToken" });
            DropIndex("dbo.ActivityUserVoices", new[] { "ActivityId" });
            DropIndex("dbo.ActivityUserVoices", new[] { "UserEmail" });
            DropIndex("dbo.Activities", new[] { "InterestId" });
            DropIndex("dbo.Activities", new[] { "OrganizerId" });
            DropTable("dbo.TempFiles");
            DropTable("dbo.Reviews");
            DropTable("dbo.Reservations");
            DropTable("dbo.Organizers");
            DropTable("dbo.Interests");
            DropTable("dbo.Pictures");
            DropTable("dbo.Cities");
            DropTable("dbo.Users");
            DropTable("dbo.ActivityUserVoices");
            DropTable("dbo.Activities");
        }
    }
}
