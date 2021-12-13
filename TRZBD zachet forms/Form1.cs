using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRZBD_zachet_forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task button1_ClickAsync(object sender, EventArgs e, object JsonSerializer)
        {
            StreamReader r = new StreamReader("C:\\Users\\Smarter\\Desktop\\data.json");
            string json = r.ReadToEnd();
            List<Class1> read = JsonSerializer.Deserialize<List<Class1>>(json);
            foreach (var i in read)
            {
                HttpClient client = new HttpClient();
                Class1 st = new Class1()
                {
                    Id_station = i.Id_station,
                    Address = i.Address,
                    price_of_fill = i.price_of_fill.ToString()
               
                };
                var str = new StringContent(JsonSerializer.Serialize(st), Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:5000/api/Class1", str);
            }
            r.Close();
        }

        private async Task button2_ClickAsync(object sender, EventArgs e, object JsonSerializer)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://localhost:5000/api/Class1" + textBox1.Text);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content;
                    var data = await content.ReadAsStringAsync();

                    Class1 des = JsonSerializer.Deserialize<>(data);
                    new Form2(des).Show();
                }
                else
                {
                    Class1 st = new Class1()
                    {
                        Id_station = Convert.ToInt32(textBox1.Text),
                        Address = "",
                        price_of_fill = "[{\"Name\": \"92\"," +
                        "\"Price\": 0.0," +
                        "\"AmountOfFuel\": 0},{ " +
                        "\"Name\": \"95\"," +
                        "\"Price\": 0.0," +
                        "\"AmountOfFuel\": 0},{ " +
                        "\"Name\": \"98\"," +
                        "\"Price\": 0.0," +
                        "\"AmountOfFuel\": 0},{" +
                        "\"Name\": " +
                        "\"Disel Fuel\"," +
                        "\"Price\": 0.0," +
                        "\"AmountOfFuel\": 0}]"
                    };
                    var input = new StringContent(JsonSerializer.Serialize(st), Encoding.UTF8, "application/json");
                    await client.PostAsync("https://localhost:44394/api/Class1", input);
                 
                    textBox1.Clear();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
