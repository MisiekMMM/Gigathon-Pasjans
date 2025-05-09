using System;

namespace Pasjans;

/// <summary>
/// Odpowiada za tworzenie i tasowanie talii
/// </summary>
public static class Talia
{
    /// <summary>
    /// Seed
    /// </summary>
    private static int seed;

    /// <summary>
    /// Generuje standardową talię 52 kart
    /// </summary>
    /// <returns>Talia</returns>
    public static List<Karta> GenerujTalie()
    {
        List<Karta> talia = new();
        //Tworzenie talii kart w Liście
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Karo")); //dodawanie kart od 1 do 13, gdzie 11 to J 12 to Q a 13 to K

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, false, "Pik"));

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Kier"));

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, false, "Trefl"));

        }
        return talia;
    }

    /// <summary>
    /// Losowo tasuje karty używając algorytmu fisher yates
    /// </summary>
    /// <param name="kartas"></param>
    /// <param name="czySeed"></param>
    /// <param name="seedPodany">Seed</param>
    /// <returns></returns>
    static public List<Karta> Tasuj(List<Karta> kartas, bool czySeed, int seedPodany = 0)
    {
        Random rnd = new Random();

        if (czySeed)
        {
            seed = seedPodany;
        }
        else
        {
            seed = rnd.Next(int.MinValue, int.MaxValue);
        }

        rnd = new(seed);
        List<Karta> shuffled = kartas;
        int n = shuffled.Count;

        // Fisher-Yates
        for (int i = n - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);

            Karta temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        return shuffled;
    }
}