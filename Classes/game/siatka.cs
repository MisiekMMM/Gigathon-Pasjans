using System;

namespace Pasjans;

/// <summary>
/// Odpowiada za tworzenie siatki i przeszukiwanie jej
/// </summary>
public static class Siatka
{
    /// <summary>
    /// Wytwarza siatkę z potasowanej talii
    /// </summary>
    /// <param name="kartas">talia</param>
    /// <param name="rezerwa">Oddaje rezerwę kart</param>
    /// <returns>Zwraca siatkę kart</returns>
    public static Karta[,] ZrobSiatke(List<Karta> kartas, out List<Karta> rezerwa)
    {
        Karta[,] siatka = new Karta[19, 7]; // 7 kolumn, 19 wierszy (maksymalna wysokość)

        int indexKarty = 0;

        for (int kolumna = 0; kolumna < 7; kolumna++)
        {
            for (int wiersz = 0; wiersz <= kolumna; wiersz++) // W każdej kolumnie o 1 więcej kart
            {
                siatka[wiersz, kolumna] = kartas[indexKarty];
                indexKarty++;
            }
        }
        rezerwa = new List<Karta>();
        for (int i = 28; i <= 51; i++)
        {
            rezerwa.Add(kartas[i]);
        }

        return siatka;
    }
    /// <summary>
    /// Sprawdza czy karta jest ostatnie w kolumnie
    /// </summary>
    /// <param name="siatka">siatka kart</param>
    /// <param name="wiersz">wiersz danej karty</param>
    /// <param name="kolumna">kolumna danej karty</param>
    public static bool CzyOstatni(Karta[,] siatka, int wiersz, int kolumna)
    {
        for (int i = wiersz + 1; i < 19; i++)
        {
            if (siatka[i, kolumna] != null)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Znajduje kartę o danej nazwie
    /// </summary>
    /// <param name="nazwa">Nazwa karty</param>
    /// <param name="siatka">Siatka</param>
    /// <param name="rezerwaOdkryta">Rezerwa odkryta</param>
    /// <param name="kartyGora">Stosy końcowe</param>
    /// <param name="wiersz">Oddahe wiersz, w którym znajduje się karta</param>
    /// <param name="kolumna">Oddaje kolumnę, w której znajduje się karta</param>
    /// <returns>0 - karta jest zakryta, 1 - karta znajduje sie w siatce, 2 - karta znajduje się na stosie końcowym, 3 - karta znajduje się w rezerwie</returns>
    public static int ZnajdzKarte(string nazwa, Karta[,] siatka, List<Karta> rezerwaOdkryta, Karta[,] kartyGora, out int wiersz, out int kolumna)//ta metoda przeszukuje siatkę, aby znaleźć specyficzną kartę i zwrócić jej pozycję
    {
        wiersz = 0;
        kolumna = 0;
        for (wiersz = 0; wiersz < 19; wiersz++) //sprawdzenie siatki
        {
            for (kolumna = 0; kolumna < 7; kolumna++)
            {
                if (siatka[wiersz, kolumna] != null && siatka[wiersz, kolumna].nazwa.ToLower() == nazwa.ToLower() && siatka[wiersz, kolumna].odkryta)
                {
                    return 1;
                }
            }
        }
        for (kolumna = 0; kolumna < 4; kolumna++) //sprawdzenie stosów końcowych
        {
            wiersz = znajdzOstatniaKarte(kartyGora, kolumna);
            if (kartyGora[wiersz, kolumna] != null && kartyGora[wiersz, kolumna].nazwa.ToLower() == nazwa.ToLower())
            {
                return 2;
            }
        }
        if (rezerwaOdkryta.Count > 0 && rezerwaOdkryta[0].nazwa.ToLower() == nazwa.ToLower())//sprawdzenie rezerwy
        {
            return 3;
        }

        return 0;
    }
    /// <summary>
    /// Znajduje ostatnią kartę w kolumnie
    /// </summary>
    /// <param name="siatka">Siatka</param>
    /// <param name="kolumna">Kolumna</param>
    /// <returns>Wiersz siatki</returns>
    public static int znajdzOstatniaKarte(Karta[,] siatka, int kolumna)
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
    /// ta metoda odkrywa karty na końcu każdej kolumny
    /// </summary>
    public static void OdkryjKarty(ref Karta[,] siatka)
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
}