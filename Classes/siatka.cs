using System;

namespace Pasjans;

public class Siatka
{
    public Karta?[,] siatka { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Siatka(List<Karta> kartas, out List<Karta> rezerwa)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
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
    }
    public void OdkryjKarty()//ta metoda odkrywa karty na końcu każdej kolumny
    {
        for (int kolumna = 0; kolumna < 7; kolumna++) //kolumny
        {
            for (int wiersz = 0; wiersz < 19; wiersz++) //wiersze
            {
                Karta karta = siatka[wiersz, kolumna]!;
                if (karta != null)
                {
                    if (wiersz + 1 < siatka.GetLength(0) && siatka[wiersz + 1, kolumna] == null) // jeżeli karta poniżej nie wychodzi za index tablicy i jest pusta:
                    {
                        siatka[wiersz, kolumna]!.odkryta = true;  //Karta zostaje odkrtyta
                    }
                }

            }
        }

    }
}