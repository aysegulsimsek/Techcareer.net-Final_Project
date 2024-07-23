using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DBlog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    ImageFile = table.Column<string>(type: "TEXT", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CommentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "ImageFile", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/1.jpg", new DateTime(2024, 7, 13, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2798), "First Article" },
                    { 2, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/6.jpg", new DateTime(2024, 6, 3, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2812), "Second Article" },
                    { 3, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/4.jpg", new DateTime(2024, 5, 4, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2813), "Third Article" },
                    { 4, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/7.jpg", new DateTime(2024, 6, 3, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2814), "Fourth Article" },
                    { 5, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/5.jpg", new DateTime(2024, 6, 23, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2816), "Fifth Article" },
                    { 6, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "/img/6.jpg", new DateTime(2024, 7, 3, 21, 53, 45, 681, DateTimeKind.Local).AddTicks(2817), "Sixth Article" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
