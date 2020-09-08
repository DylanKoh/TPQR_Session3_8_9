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
    public partial class ArrivalSummary : Form
    {
        public ArrivalSummary()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new AdminMain()).ShowDialog();
            Close();
        }

        private void ArrivalSummary_Load(object sender, EventArgs e)
        {
            var newRow = new List<string>() { "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm" };
            dataGridView1.Rows.Add(newRow.ToArray());
            dataGridView2.Rows.Add(newRow.ToArray());
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
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewColumn cell in dataGridView2.Columns)
                {
                    if (dataGridView2[cell.Index, row.Index].Value.ToString() == "9am" || dataGridView2[cell.Index, row.Index].Value.ToString() == "12pm" || dataGridView2[cell.Index, row.Index].Value.ToString() == "4pm")
                    {
                        dataGridView2[cell.Index, row.Index].Style.BackColor = Color.Black;
                    }
                }
            }

            using (var context = new Session3Entities())
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView1.Columns)
                    {
                        var timing = dataGridView1[cell.Index, row.Index].Value.ToString();
                        var date = DateTime.Parse("22 July 2020");
                        var getArrival = (from x in context.Arrivals
                                          where x.arrivalDate == date && x.arrivalTime == timing
                                          select x);
                        var sb = new StringBuilder(dataGridView1[cell.Index, row.Index].Value.ToString());
                        foreach (var item in getArrival)
                        {
                            sb.Append($"\n\n{item.User.countryName}\n({item.numberCars + item.number19seat + item.number42seat}Veh)");
                        }
                        dataGridView1[cell.Index, row.Index].Value = sb.ToString();
                    }
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView2.Columns)
                    {
                        var timing = dataGridView2[cell.Index, row.Index].Value.ToString();
                        var date = DateTime.Parse("23 July 2020");
                        var getArrival = (from x in context.Arrivals
                                          where x.arrivalDate == date && x.arrivalTime == timing
                                          select x);
                        var sb = new StringBuilder(dataGridView2[cell.Index, row.Index].Value.ToString());
                        foreach (var item in getArrival)
                        {
                            sb.Append($"\n\n{item.User.countryName}\n({item.numberCars + item.number19seat + item.number42seat}Veh)");
                        }
                        dataGridView2[cell.Index, row.Index].Value = sb.ToString();
                    }
                }
            }
        }
    }
}
