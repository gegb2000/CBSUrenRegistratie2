using CBSUrenRegistratie2.ViewModels;
using CBSUrenRegistratie2.Models;
using CBSUrenRegistratie2.Services;
namespace CBSUrenRegistratie2.Views;

public partial class WorkerDashboardPage : ContentPage
{
	public WorkerDashboardPage()
	{
		InitializeComponent();
        BindingContext = new WorkerDashboardViewModel();
    }

    
}