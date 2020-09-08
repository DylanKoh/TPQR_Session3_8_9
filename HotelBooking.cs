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
    public partial class HotelBooking : Form
    {
        User _user;
        string _hotelName;
        public HotelBooking(User user, string hotelName)
        {
            InitializeComponent();
            _user = user;
            _hotelName = hotelName;
        }

        private void HotelBooking_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
