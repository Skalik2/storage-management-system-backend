using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class GetEmptyAccessBoxesByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_empty_access_boxes_by_user(
                    user_id INTEGER
                )
                RETURNS TABLE(BoxId INTEGER)
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT b.""Id""
                    FROM ""Box"" b
                    INNER JOIN ""Access"" a ON b.""Id"" = a.""BoxId""
                    WHERE a.""UserId"" = user_id
                      AND NOT EXISTS (
                          SELECT 1
                          FROM ""ItemInstance"" ii
                          WHERE ii.""BoxId"" = b.""Id""
                      );
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_empty_access_boxes_by_user(INTEGER);
            ");
        }
    }
}
