using System.Globalization;
using System.Text.Json;

internal interface IHomeworkTask
{
    string Title { get; }
    void Run();
}

internal static class Program
{
    private static void Main()
    {
        IHomeworkTask[] tasks =
        {
            new SkiRentalTask(),
            new AlcoholTrackerTask(),
            new AgePeriodTask(),
            new GladiatorTrainingTask(),
            new BmwOwnershipTask(),
            new BeautySalonTask(),
            new GladiatorEquipmentTask(),
            new GladiatorRecordTask()
        };

        while (true)
        {
            Console.WriteLine("=== Учебные программы на C# ===");
            for (int i = 0; i < tasks.Length; i++)
                Console.WriteLine($"{i + 1}. {tasks[i].Title}");
            Console.WriteLine("0. Выход");

            int choice = ConsoleInput.ReadInt("Выбери номер программы: ", 0, tasks.Length);
            if (choice == 0)
                return;

            Console.WriteLine();
            tasks[choice - 1].Run();
            Console.WriteLine("\nНажми Enter, чтобы вернуться в меню.");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}

internal static class ConsoleInput
{
    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Введи целое число от {min} до {max}.");
        }
    }

    public static decimal ReadDecimal(string prompt, decimal min, decimal max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            string normalized = (input ?? string.Empty).Trim().Replace(',', '.');
            if (decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value)
                && value >= min && value <= max)
                return value;

            Console.WriteLine($"Введи число от {min} до {max}. Десятичную часть можно отделять запятой.");
        }
    }

    public static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Поле не должно быть пустым.");
        }
    }

    public static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (д/н): ");
            string? input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input is "д" or "да" or "y" or "yes") return true;
            if (input is "н" or "нет" or "n" or "no") return false;
            Console.WriteLine("Ответь да или нет.");
        }
    }
}

internal sealed class SkiRentalTask : IHomeworkTask
{
    public string Title => "Аренда лыжного снаряжения";

    public void Run()
    {
        const decimal skiRatePerDay = 1200m;
        const decimal snowboardRatePerDay = 1500m;
        const decimal insurancePrice = 300m;

        Console.WriteLine("Учебные тарифы: лыжи — 1200 сом/день, сноуборд — 1500 сом/день, страховка — 300 сом за заказ.");
        int days = ConsoleInput.ReadInt("Количество дней аренды: ", 1, 365);
        int equipment = ConsoleInput.ReadInt("Снаряжение (1 — лыжи, 2 — сноуборд): ", 1, 2);
        bool insurance = ConsoleInput.ReadYesNo("Добавить страховку?");

        decimal dailyRate = equipment == 1 ? skiRatePerDay : snowboardRatePerDay;
        decimal rentalCost = dailyRate * days;
        decimal total = rentalCost + (insurance ? insurancePrice : 0m);

        Console.WriteLine($"Аренда: {rentalCost:N2} сом");
        Console.WriteLine($"Страховка: {(insurance ? insurancePrice : 0m):N2} сом");
        Console.WriteLine($"Итого: {total:N2} сом");
    }
}

internal sealed class AlcoholTrackerTask : IHomeworkTask
{
    private const decimal EthanolDensityGramsPerMl = 0.789m;

    public string Title => "Учёт алкоголя";

    public void Run()
    {
        Console.WriteLine("Условная порция для расчёта: пиво 330 мл (5%), вино 150 мл (12%), водка 40 мл (40%).");
        Console.WriteLine("Расчёт приблизительный. Универсальной безопасной нормы алкоголя нет.");
        Console.WriteLine("Порог ниже — только выбранный пользователем сигнал уведомления, а не медицинская рекомендация.");

        decimal totalGrams = 0m;
        while (true)
        {
            int type = ConsoleInput.ReadInt("Напиток (1 — пиво, 2 — вино, 3 — водка, 0 — закончить ввод): ", 0, 3);
            if (type == 0) break;

            (string name, decimal volumeMl, decimal alcoholPercent) drink = type switch
            {
                1 => ("Пиво", 330m, 5m),
                2 => ("Вино", 150m, 12m),
                _ => ("Водка", 40m, 40m)
            };

            int servings = ConsoleInput.ReadInt("Количество порций: ", 1, 100);
            decimal grams = servings * drink.volumeMl * (drink.alcoholPercent / 100m) * EthanolDensityGramsPerMl;
            totalGrams += grams;
            Console.WriteLine($"Добавлено примерно {grams:F1} г чистого спирта из напитка «{drink.name}».");
        }

        decimal alertLimit = ConsoleInput.ReadDecimal("Задай порог уведомления в граммах (это не безопасная норма): ", 0.1m, 5000m);
        Console.WriteLine($"Итого по введённым данным: примерно {totalGrams:F1} г чистого спирта.");
        if (totalGrams > alertLimit)
            Console.WriteLine("Предупреждение: выбранный порог уведомления превышен.");
        else
            Console.WriteLine("Выбранный порог уведомления не превышен.");
    }
}

internal sealed class AgePeriodTask : IHomeworkTask
{
    public string Title => "Определение возрастного периода";

    public void Run()
    {
        int age = ConsoleInput.ReadInt("Введи возраст: ", 0, 120);
        string period;
        string recommendation;

        if (age <= 11)
        {
            period = "детство";
            recommendation = "Учись новому, играй и задавай вопросы.";
        }
        else if (age <= 17)
        {
            period = "подростковый возраст";
            recommendation = "Развивай навыки, отдыхай и обращайся за поддержкой к близким.";
        }
        else if (age <= 59)
        {
            period = "взрослая жизнь";
            recommendation = "Старайся поддерживать баланс между делами, отдыхом и общением.";
        }
        else
        {
            period = "старость";
            recommendation = "Поддерживай интерес к занятиям и общению с близкими.";
        }

        Console.WriteLine($"Возрастной период: {period}.");
        Console.WriteLine($"Рекомендация: {recommendation}");
    }
}

internal sealed class GladiatorTrainingTask : IHomeworkTask
{
    private static readonly string[] Days = { "понедельник", "вторник", "среда", "четверг", "пятница", "суббота", "воскресенье" };

    public string Title => "Расчёт тренировок гладиатора";

    public void Run()
    {
        decimal hoursPerDay = ConsoleInput.ReadDecimal("Часов тренировки в выбранный день: ", 0.25m, 12m);
        int type = ConsoleInput.ReadInt("Тип (1 — бой с мечом, 2 — бег, 3 — силовые упражнения): ", 1, 3);
        string trainingType = type switch
        {
            1 => "бой с мечом",
            2 => "бег",
            _ => "силовые упражнения"
        };

        int trainingDays = 0;
        foreach (string day in Days)
            if (ConsoleInput.ReadYesNo($"Тренировка в день «{day}»?")) trainingDays++;

        decimal weeklyHours = hoursPerDay * trainingDays;
        Console.WriteLine($"Тип тренировки: {trainingType}.");
        Console.WriteLine($"Тренировочных дней: {trainingDays} из 7.");
        Console.WriteLine($"Всего за неделю: {weeklyHours:F1} часа.");

        if (trainingDays == 0)
            Console.WriteLine("На этой неделе тренировки не запланированы.");
        else if (weeklyHours >= 20m)
            Console.WriteLine("Рекомендация: распределяй нагрузку по неделе и планируй дни отдыха.");
        else
            Console.WriteLine("Рекомендация: чередуй нагрузку и отдых, особенно после интенсивных тренировок.");
    }
}

internal sealed class BmwOwnershipTask : IHomeworkTask
{
    public string Title => "Стоимость владения BMW M5 F90 за год";

    public void Run()
    {
        decimal consumption = ConsoleInput.ReadDecimal("Средний расход топлива (л/100 км): ", 0m, 100m);
        decimal pricePerLiter = ConsoleInput.ReadDecimal("Цена бензина за литр (сом): ", 0m, 1_000_000m);
        decimal annualMileage = ConsoleInput.ReadDecimal("Годовой пробег (км): ", 0m, 1_000_000m);
        decimal insurance = ConsoleInput.ReadDecimal("Стоимость страховки за год (сом): ", 0m, 100_000_000m);

        decimal litersPerYear = consumption * annualMileage / 100m;
        decimal fuelCost = litersPerYear * pricePerLiter;
        decimal total = fuelCost + insurance;

        Console.WriteLine($"Топлива за год: {litersPerYear:N1} л.");
        Console.WriteLine($"Расходы на топливо: {fuelCost:N2} сом.");
        Console.WriteLine($"Страхование: {insurance:N2} сом.");
        Console.WriteLine($"Итого топливо + страховка: {total:N2} сом.");
        Console.WriteLine("Ремонт, налоги и другие расходы в этот расчёт не входят.");
    }
}

internal sealed class BeautySalonTask : IHomeworkTask
{
    public string Title => "Учёт клиентов салона красоты";

    public void Run()
    {
        string projectDirectory = FindProjectDirectory();
        string dataPath = Path.Combine(projectDirectory, "salon_clients.json");
        List<SalonClient> clients;

        try
        {
            clients = File.Exists(dataPath)
                ? JsonSerializer.Deserialize<List<SalonClient>>(File.ReadAllText(dataPath)) ?? new List<SalonClient>()
                : new List<SalonClient>();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Не удалось прочитать файл клиентов; существующие данные не изменены. {exception.Message}");
            return;
        }

        int count = ConsoleInput.ReadInt("Сколько клиентов добавить сегодня? ", 1, 100);
        DateTime today = DateTime.Today;
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Клиент {i + 1} из {count}:");
            clients.Add(new SalonClient
            {
                Name = ConsoleInput.ReadText("Имя: "),
                Services = ConsoleInput.ReadText("Услуги (например: стрижка, маникюр): "),
                Payment = ConsoleInput.ReadDecimal("Сумма оплаты (сом): ", 0m, 10_000_000m),
                Date = today
            });
        }

        try
        {
            File.WriteAllText(dataPath, JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Не удалось сохранить данные клиентов. {exception.Message}");
            return;
        }

        List<SalonClient> todayClients = clients.FindAll(client => client.Date.Date == today);
        Console.WriteLine($"\nКлиенты за сегодня ({today:dd.MM.yyyy}):");
        for (int i = 0; i < todayClients.Count; i++)
        {
            SalonClient client = todayClients[i];
            Console.WriteLine($"{i + 1}. {client.Name}; услуги: {client.Services}; оплата: {client.Payment:N2} сом.");
        }
        Console.WriteLine("Данные сохраняются локально в salon_clients.json.");
    }

    private static string FindProjectDirectory()
    {
        DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (directory.GetFiles("*.csproj").Length > 0)
                return directory.FullName;
            directory = directory.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}

internal sealed class SalonClient
{
    public string Name { get; set; } = string.Empty;
    public string Services { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
}

internal sealed class GladiatorEquipmentTask : IHomeworkTask
{
    public string Title => "Расчёт экипировки гладиатора";

    public void Run()
    {
        Console.WriteLine("Учебные значения веса и стоимости экипировки:");
        int armorChoice = ConsoleInput.ReadInt("Доспехи (1 — лёгкие, 2 — средние, 3 — тяжёлые): ", 1, 3);
        (string name, decimal weight, decimal price) armor = armorChoice switch
        {
            1 => ("лёгкие", 8m, 8000m),
            2 => ("средние", 15m, 15000m),
            _ => ("тяжёлые", 25m, 24000m)
        };

        int weaponChoice = ConsoleInput.ReadInt("Оружие (1 — меч, 2 — трезубец, 3 — копьё): ", 1, 3);
        (string name, decimal weight, decimal price) weapon = weaponChoice switch
        {
            1 => ("меч", 3m, 5000m),
            2 => ("трезубец", 4m, 6000m),
            _ => ("копьё", 2.5m, 3500m)
        };

        bool helmet = ConsoleInput.ReadYesNo("Добавить шлем?");
        bool shield = ConsoleInput.ReadYesNo("Добавить щит?");
        decimal totalWeight = armor.weight + weapon.weight + (helmet ? 2m : 0m) + (shield ? 5m : 0m);
        decimal totalPrice = armor.price + weapon.price + (helmet ? 3000m : 0m) + (shield ? 4500m : 0m);

        Console.WriteLine($"Доспехи: {armor.name}; оружие: {weapon.name}.");
        Console.WriteLine($"Общий вес: {totalWeight:F1} кг.");
        Console.WriteLine($"Общая стоимость: {totalPrice:N2} сом.");
    }
}

internal sealed class GladiatorRecordTask : IHomeworkTask
{
    public string Title => "Учёт побед и поражений гладиатора";

    public void Run()
    {
        int fightCount = ConsoleInput.ReadInt("Количество боёв для учёта: ", 1, 200);
        int wins = 0;
        int losses = 0;

        for (int i = 0; i < fightCount; i++)
        {
            int result = ConsoleInput.ReadInt($"Бой {i + 1}: (1 — победа, 2 — поражение): ", 1, 2);
            if (result == 1) wins++;
            else losses++;
        }

        decimal winRate = (decimal)wins / fightCount * 100m;
        Console.WriteLine($"Побед: {wins}; поражений: {losses}.");
        Console.WriteLine($"Процент побед: {winRate:F1}%.");

        if (winRate >= 70m)
            Console.WriteLine("Отличный результат! Продолжай тренироваться и сохраняй дисциплину.");
        else if (winRate >= 50m)
            Console.WriteLine("Хороший прогресс. Разбери бои и продолжай улучшать навыки.");
        else
            Console.WriteLine("Не сдавайся: анализируй опыт и готовься к следующим боям.");
    }
}
