using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class likes_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChapterLikes_AccountId",
                table: "ChapterLikes");

            migrationBuilder.DropIndex(
                name: "IX_ChapterLikes_ChapterId",
                table: "ChapterLikes");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLikes_ChapterId_AccountId",
                table: "ChapterLikes",
                columns: new[] { "ChapterId", "AccountId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChapterLikes_ChapterId_AccountId",
                table: "ChapterLikes");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLikes_AccountId",
                table: "ChapterLikes",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLikes_ChapterId",
                table: "ChapterLikes",
                column: "ChapterId");
        }
    }
}
