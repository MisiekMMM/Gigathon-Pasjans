using System;
using System.Dynamic;

namespace Pasjans;

/// <summary>
/// Odpowiada karcie w talii
/// </summary>
public class Karta
{
    /// <summary>
    /// nazwa karty
    /// </summary>
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
    /// <param name="color">true - czerwony, false - czarny</param>
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
    /// <summary>
    /// Sprawdza legalność ruchu
    /// </summary>
    /// <param name="karta">Karta sprawdzana</param>
    /// <param name="naMnie">Karta, na której ma być ustawiona druga karta</param>
    /// <param name="czySiatka">true - Karta naMnie znajduje się w siatce, false - karta naMnie znajduje się w siatce</param>
    /// <returns></returns>
    public bool CzyKartaPasuje(Karta naMnie, bool czySiatka) //false - na stos końcowy, true - w obrębie siatki
    {
        if (czySiatka)
        {
            if (this.numer == 13 && naMnie == null)
            {
                return true;
            }
            if (this.numer + 1 == naMnie.numer && this.kolor != naMnie.kolor)
                return true;
        }
        else
        {
            if (naMnie != null)
            {
                string kolor1 = this.nazwa.Split(" ")[1];
                string kolor2 = naMnie.nazwa.Split(" ")[1];

                if (kolor1 == kolor2 && naMnie.numer + 1 == this.numer)
                    return true;
            }
            else
            {
                if (naMnie == null && this.numer == 1)
                {
                    return true;
                }
            }
        }


        return false;
    }
}