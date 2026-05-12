using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTrip.Web;
using BlazorTrip.Web.Commans;
using BlazorTrip.Web.Data;
using BlazorTrip.Web.Facade;
using BlazorTrip.Web.Queries;
using BlazorTrip.Web.Repositories;
using BlazorTrip.Web.Services;

var culture = new CultureInfo("pt-BR");
culture.NumberFormat.CurrencySymbol = "R$ ";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddScoped<DataState>();
builder.Services.AddScoped<CsvService>();
builder.Services.AddScoped<ICategoryRepository, DataCategoryRepository>();
builder.Services.AddScoped<IPersonRepository, DataPersonRepository>();
builder.Services.AddScoped<ITransactionRepository, DataTransactionRepository>();


builder.Services.AddTransient<SelectTransactionDto>();
builder.Services.AddTransient<ReportFacade>();
builder.Services.AddTransient<CreateTransactionCommand>();

await builder.Build().RunAsync();