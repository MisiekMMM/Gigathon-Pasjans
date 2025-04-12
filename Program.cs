using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pasjans;

public static class Program
{
    public static void Main()// wzór tworzenia karty: Karta(int numer, bool kolor, string kolorSlowny) 
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały



        MainMenu.Otworz();

    }
    public static void Start()//Metoda wywoływana na początku. 
    {


        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały

        try //Podczas debugowania Console.Clear podaje błąd
        {
            Console.Clear(); //odświeżenie konsoli
        }
        catch { }

        List<Karta> talia = new List<Karta>();
        //Tworzenie talii kart w Liście
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Karo")); //dodawanie kart od 1 do 13, gdzie 11 to J 12 to Q a 13 to K

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, false, "Pik"));

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Kier"));

        }
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, false, "Trefl"));

        }

        talia = Tasuj(talia); //Tasowanie talii

        Karta[,] siatka = ZrobSiatke(talia, out List<Karta> rezerwa); //Tworzy siatkę

        List<Karta> rezerwaOdkryta = new(); //Tworzenie listy z odkrytą rezerwą

        Karta[,] kartyGora = new Karta[13, 4];  //Tworzenie kart u góry

        UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);   //Renderuje siatkę
    }
    public static void UpdateUi(Karta[,] siatka, List<Karta> rezerwaOdk, Karta[,] gora, List<Karta> rezerwa, string advice = "")  //parametr advice wyświetla się u góry ekranu
    {//Ta metoda renderuje karty w terminalu 

        string symbolZakryty = "x"; //ustawienie symbolu zakrytej karty. Do zmiany w ustawieniach

        try
        {
            Console.Clear();
        }
        catch { }  // Wyczyszczenie konsoli 

        Console.ForegroundColor = ConsoleColor.Black; //zmiana koloru na czarny


        OdkryjKarty(ref siatka); //odkrywa karty na spodzie stosu

        if (rezerwa.Count != 0)
            Console.Write("x \t", -10); //Drukuje x który udaje rezerwę
        else
            Console.Write("          ");

        if (rezerwaOdk.Count != 0)  //jeżeli pierwsza karta z odkrytej rezerwy istnieje:
            printInColor(rezerwaOdk[0].nazwa);    //drukuje nazwę
        else
            Console.Write("          ");

        Console.Write("\t\t");
        if (gora[0, 0] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 0].nazwa);
        }
        else
        {
            printInColor("Kier");
        }
        if (gora[0, 1] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 0].nazwa);
        }
        else
        {
            printInColor("Karo");
        }
        if (gora[0, 2] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 0].nazwa);
        }
        else
        {
            printInColor("Trefl");
        }
        if (gora[0, 0] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 0].nazwa);
        }
        else
        {
            printInColor("Pik");
        }
        Console.Write("\n\n"); //Później zastąpić talią rezerwową i tymi kartami u góry
        try
        {
            for (int i = 0; i < siatka.GetLength(0); i++) // Wiersze
            {
                for (int j = 0; j < siatka.GetLength(1); j++) // Kolumny
                {
                    Karta karta = siatka[i, j];
                    if (karta != null)   //Sprawdzanie czy karta nie jest pusta
                    {
                        if (karta.odkryta)//jeżeli karta jest odkryta
                        {
                            printInColor(karta.nazwa);
                        }
                        else
                        {//Jeżeli karta jest zakryta:
                            Console.ForegroundColor = ConsoleColor.Black; // Zmiana koloru na czarny
                            Console.Write($"{symbolZakryty,-10}"); //wyrównuje znak zakrytej karty w 10 znakach

                        }
                    }
                    /*
                    else //Jeżeli karta to null (wykonane aby przetestować odkrywanie kart i render): 
                    {//Wyświetla również puste karty
                        Console.ForegroundColor = ConsoleColor.Gray; // Zmiana koloru na szary
                        Console.Write($"{"null",-10}"); //wyrównuje string null w 10 znakach
                        Console.ForegroundColor = ConsoleColor.Black; // Zmiana koloru na czarny
                    }*/
                }

                if (!areAllEmpty(siatka, i)) //Jeżeli wszystkie karty następnego wiersza to nie null - przechodzimi do nowego wiersza
                {
                    Console.WriteLine(); // Przejście do nowego wiersza
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                Console.Clear();
            }
            catch { }
            Console.WriteLine($"Błąd!   {ex.Message}\nSpróbuj zrestaretować grę.");
            Environment.Exit(100);
        }

        Console.WriteLine("\n\n" + advice); //Napisanie porady pod siatką
    }
    public static void printInColor(string Text) //DRY
    {
        if (Text.Contains("Karo") || Text.Contains("Kier"))//Kier i karo 
        {
            Console.ForegroundColor = ConsoleColor.Red; // ustawianie na czerwony kolor
            Console.Write($"{Text.Replace("Karo", "♦").Replace("Kier", "♥"),-10}"); //Zamiana słownego koloru na emotkę
            Console.ForegroundColor = ConsoleColor.Black; //Zmiana koloru ponownie na czarny
        }
        else if (Text.Contains("Pik") || Text.Contains("Trefl"))//Pik i trefl
        {
            Console.ForegroundColor = ConsoleColor.Black;//Zmiana koloru na czarny (na wszelki wypadek)
            Console.Write($"{Text.Replace("Pik", "♠").Replace("Trefl", "♣"),-10}"); //Zamiana słownego koloru na emotkę

        }
        else
        {//Obsługa błędu
            throw new Exception("Nieznany kolor!");
        }
    }
    public static bool areAllEmpty(Karta[,] siatka, int i)// ta metoda sprawdza, czy wszystkie karty w poniższy
    {
        bool areAllEmpty = true;
        for (int i2 = 0; i2 < 7; i2++)
        {
            if (i + 1 < siatka.GetLength(0) && siatka[i + 1, i2] != null) //Jeżeli index nie wychodzi poza tablicę i karta pod nie jest nullem:
            {
                return false; //zwracamy false
            }
        }
        return areAllEmpty;
    }
    public static void OdkryjKarty(ref Karta[,] siatka)//ta metoda odkrywa karty na końcu każdej kolumny
    {
        for (int i = 0; i < siatka.GetLength(0); i++) // Wiersze
        {
            for (int j = 0; j < siatka.GetLength(1); j++) // Kolumny
            {
                Karta karta = siatka[i, j];
                if (karta != null)
                {
                    if (i + 1 < siatka.GetLength(0) && siatka[i + 1, j] == null) // jeżeli karta poniżej nie wychodzi za index tablicy i jest pusta:
                    {
                        siatka[i, j].odkryta = true;  //Karta zostaje odkrtyta
                    }
                }

            }
        }
    }
    public static Karta[,] ZrobSiatke(List<Karta> kartas, out List<Karta> rezerwa)
    {
        Karta[,] siatka = new Karta[19, 7]; // 7 kolumn, 19 wierszy (maksymalna wysokość)

        int indexKarty = 0;

        for (int j = 0; j < 7; j++)  //Ta pętla układa karty w kolejności z pasjansa
        {
            for (int i = 0; i < 7 - j; i++)
            {
                siatka[i, j] = kartas[indexKarty];
                indexKarty++;
            }
        }
        rezerwa = new List<Karta>();
        for (int i = 28; i <= 51; i++)
        {
            rezerwa.Add(kartas[i]);
        }

        return siatka;
    }

    static public List<Karta> Tasuj(List<Karta> kartas)
    {
        Random rnd = new Random();
        List<Karta> shuffled = new List<Karta>(kartas);
        int n = shuffled.Count;

        // Fisher-Yates
        for (int i = n - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);

            Karta temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        return shuffled;
    }
}