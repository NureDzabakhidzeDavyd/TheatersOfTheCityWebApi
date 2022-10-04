namespace TheatersOfTheCity.Core.Domain;

public class Performance : BaseEntity
{
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public string Duration { get; set; }

    public string Language { get; set; }

    public IEnumerable<Contact> Actors { get; set; }

    public IEnumerable<Contact> Directors { get; set; }
}