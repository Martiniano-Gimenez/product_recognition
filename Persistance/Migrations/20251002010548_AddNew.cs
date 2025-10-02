using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepositId",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deposit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositMovement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginDepositId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationDepositId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositMovement_Deposit_DestinationDepositId",
                        column: x => x.DestinationDepositId,
                        principalTable: "Deposit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositMovement_Deposit_OriginDepositId",
                        column: x => x.OriginDepositId,
                        principalTable: "Deposit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepositMovementDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    DepositMovementId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositMovementDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositMovementDetail_DepositMovement_DepositMovementId",
                        column: x => x.DepositMovementId,
                        principalTable: "DepositMovement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositMovementDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_DepositId",
                table: "User",
                column: "DepositId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositMovement_DestinationDepositId",
                table: "DepositMovement",
                column: "DestinationDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositMovement_OriginDepositId",
                table: "DepositMovement",
                column: "OriginDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositMovementDetail_DepositMovementId",
                table: "DepositMovementDetail",
                column: "DepositMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositMovementDetail_ProductId",
                table: "DepositMovementDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Deposit_DepositId",
                table: "User",
                column: "DepositId",
                principalTable: "Deposit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Deposit_DepositId",
                table: "User");

            migrationBuilder.DropTable(
                name: "DepositMovementDetail");

            migrationBuilder.DropTable(
                name: "DepositMovement");

            migrationBuilder.DropTable(
                name: "Deposit");

            migrationBuilder.DropIndex(
                name: "IX_User_DepositId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DepositId",
                table: "User");
        }
    }
}
