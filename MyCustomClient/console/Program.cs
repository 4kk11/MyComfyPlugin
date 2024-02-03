using System;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace MyCustomClient
{
    class Program
    {
        private static readonly string SERVER_ADDRESS = "127.0.0.1:8188";
        static async Task Main(string[] args)
        {
            // get input
            Console.WriteLine("Enter message: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is empty. Exiting...");
                return;
            }

            // send message
            try
            {
                await SendMessage(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task SendMessage(string message)
        {
            Console.WriteLine("Sending message: " + message);
            using (ClientWebSocket client = new ClientWebSocket())
            {
                // connect to websocket server
                Uri serverUri = new Uri($"ws://{SERVER_ADDRESS}/ws");
                await client.ConnectAsync(serverUri, CancellationToken.None);

                // create rest client
                RestClient restClient = new RestClient($"http://{SERVER_ADDRESS}");


                // create json
                var dic = new Dictionary<string, string>
                {
                    { "text", message }
                };

                // send message
                RestRequest restRequest = new RestRequest("/my_custom_client/update_text", Method.Post);
                string jsonData = JsonConvert.SerializeObject(dic);
                restRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                await restClient.ExecuteAsync(restRequest);

                while (client.State == WebSocketState.Open)
                {
                    var receiveBuffer = new byte[1024];
                    var result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    // Convet to json
                    var json = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                    var comfyReceiveObject = JsonConvert.DeserializeObject<ComfyReceivedObject>(json);

                    bool isClose = false;

                    switch (comfyReceiveObject?.Type)
                    {
                        case "update_text":
                            Console.WriteLine("Received update_text: " + comfyReceiveObject.Data?["text"]);
                            isClose = true;
                            break;
                    }

                    if (isClose)
                    {
                        break;
                    }
                }

            }
        }
    }

    public class ComfyReceivedObject
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object>? Data { get; set; }
    }
}
