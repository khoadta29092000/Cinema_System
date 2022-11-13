using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public partial class ServiceInCinemaDTO
    {
        public int Id { get; set; }
        public int ServiceInCinemaId { get; set; }
        public string Title { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityInCinema { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string Image { get; set; }
    }
}
