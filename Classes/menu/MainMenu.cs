using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pasjans;

/// <summary>
/// ta klasa obsługuje menu główne
/// </summary>
public static class MainMenu
{
    /// <summary>
    /// Otwiera interfejs menu głównego w terminalu
    /// </summary>
    public static void Otworz()
    {
        switch (Preferencje.ZapytajLista(["Zagraj", "Ustawienia", "Jak Grać", "Wyjdź"]))  // zapytanie użytkownika
        //switch (1)  //W przypadku debugowania visual studio code nie obsługuje Console.ReadKey();
        {
            case 1:
                Program.Start();   //Otwarcie głównego programu
                break;
            case 2:
                Ustawienia.Otworz();    //Otwarcie ustawień
                break;
            case 3:
                Tutorial.Otworz();  //Otwarcie menu jak grać
                break;
            case 4:
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }
}