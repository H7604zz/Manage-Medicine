using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using ProjectPrn.Models;

namespace ProjectPrn.View.Admin
{
    public partial class Report : Window
    {
        public Report()
        {
            InitializeComponent();
        }

        private void OnGenerateReportClick(object sender, RoutedEventArgs e)
        {
            DateOnly startDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value);
            DateOnly endDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value);

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Please select both start and end dates.");
                return;
            }

            LoadReportData(startDate, endDate);
        }

        private void LoadReportData(DateOnly startDate, DateOnly endDate)
        {
            using (var context = new Prn212medicineContext())
            {
                var orderData = context.OrderHistories
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .ToList();

                GeneratePieChart(orderData);

                decimal revenue = orderData.Where(o => o.Status == 2).Sum(o => o.Amount);
                txtRevenue.Text = revenue.ToString("C2");
            }
        }

        private void GeneratePieChart(IEnumerable<OrderHistory> orders)
        {
            using (var context = new Prn212medicineContext())
            {
                var ordersWithStatusNames = from order in context.OrderHistories
                                            join status in context.StatusOrders
                                            on order.Status equals status.Id
                                            select new
                                            {
                                                order.Id,
                                                order.AccountId,
                                                order.Amount,
                                                order.OrderDate,
                                                order.PaymentDate,
                                                order.PaymentMethod,
                                                StatusName = status.StatusName
                                            };

                var statusCounts = ordersWithStatusNames
                    .GroupBy(o => o.StatusName)
                    .Select(g => new { StatusName = g.Key, Count = g.Count() })
                    .ToList();

                var pieSeries = new PieSeries
                {
                    StrokeThickness = 2.0,
                    InsideLabelPosition = 0.8,
                    AngleSpan = 360,
                    StartAngle = 0,
                    InsideLabelFormat = "{1} - {0}"
                };

                foreach (var statusCount in statusCounts)
                {
                    pieSeries.Slices.Add(new PieSlice(statusCount.StatusName, statusCount.Count));
                }

                var pieModel = new PlotModel { Title = "Order Status Distribution" };
                pieModel.Series.Add(pieSeries);

                pieChartView.Model = pieModel;
            }
        }

    }
}
