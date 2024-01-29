using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task<List<string>> TranslateText(string originalText, string inputLanguage, string outputLanguage)
        {
            string url = $"https://translate.google.com/translate_a/single?client=gtx&sl={inputLanguage}&tl={outputLanguage}&dt=t&q={originalText}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                JArray jsonResponse = JArray.Parse(responseText);

                JArray translationParts = (JArray)jsonResponse[0];
                List<string> translatedParts = new List<string>();

                foreach (JArray part in translationParts)
                {
                    string translatedText = (string)part[0];
                    translatedParts.Add(translatedText);
                }

                return translatedParts;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string text = richText.Text;
            string inputLanguage = "fa";
            string outputLanguage = "en";

            List<string> translations = await TranslateText(text, inputLanguage, outputLanguage);

            string translatedText = string.Join("\n", translations);
            richTranslate.Text = translatedText;
        }
    }
}

