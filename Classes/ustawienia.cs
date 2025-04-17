using System;
using System.Linq.Expressions;
using Microsoft.Win32.SafeHandles;

namespace Pasjans;

public static class Ustawienia
{
    public static Dictionary<string, object>? wartosci;
    public static List<string>? ustawienia;
    public static void Otworz()
    {
        try
        {
            Utilities.Clear();

            ustawienia = new() { "Głośność" };
            wartosci = new();
            wartosci.Add("Głośność", 100);

            wartosci = Wczytaj(wartosci);

            int numerSwiatla = 0;

            while (true)
            {
                Utilities.Clear();
                Console.WriteLine(" _   _     _                 _            _       \n| | | |___| |_ __ ___      _(_) ___ _ __ (_) __ _ \n| | | / __| __/ _` \\ \\ /\\ / / |/ _ \\ '_ \\| |/ _` |\n| |_| \\__ \\ || (_| |\\ V  V /| |  __/ | | | | (_| |\n \\___/|___/\\__\\__,_| \\_/\\_/ |_|\\___|_| |_|_|\\__,_|\n\n\n");
                WydrukujListe(ustawienia, numerSwiatla, wartosci);
                Console.WriteLine("\nNawiguj strzałkami, wciśnij X aby wyjść");
                ConsoleKeyInfo CKI = Console.ReadKey(true);
                if (CKI.Key == ConsoleKey.DownArrow && numerSwiatla + 1 < ustawienia.Count)
                {
                    numerSwiatla++;
                }
                else if (CKI.Key == ConsoleKey.UpArrow && numerSwiatla - 1 >= 0)
                {
                    numerSwiatla--;
                }
                else if (CKI.Key == ConsoleKey.Enter)
                {
                    switch (numerSwiatla)
                    {
                        case 0:
                            int[] glosnosci = { 0, 25, 50, 75, 100 };
                            int aktualna = (int)wartosci["Głośność"];
                            int index = Array.IndexOf(glosnosci, aktualna);
                            int nowyIndex = (index + 1) % glosnosci.Length;
                            wartosci["Głośność"] = glosnosci[nowyIndex];

                            break;
                        case 1:
                            wartosci["Emotki"] = !(bool)wartosci["Emotki"];
                            break;

                    }
                }
                else if (CKI.Key == ConsoleKey.X)
                {
                    break;
                }
            }
            Zapisz(wartosci, ustawienia);
            MainMenu.Otworz();
        }
        catch (Exception ex)
        {
            Utilities.Error("Podczas otwierania ustawień pojawił się błąd!", "Pamiętaj, aby używać programu ostrożnie i w sposób, który go nie uszodzi!", ex);
        }

    }
    public static void WydrukujListe(List<string> lista, int numer, Dictionary<string, object> wartosci)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (i == numer)
                Console.ForegroundColor = ConsoleColor.Magenta;
            else
                Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"[{i}] {lista[i]} \t\t{wartosci[lista[i]]}");
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
    public static Dictionary<string, object> Wczytaj(Dictionary<string, object> dict)
    {
        ustawienia = new() { "Głośność" };
        wartosci = new();
        wartosci.Add("Głośność", 100);
        string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, @"ustawienia"));

        Dictionary<string, object> pairs = new();

        foreach (string s in lines)
        {
            string[] dataRead = s.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (dataRead.Length < 2)
                continue;

            object? typeName = wartosci[dataRead[0]];

            Type type = typeName.GetType();

            if (type != null && type == typeof(System.String))
            {
                pairs[dataRead[0]] = dataRead[1].ToString();
            }
            else if (type == typeof(System.Boolean))
            {
                pairs[dataRead[0]] = bool.Parse(dataRead[1]);
            }
            else if (type == typeof(System.Int32))
            {
                pairs[dataRead[0]] = int.Parse(dataRead[1]);
            }


        }

        return pairs;
    }
    public static void Zapisz(Dictionary<string, object> dict, List<string> opcje)
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, @"ustawienia");

        using (StreamWriter sr = new(filePath))
        {
            foreach (string s in opcje)
            {
                sr.WriteLine($"{s} {dict[s]}");
            }
        }
    }
}