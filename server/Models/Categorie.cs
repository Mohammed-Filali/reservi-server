using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Profetionnal> Profetionnals { get; set; } = new List<Profetionnal>();

    }
}
