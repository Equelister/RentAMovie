using RentAMovie.Core;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                onPropertyChanged();
            }
        }

        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                onPropertyChanged();
            }
        }

        private bool _menuRadios = true;
        public bool MenuRadios
        {
            get { return _menuRadios; }
            set
            {
                _menuRadios = value;
                onPropertyChanged();
            }
        }

        public HomeViewModel HomeViewModel { get; set; }
        public ClientsViewModel ClientsViewModel { get; set; }
        public MoviesViewModel MoviesViewModel { get; set; }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ClientsViewCommand { get; set; }
        public RelayCommand MoviesViewCommand { get; set; }

        public MainViewModel()
        {
            InitializeViewCommands();
        }

        public MainViewModel(UserModel user)
        {
            InitializeViewCommands();
            _user = user;
        }

        private void InitializeViewCommands()
        {
            HomeViewModel = new HomeViewModel(_user);
            ClientsViewModel = new ClientsViewModel();
            MoviesViewModel = new MoviesViewModel();

            CurrentView = HomeViewModel;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeViewModel;
            });

            ClientsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClientsViewModel;
            });

            MoviesViewCommand = new RelayCommand(o =>
            {
                CurrentView = MoviesViewModel;
            });
        }


    }
}
