using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Tasks_TaskItemId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TaskItemId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TaskItemId",
                table: "Expenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskItemId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TaskItemId",
                table: "Expenses",
                column: "TaskItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Tasks_TaskItemId",
                table: "Expenses",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
