using FluentMigrator;


namespace dotnetserver
{
    [Migration(011)]
    public class RemoveUserRegisterProc_011 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.011.RemoveUserRegisterProc.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.011.RemoveUserRegisterProc.sql");
        }
        
    }
}