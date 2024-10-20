using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class renameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorsMovie_Actors_ActorId",
                table: "ActorsMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorsMovie_Movies_MovieId",
                table: "ActorsMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorsMovie",
                table: "ActorsMovie");

            migrationBuilder.RenameTable(
                name: "ActorsMovie",
                newName: "ActorsMovies");

            migrationBuilder.RenameIndex(
                name: "IX_ActorsMovie_ActorId",
                table: "ActorsMovies",
                newName: "IX_ActorsMovies_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorsMovies",
                table: "ActorsMovies",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActorsMovies_Actors_ActorId",
                table: "ActorsMovies",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorsMovies_Movies_MovieId",
                table: "ActorsMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorsMovies_Actors_ActorId",
                table: "ActorsMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorsMovies_Movies_MovieId",
                table: "ActorsMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorsMovies",
                table: "ActorsMovies");

            migrationBuilder.RenameTable(
                name: "ActorsMovies",
                newName: "ActorsMovie");

            migrationBuilder.RenameIndex(
                name: "IX_ActorsMovies_ActorId",
                table: "ActorsMovie",
                newName: "IX_ActorsMovie_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorsMovie",
                table: "ActorsMovie",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActorsMovie_Actors_ActorId",
                table: "ActorsMovie",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorsMovie_Movies_MovieId",
                table: "ActorsMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
