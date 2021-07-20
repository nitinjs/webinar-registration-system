using System.ComponentModel.DataAnnotations;

namespace TalkBack.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}