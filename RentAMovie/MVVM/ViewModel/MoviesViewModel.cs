using MongoDB.Bson;
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
                _fieldsEnabled = value;
                onPropertyChanged();
            }
        }

        public RelayCommand UpdateMovieCommand { get; set; }
        public RelayCommand DeleteMovieCommand { get; set; }
        public RelayCommand CreateMovieCommand { get; set; }


        public RelayCommand DeleteActorCommand { get; set; }
        public RelayCommand CreateActorCommand { get; set; }


        public MoviesViewModel()
        {
            GetAllMoviesFromDBAsync();

            UpdateMovieCommand = new RelayCommand(o =>
            {
                UpdateExistingMovieAsync();
            });

            DeleteMovieCommand = new RelayCommand(o =>
            {
                DeleteExistingMovieAsync();
            });

            CreateMovieCommand = new RelayCommand(o =>
            {
                CreateNewMovieAsync();
            });

            DeleteActorCommand = new RelayCommand(o =>
            {
                DeleteExistingActorAsync();
            });

            CreateActorCommand = new RelayCommand(o =>
            {
                CreateNewActorAsync();
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
                    //MovieList.Add(newMovie);
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
        }
    }
}
