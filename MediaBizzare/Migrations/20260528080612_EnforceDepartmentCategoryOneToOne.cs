using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaBizzare.Migrations
{
    /// <inheritdoc />
    public partial class EnforceDepartmentCategoryOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_DepartmentId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepartmentId",
                table: "Categories",
                column: "DepartmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_DepartmentId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepartmentId",
                table: "Categories",
                column: "DepartmentId");
        }
    }
}
