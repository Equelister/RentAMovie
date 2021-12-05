using RentAMovie.Core;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get
            {
                if (_selectedUser is null)
                {
                    _selectedUser = new UserModel();
                }
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                onPropertyChanged();
            }
        }

        private List<UserModel> _userList;
        public List<UserModel> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                onPropertyChanged();
            }
        }

        private MovieModel _selectedMovie;
        public MovieModel SelectedMovie
        {
            get
            {
                if (_selectedMovie is null)
                {
                    _selectedMovie = new MovieModel();
                }
                return _selectedMovie;
            }
            set
            {
                _selectedMovie = value;
                onPropertyChanged();
            }
        }

        private List<MovieModel> _movieList;
        public List<MovieModel> MovieList
        {
            get
            {
                return _movieList;
            }
            set
            {
                _movieList = value;
                onPropertyChanged();
            }
        }

        private List<RentalModel> _rentalList;
        public List<RentalModel> RentalList
        {
            get
            {
                return _rentalList;
            }
            set
            {
                _rentalList = value;
                onPropertyChanged();
            }
        }

        private RentalModel _selectedRental;
        public RentalModel SelectedRental
        {
            get
            {
                if (_selectedRental is null)
                {
                    _selectedRental = new RentalModel();
                }
                return _selectedRental;
            }
            set
            {
                _selectedRental = value;
                onPropertyChanged();
            }
        }

        private UserModel user;
        public HomeViewModel(UserModel user)
        {
            this.user = user;
            GetDataFromDBAsync();
        }

        private async Task GetDataFromDBAsync()
        {
            await GetAllUsersFromDBAsync();
            await GetAllMoviesFromDBAsync();
            await GetAllRentalsFromDBAsync();
        }

        private async Task GetAllUsersFromDBAsync()
        {
            UserList = await Models.MongoDB.UserQueries.FindAllUsers();
        }
        
        private async Task GetAllRentalsFromDBAsync()
        {
            RentalList = await Models.MongoDB.RentalQueries.FindAllRentals();
        }

        private async Task GetAllMoviesFromDBAsync()
        {
            MovieList = await Models.MongoDB.MovieQueries.FindAllMovies();
        }
    }
}
