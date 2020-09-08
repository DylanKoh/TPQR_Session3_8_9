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
    public partial class GuestsSummary : Form
    {
        public GuestsSummary()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new AdminMain()).ShowDialog();
            Close();
        }

        private void GuestsSummary_Load(object sender, EventArgs e)
        {
            cbGuests.SelectedIndex = 0;
        }

        private void cbGuests_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context =new Session3Entities())
            {
                chart1.Series.Clear();
                chart1.Series.Add("Delegates");
                chart1.Series.Add("Competitors");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                if (cbGuests.SelectedIndex == 0)
                {
                    var getArrivals = (from x in context.Arrivals
                                       select x);
                    foreach (var item in getArrivals)
                    {
                        var idx1 = chart1.Series[0].Points.AddXY(item.User.countryName, item.numberDelegate + item.numberHead);
                        var idx2 = chart1.Series[1].Points.AddXY(item.User.countryName, item.numberCompetitors);
                        chart1.Series[0].Points[idx1].Label = (item.numberDelegate + item.numberHead).ToString();
                        chart1.Series[1].Points[idx2].Label = item.numberCompetitors.ToString();
                    }
                }
                else if (cbGuests.SelectedIndex == 1)
                {
                    var getArrivals = (from x in context.Arrivals
                                       select x);
                    foreach (var item in getArrivals)
                    {
                        var idx1 = chart1.Series[0].Points.AddXY(item.User.countryName, item.numberDelegate + item.numberHead);
                        chart1.Series[0].Points[idx1].Label = (item.numberDelegate + item.numberHead).ToString();
                    }
                }
                else
                {
                    var getArrivals = (from x in context.Arrivals
                                       select x);
                    foreach (var item in getArrivals)
                    {
                        var idx2 = chart1.Series[1].Points.AddXY(item.User.countryName, item.numberCompetitors);
                        chart1.Series[1].Points[idx2].Label = item.numberCompetitors.ToString();
                    }
                }
            }
            
        }
    }
}
