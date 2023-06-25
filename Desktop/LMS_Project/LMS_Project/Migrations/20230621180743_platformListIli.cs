using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Project.Migrations
{
    /// <inheritdoc />
    public partial class platformListIli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostMedias_tbl_platform_MediaListId",
                table: "PostMedias");

            migrationBuilder.DropIndex(
                name: "IX_PostMedias_MediaListId",
                table: "PostMedias");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "tbl_platform");

            migrationBuilder.DropColumn(
                name: "MediaListId",
                table: "PostMedias");

            migrationBuilder.CreateTable(
                name: "PostPlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostPlatforms_PostMedias_PostId",
                        column: x => x.PostId,
                        principalTable: "PostMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostPlatforms_tbl_platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "tbl_platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostPlatforms_PlatformId",
                table: "PostPlatforms",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_PostPlatforms_PostId",
                table: "PostPlatforms",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostPlatforms");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "tbl_platform",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaListId",
                table: "PostMedias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostMedias_MediaListId",
                table: "PostMedias",
                column: "MediaListId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostMedias_tbl_platform_MediaListId",
                table: "PostMedias",
                column: "MediaListId",
                principalTable: "tbl_platform",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
