using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScholarSystem_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumnToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");
        }
    }
}
