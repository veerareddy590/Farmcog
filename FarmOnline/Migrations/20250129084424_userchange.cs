using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmOnline.Migrations
{
    /// <inheritdoc />
    public partial class userchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_address_user_UserId",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_carts_user_CustomerId",
                table: "carts");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHeader_user_UserId",
                table: "orderHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_productFarmers_user_FarmerId",
                table: "productFarmers");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_address_AspNetUsers_UserId",
                table: "address",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_carts_AspNetUsers_CustomerId",
                table: "carts",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeader_AspNetUsers_UserId",
                table: "orderHeader",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productFarmers_AspNetUsers_FarmerId",
                table: "productFarmers",
                column: "FarmerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_address_AspNetUsers_UserId",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_carts_AspNetUsers_CustomerId",
                table: "carts");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHeader_AspNetUsers_UserId",
                table: "orderHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_productFarmers_AspNetUsers_FarmerId",
                table: "productFarmers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_address_user_UserId",
                table: "address",
                column: "UserId",
                principalTable: "user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_carts_user_CustomerId",
                table: "carts",
                column: "CustomerId",
                principalTable: "user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeader_user_UserId",
                table: "orderHeader",
                column: "UserId",
                principalTable: "user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productFarmers_user_FarmerId",
                table: "productFarmers",
                column: "FarmerId",
                principalTable: "user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
