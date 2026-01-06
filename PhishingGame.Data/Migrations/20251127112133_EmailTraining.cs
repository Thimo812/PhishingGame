using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhishingGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingEmails",
                columns: table => new
                {
                    EmailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingEmails", x => new { x.EmailsId, x.TrainingsId });
                    table.ForeignKey(
                        name: "FK_TrainingEmails_Emails_EmailsId",
                        column: x => x.EmailsId,
                        principalTable: "Emails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingEmails_Trainings_TrainingsId",
                        column: x => x.TrainingsId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingEmails_TrainingsId",
                table: "TrainingEmails",
                column: "TrainingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingEmails");
        }
    }
}
