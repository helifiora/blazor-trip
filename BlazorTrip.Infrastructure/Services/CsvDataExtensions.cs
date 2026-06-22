using BlazorTrip.Domain.Models;

namespace BlazorTrip.Infrastructure.Services;

internal static class CsvDataExtensions
{
    private record CategoryNameAndLogo(string Name, string Logo);

    extension(CsvData data)
    {
        public List<Guid> GetPeopleIds(Dictionary<string, Person> people)
        {
            return data.SharedByList
                .Distinct()
                .Select(people.GetValueOrDefault)
                .OfType<Person>()
                .Select(s => s.Id)
                .ToList();
        }
    }

    extension(List<CsvData> dataList)
    {
        public List<Category> ToCategoriesList()
        {
            return dataList
                .Select(s => new CategoryNameAndLogo(s.Category, s.CategoryLogo))
                .Distinct()
                .Select(s => Category.Create(s.Name, s.Logo))
                .ToList();
        }

        public List<Person> ToPeopleList()
        {
            return dataList
                .Select(s => s.Payer.Trim())
                .Concat(dataList.SelectMany(x => x.SharedByList))
                .Distinct()
                .Select(Person.Create)
                .ToList();
        }
    }
}