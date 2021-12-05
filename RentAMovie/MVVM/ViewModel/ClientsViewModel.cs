using RentAMovie.Core;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
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

        public ClientsViewModel()
        {
            GetAllUsersFromDBAsync();
        }

        private async Task GetAllUsersFromDBAsync()
        {
            UserList = await Models.MongoDB.UserQueries.FindAllUsers();
        }
    }
}
