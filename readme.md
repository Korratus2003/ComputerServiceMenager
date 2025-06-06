﻿#  ComputerServiceMenager

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

---

