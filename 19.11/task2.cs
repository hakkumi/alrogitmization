using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatterns
{
    // 2.1: Паттерн Observer с обработкой исключений (ИСПРАВЛЕННАЯ ВЕРСИЯ)
    public interface IEvent
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public interface ISubscriber<T> where T : IEvent
    {
        string Id { get; }
        Task HandleAsync(T @event);
        bool CanHandle(T @event);
        int FailureCount { get; }
        TimeSpan AverageProcessingTime { get; }
    }

    public class SubscriberMetrics
    {
        public int TotalProcessed { get; set; }
        public int TotalFailed { get; set; }
        public TimeSpan TotalProcessingTime { get; set; }
        public DateTime LastProcessed { get; set; }

        public TimeSpan AverageProcessingTime =>
            TotalProcessed > 0 ? TimeSpan.FromTicks(TotalProcessingTime.Ticks / TotalProcessed) : TimeSpan.Zero;

        public double SuccessRate =>
            TotalProcessed > 0 ? (double)(TotalProcessed - TotalFailed) / TotalProcessed : 0;
    }

    public class EventBus
    {
        private readonly ConcurrentDictionary<Type, List<object>> _subscribers;
        private readonly ConcurrentQueue<(IEvent @event, object subscriber)> _deadLetterQueue;
        private readonly ConcurrentDictionary<string, SubscriberMetrics> _metrics;
        private readonly Timer _retryTimer;

        public EventBus()
        {
            _subscribers = new ConcurrentDictionary<Type, List<object>>();
            _deadLetterQueue = new ConcurrentQueue<(IEvent, object)>();
            _metrics = new ConcurrentDictionary<string, SubscriberMetrics>();
            _retryTimer = new Timer(ProcessDeadLetterQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public void Subscribe<T>(ISubscriber<T> subscriber) where T : IEvent
        {
            var eventType = typeof(T);
            _subscribers.AddOrUpdate(eventType,
                _ => new List<object> { subscriber },
                (_, list) => { list.Add(subscriber); return list; });

            _metrics.TryAdd(subscriber.Id, new SubscriberMetrics());
        }

        public void Unsubscribe<T>(string subscriberId) where T : IEvent
        {
            var eventType = typeof(T);
            if (_subscribers.TryGetValue(eventType, out var subscribers))
            {
                subscribers.RemoveAll(s => ((ISubscriber<T>)s).Id == subscriberId);
            }
            _metrics.TryRemove(subscriberId, out _);
        }

        public async Task PublishAsync<T>(T @event) where T : IEvent
        {
            if (_subscribers.TryGetValue(typeof(T), out var subscribers))
            {
                var tasks = subscribers
                    .Cast<ISubscriber<T>>()
                    .Where(s => s.CanHandle(@event))
                    .Select(async subscriber =>
                    {
                        var stopwatch = Stopwatch.StartNew();
                        try
                        {
                            await subscriber.HandleAsync(@event);
                            UpdateMetrics(subscriber.Id, true, stopwatch.Elapsed);
                        }
                        catch (Exception ex)
                        {
                            UpdateMetrics(subscriber.Id, false, stopwatch.Elapsed);
                            _deadLetterQueue.Enqueue((@event, subscriber));
                            Console.WriteLine($"Error processing event {@event.Type} by {subscriber.Id}: {ex.Message}");
                        }
                    });

                await Task.WhenAll(tasks);
            }
        }

        private void UpdateMetrics(string subscriberId, bool success, TimeSpan processingTime)
        {
            var metrics = _metrics.GetOrAdd(subscriberId, _ => new SubscriberMetrics());
            metrics.TotalProcessed++;
            if (!success) metrics.TotalFailed++;
            metrics.TotalProcessingTime += processingTime;
            metrics.LastProcessed = DateTime.UtcNow;
        }

        private async void ProcessDeadLetterQueue(object state)
        {
            while (_deadLetterQueue.TryDequeue(out var item))
            {
                var (@event, subscriber) = item;
                try
                {
                    // Используем рефлексию для вызова HandleAsync
                    var subscriberType = subscriber.GetType();
                    var handleMethod = subscriberType.GetMethod("HandleAsync");
                    if (handleMethod != null)
                    {
                        var task = (Task)handleMethod.Invoke(subscriber, new object[] { @event });
                        if (task != null)
                            await task;
                        Console.WriteLine($"Successfully processed dead letter event {@event.Type} by {subscriberType.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process dead letter event {@event.Type}: {ex.Message}");
                }
            }
        }

        public SubscriberMetrics GetSubscriberMetrics(string subscriberId)
        {
            return _metrics.TryGetValue(subscriberId, out var metrics) ? metrics : null;
        }
    }

    // Пример использования Observer
    public class UserCreatedEvent : IEvent
    {
        public string Type => "UserCreated";
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    public class EmailNotificationSubscriber : ISubscriber<UserCreatedEvent>
    {
        public string Id => "EmailNotification";
        public int FailureCount => 0;
        public TimeSpan AverageProcessingTime => TimeSpan.Zero;

        public bool CanHandle(UserCreatedEvent @event) => true;

        public async Task HandleAsync(UserCreatedEvent @event)
        {
            await Task.Delay(100);
            Console.WriteLine($"Sent welcome email to {@event.Email}");

            // Имитация случайных ошибок
            if (DateTime.Now.Millisecond % 10 == 0)
                throw new InvalidOperationException("Email service unavailable");
        }
    }

    // 2.2: Паттерн Decorator
    public interface ICalculator
    {
        int Add(int a, int b);
        int Multiply(int a, int b);
        double Divide(double a, double b);
    }

    public class SimpleCalculator : ICalculator
    {
        public int Add(int a, int b) => a + b;
        public int Multiply(int a, int b) => a * b;
        public double Divide(double a, double b) => a / b;
    }

    public abstract class CalculatorDecorator : ICalculator
    {
        protected readonly ICalculator _calculator;

        protected CalculatorDecorator(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public virtual int Add(int a, int b) => _calculator.Add(a, b);
        public virtual int Multiply(int a, int b) => _calculator.Multiply(a, b);
        public virtual double Divide(double a, double b) => _calculator.Divide(a, b);
    }

    public class LoggingDecorator : CalculatorDecorator
    {
        public LoggingDecorator(ICalculator calculator) : base(calculator) { }

        public override int Add(int a, int b)
        {
            Console.WriteLine($"Calling Add({a}, {b})");
            var result = base.Add(a, b);
            Console.WriteLine($"Add returned: {result}");
            return result;
        }

        public override int Multiply(int a, int b)
        {
            Console.WriteLine($"Calling Multiply({a}, {b})");
            var result = base.Multiply(a, b);
            Console.WriteLine($"Multiply returned: {result}");
            return result;
        }

        public override double Divide(double a, double b)
        {
            Console.WriteLine($"Calling Divide({a}, {b})");
            var result = base.Divide(a, b);
            Console.WriteLine($"Divide returned: {result}");
            return result;
        }
    }

    public class CachingDecorator : CalculatorDecorator
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        public CachingDecorator(ICalculator calculator) : base(calculator) { }

        public override int Add(int a, int b)
        {
            var key = $"Add_{a}_{b}";
            return (int)_cache.GetOrAdd(key, _ => base.Add(a, b));
        }

        public override int Multiply(int a, int b)
        {
            var key = $"Multiply_{a}_{b}";
            return (int)_cache.GetOrAdd(key, _ => base.Multiply(a, b));
        }

        public override double Divide(double a, double b)
        {
            var key = $"Divide_{a}_{b}";
            return (double)_cache.GetOrAdd(key, _ => base.Divide(a, b));
        }
    }

    public class ValidationDecorator : CalculatorDecorator
    {
        public ValidationDecorator(ICalculator calculator) : base(calculator) { }

        public override double Divide(double a, double b)
        {
            if (Math.Abs(b) < double.Epsilon)
                throw new ArgumentException("Division by zero is not allowed");

            return base.Divide(a, b);
        }
    }

    public class PerformanceDecorator : CalculatorDecorator
    {
        public PerformanceDecorator(ICalculator calculator) : base(calculator) { }

        public override int Add(int a, int b)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return base.Add(a, b);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Add operation took: {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }

    // 2.3: Паттерн Strategy
    public interface ISortStrategy<T> where T : IComparable<T>
    {
        string Name { get; }
        void Sort(T[] array);
        TimeSpan AverageExecutionTime { get; }
    }

    public class QuickSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
    {
        public string Name => "QuickSort";
        public TimeSpan AverageExecutionTime { get; private set; }
        private int _executionCount;

        public void Sort(T[] array)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                QuickSort(array, 0, array.Length - 1);
            }
            finally
            {
                stopwatch.Stop();
                UpdateAverageTime(stopwatch.Elapsed);
            }
        }

        private void QuickSort(T[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right);
                QuickSort(array, left, pivot - 1);
                QuickSort(array, pivot + 1, right);
            }
        }

        private int Partition(T[] array, int left, int right)
        {
            T pivot = array[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (array[j].CompareTo(pivot) <= 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }
            Swap(array, i + 1, right);
            return i + 1;
        }

        private void Swap(T[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        private void UpdateAverageTime(TimeSpan elapsed)
        {
            _executionCount++;
            AverageExecutionTime = TimeSpan.FromTicks(
                (AverageExecutionTime.Ticks * (_executionCount - 1) + elapsed.Ticks) / _executionCount);
        }
    }

    public class MergeSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
    {
        public string Name => "MergeSort";
        public TimeSpan AverageExecutionTime { get; private set; }
        private int _executionCount;

        public void Sort(T[] array)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                MergeSort(array, 0, array.Length - 1);
            }
            finally
            {
                stopwatch.Stop();
                UpdateAverageTime(stopwatch.Elapsed);
            }
        }

        private void MergeSort(T[] array, int left, int right)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;
                MergeSort(array, left, mid);
                MergeSort(array, mid + 1, right);
                Merge(array, left, mid, right);
            }
        }

        private void Merge(T[] array, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            T[] leftArray = new T[n1];
            T[] rightArray = new T[n2];

            Array.Copy(array, left, leftArray, 0, n1);
            Array.Copy(array, mid + 1, rightArray, 0, n2);

            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                if (leftArray[i].CompareTo(rightArray[j]) <= 0)
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }

        private void UpdateAverageTime(TimeSpan elapsed)
        {
            _executionCount++;
            AverageExecutionTime = TimeSpan.FromTicks(
                (AverageExecutionTime.Ticks * (_executionCount - 1) + elapsed.Ticks) / _executionCount);
        }
    }

    public class BubbleSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
    {
        public string Name => "BubbleSort";
        public TimeSpan AverageExecutionTime { get; private set; }
        private int _executionCount;

        public void Sort(T[] array)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j].CompareTo(array[j + 1]) > 0)
                        {
                            Swap(array, j, j + 1);
                        }
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
                UpdateAverageTime(stopwatch.Elapsed);
            }
        }

        private void Swap(T[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        private void UpdateAverageTime(TimeSpan elapsed)
        {
            _executionCount++;
            AverageExecutionTime = TimeSpan.FromTicks(
                (AverageExecutionTime.Ticks * (_executionCount - 1) + elapsed.Ticks) / _executionCount);
        }
    }

    public class SortContext<T> where T : IComparable<T>
    {
        private ISortStrategy<T> _strategy;
        private readonly Dictionary<string, ISortStrategy<T>> _strategies;

        public SortContext()
        {
            _strategies = new Dictionary<string, ISortStrategy<T>>
            {
                ["QuickSort"] = new QuickSortStrategy<T>(),
                ["MergeSort"] = new MergeSortStrategy<T>(),
                ["BubbleSort"] = new BubbleSortStrategy<T>()
            };

            _strategy = _strategies["QuickSort"];
        }

        public void SetStrategy(string strategyName)
        {
            if (_strategies.TryGetValue(strategyName, out var strategy))
            {
                _strategy = strategy;
            }
            else
            {
                throw new ArgumentException($"Unknown strategy: {strategyName}");
            }
        }

        public void SetStrategy(ISortStrategy<T> strategy)
        {
            _strategy = strategy;
            _strategies[strategy.Name] = strategy;
        }

        public void Sort(T[] array)
        {
            if (_strategy == null)
            {
                _strategy = SelectOptimalStrategy(array.Length);
            }

            Console.WriteLine($"Using {_strategy.Name} strategy for array of size {array.Length}");
            _strategy.Sort(array);
        }

        private ISortStrategy<T> SelectOptimalStrategy(int size)
        {
            return size switch
            {
                < 50 => _strategies["BubbleSort"],
                < 1000 => _strategies["QuickSort"],
                _ => _strategies["MergeSort"]
            };
        }

        public string GetBestStrategy()
        {
            return _strategies.Values
                .OrderBy(s => s.AverageExecutionTime)
                .First()
                .Name;
        }

        public void DisplayStrategyPerformance()
        {
            Console.WriteLine("Strategy Performance:");
            foreach (var strategy in _strategies.Values.OrderBy(s => s.AverageExecutionTime))
            {
                Console.WriteLine($"  {strategy.Name}: {strategy.AverageExecutionTime.TotalMilliseconds:F4}ms");
            }
        }
    }

    // 2.4: Паттерн Factory с регистрацией типов
    public interface IAnimal
    {
        string Name { get; }
        void Speak();
    }

    public class Dog : IAnimal
    {
        public string Name => "Dog";
        public void Speak() => Console.WriteLine("Woof!");
    }

    public class Cat : IAnimal
    {
        public string Name => "Cat";
        public void Speak() => Console.WriteLine("Meow!");
    }

    public class Bird : IAnimal
    {
        public string Name => "Bird";
        public void Speak() => Console.WriteLine("Tweet!");
    }

    public interface IAnimalFactory
    {
        IAnimal CreateAnimal(string type);
        void RegisterAnimal(string type, Type animalType);
        bool IsRegistered(string type);
        IEnumerable<string> GetRegisteredTypes();
    }

    public class AnimalFactory : IAnimalFactory
    {
        private readonly Dictionary<string, Type> _animalTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<IAnimal>> _animalConstructors = new Dictionary<string, Func<IAnimal>>();

        public AnimalFactory()
        {
            RegisterAnimal("Dog", typeof(Dog));
            RegisterAnimal("Cat", typeof(Cat));
            RegisterAnimal("Bird", typeof(Bird));
        }

        public IAnimal CreateAnimal(string type)
        {
            if (_animalConstructors.TryGetValue(type, out var constructor))
            {
                return constructor();
            }

            if (_animalTypes.TryGetValue(type, out var animalType))
            {
                return (IAnimal)Activator.CreateInstance(animalType);
            }

            throw new ArgumentException($"Animal type '{type}' is not registered");
        }

        public void RegisterAnimal(string type, Type animalType)
        {
            if (!typeof(IAnimal).IsAssignableFrom(animalType))
            {
                throw new ArgumentException($"Type must implement IAnimal interface");
            }

            _animalTypes[type] = animalType;

            var constructor = animalType.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                _animalConstructors[type] = () => (IAnimal)constructor.Invoke(null);
            }
            else
            {
                throw new ArgumentException($"Type must have a parameterless constructor");
            }
        }

        public void RegisterAnimal<T>(string type) where T : IAnimal, new()
        {
            _animalTypes[type] = typeof(T);
            _animalConstructors[type] = () => new T();
        }

        public bool IsRegistered(string type) => _animalTypes.ContainsKey(type);

        public IEnumerable<string> GetRegisteredTypes() => _animalTypes.Keys;
    }

    // 2.5: Паттерн Builder с fluent interface (ИСПРАВЛЕННАЯ ВЕРСИЯ)
    public class Computer
    {
        public string CPU { get; }
        public string GPU { get; }
        public int RAM { get; }
        public int Storage { get; }
        public bool HasSSD { get; }
        public string Motherboard { get; }
        public string PowerSupply { get; }

        private Computer(string cpu, string gpu, int ram, int storage, bool hasSSD, string motherboard, string powerSupply)
        {
            CPU = cpu;
            GPU = gpu;
            RAM = ram;
            Storage = storage;
            HasSSD = hasSSD;
            Motherboard = motherboard;
            PowerSupply = powerSupply;
        }

        public override string ToString()
        {
            return $"Computer: CPU={CPU}, GPU={GPU}, RAM={RAM}GB, Storage={Storage}GB, " +
                   $"SSD={HasSSD}, Motherboard={Motherboard}, PSU={PowerSupply}";
        }

        public class Builder
        {
            private string _cpu;
            private string _gpu;
            private int _ram;
            private int _storage;
            private bool _hasSSD;
            private string _motherboard = "Standard";
            private string _powerSupply = "500W";

            public Builder WithCPU(string cpu)
            {
                if (string.IsNullOrEmpty(cpu))
                    throw new ArgumentException("CPU cannot be null or empty");
                _cpu = cpu;
                return this;
            }

            public Builder WithGPU(string gpu)
            {
                if (string.IsNullOrEmpty(gpu))
                    throw new ArgumentException("GPU cannot be null or empty");
                _gpu = gpu;
                return this;
            }

            public Builder WithRAM(int ram)
            {
                if (ram < 4 || ram > 128)
                    throw new ArgumentException("RAM must be between 4 and 128 GB");
                _ram = ram;
                return this;
            }

            public Builder WithStorage(int storage)
            {
                if (storage < 128 || storage > 4096)
                    throw new ArgumentException("Storage must be between 128 and 4096 GB");
                _storage = storage;
                return this;
            }

            public Builder WithSSD(bool hasSSD = true)
            {
                _hasSSD = hasSSD;
                return this;
            }

            public Builder WithMotherboard(string motherboard)
            {
                if (string.IsNullOrEmpty(motherboard))
                    throw new ArgumentException("Motherboard cannot be null or empty");
                _motherboard = motherboard;
                return this;
            }

            public Builder WithPowerSupply(string powerSupply)
            {
                if (string.IsNullOrEmpty(powerSupply))
                    throw new ArgumentException("Power supply cannot be null or empty");
                _powerSupply = powerSupply;
                return this;
            }

            public Computer Build()
            {
                if (string.IsNullOrEmpty(_cpu))
                    throw new InvalidOperationException("CPU is required");
                if (string.IsNullOrEmpty(_gpu))
                    throw new InvalidOperationException("GPU is required");
                if (_ram == 0)
                    throw new InvalidOperationException("RAM is required");
                if (_storage == 0)
                    throw new InvalidOperationException("Storage is required");

                // Автоматически повышаем мощность блока питания для мощных GPU
                if (_gpu.StartsWith("RTX") && _powerSupply == "500W")
                {
                    Console.WriteLine("Warning: RTX GPU detected, automatically upgrading to 750W power supply");
                    _powerSupply = "750W";
                }

                return new Computer(_cpu, _gpu, _ram, _storage, _hasSSD, _motherboard, _powerSupply);
            }
        }

        public Builder ToBuilder()
        {
            return new Builder()
                .WithCPU(CPU)
                .WithGPU(GPU)
                .WithRAM(RAM)
                .WithStorage(Storage)
                .WithSSD(HasSSD)
                .WithMotherboard(Motherboard)
                .WithPowerSupply(PowerSupply);
        }
    }

    // 2.6: Паттерн Proxy для ленивой загрузки
    public interface IExpensiveResource : IDisposable
    {
        string Data { get; }
        bool IsLoaded { get; }
        void Process();
    }

    public class DatabaseResource : IExpensiveResource
    {
        private bool _disposed = false;

        public string Data { get; private set; }
        public bool IsLoaded => Data != null;

        public DatabaseResource()
        {
            Console.WriteLine("DatabaseResource: Creating connection...");
            Thread.Sleep(2000);
            LoadData();
        }

        private void LoadData()
        {
            Console.WriteLine("DatabaseResource: Loading data from database...");
            Thread.Sleep(1000);
            Data = "Large dataset from database";
            Console.WriteLine("DatabaseResource: Data loaded successfully");
        }

        public void Process()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DatabaseResource));
            if (!IsLoaded) LoadData();

            Console.WriteLine($"DatabaseResource: Processing data: {Data.Substring(0, Math.Min(20, Data.Length))}...");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine("DatabaseResource: Disposing resources...");
                Data = null;
                _disposed = true;
            }
        }
    }

    public class LazyLoadingProxy : IExpensiveResource
    {
        private Lazy<IExpensiveResource> _lazyResource;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

        public string Data => _lazyResource.Value.Data;
        public bool IsLoaded => _lazyResource.IsValueCreated && _lazyResource.Value.IsLoaded;

        public LazyLoadingProxy()
        {
            _lazyResource = new Lazy<IExpensiveResource>(() => new DatabaseResource(),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public void Process()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(LazyLoadingProxy));

            lock (_lockObject)
            {
                _lazyResource.Value.Process();
            }
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                if (!_disposed && _lazyResource.IsValueCreated)
                {
                    _lazyResource.Value.Dispose();
                    _disposed = true;
                }
            }
        }
    }

    public class CachingProxy : IExpensiveResource
    {
        private readonly IExpensiveResource _realResource;
        private readonly ConcurrentDictionary<string, object> _cache;
        private bool _disposed = false;

        public string Data => _realResource.Data;
        public bool IsLoaded => _realResource.IsLoaded;

        public CachingProxy(IExpensiveResource realResource)
        {
            _realResource = realResource;
            _cache = new ConcurrentDictionary<string, object>();
        }

        public void Process()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CachingProxy));

            var cacheKey = "processed_result";
            var result = _cache.GetOrAdd(cacheKey, _ =>
            {
                Console.WriteLine("CachingProxy: Cache miss, processing data...");
                _realResource.Process();
                return "processed_data";
            });

            Console.WriteLine($"CachingProxy: Using cached result: {result}");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _realResource.Dispose();
                _cache.Clear();
                _disposed = true;
            }
        }
    }

    // 2.7: Паттерн Chain of Responsibility
    public abstract class RequestHandler
    {
        protected RequestHandler _nextHandler;
        protected string _handlerName;

        protected RequestHandler(string handlerName)
        {
            _handlerName = handlerName;
        }

        public RequestHandler SetNext(RequestHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual async Task<RequestResult> HandleAsync(Request request)
        {
            var auditEntry = new AuditEntry
            {
                HandlerName = _handlerName,
                Timestamp = DateTime.UtcNow,
                RequestId = request.Id
            };

            try
            {
                Console.WriteLine($"{_handlerName}: Processing request {request.Id}");

                await PreProcessAsync(request);

                var canHandle = await CanHandleAsync(request);
                RequestResult result = null;

                if (canHandle)
                {
                    result = await ProcessAsync(request);
                    auditEntry.Success = true;
                    auditEntry.Result = result.Message;
                }
                else if (_nextHandler != null)
                {
                    Console.WriteLine($"{_handlerName}: Passing to next handler");
                    result = await _nextHandler.HandleAsync(request);
                }
                else
                {
                    result = RequestResult.Failure($"No handler can process request {request.Id}");
                    auditEntry.Success = false;
                    auditEntry.Error = result.Message;
                }

                await PostProcessAsync(request, result);

                request.AuditTrail.Add(auditEntry);
                return result;
            }
            catch (Exception ex)
            {
                auditEntry.Success = false;
                auditEntry.Error = ex.Message;
                request.AuditTrail.Add(auditEntry);
                throw;
            }
        }

        protected virtual Task PreProcessAsync(Request request) => Task.CompletedTask;
        protected abstract Task<bool> CanHandleAsync(Request request);
        protected abstract Task<RequestResult> ProcessAsync(Request request);
        protected virtual Task PostProcessAsync(Request request, RequestResult result) => Task.CompletedTask;
    }

    public class Request
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Type { get; set; }
        public string Content { get; set; }
        public Dictionary<string, object> Metadata { get; } = new Dictionary<string, object>();
        public List<AuditEntry> AuditTrail { get; } = new List<AuditEntry>();
        public bool IsCancelled { get; set; }
    }

    public class RequestResult
    {
        public bool Success { get; }
        public string Message { get; }
        public object Data { get; }

        private RequestResult(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static RequestResult SuccessResult(string message, object data = null)
            => new RequestResult(true, message, data);

        public static RequestResult Failure(string message)
            => new RequestResult(false, message);
    }

    public class AuditEntry
    {
        public string HandlerName { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; }
        public bool Success { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
    }

    public class AuthenticationHandler : RequestHandler
    {
        public AuthenticationHandler() : base("AuthenticationHandler") { }

        protected override async Task<bool> CanHandleAsync(Request request)
        {
            return request.Type == "API" || request.Type == "Web";
        }

        protected override async Task<RequestResult> ProcessAsync(Request request)
        {
            if (request.IsCancelled)
                return RequestResult.Failure("Request cancelled");

            await Task.Delay(100);

            var hasAuth = request.Metadata.ContainsKey("Authorization");
            if (!hasAuth)
                return RequestResult.Failure("Authentication failed");

            Console.WriteLine("AuthenticationHandler: User authenticated successfully");
            return RequestResult.SuccessResult("Authenticated");
        }
    }

    public class ValidationHandler : RequestHandler
    {
        public ValidationHandler() : base("ValidationHandler") { }

        protected override async Task<bool> CanHandleAsync(Request request)
        {
            return request.Type == "API";
        }

        protected override async Task<RequestResult> ProcessAsync(Request request)
        {
            if (request.IsCancelled)
                return RequestResult.Failure("Request cancelled");

            await Task.Delay(50);

            if (string.IsNullOrEmpty(request.Content))
                return RequestResult.Failure("Content cannot be empty");

            if (request.Content.Length > 1000)
                return RequestResult.Failure("Content too long");

            Console.WriteLine("ValidationHandler: Request validated successfully");
            return RequestResult.SuccessResult("Validated", request.Content);
        }
    }

    public class ProcessingHandler : RequestHandler
    {
        public ProcessingHandler() : base("ProcessingHandler") { }

        protected override async Task<bool> CanHandleAsync(Request request)
        {
            return request.Type == "API" && !string.IsNullOrEmpty(request.Content);
        }

        protected override async Task<RequestResult> ProcessAsync(Request request)
        {
            if (request.IsCancelled)
                return RequestResult.Failure("Request cancelled");

            await Task.Delay(200);

            var processedData = $"Processed: {request.Content.ToUpper()}";
            Console.WriteLine($"ProcessingHandler: Processed data: {processedData}");

            return RequestResult.SuccessResult("Processed", processedData);
        }
    }

    public class LoggingHandler : RequestHandler
    {
        public LoggingHandler() : base("LoggingHandler") { }

        protected override async Task<bool> CanHandleAsync(Request request)
        {
            return false;
        }

        protected override async Task<RequestResult> ProcessAsync(Request request)
        {
            return RequestResult.Failure("LoggingHandler should not process requests");
        }

        protected override async Task PreProcessAsync(Request request)
        {
            Console.WriteLine($"LoggingHandler: Starting request {request.Id} of type {request.Type}");
            await Task.CompletedTask;
        }

        protected override async Task PostProcessAsync(Request request, RequestResult result)
        {
            var status = result.Success ? "SUCCESS" : "FAILED";
            Console.WriteLine($"LoggingHandler: Request {request.Id} completed with status: {status}");
            await Task.CompletedTask;
        }
    }

    // 2.8: Паттерн Command для отмены/повтора операций
    public interface ICommand
    {
        string Name { get; }
        Task ExecuteAsync();
        Task UndoAsync();
        Task RedoAsync();
        bool CanUndo { get; }
    }

    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }
        public virtual bool CanUndo => true;

        protected readonly CommandHistory _history;
        private bool _executed = false;

        protected CommandBase(CommandHistory history)
        {
            _history = history;
        }

        public async Task ExecuteAsync()
        {
            if (!_executed)
            {
                await OnExecuteAsync();
                _executed = true;
            }
        }

        public async Task UndoAsync()
        {
            if (_executed && CanUndo)
            {
                await OnUndoAsync();
                _executed = false;
            }
        }

        public async Task RedoAsync()
        {
            if (!_executed)
            {
                await OnExecuteAsync();
                _executed = true;
            }
        }

        protected abstract Task OnExecuteAsync();
        protected abstract Task OnUndoAsync();
    }

    public class TextEditorCommand : CommandBase
    {
        private readonly TextDocument _document;
        private string _oldContent;
        private readonly int _position;
        private readonly string _text;
        private readonly bool _isInsert;

        public override string Name => _isInsert ? "Insert Text" : "Delete Text";

        public TextEditorCommand(TextDocument document, int position, string text, bool isInsert, CommandHistory history)
            : base(history)
        {
            _document = document;
            _position = position;
            _text = text;
            _isInsert = isInsert;
            _oldContent = document.Content;
        }

        protected override async Task OnExecuteAsync()
        {
            _oldContent = _document.Content;

            if (_isInsert)
            {
                _document.Insert(_position, _text);
                Console.WriteLine($"Inserted '{_text}' at position {_position}");
            }
            else
            {
                _document.Delete(_position, _text.Length);
                Console.WriteLine($"Deleted {_text.Length} characters from position {_position}");
            }

            await Task.CompletedTask;
        }

        protected override async Task OnUndoAsync()
        {
            _document.Content = _oldContent;
            Console.WriteLine($"Undo: {Name}");
            await Task.CompletedTask;
        }
    }

    public class MacroCommand : CommandBase
    {
        private readonly List<ICommand> _commands = new List<ICommand>();

        public override string Name => $"Macro ({_commands.Count} commands)";
        public override bool CanUndo => _commands.All(c => c.CanUndo);

        public MacroCommand(CommandHistory history) : base(history) { }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        protected override async Task OnExecuteAsync()
        {
            foreach (var command in _commands)
            {
                await command.ExecuteAsync();
            }
        }

        protected override async Task OnUndoAsync()
        {
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                await _commands[i].UndoAsync();
            }
        }
    }

    public class CommandHistory
    {
        private Stack<ICommand> _undoStack = new Stack<ICommand>();
        private Stack<ICommand> _redoStack = new Stack<ICommand>();
        private readonly object _lockObject = new object();
        private const int MAX_HISTORY_SIZE = 100;

        public async Task ExecuteAsync(ICommand command)
        {
            lock (_lockObject)
            {
                _redoStack.Clear();
            }

            await command.ExecuteAsync();

            lock (_lockObject)
            {
                if (_undoStack.Count >= MAX_HISTORY_SIZE)
                {
                    _undoStack.Clear();
                }
                _undoStack.Push(command);
            }
        }

        public async Task<bool> UndoAsync()
        {
            ICommand command;
            lock (_lockObject)
            {
                if (_undoStack.Count == 0) return false;
                command = _undoStack.Pop();
            }

            await command.UndoAsync();

            lock (_lockObject)
            {
                _redoStack.Push(command);
            }

            return true;
        }

        public async Task<bool> RedoAsync()
        {
            ICommand command;
            lock (_lockObject)
            {
                if (_redoStack.Count == 0) return false;
                command = _redoStack.Pop();
            }

            await command.RedoAsync();

            lock (_lockObject)
            {
                _undoStack.Push(command);
            }

            return true;
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _undoStack.Clear();
                _redoStack.Clear();
            }
        }

        public CommandHistorySnapshot CreateSnapshot()
        {
            lock (_lockObject)
            {
                return new CommandHistorySnapshot
                {
                    UndoStack = new Stack<ICommand>(_undoStack.Reverse()),
                    RedoStack = new Stack<ICommand>(_redoStack.Reverse())
                };
            }
        }

        public void RestoreSnapshot(CommandHistorySnapshot snapshot)
        {
            lock (_lockObject)
            {
                _undoStack = new Stack<ICommand>(snapshot.UndoStack.Reverse());
                _redoStack = new Stack<ICommand>(snapshot.RedoStack.Reverse());
            }
        }
    }

    public class CommandHistorySnapshot
    {
        public Stack<ICommand> UndoStack { get; set; }
        public Stack<ICommand> RedoStack { get; set; }
    }

    public class TextDocument
    {
        public string Content { get; set; } = string.Empty;

        public void Insert(int position, string text)
        {
            if (position < 0 || position > Content.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            Content = Content.Insert(position, text);
        }

        public void Delete(int position, int length)
        {
            if (position < 0 || position >= Content.Length)
                throw new ArgumentOutOfRangeException(nameof(position));
            if (length < 0 || position + length > Content.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            Content = Content.Remove(position, length);
        }
    }

    // 2.9: Паттерн Adapter для интеграции несовместимых систем
    public interface IDataProvider
    {
        Task<IEnumerable<DataRecord>> GetDataAsync();
        string SourceName { get; }
    }

    public class DataRecord
    {
        public string Id { get; set; }
        public Dictionary<string, object> Fields { get; } = new Dictionary<string, object>();
        public DateTime Timestamp { get; set; }
    }

    public class CsvDataAdapter : IDataProvider
    {
        private readonly CsvFileReader _csvReader;
        private readonly string _filePath;

        public string SourceName => $"CSV: {_filePath}";

        public CsvDataAdapter(string filePath)
        {
            _filePath = filePath;
            _csvReader = new CsvFileReader();
        }

        public async Task<IEnumerable<DataRecord>> GetDataAsync()
        {
            var csvData = await _csvReader.ReadCsvAsync(_filePath);
            var records = new List<DataRecord>();

            foreach (var row in csvData.Skip(1))
            {
                var record = new DataRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow
                };

                for (int i = 0; i < Math.Min(csvData[0].Length, row.Length); i++)
                {
                    var fieldName = csvData[0][i];
                    record.Fields[fieldName] = row[i];
                }

                records.Add(record);
            }

            return records;
        }
    }

    public class CsvFileReader
    {
        public async Task<string[][]> ReadCsvAsync(string filePath)
        {
            await Task.Delay(100);

            return new[]
            {
                new[] { "Name", "Age", "Email" },
                new[] { "John Doe", "30", "john@example.com" },
                new[] { "Jane Smith", "25", "jane@example.com" },
                new[] { "Bob Johnson", "35", "bob@example.com" }
            };
        }
    }

    public class JsonApiAdapter : IDataProvider
    {
        private readonly JsonApiClient _apiClient;
        private readonly string _endpoint;

        public string SourceName => $"JSON API: {_endpoint}";

        public JsonApiAdapter(string endpoint)
        {
            _endpoint = endpoint;
            _apiClient = new JsonApiClient();
        }

        public async Task<IEnumerable<DataRecord>> GetDataAsync()
        {
            var jsonData = await _apiClient.GetJsonDataAsync(_endpoint);
            var records = new List<DataRecord>();

            foreach (var item in jsonData)
            {
                var record = new DataRecord
                {
                    Id = item.ContainsKey("id") ? item["id"].ToString() : Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow
                };

                foreach (var prop in item)
                {
                    record.Fields[prop.Key] = prop.Value;
                }

                records.Add(record);
            }

            return records;
        }
    }

    public class JsonApiClient
    {
        public async Task<List<Dictionary<string, object>>> GetJsonDataAsync(string endpoint)
        {
            await Task.Delay(150);

            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    ["id"] = "user_1",
                    ["name"] = "John Doe",
                    ["age"] = 30,
                    ["active"] = true
                },
                new Dictionary<string, object>
                {
                    ["id"] = "user_2",
                    ["name"] = "Jane Smith",
                    ["age"] = 25,
                    ["active"] = false
                }
            };
        }
    }

    public class SqlDatabaseAdapter : IDataProvider
    {
        private readonly SqlDatabase _database;
        private readonly string _tableName;

        public string SourceName => $"SQL: {_tableName}";

        public SqlDatabaseAdapter(string connectionString, string tableName)
        {
            _database = new SqlDatabase(connectionString);
            _tableName = tableName;
        }

        public async Task<IEnumerable<DataRecord>> GetDataAsync()
        {
            var dataTable = await _database.ExecuteQueryAsync($"SELECT * FROM {_tableName}");
            var records = new List<DataRecord>();

            foreach (var row in dataTable.Rows)
            {
                var record = new DataRecord
                {
                    Id = row["Id"].ToString(),
                    Timestamp = DateTime.UtcNow
                };

                foreach (var column in dataTable.Columns)
                {
                    record.Fields[column] = row[column];
                }

                records.Add(record);
            }

            return records;
        }
    }

    public class SqlDatabase
    {
        private readonly string _connectionString;

        public SqlDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            await Task.Delay(200);

            return new DataTable
            {
                Columns = new[] { "Id", "Name", "Department", "Salary" },
                Rows = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["Id"] = 1,
                        ["Name"] = "Alice Brown",
                        ["Department"] = "Engineering",
                        ["Salary"] = 75000
                    },
                    new Dictionary<string, object>
                    {
                        ["Id"] = 2,
                        ["Name"] = "Charlie Wilson",
                        ["Department"] = "Marketing",
                        ["Salary"] = 65000
                    }
                }
            };
        }
    }

    public class DataTable
    {
        public string[] Columns { get; set; }
        public Dictionary<string, object>[] Rows { get; set; }
    }

    // 2.10: Паттерн Facade для упрощения сложной системы
    public class OrderProcessingFacade
    {
        private readonly InventoryService _inventoryService;
        private readonly PaymentService _paymentService;
        private readonly ShippingService _shippingService;
        private readonly NotificationService _notificationService;
        private readonly AuditService _auditService;

        public OrderProcessingFacade()
        {
            _inventoryService = new InventoryService();
            _paymentService = new PaymentService();
            _shippingService = new ShippingService();
            _notificationService = new NotificationService();
            _auditService = new AuditService();
        }

        public async Task<OrderResult> ProcessOrderAsync(Order order)
        {
            var result = new OrderResult { OrderId = order.Id };

            try
            {
                Console.WriteLine($"Starting order processing for order {order.Id}");

                var inventoryResult = await _inventoryService.CheckAvailabilityAsync(order.Items);
                if (!inventoryResult.IsAvailable)
                {
                    result.Success = false;
                    result.ErrorMessage = $"Items not available: {string.Join(", ", inventoryResult.UnavailableItems)}";
                    await _auditService.LogFailureAsync(order, result.ErrorMessage);
                    return result;
                }

                var paymentResult = await _paymentService.ProcessPaymentAsync(order.CustomerId, order.TotalAmount);
                if (!paymentResult.Success)
                {
                    result.Success = false;
                    result.ErrorMessage = $"Payment failed: {paymentResult.ErrorMessage}";
                    await _auditService.LogFailureAsync(order, result.ErrorMessage);
                    return result;
                }

                await _inventoryService.ReserveItemsAsync(order.Items);

                var shippingResult = await _shippingService.ArrangeShippingAsync(order);
                if (!shippingResult.Success)
                {
                    await _paymentService.RefundPaymentAsync(paymentResult.TransactionId);
                    await _inventoryService.ReleaseItemsAsync(order.Items);

                    result.Success = false;
                    result.ErrorMessage = $"Shipping failed: {shippingResult.ErrorMessage}";
                    await _auditService.LogFailureAsync(order, result.ErrorMessage);
                    return result;
                }

                await _notificationService.SendOrderConfirmationAsync(order.CustomerEmail, order.Id);
                await _notificationService.SendShippingNotificationAsync(order.CustomerEmail, shippingResult.TrackingNumber);

                result.Success = true;
                result.TransactionId = paymentResult.TransactionId;
                result.TrackingNumber = shippingResult.TrackingNumber;
                result.ShippingDate = shippingResult.ShippingDate;

                await _auditService.LogSuccessAsync(order, result);

                Console.WriteLine($"Order {order.Id} processed successfully");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Unexpected error: {ex.Message}";
                await _auditService.LogFailureAsync(order, result.ErrorMessage);
            }

            return result;
        }

        public async Task<bool> CancelOrderAsync(string orderId, string reason)
        {
            try
            {
                Console.WriteLine($"Cancelling order {orderId}");
                await _auditService.LogCancellationAsync(orderId, reason);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelling order {orderId}: {ex.Message}");
                return false;
            }
        }
    }

    public class InventoryService
    {
        public async Task<InventoryCheckResult> CheckAvailabilityAsync(List<OrderLineItem> items)
        {
            await Task.Delay(50);
            Console.WriteLine("InventoryService: Checking item availability");

            return new InventoryCheckResult
            {
                IsAvailable = true,
                UnavailableItems = new List<string>()
            };
        }

        public async Task ReserveItemsAsync(List<OrderLineItem> items)
        {
            await Task.Delay(30);
            Console.WriteLine("InventoryService: Reserving items");
        }

        public async Task ReleaseItemsAsync(List<OrderLineItem> items)
        {
            await Task.Delay(30);
            Console.WriteLine("InventoryService: Releasing items");
        }
    }

    public class PaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(string customerId, decimal amount)
        {
            await Task.Delay(100);
            Console.WriteLine($"PaymentService: Processing payment of {amount:C}");

            return new PaymentResult
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString()
            };
        }

        public async Task<bool> RefundPaymentAsync(string transactionId)
        {
            await Task.Delay(80);
            Console.WriteLine($"PaymentService: Refunding payment {transactionId}");
            return true;
        }
    }

    public class ShippingService
    {
        public async Task<ShippingResult> ArrangeShippingAsync(Order order)
        {
            await Task.Delay(120);
            Console.WriteLine("ShippingService: Arranging shipping");

            return new ShippingResult
            {
                Success = true,
                TrackingNumber = Guid.NewGuid().ToString().Substring(0, 12).ToUpper(),
                ShippingDate = DateTime.UtcNow.AddDays(1)
            };
        }
    }

    public class NotificationService
    {
        public async Task SendOrderConfirmationAsync(string email, string orderId)
        {
            await Task.Delay(40);
            Console.WriteLine($"NotificationService: Sent order confirmation to {email}");
        }

        public async Task SendShippingNotificationAsync(string email, string trackingNumber)
        {
            await Task.Delay(40);
            Console.WriteLine($"NotificationService: Sent shipping notification to {email}");
        }
    }

    public class AuditService
    {
        public async Task LogSuccessAsync(Order order, OrderResult result)
        {
            await Task.Delay(20);
            Console.WriteLine($"AuditService: Logged successful order {order.Id}");
        }

        public async Task LogFailureAsync(Order order, string error)
        {
            await Task.Delay(20);
            Console.WriteLine($"AuditService: Logged failed order {order.Id}: {error}");
        }

        public async Task LogCancellationAsync(string orderId, string reason)
        {
            await Task.Delay(20);
            Console.WriteLine($"AuditService: Logged cancelled order {orderId}: {reason}");
        }
    }

    public class Order
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderLineItem> Items { get; } = new List<OrderLineItem>();
    }

    public class OrderLineItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderResult
    {
        public bool Success { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class InventoryCheckResult
    {
        public bool IsAvailable { get; set; }
        public List<string> UnavailableItems { get; set; } = new List<string>();
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ShippingResult
    {
        public bool Success { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public string ErrorMessage { get; set; }
    }

    // 2.11: Паттерн Singleton с потокобезопасностью
    public sealed class ThreadSafeSingleton : ICloneable
    {
        private static readonly Lazy<ThreadSafeSingleton> _instance =
            new Lazy<ThreadSafeSingleton>(() => new ThreadSafeSingleton(),
            LazyThreadSafetyMode.ExecutionAndPublication);

        private static int _instanceCount = 0;
        private readonly DateTime _createdAt;

        public static ThreadSafeSingleton Instance => _instance.Value;
        public DateTime CreatedAt => _createdAt;
        public int InstanceNumber => _instanceCount;

        private ThreadSafeSingleton()
        {
            _instanceCount++;
            _createdAt = DateTime.UtcNow;
            Console.WriteLine($"ThreadSafeSingleton instance #{_instanceCount} created at {_createdAt:HH:mm:ss.fff}");
            Thread.Sleep(100);
        }

        public void DoWork()
        {
            Console.WriteLine($"Singleton working... Created at: {_createdAt:HH:mm:ss.fff}");
        }

        public object Clone()
        {
            throw new NotSupportedException("Singleton cannot be cloned");
        }
    }

    // 2.12: Паттерн Template Method с вариативностью
    public abstract class DataProcessor
    {
        public async Task<ProcessingResult> ProcessAsync(string data)
        {
            var result = new ProcessingResult();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Console.WriteLine($"{GetType().Name}: Starting data processing...");

                await PreValidateAsync(data);
                if (!await ValidateDataAsync(data))
                {
                    result.Success = false;
                    result.ErrorMessage = "Data validation failed";
                    return result;
                }

                await PreProcessAsync(data);
                var processedData = await ProcessDataAsync(data);

                if (await ShouldPostProcessAsync())
                {
                    processedData = await PostProcessAsync(processedData);
                }

                await FinalizeAsync(processedData);

                result.Success = true;
                result.ProcessedData = processedData;
                result.ProcessingTime = stopwatch.Elapsed;

                Console.WriteLine($"{GetType().Name}: Processing completed successfully in {result.ProcessingTime.TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                await HandleErrorAsync(ex);
            }
            finally
            {
                await CleanupAsync();
            }

            return result;
        }

        protected abstract Task<bool> ValidateDataAsync(string data);
        protected abstract Task<string> ProcessDataAsync(string data);

        protected virtual Task PreValidateAsync(string data) => Task.CompletedTask;
        protected virtual Task PreProcessAsync(string data) => Task.CompletedTask;
        protected virtual Task<string> PostProcessAsync(string data) => Task.FromResult(data);
        protected virtual Task FinalizeAsync(string data) => Task.CompletedTask;
        protected virtual Task HandleErrorAsync(Exception ex)
        {
            Console.WriteLine($"Error in {GetType().Name}: {ex.Message}");
            return Task.CompletedTask;
        }
        protected virtual Task CleanupAsync() => Task.CompletedTask;
        protected virtual Task<bool> ShouldPostProcessAsync() => Task.FromResult(true);
    }

    public class JsonDataProcessor : DataProcessor
    {
        protected override async Task<bool> ValidateDataAsync(string data)
        {
            await Task.Delay(50);
            if (string.IsNullOrEmpty(data)) return false;

            try
            {
                return data.Trim().StartsWith("{") && data.Trim().EndsWith("}");
            }
            catch
            {
                return false;
            }
        }

        protected override async Task<string> ProcessDataAsync(string data)
        {
            await Task.Delay(100);
            Console.WriteLine("JsonDataProcessor: Processing JSON data...");
            return $"{{\"processed\": true, \"original_length\": {data.Length}, \"timestamp\": \"{DateTime.UtcNow:O}\"}}";
        }

        protected override async Task<string> PostProcessAsync(string data)
        {
            await Task.Delay(30);
            Console.WriteLine("JsonDataProcessor: Applying JSON-specific post-processing...");
            return data.Replace("\"", "\\\"");
        }
    }

    public class XmlDataProcessor : DataProcessor
    {
        protected override async Task<bool> ValidateDataAsync(string data)
        {
            await Task.Delay(40);
            if (string.IsNullOrEmpty(data)) return false;
            return data.Trim().StartsWith("<") && data.Trim().EndsWith(">");
        }

        protected override async Task<string> ProcessDataAsync(string data)
        {
            await Task.Delay(120);
            Console.WriteLine("XmlDataProcessor: Processing XML data...");
            return $"<processed><original_length>{data.Length}</original_length><timestamp>{DateTime.UtcNow:O}</timestamp></processed>";
        }

        protected override Task<bool> ShouldPostProcessAsync() => Task.FromResult(false);
    }

    public class CsvDataProcessor : DataProcessor
    {
        protected override async Task<bool> ValidateDataAsync(string data)
        {
            await Task.Delay(30);
            return !string.IsNullOrEmpty(data) && data.Contains(",");
        }

        protected override async Task<string> ProcessDataAsync(string data)
        {
            await Task.Delay(80);
            Console.WriteLine("CsvDataProcessor: Processing CSV data...");
            var lines = data.Split('\n');
            var processedLines = lines.Select(line =>
                string.Join("|", line.Split(',').Select(field => field.Trim().ToUpper())));
            return string.Join("\n", processedLines);
        }

        protected override async Task PreProcessAsync(string data)
        {
            await Task.Delay(20);
            Console.WriteLine("CsvDataProcessor: Pre-processing CSV data...");
        }
    }

    public class ProcessingResult
    {
        public bool Success { get; set; }
        public string ProcessedData { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public string ErrorMessage { get; set; }
    }

    // 2.13: Паттерн State для конечных автоматов
    public interface IOrderState
    {
        string Name { get; }
        Task<bool> CanCancelAsync(OrderContext order);
        Task<bool> CanShipAsync(OrderContext order);
        Task<bool> CanDeliverAsync(OrderContext order);
        Task<bool> CanReturnAsync(OrderContext order);
        Task<TransitionResult> CancelAsync(OrderContext order);
        Task<TransitionResult> ShipAsync(OrderContext order);
        Task<TransitionResult> DeliverAsync(OrderContext order);
        Task<TransitionResult> ReturnAsync(OrderContext order);
    }

    public class OrderContext
    {
        public string OrderId { get; }
        public IOrderState CurrentState { get; set; }
        public List<StateTransition> TransitionHistory { get; }
        public DateTime CreatedAt { get; }
        public Dictionary<string, object> Metadata { get; }

        public OrderContext(string orderId)
        {
            OrderId = orderId;
            CurrentState = new NewOrderState();
            TransitionHistory = new List<StateTransition>();
            CreatedAt = DateTime.UtcNow;
            Metadata = new Dictionary<string, object>();
            RecordTransition("Created", CurrentState.Name);
        }

        public void RecordTransition(string action, string newState)
        {
            TransitionHistory.Add(new StateTransition
            {
                Timestamp = DateTime.UtcNow,
                FromState = TransitionHistory.LastOrDefault()?.ToState ?? "None",
                ToState = newState,
                Action = action
            });
        }

        public async Task<TransitionResult> CancelAsync() => await CurrentState.CancelAsync(this);
        public async Task<TransitionResult> ShipAsync() => await CurrentState.ShipAsync(this);
        public async Task<TransitionResult> DeliverAsync() => await CurrentState.DeliverAsync(this);
        public async Task<TransitionResult> ReturnAsync() => await CurrentState.ReturnAsync(this);

        public string GetVisualization()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Order {OrderId} State Diagram:");
            sb.AppendLine($"Current State: {CurrentState.Name}");
            sb.AppendLine("Transition History:");
            foreach (var transition in TransitionHistory)
            {
                sb.AppendLine($"  {transition.Timestamp:HH:mm:ss} {transition.FromState} -> {transition.ToState} ({transition.Action})");
            }
            return sb.ToString();
        }
    }

    public class StateTransition
    {
        public DateTime Timestamp { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string Action { get; set; }
    }

    public class TransitionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IOrderState NewState { get; set; }

        public static TransitionResult SuccessResult(string message, IOrderState newState)
            => new TransitionResult { Success = true, Message = message, NewState = newState };

        public static TransitionResult Failure(string message)
            => new TransitionResult { Success = false, Message = message };
    }

    // Конкретные состояния
    public class NewOrderState : IOrderState
    {
        public string Name => "New";

        public Task<bool> CanCancelAsync(OrderContext order) => Task.FromResult(true);
        public Task<bool> CanShipAsync(OrderContext order) => Task.FromResult(true);
        public Task<bool> CanDeliverAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanReturnAsync(OrderContext order) => Task.FromResult(false);

        public async Task<TransitionResult> CancelAsync(OrderContext order)
        {
            await Task.Delay(50);
            var newState = new CancelledOrderState();
            order.CurrentState = newState;
            order.RecordTransition("Cancel", newState.Name);
            return TransitionResult.SuccessResult("Order cancelled successfully", newState);
        }

        public async Task<TransitionResult> ShipAsync(OrderContext order)
        {
            await Task.Delay(50);
            var newState = new ShippedOrderState();
            order.CurrentState = newState;
            order.RecordTransition("Ship", newState.Name);
            return TransitionResult.SuccessResult("Order shipped successfully", newState);
        }

        public Task<TransitionResult> DeliverAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot deliver a new order"));

        public Task<TransitionResult> ReturnAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot return a new order"));
    }

    public class ShippedOrderState : IOrderState
    {
        public string Name => "Shipped";

        public Task<bool> CanCancelAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanShipAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanDeliverAsync(OrderContext order) => Task.FromResult(true);
        public Task<bool> CanReturnAsync(OrderContext order) => Task.FromResult(false);

        public Task<TransitionResult> CancelAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot cancel a shipped order"));

        public Task<TransitionResult> ShipAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Order already shipped"));

        public async Task<TransitionResult> DeliverAsync(OrderContext order)
        {
            await Task.Delay(50);
            var newState = new DeliveredOrderState();
            order.CurrentState = newState;
            order.RecordTransition("Deliver", newState.Name);
            return TransitionResult.SuccessResult("Order delivered successfully", newState);
        }

        public Task<TransitionResult> ReturnAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot return a shipped order"));
    }

    public class DeliveredOrderState : IOrderState
    {
        public string Name => "Delivered";

        public Task<bool> CanCancelAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanShipAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanDeliverAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanReturnAsync(OrderContext order) => Task.FromResult(true);

        public Task<TransitionResult> CancelAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot cancel a delivered order"));

        public Task<TransitionResult> ShipAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot ship a delivered order"));

        public Task<TransitionResult> DeliverAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Order already delivered"));

        public async Task<TransitionResult> ReturnAsync(OrderContext order)
        {
            await Task.Delay(50);
            var newState = new ReturnedOrderState();
            order.CurrentState = newState;
            order.RecordTransition("Return", newState.Name);
            return TransitionResult.SuccessResult("Order returned successfully", newState);
        }
    }

    public class CancelledOrderState : IOrderState
    {
        public string Name => "Cancelled";

        public Task<bool> CanCancelAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanShipAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanDeliverAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanReturnAsync(OrderContext order) => Task.FromResult(false);

        public Task<TransitionResult> CancelAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Order already cancelled"));

        public Task<TransitionResult> ShipAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot ship a cancelled order"));

        public Task<TransitionResult> DeliverAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot deliver a cancelled order"));

        public Task<TransitionResult> ReturnAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot return a cancelled order"));
    }

    public class ReturnedOrderState : IOrderState
    {
        public string Name => "Returned";

        public Task<bool> CanCancelAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanShipAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanDeliverAsync(OrderContext order) => Task.FromResult(false);
        public Task<bool> CanReturnAsync(OrderContext order) => Task.FromResult(false);

        public Task<TransitionResult> CancelAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot cancel a returned order"));

        public Task<TransitionResult> ShipAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot ship a returned order"));

        public Task<TransitionResult> DeliverAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Cannot deliver a returned order"));

        public Task<TransitionResult> ReturnAsync(OrderContext order)
            => Task.FromResult(TransitionResult.Failure("Order already returned"));
    }

    // 2.14: Паттерн Memento для сохранения состояния
    public interface IMemento
    {
        string Name { get; }
        DateTime CreatedAt { get; }
        object GetState();
    }

    public interface IOriginator
    {
        IMemento CreateMemento();
        void Restore(IMemento memento);
    }

    public class TextEditorMemento : IOriginator
    {
        private string _content;
        private string _font;
        private int _fontSize;
        private DateTime _lastModified;

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                _lastModified = DateTime.UtcNow;
            }
        }

        public string Font
        {
            get => _font;
            set
            {
                _font = value;
                _lastModified = DateTime.UtcNow;
            }
        }

        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _lastModified = DateTime.UtcNow;
            }
        }

        public DateTime LastModified => _lastModified;

        public TextEditorMemento(string content = "", string font = "Arial", int fontSize = 12)
        {
            _content = content;
            _font = font;
            _fontSize = fontSize;
            _lastModified = DateTime.UtcNow;
        }

        public void Type(string text)
        {
            Content += text;
            Console.WriteLine($"Typed: '{text}'. Content length: {Content.Length}");
        }

        public void SetFormatting(string font, int fontSize)
        {
            Font = font;
            FontSize = fontSize;
            Console.WriteLine($"Formatting set: Font={font}, Size={fontSize}");
        }

        public IMemento CreateMemento()
        {
            Console.WriteLine("Creating memento...");
            return new TextEditorSnapshot(this, _content, _font, _fontSize, _lastModified);
        }

        public void Restore(IMemento memento)
        {
            if (memento is TextEditorSnapshot textMemento)
            {
                _content = textMemento.Content;
                _font = textMemento.Font;
                _fontSize = textMemento.FontSize;
                _lastModified = textMemento.LastModified;
                Console.WriteLine($"Restored state from {memento.Name}");
            }
        }

        public override string ToString()
        {
            return $"Content: '{_content}' | Font: {_font} {_fontSize}pt | Modified: {_lastModified:HH:mm:ss}";
        }

        // Внутренний класс Memento
        private class TextEditorSnapshot : IMemento
        {
            public string Name { get; }
            public DateTime CreatedAt { get; }
            public string Content { get; }
            public string Font { get; }
            public int FontSize { get; }
            public DateTime LastModified { get; }

            public TextEditorSnapshot(TextEditorMemento editor, string content, string font, int fontSize, DateTime lastModified)
            {
                Name = $"Snapshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
                CreatedAt = DateTime.UtcNow;
                Content = content;
                Font = font;
                FontSize = fontSize;
                LastModified = lastModified;
            }

            public object GetState()
            {
                return new { Content, Font, FontSize, LastModified };
            }
        }
    }

    public class Caretaker
    {
        private readonly List<IMemento> _mementos = new List<IMemento>();
        private readonly IOriginator _originator;
        private readonly int _maxHistorySize;

        public int HistoryCount => _mementos.Count;

        public Caretaker(IOriginator originator, int maxHistorySize = 10)
        {
            _originator = originator;
            _maxHistorySize = maxHistorySize;
        }

        public void Backup()
        {
            var memento = _originator.CreateMemento();
            _mementos.Add(memento);

            if (_mementos.Count > _maxHistorySize)
            {
                _mementos.RemoveAt(0);
            }

            Console.WriteLine($"Backup created: {memento.Name}. Total backups: {_mementos.Count}");
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                Console.WriteLine("No backups available to undo");
                return;
            }

            var memento = _mementos[^1];
            _mementos.RemoveAt(_mementos.Count - 1);

            try
            {
                _originator.Restore(memento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during undo: {ex.Message}");
                _mementos.Add(memento);
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");
            foreach (var memento in _mementos)
            {
                Console.WriteLine($"  {memento.Name} - {memento.CreatedAt:HH:mm:ss}");
            }
        }

        public void CreateDeltaSnapshot()
        {
            if (_mementos.Count == 0)
            {
                Backup();
                return;
            }
            Backup();
            Console.WriteLine("Delta snapshot created (simplified implementation)");
        }
    }

    // 2.15: Паттерн Flyweight для экономии памяти
    public interface ITextCharacter
    {
        char Symbol { get; }
        string Font { get; }
        int Size { get; }
        string Color { get; }
        void Render(int position);
    }

    public class Character : ITextCharacter
    {
        public char Symbol { get; }
        public string Font { get; }
        public int Size { get; }
        public string Color { get; }

        public Character(char symbol, string font, int size, string color)
        {
            Symbol = symbol;
            Font = font;
            Size = size;
            Color = color;
            Thread.Sleep(5);
            Console.WriteLine($"Created character: '{symbol}' {font} {size}pt {color}");
        }

        public void Render(int position)
        {
            Console.WriteLine($"Rendering '{Symbol}' at position {position} with {Font} {Size}pt in {Color}");
        }

        public override string ToString()
        {
            return $"'{Symbol}' ({Font} {Size}pt {Color})";
        }
    }

    public class CharacterFactory
    {
        private readonly Dictionary<string, ITextCharacter> _characters = new Dictionary<string, ITextCharacter>();
        private int _objectsCreated = 0;
        private int _objectsReused = 0;

        public int ObjectsCreated => _objectsCreated;
        public int ObjectsReused => _objectsReused;
        public int TotalObjects => _characters.Count;

        public ITextCharacter GetCharacter(char symbol, string font = "Arial", int size = 12, string color = "Black")
        {
            var key = $"{symbol}_{font}_{size}_{color}";

            if (_characters.TryGetValue(key, out var character))
            {
                _objectsReused++;
                Console.WriteLine($"Reusing existing character: '{symbol}'");
                return character;
            }

            character = new Character(symbol, font, size, color);
            _characters[key] = character;
            _objectsCreated++;
            return character;
        }

        public void PrintStats()
        {
            Console.WriteLine($"Flyweight Factory Statistics:");
            Console.WriteLine($"  Total unique characters: {TotalObjects}");
            Console.WriteLine($"  Objects created: {ObjectsCreated}");
            Console.WriteLine($"  Objects reused: {ObjectsReused}");
            Console.WriteLine($"  Memory savings: {(double)ObjectsReused / (ObjectsCreated + ObjectsReused) * 100:F1}%");
        }
    }

    public class FlyweightTextDocument
    {
        private readonly List<(ITextCharacter character, int position)> _content = new List<(ITextCharacter, int)>();
        private readonly CharacterFactory _factory;

        public FlyweightTextDocument(CharacterFactory factory)
        {
            _factory = factory;
        }

        public void AddCharacter(char symbol, int position, string font = "Arial", int size = 12, string color = "Black")
        {
            var character = _factory.GetCharacter(symbol, font, size, color);
            _content.Add((character, position));
        }

        public void Render()
        {
            Console.WriteLine("Rendering document:");
            foreach (var (character, position) in _content.OrderBy(x => x.position))
            {
                character.Render(position);
            }
        }

        public void PrintContent()
        {
            var text = new string(_content.OrderBy(x => x.position)
                .Select(x => x.character.Symbol).ToArray());
            Console.WriteLine($"Document content: {text}");
        }
    }

    // 2.16: Паттерн Interpreter для языковых выражений
    public interface IExpression
    {
        int Interpret(Dictionary<string, int> context);
        void Accept(IExpressionVisitor visitor);
    }

    public class NumberExpression : IExpression
    {
        private readonly int _number;

        public NumberExpression(int number) => _number = number;

        public int Interpret(Dictionary<string, int> context) => _number;

        public void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
        public override string ToString() => _number.ToString();
    }

    public class VariableExpression : IExpression
    {
        private readonly string _name;

        public VariableExpression(string name) => _name = name;

        public int Interpret(Dictionary<string, int> context)
        {
            if (context.TryGetValue(_name, out int value))
                return value;
            throw new ArgumentException($"Variable '{_name}' not found in context");
        }

        public void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
        public override string ToString() => _name;
    }

    public class AddExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public AddExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(Dictionary<string, int> context)
            => _left.Interpret(context) + _right.Interpret(context);

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
            _left.Accept(visitor);
            _right.Accept(visitor);
        }

        public override string ToString() => $"({_left} + {_right})";
    }

    public class MultiplyExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public MultiplyExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(Dictionary<string, int> context)
            => _left.Interpret(context) * _right.Interpret(context);

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
            _left.Accept(visitor);
            _right.Accept(visitor);
        }

        public override string ToString() => $"({_left} * {_right})";
    }

    public class SubtractExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public SubtractExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(Dictionary<string, int> context)
            => _left.Interpret(context) - _right.Interpret(context);

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
            _left.Accept(visitor);
            _right.Accept(visitor);
        }

        public override string ToString() => $"({_left} - {_right})";
    }

    // Парсер для построения AST
    public class ExpressionParser
    {
        private readonly List<Token> _tokens;
        private int _position;

        public ExpressionParser(string expression)
        {
            _tokens = Tokenize(expression);
            _position = 0;
        }

        private List<Token> Tokenize(string expression)
        {
            var tokens = new List<Token>();
            var sb = new StringBuilder();

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (char.IsWhiteSpace(c))
                    continue;

                if (char.IsDigit(c))
                {
                    sb.Clear();
                    while (i < expression.Length && char.IsDigit(expression[i]))
                    {
                        sb.Append(expression[i++]);
                    }
                    i--;
                    tokens.Add(new Token(TokenType.Number, sb.ToString()));
                }
                else if (char.IsLetter(c))
                {
                    sb.Clear();
                    while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                    {
                        sb.Append(expression[i++]);
                    }
                    i--;
                    tokens.Add(new Token(TokenType.Variable, sb.ToString()));
                }
                else
                {
                    tokens.Add(c switch
                    {
                        '+' => new Token(TokenType.Plus, "+"),
                        '-' => new Token(TokenType.Minus, "-"),
                        '*' => new Token(TokenType.Multiply, "*"),
                        '(' => new Token(TokenType.LeftParenthesis, "("),
                        ')' => new Token(TokenType.RightParenthesis, ")"),
                        _ => throw new ArgumentException($"Invalid character: {c}")
                    });
                }
            }

            return tokens;
        }

        public IExpression Parse()
        {
            return ParseExpression();
        }

        private IExpression ParseExpression()
        {
            var left = ParseTerm();

            while (_position < _tokens.Count &&
                   (_tokens[_position].Type == TokenType.Plus ||
                    _tokens[_position].Type == TokenType.Minus))
            {
                var op = _tokens[_position++];
                var right = ParseTerm();

                left = op.Type == TokenType.Plus
                    ? new AddExpression(left, right)
                    : new SubtractExpression(left, right);
            }

            return left;
        }

        private IExpression ParseTerm()
        {
            var left = ParseFactor();

            while (_position < _tokens.Count &&
                   _tokens[_position].Type == TokenType.Multiply)
            {
                _position++;
                var right = ParseFactor();
                left = new MultiplyExpression(left, right);
            }

            return left;
        }

        private IExpression ParseFactor()
        {
            var token = _tokens[_position++];

            return token.Type switch
            {
                TokenType.Number => new NumberExpression(int.Parse(token.Value)),
                TokenType.Variable => new VariableExpression(token.Value),
                TokenType.LeftParenthesis =>
                    ParseExpression(), // После парсинга выражения в скобках ожидаем закрывающую скобку
                _ => throw new ArgumentException($"Unexpected token: {token.Type}")
            };
        }
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public enum TokenType
    {
        Number,
        Variable,
        Plus,
        Minus,
        Multiply,
        LeftParenthesis,
        RightParenthesis
    }

    // Visitor для AST
    public interface IExpressionVisitor
    {
        void Visit(NumberExpression expression);
        void Visit(VariableExpression expression);
        void Visit(AddExpression expression);
        void Visit(MultiplyExpression expression);
        void Visit(SubtractExpression expression);
    }

    public class ExpressionEvaluator : IExpressionVisitor
    {
        private readonly Dictionary<string, int> _context;
        private int _result;

        public ExpressionEvaluator(Dictionary<string, int> context)
        {
            _context = context;
        }

        public int Evaluate(IExpression expression)
        {
            expression.Accept(this);
            return _result;
        }

        public void Visit(NumberExpression expression) => _result = expression.Interpret(_context);
        public void Visit(VariableExpression expression) => _result = expression.Interpret(_context);
        public void Visit(AddExpression expression) => _result = expression.Interpret(_context);
        public void Visit(MultiplyExpression expression) => _result = expression.Interpret(_context);
        public void Visit(SubtractExpression expression) => _result = expression.Interpret(_context);
    }

    public class ExpressionPrinter : IExpressionVisitor
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public string GetResult() => _sb.ToString();

        public void Visit(NumberExpression expression) => _sb.Append(expression);
        public void Visit(VariableExpression expression) => _sb.Append(expression);
        public void Visit(AddExpression expression) => _sb.Append(expression);
        public void Visit(MultiplyExpression expression) => _sb.Append(expression);
        public void Visit(SubtractExpression expression) => _sb.Append(expression);
    }

    // 2.17: Паттерн Composite для древовидных структур
    public interface IFileSystemComponent
    {
        string Name { get; }
        long GetSize();
        void Display(string indent = "");
        void Accept(IFileSystemVisitor visitor);
    }

    public class File : IFileSystemComponent
    {
        public string Name { get; }
        public long Size { get; }

        public File(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public long GetSize() => Size;

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}📄 {Name} ({Size} bytes)");
        }

        public void Accept(IFileSystemVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Directory : IFileSystemComponent
    {
        public string Name { get; }
        private readonly List<IFileSystemComponent> _children = new List<IFileSystemComponent>();

        public Directory(string name)
        {
            Name = name;
        }

        public void AddComponent(IFileSystemComponent component)
        {
            _children.Add(component);
        }

        public void RemoveComponent(IFileSystemComponent component)
        {
            _children.Remove(component);
        }

        public long GetSize()
        {
            return _children.Sum(child => child.GetSize());
        }

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}📁 {Name}/ (total: {GetSize()} bytes)");
            foreach (var child in _children)
            {
                child.Display(indent + "  ");
            }
        }

        public void Accept(IFileSystemVisitor visitor)
        {
            visitor.Visit(this);
            foreach (var child in _children)
            {
                child.Accept(visitor);
            }
        }

        public IFileSystemComponent Find(string name)
        {
            if (Name == name) return this;

            foreach (var child in _children)
            {
                if (child is Directory dir)
                {
                    var result = dir.Find(name);
                    if (result != null) return result;
                }
                else if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
        }
    }

    // 2.18: Паттерн Visitor для операций над структурами
    public interface IFileSystemVisitor
    {
        void Visit(File file);
        void Visit(Directory directory);
    }

    public class SizeCalculatorVisitor : IFileSystemVisitor
    {
        public long TotalSize { get; private set; }

        public void Visit(File file)
        {
            TotalSize += file.Size;
        }

        public void Visit(Directory directory)
        {
            // Directory size is calculated from files
        }
    }

    public class SearchVisitor : IFileSystemVisitor
    {
        private readonly string _searchPattern;
        public List<IFileSystemComponent> Results { get; } = new List<IFileSystemComponent>();

        public SearchVisitor(string searchPattern)
        {
            _searchPattern = searchPattern;
        }

        public void Visit(File file)
        {
            if (file.Name.Contains(_searchPattern))
                Results.Add(file);
        }

        public void Visit(Directory directory)
        {
            if (directory.Name.Contains(_searchPattern))
                Results.Add(directory);
        }
    }

    public class FileCounterVisitor : IFileSystemVisitor
    {
        public int FileCount { get; private set; }
        public int DirectoryCount { get; private set; }

        public void Visit(File file)
        {
            FileCount++;
        }

        public void Visit(Directory directory)
        {
            DirectoryCount++;
        }
    }

    // 2.19: SOLID принципы - проектирование системы
    public interface IOrder
    {
        string Id { get; }
        decimal CalculateTotal();
        bool IsValid();
    }

    public interface IOrderRepository
    {
        void Save(IOrder order);
        IOrder GetById(string id);
        IEnumerable<IOrder> GetAll();
    }

    public interface INotificationService
    {
        void SendOrderConfirmation(IOrder order);
        void SendShippingNotification(IOrder order);
    }

    public interface IPriceCalculator
    {
        decimal CalculatePrice(IOrder order);
    }

    // Single Responsibility: Каждый класс имеет одну ответственность
    public class OnlineOrder : IOrder
    {
        public string Id { get; }
        public List<OrderItem> Items { get; } = new List<OrderItem>();
        public CustomerInfo Customer { get; }
        public decimal TaxRate { get; } = 0.1m;

        public OnlineOrder(string id, CustomerInfo customer)
        {
            Id = id;
            Customer = customer;
        }

        public decimal CalculateTotal()
        {
            var subtotal = Items.Sum(item => item.Price * item.Quantity);
            var tax = subtotal * TaxRate;
            return subtotal + tax;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Id) &&
                   Items.Count > 0 &&
                   Items.All(item => item.IsValid()) &&
                   Customer.IsValid();
        }
    }

    public class OrderItem
    {
        public string ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }
        public int Quantity { get; }

        public OrderItem(string productId, string productName, decimal price, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ProductId) &&
                   Price >= 0 &&
                   Quantity > 0;
        }
    }

    public class CustomerInfo
    {
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }

        public CustomerInfo(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Id) &&
                   !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Email);
        }
    }

    // Open/Closed: Открыт для расширения, закрыт для модификации
    public class DiscountedOrder : OnlineOrder
    {
        public decimal Discount { get; }

        public DiscountedOrder(string id, CustomerInfo customer, decimal discount)
            : base(id, customer)
        {
            Discount = discount;
        }

        public new decimal CalculateTotal()
        {
            var total = base.CalculateTotal();
            return total * (1 - Discount);
        }
    }

    // Liskov Substitution: Подтипы заменяемы
    public class ExpressOrder : OnlineOrder
    {
        public decimal ExpressShippingFee { get; } = 15.0m;

        public ExpressOrder(string id, CustomerInfo customer)
            : base(id, customer) { }

        public new decimal CalculateTotal()
        {
            return base.CalculateTotal() + ExpressShippingFee;
        }
    }

    // Interface Segregation: Специфичные интерфейсы
    public interface IPersistable
    {
        void Save();
        void Load();
    }

    public interface IValidatable
    {
        bool IsValid();
    }

    public interface ICalculatable
    {
        decimal CalculateTotal();
    }

    // Dependency Inversion: Зависимость от абстракций
    public class OrderProcessor
    {
        private readonly IOrderRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IPriceCalculator _priceCalculator;

        public OrderProcessor(
            IOrderRepository repository,
            INotificationService notificationService,
            IPriceCalculator priceCalculator)
        {
            _repository = repository;
            _notificationService = notificationService;
            _priceCalculator = priceCalculator;
        }

        public void ProcessOrder(IOrder order)
        {
            if (!order.IsValid())
                throw new ArgumentException("Invalid order");

            _repository.Save(order);
            _notificationService.SendOrderConfirmation(order);

            var total = _priceCalculator.CalculatePrice(order);
            Console.WriteLine($"Order processed. Total: {total:C}");
        }
    }

    // Конкретные реализации
    public class SqlOrderRepository : IOrderRepository
    {
        private readonly Dictionary<string, IOrder> _orders = new Dictionary<string, IOrder>();

        public void Save(IOrder order)
        {
            _orders[order.Id] = order;
            Console.WriteLine($"Order {order.Id} saved to database");
        }

        public IOrder GetById(string id)
        {
            return _orders.TryGetValue(id, out var order) ? order : null;
        }

        public IEnumerable<IOrder> GetAll()
        {
            return _orders.Values;
        }
    }

    public class EmailNotificationService : INotificationService
    {
        public void SendOrderConfirmation(IOrder order)
        {
            Console.WriteLine($"Order confirmation email sent for order {order.Id}");
        }

        public void SendShippingNotification(IOrder order)
        {
            Console.WriteLine($"Shipping notification email sent for order {order.Id}");
        }
    }

    public class DefaultPriceCalculator : IPriceCalculator
    {
        public decimal CalculatePrice(IOrder order)
        {
            return order.CalculateTotal();
        }
    }

    // 2.20: Паттерн Repository для доступа к данным
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();
    }

    public interface IUnitOfWork : IDisposable
    {
        IRepository<CustomerEntity> Customers { get; }
        IRepository<OrderEntity> Orders { get; }
        IRepository<ProductEntity> Products { get; }
        int Commit();
        void Rollback();
    }

    // Переименованные классы для избежания конфликтов
    public class CustomerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }

    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public List<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }

    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _context;
        private readonly List<T> _added = new List<T>();
        private readonly List<T> _updated = new List<T>();
        private readonly List<T> _deleted = new List<T>();

        public GenericRepository(List<T> context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            var property = typeof(T).GetProperty("Id");
            return _context.FirstOrDefault(e => (int)property.GetValue(e) == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.AsReadOnly();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.AsQueryable().Where(predicate);
        }

        public void Add(T entity)
        {
            _added.Add(entity);
        }

        public void Update(T entity)
        {
            _updated.Add(entity);
        }

        public void Delete(T entity)
        {
            _deleted.Add(entity);
        }

        public int SaveChanges()
        {
            foreach (var entity in _added)
            {
                _context.Add(entity);
            }

            foreach (var entity in _updated)
            {
                // In a real implementation, this would update the entity in the context
            }

            foreach (var entity in _deleted)
            {
                _context.Remove(entity);
            }

            int changes = _added.Count + _updated.Count + _deleted.Count;
            _added.Clear();
            _updated.Clear();
            _deleted.Clear();

            return changes;
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly List<CustomerEntity> _customers = new List<CustomerEntity>();
        private readonly List<OrderEntity> _orders = new List<OrderEntity>();
        private readonly List<ProductEntity> _products = new List<ProductEntity>();

        public IRepository<CustomerEntity> Customers { get; }
        public IRepository<OrderEntity> Orders { get; }
        public IRepository<ProductEntity> Products { get; }

        public UnitOfWork()
        {
            Customers = new GenericRepository<CustomerEntity>(_customers);
            Orders = new GenericRepository<OrderEntity>(_orders);
            Products = new GenericRepository<ProductEntity>(_products);
        }

        public int Commit()
        {
            var changes = 0;
            changes += ((GenericRepository<CustomerEntity>)Customers).SaveChanges();
            changes += ((GenericRepository<OrderEntity>)Orders).SaveChanges();
            changes += ((GenericRepository<ProductEntity>)Products).SaveChanges();
            return changes;
        }

        public void Rollback()
        {
            // In a real implementation, this would rollback transactions
            Console.WriteLine("Rollback performed");
        }

        public void Dispose()
        {
            // Cleanup resources
        }
    }

    // LINQ расширения для репозитория
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IRepository<T> repository, int page, int pageSize)
            where T : class
        {
            return repository.GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public static T FirstOrDefault<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
            where T : class
        {
            return repository.Find(predicate).FirstOrDefault();
        }

        public static bool Any<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
            where T : class
        {
            return repository.Find(predicate).Any();
        }
    }

    // Кэширующий декоратор для репозитория
    public class CachingRepository<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _innerRepository;
        private readonly ConcurrentDictionary<int, T> _cache = new ConcurrentDictionary<int, T>();

        public CachingRepository(IRepository<T> innerRepository)
        {
            _innerRepository = innerRepository;
        }

        public T GetById(int id)
        {
            return _cache.GetOrAdd(id, _ => _innerRepository.GetById(id));
        }

        public IEnumerable<T> GetAll()
        {
            // Не кэшируем GetAll, так как данные могут часто меняться
            return _innerRepository.GetAll();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _innerRepository.Find(predicate);
        }

        public void Add(T entity)
        {
            _innerRepository.Add(entity);
            // Инвалидируем кэш при добавлении
            var id = GetEntityId(entity);
            _cache.TryRemove(id, out _);
        }

        public void Update(T entity)
        {
            _innerRepository.Update(entity);
            // Обновляем кэш
            var id = GetEntityId(entity);
            _cache.AddOrUpdate(id, entity, (_, __) => entity);
        }

        public void Delete(T entity)
        {
            _innerRepository.Delete(entity);
            // Удаляем из кэша
            var id = GetEntityId(entity);
            _cache.TryRemove(id, out _);
        }

        public int SaveChanges()
        {
            return _innerRepository.SaveChanges();
        }

        private int GetEntityId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            return (int)property.GetValue(entity);
        }
    }

    // Тестовый класс для демонстрации
    public class DesignPatternsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 2.1: PATTERN OBSERVER ===");
            TestObserver();

            Console.WriteLine("\n=== 2.2: PATTERN DECORATOR ===");
            TestDecorator();

            Console.WriteLine("\n=== 2.3: PATTERN STRATEGY ===");
            TestStrategy();

            Console.WriteLine("\n=== 2.4: PATTERN FACTORY ===");
            TestFactory();

            Console.WriteLine("\n=== 2.5: PATTERN BUILDER ===");
            TestBuilder();

            Console.WriteLine("\n=== 2.6: PATTERN PROXY ===");
            TestProxy();

            Console.WriteLine("\n=== 2.7: PATTERN CHAIN OF RESPONSIBILITY ===");
            TestChainOfResponsibility();

            Console.WriteLine("\n=== 2.8: PATTERN COMMAND ===");
            TestCommand();

            Console.WriteLine("\n=== 2.9: PATTERN ADAPTER ===");
            TestAdapter();

            Console.WriteLine("\n=== 2.10: PATTERN FACADE ===");
            TestFacade();

            Console.WriteLine("\n=== 2.11: PATTERN SINGLETON ===");
            TestSingleton();

            Console.WriteLine("\n=== 2.12: PATTERN TEMPLATE METHOD ===");
            TestTemplateMethod();

            Console.WriteLine("\n=== 2.13: PATTERN STATE ===");
            TestState();

            Console.WriteLine("\n=== 2.14: PATTERN MEMENTO ===");
            TestMemento();

            Console.WriteLine("\n=== 2.15: PATTERN FLYWEIGHT ===");
            TestFlyweight();

            Console.WriteLine("\n=== 2.16: PATTERN INTERPRETER ===");
            TestInterpreter();

            Console.WriteLine("\n=== 2.17: PATTERN COMPOSITE ===");
            TestComposite();

            Console.WriteLine("\n=== 2.18: PATTERN VISITOR ===");
            TestVisitor();

            Console.WriteLine("\n=== 2.19: SOLID PRINCIPLES ===");
            TestSOLID();

            Console.WriteLine("\n=== 2.20: PATTERN REPOSITORY ===");
            TestRepository();
        }

        static void TestInterpreter()
        {
            Console.WriteLine("Testing Interpreter Pattern:");

            // Создаем контекст с переменными
            var context = new Dictionary<string, int>
            {
                ["x"] = 5,
                ["y"] = 10,
                ["z"] = 2
            };

            // Тестовые выражения
            var expressions = new[]
            {
                "x + y * z",
                "(x + y) * z",
                "x * y - z"
            };

            foreach (var expr in expressions)
            {
                try
                {
                    var parser = new ExpressionParser(expr);
                    var expression = parser.Parse();

                    var evaluator = new ExpressionEvaluator(context);
                    var result = evaluator.Evaluate(expression);

                    Console.WriteLine($"Expression: {expr} = {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing '{expr}': {ex.Message}");
                }
            }

            // Тест Visitor для печати
            var testExpr = new AddExpression(
                new VariableExpression("x"),
                new MultiplyExpression(
                    new VariableExpression("y"),
                    new VariableExpression("z")
                )
            );

            var printer = new ExpressionPrinter();
            testExpr.Accept(printer);
            Console.WriteLine($"AST: {printer.GetResult()}");
        }

        static void TestComposite()
        {
            Console.WriteLine("Testing Composite Pattern:");

            // Создаем структуру файловой системы
            var root = new Directory("root");
            var documents = new Directory("documents");
            var images = new Directory("images");
            var music = new Directory("music");

            // Добавляем файлы
            documents.AddComponent(new File("report.pdf", 1024));
            documents.AddComponent(new File("budget.xlsx", 2048));
            images.AddComponent(new File("photo.jpg", 3072));
            images.AddComponent(new File("diagram.png", 4096));
            music.AddComponent(new File("song.mp3", 5120));

            // Строим иерархию
            root.AddComponent(documents);
            root.AddComponent(images);
            root.AddComponent(music);
            root.AddComponent(new File("readme.txt", 100));

            // Отображаем структуру
            root.Display();

            // Поиск в структуре
            var found = root.Find("photo.jpg");
            if (found != null)
            {
                Console.WriteLine($"Found: {found.Name}");
            }
        }

        static void TestVisitor()
        {
            Console.WriteLine("Testing Visitor Pattern:");

            var root = new Directory("root");
            var docs = new Directory("documents");
            var file1 = new File("file1.txt", 100);
            var file2 = new File("file2.txt", 200);

            docs.AddComponent(file1);
            docs.AddComponent(file2);
            root.AddComponent(docs);
            root.AddComponent(new File("root_file.txt", 50));

            // Visitor для подсчета размера
            var sizeVisitor = new SizeCalculatorVisitor();
            root.Accept(sizeVisitor);
            Console.WriteLine($"Total size: {sizeVisitor.TotalSize} bytes");

            // Visitor для подсчета файлов и папок
            var counterVisitor = new FileCounterVisitor();
            root.Accept(counterVisitor);
            Console.WriteLine($"Files: {counterVisitor.FileCount}, Directories: {counterVisitor.DirectoryCount}");

            // Visitor для поиска
            var searchVisitor = new SearchVisitor("file");
            root.Accept(searchVisitor);
            Console.WriteLine($"Search results: {searchVisitor.Results.Count} items found");
        }

        static void TestSOLID()
        {
            Console.WriteLine("Testing SOLID Principles:");

            // Создаем заказ
            var customer = new CustomerInfo("cust_001", "John Doe", "john@example.com");
            var order = new OnlineOrder("order_001", customer);

            order.Items.Add(new OrderItem("prod_1", "Laptop", 999.99m, 1));
            order.Items.Add(new OrderItem("prod_2", "Mouse", 29.99m, 2));

            // Создаем зависимости
            var repository = new SqlOrderRepository();
            var notificationService = new EmailNotificationService();
            var priceCalculator = new DefaultPriceCalculator();

            // Обрабатываем заказ
            var processor = new OrderProcessor(repository, notificationService, priceCalculator);
            processor.ProcessOrder(order);

            // Тест Liskov Substitution
            var expressOrder = new ExpressOrder("order_002", customer);
            expressOrder.Items.Add(new OrderItem("prod_3", "Keyboard", 79.99m, 1));

            Console.WriteLine($"Regular order total: {order.CalculateTotal():C}");
            Console.WriteLine($"Express order total: {expressOrder.CalculateTotal():C}");

            // Тест Open/Closed
            var discountedOrder = new DiscountedOrder("order_003", customer, 0.1m); // 10% discount
            discountedOrder.Items.Add(new OrderItem("prod_4", "Monitor", 199.99m, 1));
            Console.WriteLine($"Discounted order total: {discountedOrder.CalculateTotal():C}");
        }

        static void TestRepository()
        {
            Console.WriteLine("Testing Repository Pattern:");

            using var uow = new UnitOfWork();

            // Добавляем тестовые данные
            var customer = new CustomerEntity { Id = 1, Name = "Alice Smith", Email = "alice@example.com" };
            var product = new ProductEntity { Id = 1, Name = "Laptop", Price = 999.99m, StockQuantity = 10 };

            uow.Customers.Add(customer);
            uow.Products.Add(product);

            // Создаем заказ
            var order = new OrderEntity
            {
                Id = 1,
                CustomerId = 1,
                TotalAmount = 999.99m,
                Status = OrderStatus.Confirmed
            };

            uow.Orders.Add(order);

            // Сохраняем изменения
            var changes = uow.Commit();
            Console.WriteLine($"Saved {changes} entities");

            // Поиск и LINQ запросы
            var foundCustomer = uow.Customers.GetById(1);
            Console.WriteLine($"Found customer: {foundCustomer.Name}");

            var expensiveProducts = uow.Products.Find(p => p.Price > 500);
            Console.WriteLine($"Expensive products: {expensiveProducts.Count()}");

            // Использование расширений
            var firstOrder = uow.Orders.FirstOrDefault(o => o.Status == OrderStatus.Confirmed);
            Console.WriteLine($"First confirmed order: {firstOrder?.Id}");

            // Тест кэширующего репозитория
            var cachingRepo = new CachingRepository<CustomerEntity>(uow.Customers);
            var cachedCustomer = cachingRepo.GetById(1);
            Console.WriteLine($"Cached customer: {cachedCustomer.Name}");

            // Пагинация
            var page = uow.Customers.Paginate(1, 10);
            Console.WriteLine($"First page: {page.Count()} customers");
        }

        static void TestSingleton()
        {
            Console.WriteLine("Testing Thread-Safe Singleton:");
            var tasks = new List<Task>();
            for (int i = 0; i < 3; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    var singleton = ThreadSafeSingleton.Instance;
                    singleton.DoWork();
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Singleton instance number: {ThreadSafeSingleton.Instance.InstanceNumber}");
        }

        static async void TestTemplateMethod()
        {
            var processors = new DataProcessor[]
            {
                new JsonDataProcessor(),
                new XmlDataProcessor(),
                new CsvDataProcessor()
            };

            var testData = new[]
            {
                "{\"name\": \"John\", \"age\": 30}",
                "<user><name>Jane</name><age>25</age></user>",
                "Name, Age, Email\nJohn Doe, 30, john@example.com"
            };

            for (int i = 0; i < processors.Length; i++)
            {
                Console.WriteLine($"\nTesting {processors[i].GetType().Name}:");
                var result = await processors[i].ProcessAsync(testData[i]);
                if (result.Success)
                {
                    Console.WriteLine($"  Success: {result.ProcessedData}");
                }
                else
                {
                    Console.WriteLine($"  Failed: {result.ErrorMessage}");
                }
            }
        }

        static async void TestState()
        {
            var order = new OrderContext("ORD-001");
            Console.WriteLine("Order State Transitions:");

            var result1 = await order.ShipAsync();
            Console.WriteLine($"Ship: {result1.Success} - {result1.Message}");

            var result2 = await order.DeliverAsync();
            Console.WriteLine($"Deliver: {result2.Success} - {result2.Message}");

            Console.WriteLine("\n" + order.GetVisualization());
        }

        static void TestMemento()
        {
            var editor = new TextEditorMemento();
            var caretaker = new Caretaker(editor, maxHistorySize: 3);

            Console.WriteLine("Testing Memento Pattern:");
            editor.Type("Hello");
            caretaker.Backup();
            editor.Type(" World");
            Console.WriteLine($"Current: {editor}");

            Console.WriteLine("\nUndoing last change:");
            caretaker.Undo();
            Console.WriteLine($"After undo: {editor}");
        }

        static void TestFlyweight()
        {
            var factory = new CharacterFactory();
            var document = new FlyweightTextDocument(factory);

            Console.WriteLine("Testing Flyweight Pattern:");
            var text = "HELLO";
            for (int i = 0; i < text.Length; i++)
            {
                document.AddCharacter(text[i], i);
            }

            Console.WriteLine("\nDocument Statistics:");
            factory.PrintStats();
            document.PrintContent();
        }

        static void TestObserver()
        {
            var eventBus = new EventBus();
            var emailSubscriber = new EmailNotificationSubscriber();
            eventBus.Subscribe<UserCreatedEvent>(emailSubscriber);

            var tasks = new List<Task>();
            for (int i = 0; i < 2; i++)
            {
                var userEvent = new UserCreatedEvent { UserId = $"user_{i}", Email = $"user{i}@example.com" };
                tasks.Add(eventBus.PublishAsync(userEvent));
            }
            Task.WaitAll(tasks.ToArray());
        }

        static void TestDecorator()
        {
            ICalculator calculator = new SimpleCalculator();
            calculator = new LoggingDecorator(calculator);
            calculator = new CachingDecorator(calculator);
            Console.WriteLine($"Add(5, 3) = {calculator.Add(5, 3)}");
        }

        static void TestStrategy()
        {
            var context = new SortContext<int>();
            var random = new Random();
            int[] array = Enumerable.Range(0, 10).Select(_ => random.Next(100)).ToArray();
            context.Sort(array);
        }

        static void TestFactory()
        {
            var factory = new AnimalFactory();
            var dog = factory.CreateAnimal("Dog");
            dog.Speak();
        }

        static void TestBuilder()
        {
            var pc = new Computer.Builder()
                .WithCPU("Intel i7")
                .WithGPU("RTX 3080")
                .WithRAM(16)
                .WithStorage(512)
                .Build();
            Console.WriteLine(pc);
        }

        static void TestProxy()
        {
            using var proxy = new LazyLoadingProxy();
            proxy.Process();
        }

        static async void TestChainOfResponsibility()
        {
            var authHandler = new AuthenticationHandler();
            var validationHandler = new ValidationHandler();
            authHandler.SetNext(validationHandler);

            var request = new Request { Type = "API", Content = "test", Metadata = { ["Authorization"] = "token" } };
            await authHandler.HandleAsync(request);
        }

        static async void TestCommand()
        {
            var history = new CommandHistory();
            var document = new TextDocument();
            var command = new TextEditorCommand(document, 0, "Hello", true, history);
            await history.ExecuteAsync(command);
        }

        static async void TestAdapter()
        {
            var adapter = new CsvDataAdapter("test.csv");
            var data = await adapter.GetDataAsync();
            Console.WriteLine($"Loaded {data.Count()} records");
        }

        static async void TestFacade()
        {
            var facade = new OrderProcessingFacade();
            var order = new Order { CustomerId = "test", TotalAmount = 100 };
            await facade.ProcessOrderAsync(order);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DesignPatternsDemo.Run();
            Console.WriteLine("\nAll patterns demonstrated successfully!");
            Console.ReadKey();
        }
    }
}
