using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomStorageProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE create_custom_storage_structure(
                    company_id INT,
                    location_id INT,
                    row_count_var INT,
                    section_count INT,
                    box_count INT
                )
                LANGUAGE plpgsql
                AS $$
                DECLARE
                    storage_id INT;
                    row_id INT;
                    section_id INT;
                BEGIN
                    INSERT INTO ""Storage"" (""CompanyId"", ""LocationId"", ""Model"")
                    VALUES (company_id, location_id, 'Custom')
                    RETURNING ""Id"" INTO storage_id;

                    FOR row_index IN 1..row_count_var LOOP
                        INSERT INTO ""Row"" (""StorageId"")
                        VALUES (storage_id)
                        RETURNING ""Id"" INTO row_id;

                        FOR section_index IN 1..section_count LOOP
                            INSERT INTO ""Section"" (""RowId"")
                            VALUES (row_id)
                            RETURNING ""Id"" INTO section_id;

                            FOR box_index IN 1..box_count LOOP
                                INSERT INTO ""Box"" (""SectionId"")
                                VALUES (section_id);
                            END LOOP;
                        END LOOP;
                    END LOOP;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS create_custom_storage_structure(INTEGER, INTEGER, INTEGER, INTEGER, INTEGER);
            ");
        }
    }
}
