using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class updateV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE update_company_name(
                    IN id INTEGER,
                    IN new_name TEXT
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    UPDATE ""Company""
                    SET ""Name"" = new_name
                    WHERE ""Id"" = id;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS update_company_name(INTEGER, TEXT);
            ");
        }
    }
}
