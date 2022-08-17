using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetSearcher.Migrations
{
    public partial class AddUserIdtoNoticetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notices");
        }
    }
}
