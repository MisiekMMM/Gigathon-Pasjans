using System.Runtime.CompilerServices;

namespace Pasjans;

/// <summary>
/// Odpowiada za wykonywanie ruchów
/// </summary>
public static class Ruch
{
    /// <summary>
    /// Odpowiada za zapytanie o i wykonanie ruchu
    /// </summary>
    /// <param name="gra.siatka!">Siatka kart</param>
    /// <param name="gra.rezerwaOdkryta!">Rezerwa odkryta</param>
    /// <param name="gra.rezerwa">Rezerwa</param>
    /// <param name="gra.kartyGora!">Stosy końcowe</param>
    public static void Rusz(ref Gra gra, bool isMove, string source, string destination)
    {
        try
        {
            if (isMove)
            {
                int miejsce = Siatka.ZnajdzKarte(source, gra.siatka!, gra.rezerwaOdkryta!, gra.kartyGora!, out int wiersz, out int kolumna);

                if (miejsce == 1)
                {
                    if (destination.ToLower() == "pik" || destination.ToLower() == "karo" || destination.ToLower() == "kier" || destination.ToLower() == "trefl")
                    {
                        //int docelowaKolumna = destination == "Kier" ? 0 : destination == "Karo" ? 1 : destination == "Trefl" ? 2 : destination == "Pik" ? 3 : throw new Exception("Nieznany kolor! Błąd w linijce 111");
                        int docelowaKolumna = gra.siatka![wiersz, kolumna].indexKoloru;
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(gra.kartyGora!, docelowaKolumna);
                        bool canIt = gra.siatka![wiersz, kolumna].CzyKartaPasuje(gra.kartyGora![docelowyWiersz, docelowaKolumna], false);

                        if (canIt && Siatka.CzyOstatni(gra.siatka!, wiersz, kolumna))
                        {

                            if (gra.siatka![wiersz, kolumna].numer != 1)
                                gra.kartyGora![docelowyWiersz + 1, docelowaKolumna] = gra.siatka![wiersz, kolumna];
                            else
                                gra.kartyGora![docelowyWiersz, docelowaKolumna] = gra.siatka![wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            gra.siatka![wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.



                            UI.UpdateUi(gra);
                        }
                        else
                        {

                            UI.UpdateUi(gra, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(gra.siatka!, docelowaKolumna);

                        bool canIt = false;

                        if (gra.siatka![wiersz, kolumna].numer == 13)
                        {
                            if (gra.siatka![0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = gra.siatka![wiersz, kolumna].CzyKartaPasuje(gra.siatka![docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {

                            if (Siatka.CzyOstatni(gra.siatka!, wiersz, kolumna))
                            {
                                if (gra.siatka![wiersz, kolumna].numer == 13)
                                    gra.siatka![docelowyWiersz, docelowaKolumna] = gra.siatka![wiersz, kolumna];
                                else
                                    gra.siatka![docelowyWiersz + 1, docelowaKolumna] = gra.siatka![wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                gra.siatka![wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.




                                UI.UpdateUi(gra);
                            }
                            else
                            {
                                try
                                {
                                    int indexKarty = wiersz;
                                    while (gra.siatka![indexKarty, kolumna] != null)
                                    {
                                        if (gra.siatka![indexKarty, kolumna].numer == 13)
                                        {
                                            gra.siatka![docelowyWiersz, docelowaKolumna] = gra.siatka![indexKarty, kolumna];
                                            docelowyWiersz--;
                                        }
                                        else
                                            gra.siatka![docelowyWiersz + 1, docelowaKolumna] = gra.siatka![indexKarty, kolumna];

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                        gra.siatka![indexKarty, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                                        indexKarty++;
                                        docelowyWiersz++;
                                    }




                                    UI.UpdateUi(gra);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.Blad("Pojawił się błąd podczas przesuwania stosu!", "Niestety muszę to naprawić (linijka 183)", ex);
                                }
                            }
                        }
                        else
                        {

                            UI.UpdateUi(gra, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 2)// obsługa kart ze stosów końcowych
                {
                    if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(gra.siatka!, docelowaKolumna);

                        bool canIt = false;

                        if (gra.kartyGora![wiersz, kolumna].numer == 13)
                        {
                            if (gra.siatka![0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = gra.kartyGora![wiersz, kolumna].CzyKartaPasuje(gra.siatka![docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {

                            if (gra.kartyGora![wiersz, kolumna].numer == 13)
                                gra.siatka![docelowyWiersz, docelowaKolumna] = gra.kartyGora![wiersz, kolumna];
                            else
                                gra.siatka![docelowyWiersz + 1, docelowaKolumna] = gra.kartyGora![wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            gra.kartyGora![wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.




                            UI.UpdateUi(gra);
                        }
                        else
                        {

                            UI.UpdateUi(gra, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 3)
                {
                    if (destination.ToLower() == "pik" || destination.ToLower() == "karo" || destination.ToLower() == "kier" || destination.ToLower() == "trefl")
                    {
                        int docelowaKolumna = gra.rezerwaOdkryta![0].indexKoloru;
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(gra.kartyGora!, docelowaKolumna);
                        bool canIt = gra.rezerwaOdkryta![0].CzyKartaPasuje(gra.kartyGora![docelowyWiersz, docelowaKolumna], false);

                        if (canIt)
                        {

                            if (gra.rezerwaOdkryta![0].numer != 1)
                            {
                                gra.kartyGora![docelowyWiersz + 1, docelowaKolumna] = gra.rezerwaOdkryta![0];
                                gra.rezerwaOdkryta!.RemoveAt(0);
                            }
                            else
                            {
                                gra.kartyGora![docelowyWiersz, docelowaKolumna] = gra.rezerwaOdkryta![0];
                                gra.rezerwaOdkryta!.RemoveAt(0);
                            }


                            UI.UpdateUi(gra);
                        }
                        else
                        {

                            UI.UpdateUi(gra, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else
                    {
                        int docelowaKolumna = int.Parse(destination);
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(gra.siatka!, docelowaKolumna);

                        bool canIt = gra.rezerwaOdkryta![0].CzyKartaPasuje(gra.siatka![docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {

                            if (gra.rezerwaOdkryta![0].numer == 13)
                                gra.siatka![docelowyWiersz, docelowaKolumna] = gra.rezerwaOdkryta![0];
                            else
                                gra.siatka![docelowyWiersz + 1, docelowaKolumna] = gra.rezerwaOdkryta![0];
                            gra.rezerwaOdkryta!.RemoveAt(0);




                            UI.UpdateUi(gra);
                        }
                        else
                        {

                            UI.UpdateUi(gra, "Podano niepoprawny ruch. Sprawdź czy nazwy kart się zgadzają.");
                        }

                    }
                }
                else if (miejsce == 0)
                {

                    UI.UpdateUi(gra, "Podano niepoprawny ruch. Sprawdź czy nazwy kart się zgadzają.");
                }
            }
            else if (source == "+")
            {
                if (gra.rezerwa!.Count > 0)
                {
                    if (gra.rezerwaOdkryta!.Count > 0)
                        gra.rezerwaOdkryta![0].odkryta = false;
                    gra.rezerwaOdkryta!.Insert(0, gra.rezerwa[0]);
                    gra.rezerwa.RemoveAt(0);





                    UI.UpdateUi(gra);
                }
                else
                {
                    for (int i = gra.rezerwaOdkryta!.Count - 1; i > -1; i--)
                    {
                        gra.rezerwa.Add(gra.rezerwaOdkryta![i]);
                        gra.rezerwaOdkryta!.RemoveAt(i);
                    }

                    UI.UpdateUi(gra);
                }

            }
            else
            {

                UI.UpdateUi(gra, "Podano niepoprawny ruch!\nNapisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");

            }
        }
        catch (Exception ex)
        {
            Utilities.Blad("Coś się stało, ale nie wiem co!", "Przeczytaj instrukcję w menu Jak Grać", ex);
        }
    }
}