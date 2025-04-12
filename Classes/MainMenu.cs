using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pasjans;

public static class MainMenu
{
    public static void Otworz()
    {
        Console.OutputEncoding = Encoding.UTF8; //Obsługa emotek ♦♣♥♠
        Console.InputEncoding = Encoding.UTF8;


        switch (AskPreference(["Zagraj", "Ustawienia", "Jak Grać"]))
        {
            case 1:
                Program.Start();
                break;
            case 2:
                Ustawienia.Otworz();
                break;
            case 3:
                Tutorial.Otworz();
                break;
            default:
                break;
        }
    }
    public static int AskPreference(params List<string> options)
    {
        Console.Clear();
        Console.WriteLine(" ____            _                 \n" + "|  _ \\ __ _ ___ (_) __ _ _ __  ___ \n" + "| |_) / _` / __|| |/ _` | '_ \\/ __|\n" + "|  __/ (_| \\__ \\| | (_| | | | \\__ \\\n" + "|_|   \\__,_|___// |\\__,_|_| |_|___/\n" + "              |__/                 \n");

        foreach (string option in options)
        {
            Console.WriteLine($"[{options.IndexOf(option) + 1}] {option}");
        }

        Console.Write("\n");
        string response = Console.ReadLine()!;

        if (int.TryParse(response, out int result) && result > 0 && result <= options.Count)
        {
            return result;
        }

        return AskPreference(options);
    }
}