using System;

public class Character
{
    public string character_name;
    public int health_points;

    public Character (string name, int health)
    {
        character_name = name;
        health_points = health;
    }

    public string attack_description ()
    {
        return "character attacks";
    }
}

public class Warrior : Character
{
    public int strength;

    public Warrior(string name, int health, int str) : base(name, health) // ДЛЯ СЕБЯ, base обращаеться к изначальным параметрам character котороые были введенны в начале, после чего вводиться новая строка, она сможет работать только при условии наличия исходных параметров введенных в начале
    {
        strength = str;
    }

    public new string attack_description ()
    {
        return $"{character_name} attacks enemy {strength} points.";
    }
}

class Program
{
    static void Main ()
    {
        Warrior warrior = new Warrior ("ADNDD", 1212, 100);

        Console.WriteLine(warrior.attack_description ());
    }
}