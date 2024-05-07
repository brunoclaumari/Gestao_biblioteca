using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoBiblioteca.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbItensEmprestimo_tbEmprestimo",
                table: "tbItensEmprestimo");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "tbUsuario",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AddForeignKey(
                name: "FK_tbItensEmprestimo_tbEmprestimo",
                table: "tbItensEmprestimo",
                column: "emprestimo_id",
                principalTable: "tbEmprestimo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbItensEmprestimo_tbEmprestimo",
                table: "tbItensEmprestimo");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "tbUsuario",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_tbItensEmprestimo_tbEmprestimo",
                table: "tbItensEmprestimo",
                column: "emprestimo_id",
                principalTable: "tbEmprestimo",
                principalColumn: "id");
        }
    }
}
