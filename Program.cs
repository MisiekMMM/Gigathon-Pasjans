using System;
using System.ComponentModel.DataAnnotations.Schema;
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
        Console.Clear();         //odświeżenie konsoli
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("\n ♥");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("♠");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("♦");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("♣ \n");

        List<Karta> talia = new List<Karta>();
        //Tworzenie talii kart
        for (int i = 1; i < 14; i++)
        {
            talia.Add(new Karta(i, true, "Karo"));

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

        talia = Tasuj(talia);

        Karta[,] siatka = ZrobSiatke(talia); //Tworzy siatkę

        UpdateUi(siatka);   //Renderuje siatkę

    }
    public static void UpdateUi(Karta[,] siatka, string advice = "")  //parametr advice wyświetla się u góry ekranu
    {//Ta metoda renderuje karty w terminalu 
        Console.Clear();  // Wyczyszczenie konsoli
        Console.ForegroundColor = ConsoleColor.Black; //zmiana koloru na czarny

        for (int i = 0; i < siatka.GetLength(0); i++) // Wiersze
        {
            for (int j = 0; j < siatka.GetLength(1); j++) // Kolumny
            {
                if (siatka[i, j] != null)
                {
                    Karta karta = siatka[i, j];
                    if (karta.nazwa.Contains("Karo") || karta.nazwa.Contains("Kier"))//Kier i karo 
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // ustawianie na czerwony kolor
                        Console.Write($"{siatka[i, j].nazwa.Replace("Karo", "♦").Replace("Kier", "♥"),-10}"); //Zamiana słownego koloru na emotkę
                        Console.ForegroundColor = ConsoleColor.Black; //Zmiana koloru ponownie na czarny
                    }
                    else if (karta.nazwa.Contains("Pik") || karta.nazwa.Contains("Trefl"))//Pik i trefl
                    {
                        Console.ForegroundColor = ConsoleColor.Black;//Zmiana koloru na czarny (na wszelki wypadek)
                        Console.Write($"{siatka[i, j].nazwa.Replace("Pik", "♠").Replace("Trefl", "♣"),-10}"); //Zamiana słownego koloru na emotkę

                    }
                } // sprawdzanie czy karta nie jest pusta

                //Wyrównuje w 10 znakowym odstępie

            }
            Console.WriteLine(); // Przejście do nowego wiersza
        }
    }
    public static Karta[,] ZrobSiatke(List<Karta> kartas)
    {
        Karta[,] siatka = new Karta[7, 19]; // 7 kolumn, 19 wierszy (maksymalna wysokość)

        int indexKarty = 0;

        for (int j = 0; j < 7; j++)  //Ta pętla układa karty w kolejności z pasjansa
        {
            for (int i = 0; i < 7 - j; i++)
            {
                siatka[i, j] = kartas[indexKarty];
                indexKarty++;
            }
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