using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Npgsql;

namespace ComputerServiceManager.ViewModels
{
    public partial class SettingsPageViewModel : ViewModelBase
    {
        private const string EnvFilePath = ".env";

        [ObservableProperty] private string _dbHost;
        [ObservableProperty] private string _dbPort;
        [ObservableProperty] private string _dbUsername;
        [ObservableProperty] private string _dbPassword;
        [ObservableProperty] private string _dbName;
        [ObservableProperty] private string _errorMessage;

        public SettingsPageViewModel()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(EnvFilePath)) return;

            var lines = File.ReadAllLines(EnvFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) continue;

                switch (parts[0].Trim())
                {
                    case "DB_HOST":
                        DbHost = parts[1].Trim();
                        break;
                    case "DB_PORT":
                        DbPort = parts[1].Trim();
                        break;
                    case "DB_USERNAME":
                        DbUsername = parts[1].Trim();
                        break;
                    case "DB_PASSWORD":
                        DbPassword = parts[1].Trim();
                        break;
                    case "DB_NAME":
                        DbName = parts[1].Trim();
                        break;
                }
            }
        }

        [RelayCommand]
        private async Task SaveSettings()
        {
            if (await TestConnection())
            {
                try
                {
                    var lines = new List<string>
                    {
                        $"DB_HOST={DbHost}",
                        $"DB_PORT={DbPort}",
                        $"DB_USERNAME={DbUsername}",
                        $"DB_PASSWORD={DbPassword}",
                        $"DB_NAME={DbName}"
                    };

                    File.WriteAllLines(EnvFilePath, lines);
                    ErrorMessage = "";

                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Save error: {ex.Message}";
                }
            }
        }

        private async Task<bool> TestConnection()
        {
            if (string.IsNullOrWhiteSpace(DbHost) ||
                string.IsNullOrWhiteSpace(DbPort) ||
                string.IsNullOrWhiteSpace(DbUsername) ||
                string.IsNullOrWhiteSpace(DbPassword))
            {
                ErrorMessage = "Missing required fields to test the connection.";
                return false;
            }

            if (!int.TryParse(DbPort, out int port) || port < 1 || port > 65535)
            {
                ErrorMessage = "Invalid port number.";
                return false;
            }

            var connectionString =
                $"Host={DbHost};Port={DbPort};Username={DbUsername};Password={DbPassword};";

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                ErrorMessage = "Connection tested successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection error:\n{ex.Message}";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task ExportDatabase()
        {
            await SaveSettings();
            var filePath = await FileDialog.OpenSaveDialog(new List<(string, List<string>)>
            {
                ("Backup Files", new List<string> { "backup" })
            }, $"{DbName}.backup");
            
            if (string.IsNullOrEmpty(filePath))
                return;

            var psi = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = $"-h {DbHost} -p {DbPort} -U {DbUsername} -F c -b -v -f \"{filePath}\" {DbName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            psi.Environment["PGPASSWORD"] = DbPassword;

            try
            {
                using var process = Process.Start(psi);
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                ErrorMessage = process.ExitCode == 0 ? "Export completed successfully!" : $"Export failed:\n{error}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Export error: {ex.Message}";
            }
        }



        [RelayCommand]
        private async Task ImportDatabase()
        {
            await SaveSettings();

            var createDbPsi = new ProcessStartInfo
            {
                FileName = "createdb",
                Arguments = $"-h {DbHost} -p {DbPort} -U {DbUsername} {DbName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            createDbPsi.Environment["PGPASSWORD"] = DbPassword;

            try
            {
                using var createProcess = Process.Start(createDbPsi);
                string createOutput = await createProcess.StandardOutput.ReadToEndAsync();
                string createError = await createProcess.StandardError.ReadToEndAsync();
                await createProcess.WaitForExitAsync();

                if (createProcess.ExitCode != 0)
                {
                    ErrorMessage = $"Nie udało się utworzyć bazy danych:\n{createError}";
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd tworzenia bazy danych: {ex.Message}";
                return;
            }

            var filePath = await FileDialog.OpenFileDialog(new List<(string, List<string>)>
                {
                    ("Backup Files", new List<string> { "backup" })
                }, $"{DbName}.backup");
            
            if (string.IsNullOrEmpty(filePath))
                return;

            var restorePsi = new ProcessStartInfo
            {
                FileName = "pg_restore",
                Arguments = $"-h {DbHost} -p {DbPort} -U {DbUsername} -d {DbName} -v \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            restorePsi.Environment["PGPASSWORD"] = DbPassword;

            try
            {
                using var restoreProcess = Process.Start(restorePsi);
                string restoreOutput = await restoreProcess.StandardOutput.ReadToEndAsync();
                string restoreError = await restoreProcess.StandardError.ReadToEndAsync();
                await restoreProcess.WaitForExitAsync();

                ErrorMessage = restoreProcess.ExitCode == 0
                    ? "Import zakończony pomyślnie!"
                    : $"Import nie powiódł się:\n{restoreError}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd importu: {ex.Message}";
            }
        }
    }
}