using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                                       "Persons");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                                         "Persons",
                                         table => new
                                                  {
                                                      Id = table.Column<int>("int", nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      FirstName = table.Column<string>("nvarchar(20)", maxLength: 20, nullable: true),
                                                      LastName = table.Column<string>("nvarchar(20)", maxLength: 20, nullable: true),
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_Persons", x => x.Id); });
        }
    }
}
