using System;

namespace Pasjans;

public static class Ustawienia
{
    public static void Otworz()
    {
        Console.Clear();

        switch (AskPreference(["Wyjdź"]))
        {
            case 1:
                MainMenu.Otworz();
                break;
        }
    }

    public static int AskPreference(params List<string> options)
    {
        Console.Clear();
        Console.WriteLine(" _   _     _                 _            _       \n| | | |___| |_ __ ___      _(_) ___ _ __ (_) __ _ \n| | | / __| __/ _` \\ \\ /\\ / / |/ _ \\ '_ \\| |/ _` |\n| |_| \\__ \\ || (_| |\\ V  V /| |  __/ | | | | (_| |\n \\___/|___/\\__\\__,_| \\_/\\_/ |_|\\___|_| |_|_|\\__,_|\n\n\n");
        foreach (string option in options)
        {
            Console.WriteLine($"[{options.IndexOf(option) + 1}] {option}");
        }

        Console.Write("\n");
        string response = Console.ReadLine()!;

        if (int.TryParse(response, out int result) && result > 0 && result <= options.Count)
        {
            return result;
        }

        return AskPreference(options);
    }
}