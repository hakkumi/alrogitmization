using System;
using System.Collections.Generic;
using System.Linq;

namespace CompleteGenericsVarianceNullableDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ПОЛНАЯ РЕАЛИЗАЦИЯ 100 ЗАДАЧ ПО GENERICS, VARIANCE, NULLABLE И NULL-COALESCING\n");

            // Демонстрация Generics
            Console.WriteLine("=== GENERICS ДЕМО (25 задач) ===");
            DemoGenerics();

            // Демонстрация Variance
            Console.WriteLine("\n=== VARIANCE ДЕМО (25 задач) ===");
            DemoVariance();

            // Демонстрация Nullable
            Console.WriteLine("\n=== NULLABLE ДЕМО (25 задач) ===");
            DemoNullable();

            // Демонстрация Null-coalescing
            Console.WriteLine("\n=== NULL-COALESCING ДЕМО (25 задач) ===");
            DemoNullCoalescing();

            Console.WriteLine("\n=== ВСЕ 100 ЗАДАЧ ВЫПОЛНЕНЫ! ===");
            Console.ReadLine();
        }

        static void DemoGenerics()
        {
            // Задание 1: Stack
            Console.WriteLine("1. Stack<T>:");
            var stack = new Stack<int>();
            stack.Push(1); stack.Push(2); stack.Push(3);
            stack.Display();
            Console.WriteLine($"Pop: {stack.Pop()}");
            stack.Display();

            // Задание 2: Queue
            Console.WriteLine("\n2. Queue<T>:");
            var queue = new Queue<string>();
            queue.Enqueue("A"); queue.Enqueue("B"); queue.Enqueue("C");
            queue.Display();
            Console.WriteLine($"Dequeue: {queue.Dequeue()}");
            queue.Display();

            // Задание 3: Array Search
            Console.WriteLine("\n3. ArraySearch.FindIndex<T>:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            int index = ArraySearch.FindIndex(numbers, 3);
            Console.WriteLine($"Индекс элемента 3: {index}");

            // Задание 4: Pair
            Console.WriteLine("\n4. Pair<T>:");
            var pair = new Pair<string>("Hello", "World");
            Console.WriteLine($"Pair: {pair}");
            pair.Swap();
            Console.WriteLine($"После Swap: {pair}");

            // Задание 5: Swap
            Console.WriteLine("\n5. SwapHelper.Swap<T>:");
            int x = 10, y = 20;
            Console.WriteLine($"До Swap: x={x}, y={y}");
            SwapHelper.Swap(ref x, ref y);
            Console.WriteLine($"После Swap: x={x}, y={y}");

            // Задание 6: Cache
            Console.WriteLine("\n6. Cache<TKey, TValue>:");
            var cache = new Cache<string, int>();
            cache.Set("one", 1);
            cache.Set("two", 2);
            cache.Display();

            // Задание 7: LinkedList
            Console.WriteLine("\n7. LinkedList<T>:");
            var linkedList = new LinkedList<int>();
            linkedList.Add(1); linkedList.Add(2); linkedList.Add(3);
            linkedList.Display();

            // Задание 8: Sort
            Console.WriteLine("\n8. SortHelper.BubbleSort<T>:");
            int[] arrayToSort = { 5, 2, 8, 1, 9 };
            SortHelper.BubbleSort(arrayToSort);
            Console.WriteLine($"Отсортированный массив: [{string.Join(", ", arrayToSort)}]");

            // Задание 9: Repository
            Console.WriteLine("\n9. IRepository<T>:");
            var userRepo = new Repository<User>();
            userRepo.Add(new User { Name = "Alice" });
            userRepo.Add(new User { Name = "Bob" });
            Console.WriteLine($"Пользователь с ID 1: {userRepo.GetById(1)?.Name}");

            // Задание 10: Matrix
            Console.WriteLine("\n10. Matrix<T>:");
            var matrix = new Matrix<int>(2, 3);
            matrix[0, 0] = 1; matrix[0, 1] = 2; matrix[0, 2] = 3;
            matrix[1, 0] = 4; matrix[1, 1] = 5; matrix[1, 2] = 6;
            matrix.Display();

            // Задание 15: MinMax
            Console.WriteLine("\n15. MinMaxFinder.FindMinMax<T>:");
            var (min, max) = MinMaxFinder.FindMinMax(new[] { 5, 2, 8, 1, 9 });
            Console.WriteLine($"Min: {min}, Max: {max}");
        }

        static void DemoVariance()
        {
            // Задание 28: Ковариантность с IEnumerable
            Console.WriteLine("28. Ковариантность IEnumerable:");
            IEnumerable<Dog> dogs = new List<Dog> { new Dog { Name = "Rex" }, new Dog { Name = "Buddy" } };
            IEnumerable<Animal> animals = dogs;
            foreach (var animal1 in animals)
                Console.WriteLine($"  {animal1.Name}");

            // Задание 29: Контрвариантность с Action
            Console.WriteLine("\n29. Контрвариантность Action:");
            Action<Animal> animalAction = (animal) => Console.WriteLine($"Обработано: {animal.Name}");
            Action<Dog> dogAction = animalAction;
            dogAction(new Dog { Name = "Max" });

            // Задание 30: Ковариантность с Func
            Console.WriteLine("\n30. Ковариантность Func:");
            Func<Dog> dogFactory = () => new Dog { Name = "Собака из фабрики" };
            Func<Animal> animalFactory = dogFactory;
            var createdAnimal = animalFactory();
            Console.WriteLine($"Создано: {createdAnimal.Name}");

            // Задание 31: Иерархия классов
            Console.WriteLine("\n31. Иерархия Animal->Dog->Puppy:");
            Animal animal = new Animal { Name = "Животное" };
            Dog dog = new Dog { Name = "Собака" };
            Puppy puppy = new Puppy { Name = "Щенок" };
            animal.MakeSound();
            dog.MakeSound();
            puppy.MakeSound();

            // Задание 32: Ошибки вариантности
            Console.WriteLine("\n32. Ошибки вариантности:");
            VarianceViolationDemo.ShowCompilationErrorExample();

            // Задание 35: Ковариантность коллекций
            Console.WriteLine("\n35. Ковариантность коллекций:");
            CollectionVarianceDemo.Demo();

            // Задание 39: Безопасное приведение
            Console.WriteLine("\n39. Безопасное приведение:");
            SafeCastingDemo.ProcessAsAnimal(new Dog { Name = "SafeDog" });

            // Задание 46: Вариантность делегатов
            Console.WriteLine("\n46. Вариантность делегатов:");
            DelegateVarianceDemo.Demo();
        }

        static void DemoNullable()
        {
            // Задание 51: Nullable int
            Console.WriteLine("51. Nullable int:");
            NullableIntDemo.Demo();

            // Задание 59: Nullable в свойствах
            Console.WriteLine("\n59. UserProfile с nullable:");
            var user = new UserProfile { Username = "john_doe", Age = null, Salary = 50000 };
            Console.WriteLine(user.GetProfileInfo());

            // Задание 60: Валидация
            Console.WriteLine("\n60. Валидация nullable:");
            var (isValid, error) = DataValidator.ValidateAge(25);
            Console.WriteLine($"Валидация возраста 25: {isValid}, {error}");

            // Задание 63: Преобразование null в default
            Console.WriteLine("\n63. NullToDefaultConverter:");
            NullToDefaultConverter.Demo();

            // Задание 67: Nullable в LINQ
            Console.WriteLine("\n67. Nullable в LINQ:");
            NullableLinqDemo.Demo();

            // Задание 68: Обработка исключений
            Console.WriteLine("\n68. Обработка исключений nullable:");
            int? nullValue = null;
            int result = NullableExceptionHandler.SafeGetValue(nullValue, "Custom error message");
            Console.WriteLine($"Результат с обработкой исключения: {result}");
        }

        static void DemoNullCoalescing()
        {
            // Задание 76: ?? для строк
            Console.WriteLine("76. ?? для строк:");
            StringNullCoalescing.Demo();

            // Задание 80: ??= оператор
            Console.WriteLine("\n80. ??= оператор:");
            NullCoalescingAssignment.Demo();

            // Задание 83: ?? с коллекциями
            Console.WriteLine("\n83. ?? с коллекциями:");
            CollectionNullCoalescing.Demo();

            // Задание 84: ?? в LINQ
            Console.WriteLine("\n84. ?? в LINQ:");
            LinqNullCoalescing.Demo();

            // Задание 89: Null-safe navigation
            Console.WriteLine("\n89. Null-safe navigation:");
            var company = new Company { Department = new Department { Manager = new Employee { Name = "Alice" } } };
            Console.WriteLine($"Менеджер: {NullSafeNavigation.GetDeepValue(company)}");
            var emptyCompany = new Company();
            Console.WriteLine($"Менеджер (нет отдела): {NullSafeNavigation.GetDeepValue(emptyCompany)}");

            // Задание 92: ?? с делегатами
            Console.WriteLine("\n92. ?? с делегатами:");
            DelegateNullCoalescing.SafeInvoke(() => Console.WriteLine("Делегат выполнен!"));
            Action nullAction = null;
            DelegateNullCoalescing.SafeInvoke(nullAction);

            // Задание 94: ?? с конфигурацией
            Console.WriteLine("\n94. ?? с конфигурацией:");
            ConfigurationNullCoalescing.Demo();
        }
    }

    // ==================== РАЗДЕЛ 1: GENERICS (25 задач) ====================

    // Задание 1: Generic класс Stack
    public class Stack<T>
    {
        private List<T> _items = new List<T>();

        public void Push(T item)
        {
            _items.Add(item);
            Console.WriteLine($"Добавлен: {item}");
        }

        public T Pop()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Стек пуст");
            var item = _items[^1];
            _items.RemoveAt(_items.Count - 1);
            Console.WriteLine($"Извлечен: {item}");
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Стек пуст");
            return _items[^1];
        }

        public int Count => _items.Count;

        public void Display() => Console.WriteLine($"Стек: [{string.Join(", ", _items)}]");
    }

    // Задание 2: Generic класс Queue
    public class Queue<T>
    {
        private List<T> _items = new List<T>();

        public void Enqueue(T item)
        {
            _items.Add(item);
            Console.WriteLine($"Добавлен в очередь: {item}");
        }

        public T Dequeue()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Очередь пуста");
            var item = _items[0];
            _items.RemoveAt(0);
            Console.WriteLine($"Извлечен из очереди: {item}");
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Очередь пуста");
            return _items[0];
        }

        public int Count => _items.Count;

        public void Display() => Console.WriteLine($"Очередь: [{string.Join(", ", _items)}]");
    }

    // Задание 3: Generic метод для поиска в массиве
    public static class ArraySearch
    {
        public static int FindIndex<T>(T[] array, T element) where T : IEquatable<T>
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(element)) return i;
            return -1;
        }
    }

    // Задание 4: Generic класс Pair
    public class Pair<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public override string ToString() => $"({First}, {Second})";

        public void Swap() => (First, Second) = (Second, First);
    }

    // Задание 5: Generic метод для обмена значений
    public static class SwapHelper
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }

    // Задание 6: Generic класс Cache
    public class Cache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _storage = new Dictionary<TKey, TValue>();

        public void Set(TKey key, TValue value) => _storage[key] = value;

        public TValue Get(TKey key) => _storage.TryGetValue(key, out TValue value) ? value : default;

        public bool Contains(TKey key) => _storage.ContainsKey(key);

        public void Display()
        {
            Console.WriteLine("Cache contents:");
            foreach (var kvp in _storage)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
        }
    }

    // Задание 7: Generic класс LinkedList
    public class LinkedList<T>
    {
        public class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node(T data) => Data = data;
        }

        public Node Head { get; private set; }

        public void Add(T data)
        {
            var newNode = new Node(data);
            if (Head == null) Head = newNode;
            else
            {
                var current = Head;
                while (current.Next != null) current = current.Next;
                current.Next = newNode;
            }
        }

        public void Display()
        {
            var current = Head;
            var items = new List<T>();
            while (current != null)
            {
                items.Add(current.Data);
                current = current.Next;
            }
            Console.WriteLine($"Список: [{string.Join(" -> ", items)}]");
        }
    }

    // Задание 8: Generic метод для сортировки
    public static class SortHelper
    {
        public static void BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Length - 1; i++)
                for (int j = 0; j < array.Length - i - 1; j++)
                    if (array[j].CompareTo(array[j + 1]) > 0)
                        Swap(ref array[j], ref array[j + 1]);
        }

        private static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
    }

    // Задание 9: Generic интерфейс IRepository
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(int id);
    }

    // Базовый класс с Id для репозитория
    public abstract class Entity
    {
        public int Id { get; set; }
    }

    public class User : Entity
    {
        public string Name { get; set; }
        public override string ToString() => $"User: {Name} (ID: {Id})";
    }

    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        private List<T> _entities = new List<T>();
        private int _nextId = 1;

        public void Add(T entity)
        {
            entity.Id = _nextId++;
            _entities.Add(entity);
        }

        public T GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);

        public IEnumerable<T> GetAll() => _entities;

        public void Update(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                var index = _entities.IndexOf(existing);
                _entities[index] = entity;
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                _entities.Remove(entity);
        }
    }

    // Задание 10: Generic класс Matrix
    public class Matrix<T>
    {
        private T[,] _matrix;
        public int Rows { get; }
        public int Columns { get; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _matrix = new T[rows, columns];
        }

        public T this[int row, int col]
        {
            get => _matrix[row, col];
            set => _matrix[row, col] = value;
        }

        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    Console.Write($"{_matrix[i, j]}\t");
                Console.WriteLine();
            }
        }
    }

    // Задание 11: Generic метод для конвертации коллекции
    public static class CollectionConverter
    {
        public static List<TOut> Convert<TIn, TOut>(IEnumerable<TIn> collection, Func<TIn, TOut> converter)
            => collection.Select(converter).ToList();
    }

    // Задание 12: Generic класс History
    public class History<T>
    {
        private List<T> _history = new List<T>();
        private List<T> _future = new List<T>();

        public void Add(T item)
        {
            _history.Add(item);
            _future.Clear();
        }

        public T Undo()
        {
            if (_history.Count == 0) return default;
            var item = _history[^1];
            _history.RemoveAt(_history.Count - 1);
            _future.Insert(0, item);
            return item;
        }

        public T Redo()
        {
            if (_future.Count == 0) return default;
            var item = _future[0];
            _future.RemoveAt(0);
            _history.Add(item);
            return item;
        }

        public void Display()
        {
            Console.WriteLine($"История: [{string.Join(", ", _history)}]");
            Console.WriteLine($"Будущее: [{string.Join(", ", _future)}]");
        }
    }

    // Задание 13: Generic метод для фильтрации
    public static class FilterHelper
    {
        public static IEnumerable<T> Filter<T>(IEnumerable<T> collection, Predicate<T> predicate)
            => collection.Where(item => predicate(item));
    }

    // Задание 14: Generic класс Graph
    public class Graph<T> where T : IEquatable<T>
    {
        private Dictionary<T, List<T>> _adjacencyList = new Dictionary<T, List<T>>();

        public void AddVertex(T vertex) => _adjacencyList[vertex] = new List<T>();

        public void AddEdge(T source, T destination)
        {
            if (!_adjacencyList.ContainsKey(source)) AddVertex(source);
            if (!_adjacencyList.ContainsKey(destination)) AddVertex(destination);
            _adjacencyList[source].Add(destination);
            _adjacencyList[destination].Add(source);
        }

        public void Display()
        {
            foreach (var vertex in _adjacencyList)
            {
                Console.WriteLine($"{vertex.Key}: [{string.Join(", ", vertex.Value)}]");
            }
        }
    }

    // Задание 15: Generic метод для поиска минимума и максимума
    public static class MinMaxFinder
    {
        public static (T min, T max) FindMinMax<T>(IEnumerable<T> collection) where T : IComparable<T>
        {
            if (!collection.Any()) throw new ArgumentException("Коллекция пуста");

            T min = collection.First();
            T max = collection.First();

            foreach (var item in collection)
            {
                if (item.CompareTo(min) < 0) min = item;
                if (item.CompareTo(max) > 0) max = item;
            }

            return (min, max);
        }
    }

    // Задание 16: Generic класс Container с ограничениями
    public class Container<T> where T : class, new()
    {
        private T _item = new T();

        public T Item
        {
            get => _item;
            set => _item = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Reset() => _item = new T();

        public void Display() => Console.WriteLine($"Container: {_item}");
    }

    // Задание 17: Generic метод для обхода дерева
    public class TreeNode<T>
    {
        public T Data { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();

        public TreeNode(T data) => Data = data;

        public void AddChild(TreeNode<T> child) => Children.Add(child);
    }

    public static class TreeTraversal
    {
        public static void Traverse<T>(TreeNode<T> root, Action<T> action)
        {
            if (root == null) return;

            action(root.Data);
            foreach (var child in root.Children)
                Traverse(child, action);
        }

        public static void DisplayTree<T>(TreeNode<T> root, int level = 0)
        {
            if (root == null) return;

            Console.WriteLine($"{new string(' ', level * 2)}{root.Data}");
            foreach (var child in root.Children)
                DisplayTree(child, level + 1);
        }
    }

    // Задание 18: Generic интерфейс IComparer
    public interface IGenericComparer<T>
    {
        int Compare(T x, T y);
    }

    public class IntComparer : IGenericComparer<int>
    {
        public int Compare(int x, int y) => x.CompareTo(y);
    }

    // Задание 19: Generic класс EventDispatcher
    public class EventDispatcher<T>
    {
        private event Action<T> OnEvent;

        public void Subscribe(Action<T> handler) => OnEvent += handler;

        public void Unsubscribe(Action<T> handler) => OnEvent -= handler;

        public void Dispatch(T eventData) => OnEvent?.Invoke(eventData);
    }

    // Задание 20: Generic метод для клонирования
    public static class CloneHelper
    {
        public static T Clone<T>(T obj) where T : ICloneable => (T)obj.Clone();
    }

    // Задание 21: Generic класс Converter
    public class Converter<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _conversionFunction;

        public Converter(Func<TIn, TOut> conversionFunction) => _conversionFunction = conversionFunction;

        public TOut Convert(TIn input) => _conversionFunction(input);
    }

    // Задание 22: Generic метод для валидации
    public static class Validator
    {
        public static bool Validate<T>(T value, Predicate<T> validationRule) => validationRule(value);
    }

    // Задание 23: Generic класс Handler
    public class Handler<TRequest, TResponse>
    {
        private readonly Func<TRequest, TResponse> _handleFunction;

        public Handler(Func<TRequest, TResponse> handleFunction) => _handleFunction = handleFunction;

        public TResponse Handle(TRequest request) => _handleFunction(request);
    }

    // Задание 24: Generic метод для группировки по типам
    public static class TypeGrouper
    {
        public static Dictionary<Type, List<object>> GroupByType(IEnumerable<object> items)
            => items.GroupBy(item => item.GetType())
                   .ToDictionary(g => g.Key, g => g.ToList());
    }

    // Задание 25: Generic класс Pipeline
    public class Pipeline<T>
    {
        private List<Func<T, T>> _stages = new List<Func<T, T>>();

        public Pipeline<T> AddStage(Func<T, T> stage)
        {
            _stages.Add(stage);
            return this;
        }

        public T Execute(T input)
        {
            T result = input;
            foreach (var stage in _stages)
                result = stage(result);
            return result;
        }
    }

    // ==================== РАЗДЕЛ 2: VARIANCE (25 задач) ====================

    // Базовые классы для иерархии
    public class Animal
    {
        public string Name { get; set; }
        public virtual void MakeSound() => Console.WriteLine("Звук животного");
        public override string ToString() => Name;
    }

    public class Dog : Animal
    {
        public override void MakeSound() => Console.WriteLine("Гав! Гав!");
    }

    public class Puppy : Dog
    {
        public override void MakeSound() => Console.WriteLine("Пищик! Пищик!");
    }

    // Задание 26: Ковариантный интерфейс IProducer
    public interface IProducer<out T>
    {
        T Produce();
    }

    public class AnimalProducer : IProducer<Animal>
    {
        public Animal Produce() => new Animal { Name = "Generic Animal" };
    }

    public class DogProducer : IProducer<Dog>
    {
        public Dog Produce() => new Dog { Name = "Buddy" };
    }

    // Задание 27: Контрвариантный интерфейс IConsumer
    public interface IConsumer<in T>
    {
        void Consume(T item);
    }

    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal animal)
        {
            Console.WriteLine($"Потребление: {animal.Name}");
            animal.MakeSound();
        }
    }

    // Задание 32: Пример ошибки при нарушении правил вариантности
    public class VarianceViolationDemo
    {
        public static void ShowCompilationErrorExample()
        {
            Console.WriteLine("Следующий код вызовет ошибки компиляции:");
            Console.WriteLine("List<Dog> dogs = new List<Dog>();");
            Console.WriteLine("List<Animal> animals = dogs; // CS0029");
            Console.WriteLine("IList<Animal> animals = dogs; // CS0266");
            Console.WriteLine("Ковариантность работает только для интерфейсов с out параметрами");
        }
    }

    // Задание 33: Generic интерфейс с ковариантными параметрами
    public interface IReadOnlyRepository<out T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
    }

    public class AnimalRepository : IReadOnlyRepository<Animal>
    {
        private List<Animal> _animals = new List<Animal>
        {
            new Animal { Name = "Animal1" },
            new Dog { Name = "Dog1" },
            new Puppy { Name = "Puppy1" }
        };

        public Animal GetById(int id) => id < _animals.Count ? _animals[id] : null;
        public IEnumerable<Animal> GetAll() => _animals;
    }

    // Задание 34: Контрвариантный компаратор
    public interface IGenericComparerContravariant<in T>
    {
        int Compare(T x, T y);
    }

    public class AnimalComparer : IGenericComparerContravariant<Animal>
    {
        public int Compare(Animal x, Animal y) => string.Compare(x?.Name, y?.Name);
    }

    // Задание 35: Пример ковариантности при работе с коллекциями
    public class CollectionVarianceDemo
    {
        public static void Demo()
        {
            List<Dog> dogs = new List<Dog> { new Dog { Name = "Rex" }, new Dog { Name = "Buddy" } };
            List<Puppy> puppies = new List<Puppy> { new Puppy { Name = "Max" }, new Puppy { Name = "Charlie" } };

            IEnumerable<Animal> animalsFromDogs = dogs;
            IEnumerable<Animal> animalsFromPuppies = puppies;

            Console.WriteLine("Животные из собак:");
            foreach (var animal in animalsFromDogs)
                Console.WriteLine($"  - {animal.Name}");

            Console.WriteLine("Животные из щенков:");
            foreach (var animal in animalsFromPuppies)
                Console.WriteLine($"  - {animal.Name}");
        }
    }

    // Задание 36: Generic класс с поддержкой вариантности
    public class VariantWrapper<T> : IProducer<T>, IConsumer<T>
    {
        private T _value;
        public T Produce() => _value;
        public void Consume(T item) => _value = item;
    }

    // Задание 37: Ковариантный Factory
    public interface IAnimalFactory<out T> where T : Animal
    {
        T Create();
    }

    public class DogFactory : IAnimalFactory<Dog>
    {
        public Dog Create() => new Dog { Name = "Собака из фабрики" };
    }

    // Задание 38: Контрвариантный обработчик EventHandler
    public interface IEventHandler<in TEvent>
    {
        void Handle(TEvent @event);
    }

    public class AnimalEventHandler : IEventHandler<Animal>
    {
        public void Handle(Animal @event)
        {
            Console.WriteLine($"Обработка события: {@event.Name}");
        }
    }

    // Задание 39: Пример безопасного приведения типов
    public class SafeCastingDemo
    {
        public static void ProcessAsAnimal<T>(T animal) where T : Animal
        {
            Animal baseAnimal = animal;
            Console.WriteLine($"Безопасно приведено к Animal: {baseAnimal.Name}");
        }
    }

    // Задание 40: Интерфейс Repository с ковариантностью
    public interface ICovariantRepository<out T> where T : Animal
    {
        T Get(int id);
        IEnumerable<T> GetAll();
    }

    public class CovariantAnimalRepository : ICovariantRepository<Animal>
    {
        private List<Animal> _animals = new List<Animal>
        {
            new Animal { Name = "CovariantAnimal1" },
            new Dog { Name = "CovariantDog1" }
        };

        public Animal Get(int id) => id < _animals.Count ? _animals[id] : null;
        public IEnumerable<Animal> GetAll() => _animals;
    }

    // Задание 41: Контрвариантный validator
    public interface IValidatorContravariant<in T>
    {
        bool Validate(T item);
    }

    public class AnimalValidator : IValidatorContravariant<Animal>
    {
        public bool Validate(Animal item) => item != null && !string.IsNullOrEmpty(item.Name);
    }

    // Задание 42: Комбинирование ковариантности и контрвариантности
    public interface IProcessor<in TInput, out TOutput>
    {
        TOutput Process(TInput input);
    }

    public class AnimalProcessor : IProcessor<Dog, Animal>
    {
        public Animal Process(Dog input) => new Animal { Name = $"Обработанный {input.Name}" };
    }

    // Задание 43: Generic метод с ограничениями для вариантности
    public class VarianceConstraints
    {
        public static void ProcessProducers<T>(IEnumerable<IProducer<T>> producers) where T : Animal
        {
            foreach (var producer in producers)
            {
                var product = producer.Produce();
                Console.WriteLine($"  Произведено: {product.Name}");
            }
        }
    }

    // Задание 44: Ковариантный интерфейс для итераторов
    public interface IIterator<out T>
    {
        T Current { get; }
        bool MoveNext();
        void Reset();
    }

    public class AnimalIterator : IIterator<Animal>
    {
        private List<Animal> _animals;
        private int _position = -1;

        public AnimalIterator(IEnumerable<Animal> animals)
        {
            _animals = animals.ToList();
        }

        public Animal Current => _position >= 0 && _position < _animals.Count ? _animals[_position] : null;
        public bool MoveNext()
        {
            _position++;
            return _position < _animals.Count;
        }
        public void Reset() => _position = -1;
    }

    // Задание 45: Контрвариантный интерфейс для обработки ошибок
    public interface IErrorHandler<in TError>
    {
        void HandleError(TError error);
    }

    public class AnimalErrorHandler : IErrorHandler<Animal>
    {
        public void HandleError(Animal error)
        {
            Console.WriteLine($"Обработка ошибки с животным: {error.Name}");
        }
    }

    // Задание 46: Пример вариантности в делегатах
    public class DelegateVarianceDemo
    {
        public static void Demo()
        {
            Action<Animal> animalAction = (animal) => Console.WriteLine($"Действие с животным: {animal.Name}");
            Action<Dog> dogAction = animalAction;
            dogAction(new Dog { Name = "DelegateDog" });

            Func<Dog> dogFactory = () => new Dog { Name = "Собака из фабрики" };
            Func<Animal> animalFactory = dogFactory;
            var animal = animalFactory();
            Console.WriteLine($"Создано через ковариантный делегат: {animal.Name}");
        }
    }

    // Задание 47: Generic класс с явным указанием вариантности
    public class VariantContainer<T> : IProducer<T>, IConsumer<T>
    {
        private T _value;
        public T Produce() => _value;
        public void Consume(T item) => _value = item;
    }

    // Задание 48: Интерфейс для преобразования данных с вариантностью
    public interface ITransformer<in TInput, out TOutput>
    {
        TOutput Transform(TInput input);
    }

    public class AnimalToDogTransformer : ITransformer<Animal, Dog>
    {
        public Dog Transform(Animal input) => new Dog { Name = $"Трансформировано: {input.Name}" };
    }

    // Задание 49: Система типов с поддержкой ковариантности
    public class TypeSystemWithVariance
    {
        public static void ProcessAnimalProducers(IEnumerable<IProducer<Animal>> producers)
        {
            foreach (var producer in producers)
            {
                var animal = producer.Produce();
                Console.WriteLine($"  Произведено животное: {animal.Name}");
            }
        }
    }

    // Задание 50: Generic интерфейс с multiple вариантными параметрами
    public interface IVariantRepository<in TKey, out TValue>
    {
        TValue Get(TKey key);
        IEnumerable<TValue> GetAll();
    }

    public class AnimalVariantRepository : IVariantRepository<string, Animal>
    {
        private Dictionary<string, Animal> _animals = new Dictionary<string, Animal>
        {
            ["dog1"] = new Dog { Name = "VariantDog1" },
            ["animal1"] = new Animal { Name = "VariantAnimal1" }
        };

        public Animal Get(string key) => _animals.TryGetValue(key, out var animal) ? animal : null;
        public IEnumerable<Animal> GetAll() => _animals.Values;
    }

    // ==================== РАЗДЕЛ 3: NULLABLE TYPES (25 задач) ====================

    // Задание 51: Nullable тип int?
    public class NullableIntDemo
    {
        public static void Demo()
        {
            int? nullableInt = null;
            int? anotherNullable = 42;
            Console.WriteLine($"nullableInt: {nullableInt?.ToString() ?? "null"}");
            Console.WriteLine($"anotherNullable: {anotherNullable}");
        }
    }

    // Задание 52: Методы для работы с nullable типами
    public static class NullableHelper
    {
        public static T? CreateNullable<T>(T value) where T : struct => value;
        public static bool HasValue<T>(T? nullable) where T : struct => nullable.HasValue;
        public static T GetValueOrDefault<T>(T? nullable, T defaultValue = default) where T : struct
            => nullable ?? defaultValue;
    }

    // Задание 53: Generic метод для работы с Nullable
    public static class GenericNullableHelper
    {
        public static void ProcessNullable<T>(T? value) where T : struct
        {
            if (value.HasValue) Console.WriteLine($"Значение: {value.Value}");
            else Console.WriteLine("Значение отсутствует");
        }
    }

    // Задание 54: Проверка nullable значения перед использованием
    public static class NullableSafety
    {
        public static void SafeAccess<T>(T? nullable, Action<T> action) where T : struct
        {
            if (nullable.HasValue) action(nullable.Value);
            else Console.WriteLine("Попытка доступа к null значению предотвращена");
        }
    }

    // Задание 55: Пример использования Nullable в структурах
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) { X = x; Y = y; }
        public override string ToString() => $"({X}, {Y})";
    }

    // Задание 56: Метод для конвертации между nullable типами
    public static class NullableConverter
    {
        public static TTarget? Convert<TSource, TTarget>(TSource? source, Func<TSource, TTarget> converter)
            where TSource : struct where TTarget : struct
            => source.HasValue ? converter(source.Value) : (TTarget?)null;
    }

    // Задание 57: Класс для работы с опциональными значениями
    public class Optional<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;
        public Optional() { _hasValue = false; _value = default; }
        public Optional(T value) { _value = value; _hasValue = true; }
        public bool HasValue => _hasValue;
        public T Value => _hasValue ? _value : throw new InvalidOperationException("Значение отсутствует");
        public T GetValueOrDefault(T defaultValue = default) => _hasValue ? _value : defaultValue;
        public override string ToString() => _hasValue ? _value.ToString() : "[No Value]";
    }

    // Задание 58: Методы Equals и GetHashCode для nullable типов
    public static class NullableComparerHelper
    {
        public static bool AreEqual<T>(T? first, T? second) where T : struct, IEquatable<T>
        {
            if (!first.HasValue && !second.HasValue) return true;
            if (first.HasValue && second.HasValue) return first.Value.Equals(second.Value);
            return false;
        }
    }

    // Задание 59: Пример использования nullable типов в свойствах класса
    public class UserProfile
    {
        public string Username { get; set; }
        public int? Age { get; set; }
        public decimal? Salary { get; set; }

        public string GetProfileInfo() =>
            $"Пользователь: {Username}, Возраст: {Age?.ToString() ?? "не указан"}, Зарплата: {Salary?.ToString("C") ?? "не указана"}";
    }

    // Задание 60: Валидация данных с использованием nullable типов
    public static class DataValidator
    {
        public static (bool isValid, string error) ValidateAge(int? age)
        {
            if (!age.HasValue)
                return (false, "Возраст обязателен");

            return (age >= 0 && age <= 150, "Возраст должен быть от 0 до 150");
        }
    }

    // Задание 61: Метод для получения значения или значения по умолчанию
    public static class DefaultValueProvider
    {
        public static T GetValueOrDefault<T>(T? nullable, T defaultValue = default) where T : struct
            => nullable ?? defaultValue;
    }

    // Задание 62: Коллекция nullable значений
    public class NullableCollection<T> where T : struct
    {
        private List<T?> _items = new List<T?>();
        public void Add(T? item) => _items.Add(item);
        public IEnumerable<T> GetNonNullValues() => _items.Where(item => item.HasValue).Select(item => item.Value);
        public void Display()
        {
            Console.WriteLine($"Коллекция: [{string.Join(", ", _items.Select(i => i?.ToString() ?? "null"))}]");
        }
    }

    // Задание 63: Пример преобразования null в значение по умолчанию
    public static class NullToDefaultConverter
    {
        public static T ConvertNullToDefault<T>(T? nullable) where T : struct => nullable ?? default;
        public static T ConvertNullToDefaultClass<T>(T value, T defaultValue) where T : class => value ?? defaultValue;

        public static void Demo()
        {
            int? nullInt = null;
            int result = ConvertNullToDefault(nullInt);
            Console.WriteLine($"null int -> default: {result}");

            string nullString = null;
            string stringResult = ConvertNullToDefaultClass(nullString, "DefaultString");
            Console.WriteLine($"null string -> default: '{stringResult}'");
        }
    }

    // Задание 64: Система для работы с опциональными параметрами
    public class Configuration
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();
        public void SetSetting<T>(string key, T value) where T : struct => _settings[key] = value;
        public T? GetSetting<T>(string key) where T : struct
            => _settings.TryGetValue(key, out object value) && value is T typedValue ? typedValue : (T?)null;
    }

    // Задание 65: Метод для создания nullable копии объекта
    public static class NullableCopyCreator
    {
        public static T? CreateNullableCopy<T>(T value) where T : struct => value;
    }

    // Задание 66: Сравнение nullable значений
    public class NullableValueComparer<T> where T : struct, IComparable<T>
    {
        public static int Compare(T? x, T? y)
        {
            if (!x.HasValue && !y.HasValue) return 0;
            if (!x.HasValue) return -1;
            if (!y.HasValue) return 1;
            return x.Value.CompareTo(y.Value);
        }
    }

    // Задание 67: Пример использования nullable в LINQ запросах
    public static class NullableLinqDemo
    {
        public static void Demo()
        {
            var numbers = new int?[] { 1, null, 3, null, 5, 7, null, 9 };
            var nonNullNumbers = numbers.Where(n => n.HasValue).Select(n => n.Value);
            Console.WriteLine($"Не-null числа: [{string.Join(", ", nonNullNumbers)}]");

            var nullCount = numbers.Count(n => !n.HasValue);
            var nonNullCount = numbers.Count(n => n.HasValue);
            Console.WriteLine($"Null значений: {nullCount}, Не-null: {nonNullCount}");
        }
    }

    // Задание 68: Обработка исключений при работе с nullable
    public static class NullableExceptionHandler
    {
        public static T SafeGetValue<T>(T? nullable, string errorMessage = "Значение отсутствует") where T : struct
        {
            try { return nullable ?? throw new InvalidOperationException(errorMessage); }
            catch (InvalidOperationException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); return default; }
        }
    }

    // Задание 69: Метод для фильтрации null значений из коллекции
    public static class NullFilter
    {
        public static IEnumerable<T> FilterNulls<T>(IEnumerable<T?> source) where T : struct
            => source.Where(item => item.HasValue).Select(item => item.Value);
    }

    // Задание 70: Класс Wrapper для обеспечения nullability
    public class NullableWrapper<T> where T : struct
    {
        private T? _value;
        public bool HasValue => _value.HasValue;
        public T Value => _value ?? throw new InvalidOperationException("Значение отсутствует");
        public void SetValue(T value) => _value = value;
        public T GetValueOrDefault(T defaultValue = default) => _value ?? defaultValue;
    }

    // Задание 71: Пример использования nullable типов в генериках
    public class GenericNullableUsage<T> where T : struct
    {
        private T? _storage;
        public void Store(T value) => _storage = value;
        public T? Retrieve() => _storage;
    }

    // Задание 72: Мэпер для преобразования null значений
    public static class NullableMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource? source, Func<TSource, TTarget> mapper, TTarget defaultValue = default)
            where TSource : struct => source.HasValue ? mapper(source.Value) : defaultValue;
    }

    // Задание 73: Система для работы с отсутствующими значениями
    public class MissingValueHandler<T> where T : struct
    {
        private T? _value;
        public void SetValue(T value) => _value = value;
        public void MarkAsMissing() => _value = null;
        public T GetValue() => _value ?? default;
        public bool IsMissing => !_value.HasValue;
    }

    // Задание 74: Логирование nullable значений
    public static class NullableLogger
    {
        public static void LogValue<T>(string name, T? value) where T : struct
        {
            Console.WriteLine(value.HasValue ? $"{name}: {value.Value}" : $"{name}: [NULL]");
        }
    }

    // Задание 75: Пример кеширования nullable результатов
    public class NullableCache<TKey, TValue> where TValue : struct
    {
        private Dictionary<TKey, TValue?> _cache = new Dictionary<TKey, TValue?>();
        public TValue? Get(TKey key) => _cache.TryGetValue(key, out TValue? value) ? value : null;
        public void Set(TKey key, TValue? value) => _cache[key] = value;
    }

    // ==================== РАЗДЕЛ 4: NULL-COALESCING (25 задач) ====================

    // Задание 76: Оператор ?? для строк
    public static class StringNullCoalescing
    {
        public static void Demo()
        {
            string nullString = null;
            string result = nullString ?? "Default String";
            Console.WriteLine($"Результат: '{result}'");
        }
    }

    // Задание 77: Использование ?? для обработки null значений
    public static class NullCoalescingHelper
    {
        public static T GetValueOrDefault<T>(T value, T defaultValue) where T : class => value ?? defaultValue;
        public static string SafeToString(object obj) => obj?.ToString() ?? "[NULL]";
    }

    // Задание 78: Цепочка операторов ??
    public static class ChainNullCoalescing
    {
        public static T FirstNonNull<T>(params T[] values) where T : class
            => values.FirstOrDefault(value => value != null) ?? (values.Length > 0 ? values[0] : default);
    }

    // Задание 79: Метод для получения первого не-null значения
    public static class FirstNonNullFinder
    {
        public static T FindFirstNonNull<T>(IEnumerable<T> sequence, T defaultValue = default) where T : class
            => sequence.FirstOrDefault(item => item != null) ?? defaultValue;
    }

    // Задание 80: Оператор ??= (null-coalescing assignment)
    public static class NullCoalescingAssignment
    {
        public static void Demo()
        {
            string text = null;
            text ??= "Default Text";
            Console.WriteLine($"text: {text}");

            text ??= "Another Text";
            Console.WriteLine($"text после второго ??=: {text}");
        }
    }

    // Задание 81: Использование ?? в условных выражениях
    public static class ConditionalNullCoalescing
    {
        public static string GetDisplayName(string name, string fallback)
            => (name != null && name.Length > 0 ? name : fallback) ?? "Anonymous";
    }

    // Задание 82: Метод для установки значения только если null
    public static class SetIfNullHelper
    {
        public static void SetIfNull<T>(ref T field, T value) where T : class => field ??= value;
    }

    // Задание 83: Использование ?? с коллекциями
    public static class CollectionNullCoalescing
    {
        public static IEnumerable<T> GetSafeCollection<T>(IEnumerable<T> collection) => collection ?? Enumerable.Empty<T>();

        public static void Demo()
        {
            List<string> nullList = null;
            var safeList = GetSafeCollection(nullList);
            Console.WriteLine($"Безопасная коллекция: [{string.Join(", ", safeList)}]");
        }
    }

    // Задание 84: Пример ?? в LINQ выражениях
    public static class LinqNullCoalescing
    {
        public static void Demo()
        {
            var users = new[]
            {
                new { Name = "Alice", Age = (int?)25 },
                new { Name = "Bob", Age = (int?)null }
            };
            var namesWithAge = users.Select(u => $"{u.Name} ({(u.Age?.ToString() ?? "Unknown")})");
            Console.WriteLine($"Пользователи: {string.Join(", ", namesWithAge)}");
        }
    }

    // Задание 85: Обработка null ссылок на объекты через ??
    public static class ObjectNullCoalescing
    {
        public static string GetProductName(Product product) => product?.Name ?? "Unnamed Product";
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public override string ToString() => $"{Name} - ${Price:F2}";
    }

    // Задание 86: Метод для установки значения свойства если null
    public static class PropertyNullCoalescing
    {
        public static void InitializeIfNull<T>(ref T property, T value) where T : class => property ??= value;
    }

    // Задание 87: Использование ?? в обработчиках событий
    public static class EventNullCoalescing
    {
        public static void SafeInvoke<T>(EventHandler<T> handler, object sender, T args) where T : EventArgs
            => handler?.Invoke(sender, args);
    }

    // Задание 88: Пример ?? при парсинге данных
    public static class ParsingNullCoalescing
    {
        public static int? SafeParseInt(string input) => int.TryParse(input, out int result) ? result : (int?)null;
        public static int ParseIntWithFallback(string input, int fallback = 0) =>
            int.TryParse(input, out int result) ? result : fallback;
    }

    // Задание 89: Цепочка вызовов с ?? (null-safe navigation)
    public static class NullSafeNavigation
    {
        public static string GetDeepValue(Company company) => company?.Department?.Manager?.Name ?? "No Manager";
    }

    public class Company { public Department Department { get; set; } }
    public class Department { public Employee Manager { get; set; } }
    public class Employee { public string Name { get; set; } }

    // Задание 90: Метод для работы с null в коллекциях через ??
    public static class CollectionNullSafety
    {
        public static IEnumerable<T> ConcatSafe<T>(IEnumerable<T> first, IEnumerable<T> second)
            => (first ?? Enumerable.Empty<T>()).Concat(second ?? Enumerable.Empty<T>());
    }

    // Задание 91: Использование ?? для параметров метода
    public static class MethodParameterNullCoalescing
    {
        public static void ProcessItems(IEnumerable<string> items, string defaultItem = "default")
        {
            var safeItems = items ?? new[] { defaultItem };
            Console.WriteLine($"Обработка элементов: [{string.Join(", ", safeItems)}]");
        }
    }

    // Задание 92: Пример ?? с методом Invoke на делегатах
    public static class DelegateNullCoalescing
    {
        public static void SafeInvoke(Action action) => action?.Invoke();
        public static TResult SafeInvoke<TResult>(Func<TResult> func, TResult defaultValue = default)
            => func != null ? func() : defaultValue;
    }

    // Задание 93: Обработка null в асинхронных методах через ??
    public static class AsyncNullCoalescing
    {
        public static async System.Threading.Tasks.Task<string> GetDataAsync()
        {
            await System.Threading.Tasks.Task.Delay(100);
            return null;
        }

        public static async System.Threading.Tasks.Task DemoAsync()
        {
            var data = await GetDataAsync() ?? "Default Async Data";
            Console.WriteLine($"Асинхронные данные: {data}");
        }
    }

    // Задание 94: Пример ?? при работе с конфигурацией
    public static class ConfigurationNullCoalescing
    {
        public class AppConfig
        {
            public string DatabaseConnection { get; set; }
            public int? Timeout { get; set; }
            public string LogLevel { get; set; }
        }

        public static void Demo()
        {
            AppConfig config = null;
            var connection = config?.DatabaseConnection ?? "DefaultConnection";
            var timeout = config?.Timeout ?? 30;
            var logLevel = config?.LogLevel ?? "INFO";
            Console.WriteLine($"Конфигурация: Connection={connection}, Timeout={timeout}, LogLevel={logLevel}");
        }
    }

    // Задание 95: Использование ?? для установки значений по умолчанию
    public static class DefaultValueNullCoalescing
    {
        public static T WithDefault<T>(T value, T defaultValue) where T : class => value ?? defaultValue;
        public static string EnsureNotNull(string value) => value ?? string.Empty;
    }

    // Задание 96: Метод для преобразования null в пустую коллекцию
    public static class EmptyCollectionNullCoalescing
    {
        public static IEnumerable<T> ToEmptyIfNull<T>(IEnumerable<T> collection) => collection ?? Enumerable.Empty<T>();
    }

    // Задание 97: Логирование с использованием ?? для сообщений
    public static class LoggingNullCoalescing
    {
        public static void LogInfo(string message, string category = null)
        {
            var logMessage = $"[{category ?? "General"}] {message ?? "No message"}";
            Console.WriteLine($"LOG: {logMessage}");
        }
    }

    // Задание 98: Пример ?? при работе с базой данных
    public static class DatabaseNullCoalescing
    {
        public class DatabaseRecord
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        public static void ProcessRecord(DatabaseRecord record)
        {
            var name = record?.Name ?? "Unknown";
            var description = record?.Description ?? "No description";
            var created = record?.CreatedDate ?? DateTime.Now;
            Console.WriteLine($"Запись: {name}, {description}, создано: {created}");
        }
    }

    // Задание 99: Использование ?? в конструкторах классов
    public class SmartClass
    {
        private string _name;
        private List<int> _numbers;

        public SmartClass(string name, List<int> numbers)
        {
            _name = name ?? "Default Name";
            _numbers = numbers ?? new List<int> { 1, 2, 3 };
        }

        public void Display()
        {
            Console.WriteLine($"Name: {_name}, Numbers: [{string.Join(", ", _numbers)}]");
        }
    }

    // Задание 100: Метод для валидации и установки значений через ??
    public static class ValidationNullCoalescing
    {
        public static T ValidateAndSet<T>(T value, Func<T, bool> validator, T defaultValue) where T : class
        {
            return (value != null && validator(value)) ? value : defaultValue;
        }
    }
}
