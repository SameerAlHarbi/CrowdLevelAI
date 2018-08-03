using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HajjHK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = DateTime.Now.ToLongTimeString();
            timer2.Start();
        }

        private async Task changeImage()
        {
            string[] files = Directory.GetFiles(@"C:\Users\AbuJude\Desktop\HajjHK\HajjHK\Images", "*", SearchOption.TopDirectoryOnly);

            Random rnd = new Random();
            int idx = rnd.Next(1, files.Length);

            try
            {
                await showPercentage(files[idx]);
                Bitmap bmp = new Bitmap(files[idx]);
                this.pictureBox1.Image = bmp;
            }
            catch (Exception e)
            {
                // remove this if you don't want to see the exception message
                MessageBox.Show(e.Message);
            }

        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        private async Task showPercentage(string imgFile)
        {
            string key = "1150db2adb88481eb066be97e601af21";

            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", key);

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/a29a4648-96a0-45da-8898-37c1a5137e70/image";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            byte[] byteData = GetImageAsByteArray(imgFile);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                var res = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(res);

                var newData = JsonConvert.DeserializeObject<ModelData>(res);

                newData.Predictions = newData.Predictions.OrderByDescending(p => p.Probability).ToList();

                numericUpDown1.Value = decimal.Parse( newData.Predictions.FirstOrDefault().TagName);
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
           await updateDb(30);
        }

        private async Task updateDb(decimal percentage)
        {
            await changeImage();
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));

            string serializeItemToCreate = JsonConvert.SerializeObject(new { id = 3, crowdLevelDate=DateTime.Now, percentage = numericUpDown1.Value, LocationName="Tawaf" });

            HttpResponseMessage response = await client.PostAsync("http://hajjhkapi.azurewebsites.net/api/CrowdLevel"
                , content: new StringContent(serializeItemToCreate, Encoding.Unicode, mediaType: "application/json"));

            var r = response;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await updateDb(30);

            this.timer1.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
