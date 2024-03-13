using Bogus;
using ElasticSearchDemo.WebApi.Models;

namespace ElasticSearchDemo.WebApi.Data;

public static class InMemoryData
{
    public static void SeedData()
    {
        Customers = GenerateCustomers();
    }

    public static List<Customer> Customers { get; private set; } = [];

    private static List<Customer> GenerateCustomers()
    {
        var authorFaker = new Faker<Customer>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Birthdate, f => f.Person.DateOfBirth);

        return authorFaker.Generate(15);
    }
}