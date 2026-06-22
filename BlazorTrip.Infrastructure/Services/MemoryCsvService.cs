using System.Globalization;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Application.Services;
using BlazorTrip.Domain.Models;
using BlazorTrip.Infrastructure.InMemory;
using CommunityToolkit.Mvvm.Messaging;
using CsvHelper;
using CsvHelper.Configuration;

namespace BlazorTrip.Infrastructure.Services;

public class MemoryCsvService(InMemoryDaoContext dao, IMessenger messenger) : ICsvService
{
    public async Task<byte[]> ExportAsync()
    {
        using var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream);
        await using var csv = new CsvWriter(writer, new CsvConfiguration(new CultureInfo("pt-BR"))
        {
            Delimiter = "|"
        });

        csv.Context.RegisterClassMap<CsvDataMap>();
        var data = CreateCsvDataList();
        await csv.WriteRecordsAsync(data);
        await writer.FlushAsync();
        return stream.ToArray();
    }

    public async Task ImportAsync(StreamReader stream)
    {
        using var csv = new CsvReader(stream, new CsvConfiguration(new CultureInfo("pt-BR"))
        {
            Delimiter = "|"
        });

        csv.Context.RegisterClassMap<CsvDataMap>();
        var data = await csv.GetRecordsAsync<CsvData>().ToListAsync();
        var categories = data.ToCategoriesList();
        dao.Categories.Set(categories);

        var people = data.ToPeopleList();
        dao.People.Set(people);

        var transactions = CreateTransactionList(
            data,
            people.ToDictionary(s => s.Name),
            categories.ToDictionary(s => s.Name)
        );

        dao.Transactions.Set(transactions);
        messenger.Send(new CsvImportedMessage());
    }

    private List<CsvData> CreateCsvDataList()
    {
        var data =
            from t in dao.Transactions.Items
            join p in dao.People.Items on t.PayerId equals p.Id
            join c in dao.Categories.Items on t.CategoryId equals c.Id
            select new CsvData
            {
                Name = t.Name,
                Value = t.Amount,
                Category = c.Name,
                CategoryLogo = c.Logo,
                Payer = p.Name,
                SharedBy = string.Join(",", t.SharedIds
                    .Select(dao.People.Get)
                    .OfType<Person>()
                    .Select(s => s.Name))
            };

        return data.ToList();
    }

    private static List<Transaction> CreateTransactionList(
        List<CsvData> dataList,
        Dictionary<string, Person> people,
        Dictionary<string, Category> categories
    )
    {
        var transactions =
            from record in dataList
            join payer in people
                on record.Payer equals payer.Key
            join category in categories
                on record.Category equals category.Key
            select Transaction.Create(
                record.Name,
                record.Value,
                category.Value.Id,
                payer.Value.Id,
                record.GetPeopleIds(people)
            );

        return transactions.ToList();
    }
}