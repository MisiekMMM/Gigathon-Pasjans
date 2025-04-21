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
    private static int seed;
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały
        Console.ForegroundColor = ConsoleColor.Black;

        Ustawienia.wartosci = Ustawienia.Wczytaj(Ustawienia.wartosci!);

        Music.StartMusic();

        MainMenu.Otworz(); //Otwiera menu główne
    }
    public static void Start()//Metoda wywoływana na początku. 
    {


        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały



        Utilities.Clear(); //odświeżenie konsoli



        List<Karta> talia = GenerujTalie();

        talia = Tasuj(talia, false); //Tasowanie talii

        Karta[,] siatka = ZrobSiatke(talia, out List<Karta> rezerwa); //Tworzy siatkę

        List<Karta> rezerwaOdkryta = new(); //Tworzenie listy z odkrytą rezerwą

        Karta[,] kartyGora = new Karta[13, 4];  //Tworzenie kart u góry

        UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);   //Renderuje siatkę


        while (true)
        {
            Move(ref siatka, ref rezerwaOdkryta, ref rezerwa, ref kartyGora);
        }
    }
    private static List<Karta> GenerujTalie()
    {
        List<Karta> talia = new();
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
        return talia;
    }
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

                        if (canIt && AmILast(siatka, wiersz, kolumna))
                        {
                            if (siatka[wiersz, kolumna].numer != 1)
                                kartyGora[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
                            else
                                kartyGora[docelowyWiersz, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            Debug.Add($"{source}-{destination}");
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
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
                            if (AmILast(siatka, wiersz, kolumna))
                            {
                                if (siatka[wiersz, kolumna].numer == 13)
                                    siatka[docelowyWiersz, docelowaKolumna] = siatka[wiersz, kolumna];
                                else
                                    siatka[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                                Debug.Add($"{source}-{destination}");

                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                            }
                            else
                            {
                                try
                                {
                                    int indexKarty = wiersz;
                                    while (siatka[indexKarty, kolumna] != null)
                                    {
                                        if (siatka[indexKarty, kolumna].numer == 13)
                                            siatka[docelowyWiersz, docelowaKolumna] = siatka[indexKarty, kolumna];
                                        else
                                            siatka[docelowyWiersz + 1, docelowaKolumna] = siatka[indexKarty, kolumna];

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                        siatka[indexKarty, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                                        Debug.Add($"{source}-{destination}");

                                        indexKarty++;
                                        docelowyWiersz++;
                                    }
                                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.Blad("Pojawił się błąd podczas przesuwania stosu!", "Niestety muszę to naprawić (linijka 183)", ex);
                                }
                            }
                        }
                        else
                        {
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
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

                            Debug.Add($"{source}-{destination}");

                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
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
                            if (rezerwaOdkryta[0].numer != 1)
                            {
                                kartyGora[docelowyWiersz + 1, docelowaKolumna] = rezerwaOdkryta[0];
                                rezerwaOdkryta.RemoveAt(0);
                            }
                            else
                            {
                                kartyGora[docelowyWiersz, docelowaKolumna] = rezerwaOdkryta[0];
                                rezerwaOdkryta.RemoveAt(0);
                            }


                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else
                    {
                        int docelowaKolumna = int.Parse(destination);
                        int docelowyWiersz = znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = CanBeCardPlacedOnMe(rezerwaOdkryta[0], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (rezerwaOdkryta[0].numer == 13)
                                siatka[docelowyWiersz, docelowaKolumna] = rezerwaOdkryta[0];
                            else
                                siatka[docelowyWiersz + 1, docelowaKolumna] = rezerwaOdkryta[0];
                            rezerwaOdkryta.RemoveAt(0);

                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Nie możesz tego zrobić!");
                        }

                    }
                }
                else if (miejsce == 0)
                {
                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Nie możesz tego zrobić!");
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
                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
                else
                {
                    for (int i = rezerwaOdkryta.Count - 1; i > -1; i--)
                    {
                        rezerwa.Add(rezerwaOdkryta[i]);
                        rezerwaOdkryta.RemoveAt(i);
                    }
                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
            }
            else if (source == "bug")
            {
                Debug.Save();
                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, seed.ToString() + " error.txt zapisany");
            }
            else
            {
                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Podano niepoprawny ruch!\nNapisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");

            }
        }
        catch (Exception ex)
        {
            Utilities.Blad("Coś się stało, ale nie wiem co!", "Przeczytaj instrukcję w menu Jak Grać", ex);
        }

        return;
    }
    public static bool AmILast(Karta[,] siatka, int wiersz, int kolumna)
    {
        for (int i = wiersz + 1; i < 19; i++)
        {
            if (siatka[i, kolumna] != null)
            {
                return false;
            }
        }
        return true;
    }
    public static bool CanBeCardPlacedOnMe(Karta karta, Karta naMnie, bool czySiatka) //false - na stos końcowy, true - w obrębie siatki
    {
        if (czySiatka)
        {
            if (karta.numer == 13 && naMnie == null)
            {
                return true;
            }
            if (karta.numer + 1 == naMnie.numer && karta.kolor != naMnie.kolor)
                return true;
        }
        else
        {
            if (naMnie != null)
            {
                string kolor1 = karta.nazwa.Split(" ")[1];
                string kolor2 = naMnie.nazwa.Split(" ")[1];

                if (kolor1 == kolor2 && naMnie.numer + 1 == karta.numer)
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
            else if (ruch.ToLower() == "bug")
            {
                source = "bug";
                destination = "bug";
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
            Utilities.Blad("Wystąpił błąd w metodzie AskMove!", "Przeczytaj instrukcję obsługi w menu Jak Grać!", ex);
        }




        return true;
    }


    public static int ZnajdzKarte(string nazwa, Karta[,] siatka, List<Karta> rezerwaOdkryta, Karta[,] kartyGora, out int wiersz, out int kolumna)//ta metoda przeszukuje siatkę, aby znaleźć specyficzną kartę i zwrócić jej pozycję
    {//0 - karta jest zakryta, 1 - karta znajduje sie w siatce, 2 - karta znajduje się na stosie końcowym, 3 - karta znajduje się w rezerwie
        wiersz = 0;
        kolumna = 0;
        for (wiersz = 0; wiersz < 19; wiersz++) //sprawdzenie siatki
        {
            for (kolumna = 0; kolumna < 7; kolumna++)
            {
                if (siatka[wiersz, kolumna] != null && siatka[wiersz, kolumna].nazwa.ToLower() == nazwa.ToLower() && siatka[wiersz, kolumna].odkryta)
                {
                    return 1;
                }
            }
        }
        for (kolumna = 0; kolumna < 4; kolumna++) //sprawdzenie stosów końcowych
        {
            wiersz = znajdzOstatniaKarte(kartyGora, kolumna);
            if (kartyGora[wiersz, kolumna] != null && kartyGora[wiersz, kolumna].nazwa.ToLower() == nazwa.ToLower())
            {
                return 2;
            }
        }
        if (rezerwaOdkryta.Count > 0 && rezerwaOdkryta[0].nazwa.ToLower() == nazwa.ToLower())//sprawdzenie rezerwy
        {
            return 3;
        }

        return 0;
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

    static public List<Karta> Tasuj(List<Karta> kartas, bool czySeed, int sed = 0)
    {
        Random rnd = new Random();

        if (czySeed)
        {
            seed = sed;
        }
        else
        {
            seed = rnd.Next(int.MinValue, int.MaxValue);
        }

        Debug.seed = seed;
        rnd = new(seed);
        List<Karta> shuffled = kartas;
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