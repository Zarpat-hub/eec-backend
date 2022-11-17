using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eecbackend.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameTypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DimensionDeptht",
                table: "Products",
                newName: "DimensionDepth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DimensionDepth",
                table: "Products",
                newName: "DimensionDeptht");
        }
    }
}
