using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Models
{
    public class BillObject
    {
        public BillObject()
        {
            ServiceInBillObject = new HashSet<ServiceInBillObject>();
            TickedObject = new HashSet<TickedObject>();
        }
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? SchedulingId { get; set; }
        public int? PaymentId { get; set; }
        public string CouponId { get; set; }
        public int? Total { get; set; }

        public virtual ICollection<ServiceInBillObject> ServiceInBillObject { get; set; }

        public virtual ICollection<TickedObject> TickedObject { get; set; }

    }
}
