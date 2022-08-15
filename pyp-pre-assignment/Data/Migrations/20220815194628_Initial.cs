using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pyp_pre_assignment.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commerces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitsSold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturingPrice = table.Column<double>(type: "float", nullable: true),
                    SalePrice = table.Column<double>(type: "float", nullable: true),
                    GrossSales = table.Column<double>(type: "float", nullable: true),
                    Discounts = table.Column<double>(type: "float", nullable: true),
                    Sales = table.Column<double>(type: "float", nullable: true),
                    COGS = table.Column<double>(type: "float", nullable: true),
                    Profit = table.Column<double>(type: "float", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commerces", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commerces");
        }
    }
}
