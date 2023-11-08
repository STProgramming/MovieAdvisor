using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAModels.Migrations
{
    /// <inheritdoc />
    public partial class tabImageMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieImage",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "MoviesImage",
                columns: table => new
                {
                    MovieImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieImageExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_MoviesImage_MovieId",
                table: "MoviesImage",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesImage");

            migrationBuilder.AddColumn<byte[]>(
                name: "MovieImage",
                table: "Movies",
                type: "varbinary(4000)",
                maxLength: 4000,
                nullable: true);
        }
    }
}
