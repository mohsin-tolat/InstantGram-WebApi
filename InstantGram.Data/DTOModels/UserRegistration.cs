using System.ComponentModel.DataAnnotations;

public class UserRegistration
{
    [Required(ErrorMessage = "Please enter valid Firstname")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter valid Lastname")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter valid Username")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Please enter valid Email Address")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Please enter valid Username")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Please Enter valid password. (Must be 8 character long, containing 1 Upper and Lower case & 1 Symbol.")]
    public string Password { get; set; }

    [Compare("Password",ErrorMessage="Password & Confirm password must be same")]
    public string ConfirmPassword { get; set; }
}