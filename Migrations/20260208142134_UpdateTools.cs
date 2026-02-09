using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pets.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToolItems",
                table: "ToolItems");

            migrationBuilder.RenameTable(
                name: "ToolItems",
                newName: "tool_items");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tool_items",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tool_items",
                table: "tool_items",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tool_items",
                table: "tool_items");

            migrationBuilder.RenameTable(
                name: "tool_items",
                newName: "ToolItems");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ToolItems",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToolItems",
                table: "ToolItems",
                column: "Id");
        }
    }
}
