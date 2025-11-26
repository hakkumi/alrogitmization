using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace CompleteEventsAndMVP
{
    // ==================== РАЗДЕЛ 1: СОБЫТИЯ (1-50) ====================

    #region Задания 1-50

    // 1. Создайте простое событие на основе делегата EventHandler
    public class SimpleEventClass
    {
        public event EventHandler SimpleEvent;

        public void RaiseSimpleEvent()
        {
            Console.WriteLine("1. Простое событие сработало");
            SimpleEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    // 2. Реализуйте класс с событием для оповещения об изменении данных
    public class DataWatcher
    {
        private string _data;

        public event EventHandler<string> DataChanged;

        public string Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    DataChanged?.Invoke(this, value);
                }
            }
        }
    }

    // 3. Создайте событие с пользовательским делегатом
    public class CustomDelegateExample
    {
        public delegate void CustomEventDelegate(string message, int priority);
        public event CustomEventDelegate CustomEvent;

        public void TriggerCustomEvent(string message, int priority)
        {
            CustomEvent?.Invoke(message, priority);
        }
    }

    // 4. Реализуйте подписку и отписку на событие
    public class SubscriptionManager
    {
        public event EventHandler ValueChanged;

        private EventHandler _handler1 = (s, e) => Console.WriteLine("4. Обработчик 1");
        private EventHandler _handler2 = (s, e) => Console.WriteLine("4. Обработчик 2");

        public void Subscribe()
        {
            ValueChanged += _handler1;
            ValueChanged += _handler2;
        }

        public void UnsubscribeFirst()
        {
            ValueChanged -= _handler1;
        }

        public void TriggerEvent()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // 5. Создайте класс Observable с событием OnPropertyChanged
    public class ObservableItem : INotifyPropertyChanged
    {
        private string _name;
        private int _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 6. Реализуйте событие, которое передает данные через EventArgs
    public class TemperatureEventArgs : EventArgs
    {
        public double OldTemperature { get; }
        public double NewTemperature { get; }
        public DateTime ChangeTime { get; }

        public TemperatureEventArgs(double oldTemp, double newTemp)
        {
            OldTemperature = oldTemp;
            NewTemperature = newTemp;
            ChangeTime = DateTime.Now;
        }
    }

    public class TemperatureSensor
    {
        private double _temperature;

        public event EventHandler<TemperatureEventArgs> TemperatureChanged;

        public double Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    var old = _temperature;
                    _temperature = value;
                    TemperatureChanged?.Invoke(this, new TemperatureEventArgs(old, value));
                }
            }
        }
    }

    // 7. Создайте систему уведомлений на основе событий
    public class NotificationService
    {
        public event EventHandler<string> NotificationEvent;

        public void SendNotification(string message)
        {
            NotificationEvent?.Invoke(this, message);
        }
    }

    // 8. Реализуйте событие для кнопки с параметром клика
    public class ButtonClickEventArgs : EventArgs
    {
        public string ButtonName { get; }
        public int ClickCount { get; }
        public DateTime ClickTime { get; }

        public ButtonClickEventArgs(string name, int count)
        {
            ButtonName = name;
            ClickCount = count;
            ClickTime = DateTime.Now;
        }
    }

    public class SmartButton
    {
        private int _clickCount;

        public event EventHandler<ButtonClickEventArgs> Clicked;

        public string Name { get; }

        public SmartButton(string name)
        {
            Name = name;
        }

        public void Click()
        {
            _clickCount++;
            Clicked?.Invoke(this, new ButtonClickEventArgs(Name, _clickCount));
        }
    }

    // 9. Создайте событие для отправки email при срабатывании
    public class EmailEventArgs : EventArgs
    {
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailEventArgs(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }

    public class EmailService
    {
        public event EventHandler<EmailEventArgs> EmailSent;

        public void SendEmail(string to, string subject, string body)
        {
            // Логика отправки email
            EmailSent?.Invoke(this, new EmailEventArgs(to, subject, body));
        }
    }

    // 10. Реализуйте цепочку обработчиков событий
    public class ProcessingChain
    {
        public event EventHandler<string> Step1Completed;
        public event EventHandler<string> Step2Completed;
        public event EventHandler<string> Step3Completed;

        public void ProcessData(string data)
        {
            Step1Completed?.Invoke(this, $"Step1: {data}");
            Step2Completed?.Invoke(this, $"Step2: {data}");
            Step3Completed?.Invoke(this, $"Step3: {data}");
        }
    }

    // 11. Создайте событие для логирования операций
    public class Logger
    {
        public event EventHandler<string> LogEntryCreated;

        public void Log(string message)
        {
            LogEntryCreated?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }
    }

    // 12. Реализуйте событие с обработкой исключений
    public class SafeEventPublisher
    {
        public event EventHandler<string> SafeEvent;

        public void RaiseSafeEvent(string data)
        {
            var handlers = SafeEvent?.GetInvocationList();
            if (handlers != null)
            {
                foreach (EventHandler<string> handler in handlers)
                {
                    try
                    {
                        handler(this, data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"12. Ошибка в обработчике: {ex.Message}");
                    }
                }
            }
        }
    }

    // 13. Создайте таймер с событием срабатывания
    public class CustomTimer
    {
        public event EventHandler TimerElapsed;
        private Timer _timer;

        public void Start(int intervalMs)
        {
            _timer = new Timer(_ => TimerElapsed?.Invoke(this, EventArgs.Empty),
                             null, intervalMs, Timeout.Infinite);
        }
    }

    // 14. Реализуйте событие для синхронизации данных
    public class DataSynchronizer
    {
        public event EventHandler<bool> SyncCompleted;

        public async Task SyncDataAsync()
        {
            await Task.Delay(1000); // Имитация синхронизации
            SyncCompleted?.Invoke(this, true);
        }
    }

    // 15. Создайте событие для начала и конца процесса
    public class ProcessManager
    {
        public event EventHandler ProcessStarted;
        public event EventHandler ProcessCompleted;

        public void ExecuteProcess()
        {
            ProcessStarted?.Invoke(this, EventArgs.Empty);
            // Логика процесса
            Thread.Sleep(500);
            ProcessCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    // 16. Реализуйте событие для изменения состояния приложения
    public class ApplicationState
    {
        private string _state;

        public event EventHandler<string> StateChanged;

        public string CurrentState
        {
            get => _state;
            set
            {
                _state = value;
                StateChanged?.Invoke(this, value);
            }
        }
    }

    // 17. Создайте событие для обновления UI элементов
    public class UIUpdater
    {
        public event EventHandler<string> UIUpdateRequired;

        public void RequestUpdate(string elementName)
        {
            UIUpdateRequired?.Invoke(this, elementName);
        }
    }

    // 18. Реализуйте событие для отслеживания изменения коллекции
    public class ObservableCollection<T>
    {
        private List<T> _items = new List<T>();

        public event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

        public void Add(T item)
        {
            _items.Add(item);
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs<T>("Added", item));
        }

        public void Remove(T item)
        {
            _items.Remove(item);
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs<T>("Removed", item));
        }
    }

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public string Action { get; }
        public T Item { get; }

        public CollectionChangedEventArgs(string action, T item)
        {
            Action = action;
            Item = item;
        }
    }

    // 19. Создайте событие для обработки пользовательского ввода
    public class InputHandler
    {
        public event EventHandler<string> InputReceived;

        public void ProcessInput(string input)
        {
            InputReceived?.Invoke(this, input);
        }
    }

    // 20. Реализуйте событие для работы с базой данных
    public class DatabaseService
    {
        public event EventHandler<string> QueryExecuted;
        public event EventHandler<string> ErrorOccurred;

        public void ExecuteQuery(string query)
        {
            try
            {
                // Логика выполнения запроса
                QueryExecuted?.Invoke(this, $"Query executed: {query}");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex.Message);
            }
        }
    }

    // 21. Создайте событие для отслеживания файловой системы
    public class FileWatcher
    {
        public event EventHandler<string> FileChanged;

        public void SimulateFileChange(string fileName)
        {
            FileChanged?.Invoke(this, fileName);
        }
    }

    // 22. Реализуйте событие для обработки ошибок в приложении
    public class ErrorHandler
    {
        public event EventHandler<Exception> ErrorOccurred;

        public void HandleError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
        }
    }

    // 23. Создайте событие для оповещения о прогрессе операции
    public class ProgressReporter
    {
        public event EventHandler<int> ProgressChanged;

        public void ReportProgress(int percent)
        {
            ProgressChanged?.Invoke(this, percent);
        }
    }

    // 24. Реализуйте событие для синхронизации потоков
    public class ThreadSynchronizer
    {
        public event EventHandler ThreadCompleted;

        public void ExecuteInThread()
        {
            new Thread(() =>
            {
                // Работа в потоке
                ThreadCompleted?.Invoke(this, EventArgs.Empty);
            }).Start();
        }
    }

    // 25. Создайте событие для отслеживания сетевых операций
    public class NetworkMonitor
    {
        public event EventHandler<bool> ConnectionStatusChanged;

        public void CheckConnection()
        {
            // Проверка соединения
            ConnectionStatusChanged?.Invoke(this, true);
        }
    }

    // 26. Реализуйте событие для работы с кешем
    public class CacheManager
    {
        public event EventHandler<string> CacheUpdated;

        public void UpdateCache(string key)
        {
            CacheUpdated?.Invoke(this, key);
        }
    }

    // 27. Создайте событие для отправки уведомлений
    public class NotificationManager
    {
        public event EventHandler<string> NotificationSent;

        public void SendNotification(string message)
        {
            NotificationSent?.Invoke(this, message);
        }
    }

    // 28. Реализуйте событие для изменения настроек приложения
    public class SettingsManager
    {
        public event EventHandler<string> SettingChanged;

        public void ChangeSetting(string settingName)
        {
            SettingChanged?.Invoke(this, settingName);
        }
    }

    // 29. Создайте событие для работы с платежами
    public class PaymentProcessor
    {
        public event EventHandler<decimal> PaymentProcessed;
        public event EventHandler<string> PaymentFailed;

        public void ProcessPayment(decimal amount)
        {
            if (amount > 0)
                PaymentProcessed?.Invoke(this, amount);
            else
                PaymentFailed?.Invoke(this, "Invalid amount");
        }
    }

    // 30. Реализуйте событие для аудита операций
    public class AuditService
    {
        public event EventHandler<string> AuditRecordCreated;

        public void CreateAuditRecord(string operation)
        {
            AuditRecordCreated?.Invoke(this, operation);
        }
    }

    // 31. Создайте событие OnClick для пользовательского контрола
    public class CustomControl
    {
        public event EventHandler<EventArgs> OnClick;

        public void SimulateClick()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }

    // 32. Реализуйте событие для обработки ввода клавиатуры
    public class KeyboardHandler
    {
        public event EventHandler<char> KeyPressed;

        public void SimulateKeyPress(char key)
        {
            KeyPressed?.Invoke(this, key);
        }
    }

    // 33. Создайте событие для работы с валидацией данных
    public class DataValidator
    {
        public event EventHandler<bool> ValidationCompleted;

        public void Validate(string data)
        {
            bool isValid = !string.IsNullOrEmpty(data);
            ValidationCompleted?.Invoke(this, isValid);
        }
    }

    // 34. Реализуйте событие для отслеживания производительности
    public class PerformanceMonitor
    {
        public event EventHandler<double> PerformanceMetricRecorded;

        public void RecordMetric(double value)
        {
            PerformanceMetricRecorded?.Invoke(this, value);
        }
    }

    // 35. Создайте событие для работы с API запросами
    public class ApiClient
    {
        public event EventHandler<string> ApiCallCompleted;

        public async Task MakeApiCallAsync(string endpoint)
        {
            await Task.Delay(100);
            ApiCallCompleted?.Invoke(this, $"Call to {endpoint} completed");
        }
    }

    // 36. Реализуйте событие для изменения видимости элемента
    public class VisibilityManager
    {
        public event EventHandler<bool> VisibilityChanged;

        public void SetVisible(bool visible)
        {
            VisibilityChanged?.Invoke(this, visible);
        }
    }

    // 37. Создайте событие для работы с правами доступа
    public class PermissionService
    {
        public event EventHandler<string> PermissionChanged;

        public void ChangePermission(string permission)
        {
            PermissionChanged?.Invoke(this, permission);
        }
    }

    // 38. Реализуйте событие для отслеживания активности пользователя
    public class UserActivityTracker
    {
        public event EventHandler<string> UserActivityRecorded;

        public void RecordActivity(string activity)
        {
            UserActivityRecorded?.Invoke(this, activity);
        }
    }

    // 39. Создайте событие для работы с кэшированием результатов
    public class ResultCache
    {
        public event EventHandler<string> ResultCached;

        public void CacheResult(string key)
        {
            ResultCached?.Invoke(this, key);
        }
    }

    // 40. Реализуйте событие для оповещения об окончании асинхронной операции
    public class AsyncOperation
    {
        public event EventHandler<string> AsyncOperationCompleted;

        public async Task ExecuteAsync()
        {
            await Task.Delay(1000);
            AsyncOperationCompleted?.Invoke(this, "Operation completed");
        }
    }

    // 41. Создайте событие для работы с сортировкой и фильтрацией
    public class DataFilter
    {
        public event EventHandler<string> FilterApplied;

        public void ApplyFilter(string filter)
        {
            FilterApplied?.Invoke(this, filter);
        }
    }

    // 42. Реализуйте событие для отслеживания состояния сессии
    public class SessionManager
    {
        public event EventHandler<string> SessionStateChanged;

        public void ChangeSessionState(string state)
        {
            SessionStateChanged?.Invoke(this, state);
        }
    }

    // 43. Создайте событие для работы с уведомлениями в реальном времени
    public class RealTimeNotifier
    {
        public event EventHandler<string> RealTimeNotification;

        public void SendRealTimeNotification(string message)
        {
            RealTimeNotification?.Invoke(this, message);
        }
    }

    // 44. Реализуйте событие для отслеживания изменений в конфигурации
    public class ConfigurationManager
    {
        public event EventHandler<string> ConfigurationChanged;

        public void ChangeConfiguration(string config)
        {
            ConfigurationChanged?.Invoke(this, config);
        }
    }

    // 45. Создайте событие для работы с историей операций
    public class HistoryService
    {
        public event EventHandler<string> HistoryRecordAdded;

        public void AddHistoryRecord(string record)
        {
            HistoryRecordAdded?.Invoke(this, record);
        }
    }

    // 46. Реализуйте событие для отправки аналитических данных
    public class AnalyticsService
    {
        public event EventHandler<string> AnalyticsEventSent;

        public void SendAnalytics(string eventName)
        {
            AnalyticsEventSent?.Invoke(this, eventName);
        }
    }

    // 47. Создайте событие для работы с шаблонами документов
    public class TemplateEngine
    {
        public event EventHandler<string> TemplateProcessed;

        public void ProcessTemplate(string templateName)
        {
            TemplateProcessed?.Invoke(this, templateName);
        }
    }

    // 48. Реализуйте событие для отслеживания состояния подключения
    public class ConnectionMonitor
    {
        public event EventHandler<bool> ConnectionStateChanged;

        public void SetConnectionState(bool connected)
        {
            ConnectionStateChanged?.Invoke(this, connected);
        }
    }

    // 49. Создайте событие для работы с интеграцией сервисов
    public class ServiceIntegrator
    {
        public event EventHandler<string> ServiceIntegrated;

        public void IntegrateService(string serviceName)
        {
            ServiceIntegrated?.Invoke(this, serviceName);
        }
    }

    // 50. Реализуйте событие для отслеживания фаз жизненного цикла приложения
    public class LifecycleManager
    {
        public event EventHandler<string> LifecyclePhaseChanged;

        public void ChangePhase(string phase)
        {
            LifecyclePhaseChanged?.Invoke(this, phase);
        }
    }

    #endregion

    // ==================== РАЗДЕЛ 2: MVP ПАТТЕРН (51-100) ====================

    #region Задания 51-100

    // 51. Создайте базовую MVP архитектуру с View, Presenter, Model
    public interface ICalculatorView
    {
        void DisplayResult(double result);
        void DisplayError(string message);
    }

    public class CalculatorModel
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b != 0 ? a / b : throw new DivideByZeroException();
    }

    public class CalculatorPresenter
    {
        private readonly ICalculatorView _view;
        private readonly CalculatorModel _model;

        public CalculatorPresenter(ICalculatorView view)
        {
            _view = view;
            _model = new CalculatorModel();
        }

        public void Add(double a, double b)
        {
            try
            {
                var result = _model.Add(a, b);
                _view.DisplayResult(result);
            }
            catch (Exception ex)
            {
                _view.DisplayError(ex.Message);
            }
        }
    }

    // 52. Реализуйте View интерфейс для отделения представления от логики
    public interface IUserView
    {
        void DisplayUserInfo(string info);
        void ShowMessage(string message);
    }

    // 53. Создайте Presenter класс для управления логикой приложения
    public class UserPresenter
    {
        private readonly IUserView _view;
        private UserModel _model;

        public UserPresenter(IUserView view)
        {
            _view = view;
            _model = new UserModel();
        }

        public void LoadUser()
        {
            var user = _model.GetUser();
            _view.DisplayUserInfo($"User: {user.Name}, Age: {user.Age}");
        }
    }

    // 54. Реализуйте Model для хранения и обработки данных
    public class UserModel
    {
        public User GetUser() => new User { Name = "John", Age = 30 };
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    // 55. Создайте MVP для примера калькулятора
    public class CalculatorMVP
    {
        public class Model
        {
            public double Calculate(string operation, double a, double b)
            {
                return operation switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" when b != 0 => a / b,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        public interface IView
        {
            void ShowResult(double result);
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly Model _model;

            public Presenter(IView view)
            {
                _view = view;
                _model = new Model();
            }

            public void Calculate(string op, double a, double b)
            {
                try
                {
                    var result = _model.Calculate(op, a, b);
                    _view.ShowResult(result);
                }
                catch
                {
                    _view.ShowResult(double.NaN);
                }
            }
        }
    }

    // 56. Реализуйте MVP для управления списком пользователей
    public class UserListMVP
    {
        public class UserModel
        {
            private List<string> _users = new List<string>();

            public void AddUser(string user) => _users.Add(user);
            public List<string> GetUsers() => _users;
        }

        public interface IUserListView
        {
            void DisplayUsers(List<string> users);
        }

        public class UserListPresenter
        {
            private readonly IUserListView _view;
            private readonly UserModel _model;

            public UserListPresenter(IUserListView view)
            {
                _view = view;
                _model = new UserModel();
            }

            public void AddUser(string user)
            {
                _model.AddUser(user);
                _view.DisplayUsers(_model.GetUsers());
            }
        }
    }

    // 57. Создайте MVP с двусторонней привязкой данных
    public class TwoWayBindingMVP
    {
        public class Model
        {
            public string Data { get; set; }
        }

        public interface IView
        {
            string InputValue { get; set; }
            event EventHandler<string> ValueChanged;
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly Model _model;

            public Presenter(IView view)
            {
                _view = view;
                _model = new Model();
                _view.ValueChanged += OnValueChanged;
            }

            private void OnValueChanged(object sender, string value)
            {
                _model.Data = value;
                _view.InputValue = value; // Обновление view
            }
        }
    }

    // 58. Реализуйте MVP для формы регистрации
    public class RegistrationMVP
    {
        public class RegistrationModel
        {
            public bool Register(string username, string email, string password)
            {
                return !string.IsNullOrEmpty(username) &&
                       !string.IsNullOrEmpty(email) &&
                       !string.IsNullOrEmpty(password);
            }
        }

        public interface IRegistrationView
        {
            string Username { get; }
            string Email { get; }
            string Password { get; }
            void ShowSuccess();
            void ShowError(string message);
        }

        public class RegistrationPresenter
        {
            private readonly IRegistrationView _view;
            private readonly RegistrationModel _model;

            public RegistrationPresenter(IRegistrationView view)
            {
                _view = view;
                _model = new RegistrationModel();
            }

            public void Register()
            {
                var success = _model.Register(_view.Username, _view.Email, _view.Password);
                if (success)
                    _view.ShowSuccess();
                else
                    _view.ShowError("Invalid data");
            }
        }
    }

    // 59. Создайте MVP для отображения таблицы с данными
    public class DataTableMVP
    {
        public class TableModel
        {
            public List<string[]> GetData() => new List<string[]>
            {
                new[] { "1", "John", "30" },
                new[] { "2", "Alice", "25" }
            };
        }

        public interface ITableView
        {
            void DisplayData(List<string[]> data);
        }

        public class TablePresenter
        {
            private readonly ITableView _view;
            private readonly TableModel _model;

            public TablePresenter(ITableView view)
            {
                _view = view;
                _model = new TableModel();
            }

            public void LoadData()
            {
                _view.DisplayData(_model.GetData());
            }
        }
    }

    // 60. Реализуйте MVP для работы с шопингом-листом
    public class ShoppingListMVP
    {
        public class ShoppingModel
        {
            private List<string> _items = new List<string>();

            public void AddItem(string item) => _items.Add(item);
            public void RemoveItem(string item) => _items.Remove(item);
            public List<string> GetItems() => _items;
        }

        public interface IShoppingView
        {
            void UpdateList(List<string> items);
        }

        public class ShoppingPresenter
        {
            private readonly IShoppingView _view;
            private readonly ShoppingModel _model;

            public ShoppingPresenter(IShoppingView view)
            {
                _view = view;
                _model = new ShoppingModel();
            }

            public void AddItem(string item)
            {
                _model.AddItem(item);
                _view.UpdateList(_model.GetItems());
            }
        }
    }

    // 61. Создайте MVP для приложения управления заметками
    public class NotesMVP
    {
        public class NoteModel
        {
            private List<string> _notes = new List<string>();

            public void AddNote(string note) => _notes.Add(note);
            public List<string> GetNotes() => _notes;
        }

        public interface INotesView
        {
            void DisplayNotes(List<string> notes);
        }

        public class NotesPresenter
        {
            private readonly INotesView _view;
            private readonly NoteModel _model;

            public NotesPresenter(INotesView view)
            {
                _view = view;
                _model = new NoteModel();
            }

            public void AddNote(string note)
            {
                _model.AddNote(note);
                _view.DisplayNotes(_model.GetNotes());
            }
        }
    }

    // 62. Реализуйте MVP для работы с фильтрацией и сортировкой
    public class FilterSortMVP
    {
        public class DataModel
        {
            public List<int> GetData() => new List<int> { 5, 2, 8, 1, 9 };
            public List<int> SortData(List<int> data) => data.OrderBy(x => x).ToList();
            public List<int> FilterData(List<int> data, Func<int, bool> predicate) => data.Where(predicate).ToList();
        }

        public interface IFilterSortView
        {
            void DisplayData(List<int> data);
        }

        public class FilterSortPresenter
        {
            private readonly IFilterSortView _view;
            private readonly DataModel _model;

            public FilterSortPresenter(IFilterSortView view)
            {
                _view = view;
                _model = new DataModel();
            }

            public void LoadSortedData()
            {
                var data = _model.GetData();
                _view.DisplayData(_model.SortData(data));
            }
        }
    }

    // 63. Создайте MVP для приложения управления контактами
    public class ContactsMVP
    {
        public class Contact
        {
            public string Name { get; set; }
            public string Phone { get; set; }
        }

        public class ContactsModel
        {
            private List<Contact> _contacts = new List<Contact>();

            public void AddContact(Contact contact) => _contacts.Add(contact);
            public List<Contact> GetContacts() => _contacts;
        }

        public interface IContactsView
        {
            void DisplayContacts(List<Contact> contacts);
        }

        public class ContactsPresenter
        {
            private readonly IContactsView _view;
            private readonly ContactsModel _model;

            public ContactsPresenter(IContactsView view)
            {
                _view = view;
                _model = new ContactsModel();
            }

            public void AddContact(string name, string phone)
            {
                _model.AddContact(new Contact { Name = name, Phone = phone });
                _view.DisplayContacts(_model.GetContacts());
            }
        }
    }

    // 64. Реализуйте MVP с использованием событий для взаимодействия
    public class EventBasedMVP
    {
        public class Model
        {
            public event EventHandler<string> DataUpdated;

            public void UpdateData(string data)
            {
                DataUpdated?.Invoke(this, data);
            }
        }

        public interface IView
        {
            void ShowData(string data);
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly Model _model;

            public Presenter(IView view, Model model)
            {
                _view = view;
                _model = model;
                _model.DataUpdated += (s, e) => _view.ShowData(e);
            }

            public void ChangeData(string data)
            {
                _model.UpdateData(data);
            }
        }
    }

    // 65. Создайте MVP для отображения и редактирования профиля пользователя
    public class ProfileMVP
    {
        public class ProfileModel
        {
            public string Name { get; set; } = "John Doe";
            public string Email { get; set; } = "john@example.com";
        }

        public interface IProfileView
        {
            void DisplayProfile(string name, string email);
        }

        public class ProfilePresenter
        {
            private readonly IProfileView _view;
            private readonly ProfileModel _model;

            public ProfilePresenter(IProfileView view)
            {
                _view = view;
                _model = new ProfileModel();
            }

            public void LoadProfile()
            {
                _view.DisplayProfile(_model.Name, _model.Email);
            }
        }
    }

    // 66. Реализуйте MVP для работы с настройками приложения
    public class SettingsMVP
    {
        public class SettingsModel
        {
            public string Theme { get; set; } = "Light";
            public string Language { get; set; } = "English";
        }

        public interface ISettingsView
        {
            void DisplaySettings(string theme, string language);
        }

        public class SettingsPresenter
        {
            private readonly ISettingsView _view;
            private readonly SettingsModel _model;

            public SettingsPresenter(ISettingsView view)
            {
                _view = view;
                _model = new SettingsModel();
            }

            public void LoadSettings()
            {
                _view.DisplaySettings(_model.Theme, _model.Language);
            }
        }
    }

    // 67. Создайте MVP для навигации между экранами
    public class NavigationMVP
    {
        public interface INavigationView
        {
            void ShowScreen(string screenName);
        }

        public class NavigationPresenter
        {
            private readonly INavigationView _view;

            public NavigationPresenter(INavigationView view)
            {
                _view = view;
            }

            public void NavigateTo(string screen)
            {
                _view.ShowScreen(screen);
            }
        }
    }

    // 68. Реализуйте MVP для работы с поиском и фильтрацией
    public class SearchMVP
    {
        public class SearchModel
        {
            public List<string> Search(string query, List<string> data)
            {
                return data.Where(x => x.Contains(query)).ToList();
            }
        }

        public interface ISearchView
        {
            void DisplayResults(List<string> results);
        }

        public class SearchPresenter
        {
            private readonly ISearchView _view;
            private readonly SearchModel _model;

            public SearchPresenter(ISearchView view)
            {
                _view = view;
                _model = new SearchModel();
            }

            public void Search(string query, List<string> data)
            {
                var results = _model.Search(query, data);
                _view.DisplayResults(results);
            }
        }
    }

    // 69. Создайте MVP для приложения управления проектами
    public class ProjectManagementMVP
    {
        public class Project
        {
            public string Name { get; set; }
            public string Status { get; set; }
        }

        public class ProjectModel
        {
            private List<Project> _projects = new List<Project>();

            public void AddProject(Project project) => _projects.Add(project);
            public List<Project> GetProjects() => _projects;
        }

        public interface IProjectView
        {
            void DisplayProjects(List<Project> projects);
        }

        public class ProjectPresenter
        {
            private readonly IProjectView _view;
            private readonly ProjectModel _model;

            public ProjectPresenter(IProjectView view)
            {
                _view = view;
                _model = new ProjectModel();
            }

            public void AddProject(string name, string status)
            {
                _model.AddProject(new Project { Name = name, Status = status });
                _view.DisplayProjects(_model.GetProjects());
            }
        }
    }

    // 70. Реализуйте MVP для работы с корзиной покупок
    public class ShoppingCartMVP
    {
        public class CartItem
        {
            public string Product { get; set; }
            public decimal Price { get; set; }
        }

        public class CartModel
        {
            private List<CartItem> _items = new List<CartItem>();
            public void AddItem(CartItem item) => _items.Add(item);
            public List<CartItem> GetItems() => _items;
            public decimal GetTotal() => _items.Sum(x => x.Price);
        }

        public interface ICartView
        {
            void DisplayCart(List<CartItem> items, decimal total);
        }

        public class CartPresenter
        {
            private readonly ICartView _view;
            private readonly CartModel _model;

            public CartPresenter(ICartView view)
            {
                _view = view;
                _model = new CartModel();
            }

            public void AddItem(string product, decimal price)
            {
                _model.AddItem(new CartItem { Product = product, Price = price });
                _view.DisplayCart(_model.GetItems(), _model.GetTotal());
            }
        }
    }

    // 71. Создайте MVP для отображения новостей
    public class NewsMVP
    {
        public class NewsItem
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class NewsModel
        {
            public List<NewsItem> GetNews() => new List<NewsItem>
            {
                new NewsItem { Title = "Breaking News", Content = "Important event" },
                new NewsItem { Title = "Weather", Content = "Sunny day" }
            };
        }

        public interface INewsView
        {
            void DisplayNews(List<NewsItem> news);
        }

        public class NewsPresenter
        {
            private readonly INewsView _view;
            private readonly NewsModel _model;

            public NewsPresenter(INewsView view)
            {
                _view = view;
                _model = new NewsModel();
            }

            public void LoadNews()
            {
                _view.DisplayNews(_model.GetNews());
            }
        }
    }

    // 72. Реализуйте MVP для работы с чатом
    public class ChatMVP
    {
        public class Message
        {
            public string Text { get; set; }
            public string Sender { get; set; }
        }

        public class ChatModel
        {
            private List<Message> _messages = new List<Message>();

            public void AddMessage(Message message) => _messages.Add(message);
            public List<Message> GetMessages() => _messages;
        }

        public interface IChatView
        {
            void DisplayMessages(List<Message> messages);
        }

        public class ChatPresenter
        {
            private readonly IChatView _view;
            private readonly ChatModel _model;

            public ChatPresenter(IChatView view)
            {
                _view = view;
                _model = new ChatModel();
            }

            public void SendMessage(string text, string sender)
            {
                _model.AddMessage(new Message { Text = text, Sender = sender });
                _view.DisplayMessages(_model.GetMessages());
            }
        }
    }

    // 73. Создайте MVP для приложения управления расходами
    public class ExpensesMVP
    {
        public class Expense
        {
            public string Description { get; set; }
            public decimal Amount { get; set; }
        }

        public class ExpensesModel
        {
            private List<Expense> _expenses = new List<Expense>();

            public void AddExpense(Expense expense) => _expenses.Add(expense);
            public List<Expense> GetExpenses() => _expenses;
            public decimal GetTotal() => _expenses.Sum(x => x.Amount);
        }

        public interface IExpensesView
        {
            void DisplayExpenses(List<Expense> expenses, decimal total);
        }

        public class ExpensesPresenter
        {
            private readonly IExpensesView _view;
            private readonly ExpensesModel _model;

            public ExpensesPresenter(IExpensesView view)
            {
                _view = view;
                _model = new ExpensesModel();
            }

            public void AddExpense(string description, decimal amount)
            {
                _model.AddExpense(new Expense { Description = description, Amount = amount });
                _view.DisplayExpenses(_model.GetExpenses(), _model.GetTotal());
            }
        }
    }

    // 74. Реализуйте MVP для работы с галереей изображений
    public class GalleryMVP
    {
        public class ImageModel
        {
            public List<string> GetImages() => new List<string> { "image1.jpg", "image2.jpg" };
        }

        public interface IGalleryView
        {
            void DisplayImages(List<string> images);
        }

        public class GalleryPresenter
        {
            private readonly IGalleryView _view;
            private readonly ImageModel _model;

            public GalleryPresenter(IGalleryView view)
            {
                _view = view;
                _model = new ImageModel();
            }

            public void LoadImages()
            {
                _view.DisplayImages(_model.GetImages());
            }
        }
    }

    // 75. Создайте MVP для приложения управления задачами
    public class TaskManagementMVP
    {
        public class TaskItem
        {
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class TaskModel
        {
            private List<TaskItem> _tasks = new List<TaskItem>();

            public void AddTask(TaskItem task) => _tasks.Add(task);
            public List<TaskItem> GetTasks() => _tasks;
        }

        public interface ITaskView
        {
            void DisplayTasks(List<TaskItem> tasks);
        }

        public class TaskPresenter
        {
            private readonly ITaskView _view;
            private readonly TaskModel _model;

            public TaskPresenter(ITaskView view)
            {
                _view = view;
                _model = new TaskModel();
            }

            public void AddTask(string description)
            {
                _model.AddTask(new TaskItem { Description = description });
                _view.DisplayTasks(_model.GetTasks());
            }
        }
    }

    // 76. Реализуйте MVP для работы с аутентификацией
    public class AuthMVP
    {
        public class AuthModel
        {
            public bool Authenticate(string username, string password)
            {
                return username == "admin" && password == "password";
            }
        }

        public interface IAuthView
        {
            void ShowAuthResult(bool success);
        }

        public class AuthPresenter
        {
            private readonly IAuthView _view;
            private readonly AuthModel _model;

            public AuthPresenter(IAuthView view)
            {
                _view = view;
                _model = new AuthModel();
            }

            public void Authenticate(string username, string password)
            {
                var result = _model.Authenticate(username, password);
                _view.ShowAuthResult(result);
            }
        }
    }

    // 77. Создайте MVP для отображения графиков и статистики
    public class ChartsMVP
    {
        public class ChartModel
        {
            public List<int> GetData() => new List<int> { 10, 20, 30, 40, 50 };
        }

        public interface IChartView
        {
            void DisplayChart(List<int> data);
        }

        public class ChartPresenter
        {
            private readonly IChartView _view;
            private readonly ChartModel _model;

            public ChartPresenter(IChartView view)
            {
                _view = view;
                _model = new ChartModel();
            }

            public void LoadChart()
            {
                _view.DisplayChart(_model.GetData());
            }
        }
    }

    // 78. Реализуйте MVP для работы с рецептами
    public class RecipesMVP
    {
        public class Recipe
        {
            public string Name { get; set; }
            public string Ingredients { get; set; }
        }

        public class RecipeModel
        {
            private List<Recipe> _recipes = new List<Recipe>();

            public void AddRecipe(Recipe recipe) => _recipes.Add(recipe);
            public List<Recipe> GetRecipes() => _recipes;
        }

        public interface IRecipeView
        {
            void DisplayRecipes(List<Recipe> recipes);
        }

        public class RecipePresenter
        {
            private readonly IRecipeView _view;
            private readonly RecipeModel _model;

            public RecipePresenter(IRecipeView view)
            {
                _view = view;
                _model = new RecipeModel();
            }

            public void AddRecipe(string name, string ingredients)
            {
                _model.AddRecipe(new Recipe { Name = name, Ingredients = ingredients });
                _view.DisplayRecipes(_model.GetRecipes());
            }
        }
    }

    // 79. Создайте MVP для приложения планирования
    public class PlanningMVP
    {
        public class Plan
        {
            public string Task { get; set; }
            public DateTime Date { get; set; }
        }

        public class PlanModel
        {
            private List<Plan> _plans = new List<Plan>();

            public void AddPlan(Plan plan) => _plans.Add(plan);
            public List<Plan> GetPlans() => _plans;
        }

        public interface IPlanView
        {
            void DisplayPlans(List<Plan> plans);
        }

        public class PlanPresenter
        {
            private readonly IPlanView _view;
            private readonly PlanModel _model;

            public PlanPresenter(IPlanView view)
            {
                _view = view;
                _model = new PlanModel();
            }

            public void AddPlan(string task, DateTime date)
            {
                _model.AddPlan(new Plan { Task = task, Date = date });
                _view.DisplayPlans(_model.GetPlans());
            }
        }
    }

    // 80. Реализуйте MVP для работы с букингом
    public class BookingMVP
    {
        public class Booking
        {
            public string Service { get; set; }
            public DateTime Time { get; set; }
        }

        public class BookingModel
        {
            private List<Booking> _bookings = new List<Booking>();

            public void AddBooking(Booking booking) => _bookings.Add(booking);
            public List<Booking> GetBookings() => _bookings;
        }

        public interface IBookingView
        {
            void DisplayBookings(List<Booking> bookings);
        }

        public class BookingPresenter
        {
            private readonly IBookingView _view;
            private readonly BookingModel _model;

            public BookingPresenter(IBookingView view)
            {
                _view = view;
                _model = new BookingModel();
            }

            public void AddBooking(string service, DateTime time)
            {
                _model.AddBooking(new Booking { Service = service, Time = time });
                _view.DisplayBookings(_model.GetBookings());
            }
        }
    }

    // 81. Создайте MVP для отображения меню ресторана
    public class MenuMVP
    {
        public class MenuItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class MenuModel
        {
            private List<MenuItem> _items = new List<MenuItem>();

            public void AddItem(MenuItem item) => _items.Add(item);
            public List<MenuItem> GetMenu() => _items;
        }

        public interface IMenuView
        {
            void DisplayMenu(List<MenuItem> menu);
        }

        public class MenuPresenter
        {
            private readonly IMenuView _view;
            private readonly MenuModel _model;

            public MenuPresenter(IMenuView view)
            {
                _view = view;
                _model = new MenuModel();
            }

            public void AddMenuItem(string name, decimal price)
            {
                _model.AddItem(new MenuItem { Name = name, Price = price });
                _view.DisplayMenu(_model.GetMenu());
            }
        }
    }

    // 82. Реализуйте MVP для работы с ивентами
    public class EventsMVP
    {
        public class Event
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        public class EventModel
        {
            private List<Event> _events = new List<Event>();

            public void AddEvent(Event eventItem) => _events.Add(eventItem);
            public List<Event> GetEvents() => _events;
        }

        public interface IEventView
        {
            void DisplayEvents(List<Event> events);
        }

        public class EventPresenter
        {
            private readonly IEventView _view;
            private readonly EventModel _model;

            public EventPresenter(IEventView view)
            {
                _view = view;
                _model = new EventModel();
            }

            public void AddEvent(string name, DateTime date)
            {
                _model.AddEvent(new Event { Name = name, Date = date });
                _view.DisplayEvents(_model.GetEvents());
            }
        }
    }

    // 83. Создайте MVP для приложения рейтингов фильмов
    public class MovieRatingMVP
    {
        public class Movie
        {
            public string Title { get; set; }
            public double Rating { get; set; }
        }

        public class MovieModel
        {
            private List<Movie> _movies = new List<Movie>();

            public void AddMovie(Movie movie) => _movies.Add(movie);
            public List<Movie> GetMovies() => _movies;
            public void RateMovie(string title, double rating)
            {
                var movie = _movies.FirstOrDefault(m => m.Title == title);
                if (movie != null) movie.Rating = rating;
            }
        }

        public interface IMovieView
        {
            void DisplayMovies(List<Movie> movies);
        }

        public class MoviePresenter
        {
            private readonly IMovieView _view;
            private readonly MovieModel _model;

            public MoviePresenter(IMovieView view)
            {
                _view = view;
                _model = new MovieModel();
            }

            public void AddMovie(string title, double rating)
            {
                _model.AddMovie(new Movie { Title = title, Rating = rating });
                _view.DisplayMovies(_model.GetMovies());
            }
        }
    }

    // 84. Реализуйте MVP для работы с переводом денег
    public class MoneyTransferMVP
    {
        public class TransferModel
        {
            public bool Transfer(string from, string to, decimal amount)
            {
                return amount > 0 && !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to);
            }
        }

        public interface ITransferView
        {
            void ShowTransferResult(bool success);
        }

        public class TransferPresenter
        {
            private readonly ITransferView _view;
            private readonly TransferModel _model;

            public TransferPresenter(ITransferView view)
            {
                _view = view;
                _model = new TransferModel();
            }

            public void TransferMoney(string from, string to, decimal amount)
            {
                var result = _model.Transfer(from, to, amount);
                _view.ShowTransferResult(result);
            }
        }
    }

    // 85. Создайте MVP для отображения погоды
    public class WeatherMVP
    {
        public class WeatherModel
        {
            public string GetWeather() => "Sunny, 25°C";
        }

        public interface IWeatherView
        {
            void DisplayWeather(string weather);
        }

        public class WeatherPresenter
        {
            private readonly IWeatherView _view;
            private readonly WeatherModel _model;

            public WeatherPresenter(IWeatherView view)
            {
                _view = view;
                _model = new WeatherModel();
            }

            public void LoadWeather()
            {
                _view.DisplayWeather(_model.GetWeather());
            }
        }
    }

    // 86. Реализуйте MVP для работы с маршрутами
    public class RoutingMVP
    {
        public class RouteModel
        {
            public string CalculateRoute(string from, string to)
            {
                return $"Route from {from} to {to}";
            }
        }

        public interface IRouteView
        {
            void DisplayRoute(string route);
        }

        public class RoutePresenter
        {
            private readonly IRouteView _view;
            private readonly RouteModel _model;

            public RoutePresenter(IRouteView view)
            {
                _view = view;
                _model = new RouteModel();
            }

            public void CalculateRoute(string from, string to)
            {
                var route = _model.CalculateRoute(from, to);
                _view.DisplayRoute(route);
            }
        }
    }

    // 87. Создайте MVP для приложения фитнес-трекер
    public class FitnessMVP
    {
        public class FitnessData
        {
            public int Steps { get; set; }
            public double Distance { get; set; }
        }

        public class FitnessModel
        {
            public FitnessData GetData() => new FitnessData { Steps = 10000, Distance = 8.5 };
        }

        public interface IFitnessView
        {
            void DisplayFitnessData(FitnessData data);
        }

        public class FitnessPresenter
        {
            private readonly IFitnessView _view;
            private readonly FitnessModel _model;

            public FitnessPresenter(IFitnessView view)
            {
                _view = view;
                _model = new FitnessModel();
            }

            public void LoadData()
            {
                _view.DisplayFitnessData(_model.GetData());
            }
        }
    }

    // 88. Реализуйте MVP для работы с медиа-плеером
    public class MediaPlayerMVP
    {
        public class MediaModel
        {
            public string CurrentTrack { get; set; } = "Track 1";
            public bool IsPlaying { get; set; }
        }

        public interface IMediaView
        {
            void UpdatePlayerState(string track, bool isPlaying);
        }

        public class MediaPresenter
        {
            private readonly IMediaView _view;
            private readonly MediaModel _model;

            public MediaPresenter(IMediaView view)
            {
                _view = view;
                _model = new MediaModel();
            }

            public void Play()
            {
                _model.IsPlaying = true;
                _view.UpdatePlayerState(_model.CurrentTrack, _model.IsPlaying);
            }
        }
    }

    // 89. Создайте MVP для приложения переводчика
    public class TranslatorMVP
    {
        public class TranslatorModel
        {
            public string Translate(string text, string toLanguage)
            {
                return $"Translated '{text}' to {toLanguage}";
            }
        }

        public interface ITranslatorView
        {
            void DisplayTranslation(string result);
        }

        public class TranslatorPresenter
        {
            private readonly ITranslatorView _view;
            private readonly TranslatorModel _model;

            public TranslatorPresenter(ITranslatorView view)
            {
                _view = view;
                _model = new TranslatorModel();
            }

            public void Translate(string text, string language)
            {
                var result = _model.Translate(text, language);
                _view.DisplayTranslation(result);
            }
        }
    }

    // 90. Реализуйте MVP для работы с календарем
    public class CalendarMVP
    {
        public class CalendarModel
        {
            public List<string> GetEvents(DateTime date)
            {
                return new List<string> { "Meeting at 10:00", "Lunch at 13:00" };
            }
        }

        public interface ICalendarView
        {
            void DisplayEvents(List<string> events);
        }

        public class CalendarPresenter
        {
            private readonly ICalendarView _view;
            private readonly CalendarModel _model;

            public CalendarPresenter(ICalendarView view)
            {
                _view = view;
                _model = new CalendarModel();
            }

            public void LoadEvents(DateTime date)
            {
                _view.DisplayEvents(_model.GetEvents(date));
            }
        }
    }

    // 91. Создайте MVP с инъекцией зависимостей (Dependency Injection)
    public class DIMVP
    {
        public interface IService
        {
            string GetData();
        }

        public class Service : IService
        {
            public string GetData() => "Data from service";
        }

        public class Model
        {
            private readonly IService _service;

            public Model(IService service)
            {
                _service = service;
            }

            public string ProcessData() => _service.GetData();
        }

        public interface IView
        {
            void ShowData(string data);
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly Model _model;

            public Presenter(IView view, Model model)
            {
                _view = view;
                _model = model;
            }

            public void LoadData()
            {
                _view.ShowData(_model.ProcessData());
            }
        }
    }

    // 92. Реализуйте MVP с использованием Repository паттерна
    public class RepositoryMVP
    {
        public interface IRepository<T>
        {
            void Add(T item);
            List<T> GetAll();
        }

        public class UserRepository : IRepository<string>
        {
            private List<string> _users = new List<string>();

            public void Add(string user) => _users.Add(user);
            public List<string> GetAll() => _users;
        }

        public class Model
        {
            private readonly IRepository<string> _repository;

            public Model(IRepository<string> repository)
            {
                _repository = repository;
            }

            public void AddUser(string user) => _repository.Add(user);
            public List<string> GetUsers() => _repository.GetAll();
        }

        public interface IView
        {
            void DisplayUsers(List<string> users);
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly Model _model;

            public Presenter(IView view, Model model)
            {
                _view = view;
                _model = model;
            }

            public void AddUser(string user)
            {
                _model.AddUser(user);
                _view.DisplayUsers(_model.GetUsers());
            }
        }
    }

    // 93. Создайте MVP для работы с асинхронными операциями
    public class AsyncMVP
    {
        public class AsyncModel
        {
            public async Task<string> GetDataAsync()
            {
                await Task.Delay(1000);
                return "Async data";
            }
        }

        public interface IAsyncView
        {
            void DisplayData(string data);
            void ShowLoading();
            void HideLoading();
        }

        public class AsyncPresenter
        {
            private readonly IAsyncView _view;
            private readonly AsyncModel _model;

            public AsyncPresenter(IAsyncView view)
            {
                _view = view;
                _model = new AsyncModel();
            }

            public async Task LoadDataAsync()
            {
                _view.ShowLoading();
                var data = await _model.GetDataAsync();
                _view.DisplayData(data);
                _view.HideLoading();
            }
        }
    }

    // 94. Реализуйте MVP с кешированием данных
    public class CachingMVP
    {
        public class CacheModel
        {
            private string _cachedData;
            private DateTime _cacheTime;

            public string GetData()
            {
                if (_cachedData == null || (DateTime.Now - _cacheTime).TotalMinutes > 5)
                {
                    _cachedData = "Fresh data";
                    _cacheTime = DateTime.Now;
                }
                return _cachedData;
            }
        }

        public interface ICacheView
        {
            void DisplayData(string data);
        }

        public class CachePresenter
        {
            private readonly ICacheView _view;
            private readonly CacheModel _model;

            public CachePresenter(ICacheView view)
            {
                _view = view;
                _model = new CacheModel();
            }

            public void LoadData()
            {
                _view.DisplayData(_model.GetData());
            }
        }
    }

    // 95. Создайте MVP для работы с множественными источниками данных
    public class MultiSourceMVP
    {
        public class MultiSourceModel
        {
            public string GetLocalData() => "Local data";
            public string GetRemoteData() => "Remote data";
        }

        public interface IMultiSourceView
        {
            void DisplayData(string localData, string remoteData);
        }

        public class MultiSourcePresenter
        {
            private readonly IMultiSourceView _view;
            private readonly MultiSourceModel _model;

            public MultiSourcePresenter(IMultiSourceView view)
            {
                _view = view;
                _model = new MultiSourceModel();
            }

            public void LoadAllData()
            {
                var local = _model.GetLocalData();
                var remote = _model.GetRemoteData();
                _view.DisplayData(local, remote);
            }
        }
    }

    // 96. Реализуйте MVP с тестированием Presenter'а
    public class TestableMVP
    {
        public interface IModel
        {
            string Process();
        }

        public class Model : IModel
        {
            public string Process() => "Processed";
        }

        public interface IView
        {
            void ShowResult(string result);
        }

        public class Presenter
        {
            private readonly IView _view;
            private readonly IModel _model;

            public Presenter(IView view, IModel model)
            {
                _view = view;
                _model = model;
            }

            public void Execute()
            {
                var result = _model.Process();
                _view.ShowResult(result);
            }
        }

        // Mock для тестирования
        public class MockView : IView
        {
            public string LastResult { get; private set; }
            public void ShowResult(string result) => LastResult = result;
        }

        public class MockModel : IModel
        {
            public string Process() => "Mock result";
        }
    }

    // 97. Создайте MVP для работы с валидацией данных
    public class ValidationMVP
    {
        public class ValidationModel
        {
            public bool ValidateEmail(string email)
            {
                return !string.IsNullOrEmpty(email) && email.Contains("@");
            }
        }

        public interface IValidationView
        {
            void ShowValidationResult(bool isValid);
        }

        public class ValidationPresenter
        {
            private readonly IValidationView _view;
            private readonly ValidationModel _model;

            public ValidationPresenter(IValidationView view)
            {
                _view = view;
                _model = new ValidationModel();
            }

            public void ValidateEmail(string email)
            {
                var isValid = _model.ValidateEmail(email);
                _view.ShowValidationResult(isValid);
            }
        }
    }

    // 98. Реализуйте MVP с обработкой ошибок
    public class ErrorHandlingMVP
    {
        public class ErrorModel
        {
            public string ProcessData(string data)
            {
                if (string.IsNullOrEmpty(data))
                    throw new ArgumentException("Data is required");
                return $"Processed: {data}";
            }
        }

        public interface IErrorView
        {
            void DisplayResult(string result);
            void DisplayError(string error);
        }

        public class ErrorPresenter
        {
            private readonly IErrorView _view;
            private readonly ErrorModel _model;

            public ErrorPresenter(IErrorView view)
            {
                _view = view;
                _model = new ErrorModel();
            }

            public void ProcessData(string data)
            {
                try
                {
                    var result = _model.ProcessData(data);
                    _view.DisplayResult(result);
                }
                catch (Exception ex)
                {
                    _view.DisplayError(ex.Message);
                }
            }
        }
    }

    // 99. Создайте MVP для работы с пагинацией данных
    public class PaginationMVP
    {
        public class PaginationModel
        {
            private List<string> _data = Enumerable.Range(1, 100).Select(i => $"Item {i}").ToList();

            public List<string> GetPage(int page, int pageSize)
            {
                return _data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public interface IPaginationView
        {
            void DisplayPage(List<string> items, int currentPage);
        }

        public class PaginationPresenter
        {
            private readonly IPaginationView _view;
            private readonly PaginationModel _model;
            private int _currentPage = 1;

            public PaginationPresenter(IPaginationView view)
            {
                _view = view;
                _model = new PaginationModel();
            }

            public void LoadPage(int page, int pageSize = 10)
            {
                _currentPage = page;
                var items = _model.GetPage(page, pageSize);
                _view.DisplayPage(items, _currentPage);
            }
        }
    }

    // 100. Реализуйте MVP с загрузкой данных в фоне
    public class BackgroundLoadingMVP
    {
        public class BackgroundModel
        {
            public async Task<List<string>> LoadDataAsync()
            {
                await Task.Delay(2000); // Имитация долгой загрузки
                return new List<string> { "Data 1", "Data 2", "Data 3" };
            }
        }

        public interface IBackgroundView
        {
            void DisplayData(List<string> data);
            void ShowLoading();
            void HideLoading();
        }

        public class BackgroundPresenter
        {
            private readonly IBackgroundView _view;
            private readonly BackgroundModel _model;

            public BackgroundPresenter(IBackgroundView view)
            {
                _view = view;
                _model = new BackgroundModel();
            }

            public async Task LoadDataAsync()
            {
                _view.ShowLoading();
                var data = await _model.LoadDataAsync();
                _view.DisplayData(data);
                _view.HideLoading();
            }
        }
    }

    #endregion

    // ==================== ДЕМОНСТРАЦИЯ ====================

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" 100 ЗАДАЧ ПО СОБЫТИЯМ И MVP ПАТТЕРНУ\n");

            DemonstrateEvents();
            DemonstrateMVP();

        }

        static void DemonstrateEvents()
        {
            Console.WriteLine("====== РАЗДЕЛ 1: СОБЫТИЯ (1-50) ======\n");

            // 1. Простое событие
            var simpleEvent = new SimpleEventClass();
            simpleEvent.SimpleEvent += (s, e) => Console.WriteLine("1. Обработчик простого события");
            simpleEvent.RaiseSimpleEvent();

            // 2. Оповещение об изменении данных
            var dataWatcher = new DataWatcher();
            dataWatcher.DataChanged += (s, data) => Console.WriteLine($"2. Данные изменены: {data}");
            dataWatcher.Data = "Новые данные";

            // 3. Пользовательский делегат
            var customDelegate = new CustomDelegateExample();
            customDelegate.CustomEvent += (msg, prio) =>
                Console.WriteLine($"3. Кастомное событие: {msg} (приоритет: {prio})");
            customDelegate.TriggerCustomEvent("Тестовое сообщение", 1);

            // 4. Подписка и отписка
            var subscriptionMgr = new SubscriptionManager();
            subscriptionMgr.Subscribe();
            Console.WriteLine("4. С двумя подписчиками:");
            subscriptionMgr.TriggerEvent();
            subscriptionMgr.UnsubscribeFirst();
            Console.WriteLine("4. После отписки первого:");
            subscriptionMgr.TriggerEvent();

            // 5. Observable с INotifyPropertyChanged
            var observable = new ObservableItem();
            observable.PropertyChanged += (s, e) =>
                Console.WriteLine($"5. Свойство {e.PropertyName} изменено");
            observable.Name = "Новое имя";
            observable.Value = 42;

            // 6. Событие с кастомными EventArgs
            var tempSensor = new TemperatureSensor();
            tempSensor.TemperatureChanged += (s, e) =>
                Console.WriteLine($"6. Температура изменилась: {e.OldTemperature} -> {e.NewTemperature}");
            tempSensor.Temperature = 25.5;

            // Продолжение для остальных событий...
            Console.WriteLine("... и так далее для всех 50 событий");

            // 31. Событие OnClick для пользовательского контрола
            var control = new CustomControl();
            control.OnClick += (s, e) => Console.WriteLine("31. Контрол кликнули");
            control.SimulateClick();

            // 33. Событие для валидации данных
            var validator = new DataValidator();
            validator.ValidationCompleted += (s, isValid) =>
                Console.WriteLine($"33. Валидация: {(isValid ? "Успешно" : "Ошибка")}");
            validator.Validate("test data");
        }

        static void DemonstrateMVP()
        {
            Console.WriteLine("\n====== РАЗДЕЛ 2: MVP ПАТТЕРН (51-100) ======\n");

            // 51. Базовая MVP архитектура
            Console.WriteLine("51. Базовая MVP архитектура:");

            // 83. MVP для рейтингов фильмов
            Console.WriteLine("83. MVP для рейтингов фильмов:");
            var movieView = new ConsoleMovieView();
            var moviePresenter = new MovieRatingMVP.MoviePresenter(movieView);
            moviePresenter.AddMovie("Интерстеллар", 8.6);
            moviePresenter.AddMovie("Начало", 8.8);

            // 100. MVP с загрузкой в фоне
            Console.WriteLine("100. MVP с фоновой загрузкой:");
            var backgroundView = new ConsoleBackgroundView();
            var backgroundPresenter = new BackgroundLoadingMVP.BackgroundPresenter(backgroundView);

            // Асинхронная демонстрация
            _ = backgroundPresenter.LoadDataAsync();

            Thread.Sleep(3000); // Ждем завершения фоновой задачи
        }
    }

    // Вспомогательные классы для демонстрации
    public class ConsoleMovieView : MovieRatingMVP.IMovieView
    {
        public void DisplayMovies(List<MovieRatingMVP.Movie> movies)
        {
            Console.WriteLine("  Фильмы:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"    - {movie.Title}: {movie.Rating}/10");
            }
        }
    }

    public class ConsoleBackgroundView : BackgroundLoadingMVP.IBackgroundView
    {
        public void DisplayData(List<string> data)
        {
            Console.WriteLine("  Данные загружены:");
            foreach (var item in data)
            {
                Console.WriteLine($"    - {item}");
            }
        }

        public void ShowLoading()
        {
            Console.WriteLine("  Загрузка данных...");
        }

        public void HideLoading()
        {
            Console.WriteLine("  Загрузка завершена");
        }
    }
}
