using System;
using System.Dynamic;

namespace Pasjans;

/// <summary>
/// Ta klasa odpowiada za przechwytywanie historii ruchów i zapisywania jej wraz z seed'em
/// </summary>
public static class Debug
{
    public static List<string>? historia { get; set; }
    public static int seed = 0;

    /// <summary>
    /// Dodaje ruch do historii
    /// </summary>
    /// <param name="Move">ruch do dodania</param>
    public static void Add(string Move)
    {
        historia!.Add(Move);
    }
    /// <summary>
    /// Czyści historię
    /// </summary>
    public static void Clear()
    {
        historia = new();
    }
    public static void Zapisz()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, @"error.txt");

        string history = "";

        foreach (string s in historia!)
        {
            history += $"{s}\n";
        }

        File.WriteAllText(filePath, $"Bug reported!\nSeed: {seed.ToString()}\n{history}");
    }
}