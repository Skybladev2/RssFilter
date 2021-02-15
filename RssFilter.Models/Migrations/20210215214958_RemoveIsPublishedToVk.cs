using Microsoft.EntityFrameworkCore.Migrations;

namespace RssFilter.Models.Migrations
{
    public partial class RemoveIsPublishedToVk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("IsPublishedToVk", "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
