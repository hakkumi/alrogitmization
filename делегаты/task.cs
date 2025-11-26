using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace DelegatesCompleteDemo
{
    #region Раздел 1: Основные концепции делегатов (Задания 1-25)

    public class BasicDelegatesTasks
    {
        // Задание 1: Простой делегат для операции с двумя числами
        public delegate int BinaryOperation(int a, int b);

        public static void Task1_SimpleDelegate()
        {
            Console.WriteLine("=== Задание 1: Простой делегат для операции с двумя числами ===");

            BinaryOperation add = (a, b) => a + b;
            BinaryOperation multiply = (a, b) => a * b;

            Console.WriteLine($"5 + 3 = {add(5, 3)}");
            Console.WriteLine($"5 * 3 = {multiply(5, 3)}");
        }

        // Задание 2: Делегат для арифметической операции
        public static void Task2_ArithmeticDelegate()
        {
            Console.WriteLine("\n=== Задание 2: Делегат для арифметической операции ===");

            Func<int, int, int> calculator = null;

            calculator += (a, b) => a + b;
            calculator += (a, b) => a - b;
            calculator += (a, b) => a * b;
            calculator += (a, b) => b != 0 ? a / b : 0;

            int x = 10, y = 5;
            foreach (var operation in calculator.GetInvocationList())
            {
                var result = ((Func<int, int, int>)operation)(x, y);
                Console.WriteLine($"{x} {operation.Method.Name.Split('>')[0].Trim()} {y} = {result}");
            }
        }

        // Задание 3: Делегат для вывода сообщения
        public static void Task3_MessageDelegate()
        {
            Console.WriteLine("\n=== Задание 3: Делегат для вывода сообщения ===");

            Action<string> messagePrinter = Console.WriteLine;
            messagePrinter += msg => Console.WriteLine($"Сообщение: {msg}");
            messagePrinter += msg => Console.WriteLine($"[{DateTime.Now}] {msg}");

            messagePrinter("Тестовое сообщение");
        }

        // Задание 4: Делегат-предикат для проверки условия
        public static void Task4_PredicateDelegate()
        {
            Console.WriteLine("\n=== Задание 4: Делегат-предикат для проверки условия ===");

            Predicate<int> isEven = n => n % 2 == 0;
            Predicate<string> isLong = s => s.Length > 5;
            Predicate<double> isPositive = d => d > 0;

            Console.WriteLine($"10 четное: {isEven(10)}");
            Console.WriteLine($"'Hello' длинное: {isLong("Hello")}");
            Console.WriteLine($"-5.5 положительное: {isPositive(-5.5)}");

            // Использование с коллекциями
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var evenNumbers = numbers.FindAll(n => isEven(n));
            Console.WriteLine($"Четные числа: {string.Join(", ", evenNumbers)}");
        }

        // Задание 5: Делегат типа Action
        public static void Task5_ActionDelegate()
        {
            Console.WriteLine("\n=== Задание 5: Делегат типа Action ===");

            Action<string> simpleAction = Console.WriteLine;
            Action<int, int> complexAction = (a, b) =>
            {
                Console.WriteLine($"{a} + {b} = {a + b}");
                Console.WriteLine($"{a} * {b} = {a * b}");
            };

            simpleAction("Простое действие");
            complexAction(5, 3);

            // Многоадресный Action
            Action<string> multiAction = null;
            multiAction += msg => Console.WriteLine($"Action 1: {msg}");
            multiAction += msg => Console.WriteLine($"Action 2: {msg.ToUpper()}");
            multiAction += msg => Console.WriteLine($"Action 3: {msg.Length} символов");

            multiAction("тестовое сообщение");
        }

        // Задание 6: Делегат типа Func для преобразования
        public static void Task6_FuncDelegate()
        {
            Console.WriteLine("\n=== Задание 6: Делегат типа Func для преобразования ===");

            Func<string, string> toUpper = s => s.ToUpper();
            Func<string, int> stringLength = s => s.Length;
            Func<double, double, double> power = Math.Pow;

            Console.WriteLine($"toUpper('hello'): {toUpper("hello")}");
            Console.WriteLine($"stringLength('hello'): {stringLength("hello")}");
            Console.WriteLine($"power(2, 3): {power(2, 3)}");

            // Цепочка преобразований
            Func<string, string> transformationChain = s => s.Trim().ToUpper().Replace(" ", "_");
            Console.WriteLine($"transformationChain('  hello world  '): {transformationChain("  hello world  ")}");
        }

        // Задание 7: Делегат для фильтрации коллекции
        public static void Task7_FilterDelegate()
        {
            Console.WriteLine("\n=== Задание 7: Делегат для фильтрации коллекции ===");

            List<int> numbers = Enumerable.Range(1, 20).ToList();

            Predicate<int> filterEven = n => n % 2 == 0;
            Predicate<int> filterGreaterThan10 = n => n > 10;
            Predicate<int> filterPrime = n =>
            {
                if (n < 2) return false;
                for (int i = 2; i <= Math.Sqrt(n); i++)
                    if (n % i == 0) return false;
                return true;
            };

            Console.WriteLine("Четные числа: " + string.Join(", ", numbers.FindAll(filterEven)));
            Console.WriteLine("Числа > 10: " + string.Join(", ", numbers.FindAll(filterGreaterThan10)));
            Console.WriteLine("Простые числа: " + string.Join(", ", numbers.FindAll(filterPrime)));
        }

        // Задание 8: Делегат callback для асинхронной операции
        public static void Task8_CallbackDelegate()
        {
            Console.WriteLine("\n=== Задание 8: Делегат callback для асинхронной операции ===");

            Action<string> callback = result => Console.WriteLine($"Результат: {result}");

            // Имитация асинхронной операции
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                callback("Операция завершена успешно!");
            });

            Console.WriteLine("Ожидание завершения операции...");
            Thread.Sleep(1500);
        }

        // Задание 9: Делегат для сортировки объектов
        public static void Task9_SortingDelegate()
        {
            Console.WriteLine("\n=== Задание 9: Делегат для сортировки объектов ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000),
                new Person("Diana", 28, 55000)
            };

            Comparison<Person> byName = (p1, p2) => p1.Name.CompareTo(p2.Name);
            Comparison<Person> byAge = (p1, p2) => p1.Age.CompareTo(p2.Age);
            Comparison<Person> bySalary = (p1, p2) => p2.Salary.CompareTo(p1.Salary); // по убыванию

            Console.WriteLine("Сортировка по имени:");
            people.Sort(byName);
            people.ForEach(p => Console.WriteLine(p));

            Console.WriteLine("\nСортировка по возрасту:");
            people.Sort(byAge);
            people.ForEach(p => Console.WriteLine(p));

            Console.WriteLine("\nСортировка по зарплате (убывание):");
            people.Sort(bySalary);
            people.ForEach(p => Console.WriteLine(p));
        }

        // Задание 10: Делегат для обработки ошибок
        public static void Task10_ErrorHandlerDelegate()
        {
            Console.WriteLine("\n=== Задание 10: Делегат для обработки ошибок ===");

            Action<string, Action> safeExecutor = (operationName, operation) =>
            {
                try
                {
                    Console.WriteLine($"Выполнение: {operationName}");
                    operation();
                    Console.WriteLine("Успешно завершено");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            };

            safeExecutor("Деление на ноль", () =>
            {
                int result = 10 / int.Parse("0");
            });

            safeExecutor("Нормальная операция", () =>
            {
                Console.WriteLine("2 + 2 = " + (2 + 2));
            });
        }

        // Задание 11: Делегат для преобразования строки
        public static void Task11_StringTransformDelegate()
        {
            Console.WriteLine("\n=== Задание 11: Делегат для преобразования строки ===");

            Func<string, string>[] transformers = new Func<string, string>[]
            {
                s => s.ToUpper(),
                s => s.ToLower(),
                s => new string(s.Reverse().ToArray()),
                s => string.Join("", s.Where(char.IsLetter)),
                s => s.Replace(" ", "-")
            };

            string testString = "Hello World 123";

            foreach (var transformer in transformers)
            {
                Console.WriteLine($"{transformer.Method.Name}: {transformer(testString)}");
            }
        }

        // Задание 12: Делегат для поиска элемента в коллекции
        public static void Task12_SearchDelegate()
        {
            Console.WriteLine("\n=== Задание 12: Делегат для поиска элемента в коллекции ===");

            List<Product> products = new List<Product>
            {
                new Product("Laptop", 999.99m, "Electronics"),
                new Product("Book", 19.99m, "Education"),
                new Product("Phone", 599.99m, "Electronics"),
                new Product("Pen", 1.99m, "Office")
            };

            Predicate<Product> findExpensive = p => p.Price > 500;
            Predicate<Product> findElectronics = p => p.Category == "Electronics";
            Predicate<Product> findCheap = p => p.Price < 10;

            Console.WriteLine("Дорогие товары:");
            products.FindAll(findExpensive).ForEach(p => Console.WriteLine(p));

            Console.WriteLine("\nЭлектроника:");
            products.FindAll(findElectronics).ForEach(p => Console.WriteLine(p));

            Console.WriteLine("\nДешевые товары:");
            products.FindAll(findCheap).ForEach(p => Console.WriteLine(p));
        }

        // Задание 13: Делегат для валидации данных
        public static void Task13_ValidationDelegate()
        {
            Console.WriteLine("\n=== Задание 13: Делегат для валидации данных ===");

            Predicate<string>[] validators = new Predicate<string>[]
            {
                s => !string.IsNullOrEmpty(s),
                s => s.Length >= 3,
                s => s.All(char.IsLetterOrDigit),
                s => char.IsLetter(s[0])
            };

            string[] testData = { "User123", "ab", "123User", "", "ValidName" };

            foreach (var data in testData)
            {
                bool isValid = validators.All(validator => validator(data));
                Console.WriteLine($"'{data}': {(isValid ? "VALID" : "INVALID")}");
            }
        }

        // Задание 14: Делегат для форматирования объекта
        public static void Task14_FormattingDelegate()
        {
            Console.WriteLine("\n=== Задание 14: Делегат для форматирования объекта ===");

            Func<Person, string>[] formatters = new Func<Person, string>[]
            {
                p => $"{p.Name} ({p.Age} years)",
                p => $"{p.Name} - ${p.Salary}",
                p => $"Person: {p.Name}, Age: {p.Age}, Salary: ${p.Salary}",
                p => p.Name.ToUpper()
            };

            var person = new Person("John", 30, 50000);

            foreach (var formatter in formatters)
            {
                Console.WriteLine(formatter(person));
            }
        }

        // Задание 15: Делегат типа Comparison
        public static void Task15_ComparisonDelegate()
        {
            Console.WriteLine("\n=== Задание 15: Делегат типа Comparison ===");

            List<string> words = new List<string> { "apple", "banana", "cherry", "date", "elderberry" };

            Comparison<string> byLength = (s1, s2) => s1.Length.CompareTo(s2.Length);
            Comparison<string> alphabetical = (s1, s2) => s1.CompareTo(s2);
            Comparison<string> byVowelCount = (s1, s2) =>
                s1.Count(c => "aeiou".Contains(char.ToLower(c))).CompareTo(
                s2.Count(c => "aeiou".Contains(char.ToLower(c))));

            Console.WriteLine("По длине:");
            words.Sort(byLength);
            words.ForEach(w => Console.WriteLine(w));

            Console.WriteLine("\nПо алфавиту:");
            words.Sort(alphabetical);
            words.ForEach(w => Console.WriteLine(w));

            Console.WriteLine("\nПо количеству гласных:");
            words.Sort(byVowelCount);
            words.ForEach(w => Console.WriteLine(w));
        }

        // Задание 16: Делегат для конвертации между типами
        public static void Task16_ConversionDelegate()
        {
            Console.WriteLine("\n=== Задание 16: Делегат для конвертации между типами ===");

            Func<string, int> stringToInt = s => int.TryParse(s, out int result) ? result : 0;
            Func<int, string> intToString = i => i.ToString();
            Func<double, int> doubleToInt = d => (int)Math.Round(d);
            Func<string, DateTime> stringToDate = s => DateTime.TryParse(s, out DateTime result) ? result : DateTime.MinValue;

            Console.WriteLine($"stringToInt('123'): {stringToInt("123")}");
            Console.WriteLine($"intToString(456): {intToString(456)}");
            Console.WriteLine($"doubleToInt(78.9): {doubleToInt(78.9)}");
            Console.WriteLine($"stringToDate('2023-01-15'): {stringToDate("2023-01-15")}");
        }

        // Задание 17: Делегат для обработки элементов коллекции
        public static void Task17_CollectionProcessorDelegate()
        {
            Console.WriteLine("\n=== Задание 17: Делегат для обработки элементов коллекции ===");

            Action<int>[] processors = new Action<int>[]
            {
                n => Console.WriteLine($"Число: {n}"),
                n => Console.WriteLine($"Квадрат: {n * n}"),
                n => Console.WriteLine($"Четное: {n % 2 == 0}"),
                n => Console.WriteLine(new string('*', Math.Abs(n)))
            };

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            foreach (var processor in processors)
            {
                Console.WriteLine($"\n--- {processor.Method.Name} ---");
                numbers.ForEach(processor);
            }
        }

        // Задание 18: Делегат для выполнения математической функции
        public static void Task18_MathFunctionDelegate()
        {
            Console.WriteLine("\n=== Задание 18: Делегат для выполнения математической функции ===");

            Func<double, double>[] mathFunctions = new Func<double, double>[]
            {
                x => x * x,
                Math.Sin,
                Math.Cos,
                x => Math.Exp(x),
                x => Math.Log(x + 1),
                Math.Abs
            };

            double input = 1.0;
            foreach (var func in mathFunctions)
            {
                try
                {
                    Console.WriteLine($"{func.Method.Name}({input}) = {func(input):F4}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{func.Method.Name}({input}) = ERROR: {ex.Message}");
                }
            }
        }

        // Задание 19: Делегат для обхода графа или дерева
        public static void Task19_TreeTraversalDelegate()
        {
            Console.WriteLine("\n=== Задание 19: Делегат для обхода графа или дерева ===");

            TreeNode root = new TreeNode(1);
            root.Children.Add(new TreeNode(2));
            root.Children.Add(new TreeNode(3));
            root.Children[0].Children.Add(new TreeNode(4));
            root.Children[0].Children.Add(new TreeNode(5));
            root.Children[1].Children.Add(new TreeNode(6));

            Action<TreeNode> printNode = node => Console.Write($"{node.Value} ");

            Console.WriteLine("Pre-order traversal:");
            TraversePreOrder(root, printNode);
            Console.WriteLine();

            Console.WriteLine("Post-order traversal:");
            TraversePostOrder(root, printNode);
            Console.WriteLine();
        }

        // Задание 20: Делегат для создания объектов (factory pattern)
        public static void Task20_FactoryDelegate()
        {
            Console.WriteLine("\n=== Задание 20: Делегат для создания объектов (factory pattern) ===");

            Func<string, Person> personFactory = name => new Person(name, 25, 50000);
            Func<string, decimal, Product> productFactory = (name, price) => new Product(name, price, "General");

            var person = personFactory("John Doe");
            var product = productFactory("Widget", 9.99m);

            Console.WriteLine(person);
            Console.WriteLine(product);
        }

        // Задание 21: Делегат для кеширования результатов функции
        public static void Task21_CachingDelegate()
        {
            Console.WriteLine("\n=== Задание 21: Делегат для кеширования результатов функции ===");

            Func<int, long> fibonacci = null;
            var cache = new Dictionary<int, long>();

            fibonacci = n =>
            {
                if (cache.ContainsKey(n)) return cache[n];
                if (n <= 1) return n;
                var result = fibonacci(n - 1) + fibonacci(n - 2);
                cache[n] = result;
                return result;
            };

            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine($"Fibonacci({i}) = {fibonacci(i)}");
            }
        }

        // Задание 22: Делегат для логирования операций
        public static void Task22_LoggingDelegate()
        {
            Console.WriteLine("\n=== Задание 22: Делегат для логирования операций ===");

            Action<string, string> logger = (level, message) =>
                Console.WriteLine($"[{level}] {DateTime.Now:HH:mm:ss} - {message}");

            Action<string> operation = opName =>
            {
                logger("INFO", $"Начало операции: {opName}");
                Thread.Sleep(100);
                logger("SUCCESS", $"Завершение операции: {opName}");
            };

            operation("Чтение данных");
            operation("Запись в базу");
            operation("Отправка email");
        }

        // Задание 23: Делегат для таймера и отложенного выполнения
        public static void Task23_TimerDelegate()
        {
            Console.WriteLine("\n=== Задание 23: Делегат для таймера и отложенного выполнения ===");

            Action<string> delayedAction = message =>
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            };

            // Имитация таймера
            for (int i = 1; i <= 3; i++)
            {
                int delay = i * 1000;
                Task.Run(() =>
                {
                    Thread.Sleep(delay);
                    delayedAction($"Сообщение после {delay}ms задержки");
                });
            }

            Thread.Sleep(3500);
        }

        // Задание 24: Делегат для обработки пользовательского ввода
        public static void Task24_InputHandlerDelegate()
        {
            Console.WriteLine("\n=== Задание 24: Делегат для обработки пользовательского ввода ===");

            Dictionary<string, Action<string>> commandHandlers = new Dictionary<string, Action<string>>
            {
                ["echo"] = input => Console.WriteLine($"ECHO: {input}"),
                ["upper"] = input => Console.WriteLine(input.ToUpper()),
                ["lower"] = input => Console.WriteLine(input.ToLower()),
                ["length"] = input => Console.WriteLine($"Длина: {input.Length}"),
                ["reverse"] = input => Console.WriteLine(new string(input.Reverse().ToArray()))
            };

            // Имитация ввода
            string[] inputs = { "echo Hello", "upper test", "lower TEST", "length abcde", "reverse 123" };

            foreach (var input in inputs)
            {
                var parts = input.Split(' ', 2);
                if (parts.Length == 2 && commandHandlers.ContainsKey(parts[0]))
                {
                    Console.Write($"{input} -> ");
                    commandHandlers[parts[0]](parts[1]);
                }
            }
        }

        // Задание 25: Делегат для паттерна Observer
        public static void Task25_ObserverPatternDelegate()
        {
            Console.WriteLine("\n=== Задание 25: Делегат для паттерна Observer ===");

            var subject = new Subject();

            // Подписчики
            subject.OnChange += () => Console.WriteLine("Observer 1: Состояние изменилось!");
            subject.OnChange += () => Console.WriteLine("Observer 2: Получено обновление!");
            subject.OnChange += () => Console.WriteLine($"Observer 3: Время изменения - {DateTime.Now:HH:mm:ss}");

            // Имитация изменений
            subject.DoSomething();
            subject.DoSomething();
        }

        // Вспомогательные методы для задания 19
        static void TraversePreOrder(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;
            action(node);
            foreach (var child in node.Children)
                TraversePreOrder(child, action);
        }

        static void TraversePostOrder(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;
            foreach (var child in node.Children)
                TraversePostOrder(child, action);
            action(node);
        }
    }

    #endregion

    #region Раздел 2: Комбинированные делегаты (Задания 26-50)

    public class MulticastDelegatesTasks
    {
        // Задание 26: Использование оператора + для добавления методов
        public static void Task26_DelegateAddition()
        {
            Console.WriteLine("\n=== Задание 26: Использование оператора + для добавления методов ===");

            Action<string> logger = null;

            Action<string> fileLogger = msg => Console.WriteLine($"[FILE] {msg}");
            Action<string> consoleLogger = msg => Console.WriteLine($"[CONSOLE] {msg}");
            Action<string> emailLogger = msg => Console.WriteLine($"[EMAIL] {msg}");

            logger += fileLogger;
            logger += consoleLogger;
            logger += emailLogger;

            Console.WriteLine($"Количество методов: {logger.GetInvocationList().Length}");
            logger("Тестовое сообщение");
        }

        // Задание 27: Цепочка вызовов методов через объединение делегатов
        public static void Task27_DelegateChaining()
        {
            Console.WriteLine("\n=== Задание 27: Цепочка вызовов методов через объединение делегатов ===");

            Func<string, string> pipeline = null;

            pipeline += s => s.Trim();
            pipeline += s => s.ToUpper();
            pipeline += s => s.Replace(" ", "_");
            pipeline += s => $"RESULT_{s}_END";

            string input = "  hello world  ";
            string result = pipeline(input);
            Console.WriteLine($"Вход: '{input}'");
            Console.WriteLine($"Выход: '{result}'");

            // Вызов каждого метода отдельно
            Console.WriteLine("\nПошаговое выполнение:");
            foreach (var func in pipeline.GetInvocationList())
            {
                input = ((Func<string, string>)func)(input);
                Console.WriteLine($"Промежуточный результат: '{input}'");
            }
        }

        // Задание 28: Многоадресные делегаты
        public static void Task28_MulticastDelegates()
        {
            Console.WriteLine("\n=== Задание 28: Многоадресные делегаты ===");

            Action multicast = null;

            multicast += () => Console.WriteLine("Метод 1 выполнен");
            multicast += () => Console.WriteLine("Метод 2 выполнен");
            multicast += () => Console.WriteLine("Метод 3 выполнен");
            multicast += () => Console.WriteLine("Метод 4 выполнен");

            Console.WriteLine("Вызов многоадресного делегата:");
            multicast();

            Console.WriteLine($"\nВсего методов: {multicast.GetInvocationList().Length}");
        }

        // Задание 29: Делегат с методом подписки для нескольких обработчиков
        public static void Task29_MultipleHandlers()
        {
            Console.WriteLine("\n=== Задание 29: Делегат с методом подписки для нескольких обработчиков ===");

            var eventSource = new EventSource();

            // Подписка нескольких обработчиков
            eventSource.OnEvent += msg => Console.WriteLine($"Handler 1: {msg}");
            eventSource.OnEvent += msg => Console.WriteLine($"Handler 2: {msg.ToUpper()}");
            eventSource.OnEvent += msg => Console.WriteLine($"Handler 3: Обработано в {DateTime.Now:HH:mm:ss}");

            eventSource.TriggerEvent("Важное событие!");
        }

        // Задание 30: Делегат для логирования и отправки email одновременно
        public static void Task30_LogAndEmailDelegate()
        {
            Console.WriteLine("\n=== Задание 30: Делегат для логирования и отправки email одновременно ===");

            Action<string, string> notificationSystem = null;

            notificationSystem += (subject, body) =>
                Console.WriteLine($"[LOG] {DateTime.Now}: {subject} - {body}");

            notificationSystem += (subject, body) =>
                Console.WriteLine($"[EMAIL] Отправка: '{subject}' - {body}");

            notificationSystem += (subject, body) =>
                Console.WriteLine($"[SMS] Уведомление: {subject}");

            notificationSystem("Системное уведомление", "Система работает стабильно");
        }

        // Задание 31: Делегат с методом отписки
        public static void Task31_DelegateUnsubscription()
        {
            Console.WriteLine("\n=== Задание 31: Делегат с методом отписки ===");

            Action<string> notifier = null;

            Action<string> emailNotifier = msg => Console.WriteLine($"[EMAIL] {msg}");
            Action<string> smsNotifier = msg => Console.WriteLine($"[SMS] {msg}");
            Action<string> pushNotifier = msg => Console.WriteLine($"[PUSH] {msg}");

            // Подписка
            notifier += emailNotifier;
            notifier += smsNotifier;
            notifier += pushNotifier;

            Console.WriteLine("После подписки всех обработчиков:");
            notifier("Первое сообщение");

            // Отписка
            notifier -= smsNotifier;
            notifier -= pushNotifier;

            Console.WriteLine("\nПосле отписки SMS и PUSH:");
            notifier("Второе сообщение");

            // Полная отписка
            notifier -= emailNotifier;
            Console.WriteLine("\nПосле полной отписки:");
            Console.WriteLine($"Делегат null: {notifier == null}");
        }

        // Задание 32: Система уведомлений на основе групповых делегатов
        public static void Task32_NotificationSystem()
        {
            Console.WriteLine("\n=== Задание 32: Система уведомлений на основе групповых делегатов ===");

            var notificationManager = new NotificationManager();

            // Регистрация каналов уведомлений
            notificationManager.RegisterChannel("user_login",
                msg => Console.WriteLine($"[SECURITY] {msg}"));
            notificationManager.RegisterChannel("user_login",
                msg => Console.WriteLine($"[ANALYTICS] {msg}"));
            notificationManager.RegisterChannel("system_error",
                msg => Console.WriteLine($"[ALERT] {msg}"));

            // Отправка уведомлений
            notificationManager.Notify("user_login", "Пользователь John вошел в систему");
            notificationManager.Notify("system_error", "Критическая ошибка в модуле X");
        }

        // Задание 33: Делегат для обработки событий в цепочке обработчиков (упрощенная версия)
        public static void Task33_EventChainHandling()
        {
            Console.WriteLine("\n=== Задание 33: Делегат для обработки событий в цепочке обработчиков ===");

            Func<string, string> requestPipeline = null;

            requestPipeline += data =>
            {
                Console.WriteLine("1. Аутентификация данных");
                return data;
            };

            requestPipeline += data =>
            {
                Console.WriteLine("2. Валидация данных");
                return data;
            };

            requestPipeline += data =>
            {
                Console.WriteLine("3. Обработка бизнес-логики");
                return data + "_PROCESSED";
            };

            requestPipeline += data =>
            {
                Console.WriteLine("4. Логирование результата");
                return data;
            };

            var requestData = "test_data";
            var response = requestPipeline(requestData);

            Console.WriteLine($"Итоговый результат: {response}");
        }

        // Задание 34: Делегат для выполнения нескольких операций последовательно
        public static void Task34_SequentialOperations()
        {
            Console.WriteLine("\n=== Задание 34: Делегат для выполнения нескольких операций последовательно ===");

            Action sequentialOperations = null;

            sequentialOperations += () => Console.WriteLine("Операция 1: Инициализация");
            sequentialOperations += () => Console.WriteLine("Операция 2: Загрузка данных");
            sequentialOperations += () => Console.WriteLine("Операция 3: Обработка данных");
            sequentialOperations += () => Console.WriteLine("Операция 4: Сохранение результатов");
            sequentialOperations += () => Console.WriteLine("Операция 5: Очистка ресурсов");

            Console.WriteLine("Последовательное выполнение операций:");
            sequentialOperations();
        }

        // Задание 35: Делегат для вызова методов разных классов
        public static void Task35_MultiClassDelegate()
        {
            Console.WriteLine("\n=== Задание 35: Делегат для вызова методов разных классов ===");

            Action multiClassAction = null;

            var logger = new Logger();
            var validator = new Validator();
            var processor = new DataProcessor();

            multiClassAction += logger.LogStart;
            multiClassAction += validator.Validate;
            multiClassAction += processor.Process;
            multiClassAction += logger.LogEnd;

            Console.WriteLine("Вызов методов разных классов:");
            multiClassAction();
        }

        // Задание 36: Делегат для логирования, сохранения и вывода данных
        public static void Task36_LogSaveDisplayDelegate()
        {
            Console.WriteLine("\n=== Задание 36: Делегат для логирования, сохранения и вывода данных ===");

            Action<string> dataHandler = null;

            dataHandler += data => Console.WriteLine($"[DISPLAY] {data}");
            dataHandler += data => Console.WriteLine($"[LOG] {DateTime.Now}: {data}");
            dataHandler += data => Console.WriteLine($"[SAVE] Сохранено: {data}");

            dataHandler("Важные данные для обработки");
        }

        // Задание 37: Система подписки на события с множественными слушателями
        public static void Task37_MultiListenerSubscription()
        {
            Console.WriteLine("\n=== Задание 37: Система подписки на события с множественными слушателями ===");

            var publisher = new EventPublisher();

            // Подписчики
            var subscriber1 = new EventSubscriber("Subscriber 1");
            var subscriber2 = new EventSubscriber("Subscriber 2");
            var subscriber3 = new EventSubscriber("Subscriber 3");

            publisher.Subscribe(subscriber1.HandleEvent);
            publisher.Subscribe(subscriber2.HandleEvent);
            publisher.Subscribe(subscriber3.HandleEvent);

            publisher.Publish("Событие 1");
            publisher.Publish("Событие 2");

            // Отписка одного подписчика
            publisher.Unsubscribe(subscriber2.HandleEvent);
            publisher.Publish("Событие 3");
        }

        // Задание 38: Делегат для обработки исключений в цепочке вызовов
        public static void Task38_ExceptionHandlingChain()
        {
            Console.WriteLine("\n=== Задание 38: Делегат для обработки исключений в цепочке вызовов ===");

            Action<string> safeChain = null;

            safeChain += data =>
            {
                try
                {
                    Console.WriteLine($"Шаг 1: Обработка {data}");
                    if (data == "error") throw new Exception("Искусственная ошибка");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка на шаге 1: {ex.Message}");
                }
            };

            safeChain += data =>
            {
                try
                {
                    Console.WriteLine($"Шаг 2: Продолжение обработки {data}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка на шаге 2: {ex.Message}");
                }
            };

            safeChain += data =>
            {
                Console.WriteLine($"Шаг 3: Завершение обработки {data}");
            };

            Console.WriteLine("Нормальное выполнение:");
            safeChain("normal");

            Console.WriteLine("\nВыполнение с ошибкой:");
            safeChain("error");
        }

        // Задание 39: Делегат с проверкой результата каждого метода
        public static void Task39_ResultCheckingDelegate()
        {
            Console.WriteLine("\n=== Задание 39: Делегат с проверкой результата каждого метода ===");

            Func<int, bool> validationChain = null;

            validationChain += n =>
            {
                bool result = n > 0;
                Console.WriteLine($"Проверка 1 (n > 0): {result}");
                return result;
            };

            validationChain += n =>
            {
                bool result = n < 100;
                Console.WriteLine($"Проверка 2 (n < 100): {result}");
                return result;
            };

            validationChain += n =>
            {
                bool result = n % 2 == 0;
                Console.WriteLine($"Проверка 3 (n четное): {result}");
                return result;
            };

            int[] testNumbers = { 50, 150, -5, 75 };

            foreach (var number in testNumbers)
            {
                Console.WriteLine($"\nПроверка числа {number}:");
                bool finalResult = true;
                foreach (var validator in validationChain.GetInvocationList())
                {
                    if (!((Func<int, bool>)validator)(number))
                    {
                        finalResult = false;
                        break;
                    }
                }
                Console.WriteLine($"Финальный результат: {finalResult}");
            }
        }

        // Задание 40: Делегат для синхронизации нескольких операций
        public static void Task40_OperationSynchronization()
        {
            Console.WriteLine("\n=== Задание 40: Делегат для синхронизации нескольких операций ===");

            Action synchronizationDelegate = null;
            int completedOperations = 0;
            object lockObject = new object();

            Action operation1 = () =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Операция 1 завершена");
                lock (lockObject) completedOperations++;
            };

            Action operation2 = () =>
            {
                Thread.Sleep(1500);
                Console.WriteLine("Операция 2 завершена");
                lock (lockObject) completedOperations++;
            };

            Action operation3 = () =>
            {
                Thread.Sleep(800);
                Console.WriteLine("Операция 3 завершена");
                lock (lockObject) completedOperations++;
            };

            synchronizationDelegate += operation1;
            synchronizationDelegate += operation2;
            synchronizationDelegate += operation3;

            Console.WriteLine("Запуск параллельных операций...");
            synchronizationDelegate();

            // Ожидание завершения всех операций
            while (completedOperations < 3)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("Все операции завершены!");
        }

        // Задание 41: Делегат для обработки асинхронных операций по очереди
        public static void Task41_AsyncOperationsQueue()
        {
            Console.WriteLine("\n=== Задание 41: Делегат для обработки асинхронных операций по очереди ===");

            Func<Task> asyncQueue = null;

            asyncQueue += async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Асинхронная операция 1 завершена");
            };

            asyncQueue += async () =>
            {
                await Task.Delay(500);
                Console.WriteLine("Асинхронная операция 2 завершена");
            };

            asyncQueue += async () =>
            {
                await Task.Delay(800);
                Console.WriteLine("Асинхронная операция 3 завершена");
            };

            Console.WriteLine("Запуск асинхронной цепочки...");
            asyncQueue().Wait();
        }

        // Задание 42: Делегат для вызова методов с условием
        public static void Task42_ConditionalDelegate()
        {
            Console.WriteLine("\n=== Задание 42: Делегат для вызова методов с условием ===");

            Action<int> conditionalProcessor = null;

            conditionalProcessor += n =>
            {
                if (n > 0)
                    Console.WriteLine($"Положительное число: {n}");
            };

            conditionalProcessor += n =>
            {
                if (n % 2 == 0)
                    Console.WriteLine($"Четное число: {n}");
            };

            conditionalProcessor += n =>
            {
                if (n > 10)
                    Console.WriteLine($"Большое число: {n}");
            };

            int[] numbers = { -5, 3, 8, 15, 25 };

            foreach (var number in numbers)
            {
                Console.WriteLine($"\nОбработка числа {number}:");
                conditionalProcessor(number);
            }
        }

        // Задание 43: Делегат для накопления результатов (reduce/fold)
        public static void Task43_ReduceDelegate()
        {
            Console.WriteLine("\n=== Задание 43: Делегат для накопления результатов (reduce/fold) ===");

            Func<int, int, int> accumulator = (acc, current) => acc + current;

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            int sum = numbers.Aggregate(0, (acc, current) => accumulator(acc, current));

            Console.WriteLine($"Сумма чисел {string.Join(", ", numbers)} = {sum}");

            // Другие операции накопления
            Func<int, int, int> multiplier = (acc, current) => acc * current;
            int product = numbers.Aggregate(1, (acc, current) => multiplier(acc, current));
            Console.WriteLine($"Произведение чисел {string.Join(", ", numbers)} = {product}");
        }

        // Задание 44: Делегат для логирования каждого шага вычисления
        public static void Task44_StepLoggingDelegate()
        {
            Console.WriteLine("\n=== Задание 44: Делегат для логирования каждого шага вычисления ===");

            Func<int, int> computation = null;

            computation += n =>
            {
                Console.WriteLine($"Шаг 1: {n} * 2 = {n * 2}");
                return n * 2;
            };

            computation += n =>
            {
                Console.WriteLine($"Шаг 2: {n} + 10 = {n + 10}");
                return n + 10;
            };

            computation += n =>
            {
                Console.WriteLine($"Шаг 3: {n} / 3 = {n / 3}");
                return n / 3;
            };

            int result = computation(5);
            Console.WriteLine($"Финальный результат: {result}");
        }

        // Задание 45: Система плагинов на основе групповых делегатов
        public static void Task45_PluginSystemDelegate()
        {
            Console.WriteLine("\n=== Задание 45: Система плагинов на основе групповых делегатов ===");

            var pluginManager = new PluginManager();

            // Регистрация плагинов
            pluginManager.RegisterPlugin("text", data => $"Text Plugin: {data}");
            pluginManager.RegisterPlugin("upper", data => data.ToUpper());
            pluginManager.RegisterPlugin("reverse", data => new string(data.Reverse().ToArray()));
            pluginManager.RegisterPlugin("length", data => $"Length: {data.Length}");

            // Обработка данных через все плагины
            string result = pluginManager.ProcessData("hello world");
            Console.WriteLine($"Результат обработки: {result}");
        }

        // Задание 46: Делегат для выполнения валидации несколькими правилами
        public static void Task46_MultiRuleValidation()
        {
            Console.WriteLine("\n=== Задание 46: Делегат для выполнения валидации несколькими правилами ===");

            Predicate<string> validationPipeline = null;

            validationPipeline += s => !string.IsNullOrEmpty(s);
            validationPipeline += s => s.Length >= 5;
            validationPipeline += s => s.Any(char.IsDigit);
            validationPipeline += s => s.Any(char.IsUpper);

            string[] testStrings = { "abc", "password", "Pass123", "Weak", "StrongPass1" };

            foreach (var testString in testStrings)
            {
                bool isValid = true;
                foreach (var validator in validationPipeline.GetInvocationList())
                {
                    if (!((Predicate<string>)validator)(testString))
                    {
                        isValid = false;
                        break;
                    }
                }
                Console.WriteLine($"'{testString}': {(isValid ? "VALID" : "INVALID")}");
            }
        }

        // Задание 47: Делегат для отправки уведомлений различными каналами
        public static void Task47_MultiChannelNotification()
        {
            Console.WriteLine("\n=== Задание 47: Делегат для отправки уведомлений различными каналами ===");

            Action<string, string> notificationSystem = null;

            notificationSystem += (title, message) =>
                Console.WriteLine($"[EMAIL] Отправка: {title} - {message}");

            notificationSystem += (title, message) =>
                Console.WriteLine($"[SMS] Отправка: {title} - {message}");

            notificationSystem += (title, message) =>
                Console.WriteLine($"[PUSH] Отправка: {title} - {message}");

            notificationSystem += (title, message) =>
                Console.WriteLine($"[SLACK] Отправка: {title} - {message}");

            notificationSystem("Важное обновление", "Система будет обновлена завтра в 10:00");
        }

        // Задание 48: Делегат для мониторинга производительности методов
        public static void Task48_PerformanceMonitor()
        {
            Console.WriteLine("\n=== Задание 48: Делегат для мониторинга производительности методов ===");

            Action monitoredAction = null;

            monitoredAction += () => { Thread.Sleep(100); Console.WriteLine("Метод 1 выполнен"); };
            monitoredAction += () => { Thread.Sleep(200); Console.WriteLine("Метод 2 выполнен"); };
            monitoredAction += () => { Thread.Sleep(150); Console.WriteLine("Метод 3 выполнен"); };

            // Обертка для мониторинга производительности
            Action monitoredActionWithMetrics = () =>
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                monitoredAction();
                stopwatch.Stop();
                Console.WriteLine($"Общее время выполнения: {stopwatch.ElapsedMilliseconds}ms");
            };

            monitoredActionWithMetrics();
        }

        // Задание 49: Делегат для кеширования результатов с логированием
        public static void Task49_CachingWithLogging()
        {
            Console.WriteLine("\n=== Задание 49: Делегат для кеширования результатов с логированием ===");

            Func<int, int> expensiveOperation = n =>
            {
                Console.WriteLine($"Вычисление для {n}...");
                Thread.Sleep(1000);
                return n * n;
            };

            var cachedOperation = CreateCachedFunction(expensiveOperation);

            Console.WriteLine("Первый вызов (долгое вычисление):");
            Console.WriteLine($"Результат: {cachedOperation(5)}");

            Console.WriteLine("Второй вызов (из кеша):");
            Console.WriteLine($"Результат: {cachedOperation(5)}");

            Console.WriteLine("Вызов для нового значения:");
            Console.WriteLine($"Результат: {cachedOperation(10)}");
        }

        // Задание 50: Делегат для обработки очереди задач
        public static void Task50_TaskQueueDelegate()
        {
            Console.WriteLine("\n=== Задание 50: Делегат для обработки очереди задач ===");

            var taskQueue = new TaskQueue();
            int taskId = 1;

            // Добавление задач в очередь
            taskQueue.Enqueue(() => Console.WriteLine($"Задача {taskId++}: Отправка email"));
            taskQueue.Enqueue(() => Console.WriteLine($"Задача {taskId++}: Обновление базы данных"));
            taskQueue.Enqueue(() => Console.WriteLine($"Задача {taskId++}: Генерация отчета"));
            taskQueue.Enqueue(() => Console.WriteLine($"Задача {taskId++}: Очистка кеша"));

            // Обработка очереди
            taskQueue.ProcessQueue();
        }

        // Вспомогательные методы
        private static Func<T, TResult> CreateCachedFunction<T, TResult>(Func<T, TResult> func)
        {
            var cache = new Dictionary<T, TResult>();
            return key =>
            {
                if (cache.ContainsKey(key))
                {
                    Console.WriteLine($"[CACHE HIT] Для ключа {key}");
                    return cache[key];
                }
                Console.WriteLine($"[CACHE MISS] Для ключа {key}");
                var result = func(key);
                cache[key] = result;
                return result;
            };
        }
    }

    #endregion

    #region Раздел 3: Анонимные методы (Задания 51-75)

    public class AnonymousMethodsTasks
    {
        // Задание 51: Анонимный метод для простой операции
        public static void Task51_SimpleAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 51: Анонимный метод для простой операции ===");

            BinaryOperation add = delegate (int a, int b) { return a + b; };
            BinaryOperation multiply = delegate (int a, int b) { return a * b; };

            Console.WriteLine($"5 + 3 = {add(5, 3)}");
            Console.WriteLine($"5 * 3 = {multiply(5, 3)}");
        }

        // Задание 52: Анонимный метод для фильтрации коллекции
        public static void Task52_FilterAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 52: Анонимный метод для фильтрации коллекции ===");

            List<int> numbers = Enumerable.Range(1, 20).ToList();

            Predicate<int> isPrime = delegate (int n)
            {
                if (n < 2) return false;
                for (int i = 2; i <= Math.Sqrt(n); i++)
                    if (n % i == 0) return false;
                return true;
            };

            var primes = numbers.FindAll(isPrime);
            Console.WriteLine("Простые числа: " + string.Join(", ", primes));
        }

        // Задание 53: Анонимный метод для поиска элемента
        public static void Task53_SearchAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 53: Анонимный метод для поиска элемента ===");

            List<string> names = new List<string> { "Alice", "Bob", "Charlie", "Diana", "Eve" };

            Predicate<string> findLongName = delegate (string name)
            {
                return name.Length > 4;
            };

            var longNames = names.FindAll(findLongName);
            Console.WriteLine("Длинные имена: " + string.Join(", ", longNames));
        }

        // Задание 54: Анонимный метод с closure
        public static void Task54_ClosureAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 54: Анонимный метод с closure ===");

            int multiplier = 3;

            Func<int, int> multiply = delegate (int x)
            {
                return x * multiplier; // Захватывает переменную multiplier из внешней области
            };

            Console.WriteLine($"multiply(5) = {multiply(5)}");

            // Изменение захваченной переменной
            multiplier = 5;
            Console.WriteLine($"После изменения multiplier: multiply(5) = {multiply(5)}");
        }

        // Задание 55: Анонимный метод для обработки события
        public static void Task55_EventAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 55: Анонимный метод для обработки события ===");

            var button = new Button();

            // Анонимный метод как обработчик события
            button.OnClick += delegate (object sender, EventArgs e)
            {
                Console.WriteLine("Кнопка нажата! (анонимный обработчик)");
            };

            button.Click();
        }

        // Задание 56: Анонимный метод для преобразования данных
        public static void Task56_TransformAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 56: Анонимный метод для преобразования данных ===");

            Func<string, string> transformer = delegate (string input)
            {
                return input.ToUpper().Replace(" ", "_") + "_TRANSFORMED";
            };

            string result = transformer("hello world");
            Console.WriteLine($"Результат преобразования: {result}");
        }

        // Задание 57: Анонимный метод для валидации значения
        public static void Task57_ValidationAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 57: Анонимный метод для валидации значения ===");

            Predicate<int> validator = delegate (int value)
            {
                return value >= 0 && value <= 100;
            };

            int[] testValues = { -5, 50, 150, 75, 101 };

            foreach (var value in testValues)
            {
                Console.WriteLine($"{value}: {(validator(value) ? "VALID" : "INVALID")}");
            }
        }

        // Задание 58: Анонимный метод для отсортирования коллекции
        public static void Task58_SortAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 58: Анонимный метод для отсортирования коллекции ===");

            List<string> words = new List<string> { "banana", "apple", "cherry", "date" };

            Comparison<string> sorter = delegate (string a, string b)
            {
                return a.Length.CompareTo(b.Length);
            };

            words.Sort(sorter);
            Console.WriteLine("Слова, отсортированные по длине: " + string.Join(", ", words));
        }

        // Задание 59: Анонимный метод для логирования операций
        public static void Task59_LoggingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 59: Анонимный метод для логирования операций ===");

            Action<string> logger = delegate (string message)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            };

            logger("Приложение запущено");
            logger("Выполняется операция");
            logger("Операция завершена");
        }

        // Задание 60: Анонимный метод с параметром ref
        public static void Task60_RefParameterAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 60: Анонимный метод с параметром ref ===");

            int value = 10;

            // Для ref параметров нужно использовать делегат с явным указанием ref
            RefOperation increment = delegate (ref int x)
            {
                x++;
                Console.WriteLine($"Значение увеличено до: {x}");
            };

            Console.WriteLine($"Начальное значение: {value}");
            increment(ref value);
            Console.WriteLine($"Конечное значение: {value}");
        }

        // Задание 61: Анонимный метод для вычисления факториала
        public static void Task61_FactorialAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 61: Анонимный метод для вычисления факториала ===");

            Func<int, long> factorial = null;
            factorial = delegate (int n)
            {
                if (n <= 1) return 1;
                return n * factorial(n - 1);
            };

            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"Факториал {i} = {factorial(i)}");
            }
        }

        // Задание 62: Анонимный метод для работы со строками
        public static void Task62_StringAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 62: Анонимный метод для работы со строками ===");

            Func<string, string> stringProcessor = delegate (string input)
            {
                string result = input.Trim();
                result = result.ToUpper();
                result = result.Replace(" ", "-");
                return $"Processed: {result}";
            };

            string testString = "  hello world from csharp  ";
            Console.WriteLine($"Исходная строка: '{testString}'");
            Console.WriteLine($"Обработанная строка: '{stringProcessor(testString)}'");
        }

        // Задание 63: Анонимный метод для обработки исключений
        public static void Task63_ExceptionHandlingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 63: Анонимный метод для обработки исключений ===");

            Action dangerousOperation = delegate
            {
                try
                {
                    int zero = 0;
                    int result = 10 / zero;
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Поймано исключение: {ex.Message}");
                }
            };

            dangerousOperation();
        }

        // Задание 64: Анонимный метод для создания кешированного значения
        public static void Task64_CachingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 64: Анонимный метод для создания кешированного значения ===");

            var cache = new Dictionary<int, string>();
            Func<int, string> cachedValue = delegate (int key)
            {
                if (!cache.ContainsKey(key))
                {
                    Console.WriteLine($"Вычисление значения для ключа {key}");
                    cache[key] = $"Value_{key * 10}";
                }
                return cache[key];
            };

            Console.WriteLine($"Ключ 1: {cachedValue(1)}");
            Console.WriteLine($"Ключ 1 (из кеша): {cachedValue(1)}");
            Console.WriteLine($"Ключ 2: {cachedValue(2)}");
        }

        // Задание 65: Анонимный метод для работы с файловой системой
        public static void Task65_FileSystemAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 65: Анонимный метод для работы с файловой системой ===");

            Action<string> fileOperation = delegate (string path)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        string content = File.ReadAllText(path);
                        Console.WriteLine($"Содержимое файла: {content}");
                    }
                    else
                    {
                        Console.WriteLine("Файл не существует");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при работе с файлом: {ex.Message}");
                }
            };

            fileOperation("test.txt");
        }

        // Задание 66: Анонимный метод для парсинга данных
        public static void Task66_ParsingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 66: Анонимный метод для парсинга данных ===");

            Func<string, int?> parser = delegate (string input)
            {
                if (int.TryParse(input, out int result))
                    return result;
                return null;
            };

            string[] testInputs = { "123", "45.6", "abc", "789" };

            foreach (var input in testInputs)
            {
                var result = parser(input);
                Console.WriteLine($"'{input}' -> {(result.HasValue ? result.ToString() : "INVALID")}");
            }
        }

        // Задание 67: Анонимный метод для работы с базой данных
        public static void Task67_DatabaseAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 67: Анонимный метод для работы с базой данных ===");

            // Имитация работы с базой данных
            Action<string, Action<object>> databaseQuery = delegate (string query, Action<object> callback)
            {
                Console.WriteLine($"Выполнение запроса: {query}");
                Thread.Sleep(500);
                var result = new { Id = 1, Name = "Test User", Age = 30 };
                callback(result);
            };

            databaseQuery("SELECT * FROM Users", delegate (object result)
            {
                Console.WriteLine($"Получены данные: {result}");
            });
        }

        // Задание 68: Анонимный метод для асинхронных операций
        public static void Task68_AsyncAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 68: Анонимный метод для асинхронных операций ===");

            Action<string> asyncOperation = delegate (string message)
            {
                Task.Run(delegate
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Асинхронно: {message}");
                });
            };

            Console.WriteLine("Запуск асинхронной операции...");
            asyncOperation("Сообщение из анонимного метода");
            Thread.Sleep(1500);
        }

        // Задание 69: Анонимный метод для обхода коллекции
        public static void Task69_CollectionTraversalAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 69: Анонимный метод для обхода коллекции ===");

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            Action<List<int>, Action<int>> traverser = delegate (List<int> collection, Action<int> action)
            {
                foreach (var item in collection)
                {
                    action(item);
                }
            };

            Console.WriteLine("Обход коллекции:");
            traverser(numbers, delegate (int n)
            {
                Console.WriteLine($"Обработка элемента: {n}, квадрат: {n * n}");
            });
        }

        // Задание 70: Анонимный метод для группировки данных
        public static void Task70_GroupingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 70: Анонимный метод для группировки данных ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 25, 60000),
                new Person("Diana", 30, 55000)
            };

            Func<List<Person>, Dictionary<int, List<Person>>> grouper = delegate (List<Person> list)
            {
                return list.GroupBy(p => p.Age)
                          .ToDictionary(g => g.Key, g => g.ToList());
            };

            var grouped = grouper(people);
            foreach (var group in grouped)
            {
                Console.WriteLine($"Возраст {group.Key}: {string.Join(", ", group.Value.Select(p => p.Name))}");
            }
        }

        // Задание 71: Анонимный метод для вычисления суммы элементов
        public static void Task71_SumAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 71: Анонимный метод для вычисления суммы элементов ===");

            Func<List<int>, int> summer = delegate (List<int> numbers)
            {
                int sum = 0;
                foreach (var number in numbers)
                {
                    sum += number;
                }
                return sum;
            };

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Сумма чисел {string.Join(", ", numbers)} = {summer(numbers)}");
        }

        // Задание 72: Анонимный метод для работы с датами
        public static void Task72_DateAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 72: Анонимный метод для работы с датами ===");

            Func<DateTime, string> dateFormatter = delegate (DateTime date)
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            };

            Func<DateTime, DateTime, int> dateDifference = delegate (DateTime date1, DateTime date2)
            {
                return (int)(date2 - date1).TotalDays;
            };

            DateTime now = DateTime.Now;
            DateTime future = now.AddDays(10);

            Console.WriteLine($"Текущая дата: {dateFormatter(now)}");
            Console.WriteLine($"Будущая дата: {dateFormatter(future)}");
            Console.WriteLine($"Разница в днях: {dateDifference(now, future)}");
        }

        // Задание 73: Анонимный метод для шифрования данных
        public static void Task73_EncryptionAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 73: Анонимный метод для шифрования данных ===");

            Func<string, string> simpleEncrypt = delegate (string input)
            {
                char[] chars = input.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = (char)(chars[i] + 1);
                }
                return new string(chars);
            };

            Func<string, string> simpleDecrypt = delegate (string input)
            {
                char[] chars = input.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = (char)(chars[i] - 1);
                }
                return new string(chars);
            };

            string original = "Hello";
            string encrypted = simpleEncrypt(original);
            string decrypted = simpleDecrypt(encrypted);

            Console.WriteLine($"Оригинал: {original}");
            Console.WriteLine($"Зашифровано: {encrypted}");
            Console.WriteLine($"Расшифровано: {decrypted}");
        }

        // Задание 74: Анонимный метод для вывода отчетов
        public static void Task74_ReportAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 74: Анонимный метод для вывода отчетов ===");

            Action<List<Person>> reportGenerator = delegate (List<Person> people)
            {
                Console.WriteLine("=== ОТЧЕТ ===");
                Console.WriteLine($"Всего людей: {people.Count}");
                Console.WriteLine($"Средний возраст: {people.Average(p => p.Age):F1}");
                Console.WriteLine($"Общая зарплата: {people.Sum(p => p.Salary):C}");

                Console.WriteLine("\nДетали:");
                foreach (var person in people)
                {
                    Console.WriteLine($"  {person.Name} - {person.Age} лет - {person.Salary:C}");
                }
            };

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 35, 60000)
            };

            reportGenerator(people);
        }

        // Задание 75: Анонимный метод для системы логирования
        public static void Task75_LoggingAnonymousMethod()
        {
            Console.WriteLine("\n=== Задание 75: Анонимный метод для системы логирования ===");

            Action<string, string> logger = delegate (string level, string message)
            {
                Console.WriteLine($"[{level}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            };

            logger("INFO", "Приложение запущено");
            logger("ERROR", "Произошла ошибка");
            logger("DEBUG", "Отладочная информация");
        }

        // Делегат для ref параметров
        delegate void RefOperation(ref int x);
    }

    #endregion

    #region Раздел 4: Лямбда выражения (Задания 76-100)

    public class LambdaExpressionsTasks
    {
        // Задание 76: Простое лямбда выражение
        public static void Task76_SimpleLambda()
        {
            Console.WriteLine("\n=== Задание 76: Простое лямбда выражение ===");

            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) => x + y;
            Action<string> greet = name => Console.WriteLine($"Hello, {name}!");

            Console.WriteLine($"square(5) = {square(5)}");
            Console.WriteLine($"add(3, 4) = {add(3, 4)}");
            greet("World");
        }

        // Задание 77: Лямбда выражение для фильтрации списка
        public static void Task77_FilterLambda()
        {
            Console.WriteLine("\n=== Задание 77: Лямбда выражение для фильтрации списка ===");

            var numbers = Enumerable.Range(1, 20);
            var evenNumbers = numbers.Where(x => x % 2 == 0);
            var largeNumbers = numbers.Where(x => x > 10);
            var divisibleBy3 = numbers.Where(x => x % 3 == 0);

            Console.WriteLine("Четные: " + string.Join(", ", evenNumbers));
            Console.WriteLine("Больше 10: " + string.Join(", ", largeNumbers));
            Console.WriteLine("Делится на 3: " + string.Join(", ", divisibleBy3));
        }

        // Задание 78: Лямбда выражение для преобразования строк
        public static void Task78_StringTransformLambda()
        {
            Console.WriteLine("\n=== Задание 78: Лямбда выражение для преобразования строк ===");

            Func<string, string>[] transformers = {
                s => s.ToUpper(),
                s => s.ToLower(),
                s => new string(s.Reverse().ToArray()),
                s => string.Join("", s.Where(char.IsLetter)),
                s => s.Replace(" ", "_")
            };

            string testString = "Hello World 123!";

            foreach (var transformer in transformers)
            {
                Console.WriteLine($"{transformer(testString)}");
            }
        }

        // Задание 79: Лямбда выражение для работы с LINQ
        public static void Task79_LINQLambda()
        {
            Console.WriteLine("\n=== Задание 79: Лямбда выражение для работы с LINQ ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000),
                new Person("Diana", 28, 55000),
                new Person("Eve", 35, 70000)
            };

            // Where - фильтрация
            var youngPeople = people.Where(p => p.Age < 30);
            Console.WriteLine("Молодые люди (<30):");
            youngPeople.ToList().ForEach(p => Console.WriteLine($"  {p.Name}"));

            // Select - преобразование
            var names = people.Select(p => p.Name.ToUpper());
            Console.WriteLine("\nИмена в верхнем регистре: " + string.Join(", ", names));

            // OrderBy - сортировка
            var sortedByAge = people.OrderBy(p => p.Age);
            Console.WriteLine("\nСортировка по возрасту:");
            sortedByAge.ToList().ForEach(p => Console.WriteLine($"  {p.Name} - {p.Age}"));

            // Sum, Average - агрегация
            var totalSalary = people.Sum(p => p.Salary);
            var averageAge = people.Average(p => p.Age);
            Console.WriteLine($"\nОбщая зарплата: {totalSalary:C}");
            Console.WriteLine($"Средний возраст: {averageAge:F1}");
        }

        // Задание 80: Лямбда выражение с несколькими параметрами
        public static void Task80_MultiParameterLambda()
        {
            Console.WriteLine("\n=== Задание 80: Лямбда выражение с несколькими параметрами ===");

            Func<int, int, int> add = (x, y) => x + y;
            Func<int, int, int> multiply = (x, y) => x * y;
            Func<string, string, string> concat = (s1, s2) => s1 + " " + s2;
            Func<double, double, double, double> average = (a, b, c) => (a + b + c) / 3;

            Console.WriteLine($"add(5, 3) = {add(5, 3)}");
            Console.WriteLine($"multiply(4, 7) = {multiply(4, 7)}");
            Console.WriteLine($"concat('Hello', 'World') = {concat("Hello", "World")}");
            Console.WriteLine($"average(10, 20, 30) = {average(10, 20, 30)}");
        }

        // Задание 81: Лямбда выражение для фильтрации сложных объектов
        public static void Task81_ComplexFilterLambda()
        {
            Console.WriteLine("\n=== Задание 81: Лямбда выражение для фильтрации сложных объектов ===");

            var products = new List<Product>
            {
                new Product("Laptop", 999.99m, "Electronics"),
                new Product("Book", 19.99m, "Education"),
                new Product("Phone", 599.99m, "Electronics"),
                new Product("Pen", 1.99m, "Office"),
                new Product("Monitor", 299.99m, "Electronics")
            };

            var expensiveElectronics = products.Where(p =>
                p.Category == "Electronics" && p.Price > 500);

            var cheapProducts = products.Where(p => p.Price < 20);

            Console.WriteLine("Дорогая электроника:");
            expensiveElectronics.ToList().ForEach(p => Console.WriteLine($"  {p.Name} - {p.Price:C}"));

            Console.WriteLine("\nДешевые товары:");
            cheapProducts.ToList().ForEach(p => Console.WriteLine($"  {p.Name} - {p.Price:C}"));
        }

        // Задание 82: Лямбда выражение для группировки данных
        public static void Task82_GroupingLambda()
        {
            Console.WriteLine("\n=== Задание 82: Лямбда выражение для группировки данных ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 25, 60000),
                new Person("Diana", 30, 55000),
                new Person("Eve", 35, 70000)
            };

            var groupedByAge = people.GroupBy(p => p.Age);

            foreach (var group in groupedByAge)
            {
                Console.WriteLine($"Возраст {group.Key}:");
                foreach (var person in group)
                {
                    Console.WriteLine($"  {person.Name} - {person.Salary:C}");
                }
            }
        }

        // Задание 83: Лямбда выражение для проверки условия на элементах списка
        public static void Task83_ConditionCheckLambda()
        {
            Console.WriteLine("\n=== Задание 83: Лямбда выражение для проверки условия на элементах списка ===");

            var numbers = Enumerable.Range(1, 10).ToList();

            // All - все элементы удовлетворяют условию
            bool allEven = numbers.All(n => n % 2 == 0);
            bool allPositive = numbers.All(n => n > 0);

            // Any - хотя бы один элемент удовлетворяет условию
            bool anyOdd = numbers.Any(n => n % 2 != 0);
            bool anyGreaterThan5 = numbers.Any(n => n > 5);

            // Where - фильтрация по условию
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            var numbersGreaterThan5 = numbers.Where(n => n > 5);

            Console.WriteLine($"Все четные: {allEven}");
            Console.WriteLine($"Все положительные: {allPositive}");
            Console.WriteLine($"Есть нечетные: {anyOdd}");
            Console.WriteLine($"Есть больше 5: {anyGreaterThan5}");
            Console.WriteLine($"Четные числа: {string.Join(", ", evenNumbers)}");
            Console.WriteLine($"Числа > 5: {string.Join(", ", numbersGreaterThan5)}");
        }

        // Задание 84: Лямбда выражение для поиска минимума и максимума
        public static void Task84_MinMaxLambda()
        {
            Console.WriteLine("\n=== Задание 84: Лямбда выражение для поиска минимума и максимума ===");

            var numbers = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };

            int min = numbers.Min();
            int max = numbers.Max();
            double average = numbers.Average();

            // С использованием лямбда для сложных объектов
            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000)
            };

            int minAge = people.Min(p => p.Age);
            int maxAge = people.Max(p => p.Age);
            decimal maxSalary = people.Max(p => p.Salary);

            Console.WriteLine($"Числа: {string.Join(", ", numbers)}");
            Console.WriteLine($"Минимум: {min}, Максимум: {max}, Среднее: {average:F2}");
            Console.WriteLine($"Минимальный возраст: {minAge}, Максимальный возраст: {maxAge}");
            Console.WriteLine($"Максимальная зарплата: {maxSalary:C}");
        }

        // Задание 85: Лямбда выражение для сортировки по нескольким критериям
        public static void Task85_MultiCriteriaSortLambda()
        {
            Console.WriteLine("\n=== Задание 85: Лямбда выражение для сортировки по нескольким критериям ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 25, 60000),
                new Person("Diana", 30, 55000),
                new Person("Eve", 28, 48000)
            };

            // Сортировка по возрасту (по возрастанию), затем по зарплате (по убыванию)
            var sorted = people.OrderBy(p => p.Age)
                              .ThenByDescending(p => p.Salary)
                              .ToList();

            Console.WriteLine("Сортировка по возрасту (возрастание), затем по зарплате (убывание):");
            sorted.ForEach(p => Console.WriteLine($"  {p.Name} - {p.Age} лет - {p.Salary:C}"));
        }

        // Задание 86: Лямбда выражение для работы с датами и временем
        public static void Task86_DateTimeLambda()
        {
            Console.WriteLine("\n=== Задание 86: Лямбда выражение для работы с датами и временем ===");

            var dates = new List<DateTime>
            {
                new DateTime(2023, 1, 15),
                new DateTime(2023, 3, 20),
                new DateTime(2023, 2, 10),
                new DateTime(2023, 4, 5),
                new DateTime(2023, 1, 1)
            };

            // Фильтрация по месяцу
            var januaryDates = dates.Where(d => d.Month == 1);
            // Сортировка по дате
            var sortedDates = dates.OrderBy(d => d);
            // Форматирование дат
            var formattedDates = dates.Select(d => d.ToString("yyyy-MM-dd"));

            Console.WriteLine("Январские даты: " + string.Join(", ", januaryDates.Select(d => d.ToString("MM/dd"))));
            Console.WriteLine("Отсортированные даты: " + string.Join(", ", sortedDates.Select(d => d.ToString("MM/dd"))));
            Console.WriteLine("Форматированные даты: " + string.Join(", ", formattedDates));
        }

        // Задание 87: Лямбда выражение для работы с вложенными коллекциями
        public static void Task87_NestedCollectionsLambda()
        {
            Console.WriteLine("\n=== Задание 87: Лямбда выражение для работы с вложенными коллекциями ===");

            var departments = new List<Department>
            {
                new Department("IT", new List<Person>
                {
                    new Person("Alice", 25, 50000),
                    new Person("Bob", 30, 45000)
                }),
                new Department("HR", new List<Person>
                {
                    new Person("Charlie", 28, 40000),
                    new Person("Diana", 32, 42000)
                })
            };

            // SelectMany - flatten вложенные коллекции
            var allEmployees = departments.SelectMany(d => d.Employees);
            Console.WriteLine("Все сотрудники:");
            allEmployees.ToList().ForEach(e => Console.WriteLine($"  {e.Name} - {e.Department}"));

            // Группировка сотрудников по департаментам
            var employeesByDept = departments.SelectMany(d =>
                d.Employees.Select(e => new { e.Name, Department = d.Name }))
                .GroupBy(x => x.Department);

            foreach (var group in employeesByDept)
            {
                Console.WriteLine($"\n{group.Key}:");
                foreach (var employee in group)
                {
                    Console.WriteLine($"  {employee.Name}");
                }
            }
        }

        // Задание 88: Лямбда выражение для преобразования типов
        public static void Task88_TypeConversionLambda()
        {
            Console.WriteLine("\n=== Задание 88: Лямбда выражение для преобразования типов ===");

            var stringNumbers = new List<string> { "1", "2", "3", "4", "5" };

            // Преобразование строк в числа
            var numbers = stringNumbers.Select(s => int.Parse(s));
            var doubleNumbers = stringNumbers.Select(s => double.Parse(s));
            var nullableNumbers = stringNumbers.Select(s =>
                int.TryParse(s, out int result) ? (int?)result : null);

            Console.WriteLine($"Строки: {string.Join(", ", stringNumbers)}");
            Console.WriteLine($"Целые числа: {string.Join(", ", numbers)}");
            Console.WriteLine($"Вещественные числа: {string.Join(", ", doubleNumbers)}");
            Console.WriteLine($"Nullable числа: {string.Join(", ", nullableNumbers)}");
        }

        // Задание 89: Лямбда выражение для подсчета элементов
        public static void Task89_CountingLambda()
        {
            Console.WriteLine("\n=== Задание 89: Лямбда выражение для подсчета элементов ===");

            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var words = new List<string> { "apple", "banana", "cherry", "date", "elderberry" };

            // Подсчет по условию
            int evenCount = numbers.Count(n => n % 2 == 0);
            int longWordsCount = words.Count(w => w.Length > 5);
            int totalLetters = words.Sum(w => w.Length);

            Console.WriteLine($"Четных чисел: {evenCount}");
            Console.WriteLine($"Длинных слов (>5 букв): {longWordsCount}");
            Console.WriteLine($"Всего букв во всех словах: {totalLetters}");
        }

        // Задание 90: Лямбда выражение для работы с enum значениями
        public static void Task90_EnumLambda()
        {
            Console.WriteLine("\n=== Задание 90: Лямбда выражение для работы с enum значениями ===");

            var statuses = Enum.GetValues(typeof(Status)).Cast<Status>();

            // Фильтрация enum значений
            var activeStatuses = statuses.Where(s => s.ToString().Contains("Active"));
            var orderedStatuses = statuses.OrderBy(s => s.ToString());

            Console.WriteLine("Все статусы: " + string.Join(", ", statuses));
            Console.WriteLine("Активные статусы: " + string.Join(", ", activeStatuses));
            Console.WriteLine("Отсортированные статусы: " + string.Join(", ", orderedStatuses));
        }

        // Задание 91: Лямбда выражение для создания словаря из коллекции
        public static void Task91_DictionaryLambda()
        {
            Console.WriteLine("\n=== Задание 91: Лямбда выражение для создания словаря из коллекции ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000)
            };

            // Создание словаря: ключ - имя, значение - возраст
            var nameToAge = people.ToDictionary(p => p.Name, p => p.Age);

            // Создание словаря: ключ - имя, значение - весь объект
            var nameToPerson = people.ToDictionary(p => p.Name);

            // Группировка в Lookup
            var ageToNames = people.ToLookup(p => p.Age, p => p.Name);

            Console.WriteLine("Словарь имя->возраст:");
            foreach (var kvp in nameToAge)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} лет");
            }

            Console.WriteLine("\nLookup возраст->имена:");
            foreach (var group in ageToNames)
            {
                Console.WriteLine($"  {group.Key} лет: {string.Join(", ", group)}");
            }
        }

        // Задание 92: Лямбда выражение для фильтрации с несколькими условиями
        public static void Task92_MultiConditionFilterLambda()
        {
            Console.WriteLine("\n=== Задание 92: Лямбда выражение для фильтрации с несколькими условиями ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000),
                new Person("Diana", 28, 55000),
                new Person("Eve", 35, 40000)
            };

            // Сложное условие фильтрации
            var filtered = people.Where(p =>
                p.Age >= 25 &&
                p.Age <= 35 &&
                p.Salary > 45000 &&
                p.Name.StartsWith("A") || p.Name.StartsWith("C"));

            Console.WriteLine("Фильтр: возраст 25-35, зарплата > 45000, имя начинается на A или C");
            filtered.ToList().ForEach(p =>
                Console.WriteLine($"  {p.Name} - {p.Age} лет - {p.Salary:C}"));
        }

        // Задание 93: Лямбда выражение для рекурсивных операций
        public static void Task93_RecursiveLambda()
        {
            Console.WriteLine("\n=== Задание 93: Лямбда выражение для рекурсивных операций ===");

            // Рекурсивный факториал через лямбду
            Func<int, int> factorial = null;
            factorial = n => n <= 1 ? 1 : n * factorial(n - 1);

            // Рекурсивная сумма чисел
            Func<List<int>, int> recursiveSum = null;
            recursiveSum = list =>
                list.Count == 0 ? 0 : list[0] + recursiveSum(list.Skip(1).ToList());

            Console.WriteLine($"Факториал 5 = {factorial(5)}");
            Console.WriteLine($"Сумма [1,2,3,4,5] = {recursiveSum(new List<int> { 1, 2, 3, 4, 5 })}");
        }

        // Задание 94: Лямбда выражение для работы с исключениями
        public static void Task94_ExceptionHandlingLambda()
        {
            Console.WriteLine("\n=== Задание 94: Лямбда выражение для работы с исключениями ===");

            var numbers = new List<string> { "1", "2", "abc", "4", "5" };

            // Безопасное преобразование с обработкой исключений
            var safeNumbers = numbers.Select(s =>
            {
                try
                {
                    return int.Parse(s);
                }
                catch
                {
                    return -1; // Значение по умолчанию при ошибке
                }
            });

            // Альтернативный способ с TryParse
            var validNumbers = numbers
                .Select(s => new { Text = s, Success = int.TryParse(s, out int result), Value = result })
                .Where(x => x.Success)
                .Select(x => x.Value);

            Console.WriteLine($"Исходные данные: {string.Join(", ", numbers)}");
            Console.WriteLine($"Безопасное преобразование: {string.Join(", ", safeNumbers)}");
            Console.WriteLine($"Только валидные числа: {string.Join(", ", validNumbers)}");
        }

        // Задание 95: Лямбда выражение для параллельной обработки данных (PLINQ)
        public static void Task95_ParallelLambda()
        {
            Console.WriteLine("\n=== Задание 95: Лямбда выражение для параллельной обработки данных (PLINQ) ===");

            var numbers = Enumerable.Range(1, 1000000);

            // Последовательная обработка
            var sequentialTime = MeasureTime(() =>
            {
                var squares = numbers.Select(n => n * n).ToList();
            });

            // Параллельная обработка
            var parallelTime = MeasureTime(() =>
            {
                var squares = numbers.AsParallel().Select(n => n * n).ToList();
            });

            Console.WriteLine($"Последовательная обработка: {sequentialTime}ms");
            Console.WriteLine($"Параллельная обработка: {parallelTime}ms");
            Console.WriteLine($"Ускорение: {sequentialTime / (double)parallelTime:F2}x");
        }

        // Задание 96: Лямбда выражение для работы с Dictionary
        public static void Task96_DictionaryLambda()
        {
            Console.WriteLine("\n=== Задание 96: Лямбда выражение для работы с Dictionary ===");

            var dict = new Dictionary<string, int>
            {
                ["apple"] = 1,
                ["banana"] = 2,
                ["cherry"] = 3,
                ["date"] = 4
            };

            // Фильтрация словаря
            var filteredDict = dict.Where(kvp => kvp.Value > 2)
                                  .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Преобразование словаря
            var transformedDict = dict.ToDictionary(
                kvp => kvp.Key.ToUpper(),
                kvp => kvp.Value * 10);

            // Поиск по условию
            var foundKey = dict.First(kvp => kvp.Value == 3).Key;
            var maxValue = dict.Max(kvp => kvp.Value);

            Console.WriteLine("Исходный словарь: " + string.Join(", ", dict));
            Console.WriteLine("Отфильтрованный словарь: " + string.Join(", ", filteredDict));
            Console.WriteLine("Преобразованный словарь: " + string.Join(", ", transformedDict));
            Console.WriteLine($"Ключ со значением 3: {foundKey}");
            Console.WriteLine($"Максимальное значение: {maxValue}");
        }

        // Задание 97: Лямбда выражение для LINQ to SQL (эмуляция)
        public static void Task97_LinqToSqlLambda()
        {
            Console.WriteLine("\n=== Задание 97: Лямбда выражение для LINQ to SQL (эмуляция) ===");

            // Эмуляция базы данных
            var database = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000),
                new Person("Diana", 28, 55000)
            };

            // "SQL-like" запросы с лямбдами
            var query1 = database.Where(p => p.Age > 25)
                                .OrderBy(p => p.Name)
                                .Select(p => new { p.Name, p.Salary });

            var query2 = from p in database
                         where p.Salary > 50000
                         orderby p.Age descending
                         select new { p.Name, p.Age, p.Salary };

            Console.WriteLine("Запрос 1 (Age > 25, отсортировано по Name):");
            foreach (var item in query1)
            {
                Console.WriteLine($"  {item.Name} - {item.Salary:C}");
            }

            Console.WriteLine("\nЗапрос 2 (Salary > 50000, отсортировано по Age по убыванию):");
            foreach (var item in query2)
            {
                Console.WriteLine($"  {item.Name} - {item.Age} лет - {item.Salary:C}");
            }
        }

        // Задание 98: Лямбда выражение для Anonymous types
        public static void Task98_AnonymousTypesLambda()
        {
            Console.WriteLine("\n=== Задание 98: Лямбда выражение для Anonymous types ===");

            var people = new List<Person>
            {
                new Person("Alice", 25, 50000),
                new Person("Bob", 30, 45000),
                new Person("Charlie", 22, 60000)
            };

            // Создание анонимных типов
            var anonymousData = people.Select(p => new
            {
                UserName = p.Name.ToUpper(),
                UserAge = p.Age,
                AnnualSalary = p.Salary,
                MonthlySalary = p.Salary / 12,
                IsAdult = p.Age >= 18
            });

            Console.WriteLine("Анонимные типы:");
            foreach (var data in anonymousData)
            {
                Console.WriteLine($"  {data.UserName}, {data.UserAge} лет, " +
                                $"Годовая: {data.AnnualSalary:C}, " +
                                $"Месячная: {data.MonthlySalary:C}, " +
                                $"Взрослый: {data.IsAdult}");
            }

            // Группировка с анонимными типами
            var groupedData = people.GroupBy(p => p.Age / 10)
                                   .Select(g => new
                                   {
                                       AgeGroup = $"{g.Key * 10}-{g.Key * 10 + 9}",
                                       Count = g.Count(),
                                       AverageSalary = g.Average(p => p.Salary)
                                   });

            Console.WriteLine("\nГруппировка по возрастным группам:");
            foreach (var group in groupedData)
            {
                Console.WriteLine($"  {group.AgeGroup}: {group.Count} человек, " +
                                $"Средняя зарплата: {group.AverageSalary:C}");
            }
        }

        // Задание 99: Лямбда выражение для работы с файлами
        public static void Task99_FileOperationsLambda()
        {
            Console.WriteLine("\n=== Задание 99: Лямбда выражение для работы с файлами ===");

            // Создание тестовых файлов
            File.WriteAllText("test1.txt", "Hello World");
            File.WriteAllText("test2.txt", "CSharp Programming");
            File.WriteAllText("test3.txt", "Lambda Expressions");

            try
            {
                // Чтение всех текстовых файлов
                var txtFiles = Directory.GetFiles(".", "*.txt");

                var fileContents = txtFiles.Select(file => new
                {
                    FileName = Path.GetFileName(file),
                    Content = File.ReadAllText(file),
                    Size = new FileInfo(file).Length,
                    Lines = File.ReadAllLines(file).Length
                });

                Console.WriteLine("Файлы и их содержимое:");
                foreach (var file in fileContents)
                {
                    Console.WriteLine($"  {file.FileName} ({file.Size} bytes, {file.Lines} lines):");
                    Console.WriteLine($"    Содержимое: {file.Content}");
                }

                // Фильтрация файлов по размеру
                var largeFiles = txtFiles.Where(f => new FileInfo(f).Length > 10)
                                        .Select(f => Path.GetFileName(f));

                Console.WriteLine($"\nФайлы больше 10 bytes: {string.Join(", ", largeFiles)}");

            }
            finally
            {
                // Очистка
                foreach (var file in Directory.GetFiles(".", "test*.txt"))
                {
                    File.Delete(file);
                }
            }
        }

        // Задание 100: Лямбда выражение для Expression trees
        public static void Task100_ExpressionTreesLambda()
        {
            Console.WriteLine("\n=== Задание 100: Лямбда выражение для Expression trees ===");

            // Создание expression tree для x => x * x + 2 * x + 1
            Expression<Func<int, int>> expression = x => x * x + 2 * x + 1;

            // Компиляция expression tree в делегат
            Func<int, int> func = expression.Compile();

            // Вычисление значения
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine($"f({i}) = {func(i)}");
            }

            // Анализ expression tree
            Console.WriteLine("\nАнализ Expression Tree:");
            Console.WriteLine($"Тип: {expression.NodeType}");
            Console.WriteLine($"Тело: {expression.Body}");
            Console.WriteLine($"Параметры: {string.Join(", ", expression.Parameters.Select(p => p.Name))}");

            // Более сложный expression tree
            Expression<Func<int, int, bool>> complexExpression = (x, y) => x > y && x % 2 == 0;
            Func<int, int, bool> complexFunc = complexExpression.Compile();

            Console.WriteLine($"\nСложное выражение: {complexExpression}");
            Console.WriteLine($"complexFunc(10, 5) = {complexFunc(10, 5)}");
            Console.WriteLine($"complexFunc(7, 5) = {complexFunc(7, 5)}");
        }

        // Вспомогательный метод для измерения времени
        static long MeasureTime(Action action)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }

    #endregion

    #region Вспомогательные классы и перечисления

    public delegate int BinaryOperation(int a, int b);

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string Department { get; set; }

        public Person(string name, int age, decimal salary, string department = "")
        {
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
        }

        public override string ToString()
        {
            return $"{Name} (Age: {Age}, Salary: ${Salary})";
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public Product(string name, decimal price, string category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public override string ToString()
        {
            return $"{Name} - ${Price} ({Category})";
        }
    }

    public class TreeNode
    {
        public int Value { get; set; }
        public List<TreeNode> Children { get; set; }

        public TreeNode(int value)
        {
            Value = value;
            Children = new List<TreeNode>();
        }
    }

    public class Subject
    {
        public Action OnChange { get; set; }

        public void DoSomething()
        {
            Console.WriteLine("Subject: Выполняется действие...");
            OnChange?.Invoke();
        }
    }

    public class EventSource
    {
        public Action<string> OnEvent { get; set; }

        public void TriggerEvent(string message)
        {
            OnEvent?.Invoke(message);
        }
    }

    public class NotificationManager
    {
        private Dictionary<string, Action<string>> channels = new Dictionary<string, Action<string>>();

        public void RegisterChannel(string channelName, Action<string> handler)
        {
            if (!channels.ContainsKey(channelName))
                channels[channelName] = handler;
            else
                channels[channelName] += handler;
        }

        public void Notify(string channelName, string message)
        {
            if (channels.ContainsKey(channelName))
                channels[channelName](message);
        }
    }

    public class Request
    {
        public string User { get; set; }
        public string Data { get; set; }
    }

    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Data { get; set; }
    }

    public class Logger
    {
        public void LogStart() => Console.WriteLine("Logger: Начало операции");
        public void LogEnd() => Console.WriteLine("Logger: Конец операции");
    }

    public class Validator
    {
        public void Validate() => Console.WriteLine("Validator: Проверка данных");
    }

    public class DataProcessor
    {
        public void Process() => Console.WriteLine("DataProcessor: Обработка данных");
    }

    public class EventPublisher
    {
        private Action<string> subscribers = null;

        public void Subscribe(Action<string> handler)
        {
            subscribers += handler;
        }

        public void Unsubscribe(Action<string> handler)
        {
            subscribers -= handler;
        }

        public void Publish(string message)
        {
            Console.WriteLine($"EventPublisher: Публикация '{message}'");
            subscribers?.Invoke(message);
        }
    }

    public class EventSubscriber
    {
        public string Name { get; }

        public EventSubscriber(string name)
        {
            Name = name;
        }

        public void HandleEvent(string message)
        {
            Console.WriteLine($"{Name}: Получено сообщение - {message}");
        }
    }

    public class TaskQueue
    {
        private Queue<Action> tasks = new Queue<Action>();

        public void Enqueue(Action task)
        {
            tasks.Enqueue(task);
        }

        public void ProcessQueue()
        {
            while (tasks.Count > 0)
            {
                var task = tasks.Dequeue();
                task();
                Thread.Sleep(500); // Имитация обработки
            }
        }
    }

    public class Button
    {
        public event EventHandler OnClick;

        public void Click()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }

    public class PluginManager
    {
        private Action<string> plugins = null;

        public void RegisterPlugin(string name, Func<string, string> plugin)
        {
            plugins += input => plugin(input);
        }

        public string ProcessData(string data)
        {
            if (plugins == null) return data;

            string result = data;
            foreach (var plugin in plugins.GetInvocationList())
            {
                result = ((Func<string, string>)plugin)(result);
            }
            return result;
        }
    }

    public class Department
    {
        public string Name { get; set; }
        public List<Person> Employees { get; set; }

        public Department(string name, List<Person> employees)
        {
            Name = name;
            Employees = employees;
            foreach (var employee in employees)
            {
                employee.Department = name;
            }
        }
    }

    public enum Status
    {
        Active,
        Inactive,
        Pending,
        ActiveAndPending,
        Completed
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 ПОЛНАЯ РЕАЛИЗАЦИЯ 100 ЗАДАЧ ПО ДЕЛЕГАТАМ\n");

            // Раздел 1: Основные концепции делегатов
            Console.WriteLine("========== РАЗДЕЛ 1: ОСНОВНЫЕ КОНЦЕПЦИИ ДЕЛЕГАТОВ ==========");
            BasicDelegatesTasks.Task1_SimpleDelegate();
            BasicDelegatesTasks.Task2_ArithmeticDelegate();
            BasicDelegatesTasks.Task3_MessageDelegate();
            BasicDelegatesTasks.Task4_PredicateDelegate();
            BasicDelegatesTasks.Task5_ActionDelegate();
            BasicDelegatesTasks.Task6_FuncDelegate();
            BasicDelegatesTasks.Task7_FilterDelegate();
            BasicDelegatesTasks.Task8_CallbackDelegate();
            BasicDelegatesTasks.Task9_SortingDelegate();
            BasicDelegatesTasks.Task10_ErrorHandlerDelegate();
            BasicDelegatesTasks.Task11_StringTransformDelegate();
            BasicDelegatesTasks.Task12_SearchDelegate();
            BasicDelegatesTasks.Task13_ValidationDelegate();
            BasicDelegatesTasks.Task14_FormattingDelegate();
            BasicDelegatesTasks.Task15_ComparisonDelegate();
            BasicDelegatesTasks.Task16_ConversionDelegate();
            BasicDelegatesTasks.Task17_CollectionProcessorDelegate();
            BasicDelegatesTasks.Task18_MathFunctionDelegate();
            BasicDelegatesTasks.Task19_TreeTraversalDelegate();
            BasicDelegatesTasks.Task20_FactoryDelegate();
            BasicDelegatesTasks.Task21_CachingDelegate();
            BasicDelegatesTasks.Task22_LoggingDelegate();
            BasicDelegatesTasks.Task23_TimerDelegate();
            BasicDelegatesTasks.Task24_InputHandlerDelegate();
            BasicDelegatesTasks.Task25_ObserverPatternDelegate();

            // Раздел 2: Комбинированные делегаты
            Console.WriteLine("\n========== РАЗДЕЛ 2: КОМБИНИРОВАННЫЕ ДЕЛЕГАТЫ ==========");
            MulticastDelegatesTasks.Task26_DelegateAddition();
            MulticastDelegatesTasks.Task27_DelegateChaining();
            MulticastDelegatesTasks.Task28_MulticastDelegates();
            MulticastDelegatesTasks.Task29_MultipleHandlers();
            MulticastDelegatesTasks.Task30_LogAndEmailDelegate();
            MulticastDelegatesTasks.Task31_DelegateUnsubscription();
            MulticastDelegatesTasks.Task32_NotificationSystem();
            MulticastDelegatesTasks.Task33_EventChainHandling();
            MulticastDelegatesTasks.Task34_SequentialOperations();
            MulticastDelegatesTasks.Task35_MultiClassDelegate();
            MulticastDelegatesTasks.Task36_LogSaveDisplayDelegate();
            MulticastDelegatesTasks.Task37_MultiListenerSubscription();
            MulticastDelegatesTasks.Task38_ExceptionHandlingChain();
            MulticastDelegatesTasks.Task39_ResultCheckingDelegate();
            MulticastDelegatesTasks.Task40_OperationSynchronization();
            MulticastDelegatesTasks.Task41_AsyncOperationsQueue();
            MulticastDelegatesTasks.Task42_ConditionalDelegate();
            MulticastDelegatesTasks.Task43_ReduceDelegate();
            MulticastDelegatesTasks.Task44_StepLoggingDelegate();
            MulticastDelegatesTasks.Task45_PluginSystemDelegate();
            MulticastDelegatesTasks.Task46_MultiRuleValidation();
            MulticastDelegatesTasks.Task47_MultiChannelNotification();
            MulticastDelegatesTasks.Task48_PerformanceMonitor();
            MulticastDelegatesTasks.Task49_CachingWithLogging();
            MulticastDelegatesTasks.Task50_TaskQueueDelegate();

            // Раздел 3: Анонимные методы
            Console.WriteLine("\n========== РАЗДЕЛ 3: АНОНИМНЫЕ МЕТОДЫ ==========");
            AnonymousMethodsTasks.Task51_SimpleAnonymousMethod();
            AnonymousMethodsTasks.Task52_FilterAnonymousMethod();
            AnonymousMethodsTasks.Task53_SearchAnonymousMethod();
            AnonymousMethodsTasks.Task54_ClosureAnonymousMethod();
            AnonymousMethodsTasks.Task55_EventAnonymousMethod();
            AnonymousMethodsTasks.Task56_TransformAnonymousMethod();
            AnonymousMethodsTasks.Task57_ValidationAnonymousMethod();
            AnonymousMethodsTasks.Task58_SortAnonymousMethod();
            AnonymousMethodsTasks.Task59_LoggingAnonymousMethod();
            AnonymousMethodsTasks.Task60_RefParameterAnonymousMethod();
            AnonymousMethodsTasks.Task61_FactorialAnonymousMethod();
            AnonymousMethodsTasks.Task62_StringAnonymousMethod();
            AnonymousMethodsTasks.Task63_ExceptionHandlingAnonymousMethod();
            AnonymousMethodsTasks.Task64_CachingAnonymousMethod();
            AnonymousMethodsTasks.Task65_FileSystemAnonymousMethod();
            AnonymousMethodsTasks.Task66_ParsingAnonymousMethod();
            AnonymousMethodsTasks.Task67_DatabaseAnonymousMethod();
            AnonymousMethodsTasks.Task68_AsyncAnonymousMethod();
            AnonymousMethodsTasks.Task69_CollectionTraversalAnonymousMethod();
            AnonymousMethodsTasks.Task70_GroupingAnonymousMethod();
            AnonymousMethodsTasks.Task71_SumAnonymousMethod();
            AnonymousMethodsTasks.Task72_DateAnonymousMethod();
            AnonymousMethodsTasks.Task73_EncryptionAnonymousMethod();
            AnonymousMethodsTasks.Task74_ReportAnonymousMethod();
            AnonymousMethodsTasks.Task75_LoggingAnonymousMethod();

            // Раздел 4: Лямбда выражения
            Console.WriteLine("\n========== РАЗДЕЛ 4: ЛЯМБДА ВЫРАЖЕНИЯ ==========");
            LambdaExpressionsTasks.Task76_SimpleLambda();
            LambdaExpressionsTasks.Task77_FilterLambda();
            LambdaExpressionsTasks.Task78_StringTransformLambda();
            LambdaExpressionsTasks.Task79_LINQLambda();
            LambdaExpressionsTasks.Task80_MultiParameterLambda();
            LambdaExpressionsTasks.Task81_ComplexFilterLambda();
            LambdaExpressionsTasks.Task82_GroupingLambda();
            LambdaExpressionsTasks.Task83_ConditionCheckLambda();
            LambdaExpressionsTasks.Task84_MinMaxLambda();
            LambdaExpressionsTasks.Task85_MultiCriteriaSortLambda();
            LambdaExpressionsTasks.Task86_DateTimeLambda();
            LambdaExpressionsTasks.Task87_NestedCollectionsLambda();
            LambdaExpressionsTasks.Task88_TypeConversionLambda();
            LambdaExpressionsTasks.Task89_CountingLambda();
            LambdaExpressionsTasks.Task90_EnumLambda();
            LambdaExpressionsTasks.Task91_DictionaryLambda();
            LambdaExpressionsTasks.Task92_MultiConditionFilterLambda();
            LambdaExpressionsTasks.Task93_RecursiveLambda();
            LambdaExpressionsTasks.Task94_ExceptionHandlingLambda();
            LambdaExpressionsTasks.Task95_ParallelLambda();
            LambdaExpressionsTasks.Task96_DictionaryLambda();
            LambdaExpressionsTasks.Task97_LinqToSqlLambda();
            LambdaExpressionsTasks.Task98_AnonymousTypesLambda();
            LambdaExpressionsTasks.Task99_FileOperationsLambda();
            LambdaExpressionsTasks.Task100_ExpressionTreesLambda();

            Console.WriteLine("\nВСЕ 100 ЗАДАЧ ПО ДЕЛЕГАТАМ УСПЕШНО ВЫПОЛНЕНЫ!");
            Console.ReadLine();
        }
    }
}
