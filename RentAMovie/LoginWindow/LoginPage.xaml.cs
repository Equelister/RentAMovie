using MongoDB.Bson;
using MongoDB.Driver;
using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentAMovie.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        //OracleSQLConnectorLoginWindow oracleSQLConnectorLoginWindow = new OracleSQLConnectorLoginWindow();

        public LoginPage()
        {
            InitializeComponent();
        }

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private async Task<List<BsonDocument>> GetAll(IMongoCollection<BsonDocument> collection)
        {
           //await collection.Find(new BsonDocument()).ForEachAsync(d => Console.WriteLine(d));

            var xd = await collection.Find(new BsonDocument()).ToListAsync();


            foreach (var doc in xd)
            {
                Console.WriteLine(doc.ToJson());
            }

            return await collection.Find(new BsonDocument()).ToListAsync(); ;
        }

        private async Task<UserModel> FindUserByLogin(IMongoCollection<UserModel> usersColl, BsonString login)
        {
            var xd = await usersColl.Find(new BsonDocument()).ToListAsync();
            return await usersColl.Find(x => x.Login.Equals(login)).SingleAsync();
        }


        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            AllocConsole();
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("rentamovie");
            var usersColl = database.GetCollection<UserModel>("clients");

            //var result = collection.Find(x => x.login == "system")
            var userLoginInput = loginTextBox.Text;
            var userPaswordInput = passwordPasswordBox.Password.ToString();
            var login = new BsonString(userLoginInput);

            UserModel user = new UserModel();

            try
            {
                user = await FindUserByLogin(usersColl, login);
            }catch(Exception eeee)
            {
                errorLabel.Content = "Wrong credentials!";
                errorLabel.Visibility = Visibility.Visible;
                return;
            }
            if(user.Password.Equals(userPaswordInput))
            {
                App.Current.MainWindow.Hide();
                MainWindow.MainWindow mainWindow = new MainWindow.MainWindow();
                mainWindow.Show();
                App.Current.MainWindow.Close();
            }





            //var documents = await GetAll(usersColl);

 





            //var documents = await SpeCollection.Find(Builders<Project>.Filter.Empty).ToListAsync();
















            



            /*PersonModel person = oracleSQLConnectorLoginWindow.getAllFromDataBaseForEmail("user_password_table", emailTextBox.Text.Trim(), out int personTableID);
            if (personTableID >= 0)
            {
                if (passwordPasswordBox.Password.Equals(person.getPassword()) && emailTextBox.Text.Equals(person.getEmailAddress()))
                {
                    GlobalClass.setUserID(personTableID);
                    App.Current.MainWindow.Hide();
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    App.Current.MainWindow.Close();
                }
            }
            else
            {
                person = oracleSQLConnectorLoginWindow.getAllFromDataBaseForEmail("trainer_password_table", emailTextBox.Text.Trim(), out personTableID);
                if (personTableID >= 0)
                {
                    if (passwordPasswordBox.Password.Equals(person.getPassword()) && emailTextBox.Text.Equals(person.getEmailAddress()))
                    {
                        GlobalClass.setTrainerID(personTableID);
                        App.Current.MainWindow.Hide();
                        TrainerWindow trainerWindow = new TrainerWindow();
                        trainerWindow.Show();
                        App.Current.MainWindow.Close();
                    }
                }
                else
                {
                    errorLabel.Visibility = Visibility.Visible;
                }
            }*/
        }
    }
}
