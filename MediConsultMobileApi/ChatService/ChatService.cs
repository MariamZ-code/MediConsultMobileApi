using Microsoft.AspNetCore.SignalR.Client;

namespace MediConsultMobileApi.ChatService
{
    public class ChatService
    {
        private readonly HubConnection _connection;

        public event Action<string, string> OnMessageReceived;

        public ChatService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5026/chatHub")
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                OnMessageReceived?.Invoke(user, message);
            });
        }

        public async Task StartAsync()
        {
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                // Handle connection error
            }
        }

        public async Task SendMessageAsync(string user, string message)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                // Handle send message error
            }
        }
    }
}
