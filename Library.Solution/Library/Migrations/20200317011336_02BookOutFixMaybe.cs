using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class _02BookOutFixMaybe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Out",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Out",
                table: "Books");
        }
    }
}
