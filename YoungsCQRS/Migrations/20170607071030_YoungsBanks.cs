using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YoungsCQRS.Migrations
{
    public partial class YoungsBanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AggregateID = table.Column<Guid>(nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AggregateID);
                });

            migrationBuilder.CreateTable(
                name: "EventStorage",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Eventid = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStorage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "EventStorage");
        }
    }
}
