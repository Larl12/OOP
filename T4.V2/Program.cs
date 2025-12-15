using System;

public class GameEntity
{
    private string name;
    private int health;

    public string Name
    {
        get { return name; }
        set
        {
            if (!string.IsNullOrEmpty (value))
                name = value;
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (value >= 0)
                health = value;
        }
    }

    public GameEntity (string name, int health)
    {
        Name = name;
        Health = health;
    }

    public void TakeDamage (int dmg)
    {
        Health -= dmg;
        if (Health < 0)
            Health = 0;
    }

    public bool IsAlive ()
    {
        return Health > 0;
    }
}

public class Warrior : GameEntity
{
    private int strength;

    public int Strength
    {
        get { return strength; }
        set
        {
            if (value >= 0)
                strength = value;
        }
    }

    public Warrior (string name, int health, int strength) : base(name, health)
    {
        Strength = strength;
    }

    public string SUPERMEGAPUNCH ()
    {
        return $"Warrior uses SUPERMEGAPUNCH for {Strength} damage!!!! *Whoa* (0_o)";
    }
}

public class Villager : GameEntity
{
    private string occupation;

    public string Occupation
    {
        get { return occupation; }
        set
        {
            if (!string.IsNullOrEmpty(value))
                occupation = value;
        }
    }

    public Villager (string name, int health, string occupation) : base(name, health)
    {
        Occupation = occupation;
    }

    public string Work ()
    {
        return $"Villager works as {Occupation}.";
    }
}

class Program
{
    static void Main ()
    {
        Warrior warrior = new Warrior("ADNDD", 1212, 100);
        Villager villager = new Villager("John Marston", 100, "farmer");

        Console.WriteLine(warrior.SUPERMEGAPUNCH ());
        Console.WriteLine(villager.Work ());

        warrior.TakeDamage(30);
        Console.WriteLine($"Warrior {warrior.Name} health: {warrior.Health}");
        Console.WriteLine($"Warrior {warrior.Name} is alive: {warrior.IsAlive ()}");

        Console.WriteLine($"Villager {villager.Name} health: {villager.Health}");
        Console.WriteLine($"Villager {villager.Name} is alive: {villager.IsAlive ()}");
    }
}