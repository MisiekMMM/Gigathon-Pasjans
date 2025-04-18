using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pasjans;

public static class MainMenu  //ta klasa obsługuje menu główne
{
    public static void Otworz()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;


        //switch (AskPreference(["Zagraj", "Ustawienia", "Jak Grać"]))  // zapytanie użytkownika
        switch (1)  //W przypadku debugowania visual studio code nie obsługuje Console.ReadKey();
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
            default:
                break;
        }
    }
    public static int AskPreference(params List<string> options) //ta metoda zadaje pytanie
    {
        Utilities.Clear(); //Czyszczenie konsoli

        //Wyświetlenie napisu w ASCII art
        Console.WriteLine(" ____            _                 \n" + "|  _ \\ __ _ ___ (_) __ _ _ __  ___ \n" + "| |_) / _` / __|| |/ _` | '_ \\/ __|\n" + "|  __/ (_| \\__ \\| | (_| | | | \\__ \\\n" + "|_|   \\__,_|___// |\\__,_|_| |_|___/\n" + "              |__/                 \n");
        Console.WriteLine("          _____\n         |A .  | _____\n         | /.\\ ||A ^  | _____\n         |(_._)|| / \\ ||A _  | _____\n         |  |  || \\ / || ( ) ||A_ _ |\n         |____V||  .  ||(_'_)||( v )|\n                |____V||  |  || \\ / |\n                       |____V||  .  |\n                              |____V|\n\n\n");
        foreach (string option in options) //Wypisanie opcji
        {
            Console.WriteLine($"[{options.IndexOf(option) + 1}] {option}");
        }

        Console.WriteLine();//Nowa linia
        ConsoleKeyInfo response = Console.ReadKey();  //Pobranie odpowiedzi od użytkownika

        if (int.TryParse(response.KeyChar.ToString(), out int result) && result > 0 && result <= options.Count)//sprawdzenie czy odpowiedź mieści się w liśice odpowiedzi
        {
            return result;
        }

        return AskPreference(options); //zwracamy numer odpowiedzi
    }
}