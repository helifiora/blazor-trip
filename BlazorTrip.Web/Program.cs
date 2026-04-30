using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTrip.Web;
using BlazorTrip.Web.Commans;
using BlazorTrip.Web.Facade;
using BlazorTrip.Web.Queries;
using BlazorTrip.Web.Repositories;

var culture = new CultureInfo("pt-BR");
culture.NumberFormat.CurrencySymbol = "R$ ";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddTransient<SelectTransactionDto>();
builder.Services.AddTransient<ReportFacade>();
builder.Services.AddTransient<CreateTransactionCommand>();

await builder.Build().RunAsync();