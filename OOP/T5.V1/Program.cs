using System;

public interface ICharacter
{
    string Name { get; set; }
    int Health { get; set; }

    void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;
    }
}

public interface IWarrior
{
    int Strength { get; set; }
    int Armor { get; set; }
    void Attack(ICharacter target);
}

public class Villager : ICharacter
{
    private string name;
    private int health;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public Villager(string name, int health)
    {
        Name = name;
        Health = health;
    }
}

public class Knight : ICharacter, IWarrior
{
    private string name;
    private int health;
    private int strength;
    private int armor;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }

    public Knight(string name, int health, int strength, int armor)
    {
        Name = name;
        Health = health;
        Strength = strength;
        Armor = armor;
    }

    public void TakeDamage(int amount)
    {
        int damage = amount - Armor;
        if (damage > 0)
        {
            Health -= damage;
            if (Health < 0)
                Health = 0;
        }
    }

    public void Attack(ICharacter target)
    {
        int initialHealth = target.Health;
        target.TakeDamage(Strength);
        int damageDealt = initialHealth - target.Health;
        Console.WriteLine($"{Name} атакует {target.Name} и наносит {damageDealt} урона");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== СОЗДАНИЕ ПЕРСОНАЖЕЙ ===");

        // Создание крестьянина
        Console.WriteLine("\n--- Крестьянин ---");
        Console.Write("Введите имя крестьянина: ");
        string villagerName = Console.ReadLine();

        Console.Write($"Введите здоровье {villagerName}: ");
        int villagerHealth = int.Parse(Console.ReadLine());

        Console.Write($"Введите силу атаки {villagerName}: ");
        int villagerDamage = int.Parse(Console.ReadLine());

        // Создание рыцаря
        Console.WriteLine("\n--- Рыцарь ---");
        Console.Write("Введите имя рыцаря: ");
        string knightName = Console.ReadLine();

        Console.Write($"Введите здоровье {knightName}: ");
        int knightHealth = int.Parse(Console.ReadLine());

        Console.Write($"Введите силу атаки {knightName}: ");
        int knightStrength = int.Parse(Console.ReadLine());

        Console.Write($"Введите броню {knightName}: ");
        int knightArmor = int.Parse(Console.ReadLine());

        // Создание персонажей
        Villager villager = new Villager(villagerName, villagerHealth);
        Knight knight = new Knight(knightName, knightHealth, knightStrength, knightArmor);

        Console.WriteLine("\n=== НАЧАЛЬНОЕ СОСТОЯНИЕ ===");
        Console.WriteLine($"Крестьянин {villager.Name}: {villager.Health} HP");
        Console.WriteLine($"Рыцарь {knight.Name}: {knight.Health} HP, сила: {knight.Strength}, броня: {knight.Armor}");

        Console.WriteLine("\n=== НАЧАЛО БОЯ ===");

        int round = 1;

        
        while (villager.Health > 0 && knight.Health > 0)
        {
            Console.WriteLine($"\n--- Раунд {round} ---");

            // Ход рыцаря
            if (villager.Health > 0)
            {
                Console.WriteLine($"Ход {knight.Name}:");
                knight.Attack(villager);
                Console.WriteLine($"{villager.Name}: {villager.Health} HP");

                if (villager.Health <= 0)
                {
                    Console.WriteLine($"\n {villager.Name} повержен!");
                    Console.WriteLine($" {knight.Name} ПОБЕДИЛ!");
                    break;
                }
            }

            // Ход крестьянина
            if (knight.Health > 0)
            {
                Console.WriteLine($"\nХод {villager.Name}:");
                int initialKnightHealth = knight.Health;
                knight.TakeDamage(villagerDamage);
                int actualDamage = initialKnightHealth - knight.Health;

                Console.WriteLine($"{villager.Name} наносит {villagerDamage} урона");
                if (actualDamage > 0)
                {
                    Console.WriteLine($"Броня поглотила {villagerDamage - actualDamage} урона");
                    Console.WriteLine($"Фактический урон: {actualDamage}");
                }
                else
                {
                    Console.WriteLine("Урон полностью заблокирован бронёй!");
                }
                Console.WriteLine($"{knight.Name}: {knight.Health} HP");

                if (knight.Health <= 0)
                {
                    Console.WriteLine($"\n {knight.Name} повержен!");
                    Console.WriteLine($" {villager.Name} ПОБЕДИЛ!");
                    break;
                }
            }

            round++;
        }

        Console.WriteLine("\n=== ИТОГИ БОЯ ===");
        Console.WriteLine($"Бой длился {round - 1} раундов");
        Console.WriteLine($"Крестьянин {villager.Name}: {villager.Health} HP {(villager.Health > 0 ? "ПОБЕДИТЕЛЬ" : "ПОБЕЖДЁН")}");
        Console.WriteLine($"Рыцарь {knight.Name}: {knight.Health} HP {(knight.Health > 0 ? "ПОБЕДИТЕЛЬ" : "ПОБЕЖДЁН")}");
    }
}