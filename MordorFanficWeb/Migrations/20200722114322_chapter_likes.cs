using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class chapter_likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompositionRatings_Compositions_CompositionId",
                table: "CompositionRatings");

            migrationBuilder.AlterColumn<int>(
                name: "CompositionId",
                table: "CompositionRatings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChapterLikes",
                columns: table => new
                {
                    ChapterLikeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    ChapterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterLikes", x => x.ChapterLikeId);
                    table.ForeignKey(
                        name: "FK_ChapterLikes_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "ChapterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLikes_AccountId",
                table: "ChapterLikes",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLikes_ChapterId",
                table: "ChapterLikes",
                column: "ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionRatings_Compositions_CompositionId",
                table: "CompositionRatings",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "CompositionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompositionRatings_Compositions_CompositionId",
                table: "CompositionRatings");

            migrationBuilder.DropTable(
                name: "ChapterLikes");

            migrationBuilder.AlterColumn<int>(
                name: "CompositionId",
                table: "CompositionRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionRatings_Compositions_CompositionId",
                table: "CompositionRatings",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "CompositionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
