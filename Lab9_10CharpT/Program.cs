using System;

class MarketEventArgs : EventArgs
{
    public string Message { get; private set; }

    public MarketEventArgs(string message)
    {
        Message = message;
    }
}

class Market
{
    public event EventHandler<MarketEventArgs> BullMarketChange;
    public event EventHandler<MarketEventArgs> BearMarketChange;

    protected virtual void OnBullMarketChange(MarketEventArgs e)
    {
        BullMarketChange?.Invoke(this, e);
    }

    protected virtual void OnBearMarketChange(MarketEventArgs e)
    {
        BearMarketChange?.Invoke(this, e);
    }

    public void SimulateMarket(int days)
    {
        Random random = new Random();
        for (int day = 1; day <= days; day++)
        {
            double change = random.NextDouble() * (random.Next(0, 2) == 0 ? -1 : 1);
            string message = $"Day {day}: Market change is {change:P2}";

            if (change > 0)
            {
                OnBullMarketChange(new MarketEventArgs(message));
            }
            else
            {
                OnBearMarketChange(new MarketEventArgs(message));
            }
        }
    }
}

class Bull
{
    public void OnBullMarketChange(object sender, MarketEventArgs e)
    {
        Console.WriteLine($"Bull: Market is up. Message: {e.Message}");
    }
}

class Bear
{
    public void OnBearMarketChange(object sender, MarketEventArgs e)
    {
        Console.WriteLine($"Bear: Market is down. Message: {e.Message}");
    }
}

class OverflowDemo
{
    public static void CheckOverflow()
    {
        try
        {
            Console.Write("Введіть перше ціле число: ");
            int num1 = int.Parse(Console.ReadLine());
            Console.Write("Введіть друге ціле число: ");
            int num2 = int.Parse(Console.ReadLine());

            int result = MultiplyAndCube(num1, num2);
            Console.WriteLine($"Результат піднесення в куб добутку {num1} і {num2} дорівнює {result}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"Помилка переповнення: {ex.Message}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Некоректний формат числа.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася невідома помилка: {ex.Message}");
        }
    }

    public static int MultiplyAndCube(int a, int b)
    {
        try
        {
            int product = checked(a * b); // Перевірка переповнення при множенні
            int cube = checked(product * product * product); // Перевірка переповнення при піднесенні в куб
            return cube;
        }
        catch (OverflowException ex)
        {
            throw new OverflowException("Переповнення при піднесенні в куб добутку двох чисел", ex);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Перевірка на переповнення при піднесенні в куб");
            Console.WriteLine("2. Моделювання ринку (Бики й Ведмеді)");
            Console.WriteLine("0. Вихід\n");
            Console.Write("Виберіть опцію: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                continue;
            }

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Програма завершила роботу.");
                    return;
                case 1:
                    OverflowDemo.CheckOverflow();
                    break;
                case 2:
                    SimulateMarketLife();
                    break;
                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void SimulateMarketLife()
    {
        try
        {
            Console.Write("Введіть тривалість моделювання (кількість днів): ");
            int days = int.Parse(Console.ReadLine());

            Market market = new Market();
            Bull bull = new Bull();
            Bear bear = new Bear();

            market.BullMarketChange += bull.OnBullMarketChange;
            market.BearMarketChange += bear.OnBearMarketChange;

            market.SimulateMarket(days);
        }
        catch (FormatException)
        {
            Console.WriteLine("Некоректний формат числа днів.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася невідома помилка: {ex.Message}");
        }
    }
}
