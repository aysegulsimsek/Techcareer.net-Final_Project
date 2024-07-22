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
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CommentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsAdmin", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, "user1@example.com", false, null, "User1" },
                    { 2, "user2@example.com", false, null, "User2" },
                    { 3, "user3@example.com", false, null, "User3" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "ImageUrl", "PublishedDate", "Title", "UserId", "UserId1" },
                values: new object[,]
                {
                    { 1, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8859), "First Article", 1, null },
                    { 2, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8870), "Second Article", 1, null },
                    { 3, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://leverageedublog.s3.ap-south-1.amazonaws.com/blog/wp-content/uploads/2020/01/24145013/article-writing.jpg", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8872), "Third Article", 2, null },
                    { 4, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://th.bing.com/th/id/OIP.WwajPSJu5YunOsdTaD0hqwHaE7?rs=1&pid=ImgDetMain", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8873), "Fourth Article", 2, null },
                    { 5, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8875), "Fifth Article", 3, null },
                    { 6, "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg", new DateTime(2024, 7, 22, 21, 18, 12, 930, DateTimeKind.Local).AddTicks(8876), "Sixth Article", 3, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId1",
                table: "Articles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
