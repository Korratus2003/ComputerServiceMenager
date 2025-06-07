#  ComputerServiceMenager

# Opis
Projekt został wykonany na potrzeby zaliczenia przedmiotu Programowanie Obiektowe realizowanego na Uniwersytecie Rzeszowskim. Celem projektu było praktyczne zastosowanie zasad programowania obiektowego w języku C#, w tym implementacja logiki biznesowej, pracy z bazą danych oraz obsługi migracji z wykorzystaniem Entity Framework Core.

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

## Migracje (opcjonalne)
Aplikacja sama sobie utworzy baze danych i wypełni przykładowaymi danymi ale migracje również zadziałają


```bash
dotnet ef database update
```

Projekt zawiera m.in.:

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
- W aplikacji do komunikacja z drukarką fiskalną jest zgodna z protokołem thermal firmy POSNET. Pełna specyfikacja protokołu znajduje się [tutaj](https://www.soft-bit.pl/downloads/all/Posnet/pliki/THS-I-DEV-02-006_specyfikacja_protokolu_Thermal_w_Thermal_HS_FV.pdf) 
- W przypadku braku fizycznej drukarki, możliwe jest testowanie integracji przy pomocy emulatora DFEmul – zgodnie z instrukcją opisaną [tutaj](https://what-it.pl/2023/05/25/emulacja-drukarki-fiskalnej/)
  Na linuksie w celu emulacji podpięcia można zastosować komendy

  
  ```bash
  sudo socat -d -d pty,raw,echo=0,link=/dev/ttyV4 pty,raw,echo=0,link=/dev/ttyV5 
  sudo chmod 666 /dev/ttyV4 /dev/ttyV5
  ```
---

## Główne cele aplikacji

- Łatwe zarządzanie urządzeniami, klientami, technikami
- Generowanie rachunków za serwisy (Możliwa rozbudowa o sprzedaż urządzeń)
- Działanie na każdym systemie operacyjnym (Windows/Mac/Linux)

## Cele i założenia projektu
- Ułatwienie codziennej pracy serwisów komputerowych
- Możliwość rozbudowy o moduły sprzedaży sprzętu i magazyn
- Natywna obsługa systemów Windows, macOS i Linux
- Niezależność od przeglądarki – pełnoprawna aplikacja desktopowa

---

# Działanie
Gdy baza danych jest pusta, trzeba założyć główne konto administratora które ma dostęp do techników i zmian ustawień aplikacji
![image](https://github.com/user-attachments/assets/b14570f5-4641-452c-9d2c-22a5eb73c909)

Następnie mamy informacje że możemy przypisać do konta technika, ponieważ w serwisach często sam właściciel jest też serwisantem
![image](https://github.com/user-attachments/assets/a6b3d761-e91a-42b2-9507-b2d203468b72)

Po zapoznaniu się z informacją możemy się zalogować
![image](https://github.com/user-attachments/assets/c15a432a-d7f2-419b-b087-8fcd34b91d55)

Jako admin na pasku bocznym mamy dostęp do wszystkiego

![image](https://github.com/user-attachments/assets/2df8ee85-3b8b-4738-99c1-71a04d3028fb)

W tym zmiany ustawień
![image](https://github.com/user-attachments/assets/e73d18f9-4cda-45d8-963f-6525b86fdab3)

możemy tu zmienić dane do połączenia z bazą danych oraz wybrać port COM do komunikacji z drukarką fiskalną, w systemach UNIXowych sprawdzamy po prostu katalog z portami a w windowsie używamy biblioteki System.IO.Ports
![image](https://github.com/user-attachments/assets/0062e23b-5531-4b89-aee6-a27f616be3d7)

Możemy również wyeksportować i zaimportować baze danych, jest tu wykorzystane wywołanie podprocesu przez c#, konkretnie narzędzi z postgresa które do tego służą: pg_dump i pg_restore
![image](https://github.com/user-attachments/assets/d5593683-f501-4499-9309-2fbbb7144766)
![image](https://github.com/user-attachments/assets/ff0c389a-1521-4b38-ac5e-f2aaace93586)
![image](https://github.com/user-attachments/assets/edf2ddd1-91a5-4650-9360-2a5745071ce7)

Dodatkowo dla pewności przed importem i eksportem zapisywane są ustawienia

Import i eksport odbywa sie przy pomocy systemowego okna wyboru pliku
![image](https://github.com/user-attachments/assets/b66a5195-df7d-4185-85d8-fba8025170a6)
implementacja tego znajduje sie w pliku FileDialog.cs

Wszystkie ustawienia są zapisywane do pliku .env


## Panel techników
![image](https://github.com/user-attachments/assets/f1f53bfe-1ccc-4137-b402-4d95e0a4dac7)

Tutaj są wyświetlani technicy, można ich również filtrować, dodawać i edytować
![image](https://github.com/user-attachments/assets/fa938140-cf87-4679-b1ac-ac30fd48e858)
![image](https://github.com/user-attachments/assets/e7a37b96-20b6-4ffe-a218-83d1948bb269)
![image](https://github.com/user-attachments/assets/96eb6499-ca16-4031-9487-d08740b75634)

Podczas dodawnia/edycji dane są walidowane np. nr telefonu musi mieć 9 cyfr

## Panel generowania rachunku
![image](https://github.com/user-attachments/assets/97706121-5215-47fb-afbf-fd0f3e71af12)

Aby wygenerować rachunek trzeba wybrać klienta (można wyszukiwać)
![image](https://github.com/user-attachments/assets/aa0b5a6c-f27b-4f88-8948-ddbffe8aa084)
Następnie można wybrać serwisy tego klienta których stan jest nieopłacony
![image](https://github.com/user-attachments/assets/2e328a9a-3af0-48a6-b84d-d0b03d9d8c50)
![image](https://github.com/user-attachments/assets/fe524323-0a01-4541-bec2-0dd637aa7ac6)
Jak widać w emulatorze rachunek został wygenerowany prawidłowo


![image](https://github.com/user-attachments/assets/f8a964f5-2cef-467f-9fe0-8db26b4654f6)

## Panel zarządzania serwisami (nie dokończony) 

Panel ten miał pozwalać w łatwy sposób zarządzać dodawać i edytować serwisy oraz śledzić ich stan, każdy technik powinien mieć po zalogowaniu swoje serwisy których stan zmienia aby odzwierciedlały rzeczywistość

Aktualnie panel ten wygląda tak:
![image](https://github.com/user-attachments/assets/b1bc96dd-936f-41f6-9b5c-d363e68d3ec0)

## Panel klientów (nie dokończony)

Podobnie jak panel techników powinien zapewnić łatwy dostęp do edycji dodania i przeglądania klientów

![image](https://github.com/user-attachments/assets/fda0f0ac-b86b-4803-8c68-443416a66a77)

Aktualnie działa dodawanie i edycja klienta
![image](https://github.com/user-attachments/assets/b15bc41d-3158-4467-b796-7546df7bd794)
![image](https://github.com/user-attachments/assets/3f2aed9c-4a46-4e29-9bc2-191e13ac09d3)

Działanie formularzy zostało częściowo zaimplementowane.
Tak jak w panelu techników tu również dane są walidowane

## Panel urządzeń (nie dokończony)

![image](https://github.com/user-attachments/assets/151a4a9c-7d5d-4359-af0b-9dd6ab4b44c8)

Podobnie jak panel techników powinien zapewnić łatwy dostęp do edycji dodania i przeglądania urządzeń, powienien zawierać ich zdjęcia

Aktualnie działa tylko edycja urządzenia

![image](https://github.com/user-attachments/assets/02a68c60-529e-444e-b1d5-b013c424b7b6)

## Dodatkowe informacje o interfejsie
![image](https://github.com/user-attachments/assets/574d1e35-6ca7-4956-999d-84cc13c0c5f7)
Możemy się łatwo wylogować

Zależnie od rangi usera menu zawiera inne rzeczy]

![image](https://github.com/user-attachments/assets/443ac0ff-2e28-4239-b37e-bbf7cc220fc9)

---

# Ciekawsze fragmenty kodu
## GenerateBillUtility.cs
```bash
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ComputerServiceManager.Utils
{
    /// <summary>
    /// Reprezentuje pojedynczy produkt/usługę do wydrukowania na paragonie.
    /// </summary>
    public class Produkt
    {
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
        public decimal Cena { get; set; }
        /// <summary>
        /// Stawka VAT: 'A','B','C','D' itd. Domyślnie 'A'.
        /// </summary>
        public char StawkaVAT { get; set; } = 'A';
    }

    /// <summary>
    /// Klasa pomocnicza do generowania i wysyłania komend do drukarki Thermal HS FV.
    /// </summary>
    public static class GenerateBillUtility
    {
        /// <summary>
        /// Drukuje paragon z podanych produktów na wskazanym porcie szeregowym.
        /// </summary>
        /// <param name="produkty">Lista produktów do wydrukowania.</param>
        /// <param name="portPath">Ścieżka do urządzenia (np. "/dev/ttyV5" lub "COM1").</param>
        public static void Print(List<Produkt> produkty, string portPath = "/dev/ttyV5")
        {
            if (produkty == null || produkty.Count == 0)
            {
                Console.WriteLine("Brak produktów do wydrukowania.");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(portPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    Console.WriteLine($"Połączono z {portPath}.");

                    // 1. Wyłącz pokazywanie błędów na wyświetlaczu drukarki (LBSERM: ESC P 1#e <check> ESC \)
                    //    Dzięki temu w razie błędu nie będziemy czekali na wciśnięcie klawisza "NIE".
                    SendCommand(fs, "1#e");

                    // 2. Rozpocznij transakcję (LBTRSHDR: ESC P Pl $h <check> ESC \)
                    //    Parametr Pl = liczba pozycji sprzedaży. Jeśli Pl>0, drukarka działa w trybie "blokowym".
                    string startTrans = $"{produkty.Count}$h";
                    SendCommandWithStatusCheck(fs, startTrans, "LBTRSHDR");
                    
                    // 3. Prześlij kolejno każdą linię paragonu (LBTRSLN).
                    //    Format komendy:
                    //    ESC P Pi $l <nazwa> CR <ilość> CR <ptu>/<cena_jednostkowa>/<wartość_brutto>/ <check> ESC \
                    int linia = 1;
                    foreach (var produkt in produkty)
                    {
                        // Oblicz wartość brutto (ilość * cena)
                        decimal wartoscBrutto = produkt.Ilosc * produkt.Cena;

                        // Ograniczenie nazwy do maks. 40 znaków
                        string nazwa = produkt.Nazwa.Length <= 40
                            ? produkt.Nazwa
                            : produkt.Nazwa.Substring(0, 40);

                        // Formatowanie liczb: kompresja zer; np. "5" zamiast "5.00"
                        string cenaTekst = produkt.Cena % 1 == 0 
                            ? ((int)produkt.Cena).ToString() 
                            : produkt.Cena.ToString("0.00");
                        string wartoscTekst = wartoscBrutto % 1 == 0 
                            ? ((int)wartoscBrutto).ToString() 
                            : wartoscBrutto.ToString("0.00");

                        // Zbuduj treść komendy LBTRSLN
                        // Pi = numer linii
                        // "$l" – identyfikator sekwencji LBTRSLN
                        // Nazwa + CR + ilość + CR + stawka "/" cena "/" wartość "/"
                        string lineContent = $"{linia}$l{nazwa}\r{produkt.Ilosc}\r{produkt.StawkaVAT}/{cenaTekst}/{wartoscTekst}/";

                        // Wyślij komendę i sprawdź status
                        SendCommandWithStatusCheck(fs, lineContent, $"LBTRSLN linia {linia}");

                        linia++;
                    }

                    // 4. Zakończenie transakcji (LBTREXIT: ESC P 0 $e <check> ESC \)
                    //    Parametr "0" oznacza standardowe potwierdzenie transakcji.
                    SendCommandWithStatusCheck(fs, "0$e", "LBTREXIT");

                    // 5. Obcinanie papieru (komenda ESC [GS] 'V' 0)
                    byte[] cut = { 0x1D, 0x56, 0x00 };
                    fs.Write(cut, 0, cut.Length);
                    fs.Flush();

                    Console.WriteLine("Paragon został pomyślnie wydrukowany i papier został obcięty.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas drukowania paragonu: " + ex.Message);
            }
        }

        /// <summary>
        /// Buduje pełną sekwencję dla drukarki: ESC P + treść + checksum (2 hex) + ESC \ .
        /// </summary>
        /// <param name="content">Zawartość komendy (np. "5$h" lub "1#e" lub "2$lNazwa...").</param>
        /// <returns>Tablica bajtów gotowa do wysłania.</returns>
        private static byte[] BuildCommand(string content)
        {
            const char ESC = (char)0x1B;
            // Prefix "ESC P"
            string prefix = $"{ESC}P";
            // Oblicz checksumę (2 znaki HEX) dla całej treści od razu po "P"
            string checksum = ComputeChecksum(content);
            // Zakończenie komendy "ESC \"
            string terminator = $"{ESC}\\";

            // Pełna sekwencja: ESC P + content + checksum + ESC \
            string full = prefix + content + checksum + terminator;
            return Encoding.ASCII.GetBytes(full);
        }

        /// <summary>
        /// Oblicza bajt kontrolny (check) dla danej sekwencji (wszystkie znaki content XOR-em, 
        /// począwszy od pierwszego znaku content aż do ostatniego przed check).
        /// </summary>
        /// <param name="data">Ciąg znaków pomiędzy prefixem a terminatorem (bez ESC P i bez CC i ESC \).</param>
        /// <returns>Dwuznakowy ciąg HEX (np. "4A"). Wszystkie litery wielkie.</returns>
        private static string ComputeChecksum(string data)
        {
            byte chk = 0xFF;
            foreach (char c in data)
            {
                chk ^= (byte)c;
            }
            return chk.ToString("X2"); // dwie cyfry HEX, wielkie litery
        }

        /// <summary>
        /// Wysyła prostą komendę do drukarki (bez odczytu statusu).
        /// </summary>
        private static void SendCommand(FileStream fs, string content)
        {
            byte[] cmd = BuildCommand(content);
            fs.Write(cmd, 0, cmd.Length);
            fs.Flush();
            Thread.Sleep(100); // krótka pauza, aby drukarka zdążyła przetworzyć
        }

        /// <summary>
        /// Wysyła komendę do drukarki i zaraz potem pyta drukarkę o status (ENQ),
        /// sprawdzając, czy bit CMD=1 (ostatnia sekwencja została wykonana poprawnie).
        /// Jeśli bajt statusowy ma CMD=0, wypisuje ostrzeżenie.
        /// </summary>
        private static void SendCommandWithStatusCheck(FileStream fs, string content, string description)
        {
            // 1) Wyślij samą sekwencję
            byte[] cmd = BuildCommand(content);
            fs.Write(cmd, 0, cmd.Length);
            fs.Flush();
            Thread.Sleep(100);

            // 2) Wyślij ENQ ($05), aby pobrać status
            fs.WriteByte(0x05);
            fs.Flush();

            // 3) Poczekaj chwilę i odczytaj odpowiedź
            Thread.Sleep(50);
            int status = fs.ReadByte();
            if (status == -1)
            {
                Console.WriteLine($"[{description}] — nie otrzymano odpowiedzi na ENQ.");
                return;
            }

            // Bit CMD = bit nr 2 (licząc od 0, wartości 0x04)
            bool cmdBit = (status & 0x04) != 0;
            if (!cmdBit)
            {
                Console.WriteLine($"[{description}] — drukarka zwróciła przy ENQ status CMD=0 (błąd w wykonaniu).");
            }
            else
            {
                // Dla celów debugowania można wypisać: Console.WriteLine($"[{description}] — OK.");
            }
        }
    }

    /// <summary>
    /// Pomocniczna klasa do enumeracji dostępnych portów szeregowych,
    /// zarówno w Windows, jak i w *nix-owych systemach (Linux/macOS).
    /// </summary>
    public static class Ports
    {
        public static string[] GetAvailablePorts()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return SerialPort.GetPortNames();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                string[] prefixes = { "ttyUSB", "ttyACM", "ttyS", "ttyV" };
                return Directory.GetFiles("/dev")
                    .Where(name => prefixes.Any(p => name.StartsWith("/dev/" + p)))
                    .ToArray();
            }
            return Array.Empty<string>();
        }
    }
}

```
Tutaj następuje wysyłanie rachunku do drukarki fiskalnej

## ViewMediator.cs

```bash
using System;
using ComputerServiceManager.ViewModels;

public class ViewMediator
{
    private static readonly ViewMediator _instance = new();
    public static ViewMediator Instance => _instance;

    private ViewModelBase? _activeViewModel;

    public ViewModelBase? ActiveViewModel
    {
        get => _activeViewModel;
        private set
        {
            _activeViewModel = value;
            ViewChanged?.Invoke(value);
        }
    }

    public event Action<ViewModelBase>? ViewChanged;
    
    public void ChangeView(ViewModelBase newView)
    {
        ActiveViewModel = newView;
    }
}
```
Aby nie przekazywać widoku głównego okna w konstruktorze zastosowałem Singleton, dzięki temu mam jedną instancje klasy i moge sie odwołać do funkcji która zmienia widok w każdym pliku

## Walidacja (w tym przypadku technika)
```bash
        public bool IsValidate()
        {

            if (string.IsNullOrWhiteSpace(Technician.Name) || Technician.Name.Length < 2)
            {
                Error = "Name is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Technician.Surname) || Technician.Surname.Length < 2)
            {
                Error = "Surname is required";
                return false;
            }

            
            var digitsOnly = new string(Technician.PhoneNumber.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length != 9)
            {
                Error = "Phone number must contain exactly 9 digits.";
                return false;
            }

            var phonePattern = @"^\+?[0-9\s\-]*$";
            if (!Regex.IsMatch(Technician.PhoneNumber, phonePattern))
            {
                Error = "Phone number contains invalid characters.";
                return false;
            }

            if (Technician.EmploymentDate == null)
            {
                Error = "Employment date is required";
                return false;
            }

            return true;
        }
```

# Podsumowanie
ComputerServiceManager to projekt z potencjałem do realnego wykorzystania w małych i średnich serwisach komputerowych. Jego zalety to:

prostota wdrożenia,

przejrzysty kod źródłowy,

łatwość dalszej rozbudowy,

niezależność od konkretnej platformy systemowej.








