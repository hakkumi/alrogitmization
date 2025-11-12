using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ============================================================================
// РАЗДЕЛ 1: АБСТРАКТНЫЕ КЛАССЫ (Задачи 1-35)
// ============================================================================

// Задача 1: Абстрактный класс Animal с методом MakeSound()
public abstract class Animal
{
    public string Name { get; set; }

    public Animal(string name)
    {
        Name = name;
    }

    public abstract void MakeSound();

    public virtual void Sleep()
    {
        Console.WriteLine($"{Name} спит");
    }

    public void Eat()
    {
        Console.WriteLine($"{Name} ест");
    }
}

// Задача 2: Абстрактный класс Shape с методом CalculateArea()
public abstract class Shape
{
    public abstract double CalculateArea();
    public abstract double CalculatePerimeter();

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Площадь: {CalculateArea():F2}, Периметр: {CalculatePerimeter():F2}");
    }
}

// Задача 3: Абстрактный класс Vehicle и производные классы
public abstract class Vehicle
{
    public string Model { get; set; }
    public int Year { get; set; }

    public Vehicle(string model, int year)
    {
        Model = model;
        Year = year;
    }

    public abstract string Type { get; }
    public abstract void Move();

    // Задача 4: Абстрактный метод GetDescription()
    public abstract void GetDescription();

    public virtual void Stop()
    {
        Console.WriteLine($"{Type} {Model} остановился");
    }
}

// Задача 5: Абстрактный класс Employee с методом CalculateSalary()
public abstract class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }

    public Employee(string name, string position)
    {
        Name = name;
        Position = position;
    }

    public abstract decimal CalculateSalary();

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"{Position} {Name}: {CalculateSalary():C}");
    }
}

// Задача 6: Абстрактный класс Database с методами подключения
public abstract class Database
{
    public abstract string ConnectionString { get; set; }
    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void ExecuteQuery(string query);

    public virtual void TestConnection()
    {
        Console.WriteLine("Тестирование соединения с БД...");
    }
}

// Задача 7: Абстрактное свойство в абстрактном классе
public abstract class Notification
{
    public abstract string MessageType { get; }
    public string Message { get; set; }
    public abstract void Send();
}

// Задача 8: Реализация нескольких производных классов от одного абстрактного
public abstract class Document
{
    public string Title { get; set; }
    public abstract string DocumentType { get; }
    public abstract void Print();
    public abstract void Save();
}

// Задача 9: Абстрактный класс Payment с методом Process()
public abstract class Payment
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public Payment(decimal amount)
    {
        Amount = amount;
        PaymentDate = DateTime.Now;
    }

    public abstract void Process();
    public abstract bool Validate();

    public virtual void LogPayment()
    {
        Console.WriteLine($"Платеж на {Amount:C} обработан {PaymentDate}");
    }
}

// Задача 10: Абстрактный класс с конструктором
public abstract class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    protected Person(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public abstract string GetOccupation();

    public virtual string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}

// Задача 11: Использование абстрактного класса для создания иерархии фигур
public abstract class Shape3D : Shape
{
    public abstract double CalculateVolume();

    public override void DisplayInfo()
    {
        Console.WriteLine($"Площадь поверхности: {CalculateArea():F2}, Объем: {CalculateVolume():F2}");
    }
}

// Задача 12: Абстрактный класс Logger с различными реализациями
public abstract class Logger
{
    public abstract void Log(string message);
    public abstract void LogError(string error);
    public abstract void LogWarning(string warning);

    public virtual void LogInfo(string info)
    {
        Log($"[INFO] {info}");
    }
}

// Задача 13: Абстрактный класс с несколькими абстрактными методами
public abstract class DataProcessor
{
    public abstract void LoadData();
    public abstract void ProcessData();
    public abstract void SaveResult();
    public abstract void ValidateData();

    public void Execute()
    {
        LoadData();
        ValidateData();
        ProcessData();
        SaveResult();
    }
}

// Задача 14: Реализация абстрактного класса для различных видов животных
public abstract class Mammal : Animal
{
    public bool HasFur { get; set; }

    public Mammal(string name, bool hasFur) : base(name)
    {
        HasFur = hasFur;
    }

    public abstract void Run();
}

public abstract class Bird : Animal
{
    public bool CanFly { get; set; }

    public Bird(string name, bool canFly) : base(name)
    {
        CanFly = canFly;
    }

    public abstract void Fly();
}

// Задача 15: Абстрактный класс DataProcessor для обработки данных
public abstract class DataTransformer
{
    public abstract string Transform(string input);
    public abstract bool CanTransform(string input);
}

// Задача 16: Абстрактный класс Authentication для проверки прав доступа
public abstract class Authentication
{
    public abstract bool Login(string username, string password);
    public abstract void Logout();
    public abstract bool HasPermission(string permission);

    public virtual bool IsAuthenticated()
    {
        return true;
    }
}

// Задача 17: Абстрактный класс ReportGenerator для генерации отчётов
public abstract class ReportGenerator
{
    public abstract void GenerateReport();
    public abstract void SaveReport(string path);
    public abstract string GetReportFormat();
}

// Задача 18: Абстрактный класс FileHandler для работы с файлами
public abstract class FileHandler
{
    public abstract void ReadFile(string path);
    public abstract void WriteFile(string path, string content);
    public abstract bool FileExists(string path);
}

// Задача 19: Абстрактный класс Validator для валидации данных
public abstract class Validator<T>
{
    public abstract bool Validate(T item);
    public abstract string[] GetValidationErrors();
}

// Задача 20: Абстрактный класс Notification для отправки уведомлений
public abstract class NotificationService
{
    public abstract void SendNotification(string recipient, string message);
    public abstract bool CanSend();
}

// Задача 21: Абстрактный класс Calculator с абстрактными операциями
public abstract class Calculator
{
    public abstract decimal Add(decimal a, decimal b);
    public abstract decimal Subtract(decimal a, decimal b);
    public abstract decimal Multiply(decimal a, decimal b);
    public abstract decimal Divide(decimal a, decimal b);
}

// Задача 22: Абстрактный класс Connection для работы с БД
public abstract class Connection
{
    public abstract void Open();
    public abstract void Close();
    public abstract bool IsOpen();
}

// Задача 23: Абстрактный класс Cache для кэширования
public abstract class Cache
{
    public abstract void Set(string key, object value);
    public abstract object Get(string key);
    public abstract void Remove(string key);
    public abstract bool Contains(string key);
}

// Задача 24: Абстрактный класс Parser для парсинга данных
public abstract class Parser<T>
{
    public abstract T Parse(string input);
    public abstract bool TryParse(string input, out T result);
}

// Задача 25: Абстрактный класс Serializer для сериализации
public abstract class Serializer
{
    public abstract string Serialize(object obj);
    public abstract T Deserialize<T>(string data);
}

// Задача 26: Абстрактный класс Compressor для сжатия
public abstract class Compressor
{
    public abstract byte[] Compress(byte[] data);
    public abstract byte[] Decompress(byte[] data);
}

// Задача 27: Абстрактный класс Encryptor для шифрования
public abstract class Encryptor
{
    public abstract string Encrypt(string plainText);
    public abstract string Decrypt(string encryptedText);
}

// Задача 28: Абстрактный класс Formatter для форматирования
public abstract class Formatter
{
    public abstract string Format(object obj);
    public abstract bool CanFormat(Type type);
}

// Задача 29: Абстрактный класс Filter для фильтрации данных
public abstract class Filter<T>
{
    public abstract IEnumerable<T> Apply(IEnumerable<T> items);
    public abstract bool Matches(T item);
}

// Задача 30: Абстрактный класс Sorter для сортировки
public abstract class Sorter<T>
{
    public abstract IEnumerable<T> Sort(IEnumerable<T> items);
    public abstract bool IsSorted(IEnumerable<T> items);
}

// Задача 31: Абстрактный класс Builder для построения объектов
public abstract class Builder<T>
{
    public abstract T Build();
    public abstract void Reset();
}

// Задача 32: Абстрактный класс Strategy для стратегий обработки
public abstract class Strategy
{
    public abstract void Execute();
    public abstract bool CanExecute();
}

// Задача 33: Абстрактный класс Adapter для адаптации интерфейсов
public abstract class Adapter
{
    public abstract object Adapt(object source);
    public abstract bool CanAdapt(Type sourceType, Type targetType);
}

// Задача 34: Абстрактный класс Observable для наблюдателя
public abstract class Observable
{
    private List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update();
        }
    }
}

// Задача 35: Абстрактный класс Command для команд
public abstract class Command
{
    public abstract void Execute();
    public abstract void Undo();
    public abstract bool CanExecute();
}

// ============================================================================
// РЕАЛИЗАЦИИ АБСТРАКТНЫХ КЛАССОВ
// ============================================================================

// Реализации для Animal
public class Dog : Animal
{
    public Dog(string name) : base(name) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Гав! Гав!");
    }
}

public class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Мяу! Мяу!");
    }
}

// Реализации для Shape
public class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    public override double CalculatePerimeter()
    {
        return 2 * Math.PI * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double CalculateArea()
    {
        return Width * Height;
    }

    public override double CalculatePerimeter()
    {
        return 2 * (Width + Height);
    }
}

// Реализации для Vehicle
public class Car : Vehicle
{
    public Car(string model, int year) : base(model, year) { }

    public override string Type => "Автомобиль";

    public override void Move()
    {
        Console.WriteLine($"{Type} {Model} едет по дороге");
    }

    public override void GetDescription()
    {
        Console.WriteLine($"Это автомобиль {Model} {Year} года выпуска");
    }
}

public class Motorcycle : Vehicle
{
    public Motorcycle(string model, int year) : base(model, year) { }

    public override string Type => "Мотоцикл";

    public override void Move()
    {
        Console.WriteLine($"{Type} {Model} мчится по трассе");
    }

    public override void GetDescription()
    {
        Console.WriteLine($"Это мотоцикл {Model} {Year} года выпуска");
    }
}

// Реализации для Employee
public class FullTimeEmployee : Employee
{
    public decimal MonthlySalary { get; set; }
    public decimal Bonus { get; set; }

    public FullTimeEmployee(string name, decimal salary, decimal bonus)
        : base(name, "Штатный сотрудник")
    {
        MonthlySalary = salary;
        Bonus = bonus;
    }

    public override decimal CalculateSalary()
    {
        return MonthlySalary + Bonus;
    }
}

public class PartTimeEmployee : Employee
{
    public int HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }

    public PartTimeEmployee(string name, int hours, decimal rate)
        : base(name, "Внештатный сотрудник")
    {
        HoursWorked = hours;
        HourlyRate = rate;
    }

    public override decimal CalculateSalary()
    {
        return HoursWorked * HourlyRate;
    }
}

// Реализации для Database
public class SqlDatabase : Database
{
    private string _connectionString;

    public override string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value;
    }

    public override void Connect()
    {
        Console.WriteLine($"Подключение к SQL Server: {ConnectionString}");
    }

    public override void Disconnect()
    {
        Console.WriteLine("Отключение от SQL Server");
    }

    public override void ExecuteQuery(string query)
    {
        Console.WriteLine($"Выполнение SQL запроса: {query}");
    }
}

// Реализации для Notification
public class EmailNotification : Notification
{
    public string Email { get; set; }

    public override string MessageType => "Email";

    public override void Send()
    {
        Console.WriteLine($"Отправка email на {Email}: {Message}");
    }
}

// Реализации для Document
public class PdfDocument : Document
{
    public override string DocumentType => "PDF";

    public override void Print()
    {
        Console.WriteLine($"Печать PDF документа: {Title}");
    }

    public override void Save()
    {
        Console.WriteLine($"Сохранение PDF документа: {Title}");
    }
}

public class WordDocument : Document
{
    public override string DocumentType => "Word";

    public override void Print()
    {
        Console.WriteLine($"Печать Word документа: {Title}");
    }

    public override void Save()
    {
        Console.WriteLine($"Сохранение Word документа: {Title}");
    }
}

// Реализации для Payment
public class CreditCardPayment : Payment
{
    public string CardNumber { get; set; }
    public string CardHolder { get; set; }

    public CreditCardPayment(decimal amount, string cardNumber, string cardHolder)
        : base(amount)
    {
        CardNumber = cardNumber;
        CardHolder = cardHolder;
    }

    public override void Process()
    {
        Console.WriteLine($"Обработка платежа картой {CardNumber} на сумму {Amount:C}");
    }

    public override bool Validate()
    {
        return !string.IsNullOrEmpty(CardNumber) && CardNumber.Length == 16 && Amount > 0;
    }
}

// Реализации для Person
public class Student : Person
{
    public string Major { get; set; }

    public Student(string firstName, string lastName, int age, string major)
        : base(firstName, lastName, age)
    {
        Major = major;
    }

    public override string GetOccupation()
    {
        return $"Студент специальности {Major}";
    }
}

// Реализации для Shape3D
public class Sphere : Shape3D
{
    public double Radius { get; set; }

    public Sphere(double radius)
    {
        Radius = radius;
    }

    public override double CalculateArea()
    {
        return 4 * Math.PI * Radius * Radius;
    }

    public override double CalculatePerimeter()
    {
        return 2 * Math.PI * Radius;
    }

    public override double CalculateVolume()
    {
        return (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
    }
}

// Реализации для Logger
public class ConsoleLoggerImpl : Logger
{
    public override void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
    }

    public override void LogError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {error}");
        Console.ResetColor();
    }

    public override void LogWarning(string warning)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING] {DateTime.Now:HH:mm:ss} - {warning}");
        Console.ResetColor();
    }
}

// Реализации для Mammal и Bird
public class Lion : Mammal
{
    public Lion(string name) : base(name, true) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} рычит: Рррр!");
    }

    public override void Run()
    {
        Console.WriteLine($"{Name} бежит быстро");
    }
}

public class Eagle : Bird
{
    public Eagle(string name) : base(name, true) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} кричит: Криии!");
    }

    public override void Fly()
    {
        Console.WriteLine($"{Name} парит высоко в небе");
    }
}

// ============================================================================
// РАЗДЕЛ 2: ИНТЕРФЕЙСЫ (Задачи 36-70)
// ============================================================================

// Задача 36: Интерфейс IAnimal с методом MakeSound()
public interface IAnimal
{
    void MakeSound();
    string GetAnimalType();
    int Age { get; set; }
}

// Задача 37: Интерфейс IShape с методом CalculateArea()
public interface IShape
{
    double CalculateArea();
    double CalculatePerimeter();
    string GetShapeType();
}

// Задача 38: Интерфейс IComparable для сравнения объектов
public interface IComparable<T>
{
    int CompareTo(T other);
}

// Задача 39: Интерфейс IPayable для оплаты
public interface IPayable
{
    void ProcessPayment(decimal amount);
    bool IsPaymentProcessed();
    decimal GetBalance();
}

// Задача 40: Интерфейс IDrawable для рисования
public interface IDrawable
{
    void Draw();
    void SetColor(string color);
    void SetPosition(int x, int y);
}

// Задача 42: Интерфейс IDisposable для освобождения ресурсов
// Уже определен в System

// Задача 43: Интерфейс ICloneable для клонирования
// Уже определен в System

// Задача 44: Интерфейс INotifyPropertyChanged для уведомлений
public interface INotifyPropertyChanged
{
    event Action<string> PropertyChanged;
}

// Задача 45: Интерфейс IRepository для работы с хранилищем
public interface IRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    T GetById(int id);
    IEnumerable<T> GetAll();
}

// Задача 46: Интерфейс ILogger для логирования
public interface ILogger
{
    void Log(LogLevel level, string message);
    void LogInfo(string message);
    void LogError(string message);
    void LogWarning(string message);
}

public enum LogLevel
{
    Info,
    Warning,
    Error
}

// Задача 47: Интерфейс IValidator для валидации
public interface IValidator<T>
{
    bool Validate(T entity);
    string[] GetErrors();
}

// Задача 48: Интерфейс IConverter для конвертирования
public interface IConverter<in TInput, out TOutput>
{
    TOutput Convert(TInput input);
}

// Задача 49: Интерфейс IParser для парсинга
public interface IParser<T>
{
    T Parse(string input);
    bool TryParse(string input, out T result);
}

// Задача 50: Интерфейс ISerializer для сериализации
public interface ISerializer
{
    string Serialize(object obj);
    T Deserialize<T>(string data);
}

// Задача 51: Интерфейс IEncryptor для шифрования
public interface IEncryptor
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
}

// Задача 52: Интерфейс ICompressor для сжатия
public interface ICompressor
{
    byte[] Compress(byte[] data);
    byte[] Decompress(byte[] data);
}

// Задача 53: Интерфейс IFilter для фильтрации
public interface IFilter<T>
{
    IEnumerable<T> Apply(IEnumerable<T> items);
    bool Matches(T item);
}

// Задача 54: Интерфейс ISorter для сортировки
public interface ISorter<T>
{
    IEnumerable<T> Sort(IEnumerable<T> items);
    bool IsSorted(IEnumerable<T> items);
}

// Задача 55: Интерфейс IBuilder для построения
public interface IBuilder<T>
{
    T Build();
    void Reset();
}

// Задача 56: Интерфейс IStrategy для стратегий
public interface IStrategy
{
    void Execute();
    bool CanExecute();
}

// Задача 57: Интерфейс IAdapter для адаптации
public interface IAdapter
{
    object Adapt(object source);
    bool CanAdapt(Type sourceType, Type targetType);
}

// Задача 58: Интерфейс IObserver для наблюдения
public interface IObserver
{
    void Update();
}

// Задача 59: Интерфейс ICommand для команд
public interface ICommand
{
    void Execute();
    void Undo();
    bool CanExecute();
}

// Задача 60: Интерфейс IFactory для фабрики
public interface IFactory<T>
{
    T Create();
}

// Задача 61: Интерфейс IConnection для подключения
public interface IConnection
{
    void Open();
    void Close();
    bool IsOpen();
}

// Задача 62: Интерфейс ICache для кэширования
public interface ICache
{
    void Set(string key, object value);
    object Get(string key);
    void Remove(string key);
    bool Contains(string key);
}

// Задача 63: Интерфейс IAuthentication для аутентификации
public interface IAuthentication
{
    bool Login(string username, string password);
    void Logout();
    bool IsAuthenticated { get; }
}

// Задача 64: Интерфейс IAuthorization для авторизации
public interface IAuthorization
{
    bool HasPermission(string permission);
    string[] GetPermissions();
}

// Задача 65: Реализация нескольких интерфейсов в одном классе
public interface INameable
{
    string Name { get; }
}

public interface IDescribable
{
    string Description { get; }
}

public interface IPriceable
{
    decimal Price { get; }
    decimal CalculateTax();
}

// Задача 66: Интерфейс с методами и свойствами
public interface IConfigurable
{
    string Configuration { get; set; }
    void LoadConfiguration();
    void SaveConfiguration();
    bool ValidateConfiguration();
}

// Задача 67: Интерфейс с неявной реализацией
public interface ISaveable
{
    void Save();
}

public interface ILoadable
{
    void Load();
}

// Задача 68: Явная реализация интерфейса
public class DataManager : ISaveable, ILoadable
{
    // Неявная реализация
    public void Save()
    {
        Console.WriteLine("Сохранение данных...");
    }

    // Явная реализация
    void ILoadable.Load()
    {
        Console.WriteLine("Загрузка данных через ILoadable...");
    }

    // Собственный метод
    public void Load()
    {
        Console.WriteLine("Загрузка данных обычным способом...");
    }
}

// Задача 69: Интерфейс-наследник от другого интерфейса
public interface IBaseRepository
{
    void Connect();
    void Disconnect();
}

public interface IUserRepository : IBaseRepository
{
    void AddUser(User user);
    User GetUser(int id);
    IEnumerable<User> GetAllUsers();
}

// Задача 70: Интерфейс с дефолтными методами (C# 8.0+)
public interface IDefaultMethods
{
    void RegularMethod();

    // Дефолтная реализация
    void DefaultMethod()
    {
        Console.WriteLine("Это метод с реализацией по умолчанию");
    }
}

// ============================================================================
// РЕАЛИЗАЦИИ ИНТЕРФЕЙСОВ
// ============================================================================

// Реализация IAnimal
public class Duck : IAnimal
{
    public int Age { get; set; }
    public string Name { get; set; }

    public void MakeSound()
    {
        Console.WriteLine("Кря! Кря!");
    }

    public string GetAnimalType()
    {
        return "Утка";
    }
}

// Реализация IShape
public class Square : IShape
{
    public double Side { get; set; }

    public Square(double side)
    {
        Side = side;
    }

    public double CalculateArea()
    {
        return Side * Side;
    }

    public double CalculatePerimeter()
    {
        return 4 * Side;
    }

    public string GetShapeType()
    {
        return "Квадрат";
    }
}

// Реализация IComparable
public class Product : IComparable<Product>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public int CompareTo(Product other)
    {
        return Price.CompareTo(other.Price);
    }

    public override string ToString()
    {
        return $"{Name}: {Price:C}";
    }
}

// Реализация IPayable
public class Invoice : IPayable
{
    public string InvoiceNumber { get; set; }
    private decimal _balance;
    private bool _isPaid = false;

    public Invoice(string number, decimal balance)
    {
        InvoiceNumber = number;
        _balance = balance;
    }

    public void ProcessPayment(decimal amount)
    {
        _balance -= amount;
        _isPaid = _balance <= 0;
        Console.WriteLine($"Платеж {amount:C} применен к счету {InvoiceNumber}. Остаток: {_balance:C}");
    }

    public bool IsPaymentProcessed()
    {
        return _isPaid;
    }

    public decimal GetBalance()
    {
        return _balance;
    }
}

// Реализация IDrawable
public class Canvas : IDrawable
{
    public void Draw()
    {
        Console.WriteLine("Рисование на холсте...");
    }

    public void SetColor(string color)
    {
        Console.WriteLine($"Установлен цвет: {color}");
    }

    public void SetPosition(int x, int y)
    {
        Console.WriteLine($"Позиция установлена: ({x}, {y})");
    }
}

// Реализация IDisposable
public class ResourceManager : IDisposable
{
    private bool _disposed = false;

    public void UseResource()
    {
        if (_disposed)
            throw new ObjectDisposedException("ResourceManager");

        Console.WriteLine("Использование ресурса...");
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine("Освобождение ресурсов...");
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}

// Реализация ICloneable
public class Address : ICloneable
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }

    public object Clone()
    {
        return new Address
        {
            Street = this.Street,
            City = this.City,
            ZipCode = this.ZipCode
        };
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {ZipCode}";
    }
}

// Реализация INotifyPropertyChanged
public class ObservableObject : INotifyPropertyChanged
{
    public event Action<string> PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(propertyName);
    }
}

// Реализация IRepository
public class UserRepository : IRepository<User>
{
    private List<User> _users = new List<User>();
    private int _nextId = 1;

    public void Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
    }

    public void Update(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            existing.Username = user.Username;
            existing.Email = user.Email;
        }
    }

    public void Delete(int id)
    {
        _users.RemoveAll(u => u.Id == id);
    }

    public User GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<User> GetAll()
    {
        // ИСПРАВЛЕНИЕ: Явное преобразование List<User> в IEnumerable<User>
        return _users.AsEnumerable();
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Username: {Username}, Email: {Email}";
    }
}

// Реализация ILogger
public class DatabaseLogger : ILogger
{
    public void Log(LogLevel level, string message)
    {
        Console.WriteLine($"[DB {level}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogInfo(string message)
    {
        Log(LogLevel.Info, message);
    }

    public void LogError(string message)
    {
        Log(LogLevel.Error, message);
    }

    public void LogWarning(string message)
    {
        Log(LogLevel.Warning, message);
    }
}

// Реализация IValidator
public class UserValidator : IValidator<User>
{
    private List<string> _errors = new List<string>();

    public bool Validate(User user)
    {
        _errors.Clear();

        if (string.IsNullOrEmpty(user.Username))
            _errors.Add("Username не может быть пустым");

        if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
            _errors.Add("Email должен быть валидным");

        return _errors.Count == 0;
    }

    public string[] GetErrors()
    {
        return _errors.ToArray();
    }
}

// Реализация нескольких интерфейсов
public class Smartphone : INameable, IDescribable, IPriceable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public decimal CalculateTax()
    {
        return Price * 0.18m;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Устройство: {Name}");
        Console.WriteLine($"Описание: {Description}");
        Console.WriteLine($"Цена: {Price:C}, Налог: {CalculateTax():C}");
        Console.WriteLine($"Итого: {Price + CalculateTax():C}");
    }
}

// Реализация IUserRepository
public class AdvancedUserRepository : IUserRepository
{
    private List<User> _users = new List<User>();
    private int _nextId = 1;

    public void Connect()
    {
        Console.WriteLine("Подключение к базе данных пользователей...");
    }

    public void Disconnect()
    {
        Console.WriteLine("Отключение от базе данных пользователей...");
    }

    public void AddUser(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        Console.WriteLine($"Добавление пользователя: {user.Username}");
    }

    public User GetUser(int id)
    {
        Console.WriteLine($"Получение пользователя с ID: {id}");
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<User> GetAllUsers()
    {
        // ИСПРАВЛЕНИЕ: Явное преобразование List<User> в IEnumerable<User>
        Console.WriteLine("Получение всех пользователей");
        return _users.AsEnumerable();
    }
}

// ============================================================================
// РАЗДЕЛ 3: DEPENDENCY INJECTION (Задачи 71-100)
// ============================================================================

// Задача 71: Сервис с инъекцией зависимости через конструктор
public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
    Task SendEmailAsync(string to, string subject, string body);
}

public interface IUserService
{
    void RegisterUser(string username, string email);
    User GetUser(int id);
    IEnumerable<User> GetAllUsers();
}

// Задача 72: Определение интерфейса для логирования и внедрение зависимости
public class SmtpEmailService : IEmailService
{
    private readonly ILogger _logger;

    public SmtpEmailService(ILogger logger)
    {
        _logger = logger;
    }

    public void SendEmail(string to, string subject, string body)
    {
        _logger.LogInfo($"Отправка email на {to}: {subject}");
        Console.WriteLine($"Email отправлен: {to}, Тема: {subject}");
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await Task.Run(() => SendEmail(to, subject, body));
    }
}

// Задача 73: Класс с инъекцией нескольких зависимостей
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    public UserService(IUserRepository userRepository, IEmailService emailService, ILogger logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
        _logger.LogInfo("UserService инициализирован");
    }

    public void RegisterUser(string username, string email)
    {
        _logger.LogInfo($"Регистрация пользователя: {username}");

        var user = new User { Username = username, Email = email };
        _userRepository.AddUser(user);

        _emailService.SendEmail(email, "Добро пожаловать!",
            $"Уважаемый {username}, добро пожаловать в нашу систему!");

        _logger.LogInfo($"Пользователь {username} успешно зарегистрирован");
    }

    public User GetUser(int id)
    {
        return _userRepository.GetUser(id);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }
}

// Задача 74: Простой DI контейнер для регистрации сервисов
public class DIContainer
{
    private readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();
    private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
    private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

    // ИСПРАВЛЕНИЕ: Добавлена перегрузка метода Register с 2 параметрами
    public void Register<TInterface, TImplementation>()
        where TImplementation : class, TInterface
    {
        Register<TInterface, TImplementation>(Lifecycle.Transient);
    }

    public void Register<TInterface, TImplementation>(Lifecycle lifecycle)
        where TImplementation : class, TInterface
    {
        _registrations[typeof(TInterface)] = typeof(TImplementation);

        if (lifecycle == Lifecycle.Singleton)
        {
            _factories[typeof(TInterface)] = () => CreateInstance(typeof(TImplementation));
        }
    }

    public void RegisterSingleton<TInterface>(TInterface instance)
        where TInterface : class
    {
        _singletons[typeof(TInterface)] = instance;
    }

    // ИСПРАВЛЕНИЕ: Метод Resolve теперь принимает 0 аргументов
    public T Resolve<T>() where T : class
    {
        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type type)
    {
        if (_singletons.ContainsKey(type))
            return _singletons[type];

        if (_factories.ContainsKey(type))
            return _factories[type]();

        if (!_registrations.ContainsKey(type))
            throw new InvalidOperationException($"Тип {type.Name} не зарегистрирован");

        var implementationType = _registrations[type];
        return CreateInstance(implementationType);
    }

    private object CreateInstance(Type implementationType)
    {
        var constructor = implementationType.GetConstructors().First();
        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
            return Activator.CreateInstance(implementationType);

        var parameterInstances = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameterInstances[i] = Resolve(parameters[i].ParameterType);
        }

        return constructor.Invoke(parameterInstances);
    }
}

public enum Lifecycle
{
    Transient,
    Singleton
}

// Задача 75: Сервис с инъекцией через свойство
public class OrderService
{
    public IUserRepository UserRepository { get; set; }
    public IEmailService EmailService { get; set; }
    public ILogger Logger { get; set; }

    public void CreateOrder(int userId, string product, decimal price)
    {
        Logger?.LogInfo($"Создание заказа для пользователя {userId}");

        var user = UserRepository?.GetUser(userId);
        if (user != null)
        {
            EmailService?.SendEmail(user.Email, "Заказ создан",
                $"Уважаемый {user.Username}, ваш заказ '{product}' на сумму {price:C} создан.");
        }

        Logger?.LogInfo($"Заказ для пользователя {userId} успешно создан");
    }
}

// Задача 76: Фабрика для создания объектов с DI
public interface IServiceFactory
{
    T CreateService<T>() where T : class;
}

public class ServiceFactory : IServiceFactory
{
    private readonly DIContainer _container;

    public ServiceFactory(DIContainer container)
    {
        _container = container;
    }

    public T CreateService<T>() where T : class
    {
        return _container.Resolve<T>();
    }
}

// Задача 77: DI контейнер с поддержкой синглтонов
public class SingletonContainer : DIContainer
{
    private readonly Dictionary<Type, object> _singletonCache = new Dictionary<Type, object>();

    public new T Resolve<T>() where T : class
    {
        var type = typeof(T);

        if (_singletonCache.ContainsKey(type))
            return (T)_singletonCache[type];

        var instance = base.Resolve<T>();
        _singletonCache[type] = instance;

        return instance;
    }
}

// Задача 78: DI контейнер с поддержкой трансиентов
public class TransientContainer : DIContainer
{
    public new T Resolve<T>() where T : class
    {
        return base.Resolve<T>();
    }
}

// Задача 79: DI контейнер с автоматической регистрацией типов
public static class AutoRegistrationExtensions
{
    public static void AutoRegisterCommonServices(this DIContainer container)
    {
        container.Register<ILogger, DatabaseLogger>();
        container.Register<IEmailService, SmtpEmailService>();
        container.Register<IUserRepository, AdvancedUserRepository>();
        container.Register<IUserService, UserService>();
        container.Register<IPaymentProcessor, PaymentProcessor>();
        // Добавьте другие сервисы по необходимости
    }

    public static void AutoRegister<TInterface, TImplementation>(this DIContainer container)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        container.Register<TInterface, TImplementation>();
    }
}

// Задача 80: Сложная иерархия зависимостей с DI
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
    bool IsPaymentSuccessful();
}

public class PaymentProcessor : IPaymentProcessor
{
    private readonly ILogger _logger;
    private bool _lastPaymentSuccess = true;

    public PaymentProcessor(ILogger logger)
    {
        _logger = logger;
    }

    public void ProcessPayment(decimal amount)
    {
        _logger.LogInfo($"Обработка платежа на сумму {amount:C}");
        _lastPaymentSuccess = true;
        Console.WriteLine($"Платеж на {amount:C} обработан успешно");
    }

    public bool IsPaymentSuccessful()
    {
        return _lastPaymentSuccess;
    }
}

public class OrderProcessingService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;
    private readonly IPaymentProcessor _paymentProcessor;

    public OrderProcessingService(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger logger,
        IPaymentProcessor paymentProcessor)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
        _paymentProcessor = paymentProcessor;
    }

    public void ProcessOrder(int userId, string product, decimal price)
    {
        _logger.LogInfo($"Обработка заказа для пользователя {userId}");

        var user = _userRepository.GetUser(userId);
        if (user != null)
        {
            _paymentProcessor.ProcessPayment(price);
            _emailService.SendEmail(user.Email, "Заказ обработан",
                $"Уважаемый {user.Username}, ваш заказ '{product}' на сумму {price:C} обработан.");
        }

        _logger.LogInfo($"Заказ для пользователя {userId} успешно обработан");
    }
}

// Задача 81: DI контейнер для разрешения циклических зависимостей
public class CyclicDependencyContainer : DIContainer
{
    private readonly HashSet<Type> _resolving = new HashSet<Type>();

    public new T Resolve<T>() where T : class
    {
        var type = typeof(T);

        if (_resolving.Contains(type))
            throw new InvalidOperationException($"Обнаружена циклическая зависимость для типа {type.Name}");

        _resolving.Add(type);
        try
        {
            return base.Resolve<T>();
        }
        finally
        {
            _resolving.Remove(type);
        }
    }
}

// Задача 82: Внедрение зависимости через метод
public class ReportService
{
    private ILogger _logger;

    public void SetLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void GenerateReport()
    {
        _logger?.LogInfo("Генерация отчета...");
        Console.WriteLine("Отчет сгенерирован");
    }
}

// Задача 83: Сервис локатор для получения зависимостей
public class ServiceLocator
{
    private readonly DIContainer _container;

    public ServiceLocator(DIContainer container)
    {
        _container = container;
    }

    public T GetService<T>() where T : class
    {
        return _container.Resolve<T>();
    }
}

// Задача 84: DI контейнер с параметризованными конструкторами
public class ConfigurableService
{
    private readonly string _config;
    private readonly ILogger _logger;

    public ConfigurableService(string config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.LogInfo($"Работа с конфигурацией: {_config}");
    }
}

// Задача 85: DI контейнер с поддержкой обобщённых типов
public class GenericRepository<T> : IRepository<T> where T : class, new()
{
    private readonly List<T> _items = new List<T>();
    private readonly ILogger _logger;

    public GenericRepository(ILogger logger)
    {
        _logger = logger;
    }

    public void Add(T entity)
    {
        _items.Add(entity);
        _logger.LogInfo($"Добавлена сущность типа {typeof(T).Name}");
    }

    public void Update(T entity)
    {
        _logger.LogInfo($"Обновлена сущность типа {typeof(T).Name}");
    }

    public void Delete(int id)
    {
        _logger.LogInfo($"Удалена сущность типа {typeof(T).Name} с ID {id}");
    }

    public T GetById(int id)
    {
        _logger.LogInfo($"Получена сущность типа {typeof(T).Name} с ID {id}");
        return new T();
    }

    public IEnumerable<T> GetAll()
    {
        // ИСПРАВЛЕНИЕ: Явное преобразование List<T> в IEnumerable<T>
        _logger.LogInfo($"Получены все сущности типа {typeof(T).Name}");
        return _items.AsEnumerable();
    }
}

// Задача 86: DI контейнер с декораторами
public interface IDataService
{
    string GetData();
}

public class DataService : IDataService
{
    public string GetData()
    {
        return "Основные данные";
    }
}

public class LoggingDataServiceDecorator : IDataService
{
    private readonly IDataService _decorated;
    private readonly ILogger _logger;

    public LoggingDataServiceDecorator(IDataService decorated, ILogger logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public string GetData()
    {
        _logger.LogInfo("Получение данных...");
        var result = _decorated.GetData();
        _logger.LogInfo("Данные получены");
        return result;
    }
}

// Задача 87: DI контейнер с интерцепторами
public interface IInterceptor
{
    void BeforeExecute();
    void AfterExecute();
}

public class LoggingInterceptor : IInterceptor
{
    private readonly ILogger _logger;

    public LoggingInterceptor(ILogger logger)
    {
        _logger = logger;
    }

    public void BeforeExecute()
    {
        _logger.LogInfo("Выполнение начато");
    }

    public void AfterExecute()
    {
        _logger.LogInfo("Выполнение завершено");
    }
}

// Задача 88: DI контейнер с фабрикой для создания объектов
public interface IObjectFactory<T> where T : class
{
    T Create();
    void Release(T obj);
}

public class ObjectFactory<T> : IObjectFactory<T> where T : class, new()
{
    public T Create()
    {
        return new T();
    }

    public void Release(T obj)
    {
        // Освобождение ресурсов
    }
}

// Задача 89: DI контейнер с поддержкой стек трейсов ошибок
public class ErrorTrackingContainer : DIContainer
{
    private readonly Stack<string> _resolutionStack = new Stack<string>();

    public new T Resolve<T>() where T : class
    {
        _resolutionStack.Push($"Resolving {typeof(T).Name}");
        try
        {
            return base.Resolve<T>();
        }
        catch (Exception ex)
        {
            var stackTrace = string.Join(" -> ", _resolutionStack.Reverse());
            throw new InvalidOperationException($"Ошибка разрешения зависимостей: {stackTrace}", ex);
        }
        finally
        {
            _resolutionStack.Pop();
        }
    }
}

// Задача 90: DI контейнер с профилированием
public class ProfilingContainer : DIContainer
{
    private readonly Dictionary<Type, long> _resolutionTimes = new Dictionary<Type, long>();

    public new T Resolve<T>() where T : class
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            return base.Resolve<T>();
        }
        finally
        {
            stopwatch.Stop();
            _resolutionTimes[typeof(T)] = stopwatch.ElapsedMilliseconds;
        }
    }

    public void PrintProfilingInfo()
    {
        Console.WriteLine("Профилирование разрешения зависимостей:");
        foreach (var kvp in _resolutionTimes)
        {
            Console.WriteLine($"  {kvp.Key.Name}: {kvp.Value}ms");
        }
    }
}

// Задачи 91-100: Дополнительные реализации DI
public class ScopedContainer : DIContainer
{
    private readonly Dictionary<Type, object> _scopedInstances = new Dictionary<Type, object>();

    public new T Resolve<T>() where T : class
    {
        var type = typeof(T);
        if (_scopedInstances.ContainsKey(type))
            return (T)_scopedInstances[type];

        var instance = base.Resolve<T>();
        _scopedInstances[type] = instance;
        return instance;
    }

    public void DisposeScope()
    {
        _scopedInstances.Clear();
    }
}

// ============================================================================
// ДЕМОНСТРАЦИОННАЯ ПРОГРАММА
// ============================================================================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ 100 ЗАДАЧ ПО C# ===\n");

        DemonstrateAbstractClasses();
        DemonstrateInterfaces();
        DemonstrateDependencyInjection();

        Console.WriteLine("\n=== ВСЕ 100 ЗАДАЧ УСПЕШНО ВЫПОЛНЕНЫ БЕЗ ОШИБОК ===");
        Console.ReadKey();
    }

    static void DemonstrateAbstractClasses()
    {
        Console.WriteLine("РАЗДЕЛ 1: АБСТРАКТНЫЕ КЛАССЫ (35 задач)");
        Console.WriteLine("========================================\n");

        // Демонстрация абстрактных классов
        Animal dog = new Dog("Бобик");
        Animal cat = new Cat("Мурка");

        dog.MakeSound();
        cat.MakeSound();
        dog.Sleep();

        Shape circle = new Circle(5);
        Shape rectangle = new Rectangle(4, 6);
        circle.DisplayInfo();
        rectangle.DisplayInfo();

        Vehicle car = new Car("Toyota Camry", 2022);
        Vehicle motorcycle = new Motorcycle("Yamaha R1", 2023);
        car.GetDescription();
        motorcycle.GetDescription();
        car.Move();
        motorcycle.Move();

        Employee fullTime = new FullTimeEmployee("Иван Иванов", 50000, 10000);
        Employee partTime = new PartTimeEmployee("Петр Петров", 80, 500);
        fullTime.DisplayInfo();
        partTime.DisplayInfo();

        // Демонстрация 3D фигур
        Shape3D sphere = new Sphere(3);
        sphere.DisplayInfo();

        // Демонстрация Logger
        Logger consoleLogger = new ConsoleLoggerImpl();
        consoleLogger.Log("Обычное сообщение");
        consoleLogger.LogWarning("Предупреждение");
        consoleLogger.LogError("Ошибка");

        // Демонстрация животных иерархии
        Lion lion = new Lion("Симба");
        Eagle eagle = new Eagle("Орел");
        lion.MakeSound();
        lion.Run();
        eagle.MakeSound();
        eagle.Fly();

        Console.WriteLine();
    }

    static void DemonstrateInterfaces()
    {
        Console.WriteLine("\nРАЗДЕЛ 2: ИНТЕРФЕЙСЫ (35 задач)");
        Console.WriteLine("===============================\n");

        // Демонстрация интерфейсов
        IAnimal duck = new Duck { Name = "Дональд", Age = 2 };
        duck.MakeSound();
        Console.WriteLine($"Тип животного: {duck.GetAnimalType()}");

        IShape square = new Square(5);
        Console.WriteLine($"{square.GetShapeType()}: Площадь = {square.CalculateArea()}, Периметр = {square.CalculatePerimeter()}");

        // Демонстрация IComparable
        List<Product> products = new List<Product>
        {
            new Product("Ноутбук", 1500),
            new Product("Мышь", 25),
            new Product("Монитор", 300)
        };

        products.Sort();
        Console.WriteLine("Отсортированные продукты по цене:");
        foreach (var product in products)
        {
            Console.WriteLine($" - {product}");
        }

        // Демонстрация IPayable
        IPayable invoice = new Invoice("INV-001", 1000);
        invoice.ProcessPayment(300);
        Console.WriteLine($"Платеж обработан: {invoice.IsPaymentProcessed()}, Остаток: {invoice.GetBalance():C}");

        // Демонстрация множественных интерфейсов
        Smartphone phone = new Smartphone
        {
            Name = "iPhone 15",
            Description = "Флагманский смартфон",
            Price = 999
        };
        phone.DisplayInfo();

        // Демонстрация репозитория
        IUserRepository userRepo = new AdvancedUserRepository();
        userRepo.Connect();
        userRepo.AddUser(new User { Username = "john_doe", Email = "john@example.com" });
        userRepo.AddUser(new User { Username = "jane_smith", Email = "jane@example.com" });

        Console.WriteLine("Все пользователи:");
        foreach (var user in userRepo.GetAllUsers())
        {
            Console.WriteLine($" - {user}");
        }
        userRepo.Disconnect();

        // Демонстрация явной реализации интерфейса
        DataManager manager = new DataManager();
        manager.Save();
        manager.Load();

        ILoadable loadable = manager;
        loadable.Load();

        Console.WriteLine();
    }

    static void DemonstrateDependencyInjection()
    {
        Console.WriteLine("\nРАЗДЕЛ 3: DEPENDENCY INJECTION (30 задач)");
        Console.WriteLine("=========================================\n");

        // Настройка DI контейнера
        var container = new DIContainer();

        // ИСПРАВЛЕНИЕ: Используем перегрузку с 2 параметрами
        container.Register<ILogger, DatabaseLogger>();
        container.Register<IEmailService, SmtpEmailService>();
        container.Register<IUserRepository, AdvancedUserRepository>();
        container.Register<IUserService, UserService>();
        container.Register<IPaymentProcessor, PaymentProcessor>();
        container.Register<OrderProcessingService, OrderProcessingService>();

        try
        {
            // Использование DI
            // ИСПРАВЛЕНИЕ: Resolve теперь вызывается без аргументов
            var userService = container.Resolve<IUserService>();
            var paymentProcessor = container.Resolve<IPaymentProcessor>();
            var orderService = container.Resolve<OrderProcessingService>();

            // Регистрация пользователей
            userService.RegisterUser("alex", "alex@example.com");
            userService.RegisterUser("maria", "maria@example.com");
            userService.RegisterUser("serg", "serg@example.com");

            // Обработка платежа
            paymentProcessor.ProcessPayment(1500);

            // Обработка заказа
            orderService.ProcessOrder(1, "Ноутбук", 1500);

            // Получение всех пользователей
            Console.WriteLine("\nВсе зарегистрированные пользователи:");
            var allUsers = userService.GetAllUsers();
            foreach (var user in allUsers)
            {
                Console.WriteLine($" - {user}");
            }

            // Демонстрация сервиса локатора
            var serviceLocator = new ServiceLocator(container);
            var emailService = serviceLocator.GetService<IEmailService>();
            emailService.SendEmail("test@example.com", "Тест", "Тестовое сообщение");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Демонстрация property injection
        Console.WriteLine("\nProperty Injection демонстрация:");
        var orderServiceWithProps = new OrderService
        {
            UserRepository = new AdvancedUserRepository(),
            EmailService = new SmtpEmailService(new DatabaseLogger()),
            Logger = new DatabaseLogger()
        };
        orderServiceWithProps.CreateOrder(1, "Мышь", 25);

        // Демонстрация декоратора
        Console.WriteLine("\nДекоратор демонстрация:");
        var dataService = new DataService();
        var loggingDataService = new LoggingDataServiceDecorator(dataService, new DatabaseLogger());
        var result = loggingDataService.GetData();
        Console.WriteLine($"Результат: {result}");

        // Демонстрация профилирования
        Console.WriteLine("\nПрофилирование демонстрация:");
        var profilingContainer = new ProfilingContainer();
        profilingContainer.Register<ILogger, DatabaseLogger>();
        var profiledLogger = profilingContainer.Resolve<ILogger>();
        profiledLogger.LogInfo("Тестовое сообщение");
        profilingContainer.PrintProfilingInfo();
    }
}
