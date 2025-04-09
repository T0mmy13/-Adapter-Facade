using System;


//Adapter
public interface IOldPaymentSystem
{
    void ProcessPayment(decimal amount);
}

public class LegacyPaymentSystem : IOldPaymentSystem
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Оплата через старую систему: {amount} руб.");
    }
}

public class NewPaymentService
{
    public void Pay(decimal amount, string currency)
    {
        Console.WriteLine($"Оплата через новую систему: {amount} {currency}");
    }
}

public class NewPaymentAdapter : IOldPaymentSystem
{
    private readonly NewPaymentService _newService;

    public NewPaymentAdapter(NewPaymentService newService)
    {
        _newService = newService;
    }

    public void ProcessPayment(decimal amount)
    {
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

class Program
{
    static void Main()
    {
        Console.WriteLine("Adapter:");
        IOldPaymentSystem oldSystem = new LegacyPaymentSystem();
        oldSystem.ProcessPayment(1000);

        var newService = new NewPaymentService();
        IOldPaymentSystem adaptedSystem = new NewPaymentAdapter(newService);
        adaptedSystem.ProcessPayment(1000);
        Console.WriteLine();

        Console.WriteLine("Facade:");
        var amp = new Amplifier();
        var projector = new Projector();
        var screen = new Screen();

        var theater = new HomeTheaterFacade(amp, projector, screen);
        theater.StartMovie();
        theater.EndMovie();
    }
}