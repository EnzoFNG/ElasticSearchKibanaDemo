namespace ElasticSearchDemo.WebApi.Requests;

public sealed record CustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
}