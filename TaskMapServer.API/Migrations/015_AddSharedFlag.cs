using FluentMigrator;


namespace dotnetserver
{
    [Migration(015)]
    public class AddSharedFlag_015 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.015.AddSharedFlag.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.015.AddSharedFlag.sql");
        }
        
    }
}