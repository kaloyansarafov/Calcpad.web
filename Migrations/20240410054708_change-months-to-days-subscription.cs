using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calcpad.web.Migrations
{
    /// <inheritdoc />
    public partial class changemonthstodayssubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Months",
                table: "SubscriptionPlans",
                newName: "Days");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Days",
                table: "SubscriptionPlans",
                newName: "Months");
        }
    }
}
