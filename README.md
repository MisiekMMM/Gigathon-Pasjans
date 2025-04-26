# Pasjans CMD

![Gif pokazujący grę](https://i.imgur.com/s0Tw15X.gif)

## About <a name = "about"></a>

Projekt Pasjans na gigathon.

![Logo gigathonu](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRbH1OgE0YL9WkzSdeRhjC3raBsJka_z5MGrg&s)



### Wymagania wstępne


- [dotnet 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)


### Instalacja

Wpisz komendę:

```
dotnet run
```

Wpisz ją tylko za pierwszym razem. Później uruchamiaj program, który pojawi się w bin/debug/net9.0/Pasjans.exe 


## Jak Grać <a name = "usage"></a>

### Cel gry: 

Ułożyć wszystkie karty w czterech kolorach (♠ ♥ ♦ ♣) od asa do króla na stosach końcowych

### Rozgrywka:

- Karty są rozłożone w 7 kolumnach: od 1 do 7 kart, tylko ostatnia karta w kolumnie odkryta.

- Pozostałe karty tworzą rezerwę.

- Karty można układać malejąco i naprzemiennie kolorami w kolumnach (np. czarna 7 na czerwoną 8).

- Tylko król może być przeniesiony na pustą kolumnę.

- Asy przenosi się na stosy końcowe i buduje kolory rosnąco (A, 2, 3, ..., K).

- Z talii dobierania można przeglądać karty i przenosić je do kolumn lub na stosy końcowe.

- Aby poruszać kartą między stosami należy napisać jej nazwę myślnik (-) i numer kolumny, np. Q Kier-1. Brana pod uwagę jest wielkość liter i odstępy

- Aby przenieść kartę na stos końcowy wystarczy podać jej nazwę, np. As Pik

- Aby odkryć nową kartę z rezerwy należy napisać +

### Wygrana:
Gra kończy się, gdy wszystkie cztery kolory zostaną ułożone na stosach końcowych od asa do króla.


---
#### Wszystkie opisy klas i metod są zawarte w kodzie jako komentarze w tagach summary