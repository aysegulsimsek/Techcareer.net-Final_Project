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
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
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
                columns: new[] { "Id", "Content", "ImageFile", "ImageUrl", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8038), "First Article" },
                    { 2, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8050), "Second Article" },
                    { 3, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://leverageedublog.s3.ap-south-1.amazonaws.com/blog/wp-content/uploads/2020/01/24145013/article-writing.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8051), "Third Article" },
                    { 4, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://www.staceyeburke.com/wp-content/uploads/2018/06/publications1.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8053), "Fourth Article" },
                    { 5, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8054), "Fifth Article" },
                    { 6, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", null, "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg", new DateTime(2024, 7, 23, 1, 31, 29, 915, DateTimeKind.Local).AddTicks(8055), "Sixth Article" }
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
