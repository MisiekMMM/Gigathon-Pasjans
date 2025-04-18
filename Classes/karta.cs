using System;

namespace Pasjans;

public class Karta  //Ta klasa odpowiada karcie w talii
{
    public string nazwa;

    public int numer; // walet J 11, królowa Q 12, król K 13
    // true czerwony false czarny  
    public bool kolor;

    public bool odkryta;

    public Karta(int number, bool color, string colorSlowny) //colorSlowny to Pik, Karo, Trefl, Kier
    {
        kolor = color;
        numer = number;
        odkryta = false;

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