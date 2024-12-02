using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class GrantFullAccessToStorageBoxesProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE grant_access_to_all_boxes(
                    IN user_id INTEGER,
                    IN company_id INTEGER,
                    IN storage_id INTEGER
                )
                LANGUAGE plpgsql
                AS $$
                DECLARE
                    box RECORD;
                BEGIN
                    IF EXISTS (SELECT 1 FROM ""User"" WHERE ""Id"" = user_id AND ""CompanyId"" = company_id) THEN
                        FOR box IN
                            SELECT b.""Id""
                            FROM ""Box"" b
                            JOIN ""Section"" s ON b.""SectionId"" = s.""Id""
                            JOIN ""Row"" r ON s.""RowId"" = r.""Id""
                            WHERE r.""StorageId"" = storage_id
                        LOOP
                            IF NOT EXISTS (
                                SELECT 1
                                FROM ""Access""
                                WHERE ""UserId"" = user_id AND ""BoxId"" = box.""Id""
                            ) THEN
                                INSERT INTO ""Access"" (""UserId"", ""BoxId"")
                                VALUES (user_id, box.""Id"");
                            END IF;
                        END LOOP;
                    ELSE
                        RAISE EXCEPTION 'User is not part of the specified company';
                    END IF;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS grant_access_to_all_boxes(INTEGER, INTEGER, INTEGER);
            ");
        }
    }
}
