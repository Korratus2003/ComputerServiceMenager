using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ComputerServiceManager.Utils
{
    public class Produkt
    {
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
        public decimal Cena { get; set; }
        public char StawkaVAT { get; set; } = 'A';
    }

    public static class GenerateBillUtility
    {
        public static void DrukujParagon(List<Produkt> produkty, string portPath = "/dev/ttyV5")
        {
            try
            {
                using (FileStream fs = new FileStream(portPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    Console.WriteLine($"Połączono z {portPath}.");

                    // --- Start transakcji ---
                    string startTrans = $"{produkty.Count}$h";
                    fs.Write(BuildCommand(startTrans), 0, BuildCommand(startTrans).Length);
                    fs.Flush();
                    Thread.Sleep(200);

                    // --- Linie paragonu ---
                    int linia = 1;
                    foreach (var produkt in produkty)
                    {
                        decimal wartosc = produkt.Ilosc * produkt.Cena;
                        string line = $"{linia}$l{produkt.Nazwa}\r{produkt.Ilosc}\r{produkt.StawkaVAT}/{produkt.Cena:0.00}/{wartosc:0.00}/";
                        fs.Write(BuildCommand(line), 0, BuildCommand(line).Length);
                        fs.Flush();
                        Thread.Sleep(200);
                        linia++;
                    }

                    // --- Zakończenie transakcji ---
                    fs.Write(BuildCommand("0$e"), 0, BuildCommand("0$e").Length);
                    fs.Flush();
                    Thread.Sleep(200);

                    // --- Odcięcie papieru ---
                    byte[] cut = { 0x1D, 0x56, 0x00 };
                    fs.Write(cut, 0, cut.Length);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas drukowania paragonu: " + ex.Message);
            }
        }

        private static byte[] BuildCommand(string content)
        {
            char ESC = (char)0x1B;
            string prefix = ESC + "P";
            string terminator = ESC + "\\";
            string checksum = ComputeChecksum(content);
            string full = prefix + content + checksum + terminator;
            return Encoding.ASCII.GetBytes(full);
        }

        private static string ComputeChecksum(string data)
        {
            byte chk = 0xFF;
            foreach (char c in data)
                chk ^= (byte)c;
            return chk.ToString("X2");
        }
    }
}
