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
    public partial class ConfirmArrival : Form
    {
        User _user;
        public ConfirmArrival(User user)
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

        private void rb22_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var newRow = new List<string>() { "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm" };
            dataGridView1.Rows.Add(newRow.ToArray());
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn cell in dataGridView1.Columns)
                {
                    if (dataGridView1[cell.Index, row.Index].Value.ToString() == "10am" || dataGridView1[cell.Index, row.Index].Value.ToString() == "1pm" || dataGridView1[cell.Index, row.Index].Value.ToString() == "2pm")
                    {
                        dataGridView1[cell.Index, row.Index].Style.BackColor = Color.Black;
                    }
                }
            }
        }

        private void rb23_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var newRow = new List<string>() { "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm" };
            dataGridView1.Rows.Add(newRow.ToArray());
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn cell in dataGridView1.Columns)
                {
                    if (dataGridView1[cell.Index, row.Index].Value.ToString() == "9am" || dataGridView1[cell.Index, row.Index].Value.ToString() == "12pm" || dataGridView1[cell.Index, row.Index].Value.ToString() == "4pm")
                    {
                        dataGridView1[cell.Index, row.Index].Style.BackColor = Color.Black;
                    }
                }
            }
        }

        private void nudHead_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }


        private void nudDelegates_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }

        private void nudCompetitors_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }

        private void CalculateVehicles()
        {
            lblCar.Text = 0.ToString();
            lbl19.Text = 0.ToString();
            lbl42.Text = 0.ToString();
            int head = Convert.ToInt32(nudHead.Value);
            lblCar.Text = head.ToString();

            int visitors = Convert.ToInt32(nudDelegates.Value + nudCompetitors.Value);
            if (visitors % 42 == 0)
            {
                lbl42.Text = (visitors / 42).ToString();
            }
            else
            {
                if (visitors % 42 > 38)
                {
                    lbl42.Text = ((visitors / 42) + 1).ToString();
                }
                else if (visitors % 42 <= 38)
                {
                    if (visitors % 42 <= 19)
                    {
                        lbl42.Text = (visitors / 42).ToString();
                        lbl19.Text = 1.ToString();
                    }
                    else
                    {
                        lbl42.Text = (visitors / 42).ToString();
                        lbl19.Text = 2.ToString();
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!rb22.Checked && !rb23.Checked)
            {
                MessageBox.Show("Please select a date!");
            }
            else if ( dataGridView1.CurrentCell == null ||dataGridView1.CurrentCell.Style.BackColor == Color.Black)
            {
                MessageBox.Show("Please select a valid timing!");
            }
            else if (nudCompetitors.Value + nudDelegates.Value == 0)
            {
                MessageBox.Show("Ensure that the visitors arriving is filled!");
            }
            else
            {
                using (var context = new Session3Entities())
                {
                    if (rb22.Checked)
                    {
                        var newArrival = new Arrival()
                        {
                            arrivalDate = DateTime.Parse("22 July 2020"),
                            arrivalTime = dataGridView1.CurrentCell.Value.ToString(),
                            number19seat = int.Parse(lbl19.Text),
                            number42seat = int.Parse(lbl42.Text),
                            numberCars = int.Parse(lblCar.Text),
                            numberHead = Convert.ToInt32(nudHead.Value),
                            numberDelegate = Convert.ToInt32(nudDelegates.Value),
                            numberCompetitors = Convert.ToInt32(nudCompetitors.Value)
                        };
                        context.Arrivals.Add(newArrival);
                    }
                    else
                    {
                        var newArrival = new Arrival()
                        {
                            arrivalDate = DateTime.Parse("23 July 2020"),
                            arrivalTime = dataGridView1.CurrentCell.Value.ToString(),
                            number19seat = int.Parse(lbl19.Text),
                            number42seat = int.Parse(lbl42.Text),
                            numberCars = int.Parse(lblCar.Text),
                            numberHead = Convert.ToInt32(nudHead.Value),
                            numberDelegate = Convert.ToInt32(nudDelegates.Value),
                            numberCompetitors = Convert.ToInt32(nudCompetitors.Value)
                        };
                        context.Arrivals.Add(newArrival);
                    }

                    context.SaveChanges();
                    MessageBox.Show("Arrival confirmed!");
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                    
                }
            }
        }
    }
}
