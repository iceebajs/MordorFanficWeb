using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class chapters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    ChapterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterNumber = table.Column<int>(nullable: false),
                    ChapterTitle = table.Column<string>(nullable: true),
                    Context = table.Column<string>(nullable: true),
                    ImgSource = table.Column<string>(nullable: true),
                    CompositionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.ChapterId);
                    table.ForeignKey(
                        name: "FK_Chapter_Compositions_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "Compositions",
                        principalColumn: "CompositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_CompositionId",
                table: "Chapter",
                column: "CompositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapter");
        }
    }
}
