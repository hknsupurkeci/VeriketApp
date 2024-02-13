using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Configuration; // For accessing configuration settings

namespace WinFormApplication
{
    public partial class Form1 : Form
    {
        private readonly string[] _columnNames = { "Date", "Computer Name", "User Name" };
        private readonly string logDirectoryName = ConfigurationManager.AppSettings["LogDirectoryName"];
        private readonly string logFileName = ConfigurationManager.AppSettings["LogFileName"];

        public Form1()
        {
            InitializeComponent();
            // Automatically adjust column widths
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCsvDataIntoDataGridView();
        }

        private void LoadCsvDataIntoDataGridView()
        {
            // Clear DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Full path to the CSV file
            string csvFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), logDirectoryName, logFileName);
            if (File.Exists(csvFilePath))
            {
                try
                {
                    DataTable dataTable = CreateDataTableFromCsv(csvFilePath);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while reading the CSV file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("CSV file not found.");
            }
        }

        private DataTable CreateDataTableFromCsv(string csvFilePath)
        {
            DataTable dataTable = new DataTable();

            // Create columns for the header row
            foreach (string columnName in _columnNames)
            {
                dataTable.Columns.Add(columnName);
            }

            // Read the CSV file and convert it into a DataTable
            string[] csvLines = File.ReadAllLines(csvFilePath);
            for (int i = 1; i < csvLines.Length; i++) // Skip the header row
            {
                string[] rowData = csvLines[i].Split(',');
                dataTable.Rows.Add(rowData);
            }

            return dataTable;
        }
    }
}
