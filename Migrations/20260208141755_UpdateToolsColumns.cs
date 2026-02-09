using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pets.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToolsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tools",
                table: "Tools");

            migrationBuilder.RenameTable(
                name: "Tools",
                newName: "tools");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tools",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "tools",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tools",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "tools",
                newName: "is_active");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tools",
                table: "tools",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tools",
                table: "tools");

            migrationBuilder.RenameTable(
                name: "tools",
                newName: "Tools");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tools",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Tools",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tools",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Tools",
                newName: "IsActive");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tools",
                table: "Tools",
                column: "Id");
        }
    }
}
