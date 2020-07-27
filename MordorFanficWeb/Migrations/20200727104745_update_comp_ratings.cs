using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class update_comp_ratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompositionRatings_AccountId",
                table: "CompositionRatings");

            migrationBuilder.DropIndex(
                name: "IX_CompositionRatings_CompositionId",
                table: "CompositionRatings");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionRatings_CompositionId_AccountId",
                table: "CompositionRatings",
                columns: new[] { "CompositionId", "AccountId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompositionRatings_CompositionId_AccountId",
                table: "CompositionRatings");

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
    }
}
