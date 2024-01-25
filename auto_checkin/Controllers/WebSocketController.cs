using System.Net.WebSockets;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Controllers;

// <snippet>
public class WebSocketController : ControllerBase
{

    private readonly WebsocketHandler _wsHandler;

    public WebSocketController(WebsocketHandler wsHandler)
    {
        _wsHandler = wsHandler;
    }

    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await _wsHandler.OnConnectedAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}