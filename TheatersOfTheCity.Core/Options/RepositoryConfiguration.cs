namespace TheatersOfTheCity.Core.Options;

public class RepositoryConfiguration
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Database { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string DbConnection => $"server={Host};port={Port};database={Database};uid={User};pwd={Password}";
}