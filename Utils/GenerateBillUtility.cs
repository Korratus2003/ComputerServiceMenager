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
