using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_contenttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "AdGalleries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "AdGalleries");
        }
    }
}