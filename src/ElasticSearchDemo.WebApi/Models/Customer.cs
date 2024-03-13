namespace ElasticSearchDemo.WebApi.Models;

public sealed class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    private DateTime birthdate;

    public DateTime Birthdate
    {
        get
        {
            return birthdate;
        }
        set
        {
            birthdate = value;
            Age = CalculateAge();
        }
    }

    public int Age { get; private set; }

    private int CalculateAge()
    {
        DateTime today = DateTime.UtcNow;
        int age = today.Year - birthdate.Year;

        if (birthdate.DayOfYear > today.DayOfYear)
        {
            age--;
        }

        return age;
    }
}