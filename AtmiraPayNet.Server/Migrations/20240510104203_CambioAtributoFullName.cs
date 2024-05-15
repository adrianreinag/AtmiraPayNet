using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtmiraPayNet.Server.Migrations
{
    /// <inheritdoc />
    public partial class CambioAtributoFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LetterPDF",
                table: "PaymentLetters",
                newName: "PDF");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentLetterId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentLetterId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PDF",
                table: "PaymentLetters",
                newName: "LetterPDF");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Fullname");
        }
    }
}
