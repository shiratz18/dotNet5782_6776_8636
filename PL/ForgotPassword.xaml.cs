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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
            ques.Text = BO.Configuration.question;
        }

        private void forgotDone_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if ((password.Password == BO.Configuration.answer))
                {
                    new MainWindow().ShowDialog();
                    this.Close();

                }
                else
                {

                    ERROR error = new ERROR();
                    error.errorName.Content = "The answer is incorrect";
                    error.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                ERROR error = new ERROR();
                error.errorName.Content = ex.Message;
                error.ShowDialog();
            }
        }

        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void enter_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && managerDone.Visibility == Visibility.Visible)
            {
                try
                {
                    if ((password.Password == BO.Configuration.answer))
                    {
                        new MainWindow().ShowDialog();
                        this.Close();

                    }
                    else
                    {

                        ERROR error = new ERROR();
                        error.errorName.Content = "The answer is incorrect";
                        error.ShowDialog();

                    }
                }
                catch (Exception ex)
                {
                    ERROR error = new ERROR();
                    error.errorName.Content = ex.Message;
                    error.ShowDialog();
                }
            }
        }
    }
}
