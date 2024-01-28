using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RestSharp;

namespace MyCustomClient
{
    class Program
    {
        private static readonly string CLIENT_ID = "B9F75B5E163146F89C628CE56669C138";
        private static readonly string SERVER_ADDRESS = "127.0.0.1:8188";
        static async Task Main(string[] args)
        {   
            // get input
            Console.WriteLine("Enter message: ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Input is null");
                return;
            }

            // send message
            try
            {
                await SendMessace(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task SendMessace(string message)
        {
            Console.WriteLine("Sending message: " + message);
            using (ClientWebSocket client = new ClientWebSocket())
            {
                // connect to websocket server
                Uri serverUri = new Uri($"ws://{SERVER_ADDRESS}/ws?client_id={CLIENT_ID}");
                await client.ConnectAsync(serverUri, CancellationToken.None);

                // create rest client
                RestClient restClient = new RestClient($"http://{SERVER_ADDRESS}");
                

                // create json
                var json = new Dictionary<string, string>
                {
                    { "type", "update_text"},
                    { "text", message }
                };

                // send message
                RestRequest restRequest = new RestRequest("/my_custom_client/update_text", Method.Post);
                string jsonData = JsonConvert.SerializeObject(json);
                restRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                await restClient.ExecuteAsync(restRequest);
                
            }
        }
    }
}
