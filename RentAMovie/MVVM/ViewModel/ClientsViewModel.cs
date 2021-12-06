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
    class ClientsViewModel : ObservableObject
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
                SelectedUserTemp = _selectedUser;
                _selectedUser = value;
                onPropertyChanged();
            }
        }

        private UserModel _selectedUserTemp;
        public UserModel SelectedUserTemp
        {
            get
            {
                if (_selectedUserTemp is null)
                {
                    _selectedUserTemp = new UserModel();
                }
                
                return _selectedUserTemp;
            }
            set
            {
                _selectedUserTemp = value;
                onPropertyChanged();
            }
        }

        private string _inputedPassword = "";
        public string InputedPassword
        {
            get
            {
                return _inputedPassword;
            }
            set
            {
                _inputedPassword = value;
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
                onPropertyChanged();
            }
        }



        public RelayCommand UpdateUserCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }
        public RelayCommand CreateUserCommand { get; set; }


        public ClientsViewModel()
        {
            GetAllUsersFromDBAsync();

            UpdateUserCommand = new RelayCommand(o =>
            {
                UpdateExistingUserAsync();
            });

            DeleteUserCommand = new RelayCommand(o =>
            {
                DeleteExistingUserAsync();
            });

            CreateUserCommand = new RelayCommand(o =>
            {
                CreateNewUserAsync();
            });
        }

        private async Task UpdateExistingUserAsync()
        {
            if(SelectedUser is not null)
            {
                ObjectId a = new ObjectId();
                if (SelectedUser.ID.Equals(a) == false)
                {
                    if(InputedPassword.Equals("") == false)
                    {
                        SelectedUser.Password = InputedPassword.ToString();
                    }                    
                    bool result = await Models.MongoDB.UserQueries.UpdateUser(SelectedUser);
                    if(result)
                    {
                        InputedPassword = "";
                        GetAllUsersFromDBAsync();
                    }                    
                }
            }
        }

        private async Task DeleteExistingUserAsync()
        {
            if (SelectedUser is not null)
            {
                ObjectId a = new ObjectId();
                if (SelectedUser.ID.Equals(a) == false)
                {
                    if (SelectedUser.RentalsCount <= 0)
                    {
                        long result = await Models.MongoDB.UserQueries.DeleteUser(SelectedUser);
                        if (result == 1)
                        {
                            //UserList.Remove(SelectedUser);
                            SelectedUser = null;
                            GetAllUsersFromDBAsync();
                        }
                    }
                }
            }
        }

        private async Task CreateNewUserAsync()
        {
            if (SelectedUser is not null)
            {
                if (String.IsNullOrWhiteSpace(SelectedUser.Login) ||
                    String.IsNullOrWhiteSpace(InputedPassword) ||
                    String.IsNullOrWhiteSpace(SelectedUser.FirstName) ||
                    String.IsNullOrWhiteSpace(SelectedUser.LastName))
                {
                    return;
                }
                UserModel newUser = new UserModel();
                if (SelectedUser is not null)
                {
                    newUser.ID = ObjectId.GenerateNewId();
                    newUser.FirstName = SelectedUser.FirstName;
                    newUser.LastName = SelectedUser.LastName;
                    newUser.Address = SelectedUser.Address;
                    newUser.Phone = SelectedUser.Phone;
                    newUser.RegisterDate = DateTime.Now;
                    newUser.Login = SelectedUser.Login;
                    newUser.Password = InputedPassword;
                    newUser.IsAdmin = SelectedUser.IsAdmin;
                    newUser.RentalsCount = 0;
                    await Models.MongoDB.UserQueries.InsertUser(newUser);
                    //UserList.Add(newUser);
                    GetAllUsersFromDBAsync();
                }
                else
                {
                    return;
                }
            }
        }

        private async Task GetAllUsersFromDBAsync()
        {
            UserList = new ObservableCollection<UserModel>(await Models.MongoDB.UserQueries.FindAllUsers());
        }
    }
}
