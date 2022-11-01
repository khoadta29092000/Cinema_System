namespace CinemaSystem.ViewModel
{
    public class SeatVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? RoomId { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
    }
}
