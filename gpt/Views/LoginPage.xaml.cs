using CBSUrenRegistratie2.Services;
namespace CBSUrenRegistratie2.Views
{
public partial class LoginPage : ContentPage
{
    private AuthService authService = new AuthService();
    public LoginPage()
	{
		InitializeComponent();
	}
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;


            var user = await authService.Authenticate(username, password);
            if (user != null)
            {
                App.CurrentUserId = user.UserId;

                if (user.Role == "Admin")
                {
                    await Shell.Current.GoToAsync("//adminDashboard");
                }
                else if (user.Role == "Worker")
                {
                    await Shell.Current.GoToAsync("//workerDashboard");
                }
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
            }
        }
    }
}