using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calcpad.web.Migrations
{
    /// <inheritdoc />
    public partial class make_IsActive_subscription_property_public : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Subscriptions");
        }
    }
}
