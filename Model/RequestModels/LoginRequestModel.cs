using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class LoginRequestModel
    {
            [Required, MaxLength(250)]
            public string UserName { get; set; }
            [Required, MaxLength(250), DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
