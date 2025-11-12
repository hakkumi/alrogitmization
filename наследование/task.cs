using System;
using System.Collections.Generic;
using System.Linq;

// ==============================================
// РАЗДЕЛ 1: НАСЛЕДОВАНИЕ
// ==============================================

// 1. Базовый класс Animal и производный Cat
class Animal
{
    protected string name;
    protected int age;

    public Animal(string name, int age)
    {
        this.name = name;
        this.age = age;
    }

    public virtual void MakeSound()
    {
        Console.WriteLine("Животное издает звук");
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Имя: {name}, Возраст: {age}");
    }
}

class Cat : Animal
{
    private string breed;

    public Cat(string name, int age, string breed) : base(name, age)
    {
        this.breed = breed;
    }

    public override void MakeSound()
    {
        Console.WriteLine("Мяу! Мяу!");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Порода: {breed}");
    }

    public void ClimbTree()
    {
        Console.WriteLine($"{name} лазает по дереву");
    }
}

// 2. Класс Vehicle с производными Car и Bus
class Vehicle
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public Vehicle(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
    }

    public virtual void Start()
    {
        Console.WriteLine($"{Brand} {Model} запускается");
    }

    public virtual void Stop()
    {
        Console.WriteLine($"{Brand} {Model} останавливается");
    }
}

class Car : Vehicle
{
    public int Doors { get; set; }

    public Car(string brand, string model, int year, int doors)
        : base(brand, model, year)
    {
        Doors = doors;
    }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("Двигатель автомобиля завелся");
    }

    public void OpenTrunk()
    {
        Console.WriteLine("Багажник открыт");
    }
}

class Bus : Vehicle
{
    public int PassengerCapacity { get; set; }

    public Bus(string brand, string model, int year, int capacity)
        : base(brand, model, year)
    {
        PassengerCapacity = capacity;
    }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("Дизельный двигатель автобуса запущен");
    }

    public void AnnounceStop()
    {
        Console.WriteLine("Остановка! Будьте осторожны при выходе");
    }
}

// 3. Класс Shape с производными Circle и Rectangle
class Shape
{
    protected string color;

    public Shape(string color)
    {
        this.color = color;
    }

    public virtual double CalculateArea()
    {
        return 0;
    }

    public virtual double CalculatePerimeter()
    {
        return 0;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Фигура цвета: {color}");
    }
}

class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(string color, double radius) : base(color)
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

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Круг: радиус = {Radius}, площадь = {CalculateArea():F2}");
    }
}

class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(string color, double width, double height) : base(color)
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

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Прямоугольник: {Width}x{Height}, площадь = {CalculateArea():F2}");
    }
}

// 4. Добавление метода в базовый класс и переопределение в производном
class BaseClass
{
    public virtual void ShowMessage()
    {
        Console.WriteLine("Сообщение из базового класса");
    }
}

class DerivedClass : BaseClass
{
    public override void ShowMessage()
    {
        Console.WriteLine("Сообщение из производного класса");
    }
}

// 5. Конструктор базового класса и вызов через base
class Person
{
    protected string name;
    protected int age;

    public Person(string name, int age)
    {
        this.name = name;
        this.age = age;
        Console.WriteLine($"Создан Person: {name}, {age} лет");
    }
}

class Student : Person
{
    public int Grade { get; set; }

    public Student(string name, int age, int grade) : base(name, age)
    {
        Grade = grade;
        Console.WriteLine($"Создан Student: {name}, {age} лет, {grade} класс");
    }
}

// 6. Иерархия классов для сотрудников
class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    public Employee(string name, decimal salary)
    {
        Name = name;
        Salary = salary;
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} выполняет работу");
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Сотрудник: {Name}, Зарплата: {Salary:C}");
    }
}

class Manager : Employee
{
    public string Department { get; set; }

    public Manager(string name, decimal salary, string department)
        : base(name, salary)
    {
        Department = department;
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} управляет отделом {Department}");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Должность: Менеджер, Отдел: {Department}");
    }

    public void ConductMeeting()
    {
        Console.WriteLine($"{Name} проводит собрание");
    }
}

class Developer : Employee
{
    public string ProgrammingLanguage { get; set; }

    public Developer(string name, decimal salary, string language)
        : base(name, salary)
    {
        ProgrammingLanguage = language;
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} пишет код на {ProgrammingLanguage}");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Должность: Разработчик, Язык: {ProgrammingLanguage}");
    }

    public void DebugCode()
    {
        Console.WriteLine($"{Name} отлаживает код");
    }
}

// 7. Наследование от Person к Student
class PersonBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public PersonBase(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public int GetAge()
    {
        var today = DateTime.Today;
        int age = today.Year - BirthDate.Year;
        if (BirthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}

class StudentDerived : PersonBase
{
    public string StudentId { get; set; }
    public string Major { get; set; }

    public StudentDerived(string firstName, string lastName, DateTime birthDate,
                         string studentId, string major)
        : base(firstName, lastName, birthDate)
    {
        StudentId = studentId;
        Major = major;
    }

    public void Study()
    {
        Console.WriteLine($"{GetFullName()} изучает {Major}");
    }
}

// 8. Protected поля и доступ из наследника
class BaseWithProtected
{
    protected int protectedField;
    private int privateField;

    public BaseWithProtected(int value)
    {
        protectedField = value;
        privateField = value * 2;
    }

    protected void ProtectedMethod()
    {
        Console.WriteLine($"Protected метод: {protectedField}");
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Protected: {protectedField}, Private: {privateField}");
    }
}

class DerivedWithProtected : BaseWithProtected
{
    public DerivedWithProtected(int value) : base(value) { }

    public void AccessProtectedMembers()
    {
        // Доступ к protected полю
        protectedField += 10;

        // Вызов protected метода
        ProtectedMethod();

        // privateField недоступен здесь - ошибка компиляции
        // privateField = 5;

        Console.WriteLine($"Производный класс изменил protected поле: {protectedField}");
    }
}

// 9. Наследование для геометрических фигур
abstract class GeometricShape
{
    public string Name { get; set; }

    public GeometricShape(string name)
    {
        Name = name;
    }

    public abstract double GetArea();
    public abstract double GetPerimeter();

    public virtual void Display()
    {
        Console.WriteLine($"Фигура: {Name}");
    }
}

class Triangle : GeometricShape
{
    public double SideA { get; set; }
    public double SideB { get; set; }
    public double SideC { get; set; }

    public Triangle(double a, double b, double c) : base("Треугольник")
    {
        SideA = a;
        SideB = b;
        SideC = c;
    }

    public override double GetArea()
    {
        // Формула Герона
        double p = GetPerimeter() / 2;
        return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
    }

    public override double GetPerimeter()
    {
        return SideA + SideB + SideC;
    }

    public override void Display()
    {
        base.Display();
        Console.WriteLine($"Стороны: {SideA}, {SideB}, {SideC}");
        Console.WriteLine($"Площадь: {GetArea():F2}, Периметр: {GetPerimeter():F2}");
    }
}

// 10. Метод в базовом классе, используемый в производных
class CalculatorBase
{
    protected double result;

    public CalculatorBase()
    {
        result = 0;
    }

    public virtual void Add(double value)
    {
        result += value;
        Console.WriteLine($"Добавлено {value}, результат: {result}");
    }

    public virtual void Subtract(double value)
    {
        result -= value;
        Console.WriteLine($"Вычтено {value}, результат: {result}");
    }

    public void Clear()
    {
        result = 0;
        Console.WriteLine("Результат сброшен");
    }

    public double GetResult() => result;
}

class ScientificCalculator : CalculatorBase
{
    public void Square()
    {
        result = result * result;
        Console.WriteLine($"Возведено в квадрат, результат: {result}");
    }

    public void SquareRoot()
    {
        if (result >= 0)
        {
            result = Math.Sqrt(result);
            Console.WriteLine($"Извлечен корень, результат: {result}");
        }
    }
}

// 11. Абстрактный базовый класс с наследованием
abstract class DatabaseConnection
{
    protected string connectionString;
    protected bool isConnected;

    public DatabaseConnection(string connectionString)
    {
        this.connectionString = connectionString;
        isConnected = false;
    }

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void ExecuteQuery(string query);

    public bool IsConnected => isConnected;
}

class SqlConnection : DatabaseConnection
{
    public SqlConnection(string connectionString) : base(connectionString) { }

    public override void Connect()
    {
        Console.WriteLine("Подключение к SQL Server...");
        isConnected = true;
    }

    public override void Disconnect()
    {
        Console.WriteLine("Отключение от SQL Server...");
        isConnected = false;
    }

    public override void ExecuteQuery(string query)
    {
        if (!isConnected)
            throw new InvalidOperationException("Нет подключения к базе данных");

        Console.WriteLine($"Выполнение SQL запроса: {query}");
    }
}

// 12. Виртуальный метод для демонстрации переопределения
class PaymentProcessor
{
    protected decimal amount;

    public PaymentProcessor(decimal amount)
    {
        this.amount = amount;
    }

    public virtual void ProcessPayment()
    {
        Console.WriteLine($"Обработка платежа на сумму {amount:C}");
    }

    public virtual decimal CalculateFee()
    {
        return amount * 0.02m; // 2% комиссия
    }
}

class CreditCardProcessor : PaymentProcessor
{
    public CreditCardProcessor(decimal amount) : base(amount) { }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Обработка платежа по кредитной карте на сумму {amount:C}");
    }

    public override decimal CalculateFee()
    {
        return amount * 0.03m; // 3% комиссия для кредитных карт
    }
}

class PayPalProcessor : PaymentProcessor
{
    public PayPalProcessor(decimal amount) : base(amount) { }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Обработка платежа через PayPal на сумму {amount:C}");
    }

    public override decimal CalculateFee()
    {
        return amount * 0.025m + 0.30m; // 2.5% + $0.30
    }
}

// 13. Базовый класс и несколько производных для животных
class AnimalBase
{
    public string Species { get; set; }
    public string Habitat { get; set; }

    public AnimalBase(string species, string habitat)
    {
        Species = species;
        Habitat = habitat;
    }

    public virtual void Eat()
    {
        Console.WriteLine($"{Species} питается");
    }

    public virtual void Sleep()
    {
        Console.WriteLine($"{Species} спит");
    }

    public virtual void Move()
    {
        Console.WriteLine($"{Species} перемещается");
    }
}

class Mammal : AnimalBase
{
    public bool HasFur { get; set; }

    public Mammal(string species, string habitat, bool hasFur)
        : base(species, habitat)
    {
        HasFur = hasFur;
    }

    public override void Move()
    {
        Console.WriteLine($"{Species} ходит по земле");
    }

    public void GiveBirth()
    {
        Console.WriteLine($"{Species} рожает детенышей");
    }
}

class Bird : AnimalBase
{
    public double Wingspan { get; set; }

    public Bird(string species, string habitat, double wingspan)
        : base(species, habitat)
    {
        Wingspan = wingspan;
    }

    public override void Move()
    {
        Console.WriteLine($"{Species} летает в небе");
    }

    public void BuildNest()
    {
        Console.WriteLine($"{Species} строит гнездо");
    }
}

class Fish : AnimalBase
{
    public bool CanBreatheAir { get; set; }

    public Fish(string species, string habitat, bool canBreatheAir)
        : base(species, habitat)
    {
        CanBreatheAir = canBreatheAir;
    }

    public override void Move()
    {
        Console.WriteLine($"{Species} плавает в воде");
    }

    public void Swim()
    {
        Console.WriteLine($"{Species} быстро плывет");
    }
}

// 14. Интерфейс и реализация в производном классе
interface IFlyable
{
    void Fly();
    double MaxAltitude { get; }
}

interface ISwimmable
{
    void Swim();
    double MaxDepth { get; }
}

class Duck : AnimalBase, IFlyable, ISwimmable
{
    public double MaxAltitude => 1000;
    public double MaxDepth => 5;

    public Duck(string species, string habitat) : base(species, habitat) { }

    public void Fly()
    {
        Console.WriteLine($"{Species} летит на высоте до {MaxAltitude} метров");
    }

    public void Swim()
    {
        Console.WriteLine($"{Species} плавает на глубине до {MaxDepth} метров");
    }

    public override void Move()
    {
        Console.WriteLine($"{Species} может летать, плавать и ходить");
    }
}

// 15. Множественная иерархия наследования
class ElectronicDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; set; }

    public ElectronicDevice(string brand, string model)
    {
        Brand = brand;
        Model = model;
        IsOn = false;
    }

    public virtual void TurnOn()
    {
        IsOn = true;
        Console.WriteLine($"{Brand} {Model} включен");
    }

    public virtual void TurnOff()
    {
        IsOn = false;
        Console.WriteLine($"{Brand} {Model} выключен");
    }
}

class Computer : ElectronicDevice
{
    public int RAM { get; set; }
    public string Processor { get; set; }

    public Computer(string brand, string model, int ram, string processor)
        : base(brand, model)
    {
        RAM = ram;
        Processor = processor;
    }

    public void RunProgram(string program)
    {
        if (IsOn)
            Console.WriteLine($"Запуск программы: {program}");
        else
            Console.WriteLine("Сначала включите компьютер");
    }
}

class Laptop : Computer
{
    public double BatteryLife { get; set; }
    public double ScreenSize { get; set; }

    public Laptop(string brand, string model, int ram, string processor,
                  double batteryLife, double screenSize)
        : base(brand, model, ram, processor)
    {
        BatteryLife = batteryLife;
        ScreenSize = screenSize;
    }

    public void CloseLid()
    {
        Console.WriteLine("Крышка ноутбука закрыта, переход в спящий режим");
    }
}

// ==============================================
// РАЗДЕЛ 2: МОДИФИКАТОРЫ ДОСТУПА
// ==============================================

// 16. Класс с приватным полем и публичным свойством
class AccessExample
{
    private int privateField;
    protected int protectedField;
    internal int internalField;
    public int publicField;

    public int PrivateField
    {
        get { return privateField; }
        set { privateField = value; }
    }

    public AccessExample()
    {
        privateField = 1;
        protectedField = 2;
        internalField = 3;
        publicField = 4;
    }

    public void ShowAllFields()
    {
        Console.WriteLine($"Private: {privateField}, Protected: {protectedField}, " +
                         $"Internal: {internalField}, Public: {publicField}");
    }
}

// 17. Protected поле и доступ из наследника
class BaseWithAccess
{
    protected string protectedData = "Защищенные данные";
    private string privateData = "Приватные данные";

    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected метод вызван");
    }
}

class DerivedAccess : BaseWithAccess
{
    public void AccessProtected()
    {
        // Доступ к protected полю
        Console.WriteLine($"Доступ к protected полю: {protectedData}");

        // Вызов protected метода
        ProtectedMethod();

        // privateData недоступен - ошибка компиляции
        // Console.WriteLine(privateData);
    }
}

// 18. Internal класс (работает только в той же сборке)
internal class InternalClass
{
    public string Message { get; set; } = "Internal класс";

    public void ShowMessage()
    {
        Console.WriteLine(Message);
    }
}

// 19. Protected internal поле
class ProtectedInternalExample
{
    protected internal string protectedInternalField = "Protected Internal поле";

    protected internal void ProtectedInternalMethod()
    {
        Console.WriteLine("Protected Internal метод");
    }
}

class DerivedProtectedInternal : ProtectedInternalExample
{
    public void TestAccess()
    {
        // Доступ к protected internal членам
        Console.WriteLine(protectedInternalField);
        ProtectedInternalMethod();
    }
}

// 20. Публичное поле и изменение извне
class PublicFieldExample
{
    public int PublicValue;
    private int _privateValue;

    public PublicFieldExample(int value)
    {
        PublicValue = value;
        _privateValue = value * 2;
    }

    public void ShowValues()
    {
        Console.WriteLine($"Public: {PublicValue}, Private: {_privateValue}");
    }
}

// 21. Private модификатор защищает поле от изменений извне
class SecureData
{
    private string _secretData;
    private int _accessCount;

    public SecureData(string data)
    {
        _secretData = data;
        _accessCount = 0;
    }

    public string GetSecretData()
    {
        _accessCount++;
        return $"Данные: {_secretData} (доступов: {_accessCount})";
    }

    // Нет публичного сеттера - данные защищены от изменений извне
}

// 22. Protected метод в базовом классе
class BaseWithProtectedMethod
{
    protected void InternalCalculation(int x, int y)
    {
        Console.WriteLine($"Внутренний расчет: {x} + {y} = {x + y}");
    }

    public void PublicMethod()
    {
        Console.WriteLine("Публичный метод");
        InternalCalculation(5, 3); // Доступен внутри класса
    }
}

class DerivedProtectedMethod : BaseWithProtectedMethod
{
    public void UseProtectedMethod()
    {
        InternalCalculation(10, 20); // Доступен в наследнике
    }
}

// 23. Защищённый конструктор
class ClassWithProtectedConstructor
{
    protected string data;

    protected ClassWithProtectedConstructor(string data)
    {
        this.data = data;
        Console.WriteLine($"Защищенный конструктор вызван с данными: {data}");
    }

    public static ClassWithProtectedConstructor CreateInstance(string data)
    {
        return new ClassWithProtectedConstructor(data);
    }
}

class DerivedFromProtected : ClassWithProtectedConstructor
{
    public DerivedFromProtected(string data) : base(data)
    {
        // Может вызывать protected конструктор
    }
}

// 24. Internal модификатор (доступ только в сборке)
internal class InternalOnly
{
    public string InternalMessage = "Этот класс виден только в текущей сборке";
}

// 25. Ошибка доступа к приватному полю
class PrivateAccessExample
{
    private string _privateInfo = "Секретная информация";

    public void TryAccessPrivate()
    {
        // Внутри класса доступ есть
        Console.WriteLine($"Внутренний доступ: {_privateInfo}");
    }

    // Попытка доступа извне приведет к ошибке компиляции
}

// 26. Open-класс с public методами
public class OpenClass
{
    public string PublicProperty { get; set; }

    public void PublicMethod()
    {
        Console.WriteLine("Публичный метод открытого класса");
    }

    public virtual void VirtualMethod()
    {
        Console.WriteLine("Виртуальный метод для переопределения");
    }
}

// 27. Закрытый метод, доступный только внутри класса
class ClassWithPrivateMethod
{
    private void SecretOperation()
    {
        Console.WriteLine("Секретная операция выполняется");
    }

    public void PublicOperation()
    {
        Console.WriteLine("Начало публичной операции");
        SecretOperation(); // Внутренний вызов
        Console.WriteLine("Завершение публичной операции");
    }
}

// 28. Разница между internal и public классами
public class PublicClass
{
    public string Message = "Публичный класс - виден везде";
}

internal class InternalClassDemo
{
    public string Message = "Internal класс - виден только в сборке";
}

// 29. Класс с protected internal методом
class ProtectedInternalDemo
{
    protected internal void HybridMethod()
    {
        Console.WriteLine("Protected Internal метод - доступен в сборке и наследникам");
    }
}

// 30. Доступ к приватному свойству через метод класса
class PropertyAccess
{
    private string _encryptedData;

    public PropertyAccess(string data)
    {
        _encryptedData = Encrypt(data);
    }

    private string Encrypt(string data)
    {
        // Простое "шифрование" для демонстрации
        char[] array = data.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    private string Decrypt(string data)
    {
        return Encrypt(data); // Обратимое "шифрование"
    }

    public string GetDecryptedData()
    {
        return Decrypt(_encryptedData);
    }

    public void SetData(string data)
    {
        _encryptedData = Encrypt(data);
    }
}

// ==============================================
// РАЗДЕЛ 3: ВЫЗОВ КОНСТРУКТОРА БАЗОВОГО КЛАССА
// ==============================================

// 31. Вызов конструктора базового класса через base
class BaseConstructor
{
    protected int baseValue;

    public BaseConstructor(int value)
    {
        baseValue = value;
        Console.WriteLine($"Базовый конструктор: baseValue = {baseValue}");
    }
}

class DerivedConstructor : BaseConstructor
{
    private int derivedValue;

    public DerivedConstructor(int baseVal, int derivedVal) : base(baseVal)
    {
        derivedValue = derivedVal;
        Console.WriteLine($"Производный конструктор: derivedValue = {derivedValue}");
    }
}

// 32. Производный класс передает параметры базовому
class VehicleBase
{
    protected string type;

    public VehicleBase(string type)
    {
        this.type = type;
        Console.WriteLine($"Создан транспорт: {type}");
    }
}

class CarDerived : VehicleBase
{
    public string Model { get; set; }

    public CarDerived(string model) : base("Автомобиль")
    {
        Model = model;
        Console.WriteLine($"Модель: {Model}");
    }
}

// 33. Цепочка конструкторов с вызовом базового
class GrandParent
{
    protected string familyName;

    public GrandParent(string name)
    {
        familyName = name;
        Console.WriteLine($"Предок: фамилия {familyName}");
    }
}

class Parent : GrandParent
{
    protected string firstName;

    public Parent(string familyName, string firstName) : base(familyName)
    {
        this.firstName = firstName;
        Console.WriteLine($"Родитель: {firstName} {familyName}");
    }
}

class Child : Parent
{
    public Child(string familyName, string parentName, string childName)
        : base(familyName, parentName)
    {
        Console.WriteLine($"Ребенок: {childName} {familyName}");
    }
}

// 34. Поля и методы базового конструктора
class BaseWithFields
{
    protected int x, y;

    public BaseWithFields(int x, int y)
    {
        this.x = x;
        this.y = y;
        Console.WriteLine($"Базовый: x={x}, y={y}");
    }

    public int Calculate()
    {
        return x + y;
    }
}

class DerivedWithFields : BaseWithFields
{
    public DerivedWithFields(int a, int b) : base(a, b)
    {
        Console.WriteLine($"Производный: результат = {Calculate()}");
    }
}

// 35. Несколько перегруженных конструкторов
class MultiConstructorBase
{
    protected string data;

    public MultiConstructorBase()
    {
        data = "По умолчанию";
        Console.WriteLine("Базовый конструктор по умолчанию");
    }

    public MultiConstructorBase(string data)
    {
        this.data = data;
        Console.WriteLine($"Базовый конструктор с параметром: {data}");
    }
}

class MultiConstructorDerived : MultiConstructorBase
{
    public MultiConstructorDerived() : base()
    {
        Console.WriteLine("Производный конструктор по умолчанию");
    }

    public MultiConstructorDerived(string data) : base(data)
    {
        Console.WriteLine("Производный конструктор с параметром");
    }

    public MultiConstructorDerived(string data, int number) : base(data + number)
    {
        Console.WriteLine("Производный конструктор с двумя параметрами");
    }
}

// 36. Передача параметров в базовый конструктор
class ParameterBase
{
    protected int value;

    public ParameterBase(int val)
    {
        value = val;
        Console.WriteLine($"Базовый конструктор получил: {val}");
    }
}

class ParameterDerived : ParameterBase
{
    public ParameterDerived(int input) : base(input * 2)
    {
        Console.WriteLine($"Производный конструктор, базовое значение: {value}");
    }
}

// 37. Вызов конструктора базового класса для инициализации
class InitializationBase
{
    protected List<string> items;

    public InitializationBase(List<string> initialItems)
    {
        items = new List<string>(initialItems);
        Console.WriteLine($"Базовый: инициализировано {items.Count} элементов");
    }
}

class InitializationDerived : InitializationBase
{
    public InitializationDerived(params string[] items) : base(new List<string>(items))
    {
        Console.WriteLine($"Производный: добавлено {items.Length} элементов");
    }
}

// 38. Инициализация поля базового класса в производном
class FieldInitializationBase
{
    protected int baseField;

    public FieldInitializationBase(int value)
    {
        baseField = value;
    }
}

class FieldInitializationDerived : FieldInitializationBase
{
    private int derivedField;

    public FieldInitializationDerived(int baseVal, int derivedVal) : base(baseVal)
    {
        derivedField = derivedVal;
        Console.WriteLine($"Базовое поле: {baseField}, Производное поле: {derivedField}");
    }
}

// 39. Конструктор базового класса с разными типами параметров
class TypedBase
{
    protected object data;

    public TypedBase(object data)
    {
        this.data = data;
        Console.WriteLine($"Базовый конструктор с типом: {data.GetType().Name}");
    }
}

class TypedDerived : TypedBase
{
    public TypedDerived(string text) : base(text) { }
    public TypedDerived(int number) : base(number) { }
    public TypedDerived(double value) : base(value) { }
}

// 40. Конструктор по умолчанию и с параметрами при наследовании
class DefaultConstructorBase
{
    protected string name;

    public DefaultConstructorBase()
    {
        name = "Неизвестно";
        Console.WriteLine("Базовый конструктор по умолчанию");
    }

    public DefaultConstructorBase(string name)
    {
        this.name = name;
        Console.WriteLine($"Базовый конструктор с именем: {name}");
    }
}

class DefaultConstructorDerived : DefaultConstructorBase
{
    public DefaultConstructorDerived() : base()
    {
        Console.WriteLine("Производный конструктор по умолчанию");
    }

    public DefaultConstructorDerived(string name) : base(name)
    {
        Console.WriteLine("Производный конструктор с именем");
    }
}

// 41. Вызов базового конструктора с использованием this и base
class ComplexBase
{
    protected int x, y;

    public ComplexBase(int x, int y)
    {
        this.x = x;
        this.y = y;
        Console.WriteLine($"Базовый: x={x}, y={y}");
    }
}

class ComplexDerived : ComplexBase
{
    private int z;

    public ComplexDerived(int x, int y, int z) : base(x, y)
    {
        this.z = z;
        Console.WriteLine($"Производный: z={z}");
    }

    public ComplexDerived(int value) : this(value, value, value)
    {
        Console.WriteLine("Производный конструктор с одним параметром");
    }
}

// 42. Логика инициализации в базовом конструкторе
class LogicBase
{
    protected DateTime created;
    protected Guid id;

    public LogicBase()
    {
        created = DateTime.Now;
        id = Guid.NewGuid();
        Console.WriteLine($"Базовый: создан {created:HH:mm:ss}, ID: {id}");
    }
}

class LogicDerived : LogicBase
{
    public LogicDerived()
    {
        Console.WriteLine("Производный: логика инициализации завершена");
    }
}

// 43. Конструктор базового класса для наследования свойств
class PropertyBase
{
    public string Name { get; protected set; }
    public int Age { get; protected set; }

    public PropertyBase(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Базовый: {Name}, {Age} лет");
    }
}

class PropertyDerived : PropertyBase
{
    public string AdditionalInfo { get; set; }

    public PropertyDerived(string name, int age, string info) : base(name, age)
    {
        AdditionalInfo = info;
        Console.WriteLine($"Производный: дополнительная информация - {info}");
    }
}

// 44. Поля к базовому конструктору и инициализация в производном
class FieldBase
{
    protected int[] numbers;

    public FieldBase(int[] numbers)
    {
        this.numbers = numbers;
        Console.WriteLine($"Базовый: массив из {numbers.Length} элементов");
    }
}

class FieldDerived : FieldBase
{
    public FieldDerived(int size) : base(new int[size])
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i * i;
        }
        Console.WriteLine($"Производный: заполнен массив квадратами");
    }
}

// 45. Цепочка вызова конструкторов с наследованием
class ChainA
{
    public ChainA()
    {
        Console.WriteLine("Конструктор A");
    }
}

class ChainB : ChainA
{
    public ChainB() : base()
    {
        Console.WriteLine("Конструктор B");
    }
}

class ChainC : ChainB
{
    public ChainC() : base()
    {
        Console.WriteLine("Конструктор C");
    }
}

// ==============================================
// РАЗДЕЛ 4: UPCAST (ПРИВЕДЕНИЕ К БАЗОВОМУ ТИПУ)
// ==============================================

// 46. Upcast - присвоение производного объекта базовому типу
class AnimalUpcast
{
    public virtual void Speak()
    {
        Console.WriteLine("Животное издает звук");
    }
}

class DogUpcast : AnimalUpcast
{
    public override void Speak()
    {
        Console.WriteLine("Собака лает: Гав-гав!");
    }

    public void Fetch()
    {
        Console.WriteLine("Собака приносит палку");
    }
}

// Создаем класс CatUpcast, наследующий от AnimalUpcast
class CatUpcast : AnimalUpcast
{
    public override void Speak()
    {
        Console.WriteLine("Кошка мяукает: Мяу-мяу!");
    }

    public void ClimbTree()
    {
        Console.WriteLine("Кошка лазает по дереву");
    }
}

// 47. Upcast с интерфейсами
interface IMovable
{
    void Move();
}

class CarUpcast : IMovable
{
    public void Move()
    {
        Console.WriteLine("Машина едет по дороге");
    }

    public void Honk()
    {
        Console.WriteLine("Би-бип!");
    }
}

// 48. Upcast для коллекции базовых объектов
class ShapeUpcast
{
    public virtual void Draw()
    {
        Console.WriteLine("Рисую фигуру");
    }
}

class CircleUpcast : ShapeUpcast
{
    public override void Draw()
    {
        Console.WriteLine("Рисую круг");
    }

    public void Fill()
    {
        Console.WriteLine("Закрашиваю круг");
    }
}

class SquareUpcast : ShapeUpcast
{
    public override void Draw()
    {
        Console.WriteLine("Рисую квадрат");
    }

    public void Rotate()
    {
        Console.WriteLine("Поворачиваю квадрат");
    }
}

// 49. Передача производного объекта методу, принимающему базовый
class Processor
{
    public void Process(AnimalUpcast animal)
    {
        Console.WriteLine("Обработка животного:");
        animal.Speak();
    }
}

// 50. Массив базового типа с производными объектами
class EmployeeUpcast
{
    public string Name { get; set; }

    public EmployeeUpcast(string name)
    {
        Name = name;
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} выполняет работу");
    }
}

class ManagerUpcast : EmployeeUpcast
{
    public ManagerUpcast(string name) : base(name) { }

    public override void Work()
    {
        Console.WriteLine($"{Name} управляет командой");
    }

    public void ConductMeeting()
    {
        Console.WriteLine($"{Name} проводит собрание");
    }
}

class DeveloperUpcast : EmployeeUpcast
{
    public DeveloperUpcast(string name) : base(name) { }

    public override void Work()
    {
        Console.WriteLine($"{Name} пишет код");
    }

    public void Debug()
    {
        Console.WriteLine($"{Name} отлаживает программу");
    }
}

// 51. Upcast при работе с абстрактным классом
abstract class Device
{
    public abstract void TurnOn();
    public abstract void TurnOff();
}

class Phone : Device
{
    public override void TurnOn()
    {
        Console.WriteLine("Телефон включается");
    }

    public override void TurnOff()
    {
        Console.WriteLine("Телефон выключается");
    }

    public void Call()
    {
        Console.WriteLine("Телефон звонит");
    }
}

// 52. Upcast для вызова базовых методов от производного объекта
class BaseMethod
{
    public virtual void Show()
    {
        Console.WriteLine("Метод базового класса");
    }
}

class DerivedMethod : BaseMethod
{
    public override void Show()
    {
        Console.WriteLine("Метод производного класса");
    }

    public void SpecialMethod()
    {
        Console.WriteLine("Специальный метод производного класса");
    }
}

// 53. Сохранение ссылки на производный объект как на базовый
class ReferenceStorage
{
    public static void StoreBase(BaseMethod obj)
    {
        Console.WriteLine("Объект сохранен как базовый тип");
        obj.Show();
    }
}

// 54. Преобразование производного объекта к базовому типу
class ConversionExample
{
    public static void DemonstrateUpcast()
    {
        DerivedMethod derived = new DerivedMethod();
        BaseMethod baseRef = derived; // Неявный upcast

        baseRef.Show(); // Вызовет переопределенный метод
    }
}

// 55. Присваивание производного объекта базовому типу всегда доступно
class AlwaysAvailableUpcast
{
    public static void TestUpcast()
    {
        // Все эти присваивания допустимы
        object obj1 = new DogUpcast();
        AnimalUpcast animal1 = new DogUpcast();
        IMovable movable1 = new CarUpcast();
        Device device1 = new Phone();

        Console.WriteLine("Upcast всегда доступен для совместимых типов");
    }
}

// ==============================================
// РАЗДЕЛ 5: DOWNCAST (ПРИВЕДЕНИЕ К ПРОИЗВОДНОМУ ТИПУ)
// ==============================================

// 56. Преобразование объекта базового типа к производному через cast
class DowncastExample
{
    public static void TestDowncast()
    {
        AnimalUpcast animal = new DogUpcast(); // Upcast

        // Downcast с явным приведением
        DogUpcast dog = (DogUpcast)animal;
        dog.Speak();
        dog.Fetch();
    }
}

// 57. Оператор as для приведения типов
class AsOperatorExample
{
    public static void TestAsOperator()
    {
        AnimalUpcast animal = new DogUpcast();

        // Безопасное приведение с оператором as
        DogUpcast dog = animal as DogUpcast;
        if (dog != null)
        {
            dog.Fetch();
        }
        else
        {
            Console.WriteLine("Приведение не удалось");
        }
    }
}

// 58. Проверка типа с оператором is перед приведением
class IsOperatorExample
{
    public static void TestIsOperator()
    {
        AnimalUpcast animal = new DogUpcast();

        // Проверка типа перед приведением
        if (animal is DogUpcast)
        {
            DogUpcast dog = (DogUpcast)animal;
            dog.Fetch();
        }

        // Современный синтаксис C# 7.0+
        if (animal is DogUpcast dog2)
        {
            dog2.Fetch();
        }
    }
}

// 59. Cast с проверкой null значения после as
class NullCheckAfterAs
{
    public static void TestNullCheck()
    {
        AnimalUpcast animal = new DogUpcast();

        var dog = animal as DogUpcast;
        if (dog != null)
        {
            Console.WriteLine("Приведение успешно:");
            dog.Speak();
        }
        else
        {
            Console.WriteLine("Приведение не удалось - объект null");
        }
    }
}

// 60. Try-catch для обработки ошибочного downcast
class TryCatchDowncast
{
    public static void TestSafeDowncast()
    {
        AnimalUpcast animal = new DogUpcast();

        try
        {
            // Попытка опасного приведения - теперь используем CatUpcast вместо Cat
            CatUpcast cat = (CatUpcast)animal; // Это вызовет исключение
            cat.ClimbTree();
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Ошибка приведения: {ex.Message}");
        }
    }
}

// 61. Безопасный cast с is/as
class SafeCastExample
{
    public static void DemonstrateSafeCast()
    {
        object[] objects = { new DogUpcast(), new CatUpcast(), "строка", 42 };

        foreach (var obj in objects)
        {
            // Безопасная проверка и приведение
            if (obj is AnimalUpcast animal)
            {
                Console.Write("Найдено животное: ");
                animal.Speak();
            }
            else
            {
                Console.WriteLine($"Не животное: {obj.GetType().Name}");
            }
        }
    }
}

// 62. Доступ к методам производного класса после приведения
class AccessAfterCast
{
    public static void TestMethodAccess()
    {
        EmployeeUpcast employee = new DeveloperUpcast("Анна");

        // Upcast - доступен только базовый метод
        employee.Work();

        // Downcast для доступа к специфичным методам
        if (employee is DeveloperUpcast developer)
        {
            developer.Work();    // Базовый метод
            developer.Debug();   // Специфичный метод
        }
    }
}

// 63. Проверка приводит ли cast к exception
class CastExceptionTest
{
    public static void TestCastException()
    {
        AnimalUpcast animal = new DogUpcast();

        // Этот cast безопасен
        DogUpcast dog1 = animal as DogUpcast;
        Console.WriteLine($"Безопасный cast: {dog1 != null}");

        // Этот cast вызовет исключение
        try
        {
            CatUpcast cat = (CatUpcast)animal;
        }
        catch (InvalidCastException)
        {
            Console.WriteLine("Опасный cast вызвал исключение");
        }
    }
}

// 64. Явное приведение (cast) для downcast
class ExplicitCast
{
    public static void TestExplicitCast()
    {
        ShapeUpcast shape = new CircleUpcast();

        // Явное приведение
        CircleUpcast circle = (CircleUpcast)shape;
        circle.Draw();
        circle.Fill(); // Теперь доступен специфичный метод
    }
}

// 65. Downcast и вызов специфичных методов производного класса
class SpecificMethodCall
{
    public static void CallSpecificMethods()
    {
        // Создаем коллекцию базового типа
        List<Device> devices = new List<Device>
        {
            new Phone(),
            // Могут быть другие устройства
        };

        foreach (var device in devices)
        {
            device.TurnOn();

            // Downcast для доступа к специфичным методам
            if (device is Phone phone)
            {
                phone.Call();
            }
        }
    }
}

// ==============================================
// РАЗДЕЛ 6: ПОЛИМОРФИЗМ
// ==============================================

// 66. Виртуальный метод и переопределение
class PolymorphicBase
{
    public virtual void Display()
    {
        Console.WriteLine("Базовый класс");
    }
}

class PolymorphicDerived : PolymorphicBase
{
    public override void Display()
    {
        Console.WriteLine("Производный класс");
    }
}

// 67. Абстрактный класс с абстрактными и переопределёнными методами
abstract class AbstractShape
{
    public abstract double CalculateArea();
    public abstract double CalculatePerimeter();

    public virtual void Display()
    {
        Console.WriteLine($"Площадь: {CalculateArea():F2}, Периметр: {CalculatePerimeter():F2}");
    }
}

class ConcreteCircle : AbstractShape
{
    public double Radius { get; set; }

    public ConcreteCircle(double radius)
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

    public override void Display()
    {
        Console.Write("Круг: ");
        base.Display();
    }
}

// 68. Интерфейс для реализации полиморфизма
interface IDrawable
{
    void Draw();
    string Color { get; set; }
}

class DrawableCircle : IDrawable
{
    public string Color { get; set; }

    public void Draw()
    {
        Console.WriteLine($"Рисую круг цветом {Color}");
    }
}

class DrawableSquare : IDrawable
{
    public string Color { get; set; }

    public void Draw()
    {
        Console.WriteLine($"Рисую квадрат цветом {Color}");
    }
}

// 69. Массив базового типа с производными объектами
class Dog : Animal
{
    private string breed;

    public Dog(string name, int age, string breed) : base(name, age)
    {
        this.breed = breed;
    }

    public override void MakeSound()
    {
        Console.WriteLine("Гав! Гав!");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Порода: {breed}");
    }

    public void Fetch()
    {
        Console.WriteLine($"{name} приносит мяч!");
    }
}

class PolymorphicCollection
{
    public static void DemonstratePolymorphism()
    {
        Animal[] animals = new Animal[]
        {
            new Cat("Барсик", 3, "Персидский"),
            new Dog("Шарик", 2, "Лабрадор"),
            new Cat("Мурка", 4, "Сиамская")
        };

        foreach (var animal in animals)
        {
            animal.MakeSound(); // Полиморфный вызов
        }
    }
}

// 70. Несколько производных классов от одного базового
class Notification
{
    public virtual void Send(string message)
    {
        Console.WriteLine($"Отправка уведомления: {message}");
    }
}

class EmailNotification : Notification
{
    public override void Send(string message)
    {
        Console.WriteLine($"Отправка email: {message}");
    }
}

class SMSNotification : Notification
{
    public override void Send(string message)
    {
        Console.WriteLine($"Отправка SMS: {message}");
    }
}

class PushNotification : Notification
{
    public override void Send(string message)
    {
        Console.WriteLine($"Push уведомление: {message}");
    }
}

// 71. Интерфейсный полиморфизм с оператором as
interface IPlayable
{
    void Play();
    void Stop();
}

class MusicPlayer : IPlayable
{
    public void Play()
    {
        Console.WriteLine("Воспроизведение музыки");
    }

    public void Stop()
    {
        Console.WriteLine("Остановка музыки");
    }
}

class VideoPlayer : IPlayable
{
    public void Play()
    {
        Console.WriteLine("Воспроизведение видео");
    }

    public void Stop()
    {
        Console.WriteLine("Остановка видео");
    }

    public void Pause()
    {
        Console.WriteLine("Пауза видео");
    }
}

// 72. Динамический dispatch полиморфных объектов
class DynamicDispatch
{
    public static void Process(Notification notification)
    {
        notification.Send("Важное сообщение");
    }
}

// 73. Перегрузка методов в производном классе
class MethodOverloadBase
{
    public void Process(int number)
    {
        Console.WriteLine($"Обработка числа: {number}");
    }
}

class MethodOverloadDerived : MethodOverloadBase
{
    public void Process(string text) // Перегрузка, а не переопределение
    {
        Console.WriteLine($"Обработка текста: {text}");
    }

    public new void Process(int number) // Сокрытие метода
    {
        Console.WriteLine($"Новая обработка числа: {number * 2}");
    }
}

// 74. Полиморфизм при передаче объектов в методы
class PolymorphicParameter
{
    public static void DrawShape(Shape shape)
    {
        shape.DisplayInfo();
    }

    public static void MakeAnimalSound(Animal animal)
    {
        animal.MakeSound();
    }
}

// 75. Виртуальные и абстрактные методы для демонстрации полиморфизма
abstract class BankAccountPoly
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; protected set; }

    public BankAccountPoly(string accountNumber, decimal balance = 0)
    {
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public virtual void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"Пополнение: +{amount:C}, Баланс: {Balance:C}");
    }

    public abstract bool Withdraw(decimal amount);

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Счет: {AccountNumber}, Баланс: {Balance:C}");
    }
}

class SavingsAccountPoly : BankAccountPoly
{
    public decimal InterestRate { get; set; }

    public SavingsAccountPoly(string accountNumber, decimal balance, decimal interestRate)
        : base(accountNumber, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Снятие: -{amount:C}, Баланс: {Balance:C}");
            return true;
        }
        Console.WriteLine("Недостаточно средств на счете");
        return false;
    }

    public void ApplyInterest()
    {
        decimal interest = Balance * InterestRate;
        Balance += interest;
        Console.WriteLine($"Начислены проценты: +{interest:C}, Баланс: {Balance:C}");
    }
}

// 76. Интерфейсные методы с различными реализациями
interface ICalculator
{
    double Calculate(double a, double b);
}

class AddCalculator : ICalculator
{
    public double Calculate(double a, double b)
    {
        return a + b;
    }
}

class MultiplyCalculator : ICalculator
{
    public double Calculate(double a, double b)
    {
        return a * b;
    }
}

class PowerCalculator : ICalculator
{
    public double Calculate(double a, double b)
    {
        return Math.Pow(a, b);
    }
}

// 77. Runtime полиморфизм с переопределёнными методами
class RuntimePolymorphism
{
    public static void TestRuntimePoly()
    {
        List<BankAccountPoly> accounts = new List<BankAccountPoly>
        {
            new SavingsAccountPoly("SAV001", 1000, 0.05m),
            // Могут быть другие типы счетов
        };

        foreach (var account in accounts)
        {
            account.Deposit(500);
            account.Withdraw(200);
            account.DisplayInfo();

            // Runtime проверка типа для вызова специфичных методов
            if (account is SavingsAccountPoly savings)
            {
                savings.ApplyInterest();
            }
        }
    }
}

// 78. Базовый тип для хранения производных объектов
class BaseContainer
{
    private List<Animal> animals = new List<Animal>();

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
        Console.WriteLine($"Добавлено животное: {animal.GetType().Name}");
    }

    public void MakeAllSounds()
    {
        foreach (var animal in animals)
        {
            animal.MakeSound();
        }
    }
}

// 79. Полиморфная обработка коллекций
class PolymorphicProcessor
{
    public static void ProcessCollection(IEnumerable<IDrawable> drawables)
    {
        foreach (var drawable in drawables)
        {
            drawable.Draw();
        }
    }
}

// 80. Метод override для динамического изменения поведения наследников
class ConfigurableBase
{
    public virtual string GetConfiguration()
    {
        return "Базовая конфигурация";
    }
}

class CustomConfigurable : ConfigurableBase
{
    public override string GetConfiguration()
    {
        return "Пользовательская конфигурация";
    }
}

class AdvancedConfigurable : ConfigurableBase
{
    public override string GetConfiguration()
    {
        return "Продвинутая конфигурация: " + base.GetConfiguration();
    }
}

// ==============================================
// РАЗДЕЛ 7: ОПЕРАТОРЫ IS И AS, CAST
// ==============================================

// 81. Оператор is для проверки типа
class IsOperatorDemo
{
    public static void TestIsOperator()
    {
        object[] objects = { 42, "hello", 3.14, new DogUpcast(), new List<int>() };

        foreach (var obj in objects)
        {
            if (obj is int)
                Console.WriteLine($"{obj} - целое число");
            else if (obj is string)
                Console.WriteLine($"{obj} - строка");
            else if (obj is DogUpcast)
                Console.WriteLine("Объект - собака");
            else
                Console.WriteLine($"{obj} - неизвестный тип: {obj.GetType().Name}");
        }
    }
}

// 82. Оператор as для приведения типа
class AsOperatorDemo
{
    public static void TestAsOperator()
    {
        AnimalUpcast animal = new DogUpcast();

        // Приведение к производному типу
        var dog = animal as DogUpcast;
        if (dog != null)
        {
            Console.WriteLine("Приведение к Dog успешно");
            dog.Speak();
        }

        // Попытка приведения к несовместимому типу
        var cat = animal as CatUpcast;
        if (cat == null)
        {
            Console.WriteLine("Приведение к Cat не удалось - вернулся null");
        }
    }
}

// 83. Проверка объекта на принадлежность типу через is
class TypeCheckDemo
{
    public static void CheckTypes()
    {
        Vehicle vehicle = new Car("Toyota", "Camry", 2022, 4);

        Console.WriteLine($"vehicle is Vehicle: {vehicle is Vehicle}");
        Console.WriteLine($"vehicle is Car: {vehicle is Car}");
        Console.WriteLine($"vehicle is Bus: {vehicle is Bus}");
        Console.WriteLine($"vehicle is object: {vehicle is object}");
    }
}

// 84. Оператор as для безопасного кастинга
class SafeCastingDemo
{
    public static void DemonstrateSafeCasting()
    {
        object obj1 = new DogUpcast();
        object obj2 = "просто строка";

        // Безопасное приведение
        var dog1 = obj1 as DogUpcast;
        var dog2 = obj2 as DogUpcast;

        Console.WriteLine($"obj1 as DogUpcast: {dog1 != null}");
        Console.WriteLine($"obj2 as DogUpcast: {dog2 != null}");
    }
}

// 85. As для получения null при ошибке
class AsNullDemo
{
    public static void ShowAsBehavior()
    {
        AnimalUpcast animal = new DogUpcast();

        // Успешное приведение
        DogUpcast dog = animal as DogUpcast;
        Console.WriteLine($"animal as DogUpcast: {(dog == null ? "null" : "успех")}");

        // Неудачное приведение
        CatUpcast cat = animal as CatUpcast;
        Console.WriteLine($"animal as Cat: {(cat == null ? "null" : "успех")}");
    }
}

// 86. Логика обработки null после приведения через as
class NullHandlingDemo
{
    public static void HandleNullAfterAs()
    {
        List<object> items = new List<object>
        {
            new DogUpcast(),
            new CatUpcast(),
            "текст",
            123
        };

        foreach (var item in items)
        {
            var animal = item as AnimalUpcast;
            if (animal != null)
            {
                Console.Write($"Найдено животное: ");
                animal.Speak();
            }
            else
            {
                Console.WriteLine($"Не животное: {item.GetType().Name}");
            }
        }
    }
}

// 87. Cast и обработка исключений
class CastExceptionHandling
{
    public static void HandleCastExceptions()
    {
        AnimalUpcast animal = new DogUpcast();

        // Опасное приведение с обработкой исключения
        try
        {
            CatUpcast cat = (CatUpcast)animal;
            cat.ClimbTree();
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Исключение при приведении: {ex.Message}");
        }

        // Безопасная альтернатива
        DogUpcast dog = animal as DogUpcast;
        if (dog != null)
        {
            dog.Fetch();
        }
    }
}

// 88. Методы интерфейса после приведения as
class InterfaceAfterAs
{
    public static void TestInterfaceCast()
    {
        IPlayable player = new MusicPlayer();

        // Приведение к конкретной реализации
        MusicPlayer musicPlayer = player as MusicPlayer;
        if (musicPlayer != null)
        {
            musicPlayer.Play();
            musicPlayer.Stop();
        }

        VideoPlayer videoPlayer = player as VideoPlayer;
        if (videoPlayer != null)
        {
            videoPlayer.Pause(); // Этот код не выполнится
        }
        else
        {
            Console.WriteLine("Приведение к VideoPlayer не удалось");
        }
    }
}

// 89. Проверка типа объекта и вызов специфичного метода
class SpecificMethodDemo
{
    public static void CallSpecificMethods()
    {
        EmployeeUpcast emp = new DeveloperUpcast("Иван");

        if (emp is DeveloperUpcast dev)
        {
            dev.Debug(); // Специфичный метод разработчика
        }

        if (emp is ManagerUpcast manager)
        {
            manager.ConductMeeting(); // Этот код не выполнится
        }
    }
}

// 90. Сравнение работы is и as
class IsVsAsComparison
{
    public static void CompareIsAndAs()
    {
        AnimalUpcast animal = new DogUpcast();

        // Способ 1: is с приведением
        if (animal is DogUpcast)
        {
            DogUpcast dog1 = (DogUpcast)animal;
            dog1.Speak();
        }

        // Способ 2: is с объявлением переменной (C# 7.0+)
        if (animal is DogUpcast dog2)
        {
            dog2.Speak();
        }

        // Способ 3: as с проверкой на null
        DogUpcast dog3 = animal as DogUpcast;
        if (dog3 != null)
        {
            dog3.Speak();
        }

        Console.WriteLine("Все три способа работают одинаково в этом случае");
    }
}

// ==============================================
// РАЗДЕЛ 8: ГЕРМЕТИЗИРОВАННЫЕ КЛАССЫ (SEALED)
// ==============================================

// 91. Sealed класс и попытка наследования
sealed class SealedClass
{
    public string Message { get; set; } = "Я sealed класс";

    public void ShowMessage()
    {
        Console.WriteLine(Message);
    }
}

// Попытка наследования вызовет ошибку компиляции:
// class DerivedFromSealed : SealedClass { }

// 92. Sealed класс с закрытым методом
sealed class SecureLogger
{
    private string FormatMessage(string message)
    {
        return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
    }

    public void Log(string message)
    {
        string formatted = FormatMessage(message);
        Console.WriteLine(formatted);
    }
}

// 93. Sealed для предотвращения наследования
sealed class ConfigurationManager
{
    private static ConfigurationManager _instance;
    private Dictionary<string, string> _settings;

    private ConfigurationManager()
    {
        _settings = new Dictionary<string, string>();
    }

    public static ConfigurationManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ConfigurationManager();
            return _instance;
        }
    }

    public string GetSetting(string key)
    {
        return _settings.ContainsKey(key) ? _settings[key] : null;
    }

    public void SetSetting(string key, string value)
    {
        _settings[key] = value;
    }
}

// 94. Проверка невозможности override sealed метода
class BaseWithSealedMethod
{
    public virtual void Method1()
    {
        Console.WriteLine("Base.Method1");
    }

    public virtual void Method2()
    {
        Console.WriteLine("Base.Method2");
    }
}

class DerivedWithSealedOverride : BaseWithSealedMethod
{
    public sealed override void Method1()
    {
        Console.WriteLine("Derived.Method1 - sealed");
    }

    public override void Method2()
    {
        Console.WriteLine("Derived.Method2 - можно переопределить");
    }
}

class FurtherDerived : DerivedWithSealedOverride
{
    // Ошибка компиляции - нельзя переопределить sealed метод
    // public override void Method1() { }

    public override void Method2()
    {
        Console.WriteLine("FurtherDerived.Method2");
    }
}

// 95. Sealed класс с публичным методом
sealed class MathUtils
{
    public static double CalculateCircleArea(double radius)
    {
        return Math.PI * radius * radius;
    }

    public static double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
}

// 96. Sealed класс с ограничением на конструктор
sealed class SingletonService
{
    private static SingletonService _instance;

    private SingletonService()
    {
        Console.WriteLine("SingletonService создан");
    }

    public static SingletonService Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SingletonService();
            return _instance;
        }
    }

    public void DoWork()
    {
        Console.WriteLine("SingletonService выполняет работу");
    }
}

// 97. Sealed класс для логирования
sealed class FileLogger
{
    private string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void LogInfo(string message)
    {
        string logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        // В реальном приложении здесь была бы запись в файл
        Console.WriteLine(logEntry);
    }

    public void LogError(string message)
    {
        string logEntry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        // В реальном приложении здесь была бы запись в файл
        Console.WriteLine(logEntry);
    }
}

// 98. Sealed класс для конфиденциальных данных
sealed class SecureDataContainer
{
    private byte[] _encryptedData;

    public SecureDataContainer(string data)
    {
        _encryptedData = System.Text.Encoding.UTF8.GetBytes(data);
        // В реальном приложении здесь было бы настоящее шифрование
    }

    public string GetDecryptedData()
    {
        return System.Text.Encoding.UTF8.GetString(_encryptedData);
    }
}

// 99. Sealed класс с защищёнными свойствами
sealed class BankAccountSealed
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }
    public string Owner { get; }

    public BankAccountSealed(string accountNumber, string owner, decimal initialBalance = 0)
    {
        AccountNumber = accountNumber;
        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            Console.WriteLine($"Пополнение: +{amount:C}, Баланс: {Balance:C}");
        }
    }

    public bool Withdraw(decimal amount)
    {
        if (amount > 0 && Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Снятие: -{amount:C}, Баланс: {Balance:C}");
            return true;
        }
        return false;
    }
}

// 100. Sealed класс для контрольных объектов
sealed class AuditTrail
{
    private List<string> _auditLog;

    public AuditTrail()
    {
        _auditLog = new List<string>();
    }

    public void LogAction(string action, string user)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {user} - {action}";
        _auditLog.Add(logEntry);
        Console.WriteLine(logEntry);
    }

    public void DisplayAuditLog()
    {
        Console.WriteLine("\n=== АУДИТ ЛОГ ===");
        foreach (var entry in _auditLog)
        {
            Console.WriteLine(entry);
        }
    }
}

// ==============================================
// ГЛАВНАЯ ПРОГРАММА ДЛЯ ТЕСТИРОВАНИЯ
// ==============================================

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ТЕСТИРОВАНИЕ НАСЛЕДОВАНИЯ, ПОЛИМОРФИЗМА И ПРИВЕДЕНИЯ ТИПОВ ===\n");

        TestInheritance();
        TestAccessModifiers();
        TestBaseConstructorCalls();
        TestUpcast();
        TestDowncast();
        TestPolymorphism();
        TestIsAsOperators();
        TestSealedClasses();

        Console.WriteLine("\n=== ВСЕ ТЕСТЫ ЗАВЕРШЕНЫ УСПЕШНО! ===");
    }

    static void TestInheritance()
    {
        Console.WriteLine("1. ТЕСТИРОВАНИЕ НАСЛЕДОВАНИЯ:");

        // 1. Animal и Cat
        Cat cat = new Cat("Мурка", 3, "Сиамская");
        cat.DisplayInfo();
        cat.MakeSound();
        cat.ClimbTree();
        Console.WriteLine();

        // 2. Vehicle, Car и Bus
        Car car = new Car("Toyota", "Camry", 2022, 4);
        Bus bus = new Bus("Mercedes", "Sprinter", 2021, 20);

        car.Start();
        car.OpenTrunk();
        bus.Start();
        bus.AnnounceStop();
        Console.WriteLine();

        // 3. Shape, Circle и Rectangle
        Circle circle = new Circle("Красный", 5);
        Rectangle rectangle = new Rectangle("Синий", 4, 6);

        circle.DisplayInfo();
        rectangle.DisplayInfo();
        Console.WriteLine();

        // 4. Переопределение метода
        BaseClass baseObj = new BaseClass();
        DerivedClass derivedObj = new DerivedClass();

        baseObj.ShowMessage();
        derivedObj.ShowMessage();
        Console.WriteLine();

        // 5. Вызов конструктора базового класса
        Student student = new Student("Петр", 15, 8);
        Console.WriteLine();

        // 6. Иерархия сотрудников
        Manager manager = new Manager("Анна", 80000, "IT");
        Developer developer = new Developer("Иван", 60000, "C#");

        manager.DisplayInfo();
        manager.Work();
        manager.ConductMeeting();

        developer.DisplayInfo();
        developer.Work();
        developer.DebugCode();
        Console.WriteLine();
    }

    static void TestAccessModifiers()
    {
        Console.WriteLine("2. ТЕСТИРОВАНИЕ МОДИФИКАТОРОВ ДОСТУПА:");

        // 16. Приватное поле и публичное свойство
        AccessExample access = new AccessExample();
        access.PrivateField = 100;
        access.publicField = 200;
        access.ShowAllFields();

        // 17. Protected доступ из наследника
        DerivedAccess derived = new DerivedAccess();
        derived.AccessProtected();

        // 20. Публичное поле и изменение извне
        PublicFieldExample publicExample = new PublicFieldExample(10);
        publicExample.PublicValue = 50; // Можно изменить напрямую
        publicExample.ShowValues();

        // 21. Защита приватных данных
        SecureData secure = new SecureData("Секретная информация");
        Console.WriteLine(secure.GetSecretData());
        Console.WriteLine(secure.GetSecretData());

        Console.WriteLine();
    }

    static void TestBaseConstructorCalls()
    {
        Console.WriteLine("3. ТЕСТИРОВАНИЕ ВЫЗОВА КОНСТРУКТОРОВ БАЗОВОГО КЛАССА:");

        // 31. Вызов через base
        DerivedConstructor derived = new DerivedConstructor(10, 20);
        Console.WriteLine();

        // 32. Передача параметров базовому
        CarDerived car = new CarDerived("Tesla Model S");
        Console.WriteLine();

        // 33. Цепочка конструкторов
        Child child = new Child("Иванов", "Сергей", "Алексей");
        Console.WriteLine();

        // 35. Перегруженные конструкторы
        MultiConstructorDerived multi1 = new MultiConstructorDerived();
        MultiConstructorDerived multi2 = new MultiConstructorDerived("тест");
        MultiConstructorDerived multi3 = new MultiConstructorDerived("число", 5);
        Console.WriteLine();
    }

    static void TestUpcast()
    {
        Console.WriteLine("4. ТЕСТИРОВАНИЕ UPCAST (ПРИВЕДЕНИЕ К БАЗОВОМУ ТИПУ):");

        // 46. Upcast - присвоение производного объекта базовому типу
        DogUpcast dog = new DogUpcast();
        AnimalUpcast animal = dog; // Upcast

        animal.Speak(); // Вызовет метод Dog (полиморфизм)
        // animal.Fetch(); // Ошибка - метод Fetch недоступен через базовый тип
        Console.WriteLine();

        // 48. Upcast для коллекции базовых объектов
        List<ShapeUpcast> shapes = new List<ShapeUpcast>
        {
            new CircleUpcast(),
            new SquareUpcast(),
            new CircleUpcast()
        };

        foreach (var shape in shapes)
        {
            shape.Draw(); // Полиморфный вызов
        }
        Console.WriteLine();

        // 49. Передача производного объекта методу, принимающему базовый
        Processor processor = new Processor();
        processor.Process(new DogUpcast());
        Console.WriteLine();

        // 50. Массив базового типа с производными объектами
        EmployeeUpcast[] employees = new EmployeeUpcast[]
        {
            new ManagerUpcast("Алиса"),
            new DeveloperUpcast("Борис"),
            new ManagerUpcast("Виктор")
        };

        foreach (var emp in employees)
        {
            emp.Work(); // Полиморфный вызов
        }
        Console.WriteLine();
    }

    static void TestDowncast()
    {
        Console.WriteLine("5. ТЕСТИРОВАНИЕ DOWNCAST (ПРИВЕДЕНИЕ К ПРОИЗВОДНОМУ ТИПУ):");

        // 56. Преобразование через cast
        AnimalUpcast animal = new DogUpcast();
        DogUpcast dog = (DogUpcast)animal; // Downcast
        dog.Fetch(); // Теперь доступен специфичный метод
        Console.WriteLine();

        // 57. Оператор as для безопасного приведения
        AnimalUpcast animal2 = new DogUpcast();
        DogUpcast dog2 = animal2 as DogUpcast;
        if (dog2 != null)
        {
            Console.WriteLine("Приведение as успешно:");
            dog2.Speak();
        }
        Console.WriteLine();

        // 58. Проверка типа с оператором is
        object obj = new DogUpcast();
        if (obj is DogUpcast)
        {
            Console.WriteLine("obj является DogUpcast");
        }

        if (obj is DogUpcast dog3)
        {
            dog3.Speak();
        }
        Console.WriteLine();

        // 64. Явное приведение для downcast
        ShapeUpcast shape = new CircleUpcast();
        CircleUpcast circle = (CircleUpcast)shape;
        circle.Fill();
        Console.WriteLine();
    }

    static void TestPolymorphism()
    {
        Console.WriteLine("6. ТЕСТИРОВАНИЕ ПОЛИМОРФИЗМА:");

        // 66. Виртуальный метод и переопределение
        PolymorphicBase polyBase = new PolymorphicBase();
        PolymorphicBase polyDerived = new PolymorphicDerived();

        polyBase.Display();
        polyDerived.Display(); // Полиморфный вызов
        Console.WriteLine();

        // 68. Интерфейс для реализации полиморфизма
        List<IDrawable> drawables = new List<IDrawable>
        {
            new DrawableCircle { Color = "Красный" },
            new DrawableSquare { Color = "Синий" }
        };

        foreach (var drawable in drawables)
        {
            drawable.Draw(); // Полиморфный вызов
        }
        Console.WriteLine();

        // 69. Массив базового типа с производными объектами
        Animal[] animals = new Animal[]
        {
            new Cat("Васька", 2, "Дворовый"),
            new Dog("Рекс", 4, "Овчарка"),
            new Cat("Матроскин", 5, "Домашний")
        };

        foreach (var animal in animals)
        {
            animal.MakeSound(); // Полиморфный вызов
        }
        Console.WriteLine();

        // 70. Несколько производных классов от одного базового
        Notification[] notifications = new Notification[]
        {
            new EmailNotification(),
            new SMSNotification(),
            new PushNotification()
        };

        foreach (var notification in notifications)
        {
            notification.Send("Тестовое сообщение");
        }
        Console.WriteLine();
    }

    static void TestIsAsOperators()
    {
        Console.WriteLine("7. ТЕСТИРОВАНИЕ ОПЕРАТОРОВ IS И AS:");

        // 81. Оператор is для проверки типа
        object testObj = new DogUpcast();
        Console.WriteLine($"testObj is DogUpcast: {testObj is DogUpcast}");
        Console.WriteLine($"testObj is CatUpcast: {testObj is CatUpcast}");
        Console.WriteLine();

        // 82. Оператор as для приведения типа
        AnimalUpcast animal = new DogUpcast();
        var asDog = animal as DogUpcast;
        var asCat = animal as CatUpcast;

        Console.WriteLine($"animal as DogUpcast: {asDog != null}");
        Console.WriteLine($"animal as CatUpcast: {asCat == null}");
        Console.WriteLine();

        // 89. Проверка типа и вызов специфичного метода
        EmployeeUpcast employee = new DeveloperUpcast("Сергей");

        if (employee is DeveloperUpcast dev)
        {
            dev.Debug();
        }
        Console.WriteLine();
    }

    static void TestSealedClasses()
    {
        Console.WriteLine("8. ТЕСТИРОВАНИЕ SEALED КЛАССОВ:");

        // 91. Sealed класс
        SealedClass sealedObj = new SealedClass();
        sealedObj.ShowMessage();

        // 95. Sealed класс с публичным методом
        double area = MathUtils.CalculateCircleArea(5);
        Console.WriteLine($"Площадь круга: {area:F2}");

        // 97. Sealed класс для логирования
        FileLogger logger = new FileLogger("log.txt");
        logger.LogInfo("Тестовое сообщение");
        logger.LogError("Тестовая ошибка");

        // 99. Sealed класс с защищёнными свойствами
        BankAccountSealed account = new BankAccountSealed("123456", "Иван Иванов", 1000);
        account.Deposit(500);
        account.Withdraw(200);

        // 100. Sealed класс для контрольных объектов
        AuditTrail audit = new AuditTrail();
        audit.LogAction("Вход в систему", "user1");
        audit.LogAction("Изменение настроек", "user1");
        audit.DisplayAuditLog();

        Console.WriteLine();
    }
}
