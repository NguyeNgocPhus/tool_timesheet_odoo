using auto_checkin.Models;
using System;
using System.Net.WebSockets;
using System.Text;

namespace auto_checkin.Services.Websocket
{
    public class WebsocketHandler
    {
        private readonly static ConnectionMapping<string> _connections = new();

        #region Manager message connection
        public async Task OnConnectedAsync(WebSocket webSocket)
        {


            await SendMessage(webSocket, new { type = "welcome" });

            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var msg_str = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var client_msg = Newtonsoft.Json.JsonConvert.DeserializeObject<WsClientMessage>(msg_str.ToString());

                    if (client_msg.command == "subscribe")
                    {
                        _connections.Add(client_msg.identifier, webSocket);
                        //PingClient(client_msg.identifier, webSocket);
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close || webSocket.State == WebSocketState.Aborted)
                {

                    await CloseWs(webSocket, result);
                }
            }


        }
        private async Task CloseWs(WebSocket ws, WebSocketReceiveResult wsResult)
        {
            await ws.CloseAsync(wsResult.CloseStatus.Value, wsResult.CloseStatusDescription, CancellationToken.None);
        }
        #endregion
        #region Send message
        private async Task SendMessage<T>(IEnumerable<WebSocket> connections, T message)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(json);

            await Parallel.ForEachAsync(connections, async (socket, token) =>
            {
                await SendMessage(socket, bytes);
            });
        }
        private async Task SendMessage<T>(WebSocket ws, T message)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            await SendMessage(ws, bytes);
        }
        private async Task SendMessage(WebSocket ws, byte[] bytes)
        {
            if (ws.State == WebSocketState.Open)
            {
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessageToClient<T>(string session_id, T message)
        {
            var connections = _connections.GetConnections(session_id);
            if (connections.Any())
            {
                await SendMessage(connections, message);
            }
        }
        #endregion
        #region Ping client

        private void PingClient(string session_id, WebSocket ws)
        {
            Task.Factory.StartNew(async () =>
            {
                if (ws.State == WebSocketState.Open)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    await SendMessage(ws, new { type = "ping", message = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), connection = _connections.Statics });

                    PingClient(session_id, ws);
                }
                else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
                {
                    _connections.Remove(session_id, ws);
                }
            });
        }
        #endregion
    }
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<WebSocket>> _connections = new();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public string Statics
        {
            get
            {
                return string.Join(',', _connections.Select(t => $"customer: {t.Key} - connection: {t.Value.Count}"));
            }
        }

        public void Add(T key, WebSocket connectionId)
        {
            lock (_connections)
            {
                HashSet<WebSocket> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<WebSocket>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<WebSocket> GetConnections(T key)
        {
            HashSet<WebSocket> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<WebSocket>();
        }

        public IEnumerable<WebSocket> GetConnections()
        {
            return _connections.SelectMany(t => t.Value).ToList();
        }

        public void Remove(T key, WebSocket connectionId)
        {
            lock (_connections)
            {
                HashSet<WebSocket> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
