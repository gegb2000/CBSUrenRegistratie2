using CBSUrenRegistratie2.ViewModels;
using CBSUrenRegistratie2.Models;
namespace CBSUrenRegistratie2.Views;

public partial class AdminDashboardPage : ContentPage
{
	public AdminDashboardPage()
	{
		InitializeComponent();
        BindingContext = new AdminDashboardViewModel();
    }
    private void ToggleDrawer(object sender, EventArgs e)
    {
        // Toggle the drawer's visibility
        navigationDrawer.IsOpen = !navigationDrawer.IsOpen;
    }
    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        // Assuming there's a method in the ViewModel to handle the drawer toggle
        if (BindingContext is AdminDashboardViewModel viewModel)
        {
            viewModel.OpenDrawerCommand.Execute(null);
        }
    }
}