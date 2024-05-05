using CBSUrenRegistratie2.ViewModels;
using CBSUrenRegistratie2.Models;

namespace CBSUrenRegistratie2.Views;

public partial class UserManagementPage : ContentPage, IUserInteraction
{
	public UserManagementPage()
	{
		InitializeComponent();
        BindingContext = new UserManagementViewModel(this);
    }
    public async Task<bool> DisplayConfirmation(string title, string message, string accept, string cancel)
    {
        return await DisplayAlert(title, message, accept, cancel);
    }
    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        var username = usernameEntry.Text;
        var password = passwordEntry.Text;
        int roleId;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Username and password cannot be empty.", "OK");
            return;
        }

        if (!int.TryParse(roleIdEntry.Text, out roleId))
        {
            await DisplayAlert("Error", "Role ID must be a number.", "OK");
            return;
        }

        var viewModel = BindingContext as UserManagementViewModel;
        if (viewModel != null)
        {
            await viewModel.AddUser(new User
            {
                Username = username,
                Password = password,
                RoleId = roleId
            });
            await DisplayAlert("Success", "User added successfully.", "OK");
        }
    }
}