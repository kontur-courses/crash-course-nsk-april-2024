using System.ComponentModel.DataAnnotations;

namespace Market.DTO;

public class CreateUserRequestDto
{
    [Required] 
    [StringLength(100)] 
    public string Name { get; set; }

    [Required]
    [RegularExpression("^[A-Za-z0-9]{0,100}$")]
    public string Login { get; set; }

    [Required] 
    [StringLength(100)] 
    public string Password { get; set; }
}