using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CBSUrenRegistratie2.Models;
using CBSUrenRegistratie2.Services;
using System.Collections.ObjectModel;

namespace CBSUrenRegistratie2.ViewModels
{
    public class WorkerViewModel : BindableObject
    {
        private string name;
        private string email;
        private decimal hourlyRate;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public decimal HourlyRate
        {
            get => hourlyRate;
            set
            {
                hourlyRate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Worker> Workers { get; set; } = new ObservableCollection<Worker>();
        public ICommand AddWorkerCommand { get; }
        public ICommand LoadWorkersCommand { get; }
        public WorkerViewModel()
        {
            LoadWorkersCommand = new Command(async () => await LoadWorkers());
            AddWorkerCommand = new Command(AddWorker);
        }
        private async Task LoadWorkers()
        {
            var dbService = new DatabaseService();
            var workers = await dbService.GetAllWorkers();
            Workers.Clear();
            foreach (var worker in workers)
            {
                Workers.Add(worker);
            }
        }
        private void AddWorker()
        {
            Worker newWorker = new Worker
            {
                Name = this.Name,
                Email = this.Email,
                HourlyRate = this.HourlyRate
            };

            var dbService = new DatabaseService();
            dbService.AddWorker(newWorker).ConfigureAwait(false);
        }

    }
}
