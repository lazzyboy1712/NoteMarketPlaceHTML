using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notesmarketplace.Models
{
    public class dashboard
    {
        public int ID { get; set; }
        public int F_K_User { get; set; }
        public int UnderReviewNotes { get; set; }
        public int PublishedNotes { get; set; }
        public int DownloadedNotes { get; set; }
        public int TotalExpensis { get; set; }
        public int TotalEarning { get; set; }
        public int BuyerRequests { get; set; }
        public int SoldNotes { get; set; }
        public int RejectedNotes { get; set; }

        public virtual User User { get; set; }
    }
}