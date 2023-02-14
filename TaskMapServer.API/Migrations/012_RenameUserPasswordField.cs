using FluentMigrator;


namespace dotnetserver
{
    [Migration(012)]
    public class RenameUserPasswordField_012 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.012.RenameUserPasswordField.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.012.RenameUserPasswordField.sql");
        }
        
    }
}