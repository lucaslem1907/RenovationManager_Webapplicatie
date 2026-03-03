using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class expense20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_RenovationProjects_ProjectId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Tasks_TaskItemId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_TaskItemId",
                table: "Expenses",
                newName: "IX_Expenses_TaskItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_ProjectId",
                table: "Expenses",
                newName: "IX_Expenses_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_RenovationProjects_ProjectId",
                table: "Expenses",
                column: "ProjectId",
                principalTable: "RenovationProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Tasks_TaskItemId",
                table: "Expenses",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_RenovationProjects_ProjectId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Tasks_TaskItemId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_TaskItemId",
                table: "Expense",
                newName: "IX_Expense_TaskItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ProjectId",
                table: "Expense",
                newName: "IX_Expense_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_RenovationProjects_ProjectId",
                table: "Expense",
                column: "ProjectId",
                principalTable: "RenovationProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Tasks_TaskItemId",
                table: "Expense",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
