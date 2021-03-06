namespace api_character.Data;

public class Character
{
    public int Id { get; set; }
    public int RaceId { get; set; }
    public int UserId { get; set; }
    public int OccupationId { get; set; }
    public int Ex { get; set; }
    public int Sp { get; set; }
    public string Name { get; set; }
    public DateTime LogoutOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public Character()
    {
        Name = string.Empty;
    }
}
