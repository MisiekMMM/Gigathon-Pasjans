using System.Security.Cryptography.X509Certificates;

namespace Pasjans;

/// <summary>
/// odpowiada za grę. Zawiera pola takie jak talia siatka rezerwa rezerwa odkryta i stosy końcowe
/// </summary>
public class Gra
{
    public List<Karta>? talia;

    public Karta[,]? siatka;

    public List<Karta>? rezerwaOdkryta;

    public Karta[,]? kartyGora;

    public List<Karta>? rezerwa;

    /// <summary>
    /// Metoda odpowiadająca za przygotowanie siatki, rezerwy i stosów końcowych
    /// </summary>
    public Gra(bool isSeed, int seed = 0)
    {

        talia = Talia.GenerujTalie();

        // Great seed: -226472860

        talia = Talia.Tasuj(talia, isSeed, seed); //Tasowanie talii

        siatka = Siatka.ZrobSiatke(talia, out rezerwa); //Tworzy siatkę

        rezerwaOdkryta = new(); //Tworzenie listy z odkrytą rezerwą

        kartyGora = new Karta[14, 4];  //Tworzenie kart u góry

    }

    /// <summary>
    /// Rozpoczyna grę
    /// </summary>
    public void Graj()
    {

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały

        Utilities.Clear(); //odświeżenie konsoli

        UI.UpdateUi(this);   //Renderuje siatkę



        //  odpowiada za wczytanie ruchów

        while (!sprawdzWygrana())
        {
            bool isMove = Preferencje.ZapytajORuch(out string source, out string destination);

            var modified = this;

            bool czyKoniec = czyWszystkieOdkryte();

            Ruch.Rusz(ref modified, isMove, source, destination, czyKoniec);

            this.Wczytaj(modified);
        }
        Utilities.Clear();

        Console.WriteLine("__        __                                       \n\\ \\      / /_   _   __ _  _ __  __ _  _ __    __ _ \n\\ \\ \\ /\\ / /| | | | / _` || '__|/ _` || '_ \\  / _` |\n\\  \\ V  V / | |_| || (_| || |  | (_| || | | || (_| |\n\\   \\_/\\_/   \\__, | \\__, ||_|   \\__,_||_| |_| \\__,_|\n\\            |___/  |___/                           \n\n\n");

        Console.WriteLine("Naciśnij dowolny guzik aby wrócić do menu głównego");

        Console.ReadKey();

        MainMenu.Otworz();
    }
    /// <summary>
    /// Metoda sprawdzająca wygraną
    /// </summary>
    /// <param name="siatka"></param>
    /// <param name="kartyGora"></param>
    /// <returns>true - wygrana, false - gra wciąż trwa</returns>
    private bool sprawdzWygrana()
    {
        foreach (Karta karta in siatka!)
        {
            if (karta != null)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// sprawdza, czy wszystkie karty zostały odkryte
    /// </summary>
    private bool czyWszystkieOdkryte()
    {

        foreach (Karta karta in siatka!)
        {
            if (karta != null && !karta.odkryta)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// wczytuje grę z innego obiektu gra
    /// </summary>
    /// <param name="gra">gra do wczytania</param>
    public void Wczytaj(Gra gra)
    {
        siatka = gra.siatka;
        rezerwa = gra.rezerwa;
        kartyGora = gra.kartyGora;
        rezerwaOdkryta = gra.rezerwaOdkryta;
    }
}