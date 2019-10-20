using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// Common
using SalesHub.Common.WebAPI;

// Chatting Application Commands
using Chatting.Application.Commands.UserCommands;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Exceptions;

namespace Chatting.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class UsersController : ControllerBase
   {
      private readonly IMediator _mediator;

      public UsersController(IMediator mediator)
      {
         _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      [HttpPost]
      public async Task<IActionResult> CreateRoom(
         [FromBody] CreateUserCommand request)
      {
         try
         {
            User createdUser = await _mediator.Send(request);

            return this.OkResult(createdUser);
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