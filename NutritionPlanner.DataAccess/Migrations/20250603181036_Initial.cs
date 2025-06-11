using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NutritionPlanner.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Coefficient = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalCalories = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalProtein = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalFat = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCarbohydrates = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    Calories = table.Column<decimal>(type: "numeric", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric", nullable: false),
                    Fat = table.Column<decimal>(type: "numeric", nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "numeric", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoalTypeId = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<decimal>(type: "numeric", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric", nullable: false),
                    Fat = table.Column<decimal>(type: "numeric", nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGoals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true),
                    CaloriesConsumed = table.Column<decimal>(type: "numeric", nullable: false),
                    ProteinConsumed = table.Column<decimal>(type: "numeric", nullable: false),
                    FatConsumed = table.Column<decimal>(type: "numeric", nullable: false),
                    CarbohydratesConsumed = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterConsumed = table.Column<decimal>(type: "numeric", nullable: true),
                    ActivityMinutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    ActivityLevelId = table.Column<int>(type: "integer", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GoalTypeId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<string>(type: "text", nullable: false),
                    MealTimeId = table.Column<int>(type: "integer", nullable: false),
                    RecipeId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlanItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MealPlanId = table.Column<int>(type: "integer", nullable: false),
                    MealTimeId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    RecipeId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    MealPlanEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlanItems_MealPlans_MealPlanEntityId",
                        column: x => x.MealPlanEntityId,
                        principalTable: "MealPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ActivityLevels",
                columns: new[] { "Id", "Coefficient", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1.2, "Офисная работа, минимальная физическая активность.", "Сидячий" },
                    { 2, 1.375, "Легкая физическая нагрузка или занятия спортом 1-3 раза в неделю.", "Маленькая активность" },
                    { 3, 1.55, "Занятия спортом 3-5 раз в неделю.", "Умеренная активность" },
                    { 4, 1.7250000000000001, "Интенсивные занятия спортом 6-7 раз в неделю.", "Высокая активность" },
                    { 5, 1.8999999999999999, "Ежедневные интенсивные тренировки или физическая работа.", "Экстремальная активность" }
                });

            migrationBuilder.InsertData(
                table: "GoalTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Снижение веса за счет дефицита калорий.", "Похудение" },
                    { 2, "Увеличение мышечной массы за счет профицита калорий.", "Набор массы" },
                    { 3, "Стабилизация текущего веса.", "Поддержание веса" }
                });

            migrationBuilder.InsertData(
                table: "MealTimes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Первый прием пищи в начале дня.", "Завтрак" },
                    { 2, "Основной прием пищи днем.", "Обед" },
                    { 3, "Прием пищи в конце дня.", "Ужин" },
                    { 4, "Легкий промежуточный прием пищи.", "Перекус" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "Calories", "Carbohydrates", "CreatedByUserId", "Fat", "IsApproved", "Name", "Protein", "Weight" },
                values: new object[,]
                {
                    { 1, null, 77m, 0.3m, null, 5.6m, true, "Яйца", 6.2m, 100m },
                    { 2, null, 160m, 8.5m, null, 14.7m, true, "Авокадо", 2.0m, 100m },
                    { 3, null, 70m, 13.0m, null, 0.5m, true, "Хлеб цельнозерновой", 3.0m, 100m },
                    { 4, null, 579m, 21.6m, null, 49.9m, true, "Миндаль", 21.2m, 100m },
                    { 5, null, 52m, 14.0m, null, 0.2m, true, "Яблоко", 0.3m, 100m },
                    { 6, null, 165m, 0.0m, null, 3.6m, true, "Грудка куриная", 31.0m, 100m },
                    { 7, null, 35m, 6.6m, null, 0.4m, true, "Брокколи", 3.7m, 100m },
                    { 8, null, 654m, 13.7m, null, 65.2m, true, "Грецкие орехи", 15.2m, 100m },
                    { 9, null, 89m, 22.8m, null, 0.3m, true, "Банан", 1.1m, 100m },
                    { 10, null, 190m, 9.0m, null, 2.0m, true, "Протеиновый коктейль", 30.0m, 100m },
                    { 11, null, 96m, 3.0m, null, 5.0m, true, "Творог 5%", 10.0m, 100m },
                    { 12, null, 304m, 82.4m, null, 0.0m, true, "Мед", 0.3m, 100m },
                    { 13, null, 50m, 12.0m, null, 0.3m, true, "Ягоды", 1.0m, 100m },
                    { 14, null, 182m, 0.0m, null, 12.0m, true, "Лосось", 20.0m, 100m },
                    { 15, null, 80m, 17.0m, null, 0.1m, true, "Картофель запеченный", 2.0m, 100m },
                    { 16, null, 389m, 66.3m, null, 6.9m, true, "Овсянка", 16.9m, 100m },
                    { 17, null, 50m, 4.8m, null, 1.5m, true, "Молоко 1.5%", 3.3m, 100m },
                    { 18, null, 41m, 9.0m, null, 0.4m, true, "Киви", 0.9m, 100m },
                    { 19, null, 120m, 21.3m, null, 1.9m, true, "Киноа", 4.0m, 100m },
                    { 20, null, 158m, 0.0m, null, 5.0m, true, "Индейка", 29.0m, 100m },
                    { 21, null, 16m, 3.1m, null, 0.1m, true, "Кабачки", 1.2m, 100m },
                    { 22, null, 150m, 0.0m, null, 7.0m, true, "Говядина тушеная", 20.0m, 100m },
                    { 23, null, 130m, 28.2m, null, 0.3m, true, "Рис белый", 2.7m, 100m },
                    { 24, null, 343m, 57.5m, null, 3.3m, true, "Гречка", 13.3m, 100m },
                    { 25, null, 342m, 74.0m, null, 1.5m, true, "Булгур", 12.0m, 100m },
                    { 26, null, 82m, 0.0m, null, 0.7m, true, "Треска", 18.0m, 100m },
                    { 27, null, 23m, 3.6m, null, 0.4m, true, "Шпинат", 2.9m, 100m },
                    { 28, null, 57m, 15.0m, null, 0.1m, true, "Груша", 0.4m, 100m },
                    { 29, null, 553m, 30.2m, null, 43.8m, true, "Кешью", 18.2m, 100m },
                    { 30, null, 111m, 23.0m, null, 0.9m, true, "Рис бурый", 2.6m, 100m },
                    { 31, null, 90m, 0.0m, null, 10.0m, true, "Оливковое масло", 0.0m, 100m },
                    { 32, null, 150m, 3.0m, null, 9.0m, true, "Рикотта", 11.0m, 100m },
                    { 33, null, 60m, 4.0m, null, 1.0m, true, "Греческий йогурт", 10.0m, 100m },
                    { 34, null, 150m, 12.0m, null, 8.0m, true, "Хумус", 6.0m, 100m },
                    { 35, null, 280m, 0.0m, null, 18.0m, true, "Стейк говяжий", 26.0m, 100m },
                    { 36, null, 150m, 0.0m, null, 1.0m, true, "Тунец консервированный", 26.0m, 100m },
                    { 37, null, 22m, 4.0m, null, 0m, true, "Огурец", 1.0m, 100m },
                    { 38, null, 20m, 4.0m, null, 0m, true, "Помидоры черри", 1.0m, 100m },
                    { 39, null, 47m, 11.8m, null, 0.1m, true, "Апельсин", 0.9m, 100m },
                    { 40, null, 41m, 9.6m, null, 0.2m, true, "Морковь", 0.9m, 100m },
                    { 41, null, 579m, 21.6m, null, 49.9m, true, "Миндаль", 21.2m, 100m },
                    { 42, null, 31m, 7.0m, null, 0.1m, true, "Зеленая фасоль", 2.4m, 100m },
                    { 43, null, 25m, 6.0m, null, 0.0m, true, "Черника", 0.0m, 100m },
                    { 44, null, 82m, 0.0m, null, 0.7m, true, "Филе трески", 18.0m, 100m },
                    { 45, null, 27m, 3.3m, null, 0.3m, true, "Шампиньоны", 3.1m, 100m },
                    { 46, null, 70m, 13.0m, null, 0.5m, true, "Хлеб цельнозерновой", 3.0m, 100m },
                    { 47, null, 20m, 4.0m, null, 0.0m, true, "Помидор", 1.0m, 100m },
                    { 48, null, 60m, 15.0m, null, 0.0m, true, "Груша", 0.0m, 100m },
                    { 49, null, 30m, 6.0m, null, 0.0m, true, "Кабачки", 2.0m, 100m },
                    { 50, null, 90m, 4.0m, null, 3.0m, true, "Йогурт без сахара", 10.0m, 100m },
                    { 51, null, 140m, 30.0m, null, 2.0m, true, "Хлопья цельнозерновые", 4.0m, 100m },
                    { 52, "4607001417255", 420m, 55.0m, null, 20.0m, true, "Маффины", 6.0m, 100m }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Питательный завтрак с яйцами и авокадо", "Омлет с авокадо" },
                    { 2, "Легкий и полезный обед", "Салат с курицей и овощами" },
                    { 3, "Вегетарианский ужин", "Гречневая каша с овощами" },
                    { 4, "Освежающий перекус", "Фруктовый смузи" },
                    { 5, "Полезный ужин с рыбой", "Запеченный лосось с картофелем" }
                });

            migrationBuilder.InsertData(
                table: "WeeklyMenus",
                columns: new[] { "Id", "Amount", "DayOfWeek", "GoalTypeId", "MealTimeId", "ProductId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 2m, "Понедельник", 1, 1, 1, null },
                    { 2, 0.5m, "Понедельник", 1, 1, 2, null },
                    { 3, 0.3m, "Понедельник", 1, 1, 46, null },
                    { 4, 2m, "Понедельник", 1, 2, 6, null },
                    { 5, 2m, "Понедельник", 1, 2, 7, null },
                    { 6, 0.1m, "Понедельник", 1, 2, 31, null },
                    { 7, 1.5m, "Понедельник", 1, 3, 14, null },
                    { 8, 1.5m, "Понедельник", 1, 3, 27, null },
                    { 9, 0.2m, "Понедельник", 1, 4, 4, null },
                    { 10, 1m, "Понедельник", 1, 4, 33, null },
                    { 11, 0.1m, "Понедельник", 1, 4, 12, null },
                    { 12, 1.5m, "Понедельник", 1, 4, 5, null },
                    { 13, 0.4m, "Вторник", 1, 1, 16, null },
                    { 14, 1m, "Вторник", 1, 1, 5, null },
                    { 15, 0.1m, "Вторник", 1, 1, 8, null },
                    { 16, 2m, "Вторник", 1, 2, 20, null },
                    { 17, 0.1m, "Вторник", 1, 2, 31, null },
                    { 18, 2m, "Вторник", 1, 2, 21, null },
                    { 19, 1.5m, "Вторник", 1, 3, 36, null },
                    { 20, 1m, "Вторник", 1, 3, 37, null },
                    { 21, 0.3m, "Вторник", 1, 3, 46, null },
                    { 22, 1m, "Вторник", 1, 4, 32, null },
                    { 23, 1m, "Вторник", 1, 4, 13, null },
                    { 24, 1.5m, "Вторник", 1, 4, 17, null },
                    { 25, 1m, "Вторник", 1, 4, 28, null },
                    { 26, 2m, "Среда", 1, 1, 1, null },
                    { 27, 1m, "Среда", 1, 1, 38, null },
                    { 28, 0.3m, "Среда", 1, 1, 46, null },
                    { 29, 0.15m, "Среда", 1, 2, 22, null },
                    { 30, 2m, "Среда", 1, 2, 42, null },
                    { 31, 0.1m, "Среда", 1, 2, 31, null },
                    { 32, 1.5m, "Среда", 1, 3, 11, null },
                    { 33, 1m, "Среда", 1, 3, 27, null },
                    { 34, 0.4m, "Среда", 1, 3, 12, null },
                    { 35, 0.5m, "Среда", 1, 4, 34, null },
                    { 36, 1.5m, "Среда", 1, 4, 39, null },
                    { 37, 0.15m, "Среда", 1, 4, 41, null },
                    { 38, 0.4m, "Четверг", 1, 1, 16, null },
                    { 39, 0.5m, "Четверг", 1, 1, 43, null },
                    { 40, 0.15m, "Четверг", 1, 1, 8, null },
                    { 41, 2.0m, "Четверг", 1, 2, 44, null },
                    { 42, 0.5m, "Четверг", 1, 2, 19, null },
                    { 43, 1.5m, "Четверг", 1, 2, 7, null },
                    { 44, 1.5m, "Четверг", 1, 3, 20, null },
                    { 45, 1.5m, "Четверг", 1, 3, 45, null },
                    { 46, 1.0m, "Четверг", 1, 4, 11, null },
                    { 47, 1.0m, "Четверг", 1, 4, 33, null },
                    { 48, 1.0m, "Четверг", 1, 4, 9, null },
                    { 49, 0.1m, "Четверг", 1, 4, 12, null },
                    { 50, 2m, "Пятница", 1, 1, 1, null },
                    { 51, 0.01m, "Пятница", 1, 1, 17, null },
                    { 52, 0.3m, "Пятница", 1, 1, 46, null },
                    { 53, 1.0m, "Пятница", 1, 1, 47, null },
                    { 54, 2.0m, "Пятница", 1, 2, 6, null },
                    { 55, 1.5m, "Пятница", 1, 2, 49, null },
                    { 56, 0.1m, "Пятница", 1, 2, 31, null },
                    { 57, 1.5m, "Пятница", 1, 3, 14, null },
                    { 58, 1.0m, "Пятница", 1, 3, 27, null },
                    { 59, 0.15m, "Пятница", 1, 4, 4, null },
                    { 60, 1.5m, "Пятница", 1, 4, 28, null },
                    { 61, 1.5m, "Пятница", 1, 4, 50, null },
                    { 62, 0.1m, "Пятница", 1, 4, 12, null },
                    { 63, 0.4m, "Суббота", 1, 1, 19, null },
                    { 64, 1m, "Суббота", 1, 1, 39, null },
                    { 65, 0.3m, "Суббота", 1, 1, 46, null },
                    { 66, 2m, "Суббота", 1, 2, 6, null },
                    { 67, 2m, "Суббота", 1, 2, 7, null },
                    { 68, 0.1m, "Суббота", 1, 2, 31, null },
                    { 69, 1.5m, "Суббота", 1, 3, 10, null },
                    { 70, 0.5m, "Суббота", 1, 3, 13, null },
                    { 71, 0.2m, "Суббота", 1, 4, 4, null },
                    { 72, 1.5m, "Суббота", 1, 4, 5, null },
                    { 73, 1m, "Суббота", 1, 4, 33, null },
                    { 74, 0.5m, "Воскресенье", 1, 1, 15, null },
                    { 75, 0.5m, "Воскресенье", 1, 1, 2, null },
                    { 76, 1m, "Воскресенье", 1, 1, 47, null },
                    { 77, 0.15m, "Воскресенье", 1, 2, 22, null },
                    { 78, 2m, "Воскресенье", 1, 2, 42, null },
                    { 79, 0.1m, "Воскресенье", 1, 2, 31, null },
                    { 80, 1.5m, "Воскресенье", 1, 3, 14, null },
                    { 81, 1.5m, "Воскресенье", 1, 3, 45, null },
                    { 82, 0.1m, "Воскресенье", 1, 4, 12, null },
                    { 83, 1.5m, "Воскресенье", 1, 4, 17, null },
                    { 84, 1m, "Воскресенье", 1, 4, 28, null }
                });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "Id", "Amount", "ProductId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 100m, 1, 1 },
                    { 2, 50m, 2, 1 },
                    { 3, 10m, 31, 1 },
                    { 4, 150m, 6, 2 },
                    { 5, 100m, 7, 2 },
                    { 6, 50m, 37, 2 },
                    { 7, 70m, 38, 2 },
                    { 8, 15m, 31, 2 },
                    { 9, 100m, 24, 3 },
                    { 10, 100m, 21, 3 },
                    { 11, 50m, 40, 3 },
                    { 12, 30m, 27, 3 },
                    { 13, 100m, 9, 4 },
                    { 14, 50m, 13, 4 },
                    { 15, 100m, 33, 4 },
                    { 16, 50m, 17, 4 },
                    { 17, 200m, 14, 5 },
                    { 18, 150m, 15, 5 },
                    { 19, 50m, 27, 5 },
                    { 20, 15m, 31, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverId",
                table: "ChatMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanItems_MealPlanEntityId",
                table: "MealPlanItems",
                column: "MealPlanEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_ProductId",
                table: "RecipeIngredients",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLevels");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "GoalTypes");

            migrationBuilder.DropTable(
                name: "MealPlanItems");

            migrationBuilder.DropTable(
                name: "MealTimes");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "UserGoals");

            migrationBuilder.DropTable(
                name: "UserProgress");

            migrationBuilder.DropTable(
                name: "WeeklyMenus");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
