using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LDTSolutions.Common.WebApi.Mvc
{
   public static class ActionResultControllerExtensions
   {
      public static IActionResult OkResult(this ControllerBase controller, HttpStatusCode statusCode = HttpStatusCode.OK)
      {
         return new CustomOkActionResult(new CustomResult()
         {
            IsSuccess = true,
            ErrorCode = "",
            ErrorMessage = "",
            Result = null,
            ErrorDetails = null
         }, statusCode);
      }

      public static IActionResult OkResult(this ControllerBase controller, object result, HttpStatusCode statusCode = HttpStatusCode.OK)
      {
         return new CustomOkActionResult(new CustomResult()
         {
            IsSuccess = true,
            ErrorCode = "",
            ErrorMessage = "",
            Result = result,
            ErrorDetails = null
         }, statusCode);
      }

      public static IActionResult CreatedResult(this ControllerBase controller, string routeName, object result)
      {
         return new CustomCreatedActionResult(routeName, new CustomResult()
         {
            IsSuccess = true,
            ErrorCode = "",
            ErrorMessage = "",
            Result = result,
            ErrorDetails = null
         });
      }

      public static IActionResult ErrorResult(this ControllerBase controller, string errorCode, string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
      {
         return new CustomErrorActionResult(new CustomResult()
         {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = message,
            Result = null,
            ErrorDetails = null
         }, statusCode);
      }

      public static IActionResult ErrorResult(this ControllerBase controller, string errorCode, string message, Exception errorDetails, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
      {
         return new CustomErrorActionResult(new CustomResult()
         {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = message,
            Result = null,
            ErrorDetails = errorDetails
         }, statusCode);
      }
   }

   public class CustomResult
   {
      public bool IsSuccess { get; set; }

      public string ErrorCode { get; set; }

      public string ErrorMessage { get; set; }

      public Exception ErrorDetails { get; set; }

      public object Result { get; set; }
   }

   public class CustomOkActionResult : IActionResult
   {
      private readonly CustomResult _result;
      private readonly HttpStatusCode _statusCode;

      public CustomOkActionResult(CustomResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
      {
         _result = result;
         _statusCode = statusCode;
      }

      public async Task ExecuteResultAsync(ActionContext context)
      {
         var objectResult = new ObjectResult(_result);

         switch (_statusCode)
         {
            case HttpStatusCode.Created:
               {
                  objectResult.StatusCode = StatusCodes.Status201Created;
                  break;
               }
            case HttpStatusCode.Accepted:
               {
                  objectResult.StatusCode = StatusCodes.Status202Accepted;
                  break;
               }
            default:
               {
                  objectResult.StatusCode = StatusCodes.Status200OK;
                  break;
               }
         }

         await objectResult.ExecuteResultAsync(context);
      }
   }

   public class CustomCreatedActionResult : IActionResult
   {
      private readonly string _routeName;
      private readonly CustomResult _result;

      public CustomCreatedActionResult(CustomResult result)
      {
         _result = result;
      }

      public CustomCreatedActionResult(string routeName, CustomResult result)
      {
         _routeName = routeName;
         _result = result;
      }

      public async Task ExecuteResultAsync(ActionContext context)
      {
         var createdResult = new CreatedAtRouteResult(_routeName, _result.Result);

         createdResult.StatusCode = StatusCodes.Status201Created;

         await createdResult.ExecuteResultAsync(context);
      }
   }

   public class CustomErrorActionResult : IActionResult
   {
      private readonly CustomResult _result;
      private readonly HttpStatusCode _statusCode;

      public CustomErrorActionResult(CustomResult result, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
      {
         _result = result;
         _statusCode = statusCode;
      }

      public async Task ExecuteResultAsync(ActionContext context)
      {
         var objectResult = new ObjectResult(_result);

         switch (_statusCode)
         {
            case HttpStatusCode.BadRequest:
               {
                  objectResult.StatusCode = StatusCodes.Status400BadRequest;
                  break;
               }
            case HttpStatusCode.NotFound:
               {
                  objectResult.StatusCode = StatusCodes.Status404NotFound;
                  break;
               }
            case HttpStatusCode.Unauthorized:
               {
                  objectResult.StatusCode = StatusCodes.Status401Unauthorized;
                  break;
               }
            default:
               {
                  objectResult.StatusCode = StatusCodes.Status500InternalServerError;
                  break;
               }
         }

         await objectResult.ExecuteResultAsync(context);
      }
   }
}
