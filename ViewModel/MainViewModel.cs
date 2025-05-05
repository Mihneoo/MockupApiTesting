using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MockupApiTesting.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Users> users { get; set; }
        HttpClient Client;
        JsonSerializerOptions _serializerOptions;
        string baseURl = "https://6807403ce81df7060eb95feb.mockapi.io";

        public MainViewModel()
        {
            Client = new HttpClient();
            users = new ObservableCollection<Users>();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public ICommand RetreiveAllUsers => new Command(async () =>
        {
            var url = $"{baseURl}/users";
            var response = await Client.GetAsync(url);
            var responseStream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<List<Users>>(responseStream, _serializerOptions);

            if (data != null)
            {
                users.Clear();
                foreach (var user in data)
                {
                    users.Add(user);
                }
            }
            // Logic to retrieve all users
        });

        public ICommand SearchUser => new Command(async (userId) =>
        {
            if (userId is not string userIdString || string.IsNullOrWhiteSpace(userIdString))
            {
                return;
            }

            var url = $"{baseURl}/users/{userId}";
            var response = await Client.GetAsync(url);
            var responseStream = await response.Content.ReadAsStreamAsync();
            var searchedUser = await JsonSerializer.DeserializeAsync<Users>(responseStream, _serializerOptions);

            if (searchedUser != null)
            {
                users.Clear();

                users.Add(searchedUser);

            }
        });

        public ICommand AddUser => new Command(async (name) =>
        {
            name = "asd";
        });

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
