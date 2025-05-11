using System.Text.RegularExpressions;

namespace Pasjans;

public static class Preferencje
{
    /// <summary>
    /// Uzyskuje od użytkownika index odpowiedzi za pomocą wyświetlenia opcji
    /// </summary>
    /// <param name="options">Lista opcji</param>
    /// <returns></returns>
    public static int ZapytajLista(params List<string> options) //ta metoda zadaje pytanie
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

        return ZapytajLista(options); //zwracamy numer odpowiedzi
    }
    public static bool ZapytajORuch(out string source, out string destination) // zadaniem tej metody jest uzyskanie od użytkownika nazwy karty i numeru stosu
    {

        source = "";
        destination = "";
        try
        {
            string ruch = Console.ReadLine()!;

            if (ruch.ToLower() == "x")
            {
                MainMenu.Otworz();
                return false;
            }
            else if (ruch == "+")
            {
                source = "+";
                destination = "+";
                return false;
            }

            bool czyRozdziel = Rozdziel(ruch, out source, out destination);
            return czyRozdziel;//s
        }
        catch (Exception ex)
        {
            Utilities.Blad("Wystąpił błąd w metodzie AskMove!", "Przeczytaj instrukcję obsługi w menu Jak Grać!", ex);
        }

        return true;
    }

    public static bool Rozdziel(string ruch, out string source, out string destination)
    {
        ruch = ruch.ToLower();

        source = "";
        destination = "";

        string wzorKarty = @"^(10|[2-9]|as|[kqj]) (pik|karo|trefl|kier)$";

        Match match = Regex.Match(ruch, wzorKarty);
        if (ruch == "+")
        {
            source = "+";
            destination = "+";
        }

        if (ruch.Contains("-"))
        {


            string[] splitted = ruch.Split('-', 2);

            if (splitted.Length != 2)
                return false;


            source = splitted[0];
            destination = splitted[1];
        }
        else if (match.Success)
        {
            destination = match.Groups[2].Value;
            source = match.Groups[0].Value;
        }
        return true;
    }
}
