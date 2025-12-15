using System;

// Класс персонажа
public class CharacterContext
{
    public int Health;
    public int Armor;
    public bool Poisoned;

    public CharacterContext(int health, int armor, bool poisoned)
    {
        Health = health;
        Armor = armor;
        Poisoned = poisoned;
    }
}

public delegate void EffectHandler(CharacterContext context);

// Система эффектов
public class EffectSystem
{
    public EffectHandler EffectHandler { get; set; }

    public void Run(CharacterContext ctx)
    {
        EffectHandler?.Invoke(ctx);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== СОЗДАНИЕ ПЕРСОНАЖА ===");

        // Ввод параметров персонажа
        Console.Write("Введите начальное здоровье персонажа: ");
        int health = int.Parse(Console.ReadLine());

        Console.Write("Введите начальную броню персонажа: ");
        int armor = int.Parse(Console.ReadLine());

        Console.Write("Персонаж отравлен изначально? (да/нет): ");
        bool poisoned = Console.ReadLine().ToLower() == "да";

        CharacterContext character = new CharacterContext(health, armor, poisoned);

        Console.WriteLine("\n=== НАЧАЛЬНОЕ СОСТОЯНИЕ ===");
        Console.WriteLine($"Здоровье: {character.Health}, Броня: {character.Armor}, Отравлен: {character.Poisoned}");

        EffectSystem effectSystem = new EffectSystem();

        Console.WriteLine("\n=== ЧАСТЬ A: ДЕЛЕГАТ С ЭФФЕКТАМИ ===");

        EffectHandler handler = null;

        Console.WriteLine("Добавляем эффект регенерации (здоровье +10)");
        handler += Regenerate;

        Console.WriteLine("Добавляем эффект отравления (здоровье -5, статус: отравлен)");
        handler += ApplyPoison;

        Console.Write("Введите сколько брони добавляет щит: ");
        int shieldAmount = int.Parse(Console.ReadLine());

        Console.WriteLine($"Добавляем эффект щита (броня +{shieldAmount})");
        handler += (ctx) =>
        {
            ctx.Armor += shieldAmount;
            Console.WriteLine($"  + Добавлен щит: броня +{shieldAmount}");
        };

        effectSystem.EffectHandler = handler;
        effectSystem.Run(character);

        Console.WriteLine($"\nПосле всех эффектов: Здоровье: {character.Health}, Броня: {character.Armor}, Отравлен: {character.Poisoned}");

        Console.WriteLine("\n=== ЧАСТЬ B: ДОБАВЛЕНИЕ И УДАЛЕНИЕ ОБРАБОТЧИКОВ ===");

        // Создаём нового персонажа для теста
        Console.WriteLine("\nСоздаём нового персонажа для теста:");
        Console.Write("Введите здоровье нового персонажа: ");
        int health2 = int.Parse(Console.ReadLine());

        Console.Write("Введите броню нового персонажа: ");
        int armor2 = int.Parse(Console.ReadLine());

        Console.Write("Новый персонаж отравлен изначально? (да/нет): ");
        bool poisoned2 = Console.ReadLine().ToLower() == "да";

        CharacterContext character2 = new CharacterContext(health2, armor2, poisoned2);

        Console.WriteLine($"\nНачальное состояние нового персонажа: Здоровье: {character2.Health}, Броня: {character2.Armor}, Отравлен: {character2.Poisoned}");
        EffectHandler handler2 = null;
        string userInput;

        // Меню для добавления/удаления эффектов
        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ УПРАВЛЕНИЯ ЭФФЕКТАМИ ===");
            Console.WriteLine("1. Добавить эффект регенерации (+10 здоровья)");
            Console.WriteLine("2. Добавить эффект отравления (-5 здоровья, статус отравлен)");
            Console.WriteLine("3. Добавить эффект щита");
            Console.WriteLine("4. Удалить эффект отравления");
            Console.WriteLine("5. Применить текущие эффекты");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите действие: ");

            userInput = Console.ReadLine();

            if (userInput == "6") break;

            switch (userInput)
            {
                case "1":
                    handler2 += Regenerate;
                    Console.WriteLine("Эффект регенерации добавлен");
                    break;

                case "2":
                    handler2 += ApplyPoison;
                    Console.WriteLine("Эффект отравления добавлен");
                    break;

                case "3":
                    Console.Write("Введите сколько брони добавляет щит: ");
                    int shieldValue = int.Parse(Console.ReadLine());
                    handler2 += (ctx) =>
                    {
                        ctx.Armor += shieldValue;
                        Console.WriteLine($"  + Сработал щит: броня +{shieldValue}");
                    };
                    Console.WriteLine($"Эффект щита (+{shieldValue} брони) добавлен");
                    break;

                case "4":
                    handler2 -= ApplyPoison;
                    Console.WriteLine("Эффект отравления удалён");
                    break;

                case "5":
                    if (handler2 != null)
                    {
                        effectSystem.EffectHandler = handler2;
                        effectSystem.Run(character2);
                        Console.WriteLine($"\nТекущее состояние: Здоровье: {character2.Health}, Броня: {character2.Armor}, Отравлен: {character2.Poisoned}");
                    }
                    else
                    {
                        Console.WriteLine("Нет активных эффектов!");
                    }
                    break;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        Console.WriteLine("\n=== ФИНАЛЬНОЕ СОСТОЯНИЕ ПЕРСОНАЖА ===");
        Console.WriteLine($"Здоровье: {character2.Health}, Броня: {character2.Armor}, Отравлен: {character2.Poisoned}");
    }

    // Эффект регенерации
    static void Regenerate(CharacterContext ctx)
    {
        ctx.Health += 10;
        Console.WriteLine("  + Регенерация: здоровье +10");
    }

    // Эффект отравления
    static void ApplyPoison(CharacterContext ctx)
    {
        ctx.Health -= 5;
        ctx.Poisoned = true;
        Console.WriteLine("  - Отравление: здоровье -5, статус: отравлен");
    }
}