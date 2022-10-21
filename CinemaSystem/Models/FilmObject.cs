using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Models
{
    public partial class FilmObject
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

        public int[] TypeInFilm { get; set; }

    }
}