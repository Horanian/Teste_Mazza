using Microsoft.EntityFrameworkCore.Migrations;

namespace Trades.Migrations
{
    public partial class OutPuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutPuts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutPuts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OutPuts_Inputs_UserId",
                        column: x => x.UserId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutPuts_UserId",
                table: "OutPuts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutPuts");
        }
    }
}
