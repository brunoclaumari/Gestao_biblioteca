using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoBiblioteca.Migrations
{
    /// <inheritdoc />
    public partial class Inicia_Tabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbLivro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    autores = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    genero = table.Column<int>(type: "int", nullable: true),
                    quantidade_total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbLivro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbUsuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    endereco = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    data_registro = table.Column<DateTime>(type: "datetime", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    possui_pendencias = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUsuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbEmprestimo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    data_emprestimo = table.Column<DateTime>(type: "datetime", nullable: false),
                    data_devolucao = table.Column<DateTime>(type: "datetime", nullable: false),
                    status_emprestimo = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEmprestimo", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbEmprestimo_tbUsuario",
                        column: x => x.usuario_id,
                        principalTable: "tbUsuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbItensEmprestimo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    livro_id = table.Column<int>(type: "int", nullable: false),
                    emprestimo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbItensEmprestimo", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbItensEmprestimo_tbEmprestimo",
                        column: x => x.emprestimo_id,
                        principalTable: "tbEmprestimo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbItensEmprestimo_tbLivro",
                        column: x => x.livro_id,
                        principalTable: "tbLivro",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbEmprestimo_usuario_id",
                table: "tbEmprestimo",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbItensEmprestimo_emprestimo_id",
                table: "tbItensEmprestimo",
                column: "emprestimo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbItensEmprestimo_livro_id",
                table: "tbItensEmprestimo",
                column: "livro_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbItensEmprestimo");

            migrationBuilder.DropTable(
                name: "tbEmprestimo");

            migrationBuilder.DropTable(
                name: "tbLivro");

            migrationBuilder.DropTable(
                name: "tbUsuario");
        }
    }
}
