using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pyp_pre_assignment.Data.Migrations
{
    public partial class colum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UnitsSold",
                table: "Commerces",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitsSold",
                table: "Commerces",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
