using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAModels.Migrations
{
    /// <inheritdoc />
    public partial class newFixRelationsAndModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieDescriptions");

            migrationBuilder.DropTable(
                name: "MoviesImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieTags",
                table: "MovieTags");

            migrationBuilder.DropColumn(
                name: "MovieTags",
                table: "MovieTags");

            migrationBuilder.RenameTable(
                name: "MovieTags",
                newName: "MoviesTags");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Movies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieTagsId",
                table: "MoviesTags",
                newName: "TagId");

            migrationBuilder.AlterColumn<short>(
                name: "Vote",
                table: "Reviews",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "MoviesTags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MoviesTags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "MoviesTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesTags",
                table: "MoviesTags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoviesUsers_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieUser",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieUser", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MovieUser_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieTag",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    MovieTagId = table.Column<int>(type: "int", nullable: false),
                    MoviesListId = table.Column<int>(type: "int", nullable: false),
                    TagsListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTag", x => new { x.MovieId, x.MovieTagId });
                    table.ForeignKey(
                        name: "FK_MovieTag_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTag_Movies_MoviesListId",
                        column: x => x.MoviesListId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTag_Tags_MovieTagId",
                        column: x => x.MovieTagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTag_Tags_TagsListId",
                        column: x => x.TagsListId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTags_MovieId",
                table: "MoviesTags",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTags_TagId",
                table: "MoviesTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_MovieId",
                table: "Image",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesUsers_MovieId",
                table: "MoviesUsers",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesUsers_UserId",
                table: "MoviesUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTag_MoviesListId",
                table: "MovieTag",
                column: "MoviesListId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTag_MovieTagId",
                table: "MovieTag",
                column: "MovieTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTag_TagsListId",
                table: "MovieTag",
                column: "TagsListId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieUser_UserId",
                table: "MovieUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesTags_Movies_MovieId",
                table: "MoviesTags",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesTags_Tags_TagId",
                table: "MoviesTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesTags_Movies_MovieId",
                table: "MoviesTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesTags_Tags_TagId",
                table: "MoviesTags");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "MoviesUsers");

            migrationBuilder.DropTable(
                name: "MovieTag");

            migrationBuilder.DropTable(
                name: "MovieUser");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesTags",
                table: "MoviesTags");

            migrationBuilder.DropIndex(
                name: "IX_MoviesTags_MovieId",
                table: "MoviesTags");

            migrationBuilder.DropIndex(
                name: "IX_MoviesTags_TagId",
                table: "MoviesTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MoviesTags");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "MoviesTags");

            migrationBuilder.RenameTable(
                name: "MoviesTags",
                newName: "MovieTags");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Movies",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "MovieTags",
                newName: "MovieTagsId");

            migrationBuilder.AlterColumn<int>(
                name: "Vote",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "MovieTagsId",
                table: "MovieTags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "MovieTags",
                table: "MovieTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieTags",
                table: "MovieTags",
                column: "MovieTagsId");

            migrationBuilder.CreateTable(
                name: "MovieDescriptions",
                columns: table => new
                {
                    MovieDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    MovieTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDescriptions", x => x.MovieDescriptionId);
                    table.ForeignKey(
                        name: "FK_MovieDescriptions_MovieTags_MovieTagId",
                        column: x => x.MovieTagId,
                        principalTable: "MovieTags",
                        principalColumn: "MovieTagsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieDescriptions_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesImage",
                columns: table => new
                {
                    MovieImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    MovieImageExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesImage", x => x.MovieImageId);
                    table.ForeignKey(
                        name: "FK_MoviesImage_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieDescriptions_MovieId",
                table: "MovieDescriptions",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDescriptions_MovieTagId",
                table: "MovieDescriptions",
                column: "MovieTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesImage_MovieId",
                table: "MoviesImage",
                column: "MovieId");
        }
    }
}
