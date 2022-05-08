using FluentMigrator;

namespace TopCourseWorkBl.DataLayer.Migrations
{
    [Migration(0)]
    public class AuthMigration : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("roles")
                .WithColumn("role").AsString().PrimaryKey().Unique()
                .WithColumn("description").AsString()
                .WithColumn("rules").AsCustom("jsonb").WithDefaultValue("{}");

            Create.Table("rules")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("name").AsString().Unique();

            Create.Table("roles_rules")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rule_id").AsInt64().ForeignKey("rules", "id")
                .WithColumn("role").AsString().ForeignKey("roles", "role");

            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("username").AsString().Unique()
                .WithColumn("password").AsString()
                .WithColumn("role").AsString().ForeignKey("roles", "role");

            Create.Table("refresh_tokens")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
                .WithColumn("token").AsString()
                .WithColumn("expires_at").AsDateTime()
                .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("created_by_ip").AsString()
                .WithColumn("revoked_at").AsDateTime().Nullable()
                .WithColumn("revoked_by_ip").AsString().Nullable()
                .WithColumn("replaced_by_token").AsString().Nullable();
            
            Insert.IntoTable("roles").Row(new { role = "defaultAdmin", description = "SysAdmin" })
                .Row(new { role = "user", description = "User" });
        }
    }
}