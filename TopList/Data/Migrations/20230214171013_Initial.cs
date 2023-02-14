using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopList.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ThumbnailImageId",
                table: "Catalog_Category",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Catalog_Category",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

     
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
     

            migrationBuilder.AlterColumn<long>(
                name: "ThumbnailImageId",
                table: "Catalog_Category",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "Catalog_Category",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
