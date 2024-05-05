using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSUrenRegistratie2.Services;
using CBSUrenRegistratie2.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CBSUrenRegistratie2.ViewModels
{
    public class UserManagementViewModel : BindableObject
    {
        public UserManagementViewModel(IUserInteraction userInteraction)
        {
            this.userInteraction = userInteraction;
            LoadUsersCommand.Execute(null);
            LoadProjectsCommand.Execute(null);
            DeleteUserCommand = new Command<int>(async (userId) => await DeleteUser(userId));
        }

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set
            {
                if (_projects != value)
                {
                    _projects = value;
                    OnPropertyChanged();
                }
            }
        }

        private IUserInteraction userInteraction;
        public ICommand DeleteUserCommand { get; private set; }
        public Command LoadUsersCommand => new Command(async () => await LoadUsers());
        public async Task LoadUsers()
        {
            var dbService = new DatabaseService();
            var users = await dbService.GetAllUsers(); // Fetch users from the database
            await Application.Current.Dispatcher.DispatchAsync(() => {
                var tempUsers = new ObservableCollection<User>(users); // Create a temporary ObservableCollection
                Users = tempUsers; // Replace the existing collection completely
                OnPropertyChanged(nameof(Users)); // Notify UI about the update
            });
        }

        public Command LoadProjectsCommand => new Command(async () => await LoadProjects());
        private async Task LoadProjects()
        {
            var dbService = new DatabaseService();
            var projects = await dbService.GetAllProjects();
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }

        public Command AddUserCommand => new Command(async (object parameter) =>
        {
            var userDetails = (object[])parameter;
            var newUser = new User
            {
                Username = userDetails[0].ToString(),
                Password = userDetails[1].ToString(),
                RoleId = int.Parse(userDetails[2].ToString())
            };
            await AddUser(newUser);
        });

        public async Task AddUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);  // Ensure password is hashed
            var dbService = new DatabaseService();
            await dbService.AddUser(user);
            Users.Add(user);
            LoadUsersCommand.Execute(null);
        }


        public Command UpdateUserCommand => new Command<User>(async (user) => await UpdateUser(user));
        private async Task UpdateUser(User user)
        {
            var dbService = new DatabaseService();
            await dbService.UpdateUser(user);
            int index = Users.IndexOf(Users.FirstOrDefault(u => u.UserId == user.UserId));
            if (index != -1)
            {
                Users[index] = user;
                OnPropertyChanged(nameof(Users));
            }
        }

        private async Task DeleteUser(int userId)
        {
            if (userInteraction == null)
            {
                throw new InvalidOperationException("User interaction service is not initialized.");
            }
            var confirm = await userInteraction.DisplayConfirmation("Confirm Delete", "Are you sure you want to delete this user?", "Yes", "No");
            if (confirm)
            {
                var dbService = new DatabaseService();
                await dbService.DeleteUser(userId);
                var user = Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    Users.Remove(user);
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

    }
}
