using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPQR_Session3_8_9
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new LoginForm()).ShowDialog)();
            Close();
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            using (var context = new Session3Entities())
            {
                var getCountries = (from x in context.Users
                                    where x.userTypeIdFK == 2
                                    select x.countryName);
                foreach (var item in getCountries)
                {
                    if (cbCountry.Items.Contains(item))
                    {
                        cbCountry.Items.Remove(item);
                    }
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cbCountry.SelectedItem == null || string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtRePassword.Text))
            {
                MessageBox.Show("Please ensure all fields are filled!");
            }
            else if (txtUserID.TextLength < 8)
            {
                MessageBox.Show("Please make sure User ID is at least 8 characters long!");
            }
            else if (txtPassword.Text != txtRePassword.Text)
            {
                MessageBox.Show("Passwords do not match!");
            }
            else
            {
                using (var context = new Session3Entities())
                {
                    var getUser = (from x in context.Users
                                   where x.userId == txtUserID.Text
                                   select x).FirstOrDefault();
                    if (getUser != null)
                    {
                        MessageBox.Show("Please use another User ID!");
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            countryName = cbCountry.SelectedItem.ToString(),
                            passwd = txtPassword.Text,
                            userId = txtUserID.Text,
                            userTypeIdFK = 2
                        };
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        MessageBox.Show("User added!");
                        Hide();
                        (new LoginForm()).ShowDialog();
                        Close();
                    }
                }
            }
        }
    }
}
