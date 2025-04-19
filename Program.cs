using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using NAudio;
using NAudio.Wave;

namespace Pasjans;

public static class Program
{
    static IWavePlayer? waveOut; //Muzyka
    static AudioFileReader? audioFile;  //plik muzyki
    static Thread? playbackThread;   // osobny wątek
    static bool isPlaying = true;
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały
        Console.ForegroundColor = ConsoleColor.Black;

        Ustawienia.wartosci = Ustawienia.Wczytaj(Ustawienia.wartosci!);

        string filePath = "music.wav";

        playbackThread = new Thread(() => PlayLoop(filePath));
        playbackThread.IsBackground = true;
        playbackThread.Start();

        MainMenu.Otworz(); //Otwiera menu główne



    }
    public static void Start()//Metoda wywoływana na początku. 
    {


        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały



        Utilities.Clear(); //odświeżenie konsoli



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


        while (true)
        {
            Move(ref siatka, ref rezerwaOdkryta, ref rezerwa, ref kartyGora);
        }


    }

    //zwraca true jeśli gra trwa
    public static void Move(ref Karta[,] siatka, ref List<Karta> rezerwaOdkryta, ref List<Karta> rezerwa, ref Karta[,] kartyGora)
    {
        try
        {
            bool isMove = AskMove(out string source, out string destination);

            if (isMove)
            {
                int miejsce = ZnajdzKarte(source, siatka, rezerwaOdkryta, kartyGora, out int wiersz, out int kolumna);

                if (miejsce == 1)
                {
                    if (destination == "Pik" || destination == "Karo" || destination == "Kier" || destination == "Trefl")
                    {
                        //int docelowaKolumna = destination == "Kier" ? 0 : destination == "Karo" ? 1 : destination == "Trefl" ? 2 : destination == "Pik" ? 3 : throw new Exception("Nieznany kolor! Błąd w linijce 111");
                        int docelowaKolumna = siatka[wiersz, kolumna].indexKoloru;
                        int docelowyWiersz = znajdzOstatniaKarte(kartyGora, docelowaKolumna);
                        bool canIt = CanBeCardPlacedOnMe(siatka[wiersz, kolumna], kartyGora[docelowyWiersz, docelowaKolumna], false);

                        if (canIt)
                        {
                            kartyGora[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = false;

                        if (siatka[wiersz, kolumna].numer == 13)
                        {
                            if (siatka[0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = CanBeCardPlacedOnMe(siatka[wiersz, kolumna], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (siatka[wiersz, kolumna].numer == 13)
                                siatka[docelowyWiersz, docelowaKolumna] = siatka[wiersz, kolumna];
                            else
                                siatka[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 2)// obsługa kart ze stosów końcowych
                {
                    if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = false;

                        if (kartyGora[wiersz, kolumna].numer == 13)
                        {
                            if (siatka[0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = CanBeCardPlacedOnMe(kartyGora[wiersz, kolumna], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (kartyGora[wiersz, kolumna].numer == 13)
                                siatka[docelowyWiersz, docelowaKolumna] = kartyGora[wiersz, kolumna];
                            else
                                siatka[docelowyWiersz + 1, docelowaKolumna] = kartyGora[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            kartyGora[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 3)
                {
                    if (destination == "Pik" || destination == "Karo" || destination == "Kier" || destination == "Trefl")
                    {
                        int docelowaKolumna = rezerwaOdkryta[0].indexKoloru;
                        int docelowyWiersz = znajdzOstatniaKarte(kartyGora, docelowaKolumna);
                        bool canIt = CanBeCardPlacedOnMe(rezerwaOdkryta[0], kartyGora[docelowyWiersz, docelowaKolumna], false);

                        if (canIt)
                        {
                            kartyGora[docelowyWiersz + 1, docelowaKolumna] = rezerwaOdkryta[0];
                            rezerwaOdkryta.RemoveAt(0);

                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else
                    {
                        int docelowaKolumna = int.Parse(destination);
                        int docelowyWiersz = znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = CanBeCardPlacedOnMe(rezerwaOdkryta[0], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            siatka[docelowyWiersz + 1, docelowaKolumna] = rezerwaOdkryta[0];
                            rezerwa.RemoveAt(0);

                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta nie jest jeszcze odkryta");
                        }

                    }
                }
                else if (miejsce == 0)
                {
                    UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta nie jest jeszcze odkryta!");
                }
            }
            else if (source == "+")
            {
                if (rezerwa.Count > 0)
                {
                    if (rezerwaOdkryta.Count > 0)
                        rezerwaOdkryta[0].odkryta = false;
                    rezerwaOdkryta.Insert(0, rezerwa[0]);
                    rezerwa.RemoveAt(0);
                    UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
                else
                {
                    for (int i = rezerwaOdkryta.Count - 1; i > -1; i--)
                    {
                        rezerwa.Add(rezerwaOdkryta[i]);
                        rezerwaOdkryta.RemoveAt(i);
                    }
                    UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
            }
            else
            {
                UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Podano niepoprawny ruch!\nNapisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");

            }
        }
        catch (Exception ex)
        {
            Utilities.Error("Coś się stało, ale nie wiem co!", "Przeczytaj instrukcję w menu Jak Grać", ex);
        }

        return;
    }
    public static bool CanBeCardPlacedOnMe(Karta karta, Karta naMnie, bool czySiatka) //false - na stos końcowy, true - w obrębie siatki
    {
        if (czySiatka)
        {
            if (karta.numer + 1 == naMnie.numer && karta.kolor != naMnie.kolor)
                return true;
        }
        else
        {
            if (naMnie != null)
            {
                string kolor1 = karta.nazwa.Split(" ")[1];
                string kolor2 = naMnie.nazwa.Split(" ")[1];

                if (kolor1 == kolor2 && naMnie.numer - 1 == karta.numer)
                    return true;
            }
            else
            {
                if (naMnie == null && karta.numer == 1)
                {
                    return true;
                }
            }
        }


        return false;
    }
    public static int znajdzOstatniaKarte(Karta[,] siatka, int kolumna)
    {
        for (int i = 0; i < siatka.GetLength(0); i++)
        {
            if (siatka[i + 1, kolumna] == null && i + 1 < siatka.GetLength(0))
            {
                return i;
            }
        }
        return -1;
    }
    public static bool AskMove(out string source, out string destination) // zadaniem tej metody jest uzyskanie od użytkownika nazwy karty i numeru stosu
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

            string wzorKarty = @"^(10|[2-9]|As|[KQJ]) (Pik|Karo|Trefl|Kier)$";

            Match match = Regex.Match(ruch, wzorKarty);


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
        }
        catch (Exception ex)
        {
            Utilities.Error("Wystąpił błąd w metodzie AskMove!", "Przeczytaj instrukcję obsługi w menu Jak Grać!", ex);
        }




        return true;
    }

    public static void UpdateUi(Karta[,] siatka, List<Karta> rezerwaOdk, Karta[,] gora, List<Karta> rezerwa, string advice = "")  //parametr advice wyświetla się u góry ekranu
    {//Ta metoda renderuje karty w terminalu 

        string symbolZakryty = "x"; //ustawienie symbolu zakrytej karty. Do zmiany w ustawieniach

        Utilities.Clear();

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
            printInColor(gora[znajdzOstatniaKarte(gora, 0), 0].nazwa);
        }
        else
        {
            printInColor("Kier");
        }
        if (gora[0, 1] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 1), 1].nazwa);
        }
        else
        {
            printInColor("Karo");
        }
        if (gora[0, 2] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 2), 2].nazwa);
        }
        else
        {
            printInColor("Trefl");
        }
        if (gora[0, 3] != null)  //Renderuje fundamenty
        {
            printInColor(gora[znajdzOstatniaKarte(gora, 3), 3].nazwa);
        }
        else
        {
            printInColor("Pik");
        }


        Console.Write("\n\n");

        for (int i = 0; i < 7; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{i,-10}");
        }
        Console.Write("\n");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write("-");
            }
        }
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("\n");
        try
        {
            for (int wiersz = 0; wiersz < 19; wiersz++)
            {
                for (int kolumna = 0; kolumna < 7; kolumna++)
                {
                    Karta karta = siatka[wiersz, kolumna];

                    if (karta != null)
                    {
                        if (karta.odkryta)
                        {
                            printInColor(karta.nazwa);
                        }
                        else
                        {
                            Console.Write($"{symbolZakryty,-10}");
                        }

                    }
                    else
                    {
                        Console.Write("          ");
                    }
                }
                if (!areAllEmpty(siatka, wiersz))
                {
                    Console.WriteLine();
                }
            }


        }
        catch (Exception ex)
        {
            Utilities.Error("Błąd podczas renderowania siatki!", "Spróbuj zrestartować grę!", ex);
        }

        Console.WriteLine("\n\n" + advice); //Napisanie porady pod siatką
    }
    public static int ZnajdzKarte(string nazwa, Karta[,] siatka, List<Karta> rezerwaOdkryta, Karta[,] kartyGora, out int wiersz, out int kolumna)//ta metoda przeszukuje siatkę, aby znaleźć specyficzną kartę i zwrócić jej pozycję
    {//0 - karta jest zakryta, 1 - karta znajduje sie w siatce, 2 - karta znajduje się na stosie końcowym, 3 - karta znajduje się w rezerwie
        wiersz = 0;
        kolumna = 0;
        for (wiersz = 0; wiersz < 19; wiersz++) //sprawdzenie siatki
        {
            for (kolumna = 0; kolumna < 7; kolumna++)
            {
                if (siatka[wiersz, kolumna] != null && siatka[wiersz, kolumna].nazwa == nazwa && siatka[wiersz, kolumna].odkryta)
                {
                    return 1;
                }
            }
        }
        for (kolumna = 0; kolumna < 4; kolumna++) //sprawdzenie stosów końcowych
        {
            wiersz = znajdzOstatniaKarte(kartyGora, kolumna);
            if (kartyGora[wiersz, kolumna] != null && kartyGora[wiersz, kolumna].nazwa == nazwa)
            {
                return 2;
            }
        }
        if (rezerwaOdkryta.Count > 0 && rezerwaOdkryta[0].nazwa == nazwa)//sprawdzenie rezerwy
        {
            return 3;
        }

        return 0;
    }
    public static void printInColor(string Text)
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
        for (int kolumna = 0; kolumna < 7; kolumna++) //kolumny
        {
            for (int wiersz = 0; wiersz <= kolumna; wiersz++) //wiersze
            {
                Karta karta = siatka[wiersz, kolumna];
                if (karta != null)
                {
                    if (wiersz + 1 < siatka.GetLength(0) && siatka[wiersz + 1, kolumna] == null) // jeżeli karta poniżej nie wychodzi za index tablicy i jest pusta:
                    {
                        siatka[wiersz, kolumna].odkryta = true;  //Karta zostaje odkrtyta
                    }
                }

            }
        }

    }
    public static Karta[,] ZrobSiatke(List<Karta> kartas, out List<Karta> rezerwa)
    {
        Karta[,] siatka = new Karta[19, 7]; // 7 kolumn, 19 wierszy (maksymalna wysokość)

        int indexKarty = 0;

        for (int kolumna = 0; kolumna < 7; kolumna++)
        {
            for (int wiersz = 0; wiersz <= kolumna; wiersz++) // W każdej kolumnie o 1 więcej kart
            {
                siatka[wiersz, kolumna] = kartas[indexKarty];
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
        waveOut.Volume = (float)(int)Ustawienia.wartosci!["Głośność"] / 100f;
        waveOut.Play();



        // Keep thread alive while playing
        while (isPlaying)
        {
            waveOut.Volume = (float)(int)Ustawienia.wartosci!["Głośność"] / 100f;

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