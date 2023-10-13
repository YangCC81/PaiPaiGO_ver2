namespace PaiPaiGO.Models
{
    public class PendingVal
    {
        public class PaiVal
        {
            public string Orderclass { get; set; }
            public string QueueLabel { get; set; }
            public string PaiName { get; set; }
            public string Amount { get; set; }
            public string City { get; set; }
            public string Location { get; set; }
            public string Postcode { get; set; }

            public string Address { get; set; }
            public DateTime QueueTime { get; set; }
            public DateTime Deadline { get; set; }
            public string TaskContent { get; set; }
            public string Image { get; set; }
        }
        public class BuyVal
        {
            public string BuyOrderclass { get; set; }
            public string BuyLabel { get; set; }
            public string BuyName { get; set; }
            public string BuyLocation { get; set; }
            public string BuyAmount { get; set; }
            public DateTime BuyDelivery { get; set; }
            public DateTime BuyDeadline { get; set; }
            public string BuyCity { get; set; }
            public string BuyDistrict { get; set; }
            public string BuyPostcode { get; set; }
            public string BuyAddress { get; set; }
            public string BuyDeliveryMethod { get; set; }
            public string BuyTaskContent { get; set; }
            public string BuyImage { get; set; }
        }

    }
}
