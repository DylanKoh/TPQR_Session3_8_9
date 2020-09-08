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
        Arrival _arrival;
        DateTime endDate = new DateTime(2020, 07, 30);
        double timeSpent = 0;
        public HotelBooking(User user, string hotelName)
        {
            InitializeComponent();
            _user = user;
            _hotelName = hotelName;
        }

        private void HotelBooking_Load(object sender, EventArgs e)
        {
            lblHotelName.Text = _hotelName;
            using (var context = new Session3Entities())
            {
                _arrival = (from x in context.Arrivals
                            where x.userIdFK == _user.userId
                            select x).FirstOrDefault();
                lblCompetitors.Text = _arrival.numberCompetitors.ToString();
                lblDelegates.Text = _arrival.numberDelegate.ToString();
                timeSpent = (endDate - _arrival.arrivalDate).TotalDays;
            }
            LoadData();

        }

        private void LoadData()
        {
            using (var context = new Session3Entities())
            {
                var getHotel = (from x in context.Hotels
                                where x.hotelName == _hotelName
                                select x).FirstOrDefault();
                if (int.Parse(lblCompetitors.Text) % 2 == 0)
                {
                    var singleRow = new List<string>()
                    {
                        "Single", getHotel.singleRate.ToString(),
                        (getHotel.numSingleRoomsTotal - getHotel.numSingleRoomsBooked).ToString(),
                        lblDelegates.Text, (int.Parse(lblDelegates.Text)*timeSpent*getHotel.singleRate).ToString()
                    };
                    var doubleRow = new List<string>()
                    {
                        "Double", getHotel.doubleRate.ToString(),
                        (getHotel.numDoubleRoomsTotal - getHotel.numDoubleRoomsBooked).ToString(),
                        (int.Parse(lblCompetitors.Text)/2).ToString(),
                        ((int.Parse(lblCompetitors.Text)/2)*timeSpent*getHotel.doubleRate).ToString()

                    };
                    dataGridView1.Rows.Add(singleRow.ToArray());
                    dataGridView1.Rows.Add(doubleRow.ToArray());
                }
                else
                {
                    var singleRow = new List<string>()
                    {
                        "Single", getHotel.singleRate.ToString(),
                        (getHotel.numSingleRoomsTotal - getHotel.numSingleRoomsBooked).ToString(),
                        (int.Parse(lblDelegates.Text) + 1).ToString(),
                        ((int.Parse(lblDelegates.Text) + 1)*timeSpent*getHotel.singleRate).ToString()
                    };
                    var doubleRow = new List<string>()
                    {
                        "Double", getHotel.doubleRate.ToString(),
                        (getHotel.numDoubleRoomsTotal - getHotel.numDoubleRoomsBooked).ToString(),
                        (int.Parse(lblCompetitors.Text)/2).ToString(),
                        ((int.Parse(lblCompetitors.Text)/2)*timeSpent*getHotel.doubleRate).ToString()

                    };
                    dataGridView1.Rows.Add(singleRow.ToArray());
                    dataGridView1.Rows.Add(doubleRow.ToArray());
                }
                int total = 0;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    total += Convert.ToInt32(dataGridView1[4, item.Index].Value);
                }
                lblTotal.Text = total.ToString();

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (!int.TryParse(dataGridView1[3, e.RowIndex].Value.ToString(), out _))
                {
                    MessageBox.Show("Please input a valid integer!");
                    if (e.RowIndex == 0)
                    {
                        if (int.Parse(lblCompetitors.Text) % 2 == 0)
                            dataGridView1[3, e.RowIndex].Value = lblDelegates.Text;
                        else
                            dataGridView1[3, e.RowIndex].Value = int.Parse(lblDelegates.Text) + 1;
                    }
                    else
                    {
                        dataGridView1[3, e.RowIndex].Value = int.Parse(lblCompetitors.Text) / 2;
                    }
                }
                else
                {
                    dataGridView1[4, e.RowIndex].Value = Convert.ToInt32(dataGridView1[1, e.RowIndex].Value) * timeSpent * Convert.ToInt32(dataGridView1[3, e.RowIndex].Value);
                    int total = 0;
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        total += Convert.ToInt32(dataGridView1[4, item.Index].Value);
                    }
                    lblTotal.Text = total.ToString();
                }
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            var getTotalCap = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (dataGridView1[0, item.Index].Value.ToString() == "Single")
                {
                    getTotalCap += Convert.ToInt32(dataGridView1[3, item.Index].Value);
                }
                else
                {
                    getTotalCap += Convert.ToInt32(dataGridView1[3, item.Index].Value) * 2;
                }
            }
            if (getTotalCap < int.Parse(lblCompetitors.Text) + int.Parse(lblDelegates.Text))
            {
                MessageBox.Show("Not enough rooms are allocated to all visitors!");
            }
            else
            {
                var boolCheck = true;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(dataGridView1[2, item.Index].Value) < Convert.ToInt32(dataGridView1[3, item.Index].Value))
                    {
                        boolCheck = false;
                    }
                    if (boolCheck == false)
                    {
                        break;
                    }
                }
                if (boolCheck == false)
                {
                    MessageBox.Show("Not enough rooms at hotels to allocate to specified amount!");
                }
                else
                {
                    using (var context = new Session3Entities())
                    {
                        var getHotel = (from x in context.Hotels
                                        where x.hotelName == _hotelName
                                        select x).FirstOrDefault();
                        var newBooking = new Hotel_Booking()
                        {
                            hotelIdFK = getHotel.hotelId,
                            userIdFK = _user.userId,
                            numSingleRoomsRequired = Convert.ToInt32(dataGridView1[3, 0].Value),
                            numDoubleRoomsRequired = Convert.ToInt32(dataGridView1[3, 1].Value)

                        };
                        context.Hotel_Booking.Add(newBooking);
                        context.SaveChanges();

                        getHotel.numSingleRoomsBooked += Convert.ToInt32(dataGridView1[3, 0].Value);
                        getHotel.numDoubleRoomsBooked += Convert.ToInt32(dataGridView1[3, 1].Value);
                        context.SaveChanges();
                        MessageBox.Show("Booking complete!");
                        Close();
                    }
                }
            }
        }
    }
}
