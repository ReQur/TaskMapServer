using FluentMigrator;


namespace dotnetserver
{
    [Migration(016)]
    public class UpdateUpdateUserLastBoardTrg_016 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.016.UpdateUpdateUserLastBoardTrg.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.016.UpdateUpdateUserLastBoardTrg.sql");
        }
        
    }
}