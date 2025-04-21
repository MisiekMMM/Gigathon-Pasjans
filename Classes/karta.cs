using System;
using System.Dynamic;

namespace Pasjans;

/// <summary>
/// Odpowiada karcie w talii
/// </summary>
public class Karta
{
    public string nazwa { get; }

    /// <summary>
    /// walet J 11, królowa Q 12, król K 13
    /// </summary>
    public int numer { get; }

    /// <summary>
    ///    true czerwony false czarny 
    /// </summary>

    public bool kolor { get; }

    public bool odkryta { get; set; }

    /// <summary>
    /// 0 - kier, 1 - karo, 2 - trefl, 3 - pik
    /// </summary>
    public int indexKoloru { get; }

    /// <summary>
    /// Konstruktor klasy karta
    /// </summary>
    /// <param name="number">Numer karty (od 1 do 13)</param>
    /// <param name="color">Kolor</param>
    /// <param name="colorSlowny">pik, karo, kier, trefl</param>
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