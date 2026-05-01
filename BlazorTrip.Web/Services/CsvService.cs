using System.Globalization;
using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Services;

public class CsvService(
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IJSRuntime jsRuntime
)
{
    public async Task Import(StreamReader reader)
    {
        try
        {
            using var csv = new CsvReader(reader, new CsvConfiguration(new CultureInfo("pt-BR"))
            {
                Delimiter = "|",
            });
            var records = await csv.GetRecordsAsync<TransactionCsv>().ToListAsync();
            var categories = records.Select(record => new CategoryNameAndLogo(record.Category, record.CategoryLogo))
                .Distinct()
                .Select(s => Category.Create(s.Name, s.Logo))
                .ToDictionary(k => k.Name);

            var people = records.Select(record => record.Payer)
                .Distinct()
                .Select(Person.Create)
                .ToDictionary(k => k.Name);

            var personNames = records.SelectMany(q => q.SharedBy.Split(','))
                .Where(q => !string.IsNullOrWhiteSpace(q))
                .Select(q => q.Trim())
                .ToList();

            foreach (var personName in personNames)
            {
                if (people.ContainsKey(personName)) continue;
                var newPerson = Person.Create(personName);
                people.Add(personName, newPerson);
            }

            var transactions = records.Select(s =>
            {
                var payerId = people.GetValueOrDefault(s.Payer)!.Id;
                var categoryId = categories.GetValueOrDefault(s.Category)!.Id;
                var sharedIds = s.SharedBy.Split(",").Distinct().Select(q => people.GetValueOrDefault(q.Trim())!.Id);
                return Transaction.Create(s.Name, s.Value, categoryId, payerId, sharedIds);
            });

            await personRepository.Import(people.Select(s => s.Value));
            await categoryRepository.Import(categories.Select(s => s.Value));
            await transactionRepository.Import(transactions);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task Export()
    {
        var data = transactionRepository.Transactions
            .AsEnumerable()
            .Select(s =>
            {
                var payerName = GetPersonNameById(s.PayerId);
                var sharedPersonName = string.Join(",", s.SharedIds.Select(GetPersonNameById));
                var categoryName = categoryRepository.GetById(s.CategoryId)!.Name;
                var categoryLogo = categoryRepository.GetById(s.CategoryId)!.Logo;
                return new TransactionCsv
                {
                    Name = s.Name,
                    Value = s.Amount,
                    Payer = payerName,
                    SharedBy = sharedPersonName,
                    Category = categoryName,
                    CategoryLogo = categoryLogo
                };
            });

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, new CsvConfiguration(new CultureInfo("pt-BR"))
        {
            Delimiter = "|",
        });
        await csv.WriteRecordsAsync(data);
        await writer.FlushAsync();
        var base64String = Convert.ToBase64String(stream.ToArray());
        await jsRuntime.InvokeVoidAsync("downloadCsv", "minha-viagem.csv", base64String);
    }

    private string GetPersonNameById(Guid personId)
    {
        return personRepository.GetById(personId)!.Name.Trim();
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

record CategoryNameAndLogo(string Name, string Logo);