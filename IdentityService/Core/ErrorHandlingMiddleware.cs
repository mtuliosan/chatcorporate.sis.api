using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using IdentityService.Domain;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityService.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RequestDelegate next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is CustomException customException)
                context.Response.StatusCode = (int)customException.StatusCode;
            else
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            log.Error(ex);
            var json = JsonConvert.SerializeObject(new { message = ex.Message });

            return context.Response.WriteAsync(json);
        }
    }
}
