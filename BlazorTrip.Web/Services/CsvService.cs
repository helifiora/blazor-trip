using System.Globalization;
using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using BlazorTrip.Web.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace BlazorTrip.Web.Services;

public class CsvService(
    DataState state,
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository
)
{
    public async Task Import(StreamReader reader)
    {
        try
        {
            using var csv = new CsvReader(reader, GetConfig());
            var records = await csv.GetRecordsAsync<TransactionCsv>().ToListAsync();

            var categories = TransformRecordsToCategories(records);
            var people = TransformRecordsToPeople(records);
            var transactions = TransformRecordsToTransactions(records, people, categories);

            await categoryRepository.Import(categories.Select(s => s.Value));
            await personRepository.Import(people.Select(s => s.Value));
            await transactionRepository.Import(transactions);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<byte[]> Export()
    {
        var data = transactionRepository
            .Transactions
            .AsEnumerable()
            .Select(s =>
            {
                var payerName = GetPersonNameById(s.PayerId);
                var sharedPersonName = string.Join(",", s.SharedIds.Select(GetPersonNameById));
                var category = state.Categories.Get(s.CategoryId);
                return new TransactionCsv
                {
                    Name = s.Name,
                    Value = s.Amount,
                    Payer = payerName,
                    SharedBy = sharedPersonName,
                    Category = category.Name,
                    CategoryLogo = category.Logo
                };
            });

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, GetConfig());
        await csv.WriteRecordsAsync(data);
        await writer.FlushAsync();
        return stream.ToArray();
    }

    private string GetPersonNameById(Guid personId)
    {
        return personRepository.GetById(personId)!.Name.Trim();
    }

    private static CsvConfiguration GetConfig() => new(new CultureInfo("pt-BR"))
    {
        Delimiter = "|",
    };

    private static Dictionary<string, Category> TransformRecordsToCategories(List<TransactionCsv> records)
    {
        return records
            .Select(s => new CategoryNameAndLogo(s.Category, s.CategoryLogo))
            .Distinct()
            .Select(s => Category.Create(s.Name, s.Logo))
            .ToDictionary(k => k.Name);
    }

    private static Dictionary<string, Person> TransformRecordsToPeople(List<TransactionCsv> records)
    {
        var sharedPersonNames = records
            .SelectMany(x => x.SharedBy.Split(','))
            .Where(q => !string.IsNullOrWhiteSpace(q));

        return records
            .Select(s => s.Payer)
            .Concat(sharedPersonNames)
            .Select(s => s.Trim())
            .Distinct()
            .Select(Person.Create)
            .ToDictionary(k => k.Name);
    }

    private static List<Transaction> TransformRecordsToTransactions(
        List<TransactionCsv> records,
        Dictionary<string, Person> people,
        Dictionary<string, Category> categories
    )
    {
        return records
            .Join(people, s => s.Payer, s => s.Key, (record, person) => new { Record = record, Person = person.Value })
            .Join(categories, s => s.Record.Category, s => s.Key, (grouped, category) => Transaction.Create(
                grouped.Record.Name,
                grouped.Record.Value,
                category.Value.Id,
                grouped.Person.Id,
                TransformSharedByToPeopleIds(grouped.Record, people)
            )).ToList();
    }

    private static List<Guid> TransformSharedByToPeopleIds(
        TransactionCsv record,
        Dictionary<string, Person> people
    )
    {
        return record.SharedBy
            .Split(',')
            .Select(s => s.Trim())
            .Distinct()
            .Select(people.GetValueOrDefault)
            .OfType<Person>()
            .Select(s => s.Id)
            .ToList();
    }
}

[CultureInfo("pt-BR")]
class TransactionCsv
{
    [Index(0)] [Name("Nome")] public string Name { get; set; }

    [Index(1)] [Name("Valor")] public decimal Value { get; set; }

    [Index(2)] [Name("Pagador")] public string Payer { get; set; }

    [Index(3)] [Name("Dividido por")] public string SharedBy { get; set; }

    [Index(4)] [Name("Categoria")] public string Category { get; set; }

    [Index(5)] [Name("Categoria Logo")] public string CategoryLogo { get; set; }
}

internal record CategoryNameAndLogo(string Name, string Logo);