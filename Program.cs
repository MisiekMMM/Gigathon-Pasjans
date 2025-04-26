using System.Text;

namespace Pasjans;

/// <summary>
/// Główna klasa
/// </summary>
public static class Program
{
    /// <summary>
    /// Główna metoda
    /// </summary>
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmiana koloru tła na biały
        Console.ForegroundColor = ConsoleColor.Black;

        Music.StartMusic();

        MainMenu.Otworz(); //Otwiera menu główne
    }
    /// <summary>
    /// Metoda odpowiadająca za start gry
    /// </summary>
    public static void Start()
    {

        Debug.Clear();

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały

        Utilities.Clear(); //odświeżenie konsoli

        List<Karta> talia = Talia.GenerujTalie();

        // Great seed: -226472860

        talia = Talia.Tasuj(talia, true, -226472860); //Tasowanie talii

        Karta[,] siatka = Siatka.ZrobSiatke(talia, out List<Karta> rezerwa); //Tworzy siatkę

        List<Karta> rezerwaOdkryta = new(); //Tworzenie listy z odkrytą rezerwą

        Karta[,] kartyGora = new Karta[14, 4];  //Tworzenie kart u góry


        UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);   //Renderuje siatkę


        //  odpowiada za wczytanie ruchów


        while (!sprawdzWygrana(siatka, kartyGora))
        {
            bool isMove = Preferencje.ZapytajORuch(out string source, out string destination);
            Ruch.Rusz(ref siatka, ref rezerwaOdkryta, ref rezerwa, ref kartyGora, isMove, source, destination);
        }
        Utilities.Clear();

        Console.WriteLine(" /$$      /$$                                                           \n| $$  /$ | $$                                                           \n| $$ /$$$| $$ /$$   /$$  /$$$$$$   /$$$$$$  /$$$$$$  /$$$$$$$   /$$$$$$ \n| $$/$$ $$ $$| $$  | $$ /$$__  $$ /$$__  $$|____  $$| $$__  $$ |____  $$\n| $$$$_  $$$$| $$  | $$| $$  \\ $$| $$  \\__/ /$$$$$$$| $$  \\\\ $$  /$$$$$$$\n| $$$/\\  $$$| $$  | $$| $$  | $$| $$      /$$__  $$| $$  | $$ /$$__  $$\n| $$/   \\  $$|  $$$$$$$|  $$$$$$$| $$     |  $$$$$$$| $$  | $$|  $$$$$$$\n|__/     \\_/ \\___  $$ \\____  $$|__/      \\_______/|__/  |__/ \\_______/\n              /$$  | $$ /$$  \\ $$                                       \n             |  $$$$$$/|  $$$$$$/                                       \n              \\______/  \\______/                                        \n\n\n");

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
    private static bool sprawdzWygrana(Karta[,] siatka, Karta[,] kartyGora)
    {
        foreach (Karta karta in siatka)
        {
            if (karta != null)
            {
                return false;
            }
        }
        return true;
    }
}