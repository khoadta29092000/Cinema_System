using System;

namespace CinemaSystem.ViewModel
{
    public class BillVM
    {
        public int Id { get; set; }
        public bool Checking { get; set; }
        public int? SchedulingId { get; set; }
        public int? PaymentId { get; set; }
        public string CouponId { get; set; }
        public DateTime? Time { get; set; }
        public int? Total { get; set; }
    }
}
