using FluentMigrator;


namespace dotnetserver
{
    [Migration(013)]
    public class DoUserUnnecessaryFieldsNullable_013 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.013.DoUserUnnecessaryFieldsNullable.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.013.DoUserUnnecessaryFieldsNullable.sql");
        }
        
    }
}