using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityEventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    EntityEventType = table.Column<string>(nullable: true),
                    Exchange = table.Column<string>(nullable: true),
                    Topic = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityEventLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    SubTotal = table.Column<float>(nullable: false),
                    Tax = table.Column<float>(nullable: false),
                    Total = table.Column<float>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: true),
                    Discount = table.Column<float>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreateDate", "Description", "Name", "SubTotal", "Tax", "Total", "UserId" },
                values: new object[] { new Guid("8b793810-df77-4a09-a91c-19b52c5d63aa"), new DateTime(2020, 6, 13, 22, 21, 57, 580, DateTimeKind.Local).AddTicks(4620), null, "Order 1", 10.5f, 1.8f, 12.3f, 1 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreateDate", "Description", "Name", "SubTotal", "Tax", "Total", "UserId" },
                values: new object[] { new Guid("b93518bc-27a6-40ab-a92d-ad9a0f8843a1"), new DateTime(2020, 6, 13, 22, 21, 57, 582, DateTimeKind.Local).AddTicks(2097), null, "Order 2", 10.5f, 1.8f, 12.3f, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityEventLogs");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
