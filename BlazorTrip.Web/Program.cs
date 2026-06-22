using System.Globalization;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Application.Services;
using BlazorTrip.Application.ViewModels;
using BlazorTrip.Infrastructure.InMemory;
using BlazorTrip.Infrastructure.Repositories;
using BlazorTrip.Infrastructure.Services;
using BlazorTrip.Web;
using BlazorTrip.Web.WebApis;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var culture = new CultureInfo("pt-BR");
culture.NumberFormat.CurrencySymbol = "R$ ";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IMessenger, WeakReferenceMessenger>();
builder.Services.AddScoped<InMemoryDaoContext>();

builder.Services.AddScoped<IPersonRepository, MemoryPersonRepository>();
builder.Services.AddScoped<ICategoryRepository, MemoryCategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, MemoryTransactionRepository>();
builder.Services.AddScoped<IReportRepository, MemoryReportRepository>();

builder.Services.AddTransient<ICsvService, MemoryCsvService>();
builder.Services.AddTransient<PopoverApi>();

builder.Services.AddTransient<PersonPageViewModel>();
builder.Services.AddTransient<CategoryPageViewModel>();
builder.Services.AddTransient<TransactionPageViewModel>();
builder.Services.AddTransient<ReportPageViewModel>();


await builder.Build().RunAsync();