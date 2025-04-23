using System;

namespace Pasjans;

public static class Utilities
{
    /// <summary>
    /// Błąd
    /// </summary>
    /// <param name="title">wyświetlany tytuł</param>
    /// <param name="Message">wyświetlane ciało wiadomości</param>
    /// <param name="ex">wyjątek</param>
    public static void Blad(string title, string Message, Exception ex)
    {
        Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("      ____  _           _ \n| __ )| | __ _  __| |\n|  _ \\|/// _` |/ _` |\n| |_) //| (_| | (_| |\n|____/|_|\\__,_|\\__,_|\n            (_(      \n\n\n");
        Console.WriteLine(" |\\/\\/\\/|  \n |      |  \n |      |  \n | (o)(o)  \n C      _) \n  | ,___|  \n  |   /    \n /____\\    \n/      \\n\n\n");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"{title}\n");
        Console.WriteLine(Message);
        Debug.Zapisz();
        Console.WriteLine("\nZapisano plik error.txt\nNaciśnij dowolny guzik aby wyjść");
        Console.WriteLine("\n\n" + ex.Message);
        Console.ReadKey();
        Environment.Exit(100);
    }
    /// <summary>
    /// Wyczyszczenie konsoli. Podczas debugowania zakomentować
    /// </summary>
    public static void Clear()
    {
        Console.Clear();
    }
    public static void DrukujLinie()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Black;
    }
}