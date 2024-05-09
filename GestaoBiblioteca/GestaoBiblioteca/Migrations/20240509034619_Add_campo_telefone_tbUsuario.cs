using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoBiblioteca.Migrations
{
    /// <inheritdoc />
    public partial class Add_campo_telefone_tbUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "telefone",
                table: "tbUsuario",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telefone",
                table: "tbUsuario");
        }
    }
}
