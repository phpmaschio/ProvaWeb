using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProcessosAPI.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimaAltercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusProcessos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UltimaAlteracao = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusProcessos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UltimaAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processos_StatusProcessos_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusProcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Andamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProcessoId = table.Column<long>(type: "bigint", nullable: false),
                    AtribuidoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Andamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Andamentos_Processos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartesProcessos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParteId = table.Column<long>(type: "bigint", nullable: false),
                    ProcessoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartesProcessos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartesProcessos_Partes_ParteId",
                        column: x => x.ParteId,
                        principalTable: "Partes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartesProcessos_Processos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Andamentos_ProcessoId",
                table: "Andamentos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_PartesProcessos_ParteId",
                table: "PartesProcessos",
                column: "ParteId");

            migrationBuilder.CreateIndex(
                name: "IX_PartesProcessos_ProcessoId",
                table: "PartesProcessos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processos_StatusId",
                table: "Processos",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Andamentos");

            migrationBuilder.DropTable(
                name: "PartesProcessos");

            migrationBuilder.DropTable(
                name: "Partes");

            migrationBuilder.DropTable(
                name: "Processos");

            migrationBuilder.DropTable(
                name: "StatusProcessos");
        }
    }
}
