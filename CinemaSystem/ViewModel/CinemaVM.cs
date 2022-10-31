namespace CinemaSystem.ViewModel
{
    public class CinemaVM
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int? LocationId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string Name { get; set; }
    }
}
