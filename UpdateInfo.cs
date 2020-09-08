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
    public partial class UpdateInfo : Form
    {
        User _user;
        Arrival _arrival;
        DateTime endDate = new DateTime(2020, 07, 30);
        double timeSpent = 0;
        Hotel_Booking _booking;
        public UpdateInfo(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new CountryMain(_user)).ShowDialog();
            Close();
        }

        private void UpdateInfo_Load(object sender, EventArgs e)
        {
            using (var context = new Session3Entities())
            {
                _arrival = (from x in context.Arrivals
                            where x.userIdFK == _user.userId
                            select x).FirstOrDefault();
                nudCompetitors.Value = _arrival.numberCompetitors;
                nudDelegates.Value = _arrival.numberDelegate;
                nudHead.Value = _arrival.numberHead;
                timeSpent = (endDate - _arrival.arrivalDate).TotalDays;
                _booking = (from x in context.Hotel_Booking
                            where x.userIdFK == _user.userId
                            select x).FirstOrDefault();
            }
            LoadData();
        }
        private void LoadData()
        {
            using (var context = new Session3Entities())
            {
                var getHotel = (from x in context.Hotels
                                where x.hotelId == _booking.hotelIdFK
                                select x).FirstOrDefault();
                var singleRow = new List<string>()
                {
                        "Single", getHotel.singleRate.ToString(),
                        (getHotel.numSingleRoomsTotal - getHotel.numSingleRoomsBooked).ToString(),
                        _booking.numSingleRoomsRequired.ToString(), "",
                    (_booking.numSingleRoomsRequired*timeSpent*getHotel.singleRate).ToString()
                };
                var doubleRow = new List<string>()
                    {
                        "Double", getHotel.doubleRate.ToString(),
                        (getHotel.numDoubleRoomsTotal - getHotel.numDoubleRoomsBooked).ToString(),
                        _booking.numDoubleRoomsRequired.ToString(), "",
                        (_booking.numDoubleRoomsRequired*timeSpent*getHotel.doubleRate).ToString()

                    };
                dataGridView1.Rows.Add(singleRow.ToArray());
                dataGridView1.Rows.Add(doubleRow.ToArray());


                int total = 0;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    total += Convert.ToInt32(dataGridView1[5, item.Index].Value);
                }
                lblTotal.Text = total.ToString();

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if (!int.TryParse(dataGridView1[4, e.RowIndex].Value.ToString(), out _))
                {
                    MessageBox.Show("Please input a valid integer!");
                    dataGridView1[4, e.RowIndex].Value = "";
                }
                else
                {
                    dataGridView1[5, e.RowIndex].Value = Convert.ToInt32(dataGridView1[1, e.RowIndex].Value) * timeSpent * Convert.ToInt32(dataGridView1[4, e.RowIndex].Value);
                    int total = 0;
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        total += Convert.ToInt32(dataGridView1[5, item.Index].Value);
                    }
                    lblTotal.Text = total.ToString();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var getTotalCap = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (dataGridView1[0, item.Index].Value.ToString() == "Single")
                {
                    if (dataGridView1[4, item.Index].Value.ToString() != "")
                        getTotalCap += Convert.ToInt32(dataGridView1[4, item.Index].Value);
                    else
                        getTotalCap += Convert.ToInt32(dataGridView1[3, item.Index].Value);
                }
                else
                {
                    if (dataGridView1[4, item.Index].Value.ToString() != "")
                        getTotalCap += Convert.ToInt32(dataGridView1[4, item.Index].Value) * 2;
                    else
                        getTotalCap += Convert.ToInt32(dataGridView1[3, item.Index].Value) * 2;
                }
            }
            if (getTotalCap < Convert.ToInt32(nudCompetitors.Value + nudDelegates.Value))
            {
                MessageBox.Show("Not enough rooms are allocated to all visitors!");
            }
            else
            {
                var boolCheck = true;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (dataGridView1[4, item.Index].Value.ToString() != "")
                    {
                        if (Convert.ToInt32(dataGridView1[2, item.Index].Value) + Convert.ToInt32(dataGridView1[3, item.Index].Value) - Convert.ToInt32(dataGridView1[4, item.Index].Value) < 0)
                        {
                            boolCheck = false;
                        }
                        if (boolCheck == false)
                        {
                            break;
                        }
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
                                        where x.hotelId == _booking.hotelIdFK
                                        select x).FirstOrDefault();
                        var getBooking = (from x in context.Hotel_Booking
                                          where x.userIdFK == _user.userId
                                          select x).FirstOrDefault();
                        var getArrival = (from x in context.Arrivals
                                          where x.userIdFK == _user.userId
                                          select x).FirstOrDefault();

                        getArrival.numberCompetitors = Convert.ToInt32(nudCompetitors.Value);
                        getArrival.numberDelegate = Convert.ToInt32(nudDelegates.Value);
                        getArrival.numberCompetitors = Convert.ToInt32(nudCompetitors.Value);
                        context.SaveChanges();
                        if (dataGridView1[4, 0].Value.ToString() != "")
                        {
                            if (getHotel.numSingleRoomsBooked == 0)
                            {
                                getHotel.numSingleRoomsBooked = Convert.ToInt32(dataGridView1[4, 0].Value);
                            }
                            else
                            {
                                getHotel.numSingleRoomsBooked = getHotel.numSingleRoomsBooked + Convert.ToInt32(dataGridView1[3, 0].Value) - Convert.ToInt32(dataGridView1[4, 0].Value);
                            }
                        }
                        if (dataGridView1[4, 1].Value.ToString() != "")
                        {
                            if (getHotel.numDoubleRoomsBooked == 0)
                            {
                                getHotel.numDoubleRoomsBooked = Convert.ToInt32(dataGridView1[4, 1].Value);
                            }
                            else
                            {
                                getHotel.numDoubleRoomsBooked = getHotel.numDoubleRoomsBooked + Convert.ToInt32(dataGridView1[3, 1].Value) - Convert.ToInt32(dataGridView1[4, 1].Value);
                            }
                        }
                        context.SaveChanges();
                        if (dataGridView1[4, 0].Value.ToString() != "")
                        { 
                            getBooking.numSingleRoomsRequired = Convert.ToInt32(dataGridView1[4, 0].Value);
                        }
                        if (dataGridView1[4, 1].Value.ToString() != "")
                        {
                            getBooking.numDoubleRoomsRequired = Convert.ToInt32(dataGridView1[4, 1].Value);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Update info completed!");
                        Hide();
                        (new CountryMain(_user)).ShowDialog();
                        Close();
                    }
                }
            }

        }
    }
}
