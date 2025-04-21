using System;

namespace Pasjans;

public static class Debug
{
    private static List<string> historia = new();
    public static int seed = 0;

    public static void Add(string Move)
    {
        historia.Add(Move);
    }
    public static void Save()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, @"error.txt");

        string history = "";

        foreach (string s in historia)
        {
            history += $"[{historia.IndexOf(s)}] {s}\n";
        }

        File.AppendAllText(filePath, $"  ____                      \n / ___| __ _ _ __ ___   ___ \n| |  _ / _` | '_ ` _ \\ / _ \\n| |_| | (_| | | | | | |  __/\n \\____|\\__,_|_| |_| |_|\\___|\n\n\nBug reported!\nSeed: {seed.ToString()}\n{history}");
    }
}