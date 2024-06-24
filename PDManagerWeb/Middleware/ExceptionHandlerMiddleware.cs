namespace PDManagerWeb.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await httpContext.Response.WriteAsJsonAsync(new { error = ex.Message, 
                    statusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
