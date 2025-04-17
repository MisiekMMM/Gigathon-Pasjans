using System;

namespace Pasjans;

public static class Utilities
{
    public static void Error(string title, string Message, Exception ex)
    {
        Utilities.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("      ____  _           _ \n| __ )| | __ _  __| |\n|  _ \\|/// _` |/ _` |\n| |_) //| (_| | (_| |\n|____/|_|\\__,_|\\__,_|\n            (_(      \n\n\n");
        Console.WriteLine(" |\\/\\/\\/|  \n |      |  \n |      |  \n | (o)(o)  \n C      _) \n  | ,___|  \n  |   /    \n /____\\    \n/      \\n\n\n");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("Przez twoją nieuwagę pojawił się błąd!\n");
        Console.WriteLine(Message);
        Console.WriteLine("\nNaciśnij dowolny guzik aby wyjść");
        Console.WriteLine("\n\n" + ex.Message);
        Console.ReadKey();
        Environment.Exit(100);
    }
    public static void Clear()
    {
        Console.Clear();
    }
}