using CBSUrenRegistratie2.Services;
using CBSUrenRegistratie2.Views;

namespace CBSUrenRegistratie2
{
    public partial class App : Application
    {
        public static int CurrentUserId { get; set; }
        public App()
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            MainPage = new AppShell();
        }
    }
}
