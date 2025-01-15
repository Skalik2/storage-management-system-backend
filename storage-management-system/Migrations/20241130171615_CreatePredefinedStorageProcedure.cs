using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class CreatePredefinedStorageProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE create_predefined_storage_structure(
                    company_id INT,
                    location_id INT,
                    model TEXT
                )
                LANGUAGE plpgsql
                AS $$
                DECLARE
                    storage_id INT;
                    row_id INT;
                    section_id INT;

                    row_count_var INT;
                    section_count INT;
                    box_count INT;
                BEGIN
                    INSERT INTO ""Storage"" (""CompanyId"", ""LocationId"", ""Model"")
                    VALUES (company_id, location_id, model)
                    RETURNING ""Id"" INTO storage_id;

                    IF model = 'Small' THEN
                        row_count_var := 3;
                        section_count := 5;
                        box_count := 2;
                    ELSIF model = 'Medium' THEN
                        row_count_var := 4;
                        section_count := 6;
                        box_count := 3;
                    ELSIF model = 'Large' THEN
                        row_count_var := 5;
                        section_count := 8;
                        box_count := 4;
                    ELSE
                        RAISE EXCEPTION 'Invalid model: %', model;
                    END IF;

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

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS create_predefined_storage_structure(INTEGER, INTEGER, TEXT);
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
