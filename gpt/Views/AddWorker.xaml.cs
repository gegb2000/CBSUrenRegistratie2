using CBSUrenRegistratie2.ViewModels;

namespace CBSUrenRegistratie2.Views;

public partial class AddWorker : ContentPage
{
    public AddWorker()
    {
        InitializeComponent();
        BindingContext = new WorkerViewModel();
    }
}