using Microsoft.EntityFrameworkCore.Migrations;

namespace FileManagement.Migrations
{
    public partial class AddMIMEtypetofiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MIMEtype",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MIMEtype",
                table: "Files");
        }
    }
}
