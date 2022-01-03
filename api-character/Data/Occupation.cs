namespace api_character.Data;

public class Occupation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Occupation()
    {
        Name = string.Empty;
    }
}
