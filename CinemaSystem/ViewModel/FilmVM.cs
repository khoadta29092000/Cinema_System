using BusinessObject.Models;
using System.Collections.Generic;

namespace CinemaSystem.ViewModel
{
    public class FilmVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
        public int? Time { get; set; }
        public string Language { get; set; }
        public int? Rated { get; set; }
        public string Trailer { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<int> TypeInFilms { get; set; }
    }
}
