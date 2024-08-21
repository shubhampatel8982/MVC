using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware to terminate chain when URL contains /end
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("/end"))
    {
        await context.Response.WriteAsync("Request terminated.");
    }
    else
    {
        await next();
    }
});

// Middleware to handle URL containing hello and display hello
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("hello"))
    {
        await context.Response.WriteAsync("Hello");
        await next();
    }
    else
    {
        await next();
    }
});

// Middleware to add World! to the response if URL contains hello
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Contains("hello"))
    {
        await context.Response.WriteAsync(" World!");
    }
    else
    {
        await next();
    }
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=First}/{action=Index1}/{id?}");
});

app.Run();
