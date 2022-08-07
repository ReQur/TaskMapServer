using FluentMigrator;


namespace dotnetserver
{
    [Migration(006)]
    public class InsertUserTrigger_006 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.006.InsertUserTrigger.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.006.InsertUserTrigger.sql");
        }
        
    }
}