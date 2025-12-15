using System;

// Класс монитора CPU
public class CpuMonitor
{
    public delegate void CpuEventHandler(CpuMonitor sender, int percent);

    public event CpuEventHandler? LoadChanged;

    private EventHandler<int>? overloadReached;

    public event EventHandler<int> OverloadReached
    {
        add
        {
            overloadReached += value;
            Console.WriteLine("Подписчик добавлен на событие OverloadReached");
        }
        remove
        {
            overloadReached -= value;
            Console.WriteLine("Подписчик удалён с события OverloadReached");
        }
    }

    // Метод запуска мониторинга
    public void Start()
    {
        Random random = new Random();
        int measurements = random.Next(10, 15);
        int currentLoad = 50;

        Console.WriteLine($"\nНачинаем мониторинг CPU ({measurements} измерений)...");

        for (int i = 0; i < measurements; i++)
        {
            int change = random.Next(-20, 21);
            currentLoad += change;

            if (currentLoad < 15) currentLoad = 15;
            if (currentLoad > 100) currentLoad = 100;

            Console.WriteLine($"\nИзмерение {i + 1}/{measurements}");


            LoadChanged?.Invoke(this, currentLoad);

            if (currentLoad >= 85)
            {
                Console.WriteLine($"Порог перегрузки достигнут: {currentLoad}%");
                overloadReached?.Invoke(this, currentLoad);
            }

            System.Threading.Thread.Sleep(500);
        }
    }
}

// Класс консольной панели управления
public class ConsoleDashboard
{
    public void Subscribe(CpuMonitor monitor)
    {
        monitor.LoadChanged += OnLoadChanged;
        monitor.OverloadReached += OnOverloadReached;
    }

    public void UnsubscribeOverload(CpuMonitor monitor)
    {
        monitor.OverloadReached -= OnOverloadReached;
    }

    public void UnsubscribeAll(CpuMonitor monitor)
    {
        monitor.LoadChanged -= OnLoadChanged;
        monitor.OverloadReached -= OnOverloadReached;
    }

    private void OnLoadChanged(CpuMonitor sender, int percent)
    {
        Console.WriteLine($"CPU: {percent}%");
    }

    private void OnOverloadReached(object? sender, int percent)
    {
        Console.WriteLine($"ПЕРЕГРУЗКА CPU: {percent}% — сократите нагрузку!");
    }
}

public class OverloadStats
{
    private int highLoadCount;

    public void Subscribe(CpuMonitor monitor)
    {
        monitor.LoadChanged += OnLoadChanged;
    }

    public void Unsubscribe(CpuMonitor monitor)
    {
        monitor.LoadChanged -= OnLoadChanged;
    }

    private void OnLoadChanged(CpuMonitor sender, int percent)
    {
        if (percent >= 70)
        {
            highLoadCount++;
        }
    }

    public void Report()
    {
        Console.WriteLine($"\n=== СТАТИСТИКА ===");
        Console.WriteLine($"Загрузка ≥ 70% встречалась {highLoadCount} раз");
    }
}
// Главная программа
class Program
{
    static void Main()
    {
        Console.WriteLine("=== МОНИТОРИНГ ЗАГРУЗКИ CPU ===");

        CpuMonitor monitor = new CpuMonitor();

        ConsoleDashboard dashboard = new ConsoleDashboard();
        OverloadStats stats = new OverloadStats();

        Console.WriteLine("\nПодписываем обработчики...");
        dashboard.Subscribe(monitor);
        stats.Subscribe(monitor);

        Console.WriteLine("\nЗапускаем мониторинг...");
        monitor.Start();

        Console.WriteLine("\nОтписываем ConsoleDashboard от события OverloadReached...");
        dashboard.UnsubscribeOverload(monitor);

        stats.Report();

        Console.WriteLine("\nОтписываем все обработчики...");
        dashboard.UnsubscribeAll(monitor);
        stats.Unsubscribe(monitor);

        Console.WriteLine("\nМониторинг завершён.");
    }
}