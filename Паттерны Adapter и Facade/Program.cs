using System;


//Adapter
// Старый интерфейс оплаты (который использует клиент)
public interface IOldPaymentSystem
{
    void ProcessPayment(decimal amount);
}

// Старая реализация оплаты
public class LegacyPaymentSystem : IOldPaymentSystem
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Оплата через старую систему: {amount} руб.");
    }
}

// Новая система оплаты (с несовместимым интерфейсом)
public class NewPaymentService
{
    public void Pay(decimal amount, string currency)
    {
        Console.WriteLine($"Оплата через новую систему: {amount} {currency}");
    }
}

// Адаптер для новой системы (реализует старый интерфейс)
public class NewPaymentAdapter : IOldPaymentSystem
{
    private readonly NewPaymentService _newService;

    public NewPaymentAdapter(NewPaymentService newService)
    {
        _newService = newService;
    }

    public void ProcessPayment(decimal amount)
    {
        // Конвертируем рубли в USD (пример логики адаптера)
        _newService.Pay(amount / 75, "USD");
    }
}

//Facade
public class Amplifier
{
    public void TurnOn() => Console.WriteLine("Усилитель включен");
    public void SetVolume(int level) => Console.WriteLine($"Громкость: {level}%");
}

public class Projector
{
    public void Start() => Console.WriteLine("Проектор запущен");
    public void SetInput(string source) => Console.WriteLine($"Источник: {source}");
}

public class Screen
{
    public void Lower() => Console.WriteLine("Экран опущен");
}

// Фасад для управления кинотеатром
public class HomeTheaterFacade
{
    private readonly Amplifier _amp;
    private readonly Projector _projector;
    private readonly Screen _screen;

    public HomeTheaterFacade(Amplifier amp, Projector projector, Screen screen)
    {
        _amp = amp;
        _projector = projector;
        _screen = screen;
    }

    public void StartMovie()
    {
        _screen.Lower();
        _amp.TurnOn();
        _amp.SetVolume(20);
        _projector.Start();
        _projector.SetInput("HDMI");
        Console.WriteLine("Кино начинается!");
    }

    public void EndMovie()
    {
        Console.WriteLine("Кино завершено. Выключение системы...");
        _amp.SetVolume(0);
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        // Старая система
        Console.WriteLine("Adapter:");
        IOldPaymentSystem oldSystem = new LegacyPaymentSystem();
        oldSystem.ProcessPayment(1000); // 1000 руб.

        // Новая система через адаптер
        var newService = new NewPaymentService();
        IOldPaymentSystem adaptedSystem = new NewPaymentAdapter(newService);
        adaptedSystem.ProcessPayment(1000); // Конвертация в USD
        Console.WriteLine();

        Console.WriteLine("Facade:");
        var amp = new Amplifier();
        var projector = new Projector();
        var screen = new Screen();

        // Используем фасад
        var theater = new HomeTheaterFacade(amp, projector, screen);
        theater.StartMovie();
        theater.EndMovie();
    }
}