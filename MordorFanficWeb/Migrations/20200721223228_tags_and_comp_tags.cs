using Microsoft.EntityFrameworkCore.Migrations;

namespace MordorFanficWeb.Migrations
{
    public partial class tags_and_comp_tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "CompositionTags",
                columns: table => new
                {
                    CompositionTagsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositionId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositionTags", x => x.CompositionTagsId);
                    table.ForeignKey(
                        name: "FK_CompositionTags_Compositions_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "Compositions",
                        principalColumn: "CompositionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompositionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompositionTags_CompositionId",
                table: "CompositionTags",
                column: "CompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionTags_TagId",
                table: "CompositionTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Tag",
                table: "Tags",
                column: "Tag",
                unique: true,
                filter: "[Tag] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompositionTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
