using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NAudio;
using NAudio.Wave;

namespace Pasjans;

public static class Program
{
    static IWavePlayer? waveOut; //Muzyka
    static AudioFileReader? audioFile;  //plik muzyki
    static Thread? playbackThread;   // osobny wątek
    static bool isPlaying = true;
    public static void Main()// wzór tworzenia karty: Karta(int numer, bool kolor, string kolorSlowny) 
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały
        Console.ForegroundColor = ConsoleColor.Black;


        string filePath = "music.wav";

        playbackThread = new Thread(() => PlayLoop(filePath));
        playbackThread.IsBackground = true;
        playbackThread.Start();

        MainMenu.Otworz(); //Otwiera menu główne

    }
    public static void Start()//Metoda wywoływana na początku. 
    {


        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały



        Console.Clear(); //odświeżenie konsoli



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

        siatka = AskMove(siatka, rezerwaOdkryta, kartyGora, rezerwa);

        UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);

    }
    public static Karta[,] AskMove(Karta[,] siatka, List<Karta> rezerwaOdk, Karta[,] gora, List<Karta> rezerwa)//ta metoda zadaje zapytanie o ruch
    {
        try
        {
            string ruch = Console.ReadLine()!;

            if (String.IsNullOrWhiteSpace(ruch))//Jeżeli użytkownik nie podał ruchu
            {
                UpdateUi(siatka, rezerwaOdk, gora, rezerwa, "Podaj ruch! Jeśli nie wiesz jak to zrobić wyjdź przez napisanie X i przejdź do zakładki Jak Grać!");
                return AskMove(siatka, rezerwaOdk, gora, rezerwa);
            }
            else
            {
                if (ruch.ToLower() == "x")
                {
                    MainMenu.Otworz();
                    return new Karta[1, 1];
                }
                if (ruch.ToLower().Contains("do"))//Jeżeli komenda zawiera słowo kluczowe
                {
                    string[] ruchy = ruch.ToLower().Replace(" ", "").Split("do"); // zamienia komende na małe litery, usuwa spacje i rozdziela na tablicę

                    if (ruchy.Length == 2) //jeżeli użytkownik podał tylko jeden ruch
                    {
                        int[] source = ZnajdzKarte(ruchy[0], siatka);   //znajduje źródło ruchu
                        int[] destination = ZnajdzKarte(ruchy[1], siatka);  //znajduje cel


                    }
                    else//Jeżeli podał więcej niż jeden ruch lub nie podał go wcale
                    {
                        UpdateUi(siatka, rezerwaOdk, gora, rezerwa, "Podawaj tylko jeden ruch!\n Jeśli nie wiesz jak to zrobić wyjdź przez napisanie X i przejdź do zakładki Jak Grać!");
                        return AskMove(siatka, rezerwaOdk, gora, rezerwa);
                    }
                }
                else
                {
                    UpdateUi(siatka, rezerwaOdk, gora, rezerwa, "Podaj ruch!\n Jeśli nie wiesz jak to zrobić wyjdź przez napisanie X i przejdź do zakładki Jak Grać!");
                    return AskMove(siatka, rezerwaOdk, gora, rezerwa);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ____  _           _ \n| __ )| | __ _  __| |\n|  _ \\|/// _` |/ _` |\n| |_) //| (_| | (_| |\n|____/|_|\\__,_|\\__,_|\n            (_(      \n\n\n");
            Console.WriteLine(" |\\/\\/\\/|  \n |      |  \n |      |  \n | (o)(o)  \n C      _) \n  | ,___|  \n  |   /    \n /____\\    \n/      \\n\n\n");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Przez twoją nieuwagę pojawił się błąd!\n");
            Console.WriteLine("Błąd jest na tyle poważny, że musimy zresetować grę. Aby uniknąć błędów w przyszłości zapoznaj się z instrukcją w zakładcę Jak Grać.");
            Console.WriteLine("\nNaciśnij dowolny guzik aby wyjść");
            Console.WriteLine("\n\n" + ex.Message);
            Console.ReadKey();
            Environment.Exit(100);
        }
        UpdateUi(siatka, rezerwaOdk, gora, rezerwa, "Wystąpił błąd! Podaj ruch ponownie!\n Jeśli nie wiesz jak to zrobić wyjdź przez napisanie X i przejdź do zakładki Jak Grać!");
        return AskMove(siatka, rezerwaOdk, gora, rezerwa);
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


        if (rezerwa.Count != 0)//jeżeli pierwsza karta z rezerwy istnieje:
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
            printInColor(gora[0, 1].nazwa);
        }
        else
        {
            printInColor("Kier");
        }
        if (gora[0, 1] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 2].nazwa);
        }
        else
        {
            printInColor("Karo");
        }
        if (gora[0, 2] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 3].nazwa);
        }
        else
        {
            printInColor("Trefl");
        }
        if (gora[0, 0] != null)  //Renderuje fundamenty
        {
            printInColor(gora[0, 4].nazwa);
        }
        else
        {
            printInColor("Pik");
        }


        Console.Write("\n\n");
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
                            printInColor(karta.nazwa); //drukuje nazwę Karty
                        }
                        else
                        {//Jeżeli karta jest zakryta:
                            Console.ForegroundColor = ConsoleColor.Black; // Zmiana koloru na czarny
                            Console.Write($"{symbolZakryty,-10}"); //wyrównuje znak zakrytej karty w 10 znakach

                        }
                    }
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
    public static int[] ZnajdzKarte(string nazwa, Karta[,] siatka)//ta metoda przeszukuje siatkę, aby znaleźć specyficzną kartę i zwrócić jej pozycję
    {
        for (int i = 0; i < siatka.GetLength(0); i++) // Wiersze
        {
            for (int j = 0; j < siatka.GetLength(1); j++) // Kolumny
            {
                if (siatka[i, j].nazwa == nazwa)
                {
                    return [i, j];
                }
            }
        }
        return [0, 0];
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
    static void PlayLoop(string filePath)
    {
        waveOut = new WaveOutEvent();
        audioFile = new AudioFileReader(filePath);
        waveOut.Init(audioFile);
        waveOut.PlaybackStopped += OnPlaybackStopped!;
        waveOut.Play();

        // Keep thread alive while playing
        while (isPlaying)
        {
            Thread.Sleep(100); // Small delay to reduce CPU usage
        }
    }

    static void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        if (isPlaying)
        {
            audioFile!.Position = 0;
            waveOut!.Play();
        }
    }
}