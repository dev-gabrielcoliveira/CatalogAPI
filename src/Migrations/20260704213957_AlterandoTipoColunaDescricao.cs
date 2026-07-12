using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoTipoColunaDescricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    Preco = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Situacao = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jogos");
        }
    }
}
