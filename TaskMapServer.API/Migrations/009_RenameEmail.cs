using FluentMigrator;


namespace dotnetserver
{
    [Migration(009)]
    public class RenameEmai_009 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.009.RenameEmail.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.009.RenameEmail.sql");
        }
        
    }
}