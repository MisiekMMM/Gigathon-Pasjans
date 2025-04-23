using System;

namespace Pasjans;

public static class UI
{
    private static void printInColor(string Text)
    {
        if (Text.Contains("Karo") || Text.Contains("Kier"))//Kier i karo 
        {
            Console.ForegroundColor = ConsoleColor.Red; // ustawianie na czerwony kolor
            Console.Write($"{Text.Replace("Karo", "♦").Replace("Kier", "♥"),-10}"); //Zamiana słownego koloru na emotkę
            Console.ForegroundColor = ConsoleColor.Black; //Zmiana koloru ponownie na czarny
        }
        else if (Text.Contains("Pik") || Text.Contains("Trefl"))//Pik i trefl
        {
            Console.ForegroundColor = ConsoleColor.Black;//Zmiana koloru na czarny (na wszelki wypadek)
            Console.Write($"{Text.Replace("Pik", "♠").Replace("Trefl", "♣"),-10}"); //Zamiana słownego koloru na emotkę

        }
        else
        {//Obsługa błędu
            throw new Exception("Nieznany kolor!");
        }
    }
    public static void UpdateUi(Karta[,] siatka, List<Karta> rezerwaOdk, Karta[,] gora, List<Karta> rezerwa, string advice = "")  //parametr advice wyświetla się u góry ekranu
    {//Ta metoda renderuje karty w terminalu 

        string symbolZakryty = "x"; //ustawienie symbolu zakrytej karty. Do zmiany w ustawieniach

        Utilities.Clear();

        Console.ForegroundColor = ConsoleColor.Black; //zmiana koloru na czarny


        OdkryjKarty(ref siatka); //odkrywa karty na spodzie stosu


        if (rezerwa.Count != 0)//jeżeli pierwsza karta z rezerwy istnieje:
            Console.Write("     +     "); //Drukuje + który udaje rezerwę
        else
            Console.Write("          ");

        if (rezerwaOdk.Count != 0)  //jeżeli pierwsza karta z odkrytej rezerwy istnieje:
            printInColor(rezerwaOdk[0].nazwa);    //drukuje nazwę
        else
            Console.Write("          ");    //Pusta przestrzeń

        Console.Write("\t\t");




        if (gora[0, 0] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 0), 0].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♥",-10}");
        }
        if (gora[0, 1] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 1), 1].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♦",-10}");
        }
        if (gora[0, 2] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 2), 2].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♣",-10}");
        }
        if (gora[0, 3] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 3), 3].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♠",-10}");
        }


        Console.Write("\n\n");

        for (int i = 0; i < 7; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{i,-10}");
        }
        Console.Write("\n");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write("-");
            }
        }
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("\n");
        try
        {
            for (int wiersz = 0; wiersz < 19; wiersz++)
            {
                for (int kolumna = 0; kolumna < 7; kolumna++)
                {
                    Karta karta = siatka[wiersz, kolumna];

                    if (karta != null)
                    {
                        if (karta.odkryta)
                        {
                            printInColor(karta.nazwa);
                        }
                        else
                        {
                            Console.Write($"{symbolZakryty,-10}");
                        }

                    }
                    else
                    {
                        Console.Write("          ");
                    }
                }
                if (!areAllEmpty(siatka, wiersz))
                {
                    Console.WriteLine();
                }
            }


        }
        catch (Exception ex)
        {
            Utilities.Blad("Błąd podczas renderowania siatki!", "Spróbuj zrestartować grę!", ex);
        }

        Console.WriteLine("\n\n" + advice); //Napisanie porady pod siatką
    }
    private static void OdkryjKarty(ref Karta[,] siatka)//ta metoda odkrywa karty na końcu każdej kolumny
    {
        for (int kolumna = 0; kolumna < 7; kolumna++) //kolumny
        {
            for (int wiersz = 0; wiersz < 19; wiersz++) //wiersze
            {
                Karta karta = siatka[wiersz, kolumna];
                if (karta != null)
                {
                    if (wiersz + 1 < siatka.GetLength(0) && siatka[wiersz + 1, kolumna] == null) // jeżeli karta poniżej nie wychodzi za index tablicy i jest pusta:
                    {
                        siatka[wiersz, kolumna].odkryta = true;  //Karta zostaje odkrtyta
                    }
                }

            }
        }

    }
    private static int znajdzOstatniaKarte(Karta[,] siatka, int kolumna)
    {
        for (int i = 0; i < siatka.GetLength(0); i++)
        {
            if (siatka[i + 1, kolumna] == null && i + 1 < siatka.GetLength(0))
            {
                return i;
            }
        }
        return -1;
    }
    private static bool areAllEmpty(Karta[,] siatka, int i)// ta metoda sprawdza, czy wszystkie karty w poniższy
    {
        bool areAllEmpty = true;
        for (int i2 = 0; i2 < 7; i2++)
        {
            if (i + 1 < siatka.GetLength(0) && siatka[i + 1, i2] != null) //Jeżeli index nie wychodzi poza tablicę i karta pod nie jest nullem:
            {
                return false; //zwracamy false
            }
        }
        return areAllEmpty;
    }
}