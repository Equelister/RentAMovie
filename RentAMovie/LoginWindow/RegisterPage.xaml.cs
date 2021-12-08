using RentAMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
            //OracleSQLConnectorLoginWindow oracleSQLConnectorLoginWindow = new OracleSQLConnectorLoginWindow();
        public RegisterPage()
        {
            InitializeComponent();
        }



        /// <summary>
        /// TO DO REGISTER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void registerButton_ClickAsync(object sender, RoutedEventArgs e)
        {
                
            String firstNameValue = firstNameTextBox.Text.Trim();
            String lastNameValue = lastNameTextBox.Text.Trim();
            String addressValue = addressTextBox.Text.Trim();
            String phoneNumberStringValue = phoneNumberTextBox.Text.Trim();
            String loginStringValue = loginTextBox.Text.Trim();
            String passwordValue = passwordPasswordBox.Password.Trim();
            String retypedPasswordValue = retypePasswordPasswordBox.Password.Trim();

            if (String.IsNullOrWhiteSpace(firstNameValue) == false &&
                String.IsNullOrWhiteSpace(lastNameValue) == false &&
                String.IsNullOrWhiteSpace(addressValue) == false &&
                String.IsNullOrWhiteSpace(phoneNumberStringValue) == false &&
                String.IsNullOrWhiteSpace(loginStringValue) == false &&
                String.IsNullOrWhiteSpace(passwordValue) == false &&
                String.IsNullOrWhiteSpace(retypedPasswordValue) == false
                )
            {
                if (passwordValue.Equals(retypedPasswordValue))
                {
                    UserModel user = new UserModel(
                        firstNameValue,
                        lastNameValue,
                        addressValue,
                        phoneNumberStringValue,
                        loginStringValue,
                        passwordValue
                        );

                    Models.MongoDB.UserQueries.InsertUser(user);

                    firstNameTextBox.Text = "";
                    lastNameTextBox.Text = "";
                    addressTextBox.Text = "";
                    phoneNumberTextBox.Text = "";
                    loginTextBox.Text = "";
                    passwordPasswordBox.Password = "";
                    retypePasswordPasswordBox.Password = "";

                    failedRegisterTextBlock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    failedRegisterTextBlock.Visibility = Visibility.Visible;
                    successRegisterTextBlock.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                failedRegisterTextBlock.Visibility = Visibility.Visible;
                successRegisterTextBlock.Visibility = Visibility.Collapsed;
            }









            /* RegisterPageValidator validator = new RegisterPageValidator();

            

            if (validator.IsInputedDataValid(firstNameValue, lastNameValue, emailValue, phoneNumberStringValue, passwordValue, retypedPasswordValue))
            {
                if (userRadioButton.IsChecked == true)
                {
                    UserModel u = new UserModel(firstNameValue, lastNameValue, emailValue, phoneNumberStringValue);
                    bool isEmailUnique = isUniqueValue("email", "user_password_table", emailValue);

                    if (isEmailUnique)
                    {
                        insertUserToDataBase(u, passwordPasswordBox.Password, "user_table");
                        failedRegisterTextBlock.Visibility = Visibility.Collapsed;
                        successRegisterTextBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        failedRegisterTextBlock.Visibility = Visibility.Visible;
                        successRegisterTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    TrainerModel t = new TrainerModel(firstNameValue, lastNameValue, emailValue, phoneNumberStringValue);
                    bool isEmailUnique = isUniqueValue("email", "trainer_password_table", emailValue);

                    if (isEmailUnique)
                    {
                        insertUserToDataBase(t, passwordPasswordBox.Password, "trainer_table");
                        failedRegisterTextBlock.Visibility = Visibility.Collapsed;
                        successRegisterTextBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        failedRegisterTextBlock.Visibility = Visibility.Visible;
                        successRegisterTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                failedRegisterTextBlock.Visibility = Visibility.Visible;
                successRegisterTextBlock.Visibility = Visibility.Collapsed;
            }*/
        }

        private bool isUniqueValue(String columnName, String tableName, String searchedValue)
        {
            /*bool success = false;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleSQLConnectorLoginWindow.GetConnectionString()))
                {
                    connection.Open();
                    OracleCommand cmd;

                    string sql = String.Format("select {0} from {1} WHERE {0} LIKE '{2}'", columnName, tableName, searchedValue);

                    cmd = new OracleCommand(sql, connection);
                    cmd.CommandType = CommandType.Text;

                    OracleDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.Read())
                        {
                            success = false;
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            catch
            {
            }
            return success;*/
            return true;
        }


        private void insertUserToDataBase(/*PersonModel*/ int p, String password, String mainTableName)
        {
            /* try
            {
                using (OracleConnection connection = new OracleConnection(OracleSQLConnectorLoginWindow.GetConnectionString()))
                {
                    connection.Open();
                    OracleCommand cmd;

                    string sql = String.Format("INSERT INTO {4}(first_name, last_name, email, phone_number) VALUES('{0}', '{1}', '{2}', '{3}')", p.getFirstName(), p.getLastName(), p.getEmailAddress(), p.getPhoneNumberStr(), mainTableName);

                    cmd = new OracleCommand(sql, connection);
                    cmd.CommandType = CommandType.Text;

                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated == 0)
                    {
                        //label.Content = "Record not inserted";
                    }
                    else
                    {
                        String secondaryTableName;
                        if (mainTableName.Equals("user_table"))
                        {
                            secondaryTableName = "user_password_table";
                        }
                        else
                        {
                            secondaryTableName = "trainer_password_table";
                        }
                        sql = String.Format("INSERT INTO {3}(password, {4}_id, email) VALUES('{0}', {1}, '{2}')", password, oracleSQLConnectorLoginWindow.getIDFromDataBase(mainTableName, p.getEmailAddress()), p.getEmailAddress(), secondaryTableName, mainTableName);

                        cmd = new OracleCommand(sql, connection);
                        cmd.CommandType = CommandType.Text;
                        rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated == 0)
                        {
                            // error drop previous insert
                        }
                    }
                }
            }
            catch
            {

            }*/
        }
    }
}
