using System.ComponentModel.DataAnnotations;

namespace api_account.Data;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Hash { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

