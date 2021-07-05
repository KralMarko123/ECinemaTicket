using Microsoft.EntityFrameworkCore.Migrations;

namespace ECinemaTicket.Repository.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInCarts",
                table: "TicketInCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInCarts",
                table: "TicketInCarts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInOrders_TicketId",
                table: "TicketInOrders",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInCarts_TicketId",
                table: "TicketInCarts",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders");

            migrationBuilder.DropIndex(
                name: "IX_TicketInOrders_TicketId",
                table: "TicketInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInCarts",
                table: "TicketInCarts");

            migrationBuilder.DropIndex(
                name: "IX_TicketInCarts_TicketId",
                table: "TicketInCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders",
                columns: new[] { "TicketId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInCarts",
                table: "TicketInCarts",
                columns: new[] { "TicketId", "CartId" });
        }
    }
}
