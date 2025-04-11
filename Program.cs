using System;

namespace Pasjans;

public class Program
{
    public static void Main()// wzór tworzenia karty: Karta(int numer, bool kolor, string kolorSlowny) 
    {
        List<Karta> talia = new List<Karta>();
        //Tworzenie talii kart
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Karo"));

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


    }
}