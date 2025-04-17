using System;

namespace Pasjans;

public static class Tutorial
{
    public static void Otworz()
    {
        List<string> strony = new()
        {
            "Przenoszenie kart w kolumnach: \n● Można przesuwać karty, które są ułożone w kolejności malejącej (K → Q → J \\... 2 → A). \n● Karty muszą być układane naprzemiennie kolorami \n(czarna-czerwona-czarna-czerwona). \n● Można przenosić pojedynczą kartę lub całą sekwencję poprawnie ułożonych kart.",
            "Poprawny sposób zapisu ruchu: \n● Należy napisać kartę w formacie numer-kolor dla karty źródła i karty celu bądź Pik/Karo/Kier/Trefl dla układania na stosie końcowym\n● Między kartami źródła i celu należy umieścic słowo \"do\" \n●np. Q-trefl do trefl",
            "Przenoszenie kart na stosy końcowe:\n● Karty można przenieść na stosy końcowe tylko w kolejności od asa do króla,np.:\n\t○ A♠ → 2♠ → 3♠ → ... → K♠\n\t○ A♥ → 2♥ → 3♥ → ... → K♥\n● Każdy stos końcowy może zawierać tylko jeden kolor.",
            "Odkrywanie zakrytych kart:\n● Gdy przeniesiesz ostatnią odkrytą kartę z kolumny, zakryta karta pod nią\nzostaje odkryta.\n● Jeśli kolumna stanie się pusta, można w jej miejsce przenieść tylko króla (K)\nlub całą sekwencję kart zaczynającą się od króla.",
            "Dobieranie kart ze stosu rezerwowego:● Można dobierać karty ze stosu:\n\t○ Na raz dobiera się jedną kartę.\n\t○ Jeśli stos dobierania się wyczerpie, należy go przetasować i ponownie\nużyć.",
            "Zakończenie gry\n● Po ułożeniu wszystkich kart na stosach końcowych."
        };

        int numerStrony = 0;

        while (true)
        {
            Utilities.Clear();
            Console.WriteLine("     _       _       ____             __ \n    | | __ _| | __  / ___|_ __ __ _  /__/\n _  | |/ _` | |/ / | |  _| '__/ _` |/ __|\n| |_| | (_| |   <  | |_| | | | (_| | (__ \n \\___/ \\__,_|_|\\_\\  \\____|_|  \\__,_|\\___|\n\n\n");
            Console.WriteLine(strony[numerStrony]);
            Console.WriteLine("\nNawiguj strzałkami, wciśnij X aby wyjść");
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