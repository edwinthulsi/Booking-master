namespace Booking_master.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Awesome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        forRef = c.String(),
                        refNo = c.String(),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        numberOfPeople = c.Int(nullable: false),
                        totalPrice = c.Double(nullable: false),
                        deposit = c.Double(nullable: false),
                        venue = c.String(),
                        registrations_userId = c.Int(),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Registrations", t => t.registrations_userId)
                .Index(t => t.registrations_userId);
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        firstName = c.String(nullable: false),
                        surName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        phoneNum = c.String(nullable: false, maxLength: 10),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                        position = c.String(nullable: false),
                        forRef = c.String(),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "registrations_userId", "dbo.Registrations");
            DropIndex("dbo.Bookings", new[] { "registrations_userId" });
            DropTable("dbo.Registrations");
            DropTable("dbo.Bookings");
        }
    }
}
