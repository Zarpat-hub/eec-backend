using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eecbackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ModelIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierOrTrademark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DimensionWidth = table.Column<double>(type: "float", nullable: true),
                    DimensionHeight = table.Column<double>(type: "float", nullable: true),
                    DimensionDeptht = table.Column<double>(type: "float", nullable: true),
                    EnergyEfficiencyClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyEfficiencyIndex = table.Column<double>(type: "float", nullable: false),
                    DesignType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnergyConsumption = table.Column<double>(type: "float", nullable: false),
                    EnergySource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedCapacity = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ModelIdentifier);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
