namespace Pasjans;

public class Gra
{
    private List<Karta>? talia;

    private Karta[,]? siatka;

    private List<Karta>? rezerwaOdkryta;

    private Karta[,]? kartyGora;

    private List<Karta>? rezerwa;

    /// <summary>
    /// Metoda odpowiadająca za przygotowanie siatki, rezerwy i stosów końcowych
    /// </summary>
    public Gra(bool isSeed, int seed = 0)
    {

        Debug.Clear();

        Console.BackgroundColor = ConsoleColor.White; //Zmaina koloru tła na biały

        Utilities.Clear(); //odświeżenie konsoli

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
        UI.UpdateUi(siatka!, rezerwaOdkryta!, kartyGora!, rezerwa!);   //Renderuje siatkę


        //  odpowiada za wczytanie ruchów

        while (!sprawdzWygrana())
        {
            bool isMove = Preferencje.ZapytajORuch(out string source, out string destination);

            Ruch.Rusz(ref siatka!, ref rezerwaOdkryta!, ref rezerwa!, ref kartyGora!, isMove, source, destination);
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
}