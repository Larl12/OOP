using System;

public class Computer
{
    public string SerialNumber;
    public string OS;
    public string Motherboard;
    public CPU Processor;
    public Memory RAM;

    public Computer (string serialNumber, string os, string motherboard, CPU processor, Memory ram)
    {
        SerialNumber = serialNumber;
        OS = os;
        Motherboard = motherboard;
        Processor = processor;
        RAM = ram;
    }
}

public class CPU
{
    public int Frequency;
    public int Cores;

    public CPU (int frequency, int cores)
    {
        Frequency = frequency;
        Cores = cores;
    }
}

public class Memory
{
    public int Capacity;
    public string Type;

    public Memory (int capacity, string type)
    {
        Capacity = capacity;
        Type = type;
    }
}

class Program
{
    static void Main ()
    {
        CPU cpu = new CPU (4200, 16);
        Memory memory = new Memory (326, "DDR5");
        Computer computer = new Computer ("ADNDDpc-12w12", "Windows 10", "MSI B850", cpu, memory);
    }
}