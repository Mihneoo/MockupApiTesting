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
        public ObservableCollection<Users> users_odd { get; set; }
        public ObservableCollection<Users> users_even { get; set; }
        HttpClient Client;
        JsonSerializerOptions _serializerOptions;
        string baseURl = "https://6807403ce81df7060eb95feb.mockapi.io";

        public MainViewModel()
        {
            Client = new HttpClient();
            users = new ObservableCollection<Users>();
            users_odd = new ObservableCollection<Users>();
            users_even = new ObservableCollection<Users>();
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
                users_even.Clear();
                users_odd.Clear();
                foreach (var user in data)
                {
                    users.Add(user);
                }
                foreach (var user in data)
                {
                    int id = int.Parse(user.id);
                    if (id % 2 == 0) //user id is even
                    {
                        users_even.Add(user);
                    }
                    else //user id is odd
                    {
                        users_odd.Add(user);
                    }
                    {

                    }
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
                users_odd.Clear();
                users_even.Clear();

                users_odd.Add(searchedUser);

            }
        });

        public ICommand AddUser => new Command(async (user) =>
        {
            
        });

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
