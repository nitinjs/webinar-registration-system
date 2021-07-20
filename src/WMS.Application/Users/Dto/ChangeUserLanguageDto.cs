using System.ComponentModel.DataAnnotations;

namespace WMS.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}