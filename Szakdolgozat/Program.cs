using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
//using Szakdolgozat.Data
//this might be a problem
using Szakdolgozat.Models;
using Szakdolgozat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//Singleton regisztrálása
builder.Services.AddSingleton<MyFile>();
builder.Services.AddSingleton<MockInstructionList>();
builder.Services.AddSingleton<MenuService>();
builder.Services.AddSingleton<HardwareService>();
builder.Services.AddSingleton<Analysis>();
builder.Services.AddSingleton<DebugService>();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddSingleton<InstructionParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
