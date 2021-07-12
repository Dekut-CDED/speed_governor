using System.Threading.Tasks;
using Application.Location;
using Application.User;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.SignalRhub
{
    public class SignalRealTimeLocation : Hub
    {
        private readonly IMediator _mediator;

        public SignalRealTimeLocation(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // public async Task SendComment(Create.Command command){
        //    var location = await _mediator.Send(command);
        //    await Clients.Group(command.Location.SpeedGovId.ToString()).SendAsync("receiveLocation", location);
        // }


        public override async Task OnConnectedAsync()
        {
          var httpcontext = Context.GetHttpContext();
          var speedgovImei = httpcontext.Request.Query["speedgovImei"];
          await Groups.AddToGroupAsync(Context.ConnectionId, speedgovImei);

          var result = await _mediator.Send(new LatestLocation.Query{ Id = speedgovImei});

          await Clients.Caller.SendAsync("LoadLocations", result);
        }

    }
}