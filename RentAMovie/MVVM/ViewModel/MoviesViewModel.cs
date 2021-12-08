using MongoDB.Bson;
using RentAMovie.Core;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RentAMovie.MVVM.ViewModel
{
    class MoviesViewModel : ObservableObject
    {
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
                FieldsEnabled = false;
                if (_selectedMovie is not null)
                {
                    FieldsEnabled = true;
                    ActorList = new ObservableCollection<string>(_selectedMovie.Actors);
                }
                onPropertyChanged();
            }
        }

        private List<MovieModel> movieListTemp;
        private string _movieFilter = "";
        public string MovieFilter
        {
            get
            {
                
                return _movieFilter;
            }
            set
            {
                _movieFilter = value;
                onPropertyChanged();
                if (_movieFilter.Equals(""))
                {
                    MovieList = new ObservableCollection<MovieModel>(movieListTemp);
                    onPropertyChanged("MovieList");
                }
                else
                {
                    MovieList = new ObservableCollection<MovieModel>(movieListTemp);
                    MovieList = DoAFilter();
                    ActorList.Clear();
                    onPropertyChanged("MovieList");
                    onPropertyChanged("ActorList");
                }
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
                onPropertyChanged();
            }
        }
        private string _selectedActor = "";
        public string SelectedActor
        {
            get
            {
                return _selectedActor;
            }
            set
            {
                _selectedActor = value;
                onPropertyChanged();
            }
        }

        private ObservableCollection<string> _actorList;
        public ObservableCollection<string> ActorList
        {
            get
            {
                if(_actorList is null)
                {
                    _actorList = new ObservableCollection<string>();
                }
                return _actorList;
            }
            set
            {
                _actorList = value;
                onPropertyChanged();
            }
        }

        private bool _fieldsEnabled = true;
        public bool FieldsEnabled
        {
            get
            {
                return _fieldsEnabled;
            }
            set
            {
                if (_user.IsAdmin)
                {
                    _fieldsEnabled = value;
                }else
                {
                    _fieldsEnabled = false;
                }
                onPropertyChanged();
            }
        }

        public RelayCommand UpdateMovieCommand { get; set; }
        public RelayCommand DeleteMovieCommand { get; set; }
        public RelayCommand CreateMovieCommand { get; set; }


        public RelayCommand DeleteActorCommand { get; set; }
        public RelayCommand CreateActorCommand { get; set; }

        private UserModel _user;
        public MoviesViewModel(UserModel user)
        {
            _user = user;
            GetAllMoviesFromDBAsync();
            if (_user.IsAdmin)
            {
                FieldsEnabled = true;
            }
            else
            {
                FieldsEnabled = false;
            }

            UpdateMovieCommand = new RelayCommand(o =>
            {
                if (_user.IsAdmin)
                {
                    UpdateExistingMovieAsync();
                }
            });

            DeleteMovieCommand = new RelayCommand(o =>
            {
                if (_user.IsAdmin)
                {
                    DeleteExistingMovieAsync();
                }
            });

            CreateMovieCommand = new RelayCommand(o =>
            {
                if (_user.IsAdmin)
                {
                    CreateNewMovieAsync();
                }
            });

            DeleteActorCommand = new RelayCommand(o =>
            {
                if (_user.IsAdmin)
                {
                    DeleteExistingActorAsync();
                }
            });

            CreateActorCommand = new RelayCommand(o =>
            {
                if (_user.IsAdmin)
                {
                    CreateNewActorAsync();
                }
            });
        }

        private void CreateNewActorAsync()
        {
            ActorList.Add(SelectedActor);
            SelectedMovie.Actors = new List<string>(ActorList);
        }

        private void DeleteExistingActorAsync()
        {
            ActorList.Remove(SelectedActor);
            SelectedMovie.Actors = new List<string>(ActorList);
        }

        private async Task UpdateExistingMovieAsync()
        {
            if (SelectedMovie is not null)
            {
                ObjectId a = new ObjectId();
                if (SelectedMovie.ID.Equals(a) == false)
                {
                    bool result = await Models.MongoDB.MovieQueries.UpdateMovie(SelectedMovie);
                    if (result)
                    {
                        SelectedMovie = null;
                        _fieldsEnabled = false;
                        GetAllMoviesFromDBAsync();
                    }
                }
            }
        }

        private async Task DeleteExistingMovieAsync()
        {
            if (SelectedMovie is not null)
            {
                ObjectId a = new ObjectId();
                if (SelectedMovie.ID.Equals(a) == false)
                {
                    if (SelectedMovie.IsRented == false)
                    {
                        long result = await Models.MongoDB.MovieQueries.DeleteMovie(SelectedMovie);
                        if (result == 1)
                        {
                            SelectedMovie = null;
                            _fieldsEnabled = false;
                            GetAllMoviesFromDBAsync();
                        }
                    }
                }
            }
        }

        private async Task CreateNewMovieAsync()
        {
            if (SelectedMovie is not null)
            {
                if (String.IsNullOrWhiteSpace(SelectedMovie.Title) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.Summary) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.Rate.ToString()) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.Genre) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.Duration.ToString()) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.Director) ||
                    String.IsNullOrWhiteSpace(SelectedMovie.YearOfRelease.ToString()))
                {
                    return;
                }
                if(SelectedMovie.Actors is null)
                {
                    return;
                }
                if (SelectedMovie.Actors.Count < 1)
                {
                    return;
                }

                MovieModel newMovie = new MovieModel();
                if (SelectedMovie is not null)
                {
                    newMovie.ID = ObjectId.GenerateNewId();
                    newMovie.Title = SelectedMovie.Title;
                    newMovie.Summary = SelectedMovie.Summary;
                    newMovie.Rate = SelectedMovie.Rate;
                    newMovie.IsRented = false;
                    newMovie.InsertDate = DateTime.Now;
                    newMovie.Genre = SelectedMovie.Genre;
                    newMovie.Duration = SelectedMovie.Duration;
                    newMovie.Director = SelectedMovie.Director;
                    newMovie.Actors = SelectedMovie.Actors;
                    newMovie.YearOfRelease = SelectedMovie.YearOfRelease;
                    await Models.MongoDB.MovieQueries.InsertMovie(newMovie);
                    SelectedMovie = null;
                    _fieldsEnabled = false;
                    GetAllMoviesFromDBAsync();
                }
                else
                {
                    return;
                }
            }
        }

        private async Task GetAllMoviesFromDBAsync()
        {
            MovieList = new ObservableCollection<MovieModel>(await Models.MongoDB.MovieQueries.FindAllMovies());
            movieListTemp = new List<MovieModel>(MovieList);
        }

        private ObservableCollection<MovieModel> DoAFilter()
        {
            ObservableCollection<MovieModel> filteredList = new ObservableCollection<MovieModel>();
            foreach (var elem in MovieList)
            {
                if (elem.Title.ToLower().Contains(MovieFilter.ToLower()) ||
                    elem.Genre.ToLower().Contains(MovieFilter.ToLower()) ||
                    elem.Director.ToLower().Contains(MovieFilter.ToLower()))
                {
                    filteredList.Add(elem);
                }
            }
            return filteredList;
        }
    }
}
