using FluentMigrator;


namespace dotnetserver
{
    [Migration(018)]
    public class UpdateUserAvatarString_018 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.018.UpdateUserAvatarString.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.018.UpdateUserAvatarString.sql");
        }
        
    }
}