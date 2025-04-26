namespace Pasjans;

/// <summary>
/// Odpowiada za wykonywanie ruchów
/// </summary>
public static class Ruch
{
    /// <summary>
    /// Odpowiada za zapytanie o i wykonanie ruchu
    /// </summary>
    /// <param name="siatka">Siatka kart</param>
    /// <param name="rezerwaOdkryta">Rezerwa odkryta</param>
    /// <param name="rezerwa">Rezerwa</param>
    /// <param name="kartyGora">Stosy końcowe</param>
    public static void Rusz(ref Karta[,] siatka, ref List<Karta> rezerwaOdkryta, ref List<Karta> rezerwa, ref Karta[,] kartyGora, bool isMove, string source, string destination, bool czyError = false)
    {
        try
        {
            if (isMove)
            {
                int miejsce = Siatka.ZnajdzKarte(source, siatka, rezerwaOdkryta, kartyGora, out int wiersz, out int kolumna);

                if (miejsce == 1)
                {
                    if (destination == "Pik" || destination == "Karo" || destination == "Kier" || destination == "Trefl")
                    {
                        //int docelowaKolumna = destination == "Kier" ? 0 : destination == "Karo" ? 1 : destination == "Trefl" ? 2 : destination == "Pik" ? 3 : throw new Exception("Nieznany kolor! Błąd w linijce 111");
                        int docelowaKolumna = siatka[wiersz, kolumna].indexKoloru;
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(kartyGora, docelowaKolumna);
                        bool canIt = Karta.CzyKartaPasuje(siatka[wiersz, kolumna], kartyGora[docelowyWiersz, docelowaKolumna], false);

                        if (canIt && Siatka.CzyOstatni(siatka, wiersz, kolumna))
                        {
                            if (!czyError) if (!czyError) Debug.Add($"{source}-{destination}");

                            if (siatka[wiersz, kolumna].numer != 1)
                                kartyGora[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
                            else
                                kartyGora[docelowyWiersz, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            if (!czyError) //if (!czyError) Debug.Add($"{source}-{destination}");
                                if (!czyError)
                                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = false;

                        if (siatka[wiersz, kolumna].numer == 13)
                        {
                            if (siatka[0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = Karta.CzyKartaPasuje(siatka[wiersz, kolumna], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (!czyError) Debug.Add($"{source}-{destination}");
                            if (Siatka.CzyOstatni(siatka, wiersz, kolumna))
                            {
                                if (siatka[wiersz, kolumna].numer == 13)
                                    siatka[docelowyWiersz, docelowaKolumna] = siatka[wiersz, kolumna];
                                else
                                    siatka[docelowyWiersz + 1, docelowaKolumna] = siatka[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                siatka[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                                if (!czyError) //if (!czyError) Debug.Add($"{source}-{destination}");


                                    if (!czyError)
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
                                        {
                                            siatka[docelowyWiersz, docelowaKolumna] = siatka[indexKarty, kolumna];
                                            docelowyWiersz--;
                                        }
                                        else
                                            siatka[docelowyWiersz + 1, docelowaKolumna] = siatka[indexKarty, kolumna];

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                        siatka[indexKarty, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.



                                        indexKarty++;
                                        docelowyWiersz++;
                                    }
                                    if (!czyError)
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
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 2)// obsługa kart ze stosów końcowych
                {
                    if (int.TryParse(destination, out int docelowaKolumna))
                    {
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = false;

                        if (kartyGora[wiersz, kolumna].numer == 13)
                        {
                            if (siatka[0, docelowaKolumna] == null)
                            {
                                canIt = true;
                            }
                        }
                        else
                            canIt = Karta.CzyKartaPasuje(kartyGora[wiersz, kolumna], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (!czyError) Debug.Add($"{source}-{destination}");
                            if (kartyGora[wiersz, kolumna].numer == 13)
                                siatka[docelowyWiersz, docelowaKolumna] = kartyGora[wiersz, kolumna];
                            else
                                siatka[docelowyWiersz + 1, docelowaKolumna] = kartyGora[wiersz, kolumna];
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                            kartyGora[wiersz, kolumna] = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                            if (!czyError) //if (!czyError) Debug.Add($"{source}-{destination}");

                                if (!czyError)
                                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }
                    }
                }
                else if (miejsce == 3)
                {
                    if (destination == "Pik" || destination == "Karo" || destination == "Kier" || destination == "Trefl")
                    {
                        int docelowaKolumna = rezerwaOdkryta[0].indexKoloru;
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(kartyGora, docelowaKolumna);
                        bool canIt = Karta.CzyKartaPasuje(rezerwaOdkryta[0], kartyGora[docelowyWiersz, docelowaKolumna], false);

                        if (canIt)
                        {
                            if (!czyError) Debug.Add($"{source}-{destination}");
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
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Ta karta tu nie pasuje!\nJeżeli nie wiesz jak grać napisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");
                        }

                    }
                    else
                    {
                        int docelowaKolumna = int.Parse(destination);
                        int docelowyWiersz = Siatka.znajdzOstatniaKarte(siatka, docelowaKolumna);

                        bool canIt = Karta.CzyKartaPasuje(rezerwaOdkryta[0], siatka[docelowyWiersz, docelowaKolumna], true);

                        if (canIt)
                        {
                            if (!czyError) Debug.Add($"{source}-{destination}");
                            if (rezerwaOdkryta[0].numer == 13)
                                siatka[docelowyWiersz, docelowaKolumna] = rezerwaOdkryta[0];
                            else
                                siatka[docelowyWiersz + 1, docelowaKolumna] = rezerwaOdkryta[0];
                            rezerwaOdkryta.RemoveAt(0);

                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                        }
                        else
                        {
                            if (!czyError)
                                UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Podano niepoprawny ruch. Sprawdź czy nazwy kart się zgadzają.");
                        }

                    }
                }
                else if (miejsce == 0)
                {
                    if (!czyError)
                        UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Podano niepoprawny ruch. Sprawdź czy nazwy kart się zgadzają.");
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
                    if (!czyError) Debug.Add("+");
                    if (!czyError)
                        UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
                else
                {
                    for (int i = rezerwaOdkryta.Count - 1; i > -1; i--)
                    {
                        rezerwa.Add(rezerwaOdkryta[i]);
                        rezerwaOdkryta.RemoveAt(i);
                    }
                    if (!czyError)
                        UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa);
                }
            }
            else if (source == "bug")
            {
                Debug.Zapisz();
                if (!czyError)
                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "error.txt zapisany");
            }
            else
            {
                if (!czyError)
                    UI.UpdateUi(siatka, rezerwaOdkryta, kartyGora, rezerwa, "Podano niepoprawny ruch!\nNapisz X aby wyjść i przeczytaj instrukcję w menu Jak Grać");

            }
        }
        catch (Exception ex)
        {
            Utilities.Blad("Coś się stało, ale nie wiem co!", "Przeczytaj instrukcję w menu Jak Grać", ex);
        }
    }
}