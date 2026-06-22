using BlazorTrip.Domain.Models;
using CsvHelper.Configuration;

namespace BlazorTrip.Infrastructure.Services;

public sealed class CsvDataMap : ClassMap<CsvData>
{
    public CsvDataMap()
    {
        Map(m => m.Name).Index(0).Name("Nome");
        Map(m => m.Value).Index(1).Name("Valor");
        Map(m => m.Payer).Index(2).Name("Pagador");
        Map(m => m.SharedBy).Index(3).Name("Dividido por");
        Map(m => m.Category).Index(4).Name("Categoria");
        Map(m => m.CategoryLogo).Index(5).Name("Categoria Logo");
    }
}