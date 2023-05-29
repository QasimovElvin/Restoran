using System.ComponentModel.DataAnnotations;

namespace Restoran.ViewModels.Account;

public class LoginVM
{
    public string EmailOrUsername { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
