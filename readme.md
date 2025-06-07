#  ComputerServiceMenager

## Klonowanie projektu

Aby sklonować ten projekt, użyj poniższego polecenia:

```bash
git clone https://github.com/Korratus2003/ComputerServiceMenager.git
```



---

##  Wymagania

- [.NET 9.0 SDK lub nowszy](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL 17](https://www.postgresql.org/download/)
- macOS/Linux/Windows 10 albo nowszy  



---

## Konfiguracja środowiska

Przed uruchomieniem projektu upewnij się, że masz utworzony plik `.env` w katalogu głównym projektu zgodny z poniższym przykładem:

```env
DB_HOST=localhost
DB_PORT=5432
DB_USERNAME=postgres
DB_PASSWORD=1234
DB_NAME=postgres
```


---

## Przywracanie zależności
Wszystkie wymagane biblioteki (NuGet packages) zostaną automatycznie pobrane po uruchomieniu polecenia:
```bash
dotnet restore
```

Pprojekt zawiera m.in.:

- Avalonia UI (Avalonia, Avalonia.Desktop, Themes.Fluent, itd.)
- CommunityToolkit.Mvvm
- Entity Framework Core (z obsługą PostgreSQL)
- DotNetEnv – do odczytu pliku .env
- Microsoft.Extensions.* – generatory i analiza kodu


## Uruchamianie projektu

Po spełnieniu powyższych wymagań i utworzeniu pliku `.env`, możesz uruchomić projekt za pomocą:

```bash
dotnet run
```

---

## Informacje dodatkowe

- Projekt używa biblioteki `DotNetEnv` do ładowania zmiennych środowiskowych.
- Upewnij się, że serwer PostgreSQL działa i baza danych jest dostępna pod podanym adresem.
- W aplikacji do komunikacja z drukarką fiskalną jest zgodna z protokołem thermal firmy POSNET dostępnym [tutaj](https://www.soft-bit.pl/downloads/all/Posnet/pliki/THS-I-DEV-02-006_specyfikacja_protokolu_Thermal_w_Thermal_HS_FV.pdf) 
- Ze wzglęðu na brak drukarki, do testowania był używany emulator DFEmul według poradnika z [tej strony](https://what-it.pl/2023/05/25/emulacja-drukarki-fiskalnej/)
---

## Główne cele aplikacji

- Łatwe zarżadzanie urządzeniami, klientami, technikami
- Generwoanie rachunków za serwisy (Możliwa rozbudowa o sprzedaż urządzeń)

# Działanie
Gdy baza danych jest pusta, trzeba założyć główne konto administratora które ma dostęp do techników i zmian ustawień aplikacji


