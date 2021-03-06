﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace WebApp.Engine
{
    public class ExceptionHandlerMiddleware : OwinMiddleware
    {
        public ExceptionHandlerMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var jsonException = new
                {
                    message = e.Message,
#if DEBUG
                    stackTrace = e.StackTrace
#endif
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(jsonException));
            }
        }
    }
}