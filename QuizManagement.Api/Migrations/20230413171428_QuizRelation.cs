using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuizRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "QuizId", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 4, 13, 17, 14, 28, 154, DateTimeKind.Utc).AddTicks(9499), "$2a$11$dWojIcv/Q8wWTmrJbGYQcespf01oMsA6g6gisyTEUjcSIK/u9sTSy", null, new DateTime(2023, 4, 13, 17, 14, 28, 154, DateTimeKind.Utc).AddTicks(9502) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_QuizId",
                table: "Users",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Quizzes_QuizId",
                table: "Users",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Quizzes_QuizId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_QuizId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 4, 13, 17, 13, 32, 988, DateTimeKind.Utc).AddTicks(8014), "$2a$11$AliPySsMIP7aw7rn9gXz6usw2R.Tliu6LGRq26hh5vIrdUZ.r83EO", new DateTime(2023, 4, 13, 17, 13, 32, 988, DateTimeKind.Utc).AddTicks(8019) });
        }
    }
}
