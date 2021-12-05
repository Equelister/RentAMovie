using RentAMovie.Core;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<UserModel> _userList;
        public ObservableCollection<UserModel> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                onPropertyChanged("UserList");
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

        private ObservableCollection<MovieModel> _movieList;
        public ObservableCollection<MovieModel> MovieList
        {
            get
            {
                return _movieList;
            }
            set
            {
                _movieList = value;
                onPropertyChanged("MovieList");
            }
        }

        private ObservableCollection<RentalModel> _rentalList;
        public ObservableCollection<RentalModel> RentalList
        {
            get
            {
                return _rentalList;
            }
            set
            {
                _rentalList = value;
                onPropertyChanged("RentalList");
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

        public RelayCommand CreateNewRentalCommand { get; set; }

        private UserModel user;
        public HomeViewModel(UserModel user)
        {
            this.user = user;
            GetDataFromDBAsync();

            CreateNewRentalCommand = new RelayCommand(o =>
            {
                CreateNewRentalAsync();
            });
        }

        private async Task GetDataFromDBAsync()
        {
            await GetAllUsersFromDBAsync();
            await GetAllMoviesFromDBAsync();
            await GetAllRentalsFromDBAsync();
        }

        private async Task GetAllUsersFromDBAsync()
        {
            UserList = new ObservableCollection<UserModel>(await Models.MongoDB.UserQueries.FindAllUsers());
        }
        
        private async Task GetAllRentalsFromDBAsync()
        {
            RentalList = new ObservableCollection<RentalModel>(await Models.MongoDB.RentalQueries.FindAllRentals());
        }

        private async Task GetAllMoviesFromDBAsync()
        {
            MovieList = new ObservableCollection<MovieModel>(await Models.MongoDB.MovieQueries.FindAllMovies());
        }

        private async Task CreateNewRentalAsync()
        {
            if(SelectedMovie is not null && SelectedUser is not null)
            {
                if (await Models.MongoDB.UserQueries.CheckUserRentalLimits(SelectedUser.ID) <= 5)
                {
                    if (await Models.MongoDB.MovieQueries.CheckIsMovieRentedChangeIfNot(SelectedMovie.ID) == false)
                    {
                        await Models.MongoDB.UserQueries.IncrementUserRentalLimitsByOne(SelectedUser.ID);
                        var dateNow = DateTime.Now;
                        RentalModel newRental = new RentalModel(SelectedUser.ID, SelectedMovie.ID);
                        await Models.MongoDB.RentalQueries.InsertNewRentalToDB(newRental);
                        RentalList.Add(newRental);

                        var selectedUserTemp = SelectedUser;
                        var selectedMovieTemp = SelectedMovie;

                        selectedUserTemp.RentalsCount++;
                        selectedMovieTemp.IsRented = true;

                        UserList.Remove(SelectedUser);
                        MovieList.Remove(SelectedMovie);

                        SelectedUser = null;
                        SelectedMovie = null;

                        UserList.Add(selectedUserTemp);
                        MovieList.Add(selectedMovieTemp);

                        SelectedUser = selectedUserTemp;
                        SelectedMovie = selectedMovieTemp;

                       /* UserList.Where(x => x.ID.Equals(SelectedUser.ID)).FirstOrDefault().RentalsCount++;
                        onPropertyChanged("UserList");
                        MovieList.Where(x => x.ID.Equals(SelectedMovie.ID)).FirstOrDefault().IsRented = true;
                        onPropertyChanged("MovieList");*/
                        
                    }
                }
            }
        }
    }
}
