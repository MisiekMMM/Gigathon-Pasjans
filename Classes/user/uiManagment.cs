using System;

namespace Pasjans;

/// <summary>
/// odpowiada za interfejs użytkownika
/// </summary>
public static class UI
{
    /// <summary>
    /// drukuje tekst na kolorowo z zamianą tekstu na emoji.
    /// </summary>
    /// <param name="Text">tekst do wydrukowania</param>
    private static void printInColor(string Text)
    {
        if ((bool)Ustawienia.wartosci!["Emotki"])
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
        else
        {
            if (Text.Contains("Karo") || Text.Contains("Kier"))//Kier i karo 
            {
                Console.ForegroundColor = ConsoleColor.Red; // ustawianie na czerwony kolor
                Console.Write($"{Text,-10}"); //Zamiana słownego koloru na emotkę
                Console.ForegroundColor = ConsoleColor.Black; //Zmiana koloru ponownie na czarny
            }
            else if (Text.Contains("Pik") || Text.Contains("Trefl"))//Pik i trefl
            {
                Console.ForegroundColor = ConsoleColor.Black;//Zmiana koloru na czarny (na wszelki wypadek)
                Console.Write($"{Text,-10}"); //Zamiana słownego koloru na emotkę
            }
            else
            {
                throw new Exception("Nieznany kolor");
            }
        }

    }
    /// <summary>
    /// renderuje grę
    /// </summary>
    /// <param name="gra">gra</param>
    /// <param name="czyKoniec">czy wszystkie karty zostały odkryte</param>
    /// <param name="advice">napis który wyświetli się na dole ekranu</param>
    public static void UpdateUi(Gra gra, bool czyKoniec = false, string advice = "")
    {//Ta metoda renderuje karty w terminalu 

        string symbolZakryty = "x";

        Utilities.Clear();

        Console.ForegroundColor = ConsoleColor.Black; //zmiana koloru na czarny


        OdkryjKarty(ref gra.siatka!); //odkrywa karty na spodzie stosu


        if (gra.rezerwa!.Count != 0)//jeżeli pierwsza karta z rezerwy istnieje:
            Console.Write("     +     "); //Drukuje + który udaje rezerwę
        else
            Console.Write("          ");

        if (gra.rezerwaOdkryta!.Count != 0)  //jeżeli pierwsza karta z odkrytej rezerwy istnieje:
            printInColor(gra.rezerwaOdkryta[0].nazwa);    //drukuje nazwę
        else
            Console.Write("          ");    //Pusta przestrzeń

        Console.Write("\t\t");




        if (gra.kartyGora![0, 0] != null)  //Renderuje fundamenty
        {
            printInColor(gra.kartyGora[znajdzOstatniaKarte(gra.kartyGora, 0), 0].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♥",-10}");
        }
        if (gra.kartyGora[0, 1] != null)  //Renderuje fundamenty
        {
            printInColor(gra.kartyGora[znajdzOstatniaKarte(gra.kartyGora, 1), 1].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♦",-10}");
        }
        if (gra.kartyGora[0, 2] != null)  //Renderuje fundamenty
        {
            printInColor(gra.kartyGora[znajdzOstatniaKarte(gra.kartyGora, 2), 2].nazwa);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"♣",-10}");
        }
        if (gra.kartyGora[0, 3] != null)  //Renderuje fundamenty
        {
            printInColor(gra.kartyGora[znajdzOstatniaKarte(gra.kartyGora, 3), 3].nazwa);
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
                    Karta karta = gra.siatka[wiersz, kolumna];

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
                if (!areAllEmpty(gra.siatka, wiersz))
                {
                    Console.WriteLine();
                }
            }


        }
        catch (Exception ex)
        {
            Utilities.Blad("Błąd podczas renderowania siatki!", "Spróbuj zrestartować grę!", ex);
        }

        if (czyKoniec)
        {
            Console.WriteLine("\n\n" + advice + "\nWpisz \"wygrana\" aby zakończyć grę"); //Napisanie porady pod siatką
        }
        else
        {
            Console.WriteLine("\n\n" + advice);
        }
    }
    ///<summary>
    /// ta metoda odkrywa karty na końcu każdej kolumny
    /// </summary>
    private static void OdkryjKarty(ref Karta[,] siatka)
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
    /// <summary>
    /// znajduje ostatnią kartę w kolumnie i zwraca jej wiersz
    /// </summary>
    /// <param name="siatka">siatka</param>
    /// <param name="kolumna">kolumna</param>
    /// <returns>wiersz</returns>
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

    ///<summary>
    /// sprawdza czy wszystkie karty w wierszu poniżej są puste
    /// </summary>
    private static bool areAllEmpty(Karta[,] siatka, int i)
    {
        for (int i2 = 0; i2 < 7; i2++)
        {
            if (i + 1 < siatka.GetLength(0) && siatka[i + 1, i2] != null) //Jeżeli index nie wychodzi poza zakres tablicy i karta pod nie jest nullem:
            {
                return false; //zwracamy false
            }
        }
        return true;
    }
}