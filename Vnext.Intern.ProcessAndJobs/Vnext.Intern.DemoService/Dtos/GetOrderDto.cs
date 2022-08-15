namespace CHRobinsonCreateOrderService.Dtos
{
    public class GetOrderDto
    {
        public string SONumber { get; set; }
        public string SONumberTrim { get; set; }
        public string PONumber { get; set; }
        public string AgentId { get; set; }
        public string OriginCompany { get; set; }
        public string OriginAddress { get; set; }
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginZip { get; set; }
        public string OriginCountry { get; set; }
        public string OriginWcode { get; set; }
        public string DestinationCompany { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZip { get; set; }
        public int Qty { get; set; }
        public decimal WeightPerUnit { get; set; }
        public int OrderId { get; set; }
        public string DestinationCountry { get; set; }
        public int RepNo { get; set; }
        public decimal InsuranceValue { get; set; }
    }
}
