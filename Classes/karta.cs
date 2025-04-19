using System;

namespace Pasjans;

public class Karta  //Ta klasa odpowiada karcie w talii
{
    public string nazwa;

    public int numer; // walet J 11, królowa Q 12, król K 13
    // true czerwony false czarny  
    public bool kolor;

    public bool odkryta;

    public int indexKoloru;

    public Karta(int number, bool color, string colorSlowny) //colorSlowny to Pik, Karo, Trefl, Kier
    {
        kolor = color;
        numer = number;
        odkryta = false;

        if (colorSlowny == "Kier")
        {
            indexKoloru = 0;
        }
        else if (colorSlowny == "Karo")
        {
            indexKoloru = 1;
        }
        else if (colorSlowny == "Trefl")
        {
            indexKoloru = 2;
        }
        else if (colorSlowny == "Pik")
        {
            indexKoloru = 3;
        }
        else
        {
            indexKoloru = -1;
        }
        if (number == 11)
        {
            nazwa = $"J {colorSlowny}";
        }
        else if (number == 12)
        {
            nazwa = $"Q {colorSlowny}";
        }
        else if (number == 13)
        {
            nazwa = $"K {colorSlowny}";
        }
        else if (number == 1)
        {
            nazwa = $"As {colorSlowny}";
        }
        else
        {
            nazwa = $"{number.ToString()} {colorSlowny}";
        }
    }
}