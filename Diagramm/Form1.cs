using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diagramm
{
    public partial class Form1 : Form
    {
        private List<Product> products = new List<Product>();
        private int currentProductIndex = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int n) || n < 1 || n > 10)
            {
                MessageBox.Show("Введите корректное количество товаров (1-10).");
                return;
            }

            dataGridView1.Rows.Clear(); // Очищаем таблицу
            products.Clear(); // Очищаем список товаров
            currentProductIndex = 1;

            // Создаем верхние строки с информацией
            dataGridView1.Rows.Add("Название товара", "Цена", "Количество", "Общая стоимость товара");

            for (int i = 1; i < n; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        public class Product
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
            public double TotalSales { get; set; }

            public Product(string name, double price, int quantity, double totalSales)
            {
                Name = name;
                Price = price;
                Quantity = quantity;
                TotalSales = totalSales;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Series series = new Series("Диаграмма 1");
            series.ChartType = SeriesChartType.Pie;

            products.Clear();

            double totalSales = 0; // Общая стоимость всех товаров

            for (int rowIndex = 1; rowIndex < dataGridView1.Rows.Count; rowIndex++)
            {
                string productName = dataGridView1.Rows[rowIndex].Cells[0].Value as string;
                if (productName == null || string.IsNullOrEmpty(productName))
                {
                    continue; // Пропустить пустые строки или строки с пустыми значениями
                }

                string priceStr = dataGridView1.Rows[rowIndex].Cells[1].Value as string;
                string quantityStr = dataGridView1.Rows[rowIndex].Cells[2].Value as string;

                if (priceStr == null || quantityStr == null)
                {
                    continue; // Пропустить строки, где цена или количество отсутствуют
                }

                double price = double.Parse(priceStr);
                int quantity = int.Parse(quantityStr);

                double currentTotalSales = price * quantity;

                textBox2.Text += $"{productName}: Общая стоимость - {currentTotalSales.ToString("F2")} руб.\r\n";

                products.Add(new Product(productName, price, quantity, currentTotalSales));
            }
        
            foreach (Product product in products)
            {
                double percentage = (product.TotalSales / totalSales) * 100;

                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisLabel = $"{product.Name} ({percentage.ToString("F2")}%)";
                dataPoint.YValues = new double[] { product.TotalSales };
                dataPoint.Label = "#PERCENT{F2}"; // Отображение процентов
                series.Points.Add(dataPoint);
            }

            chart1.Series.Clear();
            chart1.Series.Add(series);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}