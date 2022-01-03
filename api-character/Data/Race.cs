namespace api_character.Data;

public class Race
{
    public int Id { get; set; }
    public int Str { get; set; }
    public int Dex { get; set; }
    public int Con { get; set; }
    public int Int { get; set; }
    public int Wit { get; set; }
    public int Men { get; set; }
    public string Name { get; set; }

    public Race()
    {
        Name = string.Empty;
    }
}
