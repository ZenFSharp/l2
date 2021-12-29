using System;
using System.ComponentModel.DataAnnotations;

namespace api_account.Account;

public class AccountModel
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }
}
