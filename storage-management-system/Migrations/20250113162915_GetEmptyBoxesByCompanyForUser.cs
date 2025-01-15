using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class GetEmptyBoxesByCompanyForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_empty_boxes_by_company_for_user(
                    user_id INTEGER
                )
                RETURNS TABLE(BoxId INTEGER)
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT b.""Id""
                    FROM ""Box"" b
                    INNER JOIN ""Section"" s ON b.""SectionId"" = s.""Id""
                    INNER JOIN ""Row"" r ON s.""RowId"" = r.""Id""
                    INNER JOIN ""Storage"" st ON r.""StorageId"" = st.""Id""
                    INNER JOIN ""User"" u ON st.""CompanyId"" = u.""CompanyId""
                    WHERE u.""Id"" = user_id
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
                DROP FUNCTION IF EXISTS get_empty_boxes_by_company_for_user(INTEGER);
            ");
        }
    }
}
