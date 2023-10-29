using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAModels.Migrations
{
    /// <inheritdoc />
    public partial class fixMovieImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "MovieImage",
                table: "Movies",
                type: "varbinary(4000)",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieImage",
                table: "Movies");
        }
    }
}
