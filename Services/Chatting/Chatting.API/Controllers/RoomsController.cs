using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// Common
using SalesHub.Common.WebAPI;

// Chatting Application Commands
using Chatting.Application.Commands.RoomCommands;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Exceptions;

namespace Chatting.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class RoomsController : ControllerBase
   {
      private readonly IMediator _mediator;

      public RoomsController(IMediator mediator)
      {
         _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      [HttpPost]
      public async Task<IActionResult> CreateRoom(
         [FromHeader] string userCode,
         [FromBody] CreateRoomCommand request)
      {
         if (!request.UserCodes.Contains(userCode))
         {
            var userCodes = request.UserCodes.ToList();

            userCodes.Add(userCode);

            request.UserCodes = userCodes;
         }

         try
         {
            Room createdRoom = await _mediator.Send(request);

            return this.OkResult(createdRoom);
         }
         catch (ChattingDomainException ex)
         {
            return this.ErrorResult(ex.ErrorCode, ex.Message, ex, HttpStatusCode.BadRequest);
         }
         catch (Exception ex)
         {
            return this.ErrorResult("", ex.Message, ex, HttpStatusCode.InternalServerError);
         }
      }
   }
}