using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatting.Application.Queries.ChatMessageQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesHub.Common.WebAPI;

namespace Chatting.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ChatMessagesController : ControllerBase
   {
      private readonly IMediator _mediator;

      public ChatMessagesController(IMediator mediator)
      {
         _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }
      public async Task<IActionResult> GetAllMessages()
      {
         var request = new GetAllMessagesQuery();

         var result = _mediator.Send(request);

         return this.OkResult(result);
      }
   }
}