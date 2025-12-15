using System;

public class Book
{
    public string Title;
    public string Author;
    public int TotalPages;
    public int ReadPages;

    public Book (string title, string author, int totalPages, int readPages)
    {
        Title = title;
        Author = author;
        TotalPages = totalPages;
        ReadPages = readPages;
    }

    public void Read (int pages)
    {
        ReadPages = ReadPages + pages;
    }

    public override string ToString ()
    {
        return $"Title: {Title}, Author: {Author}, Total Pages: {TotalPages}, Read: {ReadPages}";
    }
}

class Program
{
    static void Main ()
    {
        Book Mybook = new Book ("Mass effect: Retribution", "Drew Karpyshyn", 320, 0);

        Console.WriteLine (Mybook);

        Mybook.Read (1);
        Mybook.Read (10);
        Mybook.Read (100);

        Console.WriteLine (Mybook);
    }
}