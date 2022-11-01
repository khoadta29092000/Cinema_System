namespace CinemaSystem.ViewModel
{
    public class RoomVM
    {
        public int Id { get; set; }
        public int? CinemaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
    }
}
