using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public partial class ClientsPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new();
        private List<Client> _allClients = new();

        [ObservableProperty]
        private ObservableCollection<Client> clients = new();

        [ObservableProperty]
        private Client selectedClient;

        partial void OnSelectedClientChanged(Client value)
        {
            if (value is not null)
            {
                EditClient(value.Id);
            }
        }

        [ObservableProperty]
        private string searchText = string.Empty;

        public ClientsPageViewModel()
        {
            LoadClients();
            ApplyFilters();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    ApplyFilters();
                }
            };
        }

        private void LoadClients()
        {
            _allClients = _context.Clients
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Surname)
                .ToList();
        }

        private void ApplyFilters()
        {
            var filtered = _allClients.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.Trim().ToLowerInvariant();
                filtered = filtered.Where(c =>
                    c.Name.ToLower().Contains(lower)
                    || c.Surname.ToLower().Contains(lower)
                    || (c.PhoneNumber?.ToLower().Contains(lower) ?? false)
                    || (c.Email?.ToLower().Contains(lower) ?? false));
            }

            Clients.Clear();
            foreach (var client in filtered)
            {
                Clients.Add(client);
            }
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
        }

        [RelayCommand]
        private void AddClient()
        {
            ViewMediator.Instance.ChangeView(new AddClientPageViewModel());
        }

        [RelayCommand]
        private void EditClient(int clientId)
        {
            ViewMediator.Instance.ChangeView(new EditClientPageViewModel(clientId));
        }
    }
}
