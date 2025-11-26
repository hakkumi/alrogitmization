using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace MultithreadingMaster
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ 100 ЗАДАЧ МНОГОПОТОЧНОСТИ C# ===\n");

            // Запуск демонстраций по категориям
            await ThreadBasicsDemo();
            await TaskBasicsDemo();
            await AsyncAwaitDemo();
            await LockSynchronizationDemo();
            await MutexSemaphoreDemo();
            await CancellationTokenDemo();
            await ThreadPoolDemo();
            await ParallelDemo();
            await PLINQDemo();
            await ThreadSafeCollectionsDemo();
            await InterlockedOperationsDemo();
            await EventWaitHandleDemo();

            Console.WriteLine("\n=== ВСЕ ДЕМОНСТРАЦИИ ЗАВЕРШЕНЫ ===");
        }

        #region Категория 1: Thread Basics
        static async Task ThreadBasicsDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 1: Thread Basics ---");

            // Задание 1: Создание потока
            Console.WriteLine("\n1. Создание потока:");
            Thread thread1 = new Thread(() => Console.WriteLine("Привет из потока!"));
            thread1.Start();
            thread1.Join();

            // Задание 2: Два потока с Join()
            Console.WriteLine("\n2. Два потока с Join():");
            Thread thread2a = new Thread(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Поток 2A завершен");
            });
            Thread thread2b = new Thread(() =>
            {
                Thread.Sleep(300);
                Console.WriteLine("Поток 2B завершен");
            });

            thread2a.Start();
            thread2b.Start();
            thread2a.Join();
            thread2b.Join();
            Console.WriteLine("Оба потока завершены");

            // Задание 3: ID потока
            Console.WriteLine("\n3. ID текущего потока:");
            Console.WriteLine($"ID: {Thread.CurrentThread.ManagedThreadId}");

            // Задание 4: Параллельное выполнение
            Console.WriteLine("\n4. Параллельное выполнение:");
            var threads4 = new[]
            {
                new Thread(() => { Thread.Sleep(1000); Console.WriteLine("Операция 1 (1 сек)"); }),
                new Thread(() => { Thread.Sleep(500); Console.WriteLine("Операция 2 (0.5 сек)"); }),
                new Thread(() => { Thread.Sleep(800); Console.WriteLine("Операция 3 (0.8 сек)"); })
            };

            foreach (var t in threads4) t.Start();
            foreach (var t in threads4) t.Join();

            // Задание 5: ParameterizedThreadStart
            Console.WriteLine("\n5. ParameterizedThreadStart:");
            Thread thread5 = new Thread((obj) =>
                Console.WriteLine($"Получен параметр: {obj}"));
            thread5.Start("Hello Parameter!");
            thread5.Join();

            // Задание 6: Приоритет потока
            Console.WriteLine("\n6. Приоритет потока:");
            Thread thread6High = new Thread(() => WorkWithPriority("High"));
            Thread thread6Low = new Thread(() => WorkWithPriority("Low"));

            thread6High.Priority = ThreadPriority.Highest;
            thread6Low.Priority = ThreadPriority.Lowest;

            thread6High.Start();
            thread6Low.Start();
            thread6High.Join();
            thread6Low.Join();

            // Задание 7: Остановка потока
            Console.WriteLine("\n7. Остановка с Thread.Sleep:");
            Thread thread7 = new Thread(() =>
            {
                Console.WriteLine("Начало работы...");
                Thread.Sleep(1000);
                Console.WriteLine("Работа завершена после задержки");
            });
            thread7.Start();
            thread7.Join();

            // Задание 8: Фоновый поток
            Console.WriteLine("\n8. Фоновый поток:");
            Thread backgroundThread = new Thread(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Фоновый поток завершен");
            })
            { IsBackground = true };

            backgroundThread.Start();
            Console.WriteLine("Основной поток завершается (фоновый может не успеть)");

            // Задание 9: Имя потока
            Console.WriteLine("\n9. Имя потока:");
            Thread.CurrentThread.Name = "MainThread";
            Console.WriteLine($"Имя текущего потока: {Thread.CurrentThread.Name}");

            // Задание 10: Синхронизация вывода
            Console.WriteLine("\n10. Синхронизация вывода:");
            object lockObj = new object();
            Thread[] threads10 = new Thread[3];

            for (int i = 0; i < 3; i++)
            {
                int id = i;
                threads10[i] = new Thread(() =>
                {
                    lock (lockObj)
                    {
                        Console.WriteLine($"Поток {id} синхронизированно выводит сообщение");
                    }
                });
                threads10[i].Start();
            }

            foreach (var t in threads10) t.Join();
        }

        static void WorkWithPriority(string priority)
        {
            long count = 0;
            for (long i = 0; i < 10_000_000; i++) count++;
            Console.WriteLine($"Приоритет {priority}: {count} итераций");
        }
        #endregion

        #region Категория 2: Task Basics
        static async Task TaskBasicsDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 2: Task Basics ---");

            // Задание 11: Простая Task
            Console.WriteLine("\n11. Простая Task:");
            Task task11 = Task.Run(() => Console.WriteLine("Простая задача выполнена"));
            task11.Wait();

            // Задание 12: Ожидание Task
            Console.WriteLine("\n12. Ожидание Task:");
            Task task12 = Task.Run(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Задача с ожиданием завершена");
            });
            task12.Wait();

            // Задание 13: Task с результатом
            Console.WriteLine("\n13. Task с результатом:");
            Task<int> task13 = Task.Run(() =>
            {
                Thread.Sleep(300);
                return 42;
            });
            Console.WriteLine($"Результат: {task13.Result}");

            // Задание 14: WaitAll
            Console.WriteLine("\n14. Task.WaitAll:");
            Task[] tasks14 = new[]
            {
                Task.Run(() => { Thread.Sleep(400); Console.WriteLine("Задача 1"); }),
                Task.Run(() => { Thread.Sleep(200); Console.WriteLine("Задача 2"); }),
                Task.Run(() => { Thread.Sleep(300); Console.WriteLine("Задача 3"); })
            };
            Task.WaitAll(tasks14);

            // Задание 15: WhenAll
            Console.WriteLine("\n15. Task.WhenAll:");
            Task[] tasks15 = new[]
            {
                Task.Delay(400),
                Task.Delay(200),
                Task.Delay(300)
            };
            await Task.WhenAll(tasks15);
            Console.WriteLine("Все задачи WhenAll завершены");

            // Задание 16: ContinueWith
            Console.WriteLine("\n16. ContinueWith:");
            Task task16 = Task.Run(() =>
            {
                Console.WriteLine("Первая задача");
                return 10;
            })
            .ContinueWith(prev => Console.WriteLine($"Продолжение с результатом: {prev.Result * 2}"));
            await task16;

            // Задание 17: WaitAny
            Console.WriteLine("\n17. Task.WaitAny:");
            Task[] tasks17 = new[]
            {
                Task.Run(() => { Thread.Sleep(1000); Console.WriteLine("Медленная задача"); }),
                Task.Run(() => { Thread.Sleep(100); Console.WriteLine("Быстрая задача"); })
            };
            int firstCompleted = Task.WaitAny(tasks17);
            Console.WriteLine($"Первой завершилась задача {firstCompleted}");

            // Задание 18: WhenAny
            Console.WriteLine("\n18. Task.WhenAny:");
            Task[] tasks18 = new[]
            {
                Task.Delay(800).ContinueWith(_ => "Первый"),
                Task.Delay(400).ContinueWith(_ => "Второй")
            };
            Task<string> firstTask = await Task.WhenAny(tasks18.Cast<Task<string>>());
            Console.WriteLine($"Победитель: {firstTask.Result}");

            // Задание 19: Исключения в Task
            Console.WriteLine("\n19. Исключения в Task:");
            Task task19 = Task.Run(() => throw new InvalidOperationException("Тестовое исключение"));
            try
            {
                task19.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.InnerException.Message}");
            }

            // Задание 20: TaskScheduler
            Console.WriteLine("\n20. TaskScheduler:");
            await Task.Run(() =>
                Console.WriteLine($"Задача выполняется в контексте: {TaskScheduler.Current}"));
        }
        #endregion

        #region Категория 3: Async/Await
        static async Task AsyncAwaitDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 3: Async/Await ---");

            // Задание 21: Асинхронный метод
            Console.WriteLine("\n21. Асинхронный метод:");
            await SimpleAsyncMethod();

            // Задание 22: Await
            Console.WriteLine("\n22. Использование await:");
            Console.WriteLine("Начало...");
            await Task.Delay(200);
            Console.WriteLine("После await");

            // Задание 23: Асинхронный метод с результатом
            Console.WriteLine("\n23. Асинхронный метод с результатом:");
            int result = await GetNumberAsync();
            Console.WriteLine($"Результат: {result}");

            // Задание 24: Task.Delay
            Console.WriteLine("\n24. Task.Delay:");
            Console.WriteLine("До задержки...");
            await Task.Delay(300);
            Console.WriteLine("После задержки");

            // Задание 25: Обработка исключений
            Console.WriteLine("\n25. Обработка исключений:");
            try
            {
                await MethodWithExceptionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Поймано: {ex.Message}");
            }

            // Задание 26: ConfigureAwait
            Console.WriteLine("\n26. ConfigureAwait(false):");
            await Task.Delay(100).ConfigureAwait(false);
            Console.WriteLine("ConfigureAwait(false) завершен");

            // Задание 27: Цепочка асинхронных операций
            Console.WriteLine("\n27. Цепочка операций:");
            string finalResult = await FirstAsync()
                .ContinueWith(t => SecondAsync(t.Result)).Unwrap()
                .ContinueWith(t => ThirdAsync(t.Result)).Unwrap();
            Console.WriteLine($"Цепочка: {finalResult}");

            // Задание 28: Async void
            Console.WriteLine("\n28. Async void (обработчик событий):");
            var eventHandler = new EventHandlerExample();
            eventHandler.TriggerEvent();
            await Task.Delay(100);

            // Задание 29: Преобразование синхронного метода
            Console.WriteLine("\n29. Преобразование в асинхронный:");
            string data = await ReadFileAsync("test.txt");
            Console.WriteLine($"Прочитано: {data}");

            // Задание 30: Task.FromResult
            Console.WriteLine("\n30. Task.FromResult:");
            int cachedResult = await GetCachedValueAsync();
            Console.WriteLine($"Кэшированное значение: {cachedResult}");
        }

        static async Task SimpleAsyncMethod()
        {
            await Task.Delay(100);
            Console.WriteLine("Простой асинхронный метод завершен");
        }

        static async Task<int> GetNumberAsync()
        {
            await Task.Delay(200);
            return 100;
        }

        static async Task MethodWithExceptionAsync()
        {
            await Task.Delay(100);
            throw new ApplicationException("Исключение из асинхронного метода");
        }

        static async Task<string> FirstAsync()
        {
            await Task.Delay(50);
            return "Первый";
        }

        static async Task<string> SecondAsync(string input)
        {
            await Task.Delay(50);
            return $"{input} -> Второй";
        }

        static async Task<string> ThirdAsync(string input)
        {
            await Task.Delay(50);
            return $"{input} -> Третий";
        }

        static async Task<string> ReadFileAsync(string path)
        {
            // Имитация чтения файла
            await Task.Delay(100);
            return "Содержимое файла";
        }

        static Task<int> GetCachedValueAsync()
        {
            // Возвращаем готовый результат без асинхронной операции
            return Task.FromResult(42);
        }

        class EventHandlerExample
        {
            public event EventHandler MyEvent;

            public void TriggerEvent()
            {
                MyEvent?.Invoke(this, EventArgs.Empty);
            }

            public EventHandlerExample()
            {
                MyEvent += async (s, e) =>
                {
                    await Task.Delay(50);
                    Console.WriteLine("Async void обработчик события выполнен");
                };
            }
        }
        #endregion

        #region Категория 4: Lock Synchronization
        static async Task LockSynchronizationDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 4: Lock Synchronization ---");

            // Задание 31: Lock для защиты переменной
            Console.WriteLine("\n31. Lock для защиты переменной:");
            await RaceConditionDemo();

            // Задание 32: Критическая секция для счетчика
            Console.WriteLine("\n32. Критическая секция для счетчика:");
            await ThreadSafeCounterDemo();

            // Задание 33: Deadlock демонстрация
            Console.WriteLine("\n33. Deadlock демонстрация:");
            // DeadlockDemo(); // Раскомментировать для демонстрации дедлока

            // Задание 34: Избежание deadlock
            Console.WriteLine("\n34. Избежание deadlock:");
            await AvoidDeadlockDemo();

            // Задание 35: Разные объекты для lock
            Console.WriteLine("\n35. Разные объекты для lock:");
            await MultipleLocksDemo();

            // Задание 36: Потокобезопасный класс
            Console.WriteLine("\n36. Потокобезопасный класс:");
            await ThreadSafeClassDemo();

            // Задание 37: ReaderWriterLockSlim
            Console.WriteLine("\n37. ReaderWriterLockSlim:");
            await ReaderWriterLockDemo();

            // Задание 38: Сравнение производительности
            Console.WriteLine("\n38. Сравнение производительности:");
            await LockPerformanceDemo();

            // Задание 39: Lock для коллекции
            Console.WriteLine("\n39. Lock для коллекции:");
            await ThreadSafeCollectionWithLockDemo();

            // Задание 40: Читатели-писатели с lock
            Console.WriteLine("\n40. Паттерн Читатели-писатели:");
            await ReaderWriterPatternDemo();
        }

        static int sharedCounter = 0;
        static readonly object lockObject = new object();

        static async Task RaceConditionDemo()
        {
            sharedCounter = 0;
            Task[] tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        lock (lockObject)
                        {
                            sharedCounter++;
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Счетчик с lock: {sharedCounter}");
        }

        static async Task ThreadSafeCounterDemo()
        {
            var counter = new ThreadSafeCounter();
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        counter.Increment();
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Потокобезопасный счетчик: {counter.Value}");
        }

        static void DeadlockDemo()
        {
            object lock1 = new object();
            object lock2 = new object();

            Task task1 = Task.Run(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(100);
                    lock (lock2)
                    {
                        Console.WriteLine("Задача 1 получила оба lock");
                    }
                }
            });

            Task task2 = Task.Run(() =>
            {
                lock (lock2)
                {
                    Thread.Sleep(100);
                    lock (lock1)
                    {
                        Console.WriteLine("Задача 2 получила оба lock");
                    }
                }
            });

            Task.WaitAll(task1, task2);
        }

        static async Task AvoidDeadlockDemo()
        {
            object lock1 = new object();
            object lock2 = new object();

            await Task.Run(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(50);
                    lock (lock2)
                    {
                        Console.WriteLine("Без дедлока: получил оба lock");
                    }
                }
            });
        }

        static async Task MultipleLocksDemo()
        {
            object lockA = new object();
            object lockB = new object();
            int counterA = 0, counterB = 0;

            Task task1 = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock (lockA) { counterA++; }
                }
            });

            Task task2 = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock (lockB) { counterB++; }
                }
            });

            await Task.WhenAll(task1, task2);
            Console.WriteLine($"CounterA: {counterA}, CounterB: {counterB}");
        }

        class ThreadSafeCounter
        {
            private int _value = 0;
            private readonly object _lock = new object();

            public int Value
            {
                get { lock (_lock) return _value; }
            }

            public void Increment()
            {
                lock (_lock) _value++;
            }

            public void Decrement()
            {
                lock (_lock) _value--;
            }
        }

        static async Task ThreadSafeClassDemo()
        {
            var counter = new ThreadSafeCounter();
            Task[] tasks = new Task[3];

            for (int i = 0; i < 3; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        counter.Increment();
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"ThreadSafeCounter значение: {counter.Value}");
        }

        static async Task ReaderWriterLockDemo()
        {
            var rwLock = new ReaderWriterLockSlim();
            int sharedData = 0;

            Task[] readers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                readers[i] = Task.Run(() =>
                {
                    rwLock.EnterReadLock();
                    try
                    {
                        Console.WriteLine($"Чтение данных: {sharedData}");
                    }
                    finally
                    {
                        rwLock.ExitReadLock();
                    }
                });
            }

            Task writer = Task.Run(() =>
            {
                rwLock.EnterWriteLock();
                try
                {
                    sharedData = 42;
                    Console.WriteLine("Данные записаны");
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            });

            await Task.WhenAll(readers);
            await writer;
        }

        static async Task LockPerformanceDemo()
        {
            int iterations = 1000000;
            Stopwatch sw = Stopwatch.StartNew();

            // Без lock
            int counter1 = 0;
            for (int i = 0; i < iterations; i++) counter1++;
            sw.Stop();
            Console.WriteLine($"Без lock: {sw.ElapsedMilliseconds}ms");

            // С lock
            sw.Restart();
            int counter2 = 0;
            object lockObj = new object();
            for (int i = 0; i < iterations; i++)
            {
                lock (lockObj) counter2++;
            }
            sw.Stop();
            Console.WriteLine($"С lock: {sw.ElapsedMilliseconds}ms");
        }

        static async Task ThreadSafeCollectionWithLockDemo()
        {
            List<int> numbers = new List<int>();
            object listLock = new object();

            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int localI = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        lock (listLock)
                        {
                            numbers.Add(localI * 100 + j);
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Элементов в коллекции: {numbers.Count}");
        }

        static async Task ReaderWriterPatternDemo()
        {
            var data = new SharedData();
            Task[] readers = new Task[3];
            Task[] writers = new Task[2];

            for (int i = 0; i < 3; i++)
            {
                readers[i] = Task.Run(() => data.ReadData());
            }

            for (int i = 0; i < 2; i++)
            {
                writers[i] = Task.Run(() => data.WriteData());
            }

            await Task.WhenAll(readers);
            await Task.WhenAll(writers);
        }

        class SharedData
        {
            private int _data = 0;
            private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

            public void ReadData()
            {
                _lock.EnterReadLock();
                try
                {
                    Console.WriteLine($"Прочитано: {_data} (поток: {Thread.CurrentThread.ManagedThreadId})");
                    Thread.Sleep(10);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            public void WriteData()
            {
                _lock.EnterWriteLock();
                try
                {
                    _data++;
                    Console.WriteLine($"Записано: {_data} (поток: {Thread.CurrentThread.ManagedThreadId})");
                    Thread.Sleep(50);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }
        #endregion

        #region Категория 5: Mutex/Semaphore
        static async Task MutexSemaphoreDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 5: Mutex/Semaphore ---");

            // Задание 41: Mutex между потоками
            Console.WriteLine("\n41. Mutex между потоками:");
            await MutexDemo();

            // Задание 42: WaitOne для Mutex
            Console.WriteLine("\n42. WaitOne для Mutex:");
            await MutexWaitOneDemo();

            // Задание 43: Semaphore для ограничения потоков
            Console.WriteLine("\n43. Semaphore для ограничения потоков:");
            await SemaphoreDemo();

            // Задание 44: Пул ресурсов с Semaphore
            Console.WriteLine("\n44. Пул ресурсов с Semaphore:");
            await ResourcePoolDemo();

            // Задание 45: SemaphoreSlim для асинхронного ограничения
            Console.WriteLine("\n45. SemaphoreSlim:");
            await SemaphoreSlimDemo();

            // Задание 46: AbandonedMutexException
            Console.WriteLine("\n46. AbandonedMutexException:");
            // AbandonedMutexDemo(); // Раскомментировать для демонстрации

            // Задание 47: NamedMutex между процессами
            Console.WriteLine("\n47. NamedMutex между процессами:");
            await NamedMutexDemo();

            // Задание 48: Сравнение Mutex и lock
            Console.WriteLine("\n48. Сравнение Mutex и lock:");
            await MutexVsLockDemo();

            // Задание 49: CountdownEvent
            Console.WriteLine("\n49. CountdownEvent:");
            await CountdownEventDemo();

            // Задание 50: ManualResetEvent (переименован)
            Console.WriteLine("\n50. ManualResetEvent:");
            await ManualResetEventBasicDemo();
        }

        static Mutex mutex = new Mutex();
        static int mutexCounter = 0;

        static async Task MutexDemo()
        {
            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    mutex.WaitOne();
                    try
                    {
                        mutexCounter++;
                        Console.WriteLine($"Mutex: счетчик = {mutexCounter}");
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                });
            }
            await Task.WhenAll(tasks);
        }

        static async Task MutexWaitOneDemo()
        {
            await Task.Run(() =>
            {
                if (mutex.WaitOne(1000))
                {
                    try
                    {
                        Console.WriteLine("Mutex получен с WaitOne");
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                else
                {
                    Console.WriteLine("Timeout получения Mutex");
                }
            });
        }

        static async Task SemaphoreDemo()
        {
            Semaphore semaphore = new Semaphore(2, 2); // Максимум 2 потока одновременно
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    semaphore.WaitOne();
                    try
                    {
                        Console.WriteLine($"Задача {taskId} вошла в семафор");
                        Thread.Sleep(1000);
                    }
                    finally
                    {
                        semaphore.Release();
                        Console.WriteLine($"Задача {taskId} вышла из семафора");
                    }
                });
            }

            await Task.WhenAll(tasks);
        }

        static async Task ResourcePoolDemo()
        {
            var resourcePool = new ResourcePool(3);
            Task[] tasks = new Task[6];

            for (int i = 0; i < 6; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(async () =>
                {
                    await resourcePool.UseResource(taskId);
                });
            }

            await Task.WhenAll(tasks);
        }

        class ResourcePool
        {
            private Semaphore _semaphore;

            public ResourcePool(int poolSize)
            {
                _semaphore = new Semaphore(poolSize, poolSize);
            }

            public async Task UseResource(int taskId)
            {
                _semaphore.WaitOne();
                try
                {
                    Console.WriteLine($"Задача {taskId} использует ресурс");
                    await Task.Delay(500);
                }
                finally
                {
                    _semaphore.Release();
                    Console.WriteLine($"Задача {taskId} освободила ресурс");
                }
            }
        }

        static async Task SemaphoreSlimDemo()
        {
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(2, 2);
            Task[] tasks = new Task[4];

            for (int i = 0; i < 4; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(async () =>
                {
                    await semaphoreSlim.WaitAsync();
                    try
                    {
                        Console.WriteLine($"Задача {taskId} вошла в SemaphoreSlim");
                        await Task.Delay(500);
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                        Console.WriteLine($"Задача {taskId} вышла из SemaphoreSlim");
                    }
                });
            }

            await Task.WhenAll(tasks);
        }

        static void AbandonedMutexDemo()
        {
            Mutex abandonedMutex = new Mutex();
            Task.Run(() =>
            {
                abandonedMutex.WaitOne();
                // Не освобождаем mutex - будет AbandonedMutexException
                Thread.Sleep(100);
                // Завершаем без ReleaseMutex
            });

            Thread.Sleep(200);
            try
            {
                abandonedMutex.WaitOne();
            }
            catch (AbandonedMutexException ex)
            {
                Console.WriteLine($"Пойман AbandonedMutexException: {ex.Message}");
            }
        }

        static async Task NamedMutexDemo()
        {
            bool createdNew;
            using (Mutex namedMutex = new Mutex(true, "Global\\MyNamedMutex", out createdNew))
            {
                if (createdNew)
                {
                    Console.WriteLine("Named Mutex создан. Этот процесс владеет mutex.");
                    await Task.Delay(1000);
                    namedMutex.ReleaseMutex();
                }
                else
                {
                    Console.WriteLine("Named Mutex уже существует. Ожидание...");
                    namedMutex.WaitOne();
                    Console.WriteLine("Получен доступ к Named Mutex");
                    namedMutex.ReleaseMutex();
                }
            }
        }

        static async Task MutexVsLockDemo()
        {
            int iterations = 10000;
            Stopwatch sw = Stopwatch.StartNew();

            // Lock
            object lockObj = new object();
            int counter1 = 0;
            for (int i = 0; i < iterations; i++)
            {
                lock (lockObj) counter1++;
            }
            sw.Stop();
            long lockTime = sw.ElapsedMilliseconds;

            // Mutex
            sw.Restart();
            Mutex mutex = new Mutex();
            int counter2 = 0;
            for (int i = 0; i < iterations; i++)
            {
                mutex.WaitOne();
                counter2++;
                mutex.ReleaseMutex();
            }
            sw.Stop();
            long mutexTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"Lock: {lockTime}ms, Mutex: {mutexTime}ms");
        }

        static async Task CountdownEventDemo()
        {
            CountdownEvent countdown = new CountdownEvent(3);
            Task[] tasks = new Task[3];

            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    Thread.Sleep(100 * (taskId + 1));
                    Console.WriteLine($"Задача {taskId} завершена");
                    countdown.Signal();
                });
            }

            countdown.Wait();
            Console.WriteLine("Все задачи завершены (CountdownEvent)");
        }

        static async Task ManualResetEventBasicDemo()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Task worker = Task.Run(() =>
            {
                Console.WriteLine("Рабочий поток ожидает сигнал...");
                manualEvent.WaitOne();
                Console.WriteLine("Рабочий поток получил сигнал!");
            });

            await Task.Delay(1000);
            Console.WriteLine("Основной поток отправляет сигнал...");
            manualEvent.Set();

            await worker;
            manualEvent.Reset(); // Сброс для повторного использования
        }
        #endregion

        #region Категория 6: CancellationToken
        static async Task CancellationTokenDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 6: CancellationToken ---");

            // Задание 51: CancellationToken для потока
            Console.WriteLine("\n51. CancellationToken для потока:");
            await CancelThreadDemo();

            // Задание 52: CancellationTokenSource
            Console.WriteLine("\n52. CancellationTokenSource:");
            await CancellationTokenSourceDemo();

            // Задание 53: ThrowIfCancellationRequested
            Console.WriteLine("\n53. ThrowIfCancellationRequested:");
            await ThrowIfCancellationRequestedDemo();

            // Задание 54: Timeout для отмены
            Console.WriteLine("\n54. Timeout для отмены:");
            await TimeoutCancellationDemo();

            // Задание 55: OperationCanceledException
            Console.WriteLine("\n55. OperationCanceledException:");
            await OperationCanceledExceptionDemo();

            // Задание 56: Register для действий при отмене
            Console.WriteLine("\n56. Register для действий при отмене:");
            await CancellationRegisterDemo();

            // Задание 57: CancellationToken в Task.Delay
            Console.WriteLine("\n57. CancellationToken в Task.Delay:");
            await CancellationTokenWithDelayDemo();

            // Задание 58: Graceful shutdown
            Console.WriteLine("\n58. Graceful shutdown:");
            await GracefulShutdownDemo();
        }

        static async Task CancelThreadDemo()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task longRunningTask = Task.Run(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Работаю...");
                    Thread.Sleep(200);
                }
                Console.WriteLine("Задача отменена");
            });

            await Task.Delay(1000);
            cts.Cancel();
            await longRunningTask;
        }

        static async Task CancellationTokenSourceDemo()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Отмена запрошена");
                        return;
                    }
                    Console.WriteLine($"Итерация {i}");
                    Thread.Sleep(300);
                }
            }, token);

            await Task.Delay(1000);
            cts.Cancel();
            await task;
        }

        static async Task ThrowIfCancellationRequestedDemo()
        {
            var cts = new CancellationTokenSource();
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"Работа: {i}");
                    Thread.Sleep(200);
                }
            }, cts.Token);

            await Task.Delay(500);
            cts.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Задача отменена через ThrowIfCancellationRequested");
            }
        }

        static async Task TimeoutCancellationDemo()
        {
            var cts = new CancellationTokenSource(1000); // Таймаут 1 секунда

            try
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        Console.WriteLine($"Итерация {i}");
                        Thread.Sleep(300);
                    }
                }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Задача отменена по таймауту");
            }
        }

        static async Task OperationCanceledExceptionDemo()
        {
            var cts = new CancellationTokenSource();

            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        Console.WriteLine($"Работа: {i}");
                        Thread.Sleep(300);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("OperationCanceledException поймана в задаче");
                    throw; // Пробрасываем дальше
                }
            }, cts.Token);

            await Task.Delay(800);
            cts.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("OperationCanceledException поймана в основном потоке");
            }
        }

        static async Task CancellationRegisterDemo()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("Действие при отмене выполнено!"));

            Task task = Task.Run(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Работаю...");
                    Thread.Sleep(200);
                }
            });

            await Task.Delay(1000);
            cts.Cancel();
            await task;
        }

        static async Task CancellationTokenWithDelayDemo()
        {
            var cts = new CancellationTokenSource();

            Task task = Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Начало длительной операции...");
                    await Task.Delay(5000, cts.Token);
                    Console.WriteLine("Длительная операция завершена");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task.Delay отменен");
                }
            });

            await Task.Delay(1000);
            cts.Cancel();
            await task;
        }

        static async Task GracefulShutdownDemo()
        {
            var cts = new CancellationTokenSource();
            var tasks = new List<Task>();

            // Запускаем несколько рабочих задач
            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(async () =>
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Задача {taskId} работает...");
                        try
                        {
                            await Task.Delay(1000, cts.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            Console.WriteLine($"Задача {taskId} завершается...");
                            break;
                        }
                    }
                    Console.WriteLine($"Задача {taskId} завершена");
                }));
            }

            // Имитируем работу приложения
            await Task.Delay(3000);

            // Graceful shutdown
            Console.WriteLine("\nНачало graceful shutdown...");
            cts.Cancel();

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                // Игнорируем, так как отмена ожидаема
            }

            Console.WriteLine("Все задачи корректно завершены");
        }
        #endregion

        #region Категория 7: ThreadPool
        static async Task ThreadPoolDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 7: ThreadPool ---");

            // Задание 59: QueueUserWorkItem
            Console.WriteLine("\n59. QueueUserWorkItem:");
            await QueueUserWorkItemDemo();

            // Задание 60: Информация о ThreadPool
            Console.WriteLine("\n60. Информация о ThreadPool:");
            ThreadPoolInfoDemo();

            // Задание 61: Установка размеров ThreadPool
            Console.WriteLine("\n61. Установка размеров ThreadPool:");
            await ThreadPoolSizeDemo();

            // Задание 62: RegisterWaitForSingleObject
            Console.WriteLine("\n62. RegisterWaitForSingleObject:");
            await RegisterWaitForSingleObjectDemo();

            // Задание 63: ThreadPool vs Task
            Console.WriteLine("\n63. ThreadPool vs Task:");
            await ThreadPoolVsTaskDemo();

            // Задание 64: GetAvailableThreads
            Console.WriteLine("\n64. GetAvailableThreads:");
            MonitorThreadPoolDemo();

            // Задание 65: Собственный пул потоков
            Console.WriteLine("\n65. Собственный пул потоков:");
            await CustomThreadPoolDemo();

            // Задание 66: IOCompletionPort
            Console.WriteLine("\n66. IOCompletionPort (BindHandle):");
            await IOCompletionDemo();
        }

        static async Task QueueUserWorkItemDemo()
        {
            var completed = new CountdownEvent(3);

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"QueueUserWorkItem 1 выполнен: {state}");
                Thread.Sleep(100);
                completed.Signal();
            }, "Параметр 1");

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"QueueUserWorkItem 2 выполнен: {state}");
                Thread.Sleep(200);
                completed.Signal();
            }, "Параметр 2");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine("QueueUserWorkItem 3 выполнен");
                Thread.Sleep(150);
                completed.Signal();
            });

            completed.Wait();
            Console.WriteLine("Все QueueUserWorkItem завершены");
        }

        static void ThreadPoolInfoDemo()
        {
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxIO);
            ThreadPool.GetMinThreads(out int minWorker, out int minIO);
            ThreadPool.GetAvailableThreads(out int availableWorker, out int availableIO);

            Console.WriteLine($"Max Worker: {maxWorker}, IO: {maxIO}");
            Console.WriteLine($"Min Worker: {minWorker}, IO: {minIO}");
            Console.WriteLine($"Available Worker: {availableWorker}, IO: {availableIO}");
        }

        static async Task ThreadPoolSizeDemo()
        {
            int newMinWorker = Environment.ProcessorCount * 2;
            int newMinIO = Environment.ProcessorCount * 2;

            bool success = ThreadPool.SetMinThreads(newMinWorker, newMinIO);
            Console.WriteLine($"SetMinThreads({newMinWorker}, {newMinIO}): {success}");

            ThreadPool.GetMinThreads(out int currentMinWorker, out int currentMinIO);
            Console.WriteLine($"Текущие Min Threads: Worker={currentMinWorker}, IO={currentMinIO}");

            // Демонстрация работы с увеличенным пулом
            Task[] tasks = new Task[20];
            for (int i = 0; i < 20; i++)
            {
                tasks[i] = Task.Run(() => Thread.Sleep(100));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("Все задачи в увеличенном пуле завершены");
        }

        static async Task RegisterWaitForSingleObjectDemo()
        {
            AutoResetEvent waitHandle = new AutoResetEvent(false);
            var cts = new CancellationTokenSource();

            // Регистрируем обработчик, который сработает при сигнале waitHandle
            RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                (state, timedOut) =>
                {
                    if (timedOut)
                        Console.WriteLine("Таймаут ожидания");
                    else
                        Console.WriteLine($"WaitHandle сигнализирован. State: {state}");
                },
                "Custom State",
                2000, // Таймаут 2 секунды
                false // Однократное выполнение
            );

            // Симулируем сигнал через 1 секунду
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                waitHandle.Set();
            });

            await Task.Delay(3000);
            registration.Unregister(waitHandle);
            Console.WriteLine("RegisterWaitForSingleObject демо завершено");
        }

        static async Task ThreadPoolVsTaskDemo()
        {
            int iterations = 1000;
            Stopwatch sw = Stopwatch.StartNew();

            // ThreadPool.QueueUserWorkItem
            var countdown1 = new CountdownEvent(iterations);
            for (int i = 0; i < iterations; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.SpinWait(1000);
                    countdown1.Signal();
                });
            }
            countdown1.Wait();
            sw.Stop();
            long threadPoolTime = sw.ElapsedMilliseconds;

            // Task.Run
            sw.Restart();
            Task[] tasks = new Task[iterations];
            for (int i = 0; i < iterations; i++)
            {
                tasks[i] = Task.Run(() => Thread.SpinWait(1000));
            }
            await Task.WhenAll(tasks);
            sw.Stop();
            long taskTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"ThreadPool: {threadPoolTime}ms, Task: {taskTime}ms");
        }

        static void MonitorThreadPoolDemo()
        {
            ThreadPool.GetAvailableThreads(out int worker, out int io);
            Console.WriteLine($"Доступно потоков: Worker={worker}, IO={io}");

            // Демонстрация использования пула
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    Thread.Sleep(500);
                    ThreadPool.GetAvailableThreads(out int currentWorker, out int currentIO);
                    Console.WriteLine($"Во время работы - Worker: {currentWorker}, IO: {currentIO}");
                });
            }
            Task.WaitAll(tasks);

            ThreadPool.GetAvailableThreads(out int finalWorker, out int finalIO);
            Console.WriteLine($"После работы - Worker: {finalWorker}, IO: {finalIO}");
        }

        static async Task CustomThreadPoolDemo()
        {
            var customPool = new CustomThreadPool(3);

            Task[] tasks = new Task[6];
            for (int i = 0; i < 6; i++)
            {
                int taskId = i;
                tasks[i] = customPool.QueueWorkItem(() =>
                {
                    Console.WriteLine($"Задача {taskId} выполняется в кастомном пуле");
                    Thread.Sleep(500);
                    return taskId * 10;
                });
            }

            await Task.WhenAll(tasks);
            customPool.Dispose();
        }

        class CustomThreadPool : IDisposable
        {
            private readonly BlockingCollection<(Func<object> work, TaskCompletionSource<object> tcs)> _queue;
            private readonly Thread[] _workers;
            private volatile bool _disposed = false;

            public CustomThreadPool(int workerCount)
            {
                _queue = new BlockingCollection<(Func<object>, TaskCompletionSource<object>)>();
                _workers = new Thread[workerCount];

                for (int i = 0; i < workerCount; i++)
                {
                    _workers[i] = new Thread(Worker)
                    {
                        IsBackground = true
                    };
                    _workers[i].Start();
                }
            }

            public Task<T> QueueWorkItem<T>(Func<T> work)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(CustomThreadPool));

                var tcs = new TaskCompletionSource<object>();
                _queue.Add((() => work(), tcs));
                return tcs.Task.ContinueWith(t => (T)t.Result);
            }

            private void Worker()
            {
                foreach (var item in _queue.GetConsumingEnumerable())
                {
                    try
                    {
                        var result = item.work();
                        item.tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        item.tcs.SetException(ex);
                    }
                }
            }

            public void Dispose()
            {
                _disposed = true;
                _queue.CompleteAdding();
                foreach (var worker in _workers)
                {
                    worker.Join();
                }
                _queue.Dispose();
            }
        }

        static async Task IOCompletionDemo()
        {
            // Создаем временный файл для демонстрации
            string tempFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFile, "Тестовые данные для IOCompletionPort");

            try
            {
                using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    byte[] buffer = new byte[1024];

                    // Асинхронное чтение с использованием IO Completion Ports
                    var readTask = Task<int>.Factory.FromAsync(
                        fileStream.BeginRead,
                        fileStream.EndRead,
                        buffer, 0, buffer.Length, null);

                    int bytesRead = await readTask;
                    Console.WriteLine($"Прочитано {bytesRead} байт через IO Completion Port");
                    Console.WriteLine($"Данные: {System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead)}");
                }
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
        #endregion

        #region Категория 8: Parallel
        static async Task ParallelDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 8: Parallel ---");

            // Задание 67: Parallel.For
            Console.WriteLine("\n67. Parallel.For:");
            await ParallelForDemo();

            // Задание 68: Parallel.ForEach
            Console.WriteLine("\n68. Parallel.ForEach:");
            await ParallelForEachDemo();

            // Задание 69: Parallel.Invoke
            Console.WriteLine("\n69. Parallel.Invoke:");
            ParallelInvokeDemo();

            // Задание 70: ParallelOptions
            Console.WriteLine("\n70. ParallelOptions:");
            ParallelOptionsDemo();

            // Задание 71: CancellationToken в Parallel
            Console.WriteLine("\n71. CancellationToken в Parallel:");
            await ParallelCancellationDemo();

            // Задание 72: Partitioner
            Console.WriteLine("\n72. Partitioner:");
            PartitionerDemo();

            // Задание 73: OperationCanceledException в Parallel
            Console.WriteLine("\n73. OperationCanceledException в Parallel:");
            await ParallelExceptionDemo();

            // Задание 74: Divide-and-conquer с Parallel.Invoke
            Console.WriteLine("\n74. Divide-and-conquer алгоритм:");
            DivideAndConquerDemo();

            // Задание 75: Сравнение Parallel vs обычные циклы
            Console.WriteLine("\n75. Сравнение Parallel vs обычные циклы:");
            ParallelVsSequentialDemo();

            // Задание 76: ParallelOptions для настройки
            Console.WriteLine("\n76. ParallelOptions для настройки:");
            ParallelOptionsAdvancedDemo();
        }

        static async Task ParallelForDemo()
        {
            int[] results = new int[10];

            Parallel.For(0, 10, i =>
            {
                results[i] = i * i;
                Console.WriteLine($"Parallel.For: i={i}, result={results[i]}");
            });

            Console.WriteLine("Parallel.For завершен");
            await Task.Delay(100); // Для синхронизации вывода
        }

        static async Task ParallelForEachDemo()
        {
            var numbers = Enumerable.Range(1, 10);
            var results = new System.Collections.Concurrent.ConcurrentBag<int>();

            Parallel.ForEach(numbers, number =>
            {
                int result = number * number;
                results.Add(result);
                Console.WriteLine($"Parallel.ForEach: {number}² = {result}");
            });

            Console.WriteLine($"Всего результатов: {results.Count}");
            await Task.Delay(100);
        }

        static void ParallelInvokeDemo()
        {
            Parallel.Invoke(
                () => { Console.WriteLine("Операция 1 выполняется"); Thread.Sleep(500); },
                () => { Console.WriteLine("Операция 2 выполняется"); Thread.Sleep(300); },
                () => { Console.WriteLine("Операция 3 выполняется"); Thread.Sleep(400); },
                () => { Console.WriteLine("Операция 4 выполняется"); Thread.Sleep(200); }
            );
            Console.WriteLine("Parallel.Invoke завершен");
        }

        static void ParallelOptionsDemo()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2 // Ограничиваем 2 потоками
            };

            Console.WriteLine($"MaxDegreeOfParallelism: {options.MaxDegreeOfParallelism}");

            Parallel.For(0, 10, options, i =>
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} обрабатывает i={i}");
                Thread.Sleep(100);
            });
        }

        static async Task ParallelCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            var options = new ParallelOptions
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = 3
            };

            Task parallelTask = Task.Run(() =>
            {
                try
                {
                    Parallel.For(0, 20, options, (i, state) =>
                    {
                        if (i == 5)
                        {
                            Console.WriteLine("Отмена Parallel.For...");
                            state.Stop();
                            cts.Cancel();
                        }

                        options.CancellationToken.ThrowIfCancellationRequested();
                        Console.WriteLine($"Обработка i={i}");
                        Thread.Sleep(100);
                    });
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Parallel.For отменен");
                }
            });

            await parallelTask;
        }

        static void PartitionerDemo()
        {
            var largeArray = Enumerable.Range(0, 1000).ToArray();
            var results = new System.Collections.Concurrent.ConcurrentBag<long>();

            var partitioner = Partitioner.Create(0, largeArray.Length, 100); // Размер чанка = 100

            Parallel.ForEach(partitioner, range =>
            {
                long sum = 0;
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    sum += largeArray[i];
                }
                results.Add(sum);
                Console.WriteLine($"Обработан диапазон {range.Item1}-{range.Item2}: сумма = {sum}");
            });

            long totalSum = results.Sum();
            Console.WriteLine($"Общая сумма: {totalSum}");
        }

        static async Task ParallelExceptionDemo()
        {
            try
            {
                Parallel.For(0, 10, (i, state) =>
                {
                    if (i == 5)
                    {
                        throw new InvalidOperationException("Тестовое исключение в Parallel.For");
                    }
                    Console.WriteLine($"Обработка i={i}");
                });
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Поймано исключение: {ex.Message}");
                }
            }

            await Task.Delay(100);
        }

        static void DivideAndConquerDemo()
        {
            int[] data = Enumerable.Range(1, 100).ToArray();
            long sum = DivideAndConquerSum(data, 0, data.Length - 1);
            Console.WriteLine($"Divide-and-conquer сумма: {sum}");
        }

        static long DivideAndConquerSum(int[] data, int left, int right)
        {
            if (right - left < 10) // Базовый случай - маленький диапазон
            {
                long sum = 0;
                for (int i = left; i <= right; i++)
                {
                    sum += data[i];
                }
                return sum;
            }
            else // Рекурсивный случай - разделяй и властвуй
            {
                int mid = (left + right) / 2;
                long leftSum = 0, rightSum = 0;

                Parallel.Invoke(
                    () => leftSum = DivideAndConquerSum(data, left, mid),
                    () => rightSum = DivideAndConquerSum(data, mid + 1, right)
                );

                return leftSum + rightSum;
            }
        }

        static void ParallelVsSequentialDemo()
        {
            int[] data = Enumerable.Range(1, 1000000).ToArray();
            Stopwatch sw = Stopwatch.StartNew();

            // Sequential
            long sequentialSum = 0;
            foreach (int num in data)
            {
                sequentialSum += num;
            }
            sw.Stop();
            long sequentialTime = sw.ElapsedMilliseconds;

            // Parallel
            sw.Restart();
            long parallelSum = 0;
            Parallel.ForEach(data, () => 0L, (num, state, localSum) =>
            {
                return localSum + num;
            },
            localSum => Interlocked.Add(ref parallelSum, localSum));
            sw.Stop();
            long parallelTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"Sequential: {sequentialTime}ms, сумма: {sequentialSum}");
            Console.WriteLine($"Parallel: {parallelTime}ms, сумма: {parallelSum}");
            Console.WriteLine($"Ускорение: {(double)sequentialTime / parallelTime:F2}x");
        }

        static void ParallelOptionsAdvancedDemo()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                TaskScheduler = TaskScheduler.Default
            };

            Console.WriteLine($"Используется процессоров: {options.MaxDegreeOfParallelism}");

            var results = new System.Collections.Concurrent.ConcurrentDictionary<int, string>();

            Parallel.For(0, 20, options, i =>
            {
                string result = $"Результат для {i} (поток {Thread.CurrentThread.ManagedThreadId})";
                results[i] = result;
                Console.WriteLine(result);
            });

            Console.WriteLine($"Обработано элементов: {results.Count}");
        }
        #endregion

        #region Категория 9: PLINQ
        static async Task PLINQDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 9: PLINQ ---");

            // Задание 77: AsParallel()
            Console.WriteLine("\n77. AsParallel():");
            AsParallelDemo();

            // Задание 78: WithDegreeOfParallelism()
            Console.WriteLine("\n78. WithDegreeOfParallelism():");
            WithDegreeOfParallelismDemo();

            // Задание 79: WithCancellation()
            Console.WriteLine("\n79. WithCancellation():");
            await WithCancellationDemo();

            // Задание 80: ParallelEnumerable
            Console.WriteLine("\n80. ParallelEnumerable:");
            ParallelEnumerableDemo();

            // Задание 81: PLINQ vs LINQ
            Console.WriteLine("\n81. PLINQ vs LINQ:");
            PLINQvsLINQDemo();

            // Задание 82: ToList() для PLINQ
            Console.WriteLine("\n82. ToList() для PLINQ:");
            PLINQToListDemo();
        }

        static void AsParallelDemo()
        {
            var numbers = Enumerable.Range(1, 100);

            var parallelQuery = numbers.AsParallel()
                .Where(x => x % 2 == 0)
                .Select(x => x * x);

            Console.WriteLine("Четные числа в квадрате (PLINQ):");
            foreach (var result in parallelQuery.Take(10))
            {
                Console.Write($"{result} ");
            }
            Console.WriteLine();
        }

        static void WithDegreeOfParallelismDemo()
        {
            var numbers = Enumerable.Range(1, 1000);

            var query = numbers.AsParallel()
                .WithDegreeOfParallelism(2)
                .Where(x =>
                {
                    Console.WriteLine($"Обработка {x} в потоке {Thread.CurrentThread.ManagedThreadId}");
                    return x % 3 == 0;
                })
                .Select(x => x);

            var results = query.ToList();
            Console.WriteLine($"Найдено чисел, кратных 3: {results.Count}");
        }

        static async Task WithCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            var numbers = Enumerable.Range(1, 1000000);

            Task queryTask = Task.Run(() =>
            {
                try
                {
                    var query = numbers.AsParallel()
                        .WithCancellation(cts.Token)
                        .Where(x => IsPrime(x))
                        .Select(x => x);

                    var primes = query.Take(100).ToList();
                    Console.WriteLine($"Найдено простых чисел: {primes.Count}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("PLINQ запрос отменен");
                }
            });

            // Отменяем через 100ms
            await Task.Delay(100);
            cts.Cancel();
            await queryTask;
        }

        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        static void ParallelEnumerableDemo()
        {
            var numbers = Enumerable.Range(1, 100);

            // Использование методов ParallelEnumerable
            var result = numbers.AsParallel()
                .Where(x => x % 2 == 0)
                .Aggregate(
                    0L, // seed
                    (total, num) => total + num, // update
                    (subtotal, data) => subtotal + data, // combine
                    finalResult => finalResult // result selector
                );

            Console.WriteLine($"Сумма четных чисел: {result}");

            // Другие методы ParallelEnumerable
            var firstEven = numbers.AsParallel().First(x => x % 2 == 0);
            var allEven = numbers.AsParallel().All(x => x % 2 == 0);
            var anyEven = numbers.AsParallel().Any(x => x % 2 == 0);

            Console.WriteLine($"First even: {firstEven}, All even: {allEven}, Any even: {anyEven}");
        }

        static void PLINQvsLINQDemo()
        {
            var largeData = Enumerable.Range(1, 1000000);
            Stopwatch sw = Stopwatch.StartNew();

            // LINQ
            var linqQuery = largeData.Where(x => x % 2 == 0).Select(x => x * x);
            var linqResult = linqQuery.ToList();
            sw.Stop();
            long linqTime = sw.ElapsedMilliseconds;

            // PLINQ
            sw.Restart();
            var plinqQuery = largeData.AsParallel().Where(x => x % 2 == 0).Select(x => x * x);
            var plinqResult = plinqQuery.ToList();
            sw.Stop();
            long plinqTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"LINQ: {linqTime}ms, результат: {linqResult.Count} элементов");
            Console.WriteLine($"PLINQ: {plinqTime}ms, результат: {plinqResult.Count} элементов");
            Console.WriteLine($"Ускорение: {(double)linqTime / plinqTime:F2}x");
        }

        static void PLINQToListDemo()
        {
            var numbers = Enumerable.Range(1, 100);

            // PLINQ запрос с материализацией
            var squaredNumbers = numbers.AsParallel()
                .Select(x =>
                {
                    Console.WriteLine($"Обработка {x} в потоке {Thread.CurrentThread.ManagedThreadId}");
                    return x * x;
                })
                .ToList(); // Материализация результатов

            Console.WriteLine($"Материализовано {squaredNumbers.Count} результатов");

            // Без материализации - отложенное выполнение
            var query = numbers.AsParallel().Select(x => x * x);
            Console.WriteLine("Запрос создан, но еще не выполнен");

            // Выполняем при перечислении
            foreach (var num in query.Take(5))
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();
        }
        #endregion

        #region Категория 10: Thread-Safe Collections
        static async Task ThreadSafeCollectionsDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 10: Thread-Safe Collections ---");

            // Задание 83: ConcurrentDictionary
            Console.WriteLine("\n83. ConcurrentDictionary:");
            await ConcurrentDictionaryDemo();

            // Задание 84: ConcurrentQueue
            Console.WriteLine("\n84. ConcurrentQueue:");
            await ConcurrentQueueDemo();

            // Задание 85: ConcurrentBag
            Console.WriteLine("\n85. ConcurrentBag:");
            await ConcurrentBagDemo();

            // Задание 86: BlockingCollection
            Console.WriteLine("\n86. BlockingCollection:");
            await BlockingCollectionDemo();

            // Задание 87: Producer-Consumer с BlockingCollection
            Console.WriteLine("\n87. Producer-Consumer с BlockingCollection:");
            await ProducerConsumerDemo();

            // Задание 88: ConcurrentStack
            Console.WriteLine("\n88. ConcurrentStack:");
            await ConcurrentStackDemo();

            // Задание 89: Сравнение производительности
            Console.WriteLine("\n89. Сравнение производительности:");
            await ThreadSafeCollectionsPerformanceDemo();

            // Задание 90: TryAdd и TryRemove
            Console.WriteLine("\n90. TryAdd и TryRemove:");
            await TryAddTryRemoveDemo();
        }

        static async Task ConcurrentDictionaryDemo()
        {
            var concurrentDict = new ConcurrentDictionary<string, int>();

            Task[] addTasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int localI = i;
                addTasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        string key = $"key_{localI}_{j}";
                        concurrentDict.TryAdd(key, localI * 100 + j);
                    }
                });
            }

            await Task.WhenAll(addTasks);
            Console.WriteLine($"Элементов в словаре: {concurrentDict.Count}");

            // Обновление значения
            concurrentDict.AddOrUpdate("key_0_0", 1, (key, oldValue) => oldValue + 1);
            Console.WriteLine($"Обновленное значение key_0_0: {concurrentDict["key_0_0"]}");
        }

        static async Task ConcurrentQueueDemo()
        {
            var concurrentQueue = new ConcurrentQueue<int>();
            Task[] producers = new Task[3];

            // Производители
            for (int i = 0; i < 3; i++)
            {
                int producerId = i;
                producers[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        int item = producerId * 10 + j;
                        concurrentQueue.Enqueue(item);
                        Console.WriteLine($"Производитель {producerId} добавил: {item}");
                    }
                });
            }

            await Task.WhenAll(producers);

            // Потребитель
            Console.WriteLine("Элементы в очереди:");
            while (concurrentQueue.TryDequeue(out int item))
            {
                Console.WriteLine($"Извлечен: {item}");
            }
        }

        static async Task ConcurrentBagDemo()
        {
            var concurrentBag = new ConcurrentBag<int>();
            Task[] tasks = new Task[4];

            for (int i = 0; i < 4; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int item = taskId * 10 + j;
                        concurrentBag.Add(item);
                        Console.WriteLine($"Добавлен в bag: {item}");
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Всего элементов в bag: {concurrentBag.Count}");

            // Извлечение (порядок не гарантирован)
            Console.WriteLine("Извлечение из bag:");
            while (concurrentBag.TryTake(out int item))
            {
                Console.WriteLine($"Извлечен: {item}");
            }
        }

        static async Task BlockingCollectionDemo()
        {
            var blockingCollection = new BlockingCollection<int>(boundedCapacity: 5);

            // Производитель
            Task producer = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    blockingCollection.Add(i);
                    Console.WriteLine($"Добавлен: {i} (всего: {blockingCollection.Count})");
                    Thread.Sleep(100);
                }
                blockingCollection.CompleteAdding();
            });

            // Потребитель
            Task consumer = Task.Run(() =>
            {
                foreach (int item in blockingCollection.GetConsumingEnumerable())
                {
                    Console.WriteLine($"Обработан: {item}");
                    Thread.Sleep(150);
                }
            });

            await Task.WhenAll(producer, consumer);
            Console.WriteLine("BlockingCollection демо завершено");
        }

        static async Task ProducerConsumerDemo()
        {
            var buffer = new BlockingCollection<int>(10);
            var cts = new CancellationTokenSource();
            var results = new ConcurrentBag<int>();

            // Производители
            Task[] producers = new Task[2];
            for (int i = 0; i < 2; i++)
            {
                int producerId = i;
                producers[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        int item = producerId * 100 + j;
                        buffer.Add(item);
                        Console.WriteLine($"Производитель {producerId} создал: {item}");
                        Thread.Sleep(50);
                    }
                });
            }

            // Потребители
            Task[] consumers = new Task[2];
            for (int i = 0; i < 2; i++)
            {
                int consumerId = i;
                consumers[i] = Task.Run(() =>
                {
                    foreach (int item in buffer.GetConsumingEnumerable())
                    {
                        results.Add(item * 2);
                        Console.WriteLine($"Потребитель {consumerId} обработал: {item} -> {item * 2}");
                        Thread.Sleep(80);
                    }
                });
            }

            // Завершаем добавление после производителей
            await Task.WhenAll(producers);
            buffer.CompleteAdding();

            await Task.WhenAll(consumers);
            Console.WriteLine($"Обработано результатов: {results.Count}");
        }

        static async Task ConcurrentStackDemo()
        {
            var concurrentStack = new ConcurrentStack<int>();

            Task[] pushers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                pushers[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int item = taskId * 10 + j;
                        concurrentStack.Push(item);
                        Console.WriteLine($"Добавлен в stack: {item}");
                    }
                });
            }

            await Task.WhenAll(pushers);

            // Попытка извлечения всех элементов
            Task popper = Task.Run(() =>
            {
                while (concurrentStack.TryPop(out int item))
                {
                    Console.WriteLine($"Извлечен из stack: {item}");
                }
            });

            await popper;
        }

        static async Task ThreadSafeCollectionsPerformanceDemo()
        {
            int operations = 10000;
            Stopwatch sw = Stopwatch.StartNew();

            // ConcurrentDictionary
            var concurrentDict = new ConcurrentDictionary<int, int>();
            Parallel.For(0, operations, i => concurrentDict.TryAdd(i, i));
            sw.Stop();
            long dictTime = sw.ElapsedMilliseconds;

            // ConcurrentQueue
            sw.Restart();
            var concurrentQueue = new ConcurrentQueue<int>();
            Parallel.For(0, operations, i => concurrentQueue.Enqueue(i));
            sw.Stop();
            long queueTime = sw.ElapsedMilliseconds;

            // ConcurrentBag
            sw.Restart();
            var concurrentBag = new ConcurrentBag<int>();
            Parallel.For(0, operations, i => concurrentBag.Add(i));
            sw.Stop();
            long bagTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"ConcurrentDictionary: {dictTime}ms");
            Console.WriteLine($"ConcurrentQueue: {queueTime}ms");
            Console.WriteLine($"ConcurrentBag: {bagTime}ms");
        }

        static async Task TryAddTryRemoveDemo()
        {
            var concurrentDict = new ConcurrentDictionary<string, int>();

            // TryAdd
            bool added1 = concurrentDict.TryAdd("key1", 100);
            bool added2 = concurrentDict.TryAdd("key1", 200); // Не добавится, ключ уже существует

            Console.WriteLine($"TryAdd key1=100: {added1}");
            Console.WriteLine($"TryAdd key1=200: {added2}");

            // TryUpdate
            bool updated = concurrentDict.TryUpdate("key1", 300, 100); // Обновит, если текущее значение = 100
            Console.WriteLine($"TryUpdate key1=300 (ожидая 100): {updated}");

            // TryRemove
            bool removed = concurrentDict.TryRemove("key1", out int removedValue);
            Console.WriteLine($"TryRemove key1: {removed}, значение: {removedValue}");

            // GetOrAdd
            int value1 = concurrentDict.GetOrAdd("newKey", 500);
            int value2 = concurrentDict.GetOrAdd("newKey", 600); // Вернет существующее значение

            Console.WriteLine($"GetOrAdd newKey=500: {value1}");
            Console.WriteLine($"GetOrAdd newKey=600: {value2}");
        }
        #endregion

        #region Категория 11: Interlocked Operations
        static async Task InterlockedOperationsDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 11: Interlocked Operations ---");

            // Задание 91: Interlocked.Increment
            Console.WriteLine("\n91. Interlocked.Increment:");
            await InterlockedIncrementDemo();

            // Задание 92: Interlocked.CompareExchange
            Console.WriteLine("\n92. Interlocked.CompareExchange:");
            await CompareExchangeDemo();

            // Задание 93: Interlocked vs Lock
            Console.WriteLine("\n93. Interlocked vs Lock:");
            await InterlockedVsLockDemo();

            // Задание 94: Spinlock с Interlocked
            Console.WriteLine("\n94. Spinlock с Interlocked:");
            await SpinlockDemo();

            // Задание 95: Interlocked.Read
            Console.WriteLine("\n95. Interlocked.Read:");
            InterlockedReadDemo();

            // Задание 96: Interlocked.Exchange
            Console.WriteLine("\n96. Interlocked.Exchange:");
            await InterlockedExchangeDemo();
        }

        static async Task InterlockedIncrementDemo()
        {
            int counter = 0;
            Task[] tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        Interlocked.Increment(ref counter);
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Interlocked счетчик: {counter}");
        }

        static async Task CompareExchangeDemo()
        {
            int sharedValue = 0;
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        int initialValue, newValue;
                        do
                        {
                            initialValue = sharedValue;
                            newValue = initialValue + 1;
                        }
                        while (Interlocked.CompareExchange(ref sharedValue, newValue, initialValue) != initialValue);
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"CompareExchange результат: {sharedValue}");
        }

        static async Task InterlockedVsLockDemo()
        {
            int interlockedCounter = 0;
            int lockCounter = 0;
            object lockObject = new object();
            int iterations = 100000;

            Stopwatch sw = Stopwatch.StartNew();

            // Interlocked
            Task interlockedTask = Task.Run(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    Interlocked.Increment(ref interlockedCounter);
                }
            });

            // Lock
            Task lockTask = Task.Run(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    lock (lockObject)
                    {
                        lockCounter++;
                    }
                }
            });

            await Task.WhenAll(interlockedTask, lockTask);
            sw.Stop();

            Console.WriteLine($"Interlocked: {interlockedCounter}, время: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Lock: {lockCounter}, время: {sw.ElapsedMilliseconds}ms");
        }

        static async Task SpinlockDemo()
        {
            var spinLock = new SpinLock();
            int counter = 0;
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            spinLock.Enter(ref lockTaken);
                            counter++;
                        }
                        finally
                        {
                            if (lockTaken) spinLock.Exit();
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Spinlock счетчик: {counter}");
        }

        static void InterlockedReadDemo()
        {
            long sharedLong = 123456789012345;
            long readValue = Interlocked.Read(ref sharedLong);
            Console.WriteLine($"Interlocked.Read: {readValue}");

            // CompareExchange для long
            long comparand = 123456789012345;
            long newValue = 987654321098765;
            long original = Interlocked.CompareExchange(ref sharedLong, newValue, comparand);

            Console.WriteLine($"CompareExchange long: было {original}, стало {sharedLong}");
        }

        static async Task InterlockedExchangeDemo()
        {
            int sharedValue = 0;
            Task[] tasks = new Task[3];

            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        int previous = Interlocked.Exchange(ref sharedValue, taskId * 10 + j);
                        Console.WriteLine($"Задача {taskId}: Exchange {previous} -> {taskId * 10 + j}");
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Финальное значение: {sharedValue}");
        }
        #endregion

        #region Категория 12: EventWaitHandle
        static async Task EventWaitHandleDemo()
        {
            Console.WriteLine("\n--- КАТЕГОРИЯ 12: EventWaitHandle ---");

            // Задание 97: AutoResetEvent
            Console.WriteLine("\n97. AutoResetEvent:");
            await AutoResetEventDemo();

            // Задание 98: ManualResetEvent (переименован)
            Console.WriteLine("\n98. ManualResetEvent:");
            await ManualResetEventAdvancedDemo();

            // Задание 99: WaitHandle.WaitAll
            Console.WriteLine("\n99. WaitHandle.WaitAll:");
            await WaitAllDemo();

            // Задание 100: Producer-Consumer с EventWaitHandle
            Console.WriteLine("\n100. Producer-Consumer с EventWaitHandle:");
            await EventWaitHandleProducerConsumerDemo();
        }

        static async Task AutoResetEventDemo()
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            Task worker1 = Task.Run(() =>
            {
                Console.WriteLine("Рабочий 1 ожидает сигнал...");
                autoEvent.WaitOne();
                Console.WriteLine("Рабочий 1 получил сигнал!");
            });

            Task worker2 = Task.Run(() =>
            {
                Console.WriteLine("Рабочий 2 ожидает сигнал...");
                autoEvent.WaitOne();
                Console.WriteLine("Рабочий 2 получил сигнал!");
            });

            await Task.Delay(1000);
            Console.WriteLine("Отправка сигнала рабочим...");
            autoEvent.Set(); // Разбудит только одного рабочего

            await Task.Delay(1000);
            autoEvent.Set(); // Разбудит второго рабочего

            await Task.WhenAll(worker1, worker2);
        }

        static async Task ManualResetEventAdvancedDemo()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);

            Task[] workers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int workerId = i;
                workers[i] = Task.Run(() =>
                {
                    Console.WriteLine($"Рабочий {workerId} ожидает сигнал...");
                    manualEvent.WaitOne();
                    Console.WriteLine($"Рабочий {workerId} получил сигнал!");
                });
            }

            await Task.Delay(1000);
            Console.WriteLine("Отправка сигнала всем рабочим...");
            manualEvent.Set(); // Разбудит всех рабочих одновременно

            await Task.WhenAll(workers);
            manualEvent.Reset(); // Сбрасываем для следующего использования
        }

        static async Task WaitAllDemo()
        {
            WaitHandle[] waitHandles = new WaitHandle[]
            {
                new AutoResetEvent(false),
                new ManualResetEvent(false)
            };

            Task task1 = Task.Run(() =>
            {
                Thread.Sleep(800);
                ((AutoResetEvent)waitHandles[0]).Set();
                Console.WriteLine("Событие 1 установлено");
            });

            Task task2 = Task.Run(() =>
            {
                Thread.Sleep(500);
                ((ManualResetEvent)waitHandles[1]).Set();
                Console.WriteLine("Событие 2 установлено");
            });

            bool allSignaled = WaitHandle.WaitAll(waitHandles, 2000);
            Console.WriteLine($"Все события получены: {allSignaled}");

            await Task.WhenAll(task1, task2);

            // Освобождение ресурсов
            foreach (var handle in waitHandles)
            {
                handle.Close();
            }
        }

        static async Task EventWaitHandleProducerConsumerDemo()
        {
            var buffer = new Queue<int>();
            var bufferLock = new object();
            var itemsAvailable = new AutoResetEvent(false);
            var spaceAvailable = new AutoResetEvent(true); // Изначально есть место
            var completed = new ManualResetEvent(false);
            const int bufferSize = 5;

            Task producer = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    spaceAvailable.WaitOne(); // Ждем свободного места
                    lock (bufferLock)
                    {
                        buffer.Enqueue(i);
                        Console.WriteLine($"Произведен: {i} (в буфере: {buffer.Count})");
                        itemsAvailable.Set(); // Сигнализируем, что есть элементы
                    }
                    Thread.Sleep(100);
                }
                completed.Set();
            });

            Task consumer = Task.Run(() =>
            {
                while (true)
                {
                    if (WaitHandle.WaitAny(new WaitHandle[] { itemsAvailable, completed }) == 1)
                    {
                        break; // Завершено
                    }

                    lock (bufferLock)
                    {
                        if (buffer.Count > 0)
                        {
                            int item = buffer.Dequeue();
                            Console.WriteLine($"Потреблен: {item} (в буфере: {buffer.Count})");
                            spaceAvailable.Set(); // Сигнализируем о свободном месте
                        }
                    }
                    Thread.Sleep(150);
                }
            });

            await Task.WhenAll(producer, consumer);
            Console.WriteLine("Producer-Consumer с EventWaitHandle завершен");
        }
        #endregion
    }
}
