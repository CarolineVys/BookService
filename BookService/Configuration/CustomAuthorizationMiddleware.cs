namespace BookService.Configuration
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader == "Bearer MyTestAuthorization")
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                return;
            }
        }
    }
}
