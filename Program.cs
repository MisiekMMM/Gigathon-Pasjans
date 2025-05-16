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
    internal static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;

        Console.BackgroundColor = ConsoleColor.White; //Zmiana koloru tła na biały
        Console.ForegroundColor = ConsoleColor.Black;
        try
        {
            Ustawienia.wartosci = Ustawienia.Wczytaj();

            Music.StartMusic();
        }
        catch
        {
            Ustawienia.wartosci = Ustawienia.Wczytaj();

            Music.StartMusic();
        }


        MainMenu.Otworz(); //Otwiera menu główne
    }
    public static void Start()
    {

        Gra gra = new(false);



        gra.Graj();
    }


}