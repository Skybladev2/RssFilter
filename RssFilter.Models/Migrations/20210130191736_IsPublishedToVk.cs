using Microsoft.EntityFrameworkCore.Migrations;

namespace RssFilter.Models.Migrations
{
    public partial class IsPublishedToVk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsPublishedToVk", "Posts", defaultValue: false, nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
