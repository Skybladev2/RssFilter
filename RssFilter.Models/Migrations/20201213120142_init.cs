using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RssFilter.Models.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LastItemId = table.Column<string>(type: "TEXT", nullable: true),
                    BaseUrl = table.Column<string>(type: "TEXT", nullable: true),
                    PublicUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeedId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_Feeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeedId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Summary = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Feeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_FeedId",
                table: "Keywords",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FeedId",
                table: "Posts",
                column: "FeedId");

            migrationBuilder.InsertData(
                table: "Feeds",
                columns: new string[] { "Id", "Name", "BaseUrl" },
                values: new object[] { new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "favt.ru БПЛА", "https://favt.ru/novosti-novosti/?rss" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new string[] { "Id", "FeedId", "Text" },
                values: new object[] { new Guid("58090521-4884-41f5-9356-57a6e590e6d1"), new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "бпла" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new string[] { "Id", "FeedId", "Text" },
                values: new object[] { new Guid("8A3D5C0C-897C-4B7E-8557-9EEE8F53DD3F"), new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "бвс" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new string[] { "Id", "FeedId", "Text" },
                values: new object[] { new Guid("6CF05AB3-75DB-4CF1-97B7-1DFC4A4AF58F"), new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "дрон" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new string[] { "Id", "FeedId", "Text" },
                values: new object[] { new Guid("57F0ACC5-7BF7-4CFE-88D9-D52253FBD67B"), new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "коптер" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new string[] { "Id", "FeedId", "Text" },
                values: new object[] { new Guid("A2C2FAB9-6A43-40C8-AFC6-4B46B4F4BDE1"), new Guid("dc917bad-4e16-4ecc-9aa0-5424269442e5"), "беспилот" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Feeds");
        }
    }
}
