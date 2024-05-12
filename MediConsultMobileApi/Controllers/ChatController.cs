using FirebaseAdmin.Messaging;
using MediConsultMobileApi.ChatService;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Hubs;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMemberRepository memberRepo;

    public ChatController(IHubContext<ChatHub> hubContext, IMemberRepository memberRepo)
    {
        _hubContext = hubContext;
        this.memberRepo = memberRepo;
    }

    //[HttpPost("send")]
    //public async Task<IActionResult> SendMessage([Required]int userId, string message , string lang)
    //{
    //    var memberExsist = memberRepo.MemberExists(userId);
    //    if (!memberExsist)
    //    {
    //        return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
    //    }
    //    var member =await memberRepo.GetByID(userId);
    //    var name = member.member_name;
    //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", name, message);
    //    return Ok(new MessageDto { Message=$"{name} : {message}"});
    //}
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([Required] int userId, string message, string lang)
    {
        var memberExsist = memberRepo.MemberExists(userId);
        if (!memberExsist)
        {
            return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
        }
        var member = await memberRepo.GetByID(userId);
        var name = member.member_name;
        var chatService = new ChatService();
        var msg = new MessageDto();
        chatService.OnMessageReceived += (user, message) =>
        {
            msg.Message = $"{name} : {message}" ;

        };

        await chatService.StartAsync();

        // To send a message
        await chatService.SendMessageAsync(name,message);

        return Ok(msg);

    }


}

