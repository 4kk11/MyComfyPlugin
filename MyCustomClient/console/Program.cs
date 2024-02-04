using System;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace MyCustomClient
{
    class Program
    {
        // サーバーアドレスを静的変数で定義
        private static readonly string SERVER_ADDRESS = "127.0.0.1:8188";

        static async Task Main(string[] args)
        {
            // ユーザー入力を受け取る
            Console.WriteLine("Enter message: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is empty. Exiting...");
                return;
            }

            // メッセージ送信を試みる
            try
            {
                await SendMessage(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // サーバーにメッセージを送信するメソッド
        static async Task SendMessage(string message)
        {
            // 送信するメッセージを表示
            Console.WriteLine("Sending message: " + message);
            using (ClientWebSocket client = new ClientWebSocket())
            {
                // WebSocketサーバーへ接続
                Uri serverUri = new Uri($"ws://{SERVER_ADDRESS}/ws");
                await client.ConnectAsync(serverUri, CancellationToken.None);

                // RESTクライアントを作成
                RestClient restClient = new RestClient($"http://{SERVER_ADDRESS}");

                // 送信メッセージを格納
                var dic = new Dictionary<string, string>
                {
                    { "text", message }
                };

                // REST APIを通じてメッセージ送信
                RestRequest restRequest = new RestRequest("/my_custom_client/update_text", Method.Post);
                string jsonData = JsonConvert.SerializeObject(dic);
                restRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                await restClient.ExecuteAsync(restRequest);

                // サーバーからの応答を待ち続ける
                while (client.State == WebSocketState.Open)
                {
                    var receiveBuffer = new byte[1024];
                    var result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    // 受信データをJSONに変換
                    var json = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                    var comfyReceiveObject = JsonConvert.DeserializeObject<ComfyReceivedObject>(json);

                    bool isClose = false;

                    // 応答タイプによって処理を分岐
                    switch (comfyReceiveObject?.Type)
                    {
                        case "update_text":
                            Console.WriteLine("Received update_text: " + comfyReceiveObject.Data?["text"]);
                            isClose = true;
                            break;
                    }

                    // 応答処理後に接続を閉じる
                    if (isClose)
                    {
                        break;
                    }
                }
            }
        }
    }

    // サーバーからの応答を格納するクラス
    public class ComfyReceivedObject
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object>? Data { get; set; }
    }
}
