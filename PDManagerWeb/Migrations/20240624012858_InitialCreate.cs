using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessLevels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    login = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    passwordHash = table.Column<byte[]>(type: "binary(32)", fixedLength: true, maxLength: 32, nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFormats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    extension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFormats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    head_id = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_Head",
                        column: x => x.head_id,
                        principalTable: "Accounts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SysAdmins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    isMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysAdmins", x => x.id);
                    table.ForeignKey(
                        name: "FK_SysAdmin_Account",
                        column: x => x.id,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowedFormats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    documentType_id = table.Column<int>(type: "int", nullable: false),
                    documentFormat_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedFormats", x => x.id);
                    table.ForeignKey(
                        name: "FK_AllowedFormat_DocumentFormat",
                        column: x => x.documentFormat_id,
                        principalTable: "DocumentFormats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllowedFormat_DocumentType",
                        column: x => x.documentType_id,
                        principalTable: "DocumentTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAccesses",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    isGranted = table.Column<bool>(type: "bit", nullable: false),
                    accessLevel_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAccesses", x => new { x.product_id, x.account_id });
                    table.ForeignKey(
                        name: "FK_ProductAccess_Account",
                        column: x => x.account_id,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAccess_Product",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAccesse_AccessLevel",
                        column: x => x.accessLevel_id,
                        principalTable: "AccessLevels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramDocuments",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    documentTypeFormat_id = table.Column<int>(type: "int", nullable: false),
                    lastChangeDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    lastChangeUser_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDocuments", x => new { x.product_id, x.documentTypeFormat_id });
                    table.ForeignKey(
                        name: "FK_ProgramDocument_ChangeUser",
                        column: x => x.lastChangeUser_id,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramDocument_Product",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramDocument_TypeFormat",
                        column: x => x.documentTypeFormat_id,
                        principalTable: "AllowedFormats",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "UK_AccessLevel_Name",
                table: "AccessLevels",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Account_Login",
                table: "Accounts",
                column: "login",
                unique: true,
                filter: "([isDeleted]=(0))");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedFormats_documentFormat_id",
                table: "AllowedFormats",
                column: "documentFormat_id");

            migrationBuilder.CreateIndex(
                name: "UK_AllowedFormat_TypeFormat",
                table: "AllowedFormats",
                columns: new[] { "documentType_id", "documentFormat_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_DocumentFormat_Extension",
                table: "DocumentFormats",
                column: "extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_DocumentType_Name",
                table: "DocumentTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAccesses_accessLevel_id",
                table: "ProductAccesses",
                column: "accessLevel_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAccesses_account_id",
                table: "ProductAccesses",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_head_id",
                table: "Products",
                column: "head_id");

            migrationBuilder.CreateIndex(
                name: "UK_Product_Name",
                table: "Products",
                column: "name",
                unique: true,
                filter: "([isDeleted]=(0))");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDocuments_documentTypeFormat_id",
                table: "ProgramDocuments",
                column: "documentTypeFormat_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDocuments_lastChangeUser_id",
                table: "ProgramDocuments",
                column: "lastChangeUser_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAccesses");

            migrationBuilder.DropTable(
                name: "ProgramDocuments");

            migrationBuilder.DropTable(
                name: "SysAdmins");

            migrationBuilder.DropTable(
                name: "AccessLevels");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AllowedFormats");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "DocumentFormats");

            migrationBuilder.DropTable(
                name: "DocumentTypes");
        }
    }
}
