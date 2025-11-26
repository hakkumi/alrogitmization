using AllGenericTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllGenericTasks
{
    // ==================== ВСПОМОГАТЕЛЬНЫЕ ИНТЕРФЕЙСЫ И КЛАССЫ ====================

    public interface IValidatable
    {
        bool IsValid();
        string ValidationMessage { get; }
    }

    public interface IFilterable
    {
        bool MeetsCriteria();
    }

    public class Person : IComparable<Person>, ICloneable, IValidatable, IFilterable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public int CompareTo(Person other) => Age.CompareTo(other.Age);

        public object Clone() => new Person { Id = Id, Name = Name, Age = Age, Email = Email };

        public bool IsValid() => !string.IsNullOrEmpty(Name) && Age > 0 && !string.IsNullOrEmpty(Email);

        public string ValidationMessage => IsValid() ? "Valid" : "Invalid person data";

        public bool MeetsCriteria() => Age >= 18;

        public override string ToString() => $"{Name} ({Age}) - {Email}";
    }

    public class Product : IComparable<Product>, IEquatable<Product>, IValidatable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CompareTo(Product other) => Price.CompareTo(other.Price);

        public bool Equals(Product other) => Id == other.Id && Name == other.Name;

        public bool IsValid() => !string.IsNullOrEmpty(Name) && Price > 0 && Stock >= 0;

        public string ValidationMessage => IsValid() ? "Valid" : "Invalid product data";

        public override string ToString() => $"{Name} - ${Price} (Stock: {Stock})";
    }

    // ==================== РАЗДЕЛ 1: ОГРАНИЧЕНИЯ ПАРАМЕТРИЗИРОВАННЫХ ТИПОВ ====================

    #region Задания 1-50

    // 1. Generic класс с ограничением where T : class
    public class ReferenceContainer<T> where T : class
    {
        private T _value;
        public ReferenceContainer(T value) => _value = value;
        public T GetValue() => _value;
        public bool IsNull() => _value == null;
        public void Print() => Console.WriteLine($"[1] Type: {typeof(T).Name}, Value: {_value?.ToString() ?? "null"}");
    }

    // 2. Generic класс с ограничением where T : struct
    public class ValueContainer<T> where T : struct
    {
        private T _value;
        public ValueContainer(T value) => _value = value;
        public T GetValue() => _value;
        public void Reset() => _value = default;
        public void Print() => Console.WriteLine($"[2] Type: {typeof(T).Name}, Value: {_value}");
    }

    // 3. Generic метод с ограничением where T : new()
    public class InstanceCreator
    {
        public static T Create<T>() where T : new() => new T();
        public static T[] CreateArray<T>(int size) where T : new()
        {
            var array = new T[size];
            for (int i = 0; i < size; i++) array[i] = new T();
            return array;
        }
    }

    // 4. Generic класс с ограничением where T : IComparable
    public class ComparableHelper<T> where T : IComparable<T>
    {
        public static T Max(T a, T b) => a.CompareTo(b) > 0 ? a : b;
        public static T Min(T a, T b) => a.CompareTo(b) < 0 ? a : b;
        public static void Sort(T[] array) => Array.Sort(array);
    }

    // 5. Generic метод с ограничением where T : IEnumerable
    public class EnumerableProcessor
    {
        public static void PrintAll<T>(T collection) where T : IEnumerable
        {
            Console.Write("[5] Elements: ");
            foreach (var item in collection) Console.Write($"{item} ");
            Console.WriteLine();
        }

        public static int Count<T>(T collection) where T : IEnumerable
        {
            int count = 0;
            foreach (var item in collection) count++;
            return count;
        }
    }

    // 6. Generic класс с ограничением где один тип наследует другой
    public class InheritanceChecker<TBase, TDerived> where TDerived : TBase
    {
        public static bool CheckInheritance(TDerived obj) => obj is TBase;
        public static void Process(TDerived obj) =>
            Console.WriteLine($"[6] Processing {obj.GetType().Name} as {typeof(TBase).Name}");
    }

    // 7. Generic метод с несколькими ограничениями where T : class, IDisposable
    public class ResourceHandler
    {
        public static void UseResource<T>(T resource) where T : class, IDisposable
        {
            using (resource)
            {
                Console.WriteLine($"[7] Using resource: {resource.GetType().Name}");
            }
        }
    }

    // 8. Generic интерфейс с ограничением where T : IEquatable
    public interface IEquatableRepository<T> where T : IEquatable<T>
    {
        void Add(T item);
        bool Contains(T item);
        T Find(T item);
        bool Remove(T item);
    }

    // 9. Generic класс Repository где T : class, new()
    public class Repository<T> where T : class, new()
    {
        private List<T> _items = new List<T>();
        private int _nextId = 1;

        public void Add(T item)
        {
            var entity = item as dynamic;
            entity.Id = _nextId++;
            _items.Add(item);
            Console.WriteLine($"[9] Added: {item}");
        }

        public T GetById(int id) => _items.FirstOrDefault(x => ((dynamic)x).Id == id);
        public List<T> GetAll() => _items;
        public bool Remove(T item) => _items.Remove(item);
        public T Create() => new T();
    }

    // 10. Generic метод для сравнения where T : IComparable
    public class ComparisonHelper
    {
        public static int Compare<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b);
        public static bool AreEqual<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) == 0;
        public static T GetMax<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) > 0 ? a : b;
    }

    // 11. Generic класс с ограничением на конкретный тип
    public class NumericCalculator<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        public static T Add(T a, T b) => (dynamic)a + (dynamic)b;
        public static T Multiply(T a, T b) => (dynamic)a * (dynamic)b;
        public static bool IsGreater(T a, T b) => a.CompareTo(b) > 0;
    }

    // 12. Generic метод Clone где T : class, ICloneable
    public class CloneHelper
    {
        public static T Clone<T>(T obj) where T : class, ICloneable => (T)obj.Clone();
        public static List<T> CloneAll<T>(List<T> list) where T : class, ICloneable =>
            list.Select(x => (T)x.Clone()).ToList();
    }

    // 13. Generic класс для работы с коллекциями где T : ICollection
    public class CollectionAnalyzer<T> where T : ICollection
    {
        public static void Analyze(T collection)
        {
            Console.WriteLine($"[13] Count: {collection.Count}, Sync: {collection.IsSynchronized}");
        }
    }

    // 14. Generic метод для поиска где T : IEquatable
    public class SearchHelper
    {
        public static int FindIndex<T>(T[] array, T target) where T : IEquatable<T>
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(target)) return i;
            return -1;
        }
    }

    // 15. Generic класс Serializer где T : class, new()
    public class SimpleSerializer<T> where T : class, new()
    {
        public string Serialize(T obj) => obj.ToString();
        public T Deserialize() => new T();
    }

    // 16. Generic метод для валидации where T : IValidatable
    public class ValidationHelper
    {
        public static bool Validate<T>(T obj) where T : IValidatable => obj.IsValid();
        public static List<T> GetValidItems<T>(List<T> items) where T : IValidatable =>
            items.Where(x => x.IsValid()).ToList();
    }

    // 17. Generic класс Parser с ограничениями на парсинг
    public class GenericParser<T> where T : IConvertible
    {
        public static T Parse(string input) => (T)Convert.ChangeType(input, typeof(T));
        public static bool TryParse(string input, out T result)
        {
            result = default;
            try { result = Parse(input); return true; } catch { return false; }
        }
    }

    // 18. Generic интерфейс Factory где T : class, new()
    public interface IFactory<T> where T : class, new()
    {
        T Create();
        T[] CreateMultiple(int count);
    }

    public class PersonFactory : IFactory<Person>
    {
        public Person Create() => new Person();
        public Person[] CreateMultiple(int count) => new Person[count];
    }

    // 19. Generic класс для кеша где T : class
    public class SimpleCache<TKey, TValue> where TValue : class
    {
        private Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        public void Set(TKey key, TValue value) => _cache[key] = value;
        public TValue Get(TKey key) => _cache.TryGetValue(key, out var value) ? value : null;
        public bool Remove(TKey key) => _cache.Remove(key);
        public void Clear() => _cache.Clear();
        public int Count => _cache.Count;
    }

    // 20. Generic метод для сортировки where T : IComparable
    public class SortingHelper
    {
        public static void Sort<T>(T[] array) where T : IComparable<T> => Array.Sort(array);
        public static void SortDescending<T>(T[] array) where T : IComparable<T> => Array.Sort(array);
    }

    // 21. Generic класс Observer где T : class
    public class Observer<T> where T : class
    {
        private List<Action<T>> _subscribers = new List<Action<T>>();

        public void Subscribe(Action<T> subscriber) => _subscribers.Add(subscriber);
        public void Unsubscribe(Action<T> subscriber) => _subscribers.Remove(subscriber);
        public void Notify(T data) => _subscribers.ForEach(s => s(data));
    }

    // 22. Generic метод для обхода дерева где T : ITreeNode
    public interface ITreeNode<T>
    {
        T Data { get; }
        List<ITreeNode<T>> Children { get; }
    }

    public class TreeTraverser
    {
        public static void Traverse<T>(ITreeNode<T> root, Action<T> action)
        {
            action(root.Data);
            foreach (var child in root.Children)
                Traverse(child, action);
        }
    }

    // 23. Generic класс Graph где T : class, IEquatable
    public class Graph<T> where T : class, IEquatable<T>
    {
        private Dictionary<T, List<T>> _adjacencyList = new Dictionary<T, List<T>>();

        public void AddVertex(T vertex) => _adjacencyList[vertex] = new List<T>();
        public void AddEdge(T from, T to) => _adjacencyList[from].Add(to);
        public List<T> GetNeighbors(T vertex) => _adjacencyList[vertex];
        public bool HasVertex(T vertex) => _adjacencyList.ContainsKey(vertex);
    }

    // 24. Generic метод для мэпирования типов
    public class TypeMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            var destination = new TDestination();
            // Simplified mapping - in real project use reflection or AutoMapper
            return destination;
        }
    }

    // 25. Generic класс для работы с событиями где T : EventArgs
    public class EventManager<T> where T : EventArgs
    {
        public event EventHandler<T> EventOccurred;

        public void RaiseEvent(object sender, T args) => EventOccurred?.Invoke(sender, args);
    }

    // 26. Generic метод Sum где T : struct, IComparable
    public class MathHelper
    {
        public static T Sum<T>(T[] values) where T : struct, IComparable
        {
            if (values.Length == 0) return default;
            dynamic sum = values[0];
            for (int i = 1; i < values.Length; i++) sum += values[i];
            return sum;
        }
    }

    // 27. Generic класс Comparer где T : IComparable
    public class GenericComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y) => x.CompareTo(y);
    }

    // 28. Generic интерфейс с множественными ограничениями
    public interface IAdvancedService<T> where T : class, IComparable<T>, ICloneable, new()
    {
        T Create();
        int Compare(T a, T b);
        T Clone(T obj);
    }

    // 29. Generic метод для преобразования типов с ограничениями
    public class TypeConverter
    {
        public static TOutput Convert<TSource, TOutput>(TSource source)
            where TSource : IConvertible
            where TOutput : IConvertible =>
            (TOutput)System.Convert.ChangeType(source, typeof(TOutput));
    }

    // 30. Generic класс для работы с базой данных где T : EntityBase
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class DatabaseContext<T> where T : EntityBase
    {
        private List<T> _entities = new List<T>();

        public void Add(T entity) => _entities.Add(entity);
        public T Get(int id) => _entities.FirstOrDefault(e => e.Id == id);
        public List<T> GetAll() => _entities;
        public void Update(T entity)
        {
            var existing = Get(entity.Id);
            if (existing != null)
            {
                _entities.Remove(existing);
                _entities.Add(entity);
            }
        }
        public bool Delete(int id) => _entities.RemoveAll(e => e.Id == id) > 0;
    }

    // 31. Generic класс Validator с ограничением на interface
    public class GenericValidator<T> where T : IValidatable
    {
        public bool Validate(T obj) => obj.IsValid();
        public string GetValidationMessage(T obj) => obj.ValidationMessage;
        public List<T> ValidateAll(List<T> items) => items.Where(x => x.IsValid()).ToList();
    }

    // 32. Generic метод для создания объекта где T : class, new()
    public class ObjectFactory
    {
        public static T Create<T>() where T : class, new() => new T();
        public static List<T> CreateMultiple<T>(int count) where T : class, new()
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++) list.Add(new T());
            return list;
        }
    }

    // 33. Generic класс для кеширования с ограничением на тип ключа
    public class GenericCache<TKey, TValue> where TKey : notnull
    {
        private Dictionary<TKey, CacheEntry<TValue>> _cache = new Dictionary<TKey, CacheEntry<TValue>>();
        private TimeSpan _defaultTtl = TimeSpan.FromMinutes(30);

        public void Set(TKey key, TValue value, TimeSpan? ttl = null)
        {
            _cache[key] = new CacheEntry<TValue>(value, DateTime.Now + (ttl ?? _defaultTtl));
        }

        public TValue Get(TKey key)
        {
            if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
                return entry.Value;
            return default;
        }

        public void ClearExpired()
        {
            var expired = _cache.Where(kvp => kvp.Value.IsExpired).Select(kvp => kvp.Key).ToList();
            foreach (var key in expired) _cache.Remove(key);
        }
    }

    public class CacheEntry<T>
    {
        public T Value { get; }
        public DateTime ExpiresAt { get; }
        public bool IsExpired => DateTime.Now > ExpiresAt;

        public CacheEntry(T value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
        }
    }

    // 34. Generic метод для сериализации где T : class, ISerializable
    public interface ISerializable
    {
        string Serialize();
        void Deserialize(string data);
    }

    public class SerializationHelper
    {
        public static string Serialize<T>(T obj) where T : class, ISerializable => obj.Serialize();
        public static void Deserialize<T>(T obj, string data) where T : class, ISerializable => obj.Deserialize(data);
    }

    // 35. Generic класс Pipeline где T : class
    public class Pipeline<T> where T : class
    {
        private List<Func<T, T>> _stages = new List<Func<T, T>>();

        public Pipeline<T> AddStage(Func<T, T> stage) { _stages.Add(stage); return this; }
        public T Execute(T input) => _stages.Aggregate(input, (current, stage) => stage(current));
    }

    // 36. Generic метод для фильтрации где T : IFilterable
    public class FilterHelper
    {
        public static List<T> Filter<T>(List<T> items) where T : IFilterable =>
            items.Where(x => x.MeetsCriteria()).ToList();
    }

    // 37. Generic класс для работы с конвертером типов
    public class Converter<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> _convertFunc;

        public Converter(Func<TSource, TDestination> convertFunc) => _convertFunc = convertFunc;
        public TDestination Convert(TSource source) => _convertFunc(source);
        public List<TDestination> ConvertAll(List<TSource> sources) => sources.Select(Convert).ToList();
    }

    // 38. Generic интерфейс Mapper<TSource, TDestination>
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        List<TDestination> MapAll(List<TSource> sources);
    }

    public class PersonToProductMapper : IMapper<Person, Product>
    {
        public Product Map(Person source) => new Product { Name = source.Name, Price = source.Age * 10 };
        public List<Product> MapAll(List<Person> sources) => sources.Select(Map).ToList();
    }

    // 39. Generic класс для логирования где T : class
    public class Logger<T> where T : class
    {
        public void LogInfo(string message) => Console.WriteLine($"[{typeof(T).Name}] INFO: {message}");
        public void LogError(string message) => Console.WriteLine($"[{typeof(T).Name}] ERROR: {message}");
        public void LogWarning(string message) => Console.WriteLine($"[{typeof(T).Name}] WARN: {message}");
    }

    // 40. Generic метод для работы с опциональными типами
    public class Optional<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public Optional(T value) { _value = value; _hasValue = true; }
        public Optional() { _hasValue = false; }

        public bool HasValue => _hasValue;
        public T Value => _hasValue ? _value : throw new InvalidOperationException("No value");
        public T GetValueOrDefault(T defaultValue = default) => _hasValue ? _value : defaultValue;

        public void Match(Action<T> some, Action none)
        {
            if (_hasValue) some(_value); else none();
        }
    }

    // 41. Generic класс Command для паттерна Command
    public interface ICommand<T>
    {
        void Execute(T context);
        void Undo(T context);
    }

    public class CommandProcessor<T>
    {
        private Stack<ICommand<T>> _history = new Stack<ICommand<T>>();

        public void Execute(ICommand<T> command, T context)
        {
            command.Execute(context);
            _history.Push(command);
        }

        public void Undo(T context)
        {
            if (_history.Count > 0) _history.Pop().Undo(context);
        }
    }

    // 42. Generic метод для асинхронных операций
    public class AsyncHelper
    {
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            Console.WriteLine("Starting async operation...");
            var result = await operation();
            Console.WriteLine("Async operation completed");
            return result;
        }

        public static async Task<List<T>> ExecuteAllAsync<T>(List<Func<Task<T>>> operations)
        {
            var tasks = operations.Select(op => op());
            return (await Task.WhenAll(tasks)).ToList();
        }
    }

    // 43. Generic класс для работы с результатом где T : class
    public class Result<T> where T : class
    {
        public T Data { get; }
        public bool IsSuccess { get; }
        public string Error { get; }

        private Result(T data, bool success, string error = null)
        {
            Data = data; IsSuccess = success; Error = error;
        }

        public static Result<T> Success(T data) => new Result<T>(data, true);
        public static Result<T> Failure(string error) => new Result<T>(null, false, error);

        public void Match(Action<T> onSuccess, Action<string> onFailure)
        {
            if (IsSuccess) onSuccess(Data); else onFailure(Error);
        }
    }

    // 44. Generic интерфейс Provider с ограничениями
    public interface IProvider<T> where T : class, new()
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T item);
        bool Update(T item);
        bool Delete(int id);
    }

    // 45. Generic класс для работы с конфигурацией
    public class Configuration<T> where T : class, new()
    {
        private T _settings = new T();

        public T Settings => _settings;

        public void Update(Action<T> updater) => updater(_settings);

        public void Reset() => _settings = new T();
    }

    // 46. Generic метод FindMin где T : IComparable
    public class MinMaxFinder
    {
        public static T FindMin<T>(T[] array) where T : IComparable<T>
        {
            if (array.Length == 0) throw new ArgumentException("Array is empty");
            T min = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i].CompareTo(min) < 0) min = array[i];
            return min;
        }

        public static T FindMax<T>(T[] array) where T : IComparable<T>
        {
            if (array.Length == 0) throw new ArgumentException("Array is empty");
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i].CompareTo(max) > 0) max = array[i];
            return max;
        }
    }

    // 47. Generic класс для работы с очередью где T : class
    public class GenericQueue<T> where T : class
    {
        private Queue<T> _queue = new Queue<T>();

        public void Enqueue(T item) => _queue.Enqueue(item);
        public T Dequeue() => _queue.Count > 0 ? _queue.Dequeue() : null;
        public T Peek() => _queue.Count > 0 ? _queue.Peek() : null;
        public int Count => _queue.Count;
        public bool IsEmpty => _queue.Count == 0;
        public void Clear() => _queue.Clear();
    }

    // 48. Generic метод Distinct где T : IEquatable
    public class DistinctHelper
    {
        public static List<T> GetDistinct<T>(List<T> list) where T : IEquatable<T> =>
            list.Distinct().ToList();
    }

    // 49. Generic класс для работы со стеком где T : class
    public class GenericStack<T> where T : class
    {
        private Stack<T> _stack = new Stack<T>();

        public void Push(T item) => _stack.Push(item);
        public T Pop() => _stack.Count > 0 ? _stack.Pop() : null;
        public T Peek() => _stack.Count > 0 ? _stack.Peek() : null;
        public int Count => _stack.Count;
        public bool IsEmpty => _stack.Count == 0;
        public void Clear() => _stack.Clear();
    }

    // 50. Generic метод для агрегации данных с ограничениями
    public class AggregationHelper
    {
        public static TResult Aggregate<TSource, TResult>(
            List<TSource> source,
            TResult seed,
            Func<TResult, TSource, TResult> func)
        {
            TResult result = seed;
            foreach (var item in source) result = func(result, item);
            return result;
        }
    }

    #endregion

    // ==================== РАЗДЕЛ 2: ПАРАМЕТРИЗИРОВАННЫЕ КОЛЛЕКЦИИ ====================

    #region Задания 51-100

    // 51. Создайте List и выполните основные операции
    public class ListBasicOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[51] List Basic Operations:");
            List<int> numbers = new List<int> { 1, 2, 3 };

            // Добавление
            numbers.Add(4);
            numbers.AddRange(new[] { 5, 6, 7 });

            // Вставка
            numbers.Insert(0, 0);
            numbers.InsertRange(1, new[] { -1, -2 });

            // Удаление
            numbers.Remove(0);
            numbers.RemoveAt(0);
            numbers.RemoveAll(x => x < 0);
            numbers.RemoveRange(0, 2);

            // Поиск
            bool hasThree = numbers.Contains(3);
            int indexOfFour = numbers.IndexOf(4);
            int lastIndex = numbers.LastIndexOf(5);

            // Доступ
            int first = numbers[0];
            int last = numbers[numbers.Count - 1];

            Console.WriteLine($"List: {string.Join(", ", numbers)}");
            Console.WriteLine($"Count: {numbers.Count}, Capacity: {numbers.Capacity}");
            Console.WriteLine($"Contains 3: {hasThree}, Index of 4: {indexOfFour}");
        }
    }

    // 52. Реализуйте работу с List и методами Find, FindAll
    public class ListFindOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[52] List Find Operations:");
            List<string> names = new List<string> { "Alice", "Bob", "Charlie", "David", "Eve", "Anna" };

            // Find operations
            string found = names.Find(name => name.StartsWith("C"));
            string lastFound = names.FindLast(name => name.Length > 3);
            List<string> allWithA = names.FindAll(name => name.Contains("a"));
            int firstIndex = names.FindIndex(name => name.Length == 3);
            int lastIndex = names.FindLastIndex(name => name.StartsWith("A"));

            Console.WriteLine($"Find (starts with C): {found}");
            Console.WriteLine($"FindLast (length > 3): {lastFound}");
            Console.WriteLine($"FindAll (contains 'a'): {string.Join(", ", allWithA)}");
            Console.WriteLine($"FindIndex (length 3): {firstIndex}");
            Console.WriteLine($"FindLastIndex (starts with A): {lastIndex}");
        }
    }

    // 53. Создайте Dictionary<string, int> для подсчета элементов
    public class FrequencyCounter
    {
        public static Dictionary<string, int> CountWords(string text)
        {
            var words = text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var frequency = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (frequency.ContainsKey(word))
                    frequency[word]++;
                else
                    frequency[word] = 1;
            }

            return frequency;
        }

        public static void Demonstrate()
        {
            Console.WriteLine("\n[53] Word Frequency Counter:");
            string text = "hello world hello generic world programming";
            var frequencies = CountWords(text);

            foreach (var entry in frequencies)
                Console.WriteLine($"'{entry.Key}': {entry.Value} times");
        }
    }

    // 54. Реализуйте List для пользовательского класса
    public class PersonListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[54] Person List Operations:");
            List<Person> people = new List<Person>
            {
                new Person { Name = "Alice", Age = 25, Email = "alice@email.com" },
                new Person { Name = "Bob", Age = 30, Email = "bob@email.com" },
                new Person { Name = "Charlie", Age = 35, Email = "charlie@email.com" },
                new Person { Name = "Diana", Age = 28, Email = "diana@email.com" }
            };

            // Сортировка
            people.Sort((p1, p2) => p1.Age.CompareTo(p2.Age));

            // Поиск
            var youngPeople = people.FindAll(p => p.Age < 30);
            var alice = people.Find(p => p.Name == "Alice");

            Console.WriteLine("All people (sorted by age):");
            foreach (var person in people)
                Console.WriteLine($"  {person}");

            Console.WriteLine("Young people (<30):");
            foreach (var person in youngPeople)
                Console.WriteLine($"  {person}");
        }
    }

    // 55. Создайте Dictionary<int, List> для группировки данных
    public class GroupingDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[55] Dictionary with List Grouping:");
            var studentsByGrade = new Dictionary<int, List<string>>();

            // Добавление данных
            studentsByGrade[1] = new List<string> { "Alice", "Bob" };
            studentsByGrade[2] = new List<string> { "Charlie", "David" };
            studentsByGrade[3] = new List<string> { "Eve", "Frank" };

            // Добавление в существующую группу
            studentsByGrade[1].Add("Grace");
            studentsByGrade[2].AddRange(new[] { "Henry", "Ivan" });

            // Вывод
            foreach (var grade in studentsByGrade)
            {
                Console.WriteLine($"Grade {grade.Key}: {string.Join(", ", grade.Value)}");
            }

            // Поиск
            if (studentsByGrade.TryGetValue(2, out var grade2Students))
                Console.WriteLine($"Grade 2 students: {string.Join(", ", grade2Students)}");
        }
    }

    // 56. Реализуйте работу с List методами Sort, Reverse, BinarySearch
    public class ListSortingOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[56] List Sorting Operations:");
            List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };

            Console.WriteLine($"Original: {string.Join(", ", numbers)}");

            // Сортировка
            numbers.Sort();
            Console.WriteLine($"Sorted: {string.Join(", ", numbers)}");

            // Реверс
            numbers.Reverse();
            Console.WriteLine($"Reversed: {string.Join(", ", numbers)}");

            // Бинарный поиск
            numbers.Sort(); // Для бинарного поиска список должен быть отсортирован
            int index = numbers.BinarySearch(7);
            Console.WriteLine($"BinarySearch for 7: index = {index}");

            // Пользовательская сортировка
            List<string> names = new List<string> { "Charlie", "Alice", "Bob", "David" };
            names.Sort((a, b) => a.Length.CompareTo(b.Length));
            Console.WriteLine($"Names sorted by length: {string.Join(", ", names)}");
        }
    }

    // 57. Создайте Dictionary<string, decimal> для хранения цен
    public class PriceDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[57] Price Dictionary:");
            var prices = new Dictionary<string, decimal>
            {
                ["laptop"] = 999.99m,
                ["mouse"] = 25.50m,
                ["keyboard"] = 75.00m,
                ["monitor"] = 299.99m
            };

            // Добавление
            prices["headphones"] = 149.99m;

            // Обновление
            prices["mouse"] = 19.99m;

            // Поиск
            if (prices.TryGetValue("laptop", out decimal laptopPrice))
                Console.WriteLine($"Laptop price: ${laptopPrice}");

            // Итерация
            foreach (var item in prices)
                Console.WriteLine($"{item.Key}: ${item.Value:F2}");

            // Проверка наличия
            Console.WriteLine($"Contains 'tablet': {prices.ContainsKey("tablet")}");
        }
    }

    // 58. Реализуйте Dictionary<TKey, TValue> с пользовательским классом в качестве значения
    public class ObjectDictionaryDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[58] Dictionary with Custom Objects:");
            var productCatalog = new Dictionary<int, Product>
            {
                [1] = new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10 },
                [2] = new Product { Id = 2, Name = "Mouse", Price = 25.50m, Stock = 50 },
                [3] = new Product { Id = 3, Name = "Keyboard", Price = 75.00m, Stock = 25 }
            };

            // Добавление нового продукта
            productCatalog[4] = new Product { Id = 4, Name = "Monitor", Price = 299.99m, Stock = 15 };

            // Поиск и обновление
            if (productCatalog.TryGetValue(2, out var mouse))
            {
                mouse.Stock -= 5; // Продали 5 мышей
                Console.WriteLine($"Updated mouse stock: {mouse.Stock}");
            }

            // Фильтрация
            var expensiveProducts = productCatalog.Where(kvp => kvp.Value.Price > 100);
            Console.WriteLine("Expensive products (>$100):");
            foreach (var product in expensiveProducts)
                Console.WriteLine($"  {product.Value}");

            // Суммарная стоимость инвентаря
            decimal totalValue = productCatalog.Sum(kvp => kvp.Value.Price * kvp.Value.Stock);
            Console.WriteLine($"Total inventory value: ${totalValue:F2}");
        }
    }

    // 59. Создайте List<Tuple<int, string>> для хранения пар данных
    public class TupleListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[59] List of Tuples:");
            var data = new List<Tuple<int, string>>
            {
                Tuple.Create(1, "Alice"),
                Tuple.Create(2, "Bob"),
                Tuple.Create(3, "Charlie"),
                Tuple.Create(4, "Diana")
            };

            // Добавление
            data.Add(Tuple.Create(5, "Eve"));

            // Поиск
            var bob = data.Find(t => t.Item2 == "Bob");
            if (bob != null)
                Console.WriteLine($"Found: ID={bob.Item1}, Name={bob.Item2}");

            // Проекция
            var names = data.Select(t => t.Item2).ToList();
            Console.WriteLine($"All names: {string.Join(", ", names)}");

            // Группировка по длине имени
            var groupedByLength = data.GroupBy(t => t.Item2.Length);
            foreach (var group in groupedByLength)
            {
                Console.WriteLine($"Names with length {group.Key}: {string.Join(", ", group.Select(t => t.Item2))}");
            }
        }
    }

    // 60. Реализуйте SortedDictionary<string, int> для автоматической сортировки
    public class SortedDictionaryDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[60] Sorted Dictionary:");
            var sortedScores = new SortedDictionary<string, int>
            {
                ["Charlie"] = 85,
                ["Alice"] = 92,
                ["Bob"] = 78,
                ["Diana"] = 96
            };

            // Добавление (автоматически сортируется)
            sortedScores["Eve"] = 88;
            sortedScores["Frank"] = 73;

            Console.WriteLine("Scores (automatically sorted by name):");
            foreach (var entry in sortedScores)
                Console.WriteLine($"  {entry.Key}: {entry.Value}");

            // Диапазон операций
            var aToC = sortedScores.Where(kvp => kvp.Key[0] >= 'A' && kvp.Key[0] <= 'C');
            Console.WriteLine("Names A-C:");
            foreach (var entry in aToC)
                Console.WriteLine($"  {entry.Key}: {entry.Value}");

            // Получение ключей и значений
            var names = sortedScores.Keys.ToList();
            var scores = sortedScores.Values.ToList();
            Console.WriteLine($"All names: {string.Join(", ", names)}");
            Console.WriteLine($"All scores: {string.Join(", ", scores)}");
        }
    }

    // 61. Создайте Dictionary<int, HashSet> для работы с уникальными значениями
    public class HashSetDictionaryDemo1
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[61] Dictionary with HashSet:");
            var courseStudents = new Dictionary<int, HashSet<string>>();

            // Добавление студентов в курсы
            courseStudents[101] = new HashSet<string> { "Alice", "Bob", "Charlie" };
            courseStudents[102] = new HashSet<string> { "Bob", "Diana", "Eve" };
            courseStudents[103] = new HashSet<string> { "Alice", "Charlie", "Frank" };

            // Добавление (дубликаты игнорируются)
            courseStudents[101].Add("Alice"); // Не добавится - уже есть
            courseStudents[101].Add("Grace"); // Добавится

            // Поиск общих студентов
            var commonStudents = courseStudents[101].Intersect(courseStudents[102]);
            Console.WriteLine($"Students in both course 101 and 102: {string.Join(", ", commonStudents)}");

            // Все уникальные студенты
            var allStudents = courseStudents.Values.SelectMany(set => set).ToHashSet();
            Console.WriteLine($"All unique students: {string.Join(", ", allStudents)}");

            // Вывод
            foreach (var course in courseStudents)
            {
                Console.WriteLine($"Course {course.Key}: {string.Join(", ", course.Value)}");
            }
        }
    }

    // 62. Реализуйте List с использованием LINQ методов
    public class LinqListOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[62] LINQ with List:");
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Фильтрация
            var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
            var greaterThanFive = numbers.Where(n => n > 5).ToList();

            // Проекция
            var squares = numbers.Select(n => n * n).ToList();
            var strings = numbers.Select(n => $"Number: {n}").ToList();

            // Агрегация
            var sum = numbers.Sum();
            var average = numbers.Average();
            var max = numbers.Max();
            var min = numbers.Min();

            // Группировка
            var numberGroups = numbers.GroupBy(n => n % 2 == 0 ? "Even" : "Odd");

            Console.WriteLine($"Original: {string.Join(", ", numbers)}");
            Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");
            Console.WriteLine($"Squares: {string.Join(", ", squares)}");
            Console.WriteLine($"Sum: {sum}, Average: {average:F2}, Max: {max}, Min: {min}");

            foreach (var group in numberGroups)
            {
                Console.WriteLine($"{group.Key}: {string.Join(", ", group)}");
            }
        }
    }

    // 63. Создайте Dictionary<string, object> для динамических данных
    public class DynamicDictionaryDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[63] Dynamic Dictionary:");
            var dynamicData = new Dictionary<string, object>
            {
                ["name"] = "John Doe",
                ["age"] = 30,
                ["salary"] = 50000.0m,
                ["isEmployed"] = true,
                ["hobbies"] = new List<string> { "reading", "gaming", "coding" },
                ["address"] = new Dictionary<string, object>
                {
                    ["street"] = "123 Main St",
                    ["city"] = "New York",
                    ["zipCode"] = "10001"
                }
            };

            // Добавление динамических данных
            dynamicData["projects"] = new List<object>
            {
                new { Name = "Project A", Status = "Completed" },
                new { Name = "Project B", Status = "In Progress" }
            };

            // Чтение и преобразование типов
            string name = dynamicData["name"].ToString();
            int age = (int)dynamicData["age"];
            List<string> hobbies = (List<string>)dynamicData["hobbies"];

            Console.WriteLine($"Name: {name}, Age: {age}");
            Console.WriteLine($"Hobbies: {string.Join(", ", hobbies)}");

            // Итерация по всем данным
            foreach (var entry in dynamicData)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value} ({entry.Value.GetType().Name})");
            }
        }
    }

    // 64. Реализуйте Queue и Stack для работы с очередями
    public class QueueStackDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[64] Queue and Stack Operations:");

            // Queue (FIFO)
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");

            Console.WriteLine("Queue (FIFO):");
            while (queue.Count > 0)
                Console.WriteLine($"  Dequeue: {queue.Dequeue()}");

            // Stack (LIFO)
            Stack<string> stack = new Stack<string>();
            stack.Push("First");
            stack.Push("Second");
            stack.Push("Third");

            Console.WriteLine("Stack (LIFO):");
            while (stack.Count > 0)
                Console.WriteLine($"  Pop: {stack.Pop()}");

            // Concurrent operations
            Queue<int> numberQueue = new Queue<int>();
            for (int i = 1; i <= 5; i++) numberQueue.Enqueue(i);

            Stack<int> numberStack = new Stack<int>();
            while (numberQueue.Count > 0) numberStack.Push(numberQueue.Dequeue());

            Console.WriteLine("Transfer from Queue to Stack:");
            while (numberStack.Count > 0)
                Console.WriteLine($"  From stack: {numberStack.Pop()}");
        }
    }

    // 65. Создайте List с фильтрацией и преобразованием
    public class ListFilterTransformDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[65] List Filter and Transform:");
            List<Person> people = new List<Person>
            {
                new Person { Name = "Alice", Age = 25, Email = "alice@email.com" },
                new Person { Name = "Bob", Age = 17, Email = "bob@email.com" },
                new Person { Name = "Charlie", Age = 32, Email = "charlie@email.com" },
                new Person { Name = "Diana", Age = 16, Email = "diana@email.com" },
                new Person { Name = "Eve", Age = 28, Email = "eve@email.com" }
            };

            // Фильтрация (взрослые)
            var adults = people.Where(p => p.Age >= 18).ToList();

            // Преобразование (только имена)
            var names = people.Select(p => p.Name).ToList();

            // Комбинированная операция
            var adultNames = people
                .Where(p => p.Age >= 18)
                .Select(p => $"{p.Name} ({p.Age} years)")
                .ToList();

            // Группировка по возрасту
            var ageGroups = people.GroupBy(p => p.Age < 18 ? "Minor" : "Adult");

            Console.WriteLine("All people:");
            people.ForEach(p => Console.WriteLine($"  {p}"));

            Console.WriteLine("Adults only:");
            adults.ForEach(p => Console.WriteLine($"  {p}"));

            Console.WriteLine("Adult names with age:");
            adultNames.ForEach(name => Console.WriteLine($"  {name}"));

            foreach (var group in ageGroups)
            {
                Console.WriteLine($"{group.Key}s: {string.Join(", ", group.Select(p => p.Name))}");
            }
        }
    }

    // 66. Реализуйте Dictionary<TKey, TValue> с проверкой наличия ключа
    public class SafeDictionaryDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[66] Safe Dictionary Operations:");
            var dictionary = new Dictionary<string, int>
            {
                ["apple"] = 5,
                ["banana"] = 3,
                ["cherry"] = 8
            };

            // Безопасное добавление
            if (!dictionary.ContainsKey("orange"))
                dictionary["orange"] = 12;

            // Безопасное получение
            if (dictionary.TryGetValue("banana", out int bananaCount))
                Console.WriteLine($"Banana count: {bananaCount}");

            if (!dictionary.TryGetValue("grape", out int grapeCount))
                Console.WriteLine("Grape not found in dictionary");

            // Обновление с проверкой
            if (dictionary.ContainsKey("apple"))
                dictionary["apple"] += 2;

            // Удаление с проверкой
            bool removed = dictionary.Remove("cherry");
            Console.WriteLine($"Cherry removed: {removed}");

            // Вывод оставшихся элементов
            foreach (var item in dictionary)
                Console.WriteLine($"  {item.Key}: {item.Value}");
        }
    }

    // 67. Создайте SortedList<TKey, TValue> для сортировки по ключам
    public class SortedListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[67] SortedList Operations:");
            var sortedList = new SortedList<string, decimal>
            {
                { "zebra", 1500.00m },
                { "apple", 2.50m },
                { "monkey", 800.00m },
                { "banana", 1.75m },
                { "elephant", 5000.00m }
            };

            Console.WriteLine("Automatically sorted by key:");
            foreach (var item in sortedList)
                Console.WriteLine($"  {item.Key}: ${item.Value:F2}");

            // Доступ по индексу
            Console.WriteLine($"First item: {sortedList.Keys[0]} = ${sortedList.Values[0]:F2}");
            Console.WriteLine($"Last item: {sortedList.Keys[^1]} = ${sortedList.Values[^1]:F2}");

            // Поиск
            int index = sortedList.IndexOfKey("monkey");
            Console.WriteLine($"Index of 'monkey': {index}");

            // Диапазон значений
            var cheapItems = sortedList.Where(kvp => kvp.Value < 100).ToList();
            Console.WriteLine("Items under $100:");
            foreach (var item in cheapItems)
                Console.WriteLine($"  {item.Key}: ${item.Value:F2}");
        }
    }

    // 68. Реализуйте List для работы с событиями и уведомлениями
    public class EventListDemo
    {
        public class Notification
        {
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
            public string Type { get; set; }

            public override string ToString() => $"[{Timestamp:HH:mm:ss}] {Type}: {Message}";
        }

        public static void Demonstrate()
        {
            Console.WriteLine("\n[68] Event Notification List:");
            List<Notification> notifications = new List<Notification>();

            // Добавление уведомлений
            notifications.Add(new Notification
            {
                Message = "User logged in",
                Timestamp = DateTime.Now,
                Type = "INFO"
            });

            notifications.Add(new Notification
            {
                Message = "Database connection failed",
                Timestamp = DateTime.Now.AddSeconds(5),
                Type = "ERROR"
            });

            notifications.Add(new Notification
            {
                Message = "Payment processed",
                Timestamp = DateTime.Now.AddSeconds(10),
                Type = "SUCCESS"
            });

            // Фильтрация по типу
            var errors = notifications.Where(n => n.Type == "ERROR").ToList();
            Console.WriteLine("Error notifications:");
            errors.ForEach(n => Console.WriteLine($"  {n}"));

            // Сортировка по времени
            notifications.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            Console.WriteLine("All notifications (sorted):");
            notifications.ForEach(n => Console.WriteLine($"  {n}"));

            // Группировка по тиму
            var grouped = notifications.GroupBy(n => n.Type);
            foreach (var group in grouped)
            {
                Console.WriteLine($"{group.Key} notifications ({group.Count()}):");
                foreach (var notification in group)
                    Console.WriteLine($"  {notification.Message}");
            }
        }
    }

    // 69. Создайте Dictionary<string, List> для многоуровневого хранилища
    public class MultiLevelStorage
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[69] Multi-level Dictionary Storage:");
            var storage = new Dictionary<string, List<object>>();

            // Данные пользователей
            storage["users"] = new List<object>
            {
                new { Id = 1, Name = "Alice", Role = "Admin" },
                new { Id = 2, Name = "Bob", Role = "User" },
                new { Id = 3, Name = "Charlie", Role = "Moderator" }
            };

            // Данные продуктов
            storage["products"] = new List<object>
            {
                new { Id = 1, Name = "Laptop", Price = 999.99m },
                new { Id = 2, Name = "Mouse", Price = 25.50m },
                new { Id = 3, Name = "Keyboard", Price = 75.00m }
            };

            // Данные заказов
            storage["orders"] = new List<object>
            {
                new { OrderId = 1001, UserId = 1, Total = 1075.49m },
                new { OrderId = 1002, UserId = 2, Total = 25.50m }
            };

            // Вывод всех данных
            foreach (var category in storage)
            {
                Console.WriteLine($"\n{category.Key.ToUpper()}:");
                foreach (var item in category.Value)
                    Console.WriteLine($"  {item}");
            }

            // Поиск в категории
            if (storage.TryGetValue("users", out var users))
            {
                var admins = users.Where(u => u.ToString().Contains("Admin"));
                Console.WriteLine("\nAdmin users:");
                foreach (var admin in admins)
                    Console.WriteLine($"  {admin}");
            }
        }
    }

    // 70. Реализуйте LinkedList для связного списка
    public class LinkedListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[70] LinkedList Operations:");
            LinkedList<string> linkedList = new LinkedList<string>();

            // Добавление элементов
            linkedList.AddLast("first");
            linkedList.AddLast("third");
            linkedList.AddFirst("zero");

            // Вставка после определенного элемента
            var firstNode = linkedList.Find("first");
            if (firstNode != null)
                linkedList.AddAfter(firstNode, "second");

            // Вставка перед определенным элементом
            var thirdNode = linkedList.Find("third");
            if (thirdNode != null)
                linkedList.AddBefore(thirdNode, "2.5");

            Console.WriteLine("Linked list contents:");
            foreach (var item in linkedList)
                Console.WriteLine($"  {item}");

            // Навигация по узлам
            Console.WriteLine("Navigation using nodes:");
            var currentNode = linkedList.First;
            while (currentNode != null)
            {
                Console.WriteLine($"  Node: {currentNode.Value}, " +
                                $"Previous: {currentNode.Previous?.Value ?? "null"}, " +
                                $"Next: {currentNode.Next?.Value ?? "null"}");
                currentNode = currentNode.Next;
            }

            // Удаление элементов
            linkedList.Remove("2.5");
            linkedList.RemoveFirst();
            linkedList.RemoveLast();

            Console.WriteLine("After removals:");
            foreach (var item in linkedList)
                Console.WriteLine($"  {item}");
        }
    }

    // 71. Создайте List с использованием Contains, IndexOf, LastIndexOf
    public class ListSearchOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[71] List Search Operations:");
            List<string> items = new List<string>
            {
                "apple", "banana", "cherry", "apple", "date", "elderberry", "apple", "fig"
            };

            // Contains
            bool hasApple = items.Contains("apple");
            bool hasGrape = items.Contains("grape");
            Console.WriteLine($"Contains 'apple': {hasApple}");
            Console.WriteLine($"Contains 'grape': {hasGrape}");

            // IndexOf
            int firstApple = items.IndexOf("apple");
            int firstDate = items.IndexOf("date");
            Console.WriteLine($"First 'apple' at index: {firstApple}");
            Console.WriteLine($"First 'date' at index: {firstDate}");

            // LastIndexOf
            int lastApple = items.LastIndexOf("apple");
            Console.WriteLine($"Last 'apple' at index: {lastApple}");

            // IndexOf с начальным индексом
            int secondApple = items.IndexOf("apple", firstApple + 1);
            Console.WriteLine($"Second 'apple' at index: {secondApple}");

            // Поиск несуществующего элемента
            int notFound = items.IndexOf("grape");
            Console.WriteLine($"Index of 'grape': {notFound} (not found)");

            // Поиск с предикатом
            int firstLongWord = items.FindIndex(s => s.Length > 5);
            int lastLongWord = items.FindLastIndex(s => s.Length > 5);
            Console.WriteLine($"First long word at index: {firstLongWord} ('{items[firstLongWord]}')");
            Console.WriteLine($"Last long word at index: {lastLongWord} ('{items[lastLongWord]}')");
        }
    }

    // 72. Реализуйте Dictionary<int, string> для маппирования значений
    public class MappingDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[72] Mapping Dictionary:");

            // Коды ошибок
            var errorCodes = new Dictionary<int, string>
            {
                [404] = "Not Found",
                [500] = "Internal Server Error",
                [403] = "Forbidden",
                [200] = "OK",
                [201] = "Created",
                [400] = "Bad Request"
            };

            // Статусы заказов
            var orderStatuses = new Dictionary<int, string>
            {
                [1] = "Pending",
                [2] = "Processing",
                [3] = "Shipped",
                [4] = "Delivered",
                [5] = "Cancelled"
            };

            Console.WriteLine("Error Codes:");
            foreach (var code in errorCodes.OrderBy(kvp => kvp.Key))
                Console.WriteLine($"  {code.Key}: {code.Value}");

            Console.WriteLine("\nOrder Statuses:");
            foreach (var statusEntry in orderStatuses)
                Console.WriteLine($"  {statusEntry.Key}: {statusEntry.Value}");

            // Использование для маппирования
            int userStatusCode = 3;
            if (orderStatuses.TryGetValue(userStatusCode, out string orderStatus))
                Console.WriteLine($"\nYour order status: {orderStatus}");

            // Обратное маппирование
            string searchStatus = "Delivered";
            var statusCode = orderStatuses.FirstOrDefault(kvp => kvp.Value == searchStatus).Key;
            Console.WriteLine($"Status '{searchStatus}' has code: {statusCode}");
        }
    }

    // 73. Создайте List с добавлением и удалением элементов
    public class ListModificationOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[73] List Modification Operations:");
            List<string> fruits = new List<string>();

            // Добавление элементов
            fruits.Add("apple");
            fruits.Add("banana");
            fruits.AddRange(new[] { "cherry", "date", "elderberry" });
            fruits.Insert(1, "apricot");
            fruits.InsertRange(3, new[] { "blueberry", "cranberry" });

            Console.WriteLine("After additions:");
            fruits.ForEach(f => Console.WriteLine($"  {f}"));

            // Удаление элементов
            fruits.Remove("date");
            fruits.RemoveAt(0);
            fruits.RemoveRange(2, 2);
            fruits.RemoveAll(f => f.StartsWith("a"));

            Console.WriteLine("After removals:");
            fruits.ForEach(f => Console.WriteLine($"  {f}"));

            // Очистка и повторное заполнение
            fruits.Clear();
            Console.WriteLine($"After clear: Count = {fruits.Count}");

            fruits.AddRange(new[] { "grape", "kiwi", "mango", "orange", "pear" });
            Console.WriteLine("After refill:");
            fruits.ForEach(f => Console.WriteLine($"  {f}"));

            // Массовые операции
            var tropicalFruits = new List<string> { "pineapple", "papaya", "guava" };
            fruits.AddRange(tropicalFruits);
            fruits.Sort();

            Console.WriteLine("Final sorted list:");
            fruits.ForEach(f => Console.WriteLine($"  {f}"));
        }
    }

    // 74. Реализуйте Dictionary<string, int> для счетчика частоты
    public class FrequencyCounterAdvanced
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[74] Advanced Frequency Counter:");
            string text = "the quick brown fox jumps over the lazy dog the fox is quick";

            var wordFrequency = new Dictionary<string, int>();
            var charFrequency = new Dictionary<char, int>();

            // Подсчет частоты слов
            var words = text.Split(' ');
            foreach (var word in words)
            {
                if (wordFrequency.ContainsKey(word))
                    wordFrequency[word]++;
                else
                    wordFrequency[word] = 1;
            }

            // Подсчет частоты символов
            foreach (char c in text.Where(char.IsLetterOrDigit))
            {
                if (charFrequency.ContainsKey(c))
                    charFrequency[c]++;
                else
                    charFrequency[c] = 1;
            }

            Console.WriteLine("Word Frequency:");
            foreach (var entry in wordFrequency.OrderByDescending(kvp => kvp.Value))
                Console.WriteLine($"  '{entry.Key}': {entry.Value}");

            Console.WriteLine("\nCharacter Frequency:");
            foreach (var entry in charFrequency.OrderByDescending(kvp => kvp.Value).Take(10))
                Console.WriteLine($"  '{entry.Key}': {entry.Value}");

            // Статистика
            Console.WriteLine($"\nStatistics:");
            Console.WriteLine($"  Total unique words: {wordFrequency.Count}");
            Console.WriteLine($"  Most frequent word: '{wordFrequency.OrderByDescending(kvp => kvp.Value).First().Key}'");
            Console.WriteLine($"  Total unique characters: {charFrequency.Count}");
        }
    }

    // 75. Создайте List<KeyValuePair<TKey, TValue>> для работы с парами
    public class KeyValuePairListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[75] KeyValuePair List Operations:");

            // Создание списка пар ключ-значение
            var keyValueList = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("apple", 5),
                new KeyValuePair<string, int>("banana", 3),
                new KeyValuePair<string, int>("cherry", 8),
                new KeyValuePair<string, int>("date", 2),
                new KeyValuePair<string, int>("elderberry", 6)
            };

            // Добавление новых пар
            keyValueList.Add(new KeyValuePair<string, int>("fig", 4));
            keyValueList.Insert(1, new KeyValuePair<string, int>("apricot", 7));

            Console.WriteLine("All key-value pairs:");
            foreach (var kvp in keyValueList)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

            // Поиск по ключу
            var cherryPair = keyValueList.Find(kvp => kvp.Key == "cherry");
            Console.WriteLine($"Found: {cherryPair.Key} = {cherryPair.Value}");

            // Фильтрация по значению
            var highQuantity = keyValueList.Where(kvp => kvp.Value > 5).ToList();
            Console.WriteLine("Items with quantity > 5:");
            foreach (var kvp in highQuantity)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

            // Сортировка
            keyValueList.Sort((a, b) => a.Value.CompareTo(b.Value));
            Console.WriteLine("Sorted by value:");
            foreach (var kvp in keyValueList)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

            // Преобразование в Dictionary
            var dictionary = keyValueList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Console.WriteLine("Converted to Dictionary:");
            foreach (var item in dictionary)
                Console.WriteLine($"  {item.Key}: {item.Value}");
        }
    }

    // 76. Реализуйте Dictionary<TKey, TValue> с итерацией и фильтрацией
    public class DictionaryIterationDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[76] Dictionary Iteration and Filtering:");
            var inventory = new Dictionary<string, Product>
            {
                ["laptop"] = new Product { Name = "Gaming Laptop", Price = 1299.99m, Stock = 15 },
                ["mouse"] = new Product { Name = "Wireless Mouse", Price = 45.50m, Stock = 100 },
                ["keyboard"] = new Product { Name = "Mechanical Keyboard", Price = 89.99m, Stock = 25 },
                ["monitor"] = new Product { Name = "4K Monitor", Price = 399.99m, Stock = 8 },
                ["headphones"] = new Product { Name = "Noise Cancelling Headphones", Price = 199.99m, Stock = 30 }
            };

            // Базовая итерация
            Console.WriteLine("All inventory items:");
            foreach (var item in inventory)
                Console.WriteLine($"  {item.Key}: {item.Value}");

            // Фильтрация LINQ
            var lowStock = inventory.Where(kvp => kvp.Value.Stock < 20);
            Console.WriteLine("\nLow stock items (<20):");
            foreach (var item in lowStock)
                Console.WriteLine($"  {item.Key}: {item.Value.Stock} in stock");

            var expensiveItems = inventory.Where(kvp => kvp.Value.Price > 100);
            Console.WriteLine("\nExpensive items (>$100):");
            foreach (var item in expensiveItems)
                Console.WriteLine($"  {item.Key}: ${item.Value.Price:F2}");

            // Проекция
            var productNames = inventory.Select(kvp => kvp.Value.Name).ToList();
            Console.WriteLine($"\nAll product names: {string.Join(", ", productNames)}");

            // Агрегация
            decimal totalValue = inventory.Sum(kvp => kvp.Value.Price * kvp.Value.Stock);
            int totalItems = inventory.Sum(kvp => kvp.Value.Stock);
            Console.WriteLine($"\nTotal inventory value: ${totalValue:F2}");
            Console.WriteLine($"Total items in stock: {totalItems}");

            // Группировка по цене
            var priceGroups = inventory.GroupBy(kvp =>
                kvp.Value.Price < 50 ? "Cheap" :
                kvp.Value.Price < 200 ? "Medium" : "Expensive");

            foreach (var group in priceGroups)
            {
                Console.WriteLine($"\n{group.Key} products:");
                foreach (var item in group)
                    Console.WriteLine($"  {item.Key}: ${item.Value.Price:F2}");
            }
        }
    }

    // 77. Создайте List для кеширования результатов
    public class CacheListDemo
    {
        public class CacheItem
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastAccessed { get; set; }
            public int AccessCount { get; set; }

            public override string ToString() =>
                $"{Key}: {Value} (Created: {CreatedAt:HH:mm:ss}, Accesses: {AccessCount})";
        }

        public static void Demonstrate()
        {
            Console.WriteLine("\n[77] Cache List Implementation:");
            List<CacheItem> cache = new List<CacheItem>();

            // Добавление элементов в кеш
            cache.Add(new CacheItem
            {
                Key = "user_1",
                Value = new { Name = "Alice", Age = 25 },
                CreatedAt = DateTime.Now
            });

            cache.Add(new CacheItem
            {
                Key = "config",
                Value = new { Theme = "Dark", Language = "EN" },
                CreatedAt = DateTime.Now.AddMinutes(-5)
            });

            cache.Add(new CacheItem
            {
                Key = "products",
                Value = new List<string> { "Laptop", "Mouse", "Keyboard" },
                CreatedAt = DateTime.Now.AddMinutes(-10)
            });

            // Симуляция доступа к элементам
            var userItem = cache.Find(item => item.Key == "user_1");
            if (userItem != null)
            {
                userItem.LastAccessed = DateTime.Now;
                userItem.AccessCount++;
            }

            // Поиск устаревших элементов (старше 7 минут)
            var oldItems = cache.Where(item =>
                DateTime.Now - item.CreatedAt > TimeSpan.FromMinutes(7)).ToList();

            Console.WriteLine("All cache items:");
            cache.ForEach(item => Console.WriteLine($"  {item}"));

            Console.WriteLine($"\nOld items to remove: {oldItems.Count}");
            oldItems.ForEach(item => Console.WriteLine($"  {item.Key}"));

            // Удаление устаревших элементов
            cache.RemoveAll(item => DateTime.Now - item.CreatedAt > TimeSpan.FromMinutes(7));
            Console.WriteLine($"\nCache after cleanup: {cache.Count} items");

            // Сортировка по частоте доступа
            cache.Sort((a, b) => b.AccessCount.CompareTo(a.AccessCount));
            Console.WriteLine("\nCache sorted by access count:");
            cache.ForEach(item => Console.WriteLine($"  {item}"));
        }
    }

    // 78. Реализуйте Dictionary<int, object> для полиморфных коллекций
    public class PolymorphicDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[78] Polymorphic Dictionary:");
            var polymorphicData = new Dictionary<int, object>();

            // Добавление различных типов данных
            polymorphicData[1] = "Simple string";
            polymorphicData[2] = 42;
            polymorphicData[3] = 3.14159;
            polymorphicData[4] = true;
            polymorphicData[5] = new List<int> { 1, 2, 3, 4, 5 };
            polymorphicData[6] = new { Name = "John", Age = 30 };
            polymorphicData[7] = DateTime.Now;
            polymorphicData[8] = new Dictionary<string, object>
            {
                ["key"] = "value",
                ["number"] = 123
            };

            Console.WriteLine("Polymorphic dictionary contents:");
            foreach (var entry in polymorphicData)
            {
                Console.WriteLine($"  Key {entry.Key}: {entry.Value} ({entry.Value.GetType().Name})");
            }

            // Обработка разных типов
            Console.WriteLine("\nType-specific processing:");
            foreach (var entry in polymorphicData)
            {
                switch (entry.Value)
                {
                    case string str:
                        Console.WriteLine($"  String: '{str}' (Length: {str.Length})");
                        break;
                    case int num:
                        Console.WriteLine($"  Integer: {num} (Double: {num * 2})");
                        break;
                    case double dbl:
                        Console.WriteLine($"  Double: {dbl:F4} (Rounded: {Math.Round(dbl, 2)})");
                        break;
                    case bool boolean:
                        Console.WriteLine($"  Boolean: {boolean} (Negated: {!boolean})");
                        break;
                    case List<int> list:
                        Console.WriteLine($"  List: {string.Join(", ", list)} (Count: {list.Count})");
                        break;
                    case DateTime date:
                        Console.WriteLine($"  DateTime: {date:yyyy-MM-dd HH:mm:ss}");
                        break;
                    default:
                        Console.WriteLine($"  Unknown type: {entry.Value.GetType().Name}");
                        break;
                }
            }

            // Фильтрация по типу
            var numbers = polymorphicData.Values.OfType<int>().ToList();
            Console.WriteLine($"\nAll integers: {string.Join(", ", numbers)}");

            var collections = polymorphicData.Values.OfType<ICollection>().ToList();
            Console.WriteLine($"Collection items: {collections.Count}");
        }
    }

    // 79. Создайте List с использованием RemoveAll и RemoveRange
    public class ListBulkRemoval
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[79] List Bulk Removal Operations:");
            List<int> numbers = new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            Console.WriteLine($"Original list: {string.Join(", ", numbers)}");

            // RemoveAll с предикатом
            numbers.RemoveAll(n => n % 2 == 0); // Удалить четные
            Console.WriteLine($"After RemoveAll (even): {string.Join(", ", numbers)}");

            // RemoveRange
            numbers.RemoveRange(2, 3); // Удалить 3 элемента начиная с индекса 2
            Console.WriteLine($"After RemoveRange(2, 3): {string.Join(", ", numbers)}");

            // Комбинированные условия
            numbers.RemoveAll(n => n < 5 || n > 10);
            Console.WriteLine($"After RemoveAll (<5 or >10): {string.Join(", ", numbers)}");

            // Создание нового списка для демонстрации
            List<string> fruits = new List<string>
            {
                "apple", "banana", "cherry", "date", "elderberry",
                "fig", "grape", "kiwi", "lemon", "mango"
            };

            Console.WriteLine($"\nOriginal fruits: {string.Join(", ", fruits)}");

            // Удаление по длине строки
            fruits.RemoveAll(f => f.Length <= 3);
            Console.WriteLine($"After RemoveAll (length <= 3): {string.Join(", ", fruits)}");

            // Удаление по начальной букве
            fruits.RemoveAll(f => f.StartsWith("a") || f.StartsWith("b"));
            Console.WriteLine($"After RemoveAll (starts with a/b): {string.Join(", ", fruits)}");

            // Удаление диапазона
            if (fruits.Count >= 4)
            {
                fruits.RemoveRange(1, 2);
                Console.WriteLine($"After RemoveRange(1, 2): {string.Join(", ", fruits)}");
            }

            // Очистка всего списка
            fruits.Clear();
            Console.WriteLine($"After Clear: Count = {fruits.Count}");
        }
    }

    // 80. Реализуйте Dictionary<string, Dictionary<string, int>> для многомерных данных
    public class MultiDimensionalDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[80] Multi-dimensional Dictionary:");

            // Данные продаж по регионам и продуктам
            var salesData = new Dictionary<string, Dictionary<string, int>>
            {
                ["North"] = new Dictionary<string, int>
                {
                    ["Laptop"] = 150,
                    ["Mouse"] = 300,
                    ["Keyboard"] = 200,
                    ["Monitor"] = 75
                },
                ["South"] = new Dictionary<string, int>
                {
                    ["Laptop"] = 120,
                    ["Mouse"] = 250,
                    ["Keyboard"] = 180,
                    ["Monitor"] = 60
                },
                ["East"] = new Dictionary<string, int>
                {
                    ["Laptop"] = 180,
                    ["Mouse"] = 400,
                    ["Keyboard"] = 220,
                    ["Monitor"] = 90
                },
                ["West"] = new Dictionary<string, int>
                {
                    ["Laptop"] = 90,
                    ["Mouse"] = 200,
                    ["Keyboard"] = 150,
                    ["Monitor"] = 45
                }
            };

            // Вывод всех данных
            Console.WriteLine("Sales data by region:");
            foreach (var region in salesData)
            {
                Console.WriteLine($"\n{region.Key} Region:");
                foreach (var product in region.Value)
                {
                    Console.WriteLine($"  {product.Key}: {product.Value} units");
                }
            }

            // Агрегация данных
            var totalSalesByProduct = new Dictionary<string, int>();
            foreach (var region in salesData)
            {
                foreach (var product in region.Value)
                {
                    if (totalSalesByProduct.ContainsKey(product.Key))
                        totalSalesByProduct[product.Key] += product.Value;
                    else
                        totalSalesByProduct[product.Key] = product.Value;
                }
            }

            Console.WriteLine("\nTotal sales by product:");
            foreach (var product in totalSalesByProduct.OrderByDescending(kvp => kvp.Value))
            {
                Console.WriteLine($"  {product.Key}: {product.Value} units");
            }

            // Поиск лучшего региона для продукта
            string targetProduct = "Laptop";
            var bestRegion = salesData
                .OrderByDescending(region => region.Value.ContainsKey(targetProduct) ?
                                           region.Value[targetProduct] : 0)
                .First();

            Console.WriteLine($"\nBest region for {targetProduct}: " +
                            $"{bestRegion.Key} ({bestRegion.Value[targetProduct]} units)");

            // Добавление нового региона
            salesData["Central"] = new Dictionary<string, int>
            {
                ["Laptop"] = 110,
                ["Mouse"] = 280,
                ["Keyboard"] = 170,
                ["Monitor"] = 55
            };

            Console.WriteLine($"\nAfter adding Central region: {salesData.Count} regions total");
        }
    }

    // 81. Создайте List с сортировкой по пользовательскому компаратору
    public class CustomSortingDemo
    {
        public class Employee
        {
            public string Name { get; set; }
            public string Department { get; set; }
            public decimal Salary { get; set; }
            public int YearsOfService { get; set; }

            public override string ToString() =>
                $"{Name} ({Department}) - ${Salary:F2} - {YearsOfService} years";
        }

        public static void Demonstrate()
        {
            Console.WriteLine("\n[81] Custom Sorting with Comparer:");
            List<Employee> employees = new List<Employee>
            {
                new Employee { Name = "Alice", Department = "IT", Salary = 75000, YearsOfService = 5 },
                new Employee { Name = "Bob", Department = "HR", Salary = 60000, YearsOfService = 3 },
                new Employee { Name = "Charlie", Department = "IT", Salary = 80000, YearsOfService = 7 },
                new Employee { Name = "Diana", Department = "Finance", Salary = 90000, YearsOfService = 4 },
                new Employee { Name = "Eve", Department = "HR", Salary = 55000, YearsOfService = 2 },
                new Employee { Name = "Frank", Department = "IT", Salary = 70000, YearsOfService = 6 }
            };

            Console.WriteLine("Original list:");
            employees.ForEach(e => Console.WriteLine($"  {e}"));

            // Сортировка по зарплате (по возрастанию)
            employees.Sort((a, b) => a.Salary.CompareTo(b.Salary));
            Console.WriteLine("\nSorted by salary (ascending):");
            employees.ForEach(e => Console.WriteLine($"  {e}"));

            // Сортировка по стажу (по убыванию)
            employees.Sort((a, b) => b.YearsOfService.CompareTo(a.YearsOfService));
            Console.WriteLine("\nSorted by years of service (descending):");
            employees.ForEach(e => Console.WriteLine($"  {e}"));

            // Многокритериальная сортировка (отдел -> зарплата)
            employees.Sort((a, b) =>
            {
                int deptCompare = a.Department.CompareTo(b.Department);
                return deptCompare != 0 ? deptCompare : b.Salary.CompareTo(a.Salary);
            });
            Console.WriteLine("\nSorted by department then salary (descending):");
            employees.ForEach(e => Console.WriteLine($"  {e}"));

            // Использование компаратора
            var salaryComparer = Comparer<Employee>.Create((a, b) => a.Salary.CompareTo(b.Salary));
            employees.Sort(salaryComparer);
            Console.WriteLine("\nSorted using custom comparer (salary):");
            employees.ForEach(e => Console.WriteLine($"  {e}"));
        }
    }

    // 82. Реализуйте Dictionary<string, List> для группировки по категориям
    public class CategoryGroupingDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[82] Category Grouping Dictionary:");

            var productsByCategory = new Dictionary<string, List<Product>>();

            // Добавление продуктов по категориям
            productsByCategory["Electronics"] = new List<Product>
            {
                new Product { Name = "Smartphone", Price = 699.99m, Stock = 50 },
                new Product { Name = "Laptop", Price = 1299.99m, Stock = 25 },
                new Product { Name = "Tablet", Price = 399.99m, Stock = 30 }
            };

            productsByCategory["Home Appliances"] = new List<Product>
            {
                new Product { Name = "Refrigerator", Price = 899.99m, Stock = 10 },
                new Product { Name = "Washing Machine", Price = 599.99m, Stock = 8 },
                new Product { Name = "Microwave", Price = 149.99m, Stock = 20 }
            };

            productsByCategory["Furniture"] = new List<Product>
            {
                new Product { Name = "Office Chair", Price = 199.99m, Stock = 15 },
                new Product { Name = "Desk", Price = 299.99m, Stock = 12 },
                new Product { Name = "Bookshelf", Price = 159.99m, Stock = 18 }
            };

            // Вывод по категориям
            Console.WriteLine("Products grouped by category:");
            foreach (var category in productsByCategory)
            {
                Console.WriteLine($"\n{category.Key}:");
                foreach (var product in category.Value)
                {
                    Console.WriteLine($"  {product.Name} - ${product.Price:F2} (Stock: {product.Stock})");
                }
            }

            // Добавление нового продукта в существующую категорию
            if (productsByCategory.ContainsKey("Electronics"))
            {
                productsByCategory["Electronics"].Add(new Product
                {
                    Name = "Smart Watch",
                    Price = 249.99m,
                    Stock = 40
                });
            }

            // Создание новой категории
            productsByCategory["Clothing"] = new List<Product>
            {
                new Product { Name = "T-Shirt", Price = 19.99m, Stock = 100 },
                new Product { Name = "Jeans", Price = 49.99m, Stock = 60 }
            };

            // Статистика по категориям
            Console.WriteLine("\nCategory Statistics:");
            foreach (var category in productsByCategory)
            {
                int totalStock = category.Value.Sum(p => p.Stock);
                decimal totalValue = category.Value.Sum(p => p.Price * p.Stock);
                Console.WriteLine($"  {category.Key}: {category.Value.Count} products, " +
                                $"{totalStock} total stock, ${totalValue:F2} total value");
            }

            // Поиск продуктов по цене во всех категориях
            decimal maxPrice = 100;
            var affordableProducts = productsByCategory
                .SelectMany(kvp => kvp.Value.Where(p => p.Price <= maxPrice))
                .ToList();

            Console.WriteLine($"\nAffordable products (under ${maxPrice}):");
            foreach (var product in affordableProducts)
                Console.WriteLine($"  {product.Name} - ${product.Price:F2} ({product.Stock} in stock)");
        }
    }

    // 83. Создайте List с методами CopyTo, GetRange, InsertRange
    public class ListRangeOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[83] List Range Operations:");
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // CopyTo
            int[] array = new int[15];
            numbers.CopyTo(2, array, 5, 4);
            Console.WriteLine($"CopyTo result: {string.Join(", ", array)}");

            // GetRange
            List<int> sublist = numbers.GetRange(3, 5);
            Console.WriteLine($"GetRange(3, 5): {string.Join(", ", sublist)}");

            // InsertRange
            numbers.InsertRange(5, new List<int> { 100, 200, 300 });
            Console.WriteLine($"After InsertRange: {string.Join(", ", numbers)}");

            // RemoveRange
            numbers.RemoveRange(2, 3);
            Console.WriteLine($"After RemoveRange: {string.Join(", ", numbers)}");
        }
    }

    // 84. Реализуйте Dictionary<TKey, HashSet> для уникальных коллекций
    public class HashSetDictionaryDemo2
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[84] Dictionary with HashSet for Unique Collections:");

            // Теги по категориям
            var tagsByCategory = new Dictionary<string, HashSet<string>>();

            // Добавление тегов
            tagsByCategory["Programming"] = new HashSet<string>
            { "C#", "Java", "Python", "JavaScript", "C++" };

            tagsByCategory["Web"] = new HashSet<string>
            { "HTML", "CSS", "JavaScript", "React", "Vue" };

            tagsByCategory["Database"] = new HashSet<string>
            { "SQL", "MySQL", "PostgreSQL", "MongoDB", "Redis" };

            // Попытка добавить дубликаты (игнорируются)
            tagsByCategory["Programming"].Add("C#"); // Не добавится
            tagsByCategory["Programming"].Add("TypeScript"); // Добавится

            // Добавление новой категории
            tagsByCategory["DevOps"] = new HashSet<string>
            { "Docker", "Kubernetes", "AWS", "Azure", "CI/CD" };

            Console.WriteLine("Tags by category:");
            foreach (var category in tagsByCategory)
            {
                Console.WriteLine($"\n{category.Key}: {string.Join(", ", category.Value)}");
            }

            // Операции с множествами
            var programmingTags = tagsByCategory["Programming"];
            var webTags = tagsByCategory["Web"];

            // Пересечение (общие теги)
            var commonTags = programmingTags.Intersect(webTags).ToHashSet();
            Console.WriteLine($"\nCommon tags (Programming ∩ Web): {string.Join(", ", commonTags)}");

            // Объединение (все уникальные теги)
            var allTags = tagsByCategory.Values.SelectMany(set => set).ToHashSet();
            Console.WriteLine($"All unique tags: {string.Join(", ", allTags)}");

            // Разность (теги в Programming, но не в Web)
            var programmingOnly = programmingTags.Except(webTags).ToHashSet();
            Console.WriteLine($"Tags only in Programming: {string.Join(", ", programmingOnly)}");

            // Поиск тегов по паттерну
            var jsTags = allTags.Where(tag => tag.Contains("JS") || tag == "JavaScript").ToList();
            Console.WriteLine($"JavaScript-related tags: {string.Join(", ", jsTags)}");

            // Статистика
            Console.WriteLine($"\nStatistics:");
            Console.WriteLine($"  Total categories: {tagsByCategory.Count}");
            Console.WriteLine($"  Total unique tags: {allTags.Count}");

            var largestCategory = tagsByCategory.OrderByDescending(kvp => kvp.Value.Count).First();
            Console.WriteLine($"  Largest category: {largestCategory.Key} ({largestCategory.Value.Count} tags)");
        }
    }

    // 85. Создайте List для работы с индексами и срезами
    public class ListIndexingDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[85] List Indexing and Slicing Operations:");
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.WriteLine($"Original list: {string.Join(", ", numbers)}");

            // Базовые операции с индексами
            Console.WriteLine($"First element: numbers[0] = {numbers[0]}");
            Console.WriteLine($"Last element: numbers[^1] = {numbers[^1]}");
            Console.WriteLine($"Third from end: numbers[^3] = {numbers[^3]}");

            // Срезы с помощью GetRange
            var firstFive = numbers.GetRange(0, 5);
            Console.WriteLine($"First 5 elements: {string.Join(", ", firstFive)}");

            var lastThree = numbers.GetRange(numbers.Count - 3, 3);
            Console.WriteLine($"Last 3 elements: {string.Join(", ", lastThree)}");

            var middleSection = numbers.GetRange(3, 4);
            Console.WriteLine($"Elements 3-6: {string.Join(", ", middleSection)}");

            // Обновление через индексы
            numbers[0] = 100;
            numbers[^1] = 200;
            numbers[5] = numbers[5] * 10;
            Console.WriteLine($"After updates: {string.Join(", ", numbers)}");

            // Поиск индексов
            int indexOfFive = numbers.IndexOf(5);
            int lastIndexOfHundred = numbers.LastIndexOf(100);
            Console.WriteLine($"Index of 5: {indexOfFive}");
            Console.WriteLine($"Last index of 100: {lastIndexOfHundred}");

            // Обработка подсписков
            var sublist = numbers.GetRange(2, 6);
            sublist = sublist.Select(x => x * 2).ToList();
            Console.WriteLine($"Processed sublist (doubled): {string.Join(", ", sublist)}");

            // Замена диапазона
            numbers.RemoveRange(4, 3);
            numbers.InsertRange(4, new List<int> { 400, 500, 600 });
            Console.WriteLine($"After range replacement: {string.Join(", ", numbers)}");

            // Обратный порядок среза
            var reversedSlice = numbers.GetRange(2, 5);
            reversedSlice.Reverse();
            Console.WriteLine($"Reversed slice [2-6]: {string.Join(", ", reversedSlice)}");
        }
    }

    // 86. Реализуйте Dictionary<string, T> для типизированного хранилища
    public class TypedStorage<T> where T : class
    {
        private Dictionary<string, T> _storage = new Dictionary<string, T>();

        public void Store(string key, T item) => _storage[key] = item;

        public T Retrieve(string key) => _storage.TryGetValue(key, out T item) ? item : null;

        public bool Remove(string key) => _storage.Remove(key);

        public IEnumerable<string> GetAllKeys() => _storage.Keys;

        public IEnumerable<T> GetAllValues() => _storage.Values;

        public void Clear() => _storage.Clear();

        public int Count => _storage.Count;

        public void PrintContents()
        {
            Console.WriteLine($"Storage contents ({typeof(T).Name}):");
            foreach (var item in _storage)
                Console.WriteLine($"  {item.Key}: {item.Value}");
        }
    }

    // 87. Создайте List<Nullable> для работы с опциональными значениями
    public class NullableListDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[87] Nullable List Operations:");
            List<int?> nullableNumbers = new List<int?>
            {
                1, 2, null, 4, null, 6, 7, null, 9, 10
            };

            Console.WriteLine("Original nullable list:");
            for (int i = 0; i < nullableNumbers.Count; i++)
                Console.WriteLine($"  [{i}] = {nullableNumbers[i]?.ToString() ?? "null"}");

            // Фильтрация null значений
            var nonNullNumbers = nullableNumbers.Where(n => n.HasValue).Select(n => n.Value).ToList();
            Console.WriteLine($"\nNon-null values: {string.Join(", ", nonNullNumbers)}");

            // Замена null значений
            var filledNumbers = nullableNumbers.Select(n => n ?? -1).ToList();
            Console.WriteLine($"Nulls replaced with -1: {string.Join(", ", filledNumbers)}");

            // Статистика
            int nullCount = nullableNumbers.Count(n => !n.HasValue);
            int nonNullCount = nullableNumbers.Count(n => n.HasValue);
            double averageNonNull = nullableNumbers.Where(n => n.HasValue).Average(n => n.Value);

            Console.WriteLine($"\nStatistics:");
            Console.WriteLine($"  Total items: {nullableNumbers.Count}");
            Console.WriteLine($"  Non-null items: {nonNullCount}");
            Console.WriteLine($"  Null items: {nullCount}");
            Console.WriteLine($"  Average of non-null: {averageNonNull:F2}");

            // Работа с null при вычислениях
            var sum = nullableNumbers.Sum(n => n ?? 0);
            var max = nullableNumbers.Max();
            var min = nullableNumbers.Min();

            Console.WriteLine($"  Sum (nulls as 0): {sum}");
            Console.WriteLine($"  Max: {max}");
            Console.WriteLine($"  Min: {min}");

            // Группировка по наличию значения
            var grouped = nullableNumbers.GroupBy(n => n.HasValue ? "Has Value" : "Null");
            foreach (var group in grouped)
            {
                Console.WriteLine($"\n{group.Key}:");
                foreach (var item in group)
                    Console.WriteLine($"  {item?.ToString() ?? "null"}");
            }
        }
    }

    // 88. Реализуйте Dictionary<int, string> с методом TryGetValue
    public class TryGetValueDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[88] Dictionary TryGetValue Usage:");
            var errorCodes = new Dictionary<int, string>
            {
                [200] = "OK",
                [404] = "Not Found",
                [500] = "Internal Server Error",
                [403] = "Forbidden",
                [201] = "Created",
                [400] = "Bad Request"
            };

            // Тестовые коды для проверки
            int[] testCodes = { 200, 404, 301, 500, 999, 403 };

            Console.WriteLine("Testing error code lookup:");
            foreach (int code in testCodes)
            {
                if (errorCodes.TryGetValue(code, out string description))
                    Console.WriteLine($"  {code}: {description}");
                else
                    Console.WriteLine($"  {code}: Unknown error code");
            }

            // Безопасное обновление
            int codeToUpdate = 404;
            if (errorCodes.TryGetValue(codeToUpdate, out string currentDescription))
            {
                errorCodes[codeToUpdate] = $"{currentDescription} (Updated)";
                Console.WriteLine($"\nUpdated code {codeToUpdate}: {errorCodes[codeToUpdate]}");
            }

            // Безопасное удаление
            int codeToRemove = 301;
            if (errorCodes.ContainsKey(codeToRemove))
            {
                errorCodes.Remove(codeToRemove);
                Console.WriteLine($"Removed code {codeToRemove}");
            }
            else
            {
                Console.WriteLine($"Code {codeToRemove} not found for removal");
            }

            // Добавление с проверкой
            int newCode = 304;
            string newDescription = "Not Modified";

            if (!errorCodes.TryGetValue(newCode, out _))
            {
                errorCodes[newCode] = newDescription;
                Console.WriteLine($"Added new code {newCode}: {newDescription}");
            }

            Console.WriteLine($"\nFinal dictionary size: {errorCodes.Count} entries");
        }
    }

    // 89. Создайте List с использованием Capacity и Count
    public class ListCapacityDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[89] List Capacity and Count Management:");
            List<int> numbers = new List<int>();

            Console.WriteLine("Initial state:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Добавление элементов и отслеживание capacity
            for (int i = 1; i <= 20; i++)
            {
                numbers.Add(i);
                if (numbers.Count == numbers.Capacity)
                {
                    Console.WriteLine($"Capacity increased to {numbers.Capacity} after adding {i}");
                }
            }

            Console.WriteLine($"\nAfter adding 20 elements:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Уменьшение capacity
            numbers.TrimExcess();
            Console.WriteLine($"After TrimExcess:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Резервирование capacity
            numbers.Clear();
            numbers.Capacity = 100;
            Console.WriteLine($"\nAfter clearing and setting Capacity to 100:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Заполнение с зарезервированной capacity
            for (int i = 1; i <= 50; i++)
                numbers.Add(i * 2);

            Console.WriteLine($"After adding 50 elements with pre-allocated capacity:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Уменьшение до минимального необходимого
            numbers.TrimExcess();
            Console.WriteLine($"After final TrimExcess:");
            Console.WriteLine($"  Count: {numbers.Count}, Capacity: {numbers.Capacity}");

            // Сравнение производительности
            Console.WriteLine($"\nPerformance comparison:");
            var list1 = new List<int>();
            var list2 = new List<int>(1000);

            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++) list1.Add(i);
            watch1.Stop();

            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++) list2.Add(i);
            watch2.Stop();

            Console.WriteLine($"  Without pre-allocation: {watch1.ElapsedTicks} ticks");
            Console.WriteLine($"  With pre-allocation: {watch2.ElapsedTicks} ticks");
        }
    }

    // 90. Реализуйте Dictionary<string, decimal> для финансовых данных
    public class FinancialDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[90] Financial Data Dictionary:");
            var stockPrices = new Dictionary<string, decimal>
            {
                ["AAPL"] = 175.25m,
                ["GOOGL"] = 2850.75m,
                ["MSFT"] = 335.60m,
                ["AMZN"] = 3450.80m,
                ["TSLA"] = 850.30m,
                ["META"] = 325.45m
            };

            var currencyRates = new Dictionary<string, decimal>
            {
                ["USD"] = 1.00m,
                ["EUR"] = 0.85m,
                ["GBP"] = 0.73m,
                ["JPY"] = 110.50m,
                ["CAD"] = 1.25m
            };

            Console.WriteLine("Stock Prices:");
            foreach (var stock in stockPrices.OrderBy(s => s.Key))
                Console.WriteLine($"  {stock.Key}: ${stock.Value:F2}");

            Console.WriteLine("\nCurrency Exchange Rates (to USD):");
            foreach (var currency in currencyRates.OrderBy(c => c.Key))
                Console.WriteLine($"  1 {currency.Key} = ${currency.Value:F2}");

            // Финансовые операции
            decimal portfolioValue = CalculatePortfolioValue(stockPrices,
                new Dictionary<string, int> { ["AAPL"] = 10, ["GOOGL"] = 2, ["MSFT"] = 5 });
            Console.WriteLine($"\nPortfolio value: ${portfolioValue:F2}");

            // Конвертация валют
            decimal amountInEUR = 1000m;
            decimal amountInUSD = ConvertCurrency(amountInEUR, "EUR", "USD", currencyRates);
            Console.WriteLine($"€{amountInEUR:F2} = ${amountInUSD:F2}");

            // Обновление цен
            UpdateStockPrice(stockPrices, "AAPL", 180.50m);
            UpdateStockPrice(stockPrices, "TSLA", 820.75m);

            Console.WriteLine("\nUpdated Stock Prices:");
            foreach (var stock in stockPrices.OrderBy(s => s.Key))
                Console.WriteLine($"  {stock.Key}: ${stock.Value:F2}");

            // Финансовая статистика
            Console.WriteLine($"\nFinancial Statistics:");
            Console.WriteLine($"  Highest priced stock: ${stockPrices.Values.Max():F2}");
            Console.WriteLine($"  Lowest priced stock: ${stockPrices.Values.Min():F2}");
            Console.WriteLine($"  Average stock price: ${stockPrices.Values.Average():F2}");
            Console.WriteLine($"  Total stocks tracked: {stockPrices.Count}");
        }

        static decimal CalculatePortfolioValue(Dictionary<string, decimal> prices,
                                              Dictionary<string, int> holdings)
        {
            decimal total = 0;
            foreach (var holding in holdings)
            {
                if (prices.TryGetValue(holding.Key, out decimal price))
                    total += price * holding.Value;
            }
            return total;
        }

        static decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency,
                                      Dictionary<string, decimal> rates)
        {
            if (rates.TryGetValue(fromCurrency, out decimal fromRate) &&
                rates.TryGetValue(toCurrency, out decimal toRate))
            {
                return amount * (toRate / fromRate);
            }
            return 0;
        }

        static void UpdateStockPrice(Dictionary<string, decimal> prices,
                                   string symbol, decimal newPrice)
        {
            if (prices.ContainsKey(symbol))
                prices[symbol] = newPrice;
        }
    }

    // 91. Создайте List для реализации кастомной коллекции
    public class CustomCollection<T> : List<T>
    {
        public void AddRange(params T[] items) => base.AddRange(items);

        public bool IsEmpty => Count == 0;

        public T First => this[0];

        public T Last => this[^1];

        public CustomCollection<T> Where(Func<T, bool> predicate)
        {
            var result = new CustomCollection<T>();
            result.AddRange(this.Where(predicate).ToArray());
            return result;
        }

        public CustomCollection<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            var result = new CustomCollection<TResult>();
            result.AddRange(this.Select(selector).ToArray());
            return result;
        }

        public void ForEach(Action<T> action) => base.ForEach(action);

        public void Print(string title = "Collection")
        {
            Console.WriteLine($"{title} ({typeof(T).Name}):");
            this.ForEach(item => Console.WriteLine($"  {item}"));
        }
    }

    // 92. Реализуйте Dictionary<int, List<KeyValuePair<string, object>>>
    public class ComplexDictionaryDemo
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[92] Complex Dictionary Structure:");

            var userActivities = new Dictionary<int, List<KeyValuePair<string, object>>>
            {
                [1] = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Login", DateTime.Now.AddHours(-2)),
                    new KeyValuePair<string, object>("PageView", "Home"),
                    new KeyValuePair<string, object>("Purchase", 99.99m)
                },
                [2] = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Login", DateTime.Now.AddHours(-1)),
                    new KeyValuePair<string, object>("Search", "laptop"),
                    new KeyValuePair<string, object>("AddToCart", "Gaming Laptop"),
                    new KeyValuePair<string, object>("PageView", "Product Details")
                },
                [3] = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Login", DateTime.Now.AddMinutes(-30)),
                    new KeyValuePair<string, object>("Logout", DateTime.Now.AddMinutes(-25))
                }
            };

            Console.WriteLine("User Activities:");
            foreach (var user in userActivities)
            {
                Console.WriteLine($"\nUser {user.Key}:");
                foreach (var activity in user.Value)
                {
                    Console.WriteLine($"  {activity.Key}: {activity.Value} ({activity.Value.GetType().Name})");
                }
            }

            // Добавление новой активности
            AddUserActivity(userActivities, 2, "Review", "Great product!");
            AddUserActivity(userActivities, 4, "Login", DateTime.Now);

            // Анализ активностей
            Console.WriteLine("\nActivity Analysis:");
            foreach (var user in userActivities)
            {
                int activityCount = user.Value.Count;
                var lastActivity = user.Value.Last();
                Console.WriteLine($"User {user.Key}: {activityCount} activities, " +
                                $"last: {lastActivity.Key} at {lastActivity.Value}");
            }

            // Поиск пользователей по активности
            var usersWithPurchase = userActivities
                .Where(kvp => kvp.Value.Any(a => a.Key == "Purchase"))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Console.WriteLine($"\nUsers with purchases: {usersWithPurchase.Count}");
        }

        static void AddUserActivity(Dictionary<int, List<KeyValuePair<string, object>>> activities,
                                  int userId, string activityType, object activityData)
        {
            if (!activities.ContainsKey(userId))
                activities[userId] = new List<KeyValuePair<string, object>>();

            activities[userId].Add(new KeyValuePair<string, object>(activityType, activityData));
        }
    }

    // 93. Создайте List с использованием AddRange и InsertRange
    public class ListBulkOperations
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[93] List Bulk Operations with AddRange and InsertRange:");

            List<string> fruits = new List<string> { "apple", "banana" };
            Console.WriteLine($"Initial list: {string.Join(", ", fruits)}");

            // AddRange - добавление в конец
            fruits.AddRange(new[] { "cherry", "date", "elderberry" });
            Console.WriteLine($"After AddRange: {string.Join(", ", fruits)}");

            // InsertRange - вставка в середину
            fruits.InsertRange(2, new[] { "blueberry", "cranberry" });
            Console.WriteLine($"After InsertRange at index 2: {string.Join(", ", fruits)}");

            // Добавление из другого списка
            var tropicalFruits = new List<string> { "mango", "pineapple", "papaya" };
            fruits.AddRange(tropicalFruits);
            Console.WriteLine($"After AddRange from another list: {string.Join(", ", fruits)}");

            // Вставка в начало
            fruits.InsertRange(0, new[] { "first", "second" });
            Console.WriteLine($"After InsertRange at start: {string.Join(", ", fruits)}");

            // Комбинированные операции
            fruits.RemoveRange(0, 2); // Удаляем первые два
            fruits.InsertRange(fruits.Count - 2, new[] { "before_last_1", "before_last_2" });
            Console.WriteLine($"After combined operations: {string.Join(", ", fruits)}");

            // Массовое добавление с преобразованием
            var numbers = Enumerable.Range(1, 5).Select(n => $"Number_{n}").ToList();
            fruits.AddRange(numbers);
            Console.WriteLine($"After adding numbers: {string.Join(", ", fruits)}");

            // Очистка и массовое заполнение
            fruits.Clear();
            fruits.AddRange(Enumerable.Range(1, 10).Select(i => $"Item_{i}"));
            Console.WriteLine($"After clear and bulk add: {string.Join(", ", fruits)}");

            // Производительность массовых операций
            var bigList = new List<int>();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Медленный способ - по одному
            for (int i = 0; i < 10000; i++)
                bigList.Add(i);

            watch.Stop();
            Console.WriteLine($"\nIndividual adds: {watch.ElapsedMilliseconds}ms");

            bigList.Clear();
            watch.Restart();

            // Быстрый способ - AddRange
            bigList.AddRange(Enumerable.Range(0, 10000));

            watch.Stop();
            Console.WriteLine($"Bulk AddRange: {watch.ElapsedMilliseconds}ms");
        }
    }

    // 94. Реализуйте Dictionary<string, object> для JSON-подобных структур
    public class JsonLikeDictionary
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[94] JSON-like Dictionary Structure:");

            var personData = new Dictionary<string, object>
            {
                ["firstName"] = "John",
                ["lastName"] = "Doe",
                ["age"] = 30,
                ["isEmployed"] = true,
                ["address"] = new Dictionary<string, object>
                {
                    ["street"] = "123 Main St",
                    ["city"] = "New York",
                    ["state"] = "NY",
                    ["zipCode"] = "10001"
                },
                ["phoneNumbers"] = new List<object>
                {
                    new Dictionary<string, object> { ["type"] = "home", ["number"] = "212-555-1234" },
                    new Dictionary<string, object> { ["type"] = "mobile", ["number"] = "646-555-5678" }
                },
                ["children"] = new List<object>(),
                ["spouse"] = null
            };

            Console.WriteLine("Person Data Structure:");
            PrintDictionary(personData, 1);

            // Доступ к данным
            string firstName = personData["firstName"].ToString();
            var address = (Dictionary<string, object>)personData["address"];
            string city = address["city"].ToString();
            var phoneNumbers = (List<object>)personData["phoneNumbers"];

            Console.WriteLine($"\nExtracted Data:");
            Console.WriteLine($"  Name: {firstName} {personData["lastName"]}");
            Console.WriteLine($"  City: {city}");
            Console.WriteLine($"  Phone numbers: {phoneNumbers.Count}");

            // Динамическое добавление данных
            personData["email"] = "john.doe@example.com";
            personData["hobbies"] = new List<string> { "reading", "gaming", "hiking" };

            // Обновление данных
            personData["age"] = 31;
            address["street"] = "456 Oak Ave";

            Console.WriteLine($"\nAfter modifications:");
            PrintDictionary(personData, 1);

            // Поиск в структуре
            bool hasAddress = personData.ContainsKey("address");
            bool hasChildren = personData.ContainsKey("children") &&
                              ((List<object>)personData["children"]).Count > 0;

            Console.WriteLine($"\nValidation:");
            Console.WriteLine($"  Has address: {hasAddress}");
            Console.WriteLine($"  Has children: {hasChildren}");
        }

        static void PrintDictionary(Dictionary<string, object> dict, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);
            foreach (var entry in dict)
            {
                Console.Write($"{indent}{entry.Key}: ");

                switch (entry.Value)
                {
                    case Dictionary<string, object> nestedDict:
                        Console.WriteLine();
                        PrintDictionary(nestedDict, indentLevel + 1);
                        break;
                    case List<object> list:
                        Console.WriteLine($"[List with {list.Count} items]");
                        PrintList(list, indentLevel + 1);
                        break;
                    case null:
                        Console.WriteLine("null");
                        break;
                    default:
                        Console.WriteLine($"{entry.Value} ({entry.Value.GetType().Name})");
                        break;
                }
            }
        }

        static void PrintList(List<object> list, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);
            foreach (var item in list)
            {
                Console.Write($"{indent}- ");

                if (item is Dictionary<string, object> nestedDict)
                {
                    Console.WriteLine();
                    PrintDictionary(nestedDict, indentLevel + 1);
                }
                else
                {
                    Console.WriteLine($"{item} ({item?.GetType().Name})");
                }
            }
        }
    }

    // 95. Создайте List с использованием IndexOf и Contains
    public class ListSearchAdvanced
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[95] Advanced List Search Operations:");
            List<string> items = new List<string>
            {
                "apple", "banana", "cherry", "apple", "date", "elderberry",
                "fig", "grape", "apple", "kiwi", "lemon", "mango", "apple"
            };

            Console.WriteLine($"List: {string.Join(", ", items)}");

            // Базовые операции поиска
            bool containsBanana = items.Contains("banana");
            bool containsOrange = items.Contains("orange");
            Console.WriteLine($"\nContains 'banana': {containsBanana}");
            Console.WriteLine($"Contains 'orange': {containsOrange}");

            // Поиск индексов
            int firstApple = items.IndexOf("apple");
            int lastApple = items.LastIndexOf("apple");
            Console.WriteLine($"First 'apple' at index: {firstApple}");
            Console.WriteLine($"Last 'apple' at index: {lastApple}");

            // Поиск с начальной позиции
            int secondApple = items.IndexOf("apple", firstApple + 1);
            int thirdApple = items.IndexOf("apple", secondApple + 1);
            Console.WriteLine($"Second 'apple' at index: {secondApple}");
            Console.WriteLine($"Third 'apple' at index: {thirdApple}");

            // Поиск в диапазоне
            int appleInRange = items.IndexOf("apple", 3, 5);
            Console.WriteLine($"'apple' in range [3-7]: {appleInRange}");

            // Поиск с предикатом
            int firstLongWord = items.FindIndex(s => s.Length > 5);
            int lastLongWord = items.FindLastIndex(s => s.Length > 5);
            Console.WriteLine($"First long word (>5 chars) at index: {firstLongWord} ('{items[firstLongWord]}')");
            Console.WriteLine($"Last long word (>5 chars) at index: {lastLongWord} ('{items[lastLongWord]}')");

            // Поиск всех вхождений
            var allApples = items.Select((item, index) => new { Item = item, Index = index })
                                .Where(x => x.Item == "apple")
                                .ToList();
            Console.WriteLine($"All 'apple' occurrences: {string.Join(", ", allApples.Select(x => x.Index))}");

            // Проверка существования по условию
            bool hasShortWords = items.Exists(s => s.Length <= 3);
            bool hasVeryLongWords = items.Exists(s => s.Length > 10);
            Console.WriteLine($"Has short words (<=3 chars): {hasShortWords}");
            Console.WriteLine($"Has very long words (>10 chars): {hasVeryLongWords}");

            // Поиск всех элементов по условию
            var shortWords = items.FindAll(s => s.Length <= 3);
            var longWords = items.FindAll(s => s.Length > 5);
            Console.WriteLine($"Short words: {string.Join(", ", shortWords)}");
            Console.WriteLine($"Long words: {string.Join(", ", longWords)}");
        }
    }

    // 96. Реализуйте Dictionary<TKey, TValue> с очисткой и проверкой
    public class DictionaryMaintenance
    {
        public static void Demonstrate()
        {
            Console.WriteLine("\n[96] Dictionary Maintenance Operations:");
            var inventory = new Dictionary<string, int>
            {
                ["laptop"] = 15,
                ["mouse"] = 100,
                ["keyboard"] = 25,
                ["monitor"] = 8,
                ["headphones"] = 30,
                ["tablet"] = 12,
                ["camera"] = 5
            };

            Console.WriteLine("Initial inventory:");
            PrintInventory(inventory);

            // Проверка наличия перед операциями
            string itemToCheck = "mouse";
            if (inventory.ContainsKey(itemToCheck))
                Console.WriteLine($"\n{itemToCheck}: {inventory[itemToCheck]} in stock");

            itemToCheck = "printer";
            if (!inventory.ContainsKey(itemToCheck))
                Console.WriteLine($"{itemToCheck}: Not in inventory");

            // Безопасное обновление
            UpdateInventory(inventory, "laptop", -3); // Продали 3 ноутбука
            UpdateInventory(inventory, "mouse", 50); // Добавили 50 мышей
            UpdateInventory(inventory, "printer", 10); // Добавили новый товар

            Console.WriteLine("\nAfter updates:");
            PrintInventory(inventory);

            // Очистка нулевых остатков
            var zeroStockItems = inventory.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key).ToList();
            foreach (var item in zeroStockItems)
                inventory.Remove(item);

            Console.WriteLine($"\nRemoved {zeroStockItems.Count} items with zero stock");

            // Массовая проверка
            string[] itemsToVerify = { "laptop", "monitor", "scanner", "tablet" };
            Console.WriteLine("\nStock verification:");
            foreach (var item in itemsToVerify)
            {
                if (inventory.TryGetValue(item, out int stock))
                    Console.WriteLine($"  {item}: {stock} in stock");
                else
                    Console.WriteLine($"  {item}: NOT FOUND");
            }

            // Полная очистка
            inventory.Clear();
            Console.WriteLine($"\nAfter clear: {inventory.Count} items in inventory");

            // Перезаполнение
            inventory["smartphone"] = 20;
            inventory["watch"] = 15;
            Console.WriteLine("\nAfter refill:");
            PrintInventory(inventory);
        }

        static void UpdateInventory(Dictionary<string, int> inventory, string item, int change)
        {
            if (inventory.ContainsKey(item))
            {
                inventory[item] += change;
                if (inventory[item] < 0) inventory[item] = 0;
            }
            else if (change > 0)
            {
                inventory[item] = change;
            }
        }

        static void PrintInventory(Dictionary<string, int> inventory)
        {
            foreach (var item in inventory.OrderBy(kvp => kvp.Key))
                Console.WriteLine($"  {item.Key}: {item.Value}");
        }
    }

    // 97. Создайте List для работы с перечислениями
    public class EnumListDemo
    {
        public enum Status { Pending, Processing, Shipped, Delivered, Cancelled }

        public enum Priority { Low, Medium, High, Critical }

        [Flags]
        public enum Permissions { None = 0, Read = 1, Write = 2, Execute = 4, Delete = 8 }

        public static void Demonstrate()
        {
            Console.WriteLine("\n[97] List with Enumerations:");

            List<Status> orderStatuses = new List<Status>
        {
            Status.Pending,
            Status.Processing,
            Status.Shipped,
            Status.Pending,
            Status.Delivered,
            Status.Processing,
            Status.Cancelled
        };

            List<Priority> taskPriorities = new List<Priority>
        {
            Priority.Medium,
            Priority.High,
            Priority.Low,
            Priority.Critical,
            Priority.Medium,
            Priority.High
        };

            Console.WriteLine("Order Statuses:");
            foreach (var status in orderStatuses)
                Console.WriteLine($"  {status} ({(int)status})");

            Console.WriteLine("\nTask Priorities:");
            foreach (var priority in taskPriorities)
                Console.WriteLine($"  {priority} ({(int)priority})");

            // Статистика по перечислениям
            Console.WriteLine($"\nOrder Status Statistics:");
            var statusGroups = orderStatuses.GroupBy(s => s);
            foreach (var group in statusGroups.OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} orders");
            }

            Console.WriteLine($"\nPriority Statistics:");
            var priorityGroups = taskPriorities.GroupBy(p => p);
            foreach (var group in priorityGroups.OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} tasks");
            }

            // Фильтрация по перечислениям
            var completedOrders = orderStatuses.Where(s => s == Status.Delivered || s == Status.Cancelled).ToList();
            var highPriorityTasks = taskPriorities.Where(p => p == Priority.High || p == Priority.Critical).ToList();

            Console.WriteLine($"\nCompleted orders: {completedOrders.Count}");
            Console.WriteLine($"High priority tasks: {highPriorityTasks.Count}");

            // Преобразование в строки
            var statusStrings = orderStatuses.Select(s => s.ToString()).ToList();
            var priorityStrings = taskPriorities.Select(p => p.ToString()).ToList();

            Console.WriteLine($"\nStatus strings: {string.Join(", ", statusStrings)}");
            Console.WriteLine($"Priority strings: {string.Join(", ", priorityStrings)}");

            // Работа с флаговыми перечислениями
            List<Permissions> userPermissions = new List<Permissions>
        {
            Permissions.Read | Permissions.Write,
            Permissions.Read,
            Permissions.Read | Permissions.Write | Permissions.Execute,
            Permissions.Delete,
            Permissions.Read | Permissions.Execute
        };

            Console.WriteLine("\nUser Permissions:");
            foreach (var permission in userPermissions)
            {
                Console.WriteLine($"  {permission} ({(int)permission})");
            }

            // Проверка наличия конкретного разрешения
            var usersWithWrite = userPermissions.Where(p => p.HasFlag(Permissions.Write)).ToList();
            Console.WriteLine($"Users with Write permission: {usersWithWrite.Count}");
        }
    }

    // 98. Реализуйте Dictionary<string, Func<int, int>> для функций
    public class FunctionDictionary
{
    public static void Demonstrate()
    {
        Console.WriteLine("\n[98] Dictionary of Functions:");

        var mathOperations = new Dictionary<string, Func<int, int, int>>
        {
            ["add"] = (a, b) => a + b,
            ["subtract"] = (a, b) => a - b,
            ["multiply"] = (a, b) => a * b,
            ["divide"] = (a, b) => b != 0 ? a / b : 0,
            ["power"] = (a, b) => (int)Math.Pow(a, b),
            ["max"] = (a, b) => Math.Max(a, b),
            ["min"] = (a, b) => Math.Min(a, b)
        };

        var stringOperations = new Dictionary<string, Func<string, string, string>>
        {
            ["concat"] = (a, b) => a + b,
            ["join"] = (a, b) => $"{a} {b}",
            ["prefix"] = (a, b) => $"{a}_{b}",
            ["suffix"] = (a, b) => $"{b}_{a}"
        };

        // Выполнение математических операций
        int x = 10, y = 3;
        Console.WriteLine($"Math operations with {x} and {y}:");
        foreach (var operation in mathOperations)
        {
            int result = operation.Value(x, y);
            Console.WriteLine($"  {operation.Key}: {result}");
        }

        // Выполнение строковых операций
        string str1 = "Hello", str2 = "World";
        Console.WriteLine($"\nString operations with '{str1}' and '{str2}':");
        foreach (var operation in stringOperations)
        {
            string result = operation.Value(str1, str2);
            Console.WriteLine($"  {operation.Key}: '{result}'");
        }

        // Динамический выбор и выполнение функции
        string selectedOperation = "multiply";
        if (mathOperations.TryGetValue(selectedOperation, out var mathFunc))
        {
            int result = mathFunc(5, 4);
            Console.WriteLine($"\nDynamic execution: 5 {selectedOperation} 4 = {result}");
        }

        // Композиция функций
        var composedFunction = ComposeFunctions(
            mathOperations["add"],
            mathOperations["multiply"]
        );

        int composedResult = composedFunction(2, 3); // (2 + 3) * 4 = 20
        Console.WriteLine($"Composed function result: {composedResult}");

        // Цепочка операций
        var operationChain = new[] { "add", "multiply", "power" };
        int chainResult = ExecuteOperationChain(mathOperations, operationChain, 2, 3);
        Console.WriteLine($"Operation chain result: {chainResult}");
    }

    static Func<int, int, int> ComposeFunctions(Func<int, int, int> func1, Func<int, int, int> func2)
    {
        return (a, b) => func2(func1(a, b), 4); // Всегда умножаем на 4
    }

    static int ExecuteOperationChain(Dictionary<string, Func<int, int, int>> operations,
                                   string[] chain, int a, int b)
    {
        int result = a;
        foreach (var opName in chain)
        {
            if (operations.TryGetValue(opName, out var func))
                result = func(result, b);
        }
        return result;
    }
}

// 99. Создайте List для реализации паттерна Observer
public class ObserverPatternList<T>
{
    private List<T> _items = new List<T>();
    private List<Action<T>> _itemAddedObservers = new List<Action<T>>();
    private List<Action<T>> _itemRemovedObservers = new List<Action<T>>();
    private List<Action> _collectionChangedObservers = new List<Action>();

    public void Add(T item)
    {
        _items.Add(item);
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public bool Remove(T item)
    {
        bool removed = _items.Remove(item);
        if (removed)
        {
            NotifyItemRemoved(item);
            NotifyCollectionChanged();
        }
        return removed;
    }

    public void AddRange(IEnumerable<T> items)
    {
        _items.AddRange(items);
        foreach (var item in items)
            NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public void Clear()
    {
        var removedItems = _items.ToList();
        _items.Clear();
        foreach (var item in removedItems)
            NotifyItemRemoved(item);
        NotifyCollectionChanged();
    }

    public void SubscribeToItemAdded(Action<T> observer) => _itemAddedObservers.Add(observer);
    public void UnsubscribeFromItemAdded(Action<T> observer) => _itemAddedObservers.Remove(observer);

    public void SubscribeToItemRemoved(Action<T> observer) => _itemRemovedObservers.Add(observer);
    public void UnsubscribeFromItemRemoved(Action<T> observer) => _itemRemovedObservers.Remove(observer);

    public void SubscribeToCollectionChanged(Action observer) => _collectionChangedObservers.Add(observer);
    public void UnsubscribeFromCollectionChanged(Action observer) => _collectionChangedObservers.Remove(observer);

    private void NotifyItemAdded(T item) => _itemAddedObservers.ForEach(observer => observer(item));
    private void NotifyItemRemoved(T item) => _itemRemovedObservers.ForEach(observer => observer(item));
    private void NotifyCollectionChanged() => _collectionChangedObservers.ForEach(observer => observer());

    public int Count => _items.Count;
    public IReadOnlyList<T> Items => _items.AsReadOnly();

    public void PrintState()
    {
        Console.WriteLine($"ObserverList: {_items.Count} items");
        foreach (var item in _items)
            Console.WriteLine($"  {item}");
    }
}

// 100. Реализуйте Dictionary<DateTime, string> для логирования событий
public class EventLogger
{
    private Dictionary<DateTime, string> _events = new Dictionary<DateTime, string>();

    public void LogEvent(string eventType, string message)
    {
        DateTime timestamp = DateTime.Now;
        _events[timestamp] = $"{eventType}: {message}";
        Console.WriteLine($"[100] {timestamp:HH:mm:ss.fff} - {_events[timestamp]}");
    }

    public void PrintEvents(DateTime? startTime = null)
    {
        var filteredEvents = _events
            .Where(entry => !startTime.HasValue || entry.Key >= startTime.Value)
            .OrderBy(entry => entry.Key)
            .ToList();

        Console.WriteLine($"\n[100] Event Log ({filteredEvents.Count} events):");
        foreach (var entry in filteredEvents)
        {
            Console.WriteLine($"  {entry.Key:yyyy-MM-dd HH:mm:ss} - {entry.Value}");
        }
    }

    public void ClearEvents() => _events.Clear();
    public int GetEventCount() => _events.Count;
}

#endregion

// ==================== ДЕМОНСТРАЦИЯ ВСЕХ ЗАДАЧ ====================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ПОЛНАЯ ДЕМОНСТРАЦИЯ 100 ЗАДАЧ ПО GENERIC В C# ===\n");

        // Раздел 1: Ограничения generic типов
        DemonstrateSection1();

        // Раздел 2: Параметризированные коллекции  
        DemonstrateSection2();

        Console.WriteLine("\n=== ВСЕ 100 ЗАДАЧ УСПЕШНО ВЫПОЛНЕНЫ ===");
    }

    static void DemonstrateSection1()
    {
        Console.WriteLine("=== РАЗДЕЛ 1: ОГРАНИЧЕНИЯ GENERIC ТИПОВ (1-50) ===\n");

        // Задание 1
        var stringContainer = new ReferenceContainer<string>("Hello World");
        stringContainer.Print();

        // Задание 2
        var intContainer = new ValueContainer<int>(42);
        intContainer.Print();

        // Задание 3
        var newPerson = InstanceCreator.Create<Person>();
        Console.WriteLine($"[3] Created: {newPerson}");

        // Задание 4
        int max = ComparableHelper<int>.Max(10, 20);
        Console.WriteLine($"[4] Max of 10 and 20: {max}");

        // Задание 5
        var numbers = new List<int> { 1, 2, 3 };
        EnumerableProcessor.PrintAll(numbers);

        // Задание 9
        var repo = new Repository<Person>();
        var testPerson = new Person { Name = "Test", Age = 25, Email = "test@email.com" };
        repo.Add(testPerson);

        // Задание 10
        bool equal = ComparisonHelper.AreEqual(5, 5);
        Console.WriteLine($"[10] 5 == 5: {equal}");

        // Задание 31
        var validator = new GenericValidator<Person>();
        var person = new Person { Name = "John", Age = 25, Email = "john@email.com" };
        bool isValid = validator.Validate(person);
        Console.WriteLine($"[31] Person validation: {isValid}");

        // Задание 46
        int[] testArray = { 5, 2, 8, 1, 9 };
        int minValue = MinMaxFinder.FindMin(testArray);
        Console.WriteLine($"[46] Min value in array: {minValue}");
    }

    static void DemonstrateSection2()
    {
        Console.WriteLine("\n=== РАЗДЕЛ 2: ПАРАМЕТРИЗИРОВАННЫЕ КОЛЛЕКЦИИ (51-100) ===\n");

        // Задание 51
        ListBasicOperations.Demonstrate();

        // Задание 52
        ListFindOperations.Demonstrate();

        // Задание 53
        FrequencyCounter.Demonstrate();

        // Задание 54
        PersonListDemo.Demonstrate();

        // Задание 55
        GroupingDemo.Demonstrate();

        // Задание 56
        ListSortingOperations.Demonstrate();

        // Задание 57
        PriceDictionary.Demonstrate();

        // Задание 58
        ObjectDictionaryDemo.Demonstrate();

        // Задание 59
        TupleListDemo.Demonstrate();

        // Задание 60
        SortedDictionaryDemo.Demonstrate();

        // Задание 61
        HashSetDictionaryDemo1.Demonstrate();

        // Задание 62
        LinqListOperations.Demonstrate();

        // Задание 63
        DynamicDictionaryDemo.Demonstrate();

        // Задание 64
        QueueStackDemo.Demonstrate();

        // Задание 65
        ListFilterTransformDemo.Demonstrate();

        // Задание 83
        ListRangeOperations.Demonstrate();

        // Задание 100
        var logger = new EventLogger();
        logger.LogEvent("ApplicationStart", "Application started successfully");
        logger.LogEvent("UserLogin", "User Alice logged in");
        logger.LogEvent("DataProcessing", "Processed 150 records");
        logger.PrintEvents(DateTime.Now.AddMinutes(-1));
    }
}
}
