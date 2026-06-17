using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revenue_Recognition_System.Migrations
{
    /// <inheritdoc />
    public partial class paymentsDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Clients_ClientId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Contracts_ContractId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ContractId",
                table: "Payments",
                newName: "IX_Payments_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ClientId",
                table: "Payments",
                newName: "IX_Payments_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ContractId",
                table: "Payment",
                newName: "IX_Payment_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ClientId",
                table: "Payment",
                newName: "IX_Payment_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Clients_ClientId",
                table: "Payment",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Contracts_ContractId",
                table: "Payment",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
