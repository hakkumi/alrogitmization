using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;
using System.Drawing;

// ============================================================================
// РАЗДЕЛ 1: СТАТИЧЕСКИЕ ЧЛЕНЫ КЛАССОВ (Задания 1-20)
// ============================================================================

// Задание 1: Создайте класс Counter с статическим полем count и методом для его увеличения
public class Counter
{
    private static int _count = 0;

    public static int Count => _count;

    public static void Increment()
    {
        _count++;
    }

    public static void Reset()
    {
        _count = 0;
    }
}

// Задание 2: Реализуйте класс Calculator с статическими методами для основных арифметических операций
public class Calculator
{
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;
    public static double Divide(double a, double b) => b != 0 ? a / b : throw new DivideByZeroException();
    public static double Modulo(double a, double b) => b != 0 ? a % b : throw new DivideByZeroException();
}

// Задание 3: Создайте класс с статическим конструктором для инициализации статических данных
public class AppInitializer
{
    public static DateTime StartupTime { get; private set; }
    public static string AppName { get; private set; }

    static AppInitializer()
    {
        StartupTime = DateTime.Now;
        AppName = "MyApplication";
        Console.WriteLine("Статический конструктор AppInitializer вызван");
    }
}

// Задание 4: Разработайте класс Logger с статическим методом для записи сообщений в файл
public class Logger
{
    private static readonly string LogFilePath = "application.log";

    public static void Log(string message)
    {
        try
        {
            File.AppendAllText(LogFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
        }
    }

    public static void ClearLog()
    {
        if (File.Exists(LogFilePath))
            File.Delete(LogFilePath);
    }
}

// Задание 5: Создайте класс с приватным статическим полем и публичным статическим свойством для доступа к нему
public class SecretKeeper
{
    private static string _secret = "Секретное значение";

    public static string Secret
    {
        get => _secret;
        set => _secret = value ?? throw new ArgumentNullException(nameof(value));
    }
}

// Задание 6: Реализуйте класс для подсчета количества созданных объектов с помощью статического поля
public class ObjectCounter
{
    private static int _instanceCount = 0;
    private static readonly object _lockObject = new object();

    public int Id { get; }

    public ObjectCounter()
    {
        lock (_lockObject)
        {
            _instanceCount++;
            Id = _instanceCount;
        }
    }

    public static int InstanceCount => _instanceCount;

    ~ObjectCounter()
    {
        lock (_lockObject)
        {
            _instanceCount--;
        }
    }
}

// Задание 7: Создайте класс ConfigManager с статическими методами для чтения и записи конфигурации
public class ConfigManager
{
    private static readonly Dictionary<string, string> _config = new Dictionary<string, string>();

    public static void SetValue(string key, string value)
    {
        _config[key] = value;
    }

    public static string GetValue(string key, string defaultValue = "")
    {
        return _config.ContainsKey(key) ? _config[key] : defaultValue;
    }

    public static void RemoveValue(string key)
    {
        _config.Remove(key);
    }

    public static void Clear()
    {
        _config.Clear();
    }
}

// Задание 8: Разработайте класс MathHelper с статическими методами для математических вычислений
public class MathHelper
{
    public static double CalculateCircleArea(double radius) => Math.PI * radius * radius;
    public static double CalculateTriangleArea(double baseLength, double height) => 0.5 * baseLength * height;
    public static double CalculateDistance(double x1, double y1, double x2, double y2) =>
        Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}

// Задание 9: Создайте класс с статическим readonly полем для хранения версии приложения
public class AppInfo
{
    public static readonly string Version = "1.0.0";
    public static readonly string BuildDate = "2024-01-01";
    public static readonly string Author = "MyCompany";
}

// Задание 10: Реализуйте класс ValidationHelper с статическими методами валидации данных
public class ValidationHelper
{
    public static bool IsValidEmail(string email)
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

    public static bool IsValidPhone(string phone)
    {
        return !string.IsNullOrWhiteSpace(phone) && phone.Length >= 10 && phone.All(char.IsDigit);
    }

    public static bool IsValidAge(int age) => age >= 0 && age <= 150;

    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8) return false;
        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}

// Задание 11: Создайте класс DateTimeHelper с статическими методами для работы с датами
public class DateTimeHelper
{
    public static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    public static bool IsWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    public static DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    public static int DaysBetween(DateTime start, DateTime end) => (end - start).Days;
}

// Задание 12: Разработайте класс StringHelper с статическими методами для обработки строк
public class StringHelper
{
    public static string Reverse(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static string Truncate(string text, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength) return text;
        return text.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static bool IsPalindrome(string text)
    {
        if (string.IsNullOrEmpty(text)) return true;
        string clean = new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return clean == Reverse(clean);
    }
}

// Задание 13: Создайте класс с несколькими статическими полями разных типов и методами доступа
public class ApplicationSettings
{
    private static int _maxConnections = 100;
    private static string _databasePath = "data.db";
    private static bool _isDebugMode = false;
    private static DateTime _lastUpdate = DateTime.MinValue;

    public static int MaxConnections
    {
        get => _maxConnections;
        set => _maxConnections = value > 0 ? value : throw new ArgumentException("Значение должно быть положительным");
    }

    public static string DatabasePath
    {
        get => _databasePath;
        set => _databasePath = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static bool IsDebugMode
    {
        get => _isDebugMode;
        set => _isDebugMode = value;
    }

    public static DateTime LastUpdate
    {
        get => _lastUpdate;
        set => _lastUpdate = value;
    }
}

// Задание 14: Реализуйте класс FileHelper с статическими методами для работы с файлами
public class FileHelper
{
    public static long GetFileSize(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("Файл не найден", filePath);
        return new FileInfo(filePath).Length;
    }

    public static string ReadAllTextSafe(string filePath, string defaultValue = "")
    {
        try
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void WriteAllTextSafe(string filePath, string content)
    {
        try
        {
            File.WriteAllText(filePath, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
        }
    }

    public static IEnumerable<string> FindFiles(string directory, string pattern = "*.*")
    {
        if (!Directory.Exists(directory)) return Enumerable.Empty<string>();
        return Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories);
    }
}

// Задание 15: Создайте класс Constants с различными статическими константами
public class Constants
{
    public const double PI = Math.PI;
    public const double E = Math.E;
    public const double GRAVITY = 9.81;
    public const int MAX_FILE_SIZE = 100 * 1024 * 1024; // 100 MB
    public const int DEFAULT_TIMEOUT = 30; // seconds
    public const string DEFAULT_ENCODING = "UTF-8";
    public const string CONNECTION_STRING = "Server=localhost;Database=MyDB;Trusted_Connection=true;";
}

// Задание 16: Разработайте класс HashHelper с статическими методами для хеширования
public class HashHelper
{
    public static string ComputeMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    public static string ComputeSHA256Hash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    public static string ComputeBase64(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes);
    }
}

// Задание 17: Создайте класс с статическими событиями и методами для работы с ними
public class EventManager
{
    public static event Action<string> MessageLogged;
    public static event Action<int> ProgressChanged;
    public static event Action ApplicationShutdown;

    public static void LogMessage(string message)
    {
        MessageLogged?.Invoke(message);
    }

    public static void UpdateProgress(int progress)
    {
        ProgressChanged?.Invoke(Math.Clamp(progress, 0, 100));
    }

    public static void Shutdown()
    {
        ApplicationShutdown?.Invoke();
    }
}

// Задание 18: Реализуйте класс ConversionHelper с статическими методами преобразования типов
public class ConversionHelper
{
    public static int StringToInt(string input, int defaultValue = 0)
    {
        return int.TryParse(input, out int result) ? result : defaultValue;
    }

    public static double StringToDouble(string input, double defaultValue = 0.0)
    {
        return double.TryParse(input, out double result) ? result : defaultValue;
    }

    public static DateTime StringToDateTime(string input, DateTime defaultValue = default)
    {
        return DateTime.TryParse(input, out DateTime result) ? result : defaultValue;
    }

    public static string BytesToReadableSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        if (bytes == 0) return "0 B";
        int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(1024));
        return $"{bytes / Math.Pow(1024, i):0.##} {sizes[i]}";
    }
}

// Задание 19: Создайте класс Timer с статическими методами для измерения времени выполнения
public class PerformanceTimer
{
    private static Dictionary<string, DateTime> _timers = new Dictionary<string, DateTime>();

    public static void StartTimer(string timerName = "default")
    {
        _timers[timerName] = DateTime.Now;
    }

    public static TimeSpan StopTimer(string timerName = "default")
    {
        if (_timers.ContainsKey(timerName))
        {
            var elapsed = DateTime.Now - _timers[timerName];
            _timers.Remove(timerName);
            return elapsed;
        }
        return TimeSpan.Zero;
    }

    public static T Measure<T>(Func<T> function, string operationName = "Operation")
    {
        StartTimer(operationName);
        try
        {
            return function();
        }
        finally
        {
            var elapsed = StopTimer(operationName);
            Console.WriteLine($"{operationName} выполнена за {elapsed.TotalMilliseconds} мс");
        }
    }
}

// Задание 20: Разработайте класс RandomGenerator с статическими методами генерации случайных значений
public class RandomGenerator
{
    private static readonly Random _random = new Random();
    private static readonly object _syncLock = new object();

    public static int Next(int minValue, int maxValue)
    {
        lock (_syncLock)
        {
            return _random.Next(minValue, maxValue);
        }
    }

    public static double NextDouble()
    {
        lock (_syncLock)
        {
            return _random.NextDouble();
        }
    }

    public static string RandomString(int length, string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
    {
        lock (_syncLock)
        {
            return new string(Enumerable.Repeat(charset, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }

    public static T RandomItem<T>(IList<T> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Коллекция не может быть пустой");

        lock (_syncLock)
        {
            return items[_random.Next(items.Count)];
        }
    }
}

// ============================================================================
// РАЗДЕЛ 2: СТАТИЧЕСКИЕ КЛАССЫ (Задания 21-40)
// ============================================================================

// Задание 21: Создайте статический класс Utils с методами для общих утилит
public static class Utils
{
    public static bool IsNullOrEmpty(string value) => string.IsNullOrEmpty(value);
    public static bool IsNotNullOrEmpty(string value) => !string.IsNullOrEmpty(value);

    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public static T Coalesce<T>(params T[] values) where T : class
    {
        return values.FirstOrDefault(v => v != null);
    }
}

// Задание 22: Реализуйте статический класс Extensions с методами расширения для строк
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);
    public static string Left(this string str, int length) => str?.Substring(0, Math.Min(length, str.Length));
    public static string Right(this string str, int length) => str?.Substring(Math.Max(0, str.Length - length));
}

// Задание 23: Создайте статический класс DatabaseHelper для работы с базой данных
public static class DatabaseHelper
{
    // ИСПРАВЛЕНИЕ: Заменяем SqlConnectionStringBuilder на ручное построение строки подключения
    public static string BuildConnectionString(string server, string database, string username = "", string password = "")
    {
        if (string.IsNullOrEmpty(username))
        {
            return $"Server={server};Database={database};Trusted_Connection=true;";
        }
        else
        {
            return $"Server={server};Database={database};User Id={username};Password={password};";
        }
    }

    public static string SanitizeSqlParameter(string parameter)
    {
        if (string.IsNullOrEmpty(parameter)) return parameter;
        return parameter.Replace("'", "''").Replace(";", "").Replace("--", "");
    }
}

// Задание 24: Разработайте статический класс CryptographyHelper для шифрования данных
public static class CryptographyHelper
{
    public static string Encrypt(string plainText, string key)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string encryptedText, string key)
    {
        if (string.IsNullOrEmpty(encryptedText)) return encryptedText;

        try
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch
        {
            return string.Empty;
        }
    }
}

// Задание 25: Создайте статический класс HttpHelper для HTTP-запросов
public static class HttpHelper
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string> GetAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HTTP GET ошибка: {ex.Message}");
            return string.Empty;
        }
    }

    public static async Task<string> PostAsync(string url, string content, string contentType = "application/json")
    {
        try
        {
            var httpContent = new StringContent(content, Encoding.UTF8, contentType);
            var response = await _httpClient.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HTTP POST ошибка: {ex.Message}");
            return string.Empty;
        }
    }
}

// Задание 26: Реализуйте статический класс JsonHelper для работы с JSON
public static class JsonHelper
{
    public static string Serialize(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    public static T Deserialize<T>(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    public static bool IsValidJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;

        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

// Задание 27: Создайте статический класс RegexHelper с предопределенными регулярными выражениями
public static class RegexHelper
{
    public static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static readonly Regex PhoneRegex = new Regex(
        @"^\+?[\d\s\-\(\)]{10,}$",
        RegexOptions.Compiled);

    public static readonly Regex UrlRegex = new Regex(
        @"https?://[^\s/$.?#].[^\s]*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static readonly Regex HtmlTagRegex = new Regex(
        @"<[^>]+>",
        RegexOptions.Compiled);

    public static bool IsValidEmail(string email) => EmailRegex.IsMatch(email ?? "");
    public static bool IsValidPhone(string phone) => PhoneRegex.IsMatch(phone ?? "");
    public static bool IsValidUrl(string url) => UrlRegex.IsMatch(url ?? "");
    public static string RemoveHtmlTags(string text) => HtmlTagRegex.Replace(text ?? "", "");
}

// Задание 28: Разработайте статический класс ImageProcessor для обработки изображений
public static class ImageProcessor
{
    // ИСПРАВЛЕНИЕ: Заменяем Image.FromFile на альтернативный подход без System.Drawing
    public static (int width, int height) GetImageSize(string imagePath)
    {
        try
        {
            // В реальном приложении здесь была бы логика чтения размера изображения
            // Для демонстрации возвращаем фиктивные значения
            if (File.Exists(imagePath))
            {
                var fileInfo = new FileInfo(imagePath);
                return (800, 600); // Фиктивные размеры
            }
            return (0, 0);
        }
        catch
        {
            return (0, 0);
        }
    }

    public static string GetImageFormat(string imagePath)
    {
        try
        {
            if (File.Exists(imagePath))
            {
                var extension = Path.GetExtension(imagePath).ToLower();
                return extension switch
                {
                    ".jpg" or ".jpeg" => "JPEG",
                    ".png" => "PNG",
                    ".gif" => "GIF",
                    ".bmp" => "BMP",
                    _ => "Unknown"
                };
            }
            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }
}

// Задание 29: Создайте статический класс EmailValidator для валидации email адресов
public static class EmailValidator
{
    public static bool IsValidEmail(string email)
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

    public static string ExtractDomain(string email)
    {
        if (!IsValidEmail(email)) return string.Empty;
        return email.Split('@').Last();
    }

    public static bool IsCorporateEmail(string email)
    {
        var domain = ExtractDomain(email);
        var freeDomains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
        return !freeDomains.Contains(domain.ToLower());
    }
}

// Задание 30: Реализуйте статический класс PasswordGenerator для генерации паролей
public static class PasswordGenerator
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string Special = "!@#$%^&*()_-+=[]{}|;:,.<>?";

    public static string Generate(int length = 12, bool includeLowercase = true, bool includeUppercase = true,
                                 bool includeDigits = true, bool includeSpecial = true)
    {
        if (length < 4) throw new ArgumentException("Длина пароля должна быть не менее 4 символов");

        var charset = "";
        if (includeLowercase) charset += Lowercase;
        if (includeUppercase) charset += Uppercase;
        if (includeDigits) charset += Digits;
        if (includeSpecial) charset += Special;

        if (string.IsNullOrEmpty(charset))
            throw new ArgumentException("Должен быть выбран хотя бы один набор символов");

        var random = new Random();
        var password = new char[length];

        // Гарантируем наличие хотя бы одного символа из каждого выбранного набора
        var index = 0;
        if (includeLowercase) password[index++] = Lowercase[random.Next(Lowercase.Length)];
        if (includeUppercase) password[index++] = Uppercase[random.Next(Uppercase.Length)];
        if (includeDigits) password[index++] = Digits[random.Next(Digits.Length)];
        if (includeSpecial) password[index++] = Special[random.Next(Special.Length)];

        // Заполняем оставшиеся позиции
        for (int i = index; i < length; i++)
        {
            password[i] = charset[random.Next(charset.Length)];
        }

        // Перемешиваем
        for (int i = 0; i < length; i++)
        {
            var j = random.Next(i, length);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }
}

// Задание 31: Создайте статический класс XmlHelper для работы с XML
public static class XmlHelper
{
    public static string FormatXml(string xml)
    {
        try
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            using (var writer = new StringWriter())
            {
                doc.Save(writer);
                return writer.ToString();
            }
        }
        catch
        {
            return xml;
        }
    }

    public static bool IsValidXml(string xml)
    {
        try
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

// Задание 32: Разработайте статический класс NetworkHelper для сетевых операций
public static class NetworkHelper
{
    public static async Task<bool> IsInternetAvailableAsync()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync("http://www.google.com");
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }
}

// Задание 33: Создайте статический класс SecurityHelper для криптографических операций
public static class SecurityHelper
{
    public static string GenerateSalt(int size = 32)
    {
        var random = new Random();
        var salt = new byte[size];
        random.NextBytes(salt);
        return Convert.ToBase64String(salt);
    }

    public static string GenerateSecureToken(int length = 32)
    {
        var randomNumber = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

// Задание 34: Реализуйте статический класс PathHelper для работы с путями файлов
public static class PathHelper
{
    public static string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    public static string GetSafeFilename(string filename)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return new string(filename.Where(ch => !invalidChars.Contains(ch)).ToArray());
    }

    public static string EnsureDirectoryExists(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        return path;
    }
}

// Задание 35: Создайте статический класс ColorHelper для работы с цветами
public static class ColorHelper
{
    public static string RgbToHex(int r, int g, int b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    public static (int r, int g, int b) HexToRgb(string hex)
    {
        if (hex.StartsWith("#")) hex = hex[1..];
        if (hex.Length != 6) throw new ArgumentException("Неверный формат HEX цвета");

        var r = Convert.ToInt32(hex[0..2], 16);
        var g = Convert.ToInt32(hex[2..4], 16);
        var b = Convert.ToInt32(hex[4..6], 16);

        return (r, g, b);
    }
}

// Задание 36: Разработайте статический класс CompressionHelper для сжатия данных
public static class CompressionHelper
{
    public static byte[] Compress(byte[] data)
    {
        using (var ms = new MemoryStream())
        {
            using (var gzip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return ms.ToArray();
        }
    }

    public static byte[] Decompress(byte[] compressedData)
    {
        using (var ms = new MemoryStream(compressedData))
        using (var gzip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress))
        using (var result = new MemoryStream())
        {
            gzip.CopyTo(result);
            return result.ToArray();
        }
    }
}

// Задание 37: Создайте статический класс EnvironmentHelper для работы с переменными окружения
public static class EnvironmentHelper
{
    public static string GetEnvironmentVariable(string variable, string defaultValue = "")
    {
        return Environment.GetEnvironmentVariable(variable) ?? defaultValue;
    }

    public static bool Is64BitProcess => Environment.Is64BitProcess;
    public static bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;
    public static string CurrentDirectory => Environment.CurrentDirectory;
    public static string MachineName => Environment.MachineName;
    public static string UserName => Environment.UserName;
    public static string OSVersion => Environment.OSVersion.ToString();
}

// Задание 38: Реализуйте статический класс UrlHelper для работы с URL
public static class UrlHelper
{
    public static string Combine(string baseUrl, params string[] segments)
    {
        if (string.IsNullOrEmpty(baseUrl)) return string.Empty;

        var uri = new Uri(baseUrl);
        foreach (var segment in segments)
        {
            if (!string.IsNullOrEmpty(segment))
            {
                uri = new Uri(uri, segment.Trim('/'));
            }
        }
        return uri.ToString();
    }

    public static Dictionary<string, string> ParseQueryString(string url)
    {
        var parameters = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(url)) return parameters;

        var query = url.Split('?').LastOrDefault();
        if (string.IsNullOrEmpty(query)) return parameters;

        foreach (var param in query.Split('&'))
        {
            var parts = param.Split('=');
            if (parts.Length == 2)
            {
                parameters[parts[0]] = Uri.UnescapeDataString(parts[1]);
            }
        }
        return parameters;
    }
}

// Задание 39: Создайте статический класс TextProcessor для обработки текста
public static class TextProcessor
{
    public static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) !=
                System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    public static int CountSentences(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        return text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static string TruncateWords(string text, int maxWords, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(text)) return text;

        var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= maxWords) return text;

        return string.Join(" ", words.Take(maxWords)) + suffix;
    }
}

// Задание 40: Разработайте статический класс SystemInfo для получения информации о системе
public static class SystemInfo
{
    public static long TotalMemory => GC.GetTotalMemory(false);
    public static int ProcessorCount => Environment.ProcessorCount;
    public static string FrameworkVersion => Environment.Version.ToString();
    public static long WorkingSet => Environment.WorkingSet;

    public static void PrintSystemInfo()
    {
        Console.WriteLine($"Процессоров: {ProcessorCount}");
        Console.WriteLine($"Память: {BytesToReadableSize(WorkingSet)}");
        Console.WriteLine(".NET Version: " + FrameworkVersion);
    }

    private static string BytesToReadableSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        if (bytes == 0) return "0 B";
        int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(1024));
        return $"{bytes / Math.Pow(1024, i):0.##} {sizes[i]}";
    }
}

// ============================================================================
// РАЗДЕЛ 3: РАСШИРЯЮЩИЕ МЕТОДЫ (Задания 41-60)
// ============================================================================

// Задание 41: Создайте расширяющий метод для строки, который проверяет, является ли она палиндромом
public static class PalindromeExtensions
{
    public static bool IsPalindrome(this string str)
    {
        if (string.IsNullOrEmpty(str)) return true;
        var clean = new string(str.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return clean == new string(clean.Reverse().ToArray());
    }
}

// Задание 42: Реализуйте расширяющий метод для массива, который возвращает случайный элемент
public static class ArrayExtensions
{
    private static readonly Random _random = new Random();

    public static T RandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
            throw new ArgumentException("Массив не может быть пустым");

        return array[_random.Next(array.Length)];
    }
}

// Задание 43: Создайте расширяющий метод для числа, который проверяет, является ли оно четным
public static class NumberExtensions
{
    public static bool IsEven(this int number) => number % 2 == 0;
    public static bool IsOdd(this int number) => number % 2 != 0;
    // ИСПРАВЛЕНИЕ: Убираем конфликтующий метод IsPrime
}

// Задание 44: Разработайте расширяющий метод для DateTime, который возвращает возраст
public static class DateTimeExtensions
{
    public static int Age(this DateTime birthDate)
    {
        return DateTimeHelper.CalculateAge(birthDate);
    }

    public static bool IsWeekend(this DateTime date) => DateTimeHelper.IsWeekend(date);
    public static bool IsWeekday(this DateTime date) => !date.IsWeekend();
    public static DateTime StartOfWeek(this DateTime date) => DateTimeHelper.StartOfWeek(date);
}

// Задание 45: Создайте расширяющий метод для строки, который убирает все пробелы
public static class StringManipulationExtensions
{
    public static string RemoveWhitespace(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    public static string RemoveAll(this string str, params char[] charsToRemove)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return new string(str.Where(c => !charsToRemove.Contains(c)).ToArray());
    }
}

// Задание 46: Реализуйте расширяющий метод для списка, который перемешивает элементы
public static class ListExtensions
{
    private static readonly Random _random = new Random();

    public static List<T> Shuffle<T>(this List<T> list)
    {
        if (list == null) return list;

        var shuffled = new List<T>(list);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled;
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ToList().Shuffle();
    }
}

// Задание 47: Создайте расширяющий метод для строки, который переводит в Title Case
public static class CaseExtensions
{
    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        var words = str.ToLower().Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i][1..];
            }
        }
        return string.Join(" ", words);
    }
}

// Задание 48: Разработайте расширяющий метод для числа, который переводит в римские цифры
public static class RomanNumeralExtensions
{
    private static readonly Dictionary<int, string> RomanNumerals = new Dictionary<int, string>
    {
        {1000, "M"}, {900, "CM"}, {500, "D"}, {400, "CD"},
        {100, "C"}, {90, "XC"}, {50, "L"}, {40, "XL"},
        {10, "X"}, {9, "IX"}, {5, "V"}, {4, "IV"}, {1, "I"}
    };

    public static string ToRoman(this int number)
    {
        if (number < 1 || number > 3999)
            throw new ArgumentOutOfRangeException("Число должно быть в диапазоне 1-3999");

        var result = new StringBuilder();
        foreach (var numeral in RomanNumerals)
        {
            while (number >= numeral.Key)
            {
                result.Append(numeral.Value);
                number -= numeral.Key;
            }
        }
        return result.ToString();
    }
}

// Задание 49: Создайте расширяющий метод для коллекции, который проверяет, не пуста ли она
public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return collection == null || !collection.Any();
    }

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return !collection.IsNullOrEmpty();
    }
}

// Задание 50: Реализуйте расширяющий метод для строки, который подсчитывает количество слов
public static class WordCountExtensions
{
    public static int WordCount(this string str)
    {
        return StringHelper.CountWords(str);
    }

    public static int CharacterCount(this string str, bool excludeWhitespace = false)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        return excludeWhitespace ? str.Count(c => !char.IsWhiteSpace(c)) : str.Length;
    }
}

// Задание 51: Создайте расширяющий метод для массива, который находит медиану
public static class StatisticsExtensions
{
    public static double Median(this int[] numbers)
    {
        if (numbers.IsNullOrEmpty()) throw new ArgumentException("Массив не может быть пустым");

        var sorted = numbers.OrderBy(n => n).ToArray();
        int mid = sorted.Length / 2;

        if (sorted.Length % 2 == 0)
            return (sorted[mid - 1] + sorted[mid]) / 2.0;
        else
            return sorted[mid];
    }

    public static double Average(this int[] numbers) => numbers.Average();
    public static int Max(this int[] numbers) => numbers.Max();
    public static int Min(this int[] numbers) => numbers.Min();
}

// Задание 52: Разработайте расширяющий метод для DateTime, который возвращает начало недели
public static class DateTimeRangeExtensions
{
    public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        return DateTimeHelper.StartOfWeek(date, startOfWeek);
    }

    public static DateTime EndOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        return date.StartOfWeek(startOfWeek).AddDays(6);
    }

    public static DateTime StartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    public static DateTime EndOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
}

// Задание 53: Создайте расширяющий метод для строки, который удаляет HTML теги
public static class HtmlExtensions
{
    public static string StripHtml(this string html)
    {
        return RegexHelper.RemoveHtmlTags(html);
    }

    public static string EscapeHtml(this string text)
    {
        return System.Net.WebUtility.HtmlEncode(text);
    }
}

// Задание 54: Реализуйте расширяющий метод для числа, который проверяет, является ли оно простым
public static class PrimeExtensions
{
    // ИСПРАВЛЕНИЕ: Оставляем только один метод IsPrime
    public static bool IsPrime(this int number) => MathHelper.IsPrime(number);

    public static IEnumerable<int> GetPrimeFactors(this int number)
    {
        if (number < 2) yield break;

        for (int i = 2; i <= number; i++)
        {
            while (number % i == 0 && MathHelper.IsPrime(i))
            {
                yield return i;
                number /= i;
            }
        }
    }
}

// Задание 55: Создайте расширяющий метод для коллекции, который группирует элементы по размеру
public static class GroupingExtensions
{
    public static IEnumerable<IEnumerable<T>> GroupBySize<T>(this IEnumerable<T> collection, int groupSize)
    {
        if (groupSize <= 0) throw new ArgumentException("Размер группы должен быть положительным");

        var list = collection.ToList();
        for (int i = 0; i < list.Count; i += groupSize)
        {
            yield return list.Skip(i).Take(groupSize);
        }
    }
}

// Задание 56: Разработайте расширяющий метод для строки, который кодирует в Base64
public static class EncodingExtensions
{
    public static string ToBase64(this string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var bytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }

    public static string FromBase64(this string base64)
    {
        if (string.IsNullOrEmpty(base64)) return base64;
        try
        {
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }
}

// Задание 57: Создайте расширяющий метод для массива, который находит второй максимальный элемент
public static class ArraySearchExtensions
{
    public static T SecondMax<T>(this T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length < 2)
            throw new ArgumentException("Массив должен содержать как минимум 2 элемента");

        var max = array[0];
        var secondMax = array[0];

        foreach (var item in array)
        {
            if (item.CompareTo(max) > 0)
            {
                secondMax = max;
                max = item;
            }
            else if (item.CompareTo(secondMax) > 0 && item.CompareTo(max) < 0)
            {
                secondMax = item;
            }
        }

        return secondMax;
    }
}

// Задание 58: Реализуйте расширяющий метод для DateTime, который форматирует в читаемый вид
public static class DateTimeFormatExtensions
{
    public static string ToReadableString(this DateTime date)
    {
        var now = DateTime.Now;
        var diff = now - date;

        if (diff.TotalSeconds < 60) return "только что";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} минут назад";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} часов назад";
        if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} дней назад";

        return date.ToString("dd.MM.yyyy HH:mm");
    }
}

// Задание 59: Создайте расширяющий метод для строки, который проверяет силу пароля
public static class PasswordExtensions
{
    public static PasswordStrength CheckPasswordStrength(this string password)
    {
        if (string.IsNullOrEmpty(password)) return PasswordStrength.VeryWeak;

        int score = 0;
        if (password.Length >= 8) score++;
        if (password.Any(char.IsUpper)) score++;
        if (password.Any(char.IsLower)) score++;
        if (password.Any(char.IsDigit)) score++;
        if (password.Any(ch => !char.IsLetterOrDigit(ch))) score++;
        if (password.Length >= 12) score++;

        return score switch
        {
            <= 2 => PasswordStrength.Weak,
            3 => PasswordStrength.Medium,
            4 => PasswordStrength.Strong,
            _ => PasswordStrength.VeryStrong
        };
    }
}

public enum PasswordStrength
{
    VeryWeak,
    Weak,
    Medium,
    Strong,
    VeryStrong
}

// Задание 60: Разработайте расширяющий метод для числа, который переводит байты в читаемый формат
public static class ByteSizeExtensions
{
    public static string ToReadableSize(this long bytes)
    {
        return ConversionHelper.BytesToReadableSize(bytes);
    }

    public static string ToReadableSize(this int bytes)
    {
        return ConversionHelper.BytesToReadableSize(bytes);
    }
}

// ============================================================================
// РАЗДЕЛ 4: ВЛОЖЕННЫЕ КЛАССЫ (Задания 61-80)
// ============================================================================

// Задание 61: Создайте класс Car с вложенным классом Engine
public class Car
{
    public string Model { get; set; }
    public int Year { get; set; }
    public Engine CarEngine { get; set; }

    public Car(string model, int year, string engineType, int horsepower)
    {
        Model = model;
        Year = year;
        CarEngine = new Engine(engineType, horsepower);
    }

    public class Engine
    {
        public string Type { get; set; }
        public int Horsepower { get; set; }
        public bool IsRunning { get; private set; }

        public Engine(string type, int horsepower)
        {
            Type = type;
            Horsepower = horsepower;
        }

        public void Start() => IsRunning = true;
        public void Stop() => IsRunning = false;
    }
}

// Задание 62: Реализуйте класс Library с вложенным классом Book
public class Library
{
    private List<Book> _books = new List<Book>();

    public void AddBook(string title, string author, int year)
    {
        _books.Add(new Book(title, author, year));
    }

    public IEnumerable<Book> GetBooks() => _books;

    public class Book
    {
        public string Title { get; }
        public string Author { get; }
        public int PublicationYear { get; }
        public bool IsBorrowed { get; private set; }

        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            PublicationYear = year;
        }

        public void Borrow() => IsBorrowed = true;
        public void Return() => IsBorrowed = false;
    }
}

// Задание 63: Создайте класс Computer с приватными вложенными классами CPU и RAM
public class Computer
{
    private CPU _cpu;
    private RAM _ram;

    public Computer(string cpuModel, int ramSize)
    {
        _cpu = new CPU(cpuModel);
        _ram = new RAM(ramSize);
    }

    public string GetInfo()
    {
        return $"CPU: {_cpu.Model}, RAM: {_ram.Size}GB";
    }

    private class CPU
    {
        public string Model { get; }

        public CPU(string model)
        {
            Model = model;
        }
    }

    private class RAM
    {
        public int Size { get; }

        public RAM(int size)
        {
            Size = size;
        }
    }
}

// Задание 64: Разработайте класс University с защищенным вложенным классом Student
public class University
{
    private List<Student> _students = new List<Student>();

    public void EnrollStudent(string name, string major, int studentId)
    {
        _students.Add(new Student(name, major, studentId));
    }

    public IEnumerable<string> GetStudentNames() => _students.Select(s => s.Name);

    protected class Student
    {
        public string Name { get; }
        public string Major { get; }
        public int StudentId { get; }
        public double GPA { get; private set; }

        public Student(string name, string major, int studentId)
        {
            Name = name;
            Major = major;
            StudentId = studentId;
        }

        public void UpdateGPA(double gpa) => GPA = Math.Clamp(gpa, 0, 4);
    }
}

// Задание 65: Создайте класс BankAccount с публичным вложенным классом Transaction
public class BankAccount
{
    private decimal _balance;
    private List<Transaction> _transactions = new List<Transaction>();

    public string AccountNumber { get; }

    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
    }

    public void Deposit(decimal amount, string description = "Deposit")
    {
        _balance += amount;
        _transactions.Add(new Transaction(amount, TransactionType.Deposit, description));
    }

    public bool Withdraw(decimal amount, string description = "Withdrawal")
    {
        if (_balance >= amount)
        {
            _balance -= amount;
            _transactions.Add(new Transaction(amount, TransactionType.Withdrawal, description));
            return true;
        }
        return false;
    }

    public IEnumerable<Transaction> GetTransactionHistory() => _transactions;

    public class Transaction
    {
        public decimal Amount { get; }
        public TransactionType Type { get; }
        public string Description { get; }
        public DateTime Timestamp { get; }

        public Transaction(decimal amount, TransactionType type, string description)
        {
            Amount = amount;
            Type = type;
            Description = description;
            Timestamp = DateTime.Now;
        }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
}

// Задание 66: Реализуйте класс Game с вложенным статическим классом Rules
public class Game
{
    public string Name { get; set; }
    public int Players { get; set; }

    public static class Rules
    {
        public static int MaxPlayers = 10;
        public static int MinPlayers = 1;
        public static int MaxScore = 100;

        public static bool ValidatePlayerCount(int players)
        {
            return players >= MinPlayers && players <= MaxPlayers;
        }

        public static bool ValidateScore(int score)
        {
            return score >= 0 && score <= MaxScore;
        }
    }
}

// Задание 67: Создайте класс TreeNode с вложенными классами для обхода дерева
public class TreeNode<T>
{
    public T Value { get; set; }
    public List<TreeNode<T>> Children { get; } = new List<TreeNode<T>>();

    public void AddChild(TreeNode<T> child) => Children.Add(child);

    public class TreeEnumerator
    {
        private readonly TreeNode<T> _root;

        public TreeEnumerator(TreeNode<T> root)
        {
            _root = root;
        }

        public IEnumerable<T> PreOrderTraversal()
        {
            return TraversePreOrder(_root);
        }

        private IEnumerable<T> TraversePreOrder(TreeNode<T> node)
        {
            if (node != null)
            {
                yield return node.Value;
                foreach (var child in node.Children)
                {
                    foreach (var value in TraversePreOrder(child))
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}

// Задание 68: Разработайте класс Calculator с вложенными классами для разных операций
public class AdvancedCalculator
{
    public static class Operations
    {
        public static double Add(double a, double b) => a + b;
        public static double Subtract(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;
        public static double Divide(double a, double b) => b != 0 ? a / b : double.NaN;
    }

    public static class ScientificOperations
    {
        public static double Power(double baseValue, double exponent) => Math.Pow(baseValue, exponent);
        public static double SquareRoot(double value) => Math.Sqrt(value);
        public static double Logarithm(double value, double baseValue = 10) => Math.Log(value, baseValue);
    }
}

// Задание 69: Создайте класс Document с вложенным классом Page
public class Document
{
    private List<Page> _pages = new List<Page>();

    public string Title { get; set; }
    public string Author { get; set; }

    public void AddPage(string content, int pageNumber = -1)
    {
        if (pageNumber == -1) pageNumber = _pages.Count + 1;
        _pages.Add(new Page(content, pageNumber));
    }

    public int PageCount => _pages.Count;
    public IEnumerable<Page> GetPages() => _pages;

    public class Page
    {
        public string Content { get; }
        public int PageNumber { get; }
        public DateTime CreatedAt { get; }

        public Page(string content, int pageNumber)
        {
            Content = content;
            PageNumber = pageNumber;
            CreatedAt = DateTime.Now;
        }

        public int WordCount => Content.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// Задание 70: Реализуйте класс Network с вложенными классами Node и Connection
public class Network
{
    private List<Node> _nodes = new List<Node>();
    private List<Connection> _connections = new List<Connection>();

    public void AddNode(string name, string ipAddress)
    {
        _nodes.Add(new Node(name, ipAddress));
    }

    public void ConnectNodes(string node1Name, string node2Name, int speed)
    {
        var node1 = _nodes.FirstOrDefault(n => n.Name == node1Name);
        var node2 = _nodes.FirstOrDefault(n => n.Name == node2Name);

        if (node1 != null && node2 != null)
        {
            _connections.Add(new Connection(node1, node2, speed));
        }
    }

    public class Node
    {
        public string Name { get; }
        public string IPAddress { get; }
        public bool IsOnline { get; private set; }

        public Node(string name, string ipAddress)
        {
            Name = name;
            IPAddress = ipAddress;
        }

        public void SetOnline() => IsOnline = true;
        public void SetOffline() => IsOnline = false;
    }

    public class Connection
    {
        public Node From { get; }
        public Node To { get; }
        public int Speed { get; } // Mbps
        public bool IsActive { get; private set; }

        public Connection(Node from, Node to, int speed)
        {
            From = from;
            To = to;
            Speed = speed;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}

// Задание 71: Создайте класс House с вложенными классами Room и Furniture
public class House
{
    private List<Room> _rooms = new List<Room>();

    public string Address { get; set; }
    public int TotalArea => _rooms.Sum(r => r.Area);

    public void AddRoom(string name, int area)
    {
        _rooms.Add(new Room(name, area));
    }

    public void AddFurnitureToRoom(string roomName, string furnitureName, string type)
    {
        var room = _rooms.FirstOrDefault(r => r.Name == roomName);
        room?.AddFurniture(furnitureName, type);
    }

    public class Room
    {
        public string Name { get; }
        public int Area { get; }
        private List<Furniture> _furniture = new List<Furniture>();

        public Room(string name, int area)
        {
            Name = name;
            Area = area;
        }

        public void AddFurniture(string name, string type)
        {
            _furniture.Add(new Furniture(name, type));
        }

        public int FurnitureCount => _furniture.Count;
    }

    public class Furniture
    {
        public string Name { get; }
        public string Type { get; }

        public Furniture(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}

// Задание 72: Разработайте класс Phone с вложенными классами Contact и Message
public class Phone
{
    private List<Contact> _contacts = new List<Contact>();
    private List<Message> _messages = new List<Message>();

    public string Model { get; set; }
    public string Number { get; set; }

    public void AddContact(string name, string phoneNumber)
    {
        _contacts.Add(new Contact(name, phoneNumber));
    }

    public void SendMessage(string toNumber, string content)
    {
        var message = new Message(Number, toNumber, content);
        _messages.Add(message);
    }

    public IEnumerable<Message> GetMessages() => _messages;

    public class Contact
    {
        public string Name { get; }
        public string PhoneNumber { get; }

        public Contact(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }

    public class Message
    {
        public string From { get; }
        public string To { get; }
        public string Content { get; }
        public DateTime SentAt { get; }
        public bool IsRead { get; private set; }

        public Message(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
            SentAt = DateTime.Now;
        }

        public void MarkAsRead() => IsRead = true;
    }
}

// Задание 73: Создайте класс Restaurant с вложенными классами Menu и Order
public class Restaurant
{
    private Menu _menu = new Menu();
    private List<Order> _orders = new List<Order>();

    public string Name { get; set; }

    public void AddMenuItem(string name, decimal price, string category)
    {
        _menu.AddItem(name, price, category);
    }

    public Order CreateOrder(int tableNumber)
    {
        var order = new Order(tableNumber);
        _orders.Add(order);
        return order;
    }

    public class Menu
    {
        private List<MenuItem> _items = new List<MenuItem>();

        public void AddItem(string name, decimal price, string category)
        {
            _items.Add(new MenuItem(name, price, category));
        }

        public IEnumerable<MenuItem> GetItemsByCategory(string category)
        {
            return _items.Where(i => i.Category == category);
        }

        public class MenuItem
        {
            public string Name { get; }
            public decimal Price { get; }
            public string Category { get; }

            public MenuItem(string name, decimal price, string category)
            {
                Name = name;
                Price = price;
                Category = category;
            }
        }
    }

    public class Order
    {
        public int TableNumber { get; }
        public DateTime CreatedAt { get; }
        private List<Menu.MenuItem> _items = new List<Menu.MenuItem>();
        public OrderStatus Status { get; private set; }

        public Order(int tableNumber)
        {
            TableNumber = tableNumber;
            CreatedAt = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        public void AddItem(Menu.MenuItem item) => _items.Add(item);
        public decimal Total => _items.Sum(i => i.Price);
        public void SetStatus(OrderStatus status) => Status = status;
    }

    public enum OrderStatus
    {
        Pending,
        Cooking,
        Ready,
        Served,
        Paid
    }
}

// Задание 74: Реализуйте класс School с вложенными классами Teacher и Classroom
public class School
{
    private List<Teacher> _teachers = new List<Teacher>();
    private List<Classroom> _classrooms = new List<Classroom>();

    public string Name { get; set; }

    public void HireTeacher(string name, string subject)
    {
        _teachers.Add(new Teacher(name, subject));
    }

    public void AddClassroom(string number, int capacity)
    {
        _classrooms.Add(new Classroom(number, capacity));
    }

    public class Teacher
    {
        public string Name { get; }
        public string Subject { get; }

        public Teacher(string name, string subject)
        {
            Name = name;
            Subject = subject;
        }
    }

    public class Classroom
    {
        public string Number { get; }
        public int Capacity { get; }
        public bool IsOccupied { get; private set; }

        public Classroom(string number, int capacity)
        {
            Number = number;
            Capacity = capacity;
        }

        public void Occupy() => IsOccupied = true;
        public void Vacate() => IsOccupied = false;
    }
}

// Задание 75: Создайте класс LinkedList с вложенным классом Node
public class LinkedList<T>
{
    private Node _head;
    private Node _tail;
    private int _count;

    public int Count => _count;

    public void Add(T value)
    {
        var newNode = new Node(value);

        if (_head == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            _tail.Next = newNode;
            _tail = newNode;
        }

        _count++;
    }

    public bool Contains(T value)
    {
        var current = _head;
        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
                return true;
            current = current.Next;
        }
        return false;
    }

    private class Node
    {
        public T Value { get; }
        public Node Next { get; set; }

        public Node(T value)
        {
            Value = value;
        }
    }
}

// Задание 76: Разработайте класс Email с вложенными классами Attachment и Header
public class Email
{
    private List<Attachment> _attachments = new List<Attachment>();
    private Header _header;

    public string Body { get; set; }

    public Email(string from, string to, string subject)
    {
        _header = new Header(from, to, subject);
    }

    public void AddAttachment(string fileName, byte[] content, string contentType)
    {
        _attachments.Add(new Attachment(fileName, content, contentType));
    }

    public int AttachmentCount => _attachments.Count;
    public long TotalAttachmentSize => _attachments.Sum(a => a.Size);

    public class Header
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public DateTime SentAt { get; }

        public Header(string from, string to, string subject)
        {
            From = from;
            To = to;
            Subject = subject;
            SentAt = DateTime.Now;
        }
    }

    public class Attachment
    {
        public string FileName { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public long Size => Content.Length;

        public Attachment(string fileName, byte[] content, string contentType)
        {
            FileName = fileName;
            Content = content;
            ContentType = contentType;
        }
    }
}

// Задание 77: Создайте класс Team с вложенными классами Player и Coach
public class Team
{
    private List<Player> _players = new List<Player>();
    private Coach _coach;

    public string Name { get; set; }
    public string Sport { get; set; }

    public void AddPlayer(string name, string position, int number)
    {
        _players.Add(new Player(name, position, number));
    }

    public void SetCoach(string name, int experienceYears)
    {
        _coach = new Coach(name, experienceYears);
    }

    public int PlayerCount => _players.Count;

    public class Player
    {
        public string Name { get; }
        public string Position { get; }
        public int Number { get; }
        public bool IsInjured { get; private set; }

        public Player(string name, string position, int number)
        {
            Name = name;
            Position = position;
            Number = number;
        }

        public void SetInjured() => IsInjured = true;
        public void SetHealthy() => IsInjured = false;
    }

    public class Coach
    {
        public string Name { get; }
        public int ExperienceYears { get; }

        public Coach(string name, int experienceYears)
        {
            Name = name;
            ExperienceYears = experienceYears;
        }
    }
}

// Задание 78: Реализуйте класс Shape с вложенными классами Circle и Rectangle
public abstract class Shape
{
    public abstract double Area { get; }
    public abstract double Perimeter { get; }

    public class Circle : Shape
    {
        public double Radius { get; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double Area => Math.PI * Radius * Radius;
        public override double Perimeter => 2 * Math.PI * Radius;
    }

    public class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override double Area => Width * Height;
        public override double Perimeter => 2 * (Width + Height);
    }
}

// Задание 79: Создайте класс Database с вложенными классами Table и Column
public class Database
{
    private List<Table> _tables = new List<Table>();

    public string Name { get; set; }

    public void CreateTable(string tableName)
    {
        _tables.Add(new Table(tableName));
    }

    public void AddColumnToTable(string tableName, string columnName, string dataType)
    {
        var table = _tables.FirstOrDefault(t => t.Name == tableName);
        table?.AddColumn(columnName, dataType);
    }

    public class Table
    {
        public string Name { get; }
        private List<Column> _columns = new List<Column>();

        public Table(string name)
        {
            Name = name;
        }

        public void AddColumn(string name, string dataType)
        {
            _columns.Add(new Column(name, dataType));
        }

        public int ColumnCount => _columns.Count;
    }

    public class Column
    {
        public string Name { get; }
        public string DataType { get; }

        public Column(string name, string dataType)
        {
            Name = name;
            DataType = dataType;
        }
    }
}

// Задание 80: Разработайте класс Store с вложенными классами Product и Category
public class Store
{
    private List<Category> _categories = new List<Category>();

    public string Name { get; set; }

    public void AddCategory(string categoryName)
    {
        _categories.Add(new Category(categoryName));
    }

    public void AddProductToCategory(string categoryName, string productName, decimal price, int stock)
    {
        var category = _categories.FirstOrDefault(c => c.Name == categoryName);
        category?.AddProduct(productName, price, stock);
    }

    public class Category
    {
        public string Name { get; }
        private List<Product> _products = new List<Product>();

        public Category(string name)
        {
            Name = name;
        }

        public void AddProduct(string name, decimal price, int stock)
        {
            _products.Add(new Product(name, price, stock));
        }

        public int ProductCount => _products.Count;
        public decimal TotalValue => _products.Sum(p => p.Price * p.Stock);
    }

    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        public int Stock { get; private set; }

        public Product(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void IncreaseStock(int amount) => Stock += amount;
        public bool DecreaseStock(int amount)
        {
            if (Stock >= amount)
            {
                Stock -= amount;
                return true;
            }
            return false;
        }
    }
}

// ============================================================================
// РАЗДЕЛ 5: ПАТТЕРН SINGLETON (Задания 81-100)
// ============================================================================

// Задание 81: Реализуйте классический паттерн Singleton для класса DatabaseConnection
public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private static readonly object _lockObject = new object();

    public string ConnectionString { get; private set; }

    private DatabaseConnection()
    {
        ConnectionString = "Server=localhost;Database=MyDB;Trusted_Connection=true;";
    }

    public static DatabaseConnection Instance
    {
        get
        {
            lock (_lockObject)
            {
                return _instance ??= new DatabaseConnection();
            }
        }
    }

    public void Connect() => Console.WriteLine("Подключение к базе данных...");
    public void Disconnect() => Console.WriteLine("Отключение от базе данных...");
}

// Задание 82: Создайте thread-safe Singleton с использованием lock для класса Logger
public class ThreadSafeLogger
{
    private static ThreadSafeLogger _instance;
    private static readonly object _lockObject = new object();
    private readonly List<string> _logEntries = new List<string>();

    private ThreadSafeLogger() { }

    public static ThreadSafeLogger Instance
    {
        get
        {
            lock (_lockObject)
            {
                return _instance ??= new ThreadSafeLogger();
            }
        }
    }

    public void Log(string message)
    {
        lock (_lockObject)
        {
            var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            _logEntries.Add(entry);
            Console.WriteLine(entry);
        }
    }

    public IEnumerable<string> GetLogs() => _logEntries;
}

// Задание 83: Реализуйте Singleton с ленивой инициализацией через Lazy<T>
public class LazySingleton
{
    private static readonly Lazy<LazySingleton> _lazyInstance =
        new Lazy<LazySingleton>(() => new LazySingleton());

    public static LazySingleton Instance => _lazyInstance.Value;

    private LazySingleton()
    {
        Console.WriteLine("LazySingleton инициализирован");
    }

    public void DoWork() => Console.WriteLine("LazySingleton работает...");
}

// Задание 84: Создайте Singleton с проверкой на null и двойной блокировкой
public class DoubleCheckedSingleton
{
    private static DoubleCheckedSingleton _instance;
    private static readonly object _lockObject = new object();

    private DoubleCheckedSingleton() { }

    public static DoubleCheckedSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new DoubleCheckedSingleton();
                    }
                }
            }
            return _instance;
        }
    }
}

// Задание 85: Разработайте Singleton для Configuration Manager
public class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _instance =
        new Lazy<ConfigurationManager>(() => new ConfigurationManager());

    private readonly Dictionary<string, string> _config;

    public static ConfigurationManager Instance => _instance.Value;

    private ConfigurationManager()
    {
        _config = new Dictionary<string, string>
        {
            ["DatabaseConnection"] = "Server=localhost;Database=MyDB;",
            ["ApiUrl"] = "https://api.example.com",
            ["Timeout"] = "30",
            ["LogLevel"] = "Information"
        };
    }

    public string GetValue(string key) => _config.ContainsKey(key) ? _config[key] : null;
    public void SetValue(string key, string value) => _config[key] = value;
}

// Задание 86: Реализуйте Singleton с использованием статического конструктора
public class StaticConstructorSingleton
{
    private static readonly StaticConstructorSingleton _instance;

    static StaticConstructorSingleton()
    {
        _instance = new StaticConstructorSingleton();
        Console.WriteLine("Статический конструктор вызван");
    }

    private StaticConstructorSingleton() { }

    public static StaticConstructorSingleton Instance => _instance;
}

// Задание 87: Создайте enum-based Singleton для ApplicationSettings
public class EnumBasedSingleton
{
    public static EnumBasedSingleton Instance { get; } = new EnumBasedSingleton();

    static EnumBasedSingleton() { }

    private EnumBasedSingleton() { }
}

// Задание 88: Разработайте generic Singleton base class
public abstract class SingletonBase<T> where T : class, new()
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

    public static T Instance => _instance.Value;

    protected SingletonBase() { }
}

// Задание 89: Реализуйте Singleton с возможностью уничтожения и пересоздания
public class ResettableSingleton
{
    private static ResettableSingleton _instance;
    private static readonly object _lockObject = new object();

    public static ResettableSingleton Instance
    {
        get
        {
            lock (_lockObject)
            {
                return _instance ??= new ResettableSingleton();
            }
        }
    }

    private ResettableSingleton() { }

    public static void Reset()
    {
        lock (_lockObject)
        {
            _instance = null;
        }
    }
}

// Задание 90: Создайте Singleton для Cache Manager с методами добавления и получения
public class CacheManager
{
    private static readonly Lazy<CacheManager> _instance = new Lazy<CacheManager>();
    private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
    private readonly object _lockObject = new object();

    public static CacheManager Instance => _instance.Value;

    private CacheManager() { }

    public void Set(string key, object value, TimeSpan? expiration = null)
    {
        lock (_lockObject)
        {
            _cache[key] = value;
        }
    }

    public T Get<T>(string key)
    {
        lock (_lockObject)
        {
            return _cache.ContainsKey(key) ? (T)_cache[key] : default(T);
        }
    }

    public bool Remove(string key)
    {
        lock (_lockObject)
        {
            return _cache.Remove(key);
        }
    }

    public void Clear()
    {
        lock (_lockObject)
        {
            _cache.Clear();
        }
    }
}

// Задание 91: Разработайте Singleton для FileManager с проверкой существования файлов
public class FileManager
{
    private static readonly Lazy<FileManager> _instance = new Lazy<FileManager>();

    public static FileManager Instance => _instance.Value;

    private FileManager() { }

    public bool FileExists(string path) => File.Exists(path);
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public string ReadAllText(string path)
    {
        return FileExists(path) ? File.ReadAllText(path) : string.Empty;
    }

    public void WriteAllText(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);
        if (!DirectoryExists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(path, content);
    }
}

// Задание 92: Реализуйте Singleton для Random Generator с различными методами генерации
public class RandomManager
{
    private static readonly Lazy<RandomManager> _instance = new Lazy<RandomManager>();
    private readonly Random _random = new Random();

    public static RandomManager Instance => _instance.Value;

    private RandomManager() { }

    public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    public double NextDouble() => _random.NextDouble();
    public bool NextBool() => _random.Next(2) == 1;

    public string NextString(int length, string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        return new string(Enumerable.Repeat(charset, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}

// Задание 93: Создайте Singleton для Application State с сохранением в файл
public class ApplicationState
{
    private static readonly Lazy<ApplicationState> _instance = new Lazy<ApplicationState>();
    private readonly Dictionary<string, object> _state = new Dictionary<string, object>();
    private readonly string _stateFile = "appstate.json";

    public static ApplicationState Instance => _instance.Value;

    private ApplicationState()
    {
        LoadState();
    }

    public void SetValue(string key, object value)
    {
        _state[key] = value;
        SaveState();
    }

    public T GetValue<T>(string key, T defaultValue = default(T))
    {
        return _state.ContainsKey(key) ? (T)_state[key] : defaultValue;
    }

    private void SaveState()
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_state);
            File.WriteAllText(_stateFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения состояния: {ex.Message}");
        }
    }

    private void LoadState()
    {
        try
        {
            if (File.Exists(_stateFile))
            {
                var json = File.ReadAllText(_stateFile);
                var state = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                if (state != null)
                {
                    foreach (var item in state)
                    {
                        _state[item.Key] = item.Value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки состояния: {ex.Message}");
        }
    }
}

// Задание 94: Разработайте Singleton для Security Manager с аутентификацией
public class SecurityManager
{
    private static readonly Lazy<SecurityManager> _instance = new Lazy<SecurityManager>();
    private readonly Dictionary<string, string> _users = new Dictionary<string, string>();

    public static SecurityManager Instance => _instance.Value;

    private SecurityManager()
    {
        // Добавляем тестовых пользователей
        _users["admin"] = HashHelper.ComputeMD5Hash("admin123");
        _users["user"] = HashHelper.ComputeMD5Hash("user123");
    }

    public bool Authenticate(string username, string password)
    {
        if (_users.ContainsKey(username))
        {
            var hashedPassword = HashHelper.ComputeMD5Hash(password);
            return _users[username] == hashedPassword;
        }
        return false;
    }

    public void AddUser(string username, string password)
    {
        _users[username] = HashHelper.ComputeMD5Hash(password);
    }
}

// Задание 95: Реализуйте Singleton для Performance Counter
public class PerformanceCounter
{
    private static readonly Lazy<PerformanceCounter> _instance = new Lazy<PerformanceCounter>();
    private readonly Dictionary<string, long> _counters = new Dictionary<string, long>();
    private readonly object _lockObject = new object();

    public static PerformanceCounter Instance => _instance.Value;

    private PerformanceCounter() { }

    public void Increment(string counterName)
    {
        lock (_lockObject)
        {
            if (!_counters.ContainsKey(counterName))
                _counters[counterName] = 0;
            _counters[counterName]++;
        }
    }

    public long GetCount(string counterName)
    {
        lock (_lockObject)
        {
            return _counters.ContainsKey(counterName) ? _counters[counterName] : 0;
        }
    }

    public void Reset(string counterName)
    {
        lock (_lockObject)
        {
            _counters[counterName] = 0;
        }
    }
}

// Задание 96: Создайте Singleton для Email Service с очередью сообщений
public class EmailService
{
    private static readonly Lazy<EmailService> _instance = new Lazy<EmailService>();
    private readonly Queue<EmailMessage> _messageQueue = new Queue<EmailMessage>();
    private readonly object _lockObject = new object();

    public static EmailService Instance => _instance.Value;

    private EmailService() { }

    public void EnqueueMessage(string to, string subject, string body)
    {
        lock (_lockObject)
        {
            _messageQueue.Enqueue(new EmailMessage(to, subject, body));
        }
    }

    public bool TryDequeueMessage(out EmailMessage message)
    {
        lock (_lockObject)
        {
            if (_messageQueue.Count > 0)
            {
                message = _messageQueue.Dequeue();
                return true;
            }
            message = null;
            return false;
        }
    }

    public int QueueLength
    {
        get
        {
            lock (_lockObject)
            {
                return _messageQueue.Count;
            }
        }
    }

    public class EmailMessage
    {
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }
        public DateTime CreatedAt { get; }

        public EmailMessage(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
            CreatedAt = DateTime.Now;
        }
    }
}

// Задание 97: Разработайте Singleton для Print Manager с очередью печати
public class PrintManager
{
    private static readonly Lazy<PrintManager> _instance = new Lazy<PrintManager>();
    private readonly Queue<PrintJob> _printQueue = new Queue<PrintJob>();
    private readonly object _lockObject = new object();

    public static PrintManager Instance => _instance.Value;

    private PrintManager() { }

    public void AddPrintJob(string documentName, int pages)
    {
        lock (_lockObject)
        {
            _printQueue.Enqueue(new PrintJob(documentName, pages));
        }
    }

    public PrintJob GetNextJob()
    {
        lock (_lockObject)
        {
            return _printQueue.Count > 0 ? _printQueue.Dequeue() : null;
        }
    }

    public int JobsInQueue
    {
        get
        {
            lock (_lockObject)
            {
                return _printQueue.Count;
            }
        }
    }

    public class PrintJob
    {
        public string DocumentName { get; }
        public int Pages { get; }
        public DateTime AddedAt { get; }

        public PrintJob(string documentName, int pages)
        {
            DocumentName = documentName;
            Pages = pages;
            AddedAt = DateTime.Now;
        }
    }
}

// Задание 98: Реализуйте Singleton для Session Manager
public class SessionManager
{
    private static readonly Lazy<SessionManager> _instance = new Lazy<SessionManager>();
    private readonly Dictionary<string, UserSession> _sessions = new Dictionary<string, UserSession>();
    private readonly object _lockObject = new object();

    public static SessionManager Instance => _instance.Value;

    private SessionManager() { }

    public string CreateSession(string username)
    {
        var sessionId = Guid.NewGuid().ToString();
        var session = new UserSession(username);

        lock (_lockObject)
        {
            _sessions[sessionId] = session;
        }

        return sessionId;
    }

    public bool IsValidSession(string sessionId)
    {
        lock (_lockObject)
        {
            return _sessions.ContainsKey(sessionId) && !_sessions[sessionId].IsExpired;
        }
    }

    public void EndSession(string sessionId)
    {
        lock (_lockObject)
        {
            _sessions.Remove(sessionId);
        }
    }

    public class UserSession
    {
        public string Username { get; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; }

        public UserSession(string username)
        {
            Username = username;
            CreatedAt = DateTime.Now;
            ExpiresAt = DateTime.Now.AddHours(1);
        }

        public bool IsExpired => DateTime.Now > ExpiresAt;
    }
}

// Задание 99: Создайте Singleton для Notification Service
public class NotificationService
{
    private static readonly Lazy<NotificationService> _instance = new Lazy<NotificationService>();
    private readonly List<Notification> _notifications = new List<Notification>();
    private readonly object _lockObject = new object();

    public static NotificationService Instance => _instance.Value;

    private NotificationService() { }

    public void AddNotification(string title, string message, NotificationType type = NotificationType.Info)
    {
        lock (_lockObject)
        {
            _notifications.Add(new Notification(title, message, type));
        }
    }

    public IEnumerable<Notification> GetNotifications()
    {
        lock (_lockObject)
        {
            return _notifications.ToList();
        }
    }

    public void ClearNotifications()
    {
        lock (_lockObject)
        {
            _notifications.Clear();
        }
    }

    public class Notification
    {
        public string Title { get; }
        public string Message { get; }
        public NotificationType Type { get; }
        public DateTime CreatedAt { get; }

        public Notification(string title, string message, NotificationType type)
        {
            Title = title;
            Message = message;
            Type = type;
            CreatedAt = DateTime.Now;
        }
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }
}

// Задание 100: Разработайте Singleton для Resource Pool Manager
public class ResourcePoolManager
{
    private static readonly Lazy<ResourcePoolManager> _instance = new Lazy<ResourcePoolManager>();
    private readonly Dictionary<string, List<object>> _pools = new Dictionary<string, List<object>>();
    private readonly object _lockObject = new object();

    public static ResourcePoolManager Instance => _instance.Value;

    private ResourcePoolManager() { }

    // ИСПРАВЛЕНИЕ: Убираем generic constraint new() и используем фабричный метод
    public void CreatePool<T>(string poolName, int initialSize, Func<T> factory = null) where T : class
    {
        lock (_lockObject)
        {
            if (!_pools.ContainsKey(poolName))
            {
                var pool = new List<object>();
                for (int i = 0; i < initialSize; i++)
                {
                    T item = factory != null ? factory() : Activator.CreateInstance<T>();
                    pool.Add(item);
                }
                _pools[poolName] = pool;
            }
        }
    }

    public T Acquire<T>(string poolName, Func<T> factory = null) where T : class
    {
        lock (_lockObject)
        {
            if (_pools.ContainsKey(poolName) && _pools[poolName].Count > 0)
            {
                var resource = _pools[poolName][0];
                _pools[poolName].RemoveAt(0);
                return (T)resource;
            }
            return factory != null ? factory() : Activator.CreateInstance<T>();
        }
    }

    public void Release<T>(string poolName, T resource)
    {
        lock (_lockObject)
        {
            if (_pools.ContainsKey(poolName))
            {
                _pools[poolName].Add(resource);
            }
        }
    }

    public int GetPoolSize(string poolName)
    {
        lock (_lockObject)
        {
            return _pools.ContainsKey(poolName) ? _pools[poolName].Count : 0;
        }
    }
}

// ============================================================================
// ДЕМОНСТРАЦИОННАЯ ПРОГРАММА
// ============================================================================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ 100 ЗАДАЧ ПО C# ===");
        Console.WriteLine("====================================\n");

        DemonstrateStaticMembers();
        DemonstrateStaticClasses();
        DemonstrateExtensionMethods();
        DemonstrateNestedClasses();
        DemonstrateSingletonPattern();

        Console.WriteLine("\n=== ВСЕ 100 ЗАДАЧ УСПЕШНО ПРОДЕМОНСТРИРОВАНЫ ===");
        Console.ReadKey();
    }

    static void DemonstrateStaticMembers()
    {
        Console.WriteLine("1. СТАТИЧЕСКИЕ ЧЛЕНЫ КЛАССОВ (1-20):");
        Console.WriteLine("====================================\n");

        // Задание 1
        Counter.Increment();
        Counter.Increment();
        Console.WriteLine($"Счетчик: {Counter.Count}");

        // Задание 2
        Console.WriteLine($"5 + 3 = {Calculator.Add(5, 3)}");

        // Задание 4
        Logger.Log("Тестовое сообщение");

        // Задание 6
        var obj1 = new ObjectCounter();
        var obj2 = new ObjectCounter();
        Console.WriteLine($"Создано объектов: {ObjectCounter.InstanceCount}");

        // Задание 8
        Console.WriteLine($"Площадь круга радиусом 5: {MathHelper.CalculateCircleArea(5):F2}");

        // Задание 10
        Console.WriteLine($"Email valid: {ValidationHelper.IsValidEmail("test@example.com")}");

        // Задание 12
        Console.WriteLine($"Reverse 'hello': {StringHelper.Reverse("hello")}");

        // Задание 16
        Console.WriteLine($"MD5 of 'hello': {HashHelper.ComputeMD5Hash("hello")}");

        // Задание 20
        Console.WriteLine($"Random string: {RandomGenerator.RandomString(10)}");

        Console.WriteLine();
    }

    static void DemonstrateStaticClasses()
    {
        Console.WriteLine("2. СТАТИЧЕСКИЕ КЛАССЫ (21-40):");
        Console.WriteLine("==============================\n");

        // Задание 21
        Console.WriteLine($"Is null or empty: {Utils.IsNullOrEmpty("")}");

        // Задание 22
        Console.WriteLine($"String extension: {"hello".Left(3)}");

        // Задание 24
        var encrypted = CryptographyHelper.Encrypt("secret", "key");
        Console.WriteLine($"Encrypted: {encrypted}");

        // Задание 26
        var json = JsonHelper.Serialize(new { Name = "John", Age = 30 });
        Console.WriteLine($"JSON: {json}");

        // Задание 27
        Console.WriteLine($"Valid email: {RegexHelper.IsValidEmail("test@example.com")}");

        // Задание 29
        Console.WriteLine($"Corporate email: {EmailValidator.IsCorporateEmail("john@company.com")}");

        // Задание 30
        Console.WriteLine($"Generated password: {PasswordGenerator.Generate(8)}");

        // Задание 32
        Console.WriteLine($"Local IP: {NetworkHelper.GetLocalIPAddress()}");

        // Задание 35
        Console.WriteLine($"RGB to HEX: {ColorHelper.RgbToHex(255, 0, 0)}");

        // Задание 40
        SystemInfo.PrintSystemInfo();

        Console.WriteLine();
    }

    static void DemonstrateExtensionMethods()
    {
        Console.WriteLine("3. РАСШИРЯЮЩИЕ МЕТОДЫ (41-60):");
        Console.WriteLine("==============================\n");

        // Задание 41
        Console.WriteLine($"Is 'radar' palindrome: {"radar".IsPalindrome()}");

        // Задание 42
        var array = new[] { 1, 2, 3, 4, 5 };
        Console.WriteLine($"Random element: {array.RandomElement()}");

        // Задание 43
        Console.WriteLine($"Is 4 even: {4.IsEven()}");

        // Задание 44
        var birthDate = new DateTime(1990, 1, 1);
        Console.WriteLine($"Age: {birthDate.Age()}");

        // Задание 45
        Console.WriteLine($"No spaces: {"hello world".RemoveWhitespace()}");

        // Задание 46
        var list = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"Shuffled: {string.Join(", ", list.Shuffle())}");

        // Задание 47
        Console.WriteLine($"Title case: {"hello world".ToTitleCase()}");

        // Задание 48
        Console.WriteLine($"1984 in Roman: {1984.ToRoman()}");

        // Задание 49
        Console.WriteLine($"List empty: {list.IsNullOrEmpty()}");

        // Задание 50
        Console.WriteLine($"Word count: {"hello world".WordCount()}");

        // Задание 51
        Console.WriteLine($"Median: {array.Median()}");

        // Задание 52
        Console.WriteLine($"Start of week: {DateTime.Now.StartOfWeek():d}");

        // Задание 53
        Console.WriteLine($"Strip HTML: {"<p>Hello</p>".StripHtml()}");

        // Задание 54
        Console.WriteLine($"Is 7 prime: {7.IsPrime()}");

        // Задание 56
        Console.WriteLine($"Base64: {"hello".ToBase64()}");

        // Задание 57
        Console.WriteLine($"Second max: {array.SecondMax()}");

        // Задание 58
        Console.WriteLine($"Readable date: {DateTime.Now.AddMinutes(-5).ToReadableString()}");

        // Задание 59
        Console.WriteLine($"Password strength: {"Password123!".CheckPasswordStrength()}");

        // Задание 60
        Console.WriteLine($"Readable size: {1024.ToReadableSize()}");

        Console.WriteLine();
    }

    static void DemonstrateNestedClasses()
    {
        Console.WriteLine("4. ВЛОЖЕННЫЕ КЛАССЫ (61-80):");
        Console.WriteLine("=============================\n");

        // Задание 61
        var car = new Car("Toyota", 2020, "V6", 300);
        car.CarEngine.Start();
        Console.WriteLine($"Car with {car.CarEngine.Horsepower}HP engine");

        // Задание 62
        var library = new Library();
        library.AddBook("Book1", "Author1", 2020);
        Console.WriteLine("Library with books created");

        // Задание 65
        var account = new BankAccount("12345");
        account.Deposit(1000);
        account.Withdraw(500);
        Console.WriteLine($"Bank account transactions: {account.GetTransactionHistory().Count()}");

        // Задание 66
        Console.WriteLine($"Max players: {Game.Rules.MaxPlayers}");

        // Задание 69
        var document = new Document();
        document.AddPage("Page 1 content");
        Console.WriteLine($"Document pages: {document.PageCount}");

        // Задание 72
        var phone = new Phone();
        phone.AddContact("John", "123-456-7890");
        phone.SendMessage("123-456-7890", "Hello!");
        Console.WriteLine($"Phone messages: {phone.GetMessages().Count()}");

        // Задание 78
        var circle = new Shape.Circle(5);
        Console.WriteLine($"Circle area: {circle.Area:F2}");

        // Задание 80
        var store = new Store();
        store.AddCategory("Electronics");
        store.AddProductToCategory("Electronics", "Laptop", 999.99m, 10);
        Console.WriteLine("Store with products created");

        Console.WriteLine();
    }

    static void DemonstrateSingletonPattern()
    {
        Console.WriteLine("5. ПАТТЕРН SINGLETON (81-100):");
        Console.WriteLine("==============================\n");

        // Задание 81
        var db1 = DatabaseConnection.Instance;
        var db2 = DatabaseConnection.Instance;
        Console.WriteLine($"Database instances equal: {db1 == db2}");

        // Задание 82
        var logger = ThreadSafeLogger.Instance;
        logger.Log("Test message");

        // Задание 83
        var lazy = LazySingleton.Instance;
        lazy.DoWork();

        // Задание 85
        var config = ConfigurationManager.Instance;
        Console.WriteLine($"API URL: {config.GetValue("ApiUrl")}");

        // Задание 90
        var cache = CacheManager.Instance;
        cache.Set("key1", "value1");
        Console.WriteLine($"Cache value: {cache.Get<string>("key1")}");

        // Задание 92
        var random = RandomManager.Instance;
        Console.WriteLine($"Random number: {random.Next(1, 100)}");

        // Задание 94
        var security = SecurityManager.Instance;
        Console.WriteLine($"Admin authenticated: {security.Authenticate("admin", "admin123")}");

        // Задание 95
        var counter = PerformanceCounter.Instance;
        counter.Increment("requests");
        Console.WriteLine($"Request count: {counter.GetCount("requests")}");

        // Задание 96
        var emailService = EmailService.Instance;
        emailService.EnqueueMessage("test@example.com", "Subject", "Body");
        Console.WriteLine($"Emails in queue: {emailService.QueueLength}");

        // Задание 98
        var sessionManager = SessionManager.Instance;
        var sessionId = sessionManager.CreateSession("user1");
        Console.WriteLine($"Session valid: {sessionManager.IsValidSession(sessionId)}");

        // Задание 99
        var notificationService = NotificationService.Instance;
        notificationService.AddNotification("Test", "This is a test");
        Console.WriteLine($"Notifications: {notificationService.GetNotifications().Count()}");

        // Задание 100
        var resourcePool = ResourcePoolManager.Instance;
        resourcePool.CreatePool<string>("stringPool", 5, () => "default");
        Console.WriteLine("Resource pool created");

        Console.WriteLine();
    }
}
