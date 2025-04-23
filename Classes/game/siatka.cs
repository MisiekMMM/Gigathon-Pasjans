using System;

namespace Pasjans;

public static class Siatka
{
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
    public static int ZnajdzKarte(string nazwa, Karta[,] siatka, List<Karta> rezerwaOdkryta, Karta[,] kartyGora, out int wiersz, out int kolumna)//ta metoda przeszukuje siatkę, aby znaleźć specyficzną kartę i zwrócić jej pozycję
    {//0 - karta jest zakryta, 1 - karta znajduje sie w siatce, 2 - karta znajduje się na stosie końcowym, 3 - karta znajduje się w rezerwie
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
}