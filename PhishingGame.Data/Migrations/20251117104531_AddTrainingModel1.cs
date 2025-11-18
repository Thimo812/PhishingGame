using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhishingGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingModel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainingId",
                table: "Emails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emails_TrainingId",
                table: "Emails",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Trainings_TrainingId",
                table: "Emails",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Trainings_TrainingId",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_TrainingId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Emails");
        }
    }
}
