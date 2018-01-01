using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportsStore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(name: "Products",
                columns: table => new
                {
                    ProductID =
                    table.Column<int>(type: "INTEGER", nullable: false)
                         .Annotation("Sqlite:Autoincrement", true),
                    Category    = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name        = table.Column<string>(type: "TEXT", nullable: true),
                    Price       = table.Column<decimal>(type: "TEXT", nullable: false)
                }, constraints: table => { table.PrimaryKey("PK_Products", x => x.ProductID); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Products");
        }
    }
}