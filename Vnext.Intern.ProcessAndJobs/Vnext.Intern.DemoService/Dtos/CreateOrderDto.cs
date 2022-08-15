using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class CreateOrderDto
    {
        public List<CustomerDto> customers { get; set; } = new List<CustomerDto>();
        public List<BillToDto> billTos { get; set; } = new List<BillToDto>();
        public List<ServiceDto> services { get; set; } = new List<ServiceDto>();
        public string notes { get; set; }
    }
}
