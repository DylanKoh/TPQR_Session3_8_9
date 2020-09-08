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
    public partial class HotelSelection : Form
    {
        User _user;
        public HotelSelection(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void btnQueens_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Hotel Royal Queens")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnGrand_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Hotel Grand Pacific")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnInter_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Intercontinental Singapore")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnCarlton_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Charlton Hotel")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Pan Pacific Hotel")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnRitz_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Ritz-Carlton")).ShowDialog();
            using (var context = new Session3Entities())
            {
                var getBooking = (from x in context.Hotel_Booking
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getBooking != null)
                {
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }
    }
}
