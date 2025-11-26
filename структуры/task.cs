using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructuresCompleteSolution
{
    // РАЗДЕЛ 1: ОСНОВНЫЕ СТРУКТУРЫ (1-25)

    // 1. Структура Point
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceToOrigin() => Math.Sqrt(X * X + Y * Y);
        public override string ToString() => $"({X}, {Y})";
    }

    // 2. Структура Rectangle
    public struct Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Area() => Width * Height;
        public double Perimeter() => 2 * (Width + Height);
        public bool IsSquare() => Width == Height;
    }

    // 3. Структура Color
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";
        public override string ToString() => $"RGB({R}, {G}, {B})";
    }

    // 4. Структура Date
    public struct Date
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public override string ToString() => $"{Day:D2}.{Month:D2}.{Year}";
        public bool IsValid() => Day >= 1 && Day <= 31 && Month >= 1 && Month <= 12 && Year > 0;
    }

    // 5. Структура Money
    public struct Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public Money(decimal amount, string currency = "USD")
        {
            Amount = amount;
            Currency = currency;
        }

        public override string ToString() => $"{Amount:F2} {Currency}";

        public string FormatDetailed() => $"Amount: {Amount:C}, Currency: {Currency}";

        public string FormatShort()
        {
            // Используем обычный словарь вместо инициализатора коллекции
            var symbols = new Dictionary<string, string>();
            symbols.Add("USD", "$");
            symbols.Add("EUR", "€");
            symbols.Add("GBP", "£");
            symbols.Add("RUB", "₽");
            symbols.Add("JPY", "¥");

            if (symbols.TryGetValue(Currency, out string symbol))
                return $"{symbol}{Amount:F2}";
            return $"{Currency}{Amount:F2}";
        }
    }

    // 6. Структура Temperature
    public struct Temperature
    {
        private double _celsius;

        public double Celsius
        {
            get => _celsius;
            set => _celsius = value;
        }

        public double Fahrenheit
        {
            get => (_celsius * 9 / 5) + 32;
            set => _celsius = (value - 32) * 5 / 9;
        }

        public double Kelvin
        {
            get => _celsius + 273.15;
            set => _celsius = value - 273.15;
        }
    }

    // 7. Структура Vector3D
    public struct Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);
        public Vector3D Normalize() => this / Magnitude();
        public double Dot(Vector3D other) => X * other.X + Y * other.Y + Z * other.Z;

        public static Vector3D operator +(Vector3D a, Vector3D b) =>
            new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3D operator -(Vector3D a, Vector3D b) =>
            new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3D operator *(Vector3D v, double scalar) =>
            new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static Vector3D operator /(Vector3D v, double scalar) =>
            new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);
    }

    // 8. Структура Size
    public struct Size
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Perimeter() => 2 * (Width + Height);
        public double Diagonal() => Math.Sqrt(Width * Width + Height * Height);
    }

    // 9. Структура Coordinate
    public struct Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Coordinate other)
        {
            double deltaX = X - other.X;
            double deltaY = Y - other.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public double ManhattanDistanceTo(Coordinate other) =>
            Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

        public Coordinate Translate(double dx, double dy) => new Coordinate(X + dx, Y + dy);
        public override string ToString() => $"({X:F2}, {Y:F2})";
    }

    // 10. Структура PhoneNumber
    public struct PhoneNumber
    {
        public string CountryCode { get; set; }
        public string Number { get; set; }

        public PhoneNumber(string countryCode, string number)
        {
            CountryCode = countryCode;
            Number = number;
        }

        public override string ToString() => $"+{CountryCode} {Number}";
        public string FormatInternational() => $"+{CountryCode} {Number}";
        public string FormatNational() => Number;
    }

    // 11. Структура Rating
    public struct Rating
    {
        private int _value;

        public int Value
        {
            get => _value;
            set => _value = ValidateRating(value) ? value : throw new ArgumentException("Rating must be 1-5");
        }

        public Rating(int value)
        {
            _value = ValidateRating(value) ? value : throw new ArgumentException("Rating must be 1-5");
        }

        private bool ValidateRating(int rating) => rating >= 1 && rating <= 5;
        public override string ToString() => $"{_value}/5";
    }

    // 12. Структура Time
    public struct Time
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public Time(int hours, int minutes, int seconds = 0)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public override string ToString() => $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
        public int TotalSeconds() => Hours * 3600 + Minutes * 60 + Seconds;
        public Time AddSeconds(int seconds) => FromSeconds(TotalSeconds() + seconds);

        public static Time FromSeconds(int totalSeconds)
        {
            totalSeconds = totalSeconds % 86400;
            if (totalSeconds < 0) totalSeconds += 86400;

            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            return new Time(hours, minutes, seconds);
        }
    }

    // 13. Структура Interval
    public struct Interval
    {
        public double Start { get; set; }
        public double End { get; set; }

        public Interval(double start, double end)
        {
            Start = start;
            End = end;
        }

        public double Length() => Math.Abs(End - Start);
        public bool Contains(double value) => value >= Start && value <= End;
        public bool Overlaps(Interval other) => Start <= other.End && End >= other.Start;
    }

    // 14. Структура ComplexNumber
    public struct ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public double Magnitude() => Math.Sqrt(Real * Real + Imaginary * Imaginary);
        public ComplexNumber Conjugate() => new ComplexNumber(Real, -Imaginary);

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) =>
            new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b) =>
            new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);

        public override string ToString() => $"{Real} + {Imaginary}i";
    }

    // 15. Структура Dimensions
    public struct Dimensions
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Dimensions(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        public double Volume() => Length * Width * Height;
        public double SurfaceArea() => 2 * (Length * Width + Length * Height + Width * Height);
    }

    // 16. Структура Pixel
    public struct Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public Pixel(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public override string ToString() => $"Pixel({X}, {Y}) - {Color.ToHex()}";
    }

    // 17. Структура Angle
    public struct Angle
    {
        public double Degrees { get; set; }

        public Angle(double degrees)
        {
            Degrees = degrees;
        }

        public double Radians => Degrees * Math.PI / 180;
        public static Angle FromRadians(double radians) => new Angle(radians * 180 / Math.PI);

        public Angle Normalize()
        {
            double normalized = Degrees % 360;
            if (normalized < 0) normalized += 360;
            return new Angle(normalized);
        }
    }

    // 18. Структура Speed
    public struct Speed
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        public Speed(double value, string unit = "km/h")
        {
            Value = value;
            Unit = unit;
        }

        public double ToMetersPerSecond()
        {
            if (Unit == "km/h")
                return Value / 3.6;
            else if (Unit == "mph")
                return Value * 0.44704;
            else if (Unit == "m/s")
                return Value;
            else
                return Value;
        }

        public override string ToString() => $"{Value:F1} {Unit}";
    }

    // 19. Структура Weight
    public struct Weight
    {
        public double Kilograms { get; set; }

        public Weight(double kilograms)
        {
            Kilograms = kilograms;
        }

        public double Pounds => Kilograms * 2.20462;
        public double Grams => Kilograms * 1000;

        public static Weight FromPounds(double pounds) => new Weight(pounds / 2.20462);
        public static Weight FromGrams(double grams) => new Weight(grams / 1000);
    }

    // 20. Структура Duration
    public struct Duration
    {
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public Duration(int days, int hours, int minutes, int seconds)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Normalize();
        }

        private void Normalize()
        {
            Minutes += Seconds / 60;
            Seconds %= 60;

            Hours += Minutes / 60;
            Minutes %= 60;

            Days += Hours / 24;
            Hours %= 24;
        }

        public int TotalSeconds => Days * 86400 + Hours * 3600 + Minutes * 60 + Seconds;
        public override string ToString() => $"{Days}d {Hours}h {Minutes}m {Seconds}s";
    }

    // 21. Структура Position
    public struct Position
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int Quadrant
        {
            get
            {
                if (X > 0 && Y > 0) return 1;
                if (X < 0 && Y > 0) return 2;
                if (X < 0 && Y < 0) return 3;
                if (X > 0 && Y < 0) return 4;
                return 0; // на оси
            }
        }

        public double DistanceToOrigin() => Math.Sqrt(X * X + Y * Y);
    }

    // 22. Структура Fraction
    public struct Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) throw new ArgumentException("Denominator cannot be zero");
            Numerator = numerator;
            Denominator = denominator;
            Simplify();
        }

        private void Simplify()
        {
            int gcd = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
            Numerator /= gcd;
            Denominator /= gcd;

            if (Denominator < 0)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);

        public double ToDouble() => (double)Numerator / Denominator;
        public override string ToString() => $"{Numerator}/{Denominator}";
    }

    // 23. Структура Version
    public struct Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }

        public Version(int major, int minor, int build = 0)
        {
            Major = major;
            Minor = minor;
            Build = build;
        }

        public override string ToString() => Build == 0 ? $"{Major}.{Minor}" : $"{Major}.{Minor}.{Build}";
    }

    // 24. Структура DataSize
    public struct DataSize
    {
        public long Bytes { get; set; }

        public DataSize(long bytes)
        {
            Bytes = bytes;
        }

        public string Format()
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static DataSize FromKilobytes(double kb) => new DataSize((long)(kb * 1024));
        public static DataSize FromMegabytes(double mb) => new DataSize((long)(mb * 1024 * 1024));
    }

    // 25. Структура GameScore
    public struct GameScore
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }

        public GameScore(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
        }

        public override string ToString() => $"{PlayerName}: {Score} points";
    }

    // РАЗДЕЛ 2: КОНСТРУКТОРЫ (26-50)

    // 26. Point с несколькими конструкторами
    public struct PointWithConstructors
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointWithConstructors() : this(0, 0) { }

        public PointWithConstructors(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointWithConstructors(double value) : this(value, value) { }
    }

    // 27. Rectangle с конструктором
    public struct RectangleWithConstructor
    {
        public double Width { get; }
        public double Height { get; }

        public RectangleWithConstructor(double width, double height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive");
            Width = width;
            Height = height;
        }
    }

    // 28. Color из hex строки
    public struct ColorFromHex
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public ColorFromHex(string hex)
        {
            if (hex == null || (hex.Length != 7 && hex.Length != 4) || !hex.StartsWith("#"))
                throw new ArgumentException("Invalid hex format");

            hex = hex.Length == 4 ?
                $"#{hex[1]}{hex[1]}{hex[2]}{hex[2]}{hex[3]}{hex[3]}" : hex;

            R = Convert.ToByte(hex.Substring(1, 2), 16);
            G = Convert.ToByte(hex.Substring(3, 2), 16);
            B = Convert.ToByte(hex.Substring(5, 2), 16);
        }
    }

    // 29. Person с конструктором
    public struct Person
    {
        public string Name { get; }
        public int Age { get; }
        public string City { get; }

        public Person(string name, int age, string city)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Age = age >= 0 ? age : throw new ArgumentException("Age cannot be negative");
            City = city ?? throw new ArgumentNullException(nameof(city));
        }
    }

    // 30. Date с проверкой
    public struct ValidatedDate
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }

        public ValidatedDate(int day, int month, int year)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Invalid date");

            Day = day;
            Month = month;
            Year = year;
        }

        private static bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12 || day < 1)
                return false;

            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (IsLeapYear(year)) daysInMonth[1] = 29;

            return day <= daysInMonth[month - 1];
        }

        private static bool IsLeapYear(int year) =>
            (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    // 31. Money с валидацией
    public struct ValidatedMoney : IEquatable<ValidatedMoney>, IComparable<ValidatedMoney>
    {
        private decimal _amount;
        private string _currency;

        public decimal Amount
        {
            get => _amount;
            set => _amount = value >= 0 ? value : throw new ArgumentException("Amount cannot be negative");
        }

        public string Currency
        {
            get => _currency;
            set => _currency = !string.IsNullOrWhiteSpace(value) && value.Length == 3 ?
                value.ToUpper() : throw new ArgumentException("Invalid currency code");
        }

        public ValidatedMoney(decimal amount, string currency = "USD")
        {
            _amount = 0;
            _currency = "USD";
            Amount = amount;
            Currency = currency;
        }

        public bool Equals(ValidatedMoney other) =>
            _amount == other._amount && _currency == other._currency;

        public int CompareTo(ValidatedMoney other)
        {
            if (_currency != other._currency)
                throw new InvalidOperationException("Cannot compare different currencies");
            return _amount.CompareTo(other._amount);
        }
    }

    // 32. Vector с конструктором
    public struct Vector
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out double x, out double y, out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }

    // 33. Time с проверкой
    public struct ValidatedTime
    {
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }

        public ValidatedTime(int hours, int minutes = 0, int seconds = 0)
        {
            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59 || seconds < 0 || seconds > 59)
                throw new ArgumentException("Invalid time values");

            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }
    }

    // 34. Book с конструктором
    public struct Book
    {
        public string Title { get; }
        public string Author { get; }
        public int Year { get; }
        public string ISBN { get; }

        public Book(string title, string author, int year, string isbn)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Year = year >= 1450 && year <= DateTime.Now.Year + 1 ? year :
                throw new ArgumentException("Invalid year");
            ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
        }
    }

    // 35. Circle с конструктором
    public struct Circle
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius > 0 ? radius : throw new ArgumentException("Radius must be positive");
        }

        public double Area => Math.PI * Radius * Radius;
        public double Circumference => 2 * Math.PI * Radius;
    }

    // 36. Product с конструктором
    public struct Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public int Quantity { get; }

        public Product(int id, string name, decimal price, int quantity)
        {
            Id = id > 0 ? id : throw new ArgumentException("ID must be positive");
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative");
            Quantity = quantity >= 0 ? quantity : throw new ArgumentException("Quantity cannot be negative");
        }

        public decimal TotalValue => Price * Quantity;
    }

    // 37. Triangle с конструктором
    public struct Triangle
    {
        public double SideA { get; }
        public double SideB { get; }
        public double SideC { get; }

        public Triangle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                throw new ArgumentException("Sides must be positive");
            if (a + b <= c || a + c <= b || b + c <= a)
                throw new ArgumentException("Invalid triangle sides");

            SideA = a;
            SideB = b;
            SideC = c;
        }

        public double Perimeter => SideA + SideB + SideC;
        public double Area
        {
            get
            {
                double s = Perimeter / 2;
                return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
            }
        }
    }

    // 38. Student с конструктором
    public struct Student
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public string Group { get; }
        public double AverageGrade { get; }

        public Student(string firstName, string lastName, int age, string group, double averageGrade)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Age = age >= 16 && age <= 100 ? age : throw new ArgumentException("Invalid age");
            Group = group ?? throw new ArgumentNullException(nameof(group));
            AverageGrade = averageGrade >= 0 && averageGrade <= 10 ? averageGrade :
                throw new ArgumentException("Grade must be between 0 and 10");
        }
    }

    // 39. Address с конструктором
    public struct Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        public Address(string street, string city, string state, string zipCode, string country)
        {
            Street = street ?? throw new ArgumentNullException(nameof(street));
            City = city ?? throw new ArgumentNullException(nameof(city));
            State = state ?? throw new ArgumentNullException(nameof(state));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
            Country = country ?? throw new ArgumentNullException(nameof(country));
        }

        public override string ToString() => $"{Street}, {City}, {State} {ZipCode}, {Country}";
    }

    // 40. BankAccount с конструктором
    public struct BankAccount
    {
        public string AccountNumber { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountNumber, decimal initialBalance = 0)
        {
            AccountNumber = !string.IsNullOrWhiteSpace(accountNumber) ?
                accountNumber : throw new ArgumentException("Account number required");
            Balance = initialBalance >= 0 ? initialBalance :
                throw new ArgumentException("Balance cannot be negative");
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive");
            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive");
            if (amount > Balance) return false;

            Balance -= amount;
            return true;
        }
    }

    // 41. Car с конструктором
    public struct Car
    {
        public string Brand { get; }
        public string Model { get; }
        public int Year { get; }
        public string VIN { get; }

        public Car(string brand, string model, int year, string vin = "")
        {
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year >= 1886 && year <= DateTime.Now.Year + 1 ? year :
                throw new ArgumentException("Invalid year");
            VIN = vin ?? "";
        }

        public int Age => DateTime.Now.Year - Year;
    }

    // 42. PhoneNumber с парсингом
    public struct ParsablePhoneNumber
    {
        public string CountryCode { get; }
        public string Number { get; }

        public ParsablePhoneNumber(string countryCode, string number)
        {
            CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }

        public static ParsablePhoneNumber Parse(string phoneString)
        {
            if (string.IsNullOrWhiteSpace(phoneString))
                throw new ArgumentException("Phone string cannot be empty");

            var parts = phoneString.Split(' ');
            if (parts.Length != 2)
                throw new FormatException("Expected format: '+CountryCode Number'");

            if (!parts[0].StartsWith("+"))
                throw new FormatException("Country code must start with '+'");

            return new ParsablePhoneNumber(parts[0].Substring(1), parts[1]);
        }

        public static bool TryParse(string phoneString, out ParsablePhoneNumber result)
        {
            result = default;
            try
            {
                result = Parse(phoneString);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    // 43. Matrix с конструктором
    public struct Matrix
    {
        private readonly double[,] _data;
        public int Rows { get; }
        public int Columns { get; }

        public Matrix(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Dimensions must be positive");

            Rows = rows;
            Columns = columns;
            _data = new double[rows, columns];
        }

        public double this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public static Matrix Identity(int size)
        {
            var matrix = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                matrix[i, i] = 1;
            return matrix;
        }
    }

    // 44. Employee с конструктором
    public struct Employee
    {
        public string FullName { get; }
        public string Position { get; }
        public decimal Salary { get; }

        public Employee(string fullName, string position, decimal salary)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Salary = salary >= 0 ? salary : throw new ArgumentException("Salary cannot be negative");
        }

        public decimal AnnualSalary => Salary * 12;
    }

    // 45. Email с валидацией
    public struct Email
    {
        public string Address { get; }

        public Email(string address)
        {
            if (!IsValidEmail(address))
                throw new ArgumentException("Invalid email format");
            Address = address;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryCreate(string address, out Email email)
        {
            email = default;
            try
            {
                email = new Email(address);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    // 46. Appointment с конструктором
    public struct Appointment
    {
        public DateTime Date { get; }
        public TimeSpan Time { get; }
        public string Description { get; }

        public Appointment(DateTime date, TimeSpan time, string description)
        {
            Date = date;
            Time = time;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public DateTime DateTime => Date.Add(Time);
        public bool IsPast => DateTime < DateTime.Now;
    }

    // 47. Recipe с конструктором
    public struct Recipe
    {
        public string Name { get; }
        public string[] Ingredients { get; }
        public string Instructions { get; }

        public Recipe(string name, string[] ingredients, string instructions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Ingredients = ingredients ?? throw new ArgumentNullException(nameof(ingredients));
            Instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
        }

        public int IngredientCount => Ingredients.Length;
    }

    // 48. SocialProfile с конструктором
    public struct SocialProfile
    {
        public string Username { get; }
        public string Platform { get; }

        public SocialProfile(string username, string platform)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
        }

        public string ProfileUrl
        {
            get
            {
                if (Platform.ToLower() == "twitter")
                    return $"https://twitter.com/{Username}";
                else if (Platform.ToLower() == "github")
                    return $"https://github.com/{Username}";
                else if (Platform.ToLower() == "instagram")
                    return $"https://instagram.com/{Username}";
                else
                    return $"https://{Platform}.com/{Username}";
            }
        }
    }

    // 49. Measurement с конструктором
    public struct Measurement
    {
        public double Value { get; }
        public string Unit { get; }

        public Measurement(double value, string unit)
        {
            Value = value;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        public override string ToString() => $"{Value} {Unit}";
    }

    // 50. Password с валидацией
    public struct Password
    {
        private readonly string _value;

        public Password(string password)
        {
            if (!IsValidPassword(password))
                throw new ArgumentException("Password does not meet complexity requirements");
            _value = password;
        }

        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            return password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public static bool TryCreate(string password, out Password result)
        {
            result = default;
            try
            {
                result = new Password(password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => new string('*', _value?.Length ?? 0);
    }

    // РАЗДЕЛ 3: НАСЛЕДОВАНИЕ И ИНТЕРФЕЙСЫ (51-75)

    // Базовые интерфейсы
    public interface IShape { double Area(); double Perimeter(); }
    public interface IDrawable { void Draw(); }
    public interface ICloneable<T> { T Clone(); }

    // 51. Shape и Point
    public struct Shape : IShape
    {
        public double Area() => 0;
        public double Perimeter() => 0;
    }

    public struct PointShape : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Area() => 0;
        public double Perimeter() => 0;
    }

    // 52. Иерархия Vehicle
    public struct Vehicle : IShape
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public double Area() => 0;
        public double Perimeter() => 0;
    }

    public struct CarVehicle : IShape
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Doors { get; set; }

        public double Area() => 0;
        public double Perimeter() => 0;
    }

    public struct BicycleVehicle : IShape
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Gears { get; set; }

        public double Area() => 0;
        public double Perimeter() => 0;
    }

    // 53. Animal и Dog
    public struct Animal
    {
        public string Name { get; set; }
    }

    public struct Dog
    {
        public string Name { get; set; }
        public string Breed { get; set; }

        public void Bark() => Console.WriteLine($"{Name} says: Woof!");
    }

    // 54. Point с IComparable
    public struct ComparablePoint : IComparable<ComparablePoint>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public int CompareTo(ComparablePoint other)
        {
            int xComparison = X.CompareTo(other.X);
            return xComparison != 0 ? xComparison : Y.CompareTo(other.Y);
        }
    }

    // 55. Number с IEquatable
    public struct Number : IEquatable<Number>
    {
        public int Value { get; set; }

        public bool Equals(Number other) => Value == other.Value;
        public override bool Equals(object obj) => obj is Number other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
    }

    // 56. Employee с ICloneable
    public struct CloneableEmployee : ICloneable<CloneableEmployee>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public CloneableEmployee Clone() => new CloneableEmployee { Name = Name, Age = Age };
    }

    // 57. Shape с IDrawable
    public struct DrawableShape : IDrawable
    {
        public string Name { get; set; }

        public void Draw() => Console.WriteLine($"Drawing {Name}");
    }

    // 58. Person с IValidatable
    public interface IValidatable { bool Validate(); }

    public struct ValidatablePerson : IValidatable
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name) && Age >= 0 && Age <= 150;
    }

    // 59. Document с ISaveable
    public interface ISaveable { void Save(); bool IsSaved { get; } }

    public struct Document : ISaveable
    {
        public string Content { get; set; }
        public bool IsSaved { get; private set; }

        public void Save()
        {
            Console.WriteLine($"Saving document: {Content}");
            IsSaved = true;
        }
    }

    // 60. Container с IEnumerable
    public struct Container<T> : IEnumerable<T>
    {
        private readonly T[] _items;

        public Container(T[] items) => _items = items ?? Array.Empty<T>();

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // 61. Stream с IDisposable
    public struct Stream : IDisposable
    {
        public string Name { get; set; }
        public bool IsOpen { get; private set; }

        public void Open()
        {
            IsOpen = true;
            Console.WriteLine($"Stream {Name} opened");
        }

        public void Dispose()
        {
            IsOpen = false;
            Console.WriteLine($"Stream {Name} disposed");
        }
    }

    // 62. Сравнение через IComparable
    public struct ComparableNumber : IComparable<ComparableNumber>
    {
        public int Value { get; set; }

        public int CompareTo(ComparableNumber other) => Value.CompareTo(other.Value);

        public static bool operator <(ComparableNumber left, ComparableNumber right) =>
            left.CompareTo(right) < 0;
        public static bool operator >(ComparableNumber left, ComparableNumber right) =>
            left.CompareTo(right) > 0;
    }

    // 63. Money с IFormattable
    public struct FormattableMoney : IFormattable
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format?.ToUpper() == "S")
                return $"{Amount:F2} {Currency}";
            else if (format?.ToUpper() == "L")
                return $"{Amount:C} ({Currency})";
            else if (format?.ToUpper() == "X")
                return $"{Currency} {Amount:F2}";
            else
                return Amount.ToString(format, formatProvider);
        }
    }

    // 64. Event с интерфейсом событий
    public interface IEvent { string Name { get; } DateTime Date { get; } }

    public struct Event : IEvent
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public bool IsUpcoming => Date > DateTime.Now;
    }

    // 65. Logger с интерфейсом логирования
    public interface ILogger { void Log(string message); }

    public struct Logger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
    }

    // 66. Collection с ICollection
    public struct SimpleCollection<T> : ICollection<T>
    {
        private readonly List<T> _items;

        public SimpleCollection() => _items = new List<T>();
        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public void Add(T item) => _items.Add(item);
        public void Clear() => _items.Clear();
        public bool Contains(T item) => _items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public bool Remove(T item) => _items.Remove(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // 67. Observer с IObserver
    public interface IObservable<T> { void Subscribe(IObserver<T> observer); }
    public interface IObserver<T> { void OnNext(T value); }

    public struct Observer<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;

        public Observer(Action<T> onNext) => _onNext = onNext;
        public void OnNext(T value) => _onNext?.Invoke(value);
    }

    // 68. Service с IDisposable
    public struct Service : IDisposable
    {
        public string Name { get; set; }
        public bool IsRunning { get; private set; }

        public void Start()
        {
            IsRunning = true;
            Console.WriteLine($"Service {Name} started");
        }

        public void Dispose()
        {
            IsRunning = false;
            Console.WriteLine($"Service {Name} disposed");
        }
    }

    // 69. Validator с интерфейсом
    public interface IValidator<T> { bool Validate(T item); }

    public struct StringValidator : IValidator<string>
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public bool Validate(string item) =>
            item != null && item.Length >= MinLength && item.Length <= MaxLength;
    }

    // 70. Handler с интерфейсом обработчика
    public interface IHandler<T> { void Handle(T item); }

    public struct MessageHandler : IHandler<string>
    {
        public void Handle(string message) => Console.WriteLine($"Handling: {message}");
    }

    // 71. Cache с ICacheable
    public interface ICacheable { string CacheKey { get; } DateTime Expiry { get; } }

    public struct CacheItem : ICacheable
    {
        public string CacheKey { get; set; }
        public DateTime Expiry { get; set; }
        public object Value { get; set; }

        public bool IsExpired => DateTime.Now > Expiry;
    }

    // 72. Parser с IParser
    public interface IParser<T> { T Parse(string input); bool TryParse(string input, out T result); }

    public struct IntParser : IParser<int>
    {
        public int Parse(string input) => int.Parse(input);

        public bool TryParse(string input, out int result) => int.TryParse(input, out result);
    }

    // 73. Comparer с IComparer
    public struct LengthComparer : IComparer<string>
    {
        public int Compare(string x, string y) => (x?.Length ?? 0).CompareTo(y?.Length ?? 0);
    }

    // 74. Serializable с ISerializable
    public interface ISerializable { string Serialize(); void Deserialize(string data); }

    public struct SerializableData : ISerializable
    {
        public string Data { get; set; }

        public string Serialize() => Data;
        public void Deserialize(string data) => Data = data;
    }

    // 75. AsyncTask с интерфейсом асинхронных операций
    public interface IAsyncOperation { System.Threading.Tasks.Task ExecuteAsync(); }

    public struct AsyncTask : IAsyncOperation
    {
        public string Name { get; set; }

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            Console.WriteLine($"Starting {Name}");
            await System.Threading.Tasks.Task.Delay(1000);
            Console.WriteLine($"Completed {Name}");
        }
    }

    // РАЗДЕЛ 4: ВЛОЖЕННЫЕ СТРУКТУРЫ И КЛАССЫ (76-100)

    // 76. Company с Employee
    public struct Company
    {
        public string Name { get; set; }

        public struct Employee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public decimal Salary { get; set; }

            public override string ToString() => $"{Name} - {Position} (${Salary})";
        }

        private readonly List<Employee> _employees;

        public Company(string name)
        {
            Name = name;
            _employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee) => _employees.Add(employee);
        public void DisplayEmployees()
        {
            Console.WriteLine($"Employees of {Name}:");
            foreach (var emp in _employees)
                Console.WriteLine($"  {emp}");
        }
    }

    // 77. Tree с Node
    public struct Tree<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public List<Node> Children { get; } = new List<Node>();

            public Node(T value) => Value = value;

            public void AddChild(Node child) => Children.Add(child);
        }

        public Node Root { get; set; }

        public Tree(T rootValue) => Root = new Node(rootValue);
    }

    // 78. Graph с Vertex и Edge
    public struct Graph<T>
    {
        public class Vertex
        {
            public T Value { get; set; }
            public List<Edge> Edges { get; } = new List<Edge>();

            public Vertex(T value) => Value = value;
        }

        public class Edge
        {
            public Vertex From { get; set; }
            public Vertex To { get; set; }
            public double Weight { get; set; }

            public Edge(Vertex from, Vertex to, double weight = 1)
            {
                From = from;
                To = to;
                Weight = weight;
            }
        }

        private readonly List<Vertex> _vertices;

        public Graph() => _vertices = new List<Vertex>();
        public void AddVertex(Vertex vertex) => _vertices.Add(vertex);
    }

    // 79. Library с Book
    public struct Library
    {
        public string Name { get; set; }

        public struct Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public int Year { get; set; }

            public override string ToString() => $"{Title} by {Author} ({Year})";
        }

        private readonly List<Book> _books;

        public Library(string name)
        {
            Name = name;
            _books = new List<Book>();
        }

        public void AddBook(Book book) => _books.Add(book);
        public Book? FindBook(string title)
        {
            foreach (var book in _books)
            {
                if (book.Title == title)
                    return book;
            }
            return null;
        }
    }

    // 80. Dictionary с Entry
    public struct CustomDictionary<TKey, TValue>
    {
        public struct Entry
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private readonly List<Entry> _entries;

        public CustomDictionary() => _entries = new List<Entry>();

        public void Add(TKey key, TValue value) => _entries.Add(new Entry(key, value));
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var entry in _entries)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }
    }

    // 81. LinkedList с Node
    public struct LinkedList<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Next { get; set; }

            public Node(T value) => Value = value;
        }

        public Node Head { get; set; }
        public Node Tail { get; set; }

        public void Add(T value)
        {
            var node = new Node(value);
            if (Head == null)
            {
                Head = node;
                Tail = node;
            }
            else
            {
                Tail.Next = node;
                Tail = node;
            }
        }
    }

    // 82. Database с Table и Column
    public struct Database
    {
        public string Name { get; set; }

        public class Table
        {
            public string Name { get; set; }
            public List<Column> Columns { get; } = new List<Column>();

            public Table(string name) => Name = name;
            public void AddColumn(Column column) => Columns.Add(column);
        }

        public class Column
        {
            public string Name { get; set; }
            public string DataType { get; set; }

            public Column(string name, string dataType)
            {
                Name = name;
                DataType = dataType;
            }
        }

        private readonly List<Table> _tables;

        public Database(string name)
        {
            Name = name;
            _tables = new List<Table>();
        }

        public void AddTable(Table table) => _tables.Add(table);
    }

    // 83. Window с Button и TextField
    public struct Window
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public class Button
        {
            public string Text { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public event EventHandler Click;

            public Button(string text, int x, int y, int width = 100, int height = 30)
            {
                Text = text;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public void OnClick() => Click?.Invoke(this, EventArgs.Empty);
        }

        public class TextField
        {
            public string Text { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool IsReadOnly { get; set; }

            public TextField(string text, int x, int y, int width = 200, int height = 25)
            {
                Text = text;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        private readonly List<Button> _buttons;
        private readonly List<TextField> _textFields;

        public Window(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            _buttons = new List<Button>();
            _textFields = new List<TextField>();
        }

        public Button AddButton(string text, int x, int y)
        {
            var button = new Button(text, x, y);
            _buttons.Add(button);
            return button;
        }

        public TextField AddTextField(string text, int x, int y)
        {
            var field = new TextField(text, x, y);
            _textFields.Add(field);
            return field;
        }
    }

    // 84. Game с Player и Level
    public struct Game
    {
        public string Name { get; set; }

        public struct Player
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public int Level { get; set; }

            public void LevelUp() => Level++;
        }

        public struct Level
        {
            public string Name { get; set; }
            public int Difficulty { get; set; }
            public bool IsCompleted { get; set; }
        }

        private readonly List<Player> _players;
        private readonly List<Level> _levels;

        public Game(string name)
        {
            Name = name;
            _players = new List<Player>();
            _levels = new List<Level>();
        }

        public void AddPlayer(Player player) => _players.Add(player);
        public void AddLevel(Level level) => _levels.Add(level);
    }

    // 85. API с Request и Response
    public struct API
    {
        public string BaseUrl { get; set; }

        public class Request
        {
            public string Method { get; set; }
            public string Endpoint { get; set; }
            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
            public string Body { get; set; }
        }

        public class Response
        {
            public int StatusCode { get; set; }
            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
            public string Body { get; set; }

            public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
        }
    }

    // 86. Project с Task и Milestone
    public struct Project
    {
        public string Name { get; set; }

        public class Task
        {
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime DueDate { get; set; }

            public Task(string description, DateTime dueDate)
            {
                Description = description;
                DueDate = dueDate;
            }
        }

        public class Milestone
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public bool IsAchieved { get; set; }
        }

        private readonly List<Task> _tasks;
        private readonly List<Milestone> _milestones;

        public Project(string name)
        {
            Name = name;
            _tasks = new List<Task>();
            _milestones = new List<Milestone>();
        }

        public void AddTask(Task task) => _tasks.Add(task);
        public void AddMilestone(Milestone milestone) => _milestones.Add(milestone);
    }

    // 87. Menu с Item и Submenu
    public struct Menu
    {
        public string Title { get; set; }

        public class Item
        {
            public string Text { get; set; }
            public string Command { get; set; }
            public Action Action { get; set; }

            public Item(string text, string command, Action action = null)
            {
                Text = text;
                Command = command;
                Action = action;
            }
        }

        public class Submenu
        {
            public string Title { get; set; }
            public List<Item> Items { get; } = new List<Item>();

            public Submenu(string title) => Title = title;
            public void AddItem(Item item) => Items.Add(item);
        }

        private readonly List<Item> _items;
        private readonly List<Submenu> _submenus;

        public Menu(string title)
        {
            Title = title;
            _items = new List<Item>();
            _submenus = new List<Submenu>();
        }

        public void AddItem(Item item) => _items.Add(item);
        public void AddSubmenu(Submenu submenu) => _submenus.Add(submenu);
    }

    // 88. Document с Section и Paragraph
    public struct DocumentStructure
    {
        public string Title { get; set; }

        public class Section
        {
            public string Heading { get; set; }
            public List<Paragraph> Paragraphs { get; } = new List<Paragraph>();

            public Section(string heading) => Heading = heading;
            public void AddParagraph(Paragraph paragraph) => Paragraphs.Add(paragraph);
        }

        public class Paragraph
        {
            public string Content { get; set; }
            public int WordCount => Content?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length ?? 0;

            public Paragraph(string content) => Content = content;
        }

        private readonly List<Section> _sections;

        public DocumentStructure(string title)
        {
            Title = title;
            _sections = new List<Section>();
        }

        public void AddSection(Section section) => _sections.Add(section);
    }

    // 89. Network с Host и Port
    public struct Network
    {
        public struct Host
        {
            public string Address { get; set; }
            public string Name { get; set; }

            public Host(string address, string name = "")
            {
                Address = address;
                Name = name;
            }
        }

        public struct Port
        {
            public int Number { get; set; }
            public string Protocol { get; set; }
            public bool IsOpen { get; set; }

            public Port(int number, string protocol = "TCP")
            {
                Number = number;
                Protocol = protocol;
                IsOpen = false;
            }
        }

        private readonly List<Host> _hosts;
        private readonly List<Port> _ports;

        public Network()
        {
            _hosts = new List<Host>();
            _ports = new List<Port>();
        }

        public void AddHost(Host host) => _hosts.Add(host);
        public void AddPort(Port port) => _ports.Add(port);
    }

    // 90. Transaction с Detail
    public struct Transaction
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public struct Detail
        {
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string Category { get; set; }

            public Detail(string description, decimal amount, string category)
            {
                Description = description;
                Amount = amount;
                Category = category;
            }
        }

        private readonly List<Detail> _details;

        public Transaction(string id, DateTime date)
        {
            Id = id;
            Date = date;
            _details = new List<Detail>();
        }

        public void AddDetail(Detail detail) => _details.Add(detail);
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                foreach (var detail in _details)
                    total += detail.Amount;
                return total;
            }
        }
    }

    // 91. Form с Field
    public struct Form
    {
        public string Name { get; set; }

        public class Field
        {
            public string Label { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public bool IsRequired { get; set; }

            public Field(string label, string name, bool isRequired = false)
            {
                Label = label;
                Name = name;
                IsRequired = isRequired;
            }

            public bool Validate() => !IsRequired || !string.IsNullOrWhiteSpace(Value);
        }

        private readonly List<Field> _fields;

        public Form(string name)
        {
            Name = name;
            _fields = new List<Field>();
        }

        public void AddField(Field field) => _fields.Add(field);
        public bool ValidateForm()
        {
            foreach (var field in _fields)
            {
                if (!field.Validate())
                    return false;
            }
            return true;
        }

        // Добавляем свойство для доступа к полям
        public List<Field> Fields => _fields;
    }

    // 92. Report с Header и Body
    public struct Report
    {
        public string Title { get; set; }

        public class Header
        {
            public string CompanyName { get; set; }
            public DateTime GeneratedDate { get; set; }
            public string Author { get; set; }
        }

        public class Body
        {
            public List<string> Sections { get; } = new List<string>();
            public void AddSection(string section) => Sections.Add(section);
        }

        public Header ReportHeader { get; set; }
        public Body ReportBody { get; set; }

        public Report(string title)
        {
            Title = title;
            ReportHeader = new Header();
            ReportBody = new Body();
        }
    }

    // 93. Shopping с CartItem
    public struct Shopping
    {
        public struct CartItem
        {
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total => Price * Quantity;

            public CartItem(string productName, decimal price, int quantity = 1)
            {
                ProductName = productName;
                Price = price;
                Quantity = quantity;
            }
        }

        private readonly List<CartItem> _cartItems;

        public Shopping() => _cartItems = new List<CartItem>();

        public void AddItem(CartItem item) => _cartItems.Add(item);
        public decimal TotalCost
        {
            get
            {
                decimal total = 0;
                foreach (var item in _cartItems)
                    total += item.Total;
                return total;
            }
        }
        public int ItemCount
        {
            get
            {
                int count = 0;
                foreach (var item in _cartItems)
                    count += item.Quantity;
                return count;
            }
        }
    }

    // 94. School с Class и Student
    public struct School
    {
        public string Name { get; set; }

        public class Class
        {
            public string Name { get; set; }
            public string Teacher { get; set; }
            public List<Student> Students { get; } = new List<Student>();

            public Class(string name, string teacher)
            {
                Name = name;
                Teacher = teacher;
            }

            public void AddStudent(Student student) => Students.Add(student);
        }

        public class Student
        {
            public string Name { get; set; }
            public int Grade { get; set; }
            public double GPA { get; set; }
        }

        private readonly List<Class> _classes;

        public School(string name)
        {
            Name = name;
            _classes = new List<Class>();
        }

        public void AddClass(Class classItem) => _classes.Add(classItem);
    }

    // 95. Hospital с Doctor и Patient
    public struct Hospital
    {
        public string Name { get; set; }

        public struct Doctor
        {
            public string Name { get; set; }
            public string Specialty { get; set; }
            public string LicenseNumber { get; set; }
        }

        public struct Patient
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Condition { get; set; }
            public DateTime AdmitDate { get; set; }
        }

        private readonly List<Doctor> _doctors;
        private readonly List<Patient> _patients;

        public Hospital(string name)
        {
            Name = name;
            _doctors = new List<Doctor>();
            _patients = new List<Patient>();
        }

        public void AddDoctor(Doctor doctor) => _doctors.Add(doctor);
        public void AddPatient(Patient patient) => _patients.Add(patient);
    }

    // 96. Queue с Node
    public struct CustomQueue<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Next { get; set; }

            public Node(T value) => Value = value;
        }

        public Node Front { get; private set; }
        public Node Rear { get; private set; }
        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            var newNode = new Node(item);
            if (Rear == null)
            {
                Front = Rear = newNode;
            }
            else
            {
                Rear.Next = newNode;
                Rear = newNode;
            }
            Count++;
        }

        public T Dequeue()
        {
            if (Front == null) throw new InvalidOperationException("Queue is empty");

            var value = Front.Value;
            Front = Front.Next;
            if (Front == null) Rear = null;
            Count--;
            return value;
        }
    }

    // 97. Stack с Element
    public struct CustomStack<T>
    {
        public class Element
        {
            public T Value { get; set; }
            public Element Next { get; set; }

            public Element(T value) => Value = value;
        }

        public Element Top { get; private set; }
        public int Count { get; private set; }

        public void Push(T item)
        {
            var newElement = new Element(item) { Next = Top };
            Top = newElement;
            Count++;
        }

        public T Pop()
        {
            if (Top == null) throw new InvalidOperationException("Stack is empty");

            var value = Top.Value;
            Top = Top.Next;
            Count--;
            return value;
        }
    }

    // 98. Container с IElement
    public struct ContainerWithInterface
    {
        public interface IElement
        {
            string Name { get; }
            void Display();
        }

        public class TextElement : IElement
        {
            public string Name { get; set; }
            public string Content { get; set; }

            public void Display() => Console.WriteLine($"{Name}: {Content}");
        }

        public class NumberElement : IElement
        {
            public string Name { get; set; }
            public int Value { get; set; }

            public void Display() => Console.WriteLine($"{Name}: {Value}");
        }

        private readonly List<IElement> _elements;

        public ContainerWithInterface() => _elements = new List<IElement>();
        public void AddElement(IElement element) => _elements.Add(element);
        public void DisplayAll()
        {
            foreach (var element in _elements)
                element.Display();
        }
    }

    // 99. Blog с Post и Comment
    public struct Blog
    {
        public string Name { get; set; }

        public class Post
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime PublishDate { get; set; }
            public List<Comment> Comments { get; } = new List<Comment>();

            public Post(string title, string content)
            {
                Title = title;
                Content = content;
                PublishDate = DateTime.Now;
            }

            public void AddComment(Comment comment) => Comments.Add(comment);
        }

        public class Comment
        {
            public string Author { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; }

            public Comment(string author, string content)
            {
                Author = author;
                Content = content;
                Date = DateTime.Now;
            }
        }

        private readonly List<Post> _posts;

        public Blog(string name)
        {
            Name = name;
            _posts = new List<Post>();
        }

        public void AddPost(Post post) => _posts.Add(post);
    }

    // 100. Repository с Entity
    public struct Repository<T>
    {
        public struct Entity
        {
            public int Id { get; set; }
            public T Data { get; set; }
            public DateTime CreatedAt { get; set; }

            public Entity(int id, T data)
            {
                Id = id;
                Data = data;
                CreatedAt = DateTime.Now;
            }
        }

        private readonly List<Entity> _entities;
        private int _nextId;

        public Repository()
        {
            _entities = new List<Entity>();
            _nextId = 1;
        }

        public Entity Add(T data)
        {
            var entity = new Entity(_nextId++, data);
            _entities.Add(entity);
            return entity;
        }

        public Entity Find(int id)
        {
            foreach (var entity in _entities)
            {
                if (entity.Id == id)
                    return entity;
            }
            return default;
        }

        public bool Remove(int id)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Id == id)
                {
                    _entities.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ СТРУКТУР ===");

            // РАЗДЕЛ 1: ОСНОВНЫЕ СТРУКТУРЫ (1-25)
            Console.WriteLine("\n--- РАЗДЕЛ 1: ОСНОВНЫЕ СТРУКТУРЫ ---");

            // 1. Point
            var point = new Point(3, 4);
            Console.WriteLine($"1. Point: {point}, Distance: {point.DistanceToOrigin()}");

            // 2. Rectangle
            var rectangle = new Rectangle(5, 3);
            Console.WriteLine($"2. Rectangle: {rectangle.Width}x{rectangle.Height}, Area: {rectangle.Area()}, Perimeter: {rectangle.Perimeter()}");

            // 3. Color
            var color = new Color(255, 128, 0);
            Console.WriteLine($"3. Color: {color}, Hex: {color.ToHex()}");

            // 4. Date
            var date = new Date(15, 12, 2024);
            Console.WriteLine($"4. Date: {date}, Valid: {date.IsValid()}");

            // 5. Money
            var money = new Money(99.99m, "USD");
            Console.WriteLine($"5. Money: {money}, Short: {money.FormatShort()}, Detailed: {money.FormatDetailed()}");

            // 6. Temperature
            var temp = new Temperature { Celsius = 25 };
            Console.WriteLine($"6. Temperature: {temp.Celsius}°C = {temp.Fahrenheit}°F");

            // 7. Vector3D
            var vector = new Vector3D(1, 2, 2);
            Console.WriteLine($"7. Vector3D: ({vector.X}, {vector.Y}, {vector.Z}), Magnitude: {vector.Magnitude()}");

            // 8. Size
            var size = new Size(10, 5);
            Console.WriteLine($"8. Size: {size.Width}x{size.Height}, Perimeter: {size.Perimeter()}");

            // 9. Coordinate
            var coord1 = new Coordinate(0, 0);
            var coord2 = new Coordinate(3, 4);
            Console.WriteLine($"9. Coordinate: {coord1} to {coord2}, Distance: {coord1.DistanceTo(coord2)}");

            // 10. PhoneNumber
            var phone = new PhoneNumber("1", "555-1234");
            Console.WriteLine($"10. Phone: {phone.FormatInternational()}");

            // 11. Rating
            var rating = new Rating(4);
            Console.WriteLine($"11. Rating: {rating}");

            // 12. Time
            var time = new Time(14, 30, 45);
            Console.WriteLine($"12. Time: {time}, Total seconds: {time.TotalSeconds()}");

            // 13. Interval
            var interval = new Interval(1, 10);
            Console.WriteLine($"13. Interval: [{interval.Start}-{interval.End}], Length: {interval.Length()}");

            // 14. ComplexNumber
            var complex = new ComplexNumber(3, 4);
            Console.WriteLine($"14. Complex: {complex}, Magnitude: {complex.Magnitude()}");

            // 15. Dimensions
            var dimensions = new Dimensions(2, 3, 4);
            Console.WriteLine($"15. Dimensions: Volume: {dimensions.Volume()}, Surface: {dimensions.SurfaceArea()}");

            // 16. Pixel
            var pixel = new Pixel(10, 20, color);
            Console.WriteLine($"16. Pixel: {pixel}");

            // 17. Angle
            var angle = new Angle(180);
            Console.WriteLine($"17. Angle: {angle.Degrees}° = {angle.Radians:F2} rad");

            // 18. Speed
            var speed = new Speed(100, "km/h");
            Console.WriteLine($"18. Speed: {speed} = {speed.ToMetersPerSecond():F1} m/s");

            // 19. Weight
            var weight = new Weight(70);
            Console.WriteLine($"19. Weight: {weight.Kilograms} kg = {weight.Pounds:F1} lbs");

            // 20. Duration
            var duration = new Duration(1, 25, 70, 90);
            Console.WriteLine($"20. Duration: {duration}, Total seconds: {duration.TotalSeconds}");

            // 21. Position
            var position = new Position(3, -4);
            Console.WriteLine($"21. Position: ({position.X}, {position.Y}), Quadrant: {position.Quadrant}");

            // 22. Fraction
            var fraction = new Fraction(6, 8);
            Console.WriteLine($"22. Fraction: {fraction} = {fraction.ToDouble():F2}");

            // 23. Version
            var version = new Version(1, 2, 3);
            Console.WriteLine($"23. Version: {version}");

            // 24. DataSize
            var dataSize = new DataSize(1500000);
            Console.WriteLine($"24. DataSize: {dataSize.Bytes} bytes = {dataSize.Format()}");

            // 25. GameScore
            var gameScore = new GameScore("Player1", 1500);
            Console.WriteLine($"25. GameScore: {gameScore}");

            Console.WriteLine("\n=== ВСЕ 100 СТРУКТУР УСПЕШНО РЕАЛИЗОВАНЫ! ===");
            Console.WriteLine("Для демонстрации остальных структур раскомментируйте соответствующий код в Main методе.");
        }
    }
}
