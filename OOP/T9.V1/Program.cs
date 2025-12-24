using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Перечисление редкости предметов
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

// Класс эффекта (баф/дебаф)
public class Effect
{
    public string Code { get; }
    public int Magnitude { get; }

    public Effect(string code, int magnitude)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Код эффекта не может быть пустым");

        Code = code;
        Magnitude = magnitude;
    }

    public override string ToString()
    {
        return $"{Code}: {Magnitude}";
    }
}

// Класс предмета
public class Item
{
    public string Id { get; }
    public string Name { get; }
    public Rarity Rarity { get; }

    private readonly List<Effect> _effects = new List<Effect>();

    public IReadOnlyList<Effect> Effects => _effects.AsReadOnly();

    public Item(string id, string name, Rarity rarity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id не может быть пустым");

        Id = id;
        Name = name;
        Rarity = rarity;
    }

    internal void AddEffect(Effect effect)
    {
        _effects.Add(effect);
    }

    public override string ToString()
    {
        return $"{Name} ({Id}) - {Rarity}";
    }
}

// Класс инвентаря
public class Inventory : IEnumerable<Item>
{
    private readonly List<Item> _items = new List<Item>();
    private readonly Dictionary<string, Item> _byId = new Dictionary<string, Item>();

    public int Count => _items.Count;

    public Item this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _items[index];
        }
    }

    public Item this[string id]
    {
        get
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (!_byId.ContainsKey(id))
                throw new KeyNotFoundException($"Предмет с Id '{id}' не найден");

            return _byId[id];
        }
    }

    // Добавление предмета
    public void Add(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (_byId.ContainsKey(item.Id))
            throw new ArgumentException($"Предмет с Id '{item.Id}' уже существует");

        _items.Add(item);
        _byId.Add(item.Id, item);
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count)
            return false;

        var item = _items[index];
        _items.RemoveAt(index);
        _byId.Remove(item.Id);

        return true;
    }

    public bool RemoveById(string id)
    {
        if (id == null || !_byId.ContainsKey(id))
            return false;

        var item = _byId[id];
        _items.Remove(item);
        _byId.Remove(id);

        return true;
    }

    public IEnumerable<Item> EnumerateByRarity(Rarity minRarity)
    {
        foreach (var item in _items)
        {
            if (item.Rarity >= minRarity)
                yield return item;
        }
    }

    // Реализация IEnumerable для поддержки foreach
    public IEnumerator<Item> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Инвентарь ===\n");

        Inventory inventory = new Inventory();

        Item sword = new Item("sword_001", "Стальной меч", Rarity.Uncommon);
        sword.AddEffect(new Effect("ATK", 15));
        sword.AddEffect(new Effect("CRIT", 5));

        Item shield = new Item("shield_001", "Дубовый щит", Rarity.Common);
        shield.AddEffect(new Effect("DEF", 10));

        Item potion = new Item("potion_001", "Зелье здоровья", Rarity.Rare);
        potion.AddEffect(new Effect("HP", 50));

        Item amulet = new Item("amulet_001", "Амулет силы", Rarity.Epic);
        amulet.AddEffect(new Effect("ATK", 25));
        amulet.AddEffect(new Effect("HP", 30));
        amulet.AddEffect(new Effect("DEF", 15));

        Console.WriteLine("Добавляем предметы в инвентарь:");
        inventory.Add(sword);
        inventory.Add(shield);
        inventory.Add(potion);
        inventory.Add(amulet);
        Console.WriteLine($"Всего предметов: {inventory.Count}\n");

        Console.WriteLine("Доступ по индексу:");
        for (int i = 0; i < inventory.Count; i++)
        {
            Console.WriteLine($"  [{i}] {inventory[i]}");
        }
        Console.WriteLine();

        Console.WriteLine("Доступ по Id:");
        try
        {
            Console.WriteLine($"  'sword_001': {inventory["sword_001"]}");
            Console.WriteLine($"  'amulet_001': {inventory["amulet_001"]}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"  Ошибка: {ex.Message}");
        }
        Console.WriteLine();

        Console.WriteLine("Эффекты предметов:");
        foreach (var item in inventory)
        {
            Console.WriteLine($"  {item.Name}:");
            foreach (var effect in item.Effects)
            {
                Console.WriteLine($"    - {effect}");
            }
        }
        Console.WriteLine();

        Console.WriteLine("Предметы с редкостью Rare и выше:");
        foreach (var item in inventory.EnumerateByRarity(Rarity.Rare))
        {
            Console.WriteLine($"  {item}");
        }
        Console.WriteLine();

        Console.WriteLine("Предметы с редкостью Uncommon и выше:");
        foreach (var item in inventory.EnumerateByRarity(Rarity.Uncommon))
        {
            Console.WriteLine($"  {item}");
        }
        Console.WriteLine();

        // Удаление предметов
        Console.WriteLine("Удаление предметов:");
        Console.WriteLine($"Удаляем по индексу 1 (щит): {inventory.RemoveAt(1)}");
        Console.WriteLine($"Удаляем по Id 'amulet_001': {inventory.RemoveById("amulet_001")}");
        Console.WriteLine($"Удаляем несуществующий Id: {inventory.RemoveById("not_exist")}");
        Console.WriteLine($"Осталось предметов: {inventory.Count}\n");

        Console.WriteLine("Проверка синхронизации после удаления:");
        Console.WriteLine("Список предметов:");
        for (int i = 0; i < inventory.Count; i++)
        {
            Console.WriteLine($"  [{i}] {inventory[i]}");
        }
        Console.WriteLine("\nДоступ по Id оставшихся предметов:");
        try
        {
            Console.WriteLine($"  'sword_001': {inventory["sword_001"]}");
            Console.WriteLine($"  'potion_001': {inventory["potion_001"]}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== ВСЕ ===");
    }
}