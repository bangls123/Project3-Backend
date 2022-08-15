using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class BillToDto
    {
        public string customerCode { get; set; }
        public string currencyCode { get; set; }
        public List<ContactDto> contacts { get; set; } = new List<ContactDto>();
        public List<ReferenceNumberDto> referenceNumbers { get; set; } = new List<ReferenceNumberDto>();
    }
}
