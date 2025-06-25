using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess
{
    public class NutritionPlannerDbContext : DbContext
    {
        public NutritionPlannerDbContext(DbContextOptions<NutritionPlannerDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ActivityLevelEntity> ActivityLevels { get; set; }
        public DbSet<GoalTypeEntity> GoalTypes { get; set; }
        public DbSet<UserGoalEntity> UserGoals { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<RecipeEntity> Recipes { get; set; }
        public DbSet<RecipeIngredientEntity> RecipeIngredients { get; set; }
        public DbSet<MealTimeEntity> MealTimes { get; set; }
        public DbSet<MealPlanEntity> MealPlans { get; set; }
        public DbSet<MealPlanItemEntity> MealPlanItems { get; set; }
        public DbSet<UserProgressEntity> UserProgress { get; set; }
        public DbSet<WeeklyMenuEntity> WeeklyMenus { get; set; }
        public DbSet<ChatMessageEntity> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMessageEntity>()
               .HasOne(cm => cm.Sender)
               .WithMany()
               .HasForeignKey(cm => cm.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessageEntity>()
                .HasOne(cm => cm.Receiver)
                .WithMany()
                .HasForeignKey(cm => cm.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity Levels
            modelBuilder.Entity<ActivityLevelEntity>().HasData(
                 new ActivityLevelEntity { Id = 1, Name = "Сидячий", Description = "Офисная работа, минимальная физическая активность.", Coefficient = 1.2 },
                 new ActivityLevelEntity { Id = 2, Name = "Маленькая активность", Description = "Легкая физическая нагрузка или занятия спортом 1-3 раза в неделю.", Coefficient = 1.375 },
                 new ActivityLevelEntity { Id = 3, Name = "Умеренная активность", Description = "Занятия спортом 3-5 раз в неделю.", Coefficient = 1.55 },
                 new ActivityLevelEntity { Id = 4, Name = "Высокая активность", Description = "Интенсивные занятия спортом 6-7 раз в неделю.", Coefficient = 1.725 },
                 new ActivityLevelEntity { Id = 5, Name = "Экстремальная активность", Description = "Ежедневные интенсивные тренировки или физическая работа.", Coefficient = 1.9 }
             );

            // Goal Types
            modelBuilder.Entity<GoalTypeEntity>().HasData(
                new GoalTypeEntity { Id = 1, Name = "Похудение", Description = "Снижение веса за счет дефицита калорий." },
                new GoalTypeEntity { Id = 2, Name = "Набор массы", Description = "Увеличение мышечной массы за счет профицита калорий." },
                new GoalTypeEntity { Id = 3, Name = "Поддержание веса", Description = "Стабилизация текущего веса." }
            );

            // Meal Times
            modelBuilder.Entity<MealTimeEntity>().HasData(
                new MealTimeEntity { Id = 1, Name = "Завтрак", Description = "Первый прием пищи в начале дня." },
                new MealTimeEntity { Id = 2, Name = "Обед", Description = "Основной прием пищи днем." },
                new MealTimeEntity { Id = 3, Name = "Ужин", Description = "Прием пищи в конце дня." },
                new MealTimeEntity { Id = 4, Name = "Перекус", Description = "Легкий промежуточный прием пищи." }
            );

            modelBuilder.Entity<ProductEntity>().HasData(
                 new ProductEntity { Id = 1, Name = "Яйцо куринное", Calories = 77, Protein = 6.2m, Fat = 5.6m, Carbohydrates = 0.3m },
                 new ProductEntity { Id = 2, Name = "Авокадо", Calories = 160, Protein = 2.0m, Fat = 14.7m, Carbohydrates = 8.5m },
                 new ProductEntity { Id = 3, Name = "Хлеб цельнозерновой", Calories = 70, Protein = 3.0m, Fat = 0.5m, Carbohydrates = 13.0m },
                 new ProductEntity { Id = 4, Name = "Миндаль", Calories = 579, Protein = 21.2m, Fat = 49.9m, Carbohydrates = 21.6m },
                 new ProductEntity { Id = 5, Name = "Яблоко", Calories = 52, Protein = 0.3m, Fat = 0.2m, Carbohydrates = 14.0m },
                 new ProductEntity { Id = 6, Name = "Грудка куриная", Calories = 165, Protein = 31.0m, Fat = 3.6m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 7, Name = "Брокколи", Calories = 35, Protein = 3.7m, Fat = 0.4m, Carbohydrates = 6.6m },
                 new ProductEntity { Id = 8, Name = "Грецкие орехи", Calories = 654, Protein = 15.2m, Fat = 65.2m, Carbohydrates = 13.7m },
                 new ProductEntity { Id = 9, Name = "Банан", Calories = 89, Protein = 1.1m, Fat = 0.3m, Carbohydrates = 22.8m },
                 new ProductEntity { Id = 10, Name = "Протеиновый коктейль", Calories = 190, Protein = 30.0m, Fat = 2.0m, Carbohydrates = 9.0m },
                 new ProductEntity { Id = 11, Name = "Творог 5%", Calories = 96, Protein = 10.0m, Fat = 5.0m, Carbohydrates = 3.0m },
                 new ProductEntity { Id = 12, Name = "Мед", Calories = 304, Protein = 0.3m, Fat = 0.0m, Carbohydrates = 82.4m },
                 new ProductEntity { Id = 13, Name = "Ягоды", Calories = 50, Protein = 1.0m, Fat = 0.3m, Carbohydrates = 12.0m },
                 new ProductEntity { Id = 14, Name = "Лосось", Calories = 182, Protein = 20.0m, Fat = 12.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 15, Name = "Картофель запеченный", Calories = 80, Protein = 2.0m, Fat = 0.1m, Carbohydrates = 17.0m },
                 new ProductEntity { Id = 16, Name = "Овсянка", Calories = 389, Protein = 16.9m, Fat = 6.9m, Carbohydrates = 66.3m },
                 new ProductEntity { Id = 17, Name = "Молоко 1.5%", Calories = 50, Protein = 3.3m, Fat = 1.5m, Carbohydrates = 4.8m },
                 new ProductEntity { Id = 18, Name = "Киви", Calories = 41, Protein = 0.9m, Fat = 0.4m, Carbohydrates = 9.0m },
                 new ProductEntity { Id = 19, Name = "Киноа", Calories = 120, Protein = 4.0m, Fat = 1.9m, Carbohydrates = 21.3m },
                 new ProductEntity { Id = 20, Name = "Индейка", Calories = 158, Protein = 29.0m, Fat = 5.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 21, Name = "Кабачки", Calories = 16, Protein = 1.2m, Fat = 0.1m, Carbohydrates = 3.1m },
                 new ProductEntity { Id = 22, Name = "Говядина тушеная", Calories = 150, Protein = 20.0m, Fat = 7.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 23, Name = "Рис белый", Calories = 130, Protein = 2.7m, Fat = 0.3m, Carbohydrates = 28.2m },
                 new ProductEntity { Id = 24, Name = "Гречка", Calories = 343, Protein = 13.3m, Fat = 3.3m, Carbohydrates = 57.5m },
                 new ProductEntity { Id = 25, Name = "Булгур", Calories = 342, Protein = 12.0m, Fat = 1.5m, Carbohydrates = 74.0m },
                 new ProductEntity { Id = 26, Name = "Треска", Calories = 82, Protein = 18.0m, Fat = 0.7m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 27, Name = "Шпинат", Calories = 23, Protein = 2.9m, Fat = 0.4m, Carbohydrates = 3.6m },
                 new ProductEntity { Id = 28, Name = "Груша", Calories = 57, Protein = 0.4m, Fat = 0.1m, Carbohydrates = 15.0m },
                 new ProductEntity { Id = 29, Name = "Кешью", Calories = 553, Protein = 18.2m, Fat = 43.8m, Carbohydrates = 30.2m },
                 new ProductEntity { Id = 30, Name = "Рис бурый", Calories = 111, Protein = 2.6m, Fat = 0.9m, Carbohydrates = 23.0m },
                 new ProductEntity { Id = 31, Name = "Оливковое масло", Calories = 90, Protein = 0.0m, Fat = 10.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 32, Name = "Рикотта", Calories = 150, Protein = 11.0m, Fat = 9.0m, Carbohydrates = 3.0m },
                 new ProductEntity { Id = 33, Name = "Греческий йогурт", Calories = 60, Protein = 10.0m, Fat = 1.0m, Carbohydrates = 4.0m },
                 new ProductEntity { Id = 34, Name = "Хумус", Calories = 150, Protein = 6.0m, Fat = 8.0m, Carbohydrates = 12.0m },
                 new ProductEntity { Id = 35, Name = "Стейк говяжий", Calories = 280, Protein = 26.0m, Fat = 18.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 36, Name = "Тунец консервированный", Calories = 150, Protein = 26.0m, Fat = 1.0m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 37, Name = "Огурец", Calories = 22, Protein = 1.0m, Fat = 0m, Carbohydrates = 4.0m },
                 new ProductEntity { Id = 38, Name = "Помидоры черри", Calories = 20, Protein = 1.0m, Fat = 0m, Carbohydrates = 4.0m },
                 new ProductEntity { Id = 39, Name = "Апельсин", Calories = 47, Protein = 0.9m, Fat = 0.1m, Carbohydrates = 11.8m },
                 new ProductEntity { Id = 40, Name = "Морковь", Calories = 41, Protein = 0.9m, Fat = 0.2m, Carbohydrates = 9.6m },
                 new ProductEntity { Id = 41, Name = "Миндаль", Calories = 579, Protein = 21.2m, Fat = 49.9m, Carbohydrates = 21.6m },
                 new ProductEntity { Id = 42, Name = "Зеленая фасоль", Calories = 31, Protein = 2.4m, Fat = 0.1m, Carbohydrates = 7.0m },
                 new ProductEntity { Id = 43, Name = "Черника", Calories = 25, Protein = 0.0m, Fat = 0.0m, Carbohydrates = 6.0m },
                 new ProductEntity { Id = 44, Name = "Филе трески", Calories = 82, Protein = 18.0m, Fat = 0.7m, Carbohydrates = 0.0m },
                 new ProductEntity { Id = 45, Name = "Шампиньоны", Calories = 27, Protein = 3.1m, Fat = 0.3m, Carbohydrates = 3.3m },
                 new ProductEntity { Id = 46, Name = "Хлеб цельнозерновой", Calories = 70, Protein = 3.0m, Fat = 0.5m, Carbohydrates = 13.0m },
                 new ProductEntity { Id = 47, Name = "Помидор", Calories = 20, Protein = 1.0m, Fat = 0.0m, Carbohydrates = 4.0m },
                 new ProductEntity { Id = 48, Name = "Груша", Calories = 60, Protein = 0.0m, Fat = 0.0m, Carbohydrates = 15.0m },
                 new ProductEntity { Id = 49, Name = "Кабачки", Calories = 30, Protein = 2.0m, Fat = 0.0m, Carbohydrates = 6.0m },
                 new ProductEntity { Id = 50, Name = "Йогурт без сахара", Calories = 90, Protein = 10.0m, Fat = 3.0m, Carbohydrates = 4.0m },
                 new ProductEntity { Id = 51, Name = "Хлопья цельнозерновые", Calories = 140, Protein = 4.0m, Fat = 2.0m, Carbohydrates = 30.0m },
                 new ProductEntity { Id = 52, Name = "Маффины", Calories = 420, Protein = 6.0m, Fat = 20.0m, Carbohydrates = 55.0m, Barcode = "4607001417255" }
            );

            modelBuilder.Entity<RecipeEntity>().HasData(
                new RecipeEntity { Id = 1, Name = "Омлет с авокадо", Description = "Питательный завтрак с яйцами и авокадо" },
                new RecipeEntity { Id = 2, Name = "Салат с курицей и овощами", Description = "Легкий и полезный обед" },
                new RecipeEntity { Id = 3, Name = "Гречневая каша с овощами", Description = "Вегетарианский ужин" },
                new RecipeEntity { Id = 4, Name = "Фруктовый смузи", Description = "Освежающий перекус" },
                new RecipeEntity { Id = 5, Name = "Запеченный лосось с картофелем", Description = "Полезный ужин с рыбой" }
            );

            // Добавляем ингредиенты для рецептов
            modelBuilder.Entity<RecipeIngredientEntity>().HasData(
                // Ингредиенты для омлета с авокадо (id=1)
                new RecipeIngredientEntity { Id = 1, RecipeId = 1, ProductId = 1, Amount = 100 }, // Яйца
                new RecipeIngredientEntity { Id = 2, RecipeId = 1, ProductId = 2, Amount = 50 },   // Авокадо
                new RecipeIngredientEntity { Id = 3, RecipeId = 1, ProductId = 31, Amount = 10 },  // Оливковое масло

                // Ингредиенты для салата с курицей (id=2)
                new RecipeIngredientEntity { Id = 4, RecipeId = 2, ProductId = 6, Amount = 150 },  // Куриная грудка
                new RecipeIngredientEntity { Id = 5, RecipeId = 2, ProductId = 7, Amount = 100 },  // Брокколи
                new RecipeIngredientEntity { Id = 6, RecipeId = 2, ProductId = 37, Amount = 50 },  // Огурец
                new RecipeIngredientEntity { Id = 7, RecipeId = 2, ProductId = 38, Amount = 70 },  // Помидоры черри
                new RecipeIngredientEntity { Id = 8, RecipeId = 2, ProductId = 31, Amount = 15 },  // Оливковое масло

                // Ингредиенты для гречневой каши (id=3)
                new RecipeIngredientEntity { Id = 9, RecipeId = 3, ProductId = 24, Amount = 100 }, // Гречка
                new RecipeIngredientEntity { Id = 10, RecipeId = 3, ProductId = 21, Amount = 100 }, // Кабачки
                new RecipeIngredientEntity { Id = 11, RecipeId = 3, ProductId = 40, Amount = 50 },  // Морковь
                new RecipeIngredientEntity { Id = 12, RecipeId = 3, ProductId = 27, Amount = 30 },  // Шпинат

                // Ингредиенты для фруктового смузи (id=4)
                new RecipeIngredientEntity { Id = 13, RecipeId = 4, ProductId = 9, Amount = 100 }, // Банан
                new RecipeIngredientEntity { Id = 14, RecipeId = 4, ProductId = 13, Amount = 50 },  // Ягоды
                new RecipeIngredientEntity { Id = 15, RecipeId = 4, ProductId = 33, Amount = 100 }, // Греческий йогурт
                new RecipeIngredientEntity { Id = 16, RecipeId = 4, ProductId = 17, Amount = 50 },  // Молоко

                // Ингредиенты для лосося с картофелем (id=5)
                new RecipeIngredientEntity { Id = 17, RecipeId = 5, ProductId = 14, Amount = 200 }, // Лосось
                new RecipeIngredientEntity { Id = 18, RecipeId = 5, ProductId = 15, Amount = 150 },  // Картофель
                new RecipeIngredientEntity { Id = 19, RecipeId = 5, ProductId = 27, Amount = 50 },  // Шпинат
                new RecipeIngredientEntity { Id = 20, RecipeId = 5, ProductId = 31, Amount = 15 }   // Оливковое масло
            );

            modelBuilder.Entity<WeeklyMenuEntity>().HasData(
                // День 1 (Понедельник)
                new WeeklyMenuEntity { Id = 1, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 1, ProductId = 1, Amount = 2 }, // Яйца
                new WeeklyMenuEntity { Id = 2, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 1, ProductId = 2, Amount = 0.5m }, // Авокадо
                new WeeklyMenuEntity { Id = 3, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 1, ProductId = 46, Amount = 0.3m }, // Хлеб цельнозерновой

                new WeeklyMenuEntity { Id = 4, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 2, ProductId = 6, Amount = 2 }, // Грудка куриная
                new WeeklyMenuEntity { Id = 5, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 2, ProductId = 7, Amount = 2 }, // Брокколи
                new WeeklyMenuEntity { Id = 6, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло

                new WeeklyMenuEntity { Id = 7, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 3, ProductId = 14, Amount = 1.5m }, // Лосось
                new WeeklyMenuEntity { Id = 8, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 3, ProductId = 27, Amount = 1.5m }, // Шпинат

                new WeeklyMenuEntity { Id = 9, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 4, ProductId = 4, Amount = 0.2m }, // Миндаль
                new WeeklyMenuEntity { Id = 10, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 4, ProductId = 33, Amount = 1 }, // Греческий йогурт
                new WeeklyMenuEntity { Id = 11,GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 4, ProductId = 12, Amount = 0.1m }, // Мед
                new WeeklyMenuEntity { Id = 12, GoalTypeId = 1, DayOfWeek = "Понедельник", MealTimeId = 4, ProductId = 5, Amount = 1.5m }, // Яблоко

                // День 2 (Вторник)
                new WeeklyMenuEntity { Id = 13,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 1, ProductId = 16, Amount = 0.4m }, // Овсянка
                new WeeklyMenuEntity { Id = 14,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 1, ProductId = 5, Amount = 1m }, // Яблоко
                new WeeklyMenuEntity { Id = 15,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 1, ProductId = 8, Amount = 0.1m }, // Орехи грецкие

                new WeeklyMenuEntity { Id = 16,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 2, ProductId = 20, Amount = 2 }, // Индейка
                new WeeklyMenuEntity { Id = 17,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло
                new WeeklyMenuEntity { Id = 18,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 2, ProductId = 21, Amount = 2 }, // Кабачки

                new WeeklyMenuEntity { Id = 19,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 3, ProductId = 36, Amount = 1.5m }, // Тунец консервированный
                new WeeklyMenuEntity { Id = 20,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 3, ProductId = 37, Amount = 1 }, // Огурец
                new WeeklyMenuEntity { Id = 21,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 3, ProductId = 46, Amount = 0.3m }, // Хлеб цельнозерновой

                new WeeklyMenuEntity { Id = 22,GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 4, ProductId = 32, Amount = 1 }, // Рикотта
                new WeeklyMenuEntity {Id = 23, GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 4, ProductId = 13, Amount = 1 }, // Ягоды
                new WeeklyMenuEntity {Id = 24, GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 4, ProductId = 17, Amount = 1.5m }, // Йогурт без сахара
                new WeeklyMenuEntity {Id = 25, GoalTypeId = 1, DayOfWeek = "Вторник", MealTimeId = 4, ProductId = 28, Amount = 1 }, // Груша

                // День 3 (Среда)
                new WeeklyMenuEntity {Id = 26, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 1, ProductId = 1, Amount = 2 }, // Яйца
                new WeeklyMenuEntity {Id = 27, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 1, ProductId = 38, Amount = 1 }, // Помидоры черри
                new WeeklyMenuEntity {Id = 28, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 1, ProductId = 46, Amount = 0.3m }, // Ржаной хлеб

                new WeeklyMenuEntity {Id = 29, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 2, ProductId = 22, Amount = 0.15m }, // Говядина отварная
                new WeeklyMenuEntity {Id = 30, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 2, ProductId = 42, Amount = 2 }, // Зеленая фасоль
                new WeeklyMenuEntity {Id = 31,GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло

                new WeeklyMenuEntity {Id = 32, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 3, ProductId = 11, Amount = 1.5m }, // Творог 5%
                new WeeklyMenuEntity {Id = 33, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 3, ProductId = 27, Amount = 1 }, // Морковь
                new WeeklyMenuEntity {Id = 34, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 3, ProductId = 12, Amount = 0.4m }, // Мед

                new WeeklyMenuEntity {Id = 35, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 4, ProductId = 34, Amount = 0.5m }, // Хумус
                new WeeklyMenuEntity {Id = 36, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 4, ProductId = 39, Amount = 1.5m }, // Апельсин
                new WeeklyMenuEntity {Id = 37, GoalTypeId = 1, DayOfWeek = "Среда", MealTimeId = 4, ProductId = 41, Amount = 0.15m }, // Миндаль

                // День 4 (Четверг)
                new WeeklyMenuEntity {Id = 38, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 1, ProductId = 16, Amount = 0.4m }, // Овсянка
                new WeeklyMenuEntity { Id = 39,GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 1, ProductId = 43, Amount = 0.5m }, // Черника
                new WeeklyMenuEntity {Id = 40, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 1, ProductId = 8, Amount = 0.15m }, // Грецкие орехи

                new WeeklyMenuEntity { Id = 41,GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 2, ProductId = 44, Amount = 2.0m }, // Филе трески
                new WeeklyMenuEntity {Id = 42, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 2, ProductId = 19, Amount = 0.5m }, // Киноа
                new WeeklyMenuEntity {Id = 43, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 2, ProductId = 7, Amount = 1.5m }, // Брокколи

                new WeeklyMenuEntity {Id = 44, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 3, ProductId = 20, Amount = 1.5m }, // Филе индейки
                new WeeklyMenuEntity {Id = 45, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 3, ProductId = 45, Amount = 1.5m }, // Шампиньоны

                new WeeklyMenuEntity {Id = 46, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 4, ProductId = 11, Amount = 1.0m }, // Творог 5%
                new WeeklyMenuEntity {Id = 47, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 4, ProductId = 33, Amount = 1.0m }, // Греческий йогурт
                new WeeklyMenuEntity {Id = 48, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 4, ProductId = 9, Amount = 1.0m }, // Банан
                new WeeklyMenuEntity {Id = 49, GoalTypeId = 1, DayOfWeek = "Четверг", MealTimeId = 4, ProductId = 12, Amount = 0.1m }, // Мед

                // День 5 (Пятница)
                new WeeklyMenuEntity { Id = 50,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 1, ProductId = 1, Amount = 2 }, // Яйца
                new WeeklyMenuEntity { Id = 51,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 1, ProductId = 17, Amount = 0.01m }, // Молоко 1.5%
                new WeeklyMenuEntity { Id = 52,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 1, ProductId = 46, Amount = 0.3m }, // Ржаной хлеб
                new WeeklyMenuEntity {Id = 53, GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 1, ProductId = 47, Amount = 1.0m }, // Помидор

                new WeeklyMenuEntity { Id = 54,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 2, ProductId = 6, Amount = 2.0m }, // Грудка куриная
                new WeeklyMenuEntity {Id = 55, GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 2, ProductId = 49, Amount = 1.5m }, // Кабачки
                new WeeklyMenuEntity { Id = 56,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло

                new WeeklyMenuEntity { Id = 57,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 3, ProductId = 14, Amount = 1.5m }, // Лосось
                new WeeklyMenuEntity {Id = 58, GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 3, ProductId = 27, Amount = 1.0m }, // Зеленый салат

                new WeeklyMenuEntity {Id = 59, GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 4, ProductId = 4, Amount = 0.15m }, // Миндаль
                new WeeklyMenuEntity { Id = 60,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 4, ProductId = 28, Amount = 1.5m }, // Груша
                new WeeklyMenuEntity {Id = 61, GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 4, ProductId = 50, Amount = 1.5m }, // Йогурт без сахара
                new WeeklyMenuEntity { Id = 62,GoalTypeId = 1, DayOfWeek = "Пятница", MealTimeId = 4, ProductId = 12, Amount = 0.1m }, // Мед

                // День 6 (Суббота)
                new WeeklyMenuEntity {Id = 63, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 1, ProductId = 19, Amount = 0.4m }, // Киноа
                new WeeklyMenuEntity {Id = 64, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 1, ProductId = 39, Amount = 1 }, // Апельсин
                new WeeklyMenuEntity {Id = 65, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 1, ProductId = 46, Amount = 0.3m }, // Хлеб цельнозерновой

                new WeeklyMenuEntity {Id = 66, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 2, ProductId = 6, Amount = 2 }, // Грудка куриная
                new WeeklyMenuEntity {Id = 67, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 2, ProductId = 7, Amount = 2 }, // Брокколи
                new WeeklyMenuEntity {Id = 68,GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло

                new WeeklyMenuEntity {Id = 69, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 3, ProductId = 10, Amount = 1.5m }, // Протеиновый коктейль
                new WeeklyMenuEntity {Id = 70, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 3, ProductId = 13, Amount = 0.5m }, // Ягоды
                new WeeklyMenuEntity {Id = 71, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 4, ProductId = 4, Amount = 0.2m }, // Миндаль
                new WeeklyMenuEntity {Id = 72, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 4, ProductId = 5, Amount = 1.5m }, // Яблоко
                new WeeklyMenuEntity {Id = 73, GoalTypeId = 1, DayOfWeek = "Суббота", MealTimeId = 4, ProductId = 33, Amount = 1 }, // Греческий йогурт

                // День 7 (Воскресенье)
                new WeeklyMenuEntity {Id = 74, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 1, ProductId = 15, Amount = 0.5m }, // Картофель запеченный
                new WeeklyMenuEntity {Id = 75, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 1, ProductId = 2, Amount = 0.5m }, // Авокадо
                new WeeklyMenuEntity {Id = 76, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 1, ProductId = 47, Amount = 1 }, // Помидор

                new WeeklyMenuEntity { Id = 77, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 2, ProductId = 22, Amount = 0.15m }, // Говядина тушеная
                new WeeklyMenuEntity { Id = 78, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 2, ProductId = 42, Amount = 2 }, // Зеленая фасоль
                new WeeklyMenuEntity { Id = 79, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 2, ProductId = 31, Amount = 0.1m }, // Оливковое масло

                new WeeklyMenuEntity { Id = 80, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 3, ProductId = 14, Amount = 1.5m }, // Лосось
                new WeeklyMenuEntity { Id = 81, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 3, ProductId = 45, Amount = 1.5m }, // Шампиньоны

                new WeeklyMenuEntity { Id = 82, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 4, ProductId = 12, Amount = 0.1m }, // Мед
                new WeeklyMenuEntity { Id = 83, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 4, ProductId = 17, Amount = 1.5m }, // Йогурт без сахара
                new WeeklyMenuEntity { Id = 84, GoalTypeId = 1, DayOfWeek = "Воскресенье", MealTimeId = 4, ProductId = 28, Amount = 1 } // Груша
            );
        }
    }
}
