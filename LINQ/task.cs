using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace LINQ100TasksComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ПОЛНАЯ РЕАЛИЗАЦИЯ 100 ЗАДАЧ LINQ ===\n");

            // Запуск всех категорий задач
            RunCategory1_Where();
            RunCategory2_Select();
            RunCategory3_OrderBy();
            RunCategory4_GroupBy();
            RunCategory5_Join();
            RunCategory6_Aggregate();
            RunCategory7_SetOperations();
            RunCategory8_Partitioning();
            RunCategory9_SelectMany();
            RunCategory10_Quantifiers();
            RunCategory11_Concatenation();
            RunCategory12_Casting();
            RunCategory13_ComplexQueries();

            Console.WriteLine("\n=== ВСЕ 100 ЗАДАЧ УСПЕШНО ВЫПОЛНЕНЫ! ===");
        }

        #region Категория 1: Where (Фильтрация)
        static void RunCategory1_Where()
        {
            Console.WriteLine("=== КАТЕГОРИЯ 1: WHERE (ФИЛЬТРАЦИЯ) ===\n");

            // Задание 1: Числа больше 50
            int[] numbers1 = { 10, 25, 50, 75, 100, 150 };
            var task1 = numbers1.Where(n => n > 50).ToList();
            Console.WriteLine($"1. Числа > 50: {string.Join(", ", task1)}");

            // Задание 2: Четные числа
            int[] numbers2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var task2 = numbers2.Where(n => n % 2 == 0).ToList();
            Console.WriteLine($"2. Четные числа: {string.Join(", ", task2)}");

            // Задание 3: Строки на 'A'
            string[] fruits = { "Apple", "Banana", "Apricot", "Orange", "Avocado" };
            var task3 = fruits.Where(f => f.StartsWith("A", StringComparison.OrdinalIgnoreCase)).ToList();
            Console.WriteLine($"3. Фрукты на 'A': {string.Join(", ", task3)}");

            // Задание 4: Люди старше 30
            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 35, 45000, 3),
                new Person("Петр", 30, 50000, 5),
                new Person("Анна", 28, 35000, 2),
                new Person("Сергей", 42, 60000, 10)
            };
            var task4 = people.Where(p => p.Age > 30).ToList();
            Console.WriteLine($"4. Люди старше 30: {string.Join(", ", task4.Select(p => $"{p.Name}({p.Age})"))}");

            // Задание 5: Числа кратные 3
            var task5 = numbers2.Where(n => n % 3 == 0).ToList();
            Console.WriteLine($"5. Числа кратные 3: {string.Join(", ", task5)}");

            // Задание 6: Студенты с GPA > 4.0
            var students = new[]
            {
                new Student("Алексей", "Информатика", 3.8),
                new Student("Елена", "Математика", 4.2),
                new Student("Дмитрий", "Информатика", 3.5),
                new Student("Ольга", "Математика", 4.5),
                new Student("Ирина", "Физика", 4.1)
            };
            var task6 = students.Where(s => s.GPA > 4.0).ToList();
            Console.WriteLine($"6. Студенты с GPA > 4.0: {string.Join(", ", task6.Select(s => $"{s.Name}({s.GPA})"))}");

            // Задание 7: Строки длиной > 5
            string[] strings = { "Hi", "Hello", "World", "Programming", "C#" };
            var task7 = strings.Where(s => s.Length > 5).ToList();
            Console.WriteLine($"7. Строки длиной > 5: {string.Join(", ", task7)}");

            // Задание 8: Товары 100-500 руб
            var products = new[]
            {
                new Product("Ноутбук", 800),
                new Product("Мышь", 50),
                new Product("Клавиатура", 120),
                new Product("Монитор", 300),
                new Product("Наушники", 250),
                new Product("Флешка", 80)
            };
            var task8 = products.Where(p => p.Price >= 100 && p.Price <= 500).ToList();
            Console.WriteLine($"8. Товары 100-500 руб: {string.Join(", ", task8.Select(p => $"{p.Name}({p.Price}р)"))}");

            // Задание 9: Положительные числа
            int[] mixedNumbers = { -5, 10, -3, 8, 0, -1, 15, -7, 20 };
            var task9 = mixedNumbers.Where(n => n > 0).ToList();
            Console.WriteLine($"9. Положительные числа: {string.Join(", ", task9)}");

            // Задание 10: Сотрудники с зарплатой выше средней и стажем > 2 лет
            var avgSalary = people.Average(p => p.Salary);
            var task10 = people.Where(p => p.Salary > avgSalary && p.Experience > 2).ToList();
            Console.WriteLine($"10. Сотрудники с зарплатой > {avgSalary:F0}р и стажем > 2 лет: " +
                $"{string.Join(", ", task10.Select(p => $"{p.Name}({p.Salary}р, {p.Experience}л)"))}");
        }
        #endregion

        #region Категория 2: Select (Проекция)
        static void RunCategory2_Select()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 2: SELECT (ПРОЕКЦИЯ) ===");

            // Задание 11: Квадраты чисел
            int[] numbers = { 1, 2, 3, 4, 5 };
            var task11 = numbers.Select(n => n * n).ToList();
            Console.WriteLine($"11. Квадраты чисел: {string.Join(", ", task11)}");

            // Задание 12: Имена людей
            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 35, 45000, 3),
                new Person("Петр", 30, 50000, 5)
            };
            var task12 = people.Select(p => p.Name).ToList();
            Console.WriteLine($"12. Имена людей: {string.Join(", ", task12)}");

            // Задание 13: Имя - Возраст
            var task13 = people.Select(p => $"{p.Name} - {p.Age}").ToList();
            Console.WriteLine($"13. Имя - Возраст: {string.Join("; ", task13)}");

            // Задание 14: Длины строк
            string[] fruits = { "Apple", "Banana", "Apricot", "Orange" };
            var task14 = fruits.Select(f => f.Length).ToList();
            Console.WriteLine($"14. Длины строк: {string.Join(", ", task14)}");

            // Задание 15: Имя и зарплата
            var task15 = people.Select(p => new { p.Name, p.Salary }).ToList();
            Console.WriteLine($"15. Имя и зарплата: {string.Join("; ", task15.Select(x => $"{x.Name}: {x.Salary}р"))}");

            // Задание 16: Цены в USD (ИСПРАВЛЕНО - использование decimal вместо double)
            var products = new[]
            {
                new Product("Ноутбук", 80000),
                new Product("Мышь", 5000),
                new Product("Клавиатура", 12000)
            };
            var task16 = products.Select(p => new { p.Name, PriceUSD = p.Price / 90m }).ToList(); // Используем 90m вместо 90.0
            Console.WriteLine($"16. Цены в USD: {string.Join("; ", task16.Select(x => $"{x.Name}: ${x.PriceUSD:F2}"))}");

            // Задание 17: Анонимные типы
            var task17 = people.Select(p => new { PersonName = p.Name, Years = p.Age, Experience = p.Experience }).ToList();
            Console.WriteLine($"17. Анонимные типы: {string.Join("; ", task17.Select(x => $"{x.PersonName}: {x.Years} лет, опыт {x.Experience}"))}");

            // Задание 18: С префиксом
            var task18 = fruits.Select(f => "Фрукт: " + f).ToList();
            Console.WriteLine($"18. С префиксом: {string.Join(", ", task18)}");

            // Задание 19: Форматированные email
            string[] emails = { "user@example.com", "admin@test.org", "info@company.ru" };
            var task19 = emails.Select(e => e.Replace("@", " [at] ")).ToList();
            Console.WriteLine($"19. Форматированные email: {string.Join(", ", task19)}");

            // Задание 20: Кортежи
            var task20 = people.Select(p => (Name: p.Name, Age: p.Age, Salary: p.Salary)).ToList();
            Console.WriteLine($"20. Кортежи: {string.Join("; ", task20.Select(t => $"{t.Name}: {t.Age} лет, {t.Salary}р"))}");
        }
        #endregion

        #region Категория 3: OrderBy/ThenBy (Сортировка)
        static void RunCategory3_OrderBy()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 3: ORDERBY/THENBY (СОРТИРОВКА) ===");

            // Задание 21: Сортировка по возрастанию
            int[] numbers = { 5, 2, 8, 1, 9, 3, 7 };
            var task21 = numbers.OrderBy(n => n).ToList();
            Console.WriteLine($"21. Сортировка по возрастанию: {string.Join(", ", task21)}");

            // Задание 22: Имена Z-A
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            var task22 = names.OrderByDescending(n => n).ToList();
            Console.WriteLine($"22. Имена Z-A: {string.Join(", ", task22)}");

            // Задание 23: Сортировка по возрасту и имени
            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 30, 45000, 3),
                new Person("Петр", 25, 50000, 5),
                new Person("Анна", 30, 35000, 2)
            };
            var task23 = people.OrderBy(p => p.Age).ThenBy(p => p.Name).ToList();
            Console.WriteLine($"23. Сортировка по возрасту и имени: {string.Join(", ", task23.Select(p => $"{p.Name}({p.Age})"))}");

            // Задание 24: Сортировка по длине
            string[] words = { "яблоко", "дом", "программирование", "код", "алгоритм" };
            var task24 = words.OrderBy(w => w.Length).ToList();
            Console.WriteLine($"24. Сортировка по длине: {string.Join(", ", task24)}");

            // Задание 25: Товары по убыванию цены
            var products = new[]
            {
                new Product("Ноутбук", 80000),
                new Product("Мышь", 5000),
                new Product("Клавиатура", 12000),
                new Product("Монитор", 30000)
            };
            var task25 = products.OrderByDescending(p => p.Price).ToList();
            Console.WriteLine($"25. Товары по убыванию цены: {string.Join(", ", task25.Select(p => $"{p.Name}: {p.Price}р"))}");

            // Задание 26: Студенты по баллам и фамилии
            var students = new[]
            {
                new StudentScore("Иванов", "Алексей", 85),
                new StudentScore("Петров", "Дмитрий", 92),
                new StudentScore("Сидоров", "Михаил", 85),
                new StudentScore("Иванова", "Мария", 78),
                new StudentScore("Петрова", "Анна", 92)
            };
            var task26 = students.OrderByDescending(s => s.Score).ThenBy(s => s.LastName).ToList();
            Console.WriteLine($"26. Студенты по баллам и фамилии: {string.Join(", ", task26.Select(s => $"{s.LastName} {s.FirstName}: {s.Score}"))}");

            // Задание 27: По количеству делителей
            int[] numbersForDivisors = { 12, 5, 24, 7, 18, 9, 11, 6 };
            var task27 = numbersForDivisors.OrderBy(n => CountDivisors(n)).ThenBy(n => n).ToList();
            Console.WriteLine($"27. По количеству делителей: {string.Join(", ", task27)} (в скобках - делители)");
            foreach (var num in task27)
            {
                Console.WriteLine($"   {num} - {CountDivisors(num)} делителей");
            }

            // Задание 28: Сотрудники по отделу и зарплате
            var employees = new[]
            {
                new Employee("IT", "Алиса", 50000),
                new Employee("HR", "Боб", 45000),
                new Employee("IT", "Чарли", 55000),
                new Employee("Финансы", "Диана", 48000),
                new Employee("HR", "Ева", 42000)
            };
            var task28 = employees.OrderBy(e => e.Department).ThenBy(e => e.Salary).ToList();
            Console.WriteLine($"28. Сотрудники по отделу и зарплате:");
            foreach (var emp in task28)
            {
                Console.WriteLine($"   {emp.Department} - {emp.Name}: {emp.Salary}р");
            }

            // Задание 29: Даты по порядку
            DateTime[] dates = {
                new DateTime(2023, 5, 1),
                new DateTime(2023, 1, 15),
                new DateTime(2023, 3, 10),
                new DateTime(2022, 12, 25)
            };
            var task29 = dates.OrderBy(d => d).ToList();
            Console.WriteLine($"29. Даты по порядку: {string.Join(", ", task29.Select(d => d.ToString("dd.MM.yyyy")))}");

            // Задание 30: Продукты по категории и популярности
            var productsWithCategory = new[]
            {
                new ProductCategory("Электроника", "Ноутбук", 4.5),
                new ProductCategory("Электроника", "Телефон", 4.8),
                new ProductCategory("Книги", "Роман", 4.2),
                new ProductCategory("Электроника", "Планшет", 4.3),
                new ProductCategory("Книги", "Учебник", 4.1)
            };
            var task30 = productsWithCategory.OrderBy(p => p.Category).ThenByDescending(p => p.Popularity).ToList();
            Console.WriteLine($"30. Продукты по категории и популярности:");
            foreach (var product in task30)
            {
                Console.WriteLine($"   {product.Category} - {product.Name}: ★{product.Popularity:F1}");
            }
        }
        #endregion

        #region Категория 4: GroupBy (Группировка)
        static void RunCategory4_GroupBy()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 4: GROUPBY (ГРУППИРОВКА) ===");

            // Задание 31: Группировка по четности
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var task31 = numbers.GroupBy(n => n % 2 == 0 ? "Четные" : "Нечетные");
            Console.WriteLine("31. Группировка по четности:");
            foreach (var group in task31)
            {
                Console.WriteLine($"   {group.Key}: {string.Join(", ", group)}");
            }

            // Задание 32: Группировка по возрасту
            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 30, 45000, 3),
                new Person("Петр", 25, 50000, 5),
                new Person("Анна", 30, 35000, 2),
                new Person("Сергей", 35, 60000, 10)
            };
            var task32 = people.GroupBy(p => p.Age);
            Console.WriteLine("32. Группировка по возрасту:");
            foreach (var group in task32.OrderBy(g => g.Key))
            {
                Console.WriteLine($"   {group.Key} лет: {string.Join(", ", group.Select(p => p.Name))}");
            }

            // Задание 33: Количество товаров по категориям
            var products = new[]
            {
                new ProductCategory("Электроника", "Ноутбук", 4.5),
                new ProductCategory("Электроника", "Телефон", 4.8),
                new ProductCategory("Книги", "Роман", 4.2),
                new ProductCategory("Электроника", "Планшет", 4.3),
                new ProductCategory("Книги", "Учебник", 4.1),
                new ProductCategory("Одежда", "Футболка", 4.0)
            };
            var task33 = products.GroupBy(p => p.Category)
                                .Select(g => new { Category = g.Key, Count = g.Count() });
            Console.WriteLine($"33. Количество товаров по категориям: {string.Join("; ", task33.Select(x => $"{x.Category}: {x.Count}"))}");

            // Задание 34: Средний балл по факультетам
            var students = new[]
            {
                new Student("Алексей", "Информатика", 3.8),
                new Student("Елена", "Математика", 4.2),
                new Student("Дмитрий", "Информатика", 3.5),
                new Student("Ольга", "Математика", 4.5),
                new Student("Ирина", "Физика", 4.1),
                new Student("Михаил", "Физика", 3.9)
            };
            var task34 = students.GroupBy(s => s.Department)
                                .Select(g => new { Department = g.Key, AvgGPA = g.Average(s => s.GPA) });
            Console.WriteLine($"34. Средний балл по факультетам: {string.Join("; ", task34.Select(x => $"{x.Department}: {x.AvgGPA:F2}"))}");

            // Задание 35: Сумма зарплат по отделам
            var employees = new[]
            {
                new Employee("IT", "Алиса", 50000),
                new Employee("HR", "Боб", 45000),
                new Employee("IT", "Чарли", 55000),
                new Employee("Финансы", "Диана", 48000),
                new Employee("HR", "Ева", 42000),
                new Employee("IT", "Фрэнк", 60000)
            };
            var task35 = employees.GroupBy(e => e.Department)
                                 .Select(g => new { Department = g.Key, TotalSalary = g.Sum(e => e.Salary) });
            Console.WriteLine($"35. Сумма зарплат по отделам: {string.Join("; ", task35.Select(x => $"{x.Department}: {x.TotalSalary}р"))}");

            // Задание 36: Максимальный заказ по клиентам
            var orders = new[]
            {
                new Order("Клиент1", "Ноутбук", 80000),
                new Order("Клиент1", "Мышь", 5000),
                new Order("Клиент2", "Телефон", 60000),
                new Order("Клиент2", "Чехол", 2000),
                new Order("Клиент3", "Планшет", 40000),
                new Order("Клиент1", "Клавиатура", 12000)
            };
            var task36 = orders.GroupBy(o => o.CustomerName)
                              .Select(g => new { Customer = g.Key, MaxOrder = g.Max(o => o.Price) });
            Console.WriteLine($"36. Максимальный заказ по клиентам: {string.Join("; ", task36.Select(x => $"{x.Customer}: {x.MaxOrder}р"))}");

            // Задание 37: Группировка по первой букве
            string[] words = { "Apple", "Banana", "Apricot", "Blueberry", "Cherry", "Avocado" };
            var task37 = words.GroupBy(w => w[0]).OrderBy(g => g.Key);
            Console.WriteLine("37. Группировка по первой букве:");
            foreach (var group in task37)
            {
                Console.WriteLine($"   '{group.Key}': {string.Join(", ", group)}");
            }

            // Задание 38: Событий по дням
            var events = new[]
            {
                new Event(new DateTime(2023, 5, 1), "Совещание"),
                new Event(new DateTime(2023, 5, 1), "Конференция"),
                new Event(new DateTime(2023, 5, 2), "Воркшоп"),
                new Event(new DateTime(2023, 5, 2), "Семинар"),
                new Event(new DateTime(2023, 5, 2), "Презентация"),
                new Event(new DateTime(2023, 5, 3), "Интервью")
            };
            var task38 = events.GroupBy(e => e.Date)
                              .Select(g => new { Date = g.Key.ToString("dd.MM.yyyy"), Count = g.Count() });
            Console.WriteLine($"38. Событий по дням: {string.Join("; ", task38.Select(x => $"{x.Date}: {x.Count}"))}");

            // Задание 39: Продажи по месяцам
            var sales = new[]
            {
                new Sale(new DateTime(2023, 1, 15), 100000),
                new Sale(new DateTime(2023, 1, 20), 150000),
                new Sale(new DateTime(2023, 2, 10), 80000),
                new Sale(new DateTime(2023, 2, 25), 120000),
                new Sale(new DateTime(2023, 3, 5), 200000),
                new Sale(new DateTime(2023, 3, 18), 90000)
            };
            var task39 = sales.GroupBy(s => s.Date.Month)
                             .Select(g => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key), Total = g.Sum(s => s.Amount) });
            Console.WriteLine($"39. Продажи по месяцам: {string.Join("; ", task39.Select(x => $"{x.Month}: {x.Total}р"))}");

            // Задание 40: Двухуровневая группировка
            var twoLevelData = new[]
            {
                new { Category = "Электроника", SubCategory = "Телефоны", Product = "iPhone" },
                new { Category = "Электроника", SubCategory = "Телефоны", Product = "Samsung" },
                new { Category = "Электроника", SubCategory = "Ноутбуки", Product = "MacBook" },
                new { Category = "Электроника", SubCategory = "Ноутбуки", Product = "Dell" },
                new { Category = "Книги", SubCategory = "Художественные", Product = "Роман" },
                new { Category = "Книги", SubCategory = "Учебные", Product = "Учебник" },
                new { Category = "Одежда", SubCategory = "Верхняя", Product = "Куртка" }
            };
            var task40 = twoLevelData.GroupBy(x => x.Category)
                                    .Select(g => new
                                    {
                                        Category = g.Key,
                                        SubGroups = g.GroupBy(x => x.SubCategory)
                                                    .Select(sg => new { SubCategory = sg.Key, Products = sg.Select(x => x.Product) })
                                    });
            Console.WriteLine("40. Двухуровневая группировка:");
            foreach (var category in task40)
            {
                Console.WriteLine($"   {category.Category}:");
                foreach (var subGroup in category.SubGroups)
                {
                    Console.WriteLine($"     {subGroup.SubCategory}: {string.Join(", ", subGroup.Products)}");
                }
            }
        }
        #endregion

        #region Категория 5: Join (Объединение)
        static void RunCategory5_Join()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 5: JOIN (ОБЪЕДИНЕНИЕ) ===");

            // Задание 41: Студенты с оценками
            var students = new[]
            {
                new { Id = 1, Name = "Алексей" },
                new { Id = 2, Name = "Елена" },
                new { Id = 3, Name = "Дмитрий" }
            };
            var grades = new[]
            {
                new { StudentId = 1, Grade = "A" },
                new { StudentId = 2, Grade = "B" },
                new { StudentId = 3, Grade = "A" },
                new { StudentId = 1, Grade = "C" }
            };
            var task41 = students.Join(grades, s => s.Id, g => g.StudentId, (s, g) => new { s.Name, g.Grade });
            Console.WriteLine($"41. Студенты с оценками: {string.Join("; ", task41.Select(x => $"{x.Name}: {x.Grade}"))}");

            // Задание 42: Сотрудники с отделами
            var departments = new[]
            {
                new { Id = 1, Name = "IT" },
                new { Id = 2, Name = "HR" },
                new { Id = 3, Name = "Финансы" }
            };
            var employees = new[]
            {
                new { Id = 1, Name = "Алиса", DeptId = 1 },
                new { Id = 2, Name = "Боб", DeptId = 2 },
                new { Id = 3, Name = "Чарли", DeptId = 1 },
                new { Id = 4, Name = "Диана", DeptId = 3 }
            };
            var task42 = employees.Join(departments, e => e.DeptId, d => d.Id, (e, d) => new { e.Name, Department = d.Name });
            Console.WriteLine($"42. Сотрудники с отделами: {string.Join("; ", task42.Select(x => $"{x.Name}: {x.Department}"))}");

            // Задание 43: Все клиенты с заказами (Left Join)
            var customers = new[]
            {
                new { Id = 1, Name = "Клиент1" },
                new { Id = 2, Name = "Клиент2" },
                new { Id = 3, Name = "Клиент3" },
                new { Id = 4, Name = "Клиент4" }
            };
            var customerOrders = new[]
            {
                new { CustomerId = 1, Order = "Заказ1" },
                new { CustomerId = 2, Order = "Заказ2" },
                new { CustomerId = 1, Order = "Заказ3" }
            };
            var task43 = from c in customers
                         join o in customerOrders on c.Id equals o.CustomerId into customerOrdersGroup
                         from order in customerOrdersGroup.DefaultIfEmpty()
                         select new { c.Name, Order = order?.Order ?? "Нет заказов" };
            Console.WriteLine($"43. Все клиенты с заказами: {string.Join("; ", task43.Select(x => $"{x.Name}: {x.Order}"))}");

            // Задание 44: Товары с производителями
            var manufacturers = new[]
            {
                new { Id = 1, Name = "Apple" },
                new { Id = 2, Name = "Samsung" },
                new { Id = 3, Name = "Lenovo" }
            };
            var products = new[]
            {
                new { Id = 1, Name = "iPhone", ManufacturerId = 1 },
                new { Id = 2, Name = "Galaxy", ManufacturerId = 2 },
                new { Id = 3, Name = "ThinkPad", ManufacturerId = 3 },
                new { Id = 4, Name = "MacBook", ManufacturerId = 1 }
            };
            var task44 = products.Join(manufacturers, p => p.ManufacturerId, m => m.Id, (p, m) => new { p.Name, Manufacturer = m.Name });
            Console.WriteLine($"44. Товары с производителями: {string.Join("; ", task44.Select(x => $"{x.Name} от {x.Manufacturer}"))}");

            // Задание 45: Составной ключ
            var data1 = new[]
            {
                new { Key1 = 1, Key2 = "A", Value = "Значение1" },
                new { Key1 = 2, Key2 = "B", Value = "Значение2" }
            };
            var data2 = new[]
            {
                new { Key1 = 1, Key2 = "A", Additional = "Дополнительно1" },
                new { Key1 = 2, Key2 = "B", Additional = "Дополнительно2" }
            };
            var task45 = data1.Join(data2,
                x => new { x.Key1, x.Key2 },
                y => new { y.Key1, y.Key2 },
                (x, y) => new { x.Value, y.Additional });
            Console.WriteLine($"45. Составной ключ: {string.Join("; ", task45.Select(x => $"{x.Value} - {x.Additional}"))}");

            // Задание 46: Три таблицы
            var task46 = from c in customers
                         join o in customerOrders on c.Id equals o.CustomerId
                         join p in products on 1 equals 1 // Упрощенный пример для демонстрации
                         where c.Id == 1
                         select new { c.Name, o.Order, Product = p.Name };
            Console.WriteLine($"46. Три таблицы (первые 3): {string.Join("; ", task46.Take(3).Select(x => $"{x.Name} - {x.Order} - {x.Product}"))}");

            // Задание 47: Группированное объединение
            var task47 = departments.GroupJoin(employees,
                d => d.Id,
                e => e.DeptId,
                (d, emps) => new { Department = d.Name, Employees = string.Join(", ", emps.Select(e => e.Name)) });
            Console.WriteLine("47. Группированное объединение:");
            foreach (var item in task47)
            {
                Console.WriteLine($"   {item.Department}: {item.Employees}");
            }

            // Задание 48: Кросс-соединение
            var colors = new[] { "Красный", "Синий", "Зеленый" };
            var sizes = new[] { "Маленький", "Средний", "Большой" };
            var task48 = from c in colors
                         from s in sizes
                         select new { Color = c, Size = s };
            Console.WriteLine($"48. Кросс-соединение (первые 5): {string.Join("; ", task48.Take(5).Select(x => $"{x.Color} {x.Size}"))}");

            // Задание 49: Объединение с фильтрацией
            var filteredProducts = products.Where(p => p.Name.Contains("i"));
            var task49 = filteredProducts.Join(manufacturers, p => p.ManufacturerId, m => m.Id, (p, m) => new { p.Name, Manufacturer = m.Name });
            Console.WriteLine($"49. Объединение с фильтрацией (продукты с 'i'): {string.Join("; ", task49.Select(x => $"{x.Name}"))}");

            // Задание 50: Разные структуры
            var source1 = new[] { new { Id = 1, Data = "Данные1" }, new { Id = 2, Data = "Данные2" } };
            var source2 = new[] { new { ID = 1, Info = "Инфо1" }, new { ID = 2, Info = "Инфо2" } };
            var task50 = source1.Join(source2, s1 => s1.Id, s2 => s2.ID, (s1, s2) => new { s1.Data, s2.Info });
            Console.WriteLine($"50. Разные структуры: {string.Join("; ", task50.Select(x => $"{x.Data} - {x.Info}"))}");
        }
        #endregion

        #region Категория 6: Aggregate Functions (Агрегирование)
        static void RunCategory6_Aggregate()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 6: AGGREGATE FUNCTIONS (АГРЕГИРОВАНИЕ) ===");

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Задание 51: Сумма чисел
            var task51 = numbers.Sum();
            Console.WriteLine($"51. Сумма чисел: {task51}");

            // Задание 52: Среднее значение
            var task52 = numbers.Average();
            Console.WriteLine($"52. Среднее значение: {task52:F2}");

            // Задание 53: Минимум и максимум
            var task53Min = numbers.Min();
            var task53Max = numbers.Max();
            Console.WriteLine($"53. Минимум: {task53Min}, Максимум: {task53Max}");

            // Задание 54: Количество четных чисел
            var task54 = numbers.Count(n => n % 2 == 0);
            Console.WriteLine($"54. Количество четных чисел: {task54}");

            // Задание 55: Произведение чисел
            var task55 = numbers.Aggregate(1, (acc, n) => acc * n);
            Console.WriteLine($"55. Произведение чисел: {task55}");

            // Задание 56: Медиана
            var sorted = numbers.OrderBy(n => n).ToList();
            var task56 = sorted.Count % 2 == 0
                ? (sorted[sorted.Count / 2 - 1] + sorted[sorted.Count / 2]) / 2.0
                : sorted[sorted.Count / 2];
            Console.WriteLine($"56. Медиана: {task56}");

            // Задание 57: Есть числа > 100
            var task57 = numbers.Any(n => n > 100);
            Console.WriteLine($"57. Есть числа > 100: {task57}");

            // Задание 58: Факториал 5
            var factorial = numbers.Take(5).Aggregate(1, (acc, n) => acc * n);
            Console.WriteLine($"58. Факториал 5: {factorial}");

            // Задание 59: Сумма квадратов нечетных
            var task59 = numbers.Where(n => n % 2 != 0).Sum(n => n * n);
            Console.WriteLine($"59. Сумма квадратов нечетных: {task59}");

            // Задание 60: Стандартное отклонение
            var avg = numbers.Average();
            var variance = numbers.Average(n => Math.Pow(n - avg, 2));
            var stdDev = Math.Sqrt(variance);
            Console.WriteLine($"60. Стандартное отклонение: {stdDev:F2}");
        }
        #endregion

        #region Категория 7: Set Operations (Операции над множествами)
        static void RunCategory7_SetOperations()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 7: SET OPERATIONS (ОПЕРАЦИИ НАД МНОЖЕСТВАМИ) ===");

            // Задание 61: Без дубликатов
            int[] duplicates = { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5 };
            var task61 = duplicates.Distinct().ToList();
            Console.WriteLine($"61. Без дубликатов: {string.Join(", ", task61)}");

            // Задание 62: Объединение
            int[] listA = { 1, 2, 3, 4 };
            int[] listB = { 3, 4, 5, 6 };
            var task62 = listA.Union(listB).ToList();
            Console.WriteLine($"62. Объединение A ∪ B: {string.Join(", ", task62)}");

            // Задание 63: Разность
            var task63 = listA.Except(listB).ToList();
            Console.WriteLine($"63. Разность A - B: {string.Join(", ", task63)}");

            // Задание 64: Пересечение
            var task64 = listA.Intersect(listB).ToList();
            Console.WriteLine($"64. Пересечение A ∩ B: {string.Join(", ", task64)}");

            // Задание 65: Без дубликатов (регистр)
            string[] caseStrings = { "Apple", "apple", "BANANA", "Banana", "CHERRY", "cherry" };
            var task65 = caseStrings.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            Console.WriteLine($"65. Без дубликатов (игнорировать регистр): {string.Join(", ", task65)}");

            // Задание 66: Distinct с анонимными типами
            var people = new[]
            {
                new { Name = "John", Age = 25 },
                new { Name = "John", Age = 25 },
                new { Name = "Jane", Age = 30 },
                new { Name = "Jane", Age = 30 },
                new { Name = "Bob", Age = 35 }
            };
            var task66 = people.Distinct().ToList();
            Console.WriteLine($"66. Distinct с анонимными типами: {string.Join("; ", task66.Select(p => $"{p.Name}: {p.Age}"))}");

            // Задание 67: Элементы в обоих списках
            var task67 = listA.Intersect(listB).ToList();
            Console.WriteLine($"67. Элементы в обоих списках: {string.Join(", ", task67)}");

            // Задание 68: Симметрическая разность
            var task68 = listA.Union(listB).Except(listA.Intersect(listB)).ToList();
            Console.WriteLine($"68. Симметрическая разность: {string.Join(", ", task68)}");
        }
        #endregion

        #region Категория 8: Partitioning (Партиционирование)
        static void RunCategory8_Partitioning()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 8: PARTITIONING (ПАРТИЦИОНИРОВАНИЕ) ===");

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Задание 69: Первые 3 элемента
            var task69 = numbers.Take(3).ToList();
            Console.WriteLine($"69. Первые 3 элемента: {string.Join(", ", task69)}");

            // Задание 70: Пропустить 2, взять остальные
            var task70 = numbers.Skip(2).ToList();
            Console.WriteLine($"70. Пропустить 2, взять остальные: {string.Join(", ", task70)}");

            // Задание 71: Первый элемент > 5
            var task71 = numbers.First(n => n > 5);
            Console.WriteLine($"71. Первый элемент > 5: {task71}");

            // Задание 72: Последний элемент
            var task72 = numbers.Last();
            Console.WriteLine($"72. Последний элемент: {task72}");

            // Задание 73: Пагинация
            int pageSize = 3;
            int pageNumber = 2;
            var task73 = numbers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            Console.WriteLine($"73. Пагинация (страница {pageNumber}, размер {pageSize}): {string.Join(", ", task73)}");

            // Задание 74: TakeWhile
            var task74 = numbers.TakeWhile(n => n < 6).ToList();
            Console.WriteLine($"74. TakeWhile (n < 6): {string.Join(", ", task74)}");

            // Задание 75: SkipWhile
            var task75 = numbers.SkipWhile(n => n < 6).ToList();
            Console.WriteLine($"75. SkipWhile (n < 6): {string.Join(", ", task75)}");

            // Задание 76: Последние 3 элемента
            var task76 = numbers.TakeLast(3).ToList();
            Console.WriteLine($"76. Последние 3 элемента: {string.Join(", ", task76)}");
        }
        #endregion

        #region Категория 9: SelectMany (Разворачивание)
        static void RunCategory9_SelectMany()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 9: SELECTMANY (РАЗВОРАЧИВАНИЕ) ===");

            // Задание 77: Развернутый список
            int[][] listOfLists = { new[] { 1, 2, 3 }, new[] { 4, 5 }, new[] { 6, 7, 8, 9 } };
            var task77 = listOfLists.SelectMany(x => x).ToList();
            Console.WriteLine($"77. Развернутый список: {string.Join(", ", task77)}");

            // Задание 78: Декартово произведение
            var numbers = new[] { 1, 2, 3 };
            var letters = new[] { "A", "B", "C" };
            var task78 = from n in numbers
                         from l in letters
                         select new { Number = n, Letter = l };
            Console.WriteLine($"78. Декартово произведение: {string.Join("; ", task78.Select(x => $"{x.Number}{x.Letter}"))}");

            // Задание 79: Иерархия в плоский список
            var departments = new[]
            {
                new DepartmentWithEmployees("IT", new[] { "Алиса", "Боб", "Чарли" }),
                new DepartmentWithEmployees("HR", new[] { "Диана", "Ева" }),
                new DepartmentWithEmployees("Финансы", new[] { "Фрэнк", "Грейс" })
            };
            var task79 = departments.SelectMany(d => d.Employees, (d, e) => new { Department = d.Name, Employee = e });
            Console.WriteLine($"79. Иерархия в плоский список: {string.Join("; ", task79.Select(x => $"{x.Employee} ({x.Department})"))}");

            // Задание 80: Вложенные коллекции
            var blogPosts = new[]
            {
                new { Title = "Post1", Tags = new[] { "C#", "LINQ", ".NET" } },
                new { Title = "Post2", Tags = new[] { "SQL", "Database" } },
                new { Title = "Post3", Tags = new[] { "C#", "ASP.NET" } }
            };
            var task80 = blogPosts.SelectMany(p => p.Tags, (p, tag) => new { p.Title, Tag = tag });
            Console.WriteLine($"80. Вложенные коллекции: {string.Join("; ", task80.Select(x => $"{x.Title}: #{x.Tag}"))}");

            // Задание 81: Все комбинации (упрощенный вариант)
            var sets = new[] { new[] { 1, 2 }, new[] { 3, 4 } };
            var task81 = from a in sets[0]
                         from b in sets[1]
                         select new { A = a, B = b };
            Console.WriteLine($"81. Все комбинации из 2 наборов: {string.Join("; ", task81.Select(x => $"{x.A},{x.B}"))}");

            // Задание 82: Все символы
            string[] words = { "Hello", "World", "LINQ" };
            var task82 = words.SelectMany(w => w.ToCharArray()).Distinct().OrderBy(c => c).ToList();
            Console.WriteLine($"82. Все уникальные символы: {string.Join(", ", task82)}");

            // Задание 83: С индексами
            var arrays = new[] { new[] { 10, 20 }, new[] { 30, 40 }, new[] { 50, 60 } };
            var task83 = arrays.SelectMany((arr, index) => arr.Select(x => new { ArrayIndex = index, Value = x }));
            Console.WriteLine($"83. С индексами: {string.Join("; ", task83.Select(x => $"[{x.ArrayIndex}]: {x.Value}"))}");

            // Задание 84: Объединение коллекций
            var collections = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var task84 = collections.SelectMany(x => x).ToList();
            Console.WriteLine($"84. Объединение коллекций: {string.Join(", ", task84)}");
        }
        #endregion

        #region Категория 10: Quantifiers (Проверка условий)
        static void RunCategory10_Quantifiers()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 10: QUANTIFIERS (ПРОВЕРКА УСЛОВИЙ) ===");

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Задание 85: Есть числа > 10
            var task85 = numbers.Any(n => n > 10);
            Console.WriteLine($"85. Есть числа > 10: {task85}");

            // Задание 86: Все числа > 0
            var task86 = numbers.All(n => n > 0);
            Console.WriteLine($"86. Все числа > 0: {task86}");

            // Задание 87: Содержит 5
            var task87 = numbers.Contains(5);
            Console.WriteLine($"87. Содержит 5: {task87}");

            // Задание 88: Есть люди > 30 лет с зарплатой > 40000
            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 35, 45000, 3),
                new Person("Петр", 30, 50000, 5),
                new Person("Анна", 28, 35000, 2)
            };
            var task88 = people.Any(p => p.Age > 30 && p.Salary > 40000);
            Console.WriteLine($"88. Есть люди > 30 лет с зарплатой > 40000: {task88}");

            // Задание 89: Список пуст
            var emptyList = new List<int>();
            var task89 = !emptyList.Any();
            Console.WriteLine($"89. Список пуст: {task89}");

            // Задание 90: Ровно 10 элементов
            var task90 = numbers.Count() == 10;
            Console.WriteLine($"90. Ровно 10 элементов: {task90}");
        }
        #endregion

        #region Категория 11: Concatenation (Объединение)
        static void RunCategory11_Concatenation()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 11: CONCATENATION (ОБЪЕДИНЕНИЕ) ===");

            // Задание 91: Объединение массивов
            int[] arr1 = { 1, 2, 3 };
            int[] arr2 = { 4, 5, 6 };
            var task91 = arr1.Concat(arr2).ToList();
            Console.WriteLine($"91. Объединение массивов: {string.Join(", ", task91)}");

            // Задание 92: Объединение строк
            string[][] stringLists = { new[] { "Hello", "World" }, new[] { "Good", "Morning" }, new[] { "C#", "LINQ" } };
            var task92 = stringLists.SelectMany(x => x).ToList();
            Console.WriteLine($"92. Объединение строк: {string.Join(" ", task92)}");

            // Задание 93: Разные типы
            object[] objects = { 1, "two", 3, "four", 5, "six" };
            var task93 = objects.OfType<int>().Concat(objects.OfType<string>().Select(s => s.Length));
            Console.WriteLine($"93. Числа и длины строк: {string.Join(", ", task93)}");

            // Задание 94: Объединение с сортировкой
            int[] sorted1 = { 1, 3, 5, 7 };
            int[] sorted2 = { 2, 4, 6, 8 };
            var task94 = sorted1.Concat(sorted2).OrderBy(x => x).ToList();
            Console.WriteLine($"94. Объединение с сортировкой: {string.Join(", ", task94)}");
        }
        #endregion

        #region Категория 12: Casting (Приведение типов)
        static void RunCategory12_Casting()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 12: CASTING (ПРИВЕДЕНИЕ ТИПОВ) ===");

            // Задание 95: Приведение к int
            object[] objectsForCast = { 1, 2, 3, 4, 5 };
            var task95 = objectsForCast.Cast<int>().ToList();
            Console.WriteLine($"95. Приведение к int: {string.Join(", ", task95)}");

            // Задание 96: Только int
            object[] mixed = { 1, "hello", 2, "world", 3.14, 4, true, "test", 5 };
            var task96 = mixed.OfType<int>().ToList();
            Console.WriteLine($"96. Только int: {string.Join(", ", task96)}");

            // Задание 97: Только строки
            var task97 = mixed.OfType<string>().ToList();
            Console.WriteLine($"97. Только строки: {string.Join(", ", task97)}");

            // Задание 98: Безопасное преобразование
            string[] stringsForParse = { "1", "2", "3", "invalid", "5", "6.5", "7" };
            var task98 = stringsForParse.Select(s => int.TryParse(s, out int result) ? result : (int?)null)
                                      .Where(n => n.HasValue)
                                      .Select(n => n.Value)
                                      .ToList();
            Console.WriteLine($"98. Безопасное преобразование: {string.Join(", ", task98)}");
        }
        #endregion

        #region Категория 13: Complex Queries (Сложные запросы)
        static void RunCategory13_ComplexQueries()
        {
            Console.WriteLine("\n=== КАТЕГОРИЯ 13: COMPLEX QUERIES (СЛОЖНЫЕ ЗАПРОСЫ) ===");

            var people = new[]
            {
                new Person("Иван", 25, 30000, 1),
                new Person("Мария", 35, 45000, 3),
                new Person("Петр", 30, 50000, 5),
                new Person("Анна", 28, 35000, 2),
                new Person("Сергей", 42, 60000, 10),
                new Person("Ольга", 29, 40000, 4)
            };

            var departments = new[]
            {
                new Department(1, "IT"),
                new Department(2, "HR"),
                new Department(3, "Финансы")
            };

            var employees = new[]
            {
                new Employee("IT", "Иван", 50000),
                new Employee("HR", "Мария", 45000),
                new Employee("IT", "Петр", 55000),
                new Employee("Финансы", "Анна", 48000)
            };

            // Задание 99: Сложный запрос (группировка -> фильтрация -> сортировка)
            var task99 = people
                .Where(p => p.Age > 25)
                .GroupBy(p => p.Experience)
                .Select(g => new
                {
                    Experience = g.Key,
                    Count = g.Count(),
                    AvgSalary = g.Average(p => p.Salary),
                    MaxSalary = g.Max(p => p.Salary),
                    People = string.Join(", ", g.Select(p => p.Name))
                })
                .OrderByDescending(x => x.AvgSalary)
                .ThenBy(x => x.Experience)
                .ToList();

            Console.WriteLine("99. Сложный запрос (группировка -> фильтрация -> сортировка):");
            foreach (var item in task99)
            {
                Console.WriteLine($"   Опыт {item.Experience} лет: {item.Count} чел, средняя зарплата {item.AvgSalary:F0}р, макс {item.MaxSalary}р");
                Console.WriteLine($"   Люди: {item.People}");
            }

            // Задание 100: Многоэтапный запрос
            var task100 = from p in people
                          join d in departments on p.Experience % 3 + 1 equals d.Id
                          where p.Salary > 35000
                          group p by d.Name into g
                          select new
                          {
                              Department = g.Key,
                              EmployeeCount = g.Count(),
                              TotalSalary = g.Sum(x => x.Salary),
                              AvgAge = g.Average(x => x.Age),
                              Employees = string.Join(", ", g.Select(x => x.Name))
                          } into result
                          orderby result.TotalSalary descending
                          select result;

            Console.WriteLine("100. Многоэтапный запрос (соединение -> группировка -> агрегация -> сортировка):");
            foreach (var item in task100)
            {
                Console.WriteLine($"   Отдел {item.Department}:");
                Console.WriteLine($"   Количество: {item.EmployeeCount}, Общая зарплата: {item.TotalSalary}р");
                Console.WriteLine($"   Средний возраст: {item.AvgAge:F1} лет");
                Console.WriteLine($"   Сотрудники: {item.Employees}");
            }
        }
        #endregion

        // Вспомогательный метод для подсчета делителей
        static int CountDivisors(int n)
        {
            int count = 0;
            for (int i = 1; i <= n; i++)
                if (n % i == 0) count++;
            return count;
        }
    }

    #region Вспомогательные классы
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public int Experience { get; set; }

        public Person(string name, int age, decimal salary, int experience)
        {
            Name = name;
            Age = age;
            Salary = salary;
            Experience = experience;
        }

        public override string ToString() => $"{Name} ({Age} лет)";
    }

    class Student
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public double GPA { get; set; }

        public Student(string name, string department, double gpa)
        {
            Name = name;
            Department = department;
            GPA = gpa;
        }
    }

    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    class StudentScore
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Score { get; set; }

        public StudentScore(string lastName, string firstName, int score)
        {
            LastName = lastName;
            FirstName = firstName;
            Score = score;
        }
    }

    class Employee
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public Employee(string department, string name, decimal salary)
        {
            Department = department;
            Name = name;
            Salary = salary;
        }
    }

    class ProductCategory
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public double Popularity { get; set; }

        public ProductCategory(string category, string name, double popularity)
        {
            Category = category;
            Name = name;
            Popularity = popularity;
        }
    }

    class Order
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public Order(string customerName, string productName, decimal price)
        {
            CustomerName = customerName;
            ProductName = productName;
            Price = price;
        }
    }

    class Event
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public Event(DateTime date, string name)
        {
            Date = date;
            Name = name;
        }
    }

    class Sale
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public Sale(DateTime date, decimal amount)
        {
            Date = date;
            Amount = amount;
        }
    }

    class DepartmentWithEmployees
    {
        public string Name { get; set; }
        public string[] Employees { get; set; }

        public DepartmentWithEmployees(string name, string[] employees)
        {
            Name = name;
            Employees = employees;
        }
    }

    class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    #endregion
}
