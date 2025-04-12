using System;

namespace Pasjans;

public class Karta
{
    public string nazwa;

    public int numer; // walet J 11, królowa Q 12, król K 13
    // true czerwony false czarny  
    public bool kolor;

    public bool odkryta;

    public Karta(int numer, bool kolor, string kolorSlowny) //kolorSlowny to Pik, Karo, Trefl, Kier
    {
        odkryta = false;

        if (numer == 11)
        {
            nazwa = $"J {kolorSlowny}";
        }
        else if (numer == 12)
        {
            nazwa = $"Q {kolorSlowny}";
        }
        else if (numer == 13)
        {
            nazwa = $"K {kolorSlowny}";
        }
        else if (numer == 1)
        {
            nazwa = $"As {kolorSlowny}";
        }
        else
        {
            nazwa = $"{numer.ToString()} {kolorSlowny}";
        }
    }
}