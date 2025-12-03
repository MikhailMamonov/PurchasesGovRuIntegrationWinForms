using PurchasesGovRuIntegration.Services;
using PurchasesGovRuIntegration.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchasesGovRuIntegration
{
    public partial class Form1 : Form
    {
        private readonly IPurchaseService _service = new PurchaseService();

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string regNumber = textBox1.Text;

            Dictionary<string, Dictionary<string, string>> resultDict = await _service.Find(regNumber);

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            foreach (var item in resultDict)
            {
                string key = item.Key;
                string value = String.Join("\n", item.Value.Select(kv => $"{kv.Key}  {kv.Value}"));
                result.Add(new KeyValuePair<string, string>(key, value));

            }
            dataGridView1.DataSource = result;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, control characters (like backspace), and the decimal separator (if needed)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || textBox1.Text.Contains('.')))
            {
                e.Handled = true; // Mark the event as handled to prevent the character from being entered
            }
        }
    }
}
