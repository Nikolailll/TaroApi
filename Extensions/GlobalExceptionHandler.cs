using System.Net;
using LogServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using NewWebApi.Exceptions;
using NewWebApi.Models;

namespace NewWebApi.Extensions
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILoggerManager _logger;

		public GlobalExceptionHandler(ILoggerManager logger)
		{
			_logger = logger;
		}
		
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			httpContext.Response.ContentType = "application/json";
			
			var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
			if(contextFeature != null)
			{
				httpContext.Response.StatusCode = contextFeature.Error switch {
					NotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError 
					};
				_logger.LogError($"Something went wrong: {contextFeature.Error}");
				await httpContext.Response.WriteAsync(new ErrorDetails()
				{
					StatusCode = httpContext.Response.StatusCode,
					Message = contextFeature.Error.Message,
				}.ToString());
			}

			return true;
			
		}
	}
}