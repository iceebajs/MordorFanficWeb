using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class composition_ratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompositionRatings",
                columns: table => new
                {
                    CompositionRatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    CompositionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositionRatings", x => x.CompositionRatingId);
                    table.ForeignKey(
                        name: "FK_CompositionRatings_Compositions_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "Compositions",
                        principalColumn: "CompositionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompositionRatings_AccountId",
                table: "CompositionRatings",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompositionRatings_CompositionId",
                table: "CompositionRatings",
                column: "CompositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompositionRatings");
        }
    }
}
