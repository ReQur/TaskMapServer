using FluentMigrator;


namespace dotnetserver
{
    [Migration(005)]
    public class CreateUserBoardForeignKey_005 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.005.CreateUserBoardForeignKey.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.005.CreateUserBoardForeignKey.sql");
        }
        
    }
}