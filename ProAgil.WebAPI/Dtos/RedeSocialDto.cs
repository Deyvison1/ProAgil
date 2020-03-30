using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class RedeSocialDto
    {
        public int Id                  { get; set; }
        [Required]
        public string Nome             { get; set; }
        [Required(ErrorMessage = "O campo {0} e Obrigatorio")]
        public string URL              { get; set; }
    }
}