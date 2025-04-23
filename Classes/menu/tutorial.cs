using System;
using NAudio.CoreAudioApi;

namespace Pasjans;

/// <summary>
/// Odpowiada za menu jak grać 
/// </summary>
public static class Tutorial
{
    /// <summary>
    /// Otwiera menu jak grać
    /// </summary>
    public static void Otworz()
    {
        List<string> strony = new()
        {
            "Cel gry: \n● Ułożyć wszystkie karty w czterech kolorach (♠ ♥ ♦ ♣) od asa do króla na stosach końcowych",
            "Rozgrywka: \n●Karty są rozłożone w 7 kolumnach: od 1 do 7 kart, tylko ostatnia karta w kolumnie odkryta.\n●Pozostałe karty tworzą rezerwę.\n●Karty można układać malejąco i naprzemiennie kolorami w kolumnach (np. czarna 7 na czerwoną 8).\n●Tylko król może być przeniesiony na pustą kolumnę.\n●Asy przenosi się na stosy końcowe i buduje kolory rosnąco (A, 2, 3, ..., K).\n●Z talii dobierania można przeglądać karty i przenosić je do kolumn lub na stosy końcowe.",
            "● Aby poruszać kartą między stosami należy napisać jej nazwę myślnik (-) i numer kolumny, np. Q Kier-1. Brana pod uwagę jest wielkość liter i odstępy\n● Aby przenieść kartę na stos końcowy wystarczy podać jej nazwę, np. As Pik\n● Aby odkryć nową kartę z rezerwy należy napisać +",
            "Wygrana:\n● Gra kończy się, gdy wszystkie cztery kolory zostaną ułożone na stosach końcowych od asa do króla.",
            " - Jeżeli znajdziesz błąd wpisz \"bug\". To powinno zapisać seed i historię ruchów w pliku error.txt"
        };

        int numerStrony = 0;

        while (true)
        {
            Utilities.Clear();
            Console.WriteLine("     _       _       ____             __ \n    | | __ _| | __  / ___|_ __ __ _  /__/\n _  | |/ _` | |/ / | |  _| '__/ _` |/ __|\n| |_| | (_| |   <  | |_| | | | (_| | (__ \n \\___/ \\__,_|_|\\_\\  \\____|_|  \\__,_|\\___|\n\n\n");
            Console.WriteLine(strony[numerStrony]);
            Console.WriteLine($"Strona {numerStrony + 1} z {strony.Count}");
            Utilities.DrukujLinie();
            if (numerStrony == 0)
            {
                Console.WriteLine("\nWciśnij --> aby przejść do kolejnej strony\nwciśnij X aby wrócić do menu głównego");
            }
            else if (numerStrony == strony.Count - 1)
            {
                Console.WriteLine("\nWciśnij <-- aby przejść do poprzedniej strony\nwciśnij X aby wrócić do menu głównego");
            }
            else
            {
                Console.WriteLine("\nWciśnij --> aby przejść do kolejnej strony i <-- aby przejść do poprzedniej\nwciśnij X aby wrócić do menu głównego");
            }

            ConsoleKeyInfo CKI = Console.ReadKey(true);
            if (CKI.Key == ConsoleKey.RightArrow && numerStrony + 1 < strony.Count)
            {
                numerStrony++;
            }
            else if (CKI.Key == ConsoleKey.LeftArrow && numerStrony - 1 >= 0)
            {
                numerStrony--;
            }
            else if (CKI.Key == ConsoleKey.X)
            {
                break;
            }
        }
        MainMenu.Otworz();
    }
}