using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storage_management_system.Migrations
{
    /// <inheritdoc />
    public partial class DeleteStorageByIdProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE delete_storage_by_id(storage_id INT)
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    DELETE FROM ""Box"" WHERE ""SectionId"" IN (
                        SELECT ""Id"" FROM ""Section"" WHERE ""RowId"" IN (
                            SELECT ""Id"" FROM ""Row"" WHERE ""StorageId"" = storage_id
                        )
                    );

                    DELETE FROM ""Section"" WHERE ""RowId"" IN (
                        SELECT ""Id"" FROM ""Row"" WHERE ""StorageId"" = storage_id
                    );

                    DELETE FROM ""Row"" WHERE ""StorageId"" = storage_id;

                    DELETE FROM ""Storage"" WHERE ""Id"" = storage_id;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS delete_storage_by_id;");
        }
    }
}
