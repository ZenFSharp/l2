namespace api_character.Data;

public class Occupation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Occupation()
    {
        Name = string.Empty;
    }
}
