using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiApi.UTNGolMundial.Migrations
{
    /// <inheritdoc />
    public partial class HacerGrupoCodigoNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Grupos_GrupoCodigo",
                table: "Partidos");

            migrationBuilder.AlterColumn<char>(
                name: "GrupoCodigo",
                table: "Partidos",
                type: "character(1)",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "character(1)");

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Grupos_GrupoCodigo",
                table: "Partidos",
                column: "GrupoCodigo",
                principalTable: "Grupos",
                principalColumn: "Codigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Grupos_GrupoCodigo",
                table: "Partidos");

            migrationBuilder.AlterColumn<char>(
                name: "GrupoCodigo",
                table: "Partidos",
                type: "character(1)",
                nullable: false,
                defaultValue: '\0',
                oldClrType: typeof(char),
                oldType: "character(1)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Grupos_GrupoCodigo",
                table: "Partidos",
                column: "GrupoCodigo",
                principalTable: "Grupos",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
