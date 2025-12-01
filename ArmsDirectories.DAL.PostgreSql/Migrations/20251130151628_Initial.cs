using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArmsDirectories.DAL.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "arms");

            migrationBuilder.CreateSequence<int>(
                name: "column_name_seq",
                schema: "arms");

            migrationBuilder.CreateSequence<int>(
                name: "table_name_seq",
                schema: "arms");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "arms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Surname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArmsDirectory",
                schema: "arms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TableName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'table_' || nextval('arms.table_name_seq')"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmsDirectory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmsDirectory_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "arms",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmsDirectory_User_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "arms",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmsAttribute",
                schema: "arms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    directoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SystemName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ColumnName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'column_' || nextval('arms.column_name_seq')"),
                    DataType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MaxLength = table.Column<int>(type: "integer", nullable: false),
                    Precision = table.Column<int>(type: "integer", nullable: false),
                    Scale = table.Column<int>(type: "integer", nullable: false),
                    IsUnique = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultValue = table.Column<string>(type: "text", nullable: false),
                    ReferenceDirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmsAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmsAttribute_ArmsDirectory_ReferenceDirectoryId",
                        column: x => x.ReferenceDirectoryId,
                        principalSchema: "arms",
                        principalTable: "ArmsDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmsAttribute_ArmsDirectory_directoryId",
                        column: x => x.directoryId,
                        principalSchema: "arms",
                        principalTable: "ArmsDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmsAttribute_directoryId",
                schema: "arms",
                table: "ArmsAttribute",
                column: "directoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmsAttribute_ReferenceDirectoryId",
                schema: "arms",
                table: "ArmsAttribute",
                column: "ReferenceDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmsDirectory_CreatedById",
                schema: "arms",
                table: "ArmsDirectory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ArmsDirectory_UpdatedById",
                schema: "arms",
                table: "ArmsDirectory",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmsAttribute",
                schema: "arms");

            migrationBuilder.DropTable(
                name: "ArmsDirectory",
                schema: "arms");

            migrationBuilder.DropTable(
                name: "User",
                schema: "arms");

            migrationBuilder.DropSequence(
                name: "column_name_seq",
                schema: "arms");

            migrationBuilder.DropSequence(
                name: "table_name_seq",
                schema: "arms");
        }
    }
}
