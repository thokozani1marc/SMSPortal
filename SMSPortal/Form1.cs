using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Windows.Forms;

using System.Net.Http;
using System.Net.Http.Headers; 

namespace SMSPortal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var client = new RestClient("https://rest.smsportal.com");
            var authToken = "";

            var apiKey = "77323a1b-636c-4a8f-97a0-2ecf394ee916";
            var apiSecret = "bj623FS4lO3gkbhWA/shUmLYSHLRLrwA";
            var accountApiCredentials = $"{apiKey}:{apiSecret}";

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(accountApiCredentials);
            var base64Credentials = Convert.ToBase64String(plainTextBytes);


            var authRequest = new RestRequest("/v1/Authentication", Method.GET);

            authRequest.AddHeader("Authorization", $"Basic {base64Credentials}");


            var authResponse = client.Execute(authRequest);
            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = JObject.Parse(authResponse.Content);
                authToken = authResponseBody["token"].ToString();
            }
            else
            {
                //Console.WriteLine(authResponse.ErrorMessage);
                MessageBox.Show(authResponse.ErrorMessage);
                return;
            }


            //send request
            var sendRequest = new RestRequest("/v1/bulkmessages", Method.POST);

            var authHeader = $"Bearer {authToken}";
            sendRequest.AddHeader("Authorization", $"{authHeader}");

            sendRequest.AddJsonBody(new
            {
                Messages = new[]
                {
                    new
                    {
                    content = "Hello SMS World from C#",
                    destination = ">>0786384291<<"
                    }
                }
            });

            //response validation
            var sendResponse = client.Execute(sendRequest); 
            if (sendResponse.StatusCode == HttpStatusCode.OK)
            {
                //Console.WriteLine(sendResponse.Content);
                MessageBox.Show(sendResponse.Content);
                MessageBox.Show("message sent succefully");
            }
            else
            {
                //Console.WriteLine(sendResponse.ErrorMessage);
                MessageBox.Show(sendResponse.ErrorMessage);
            }
        }
    }
}
