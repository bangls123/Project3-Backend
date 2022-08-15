using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class CustomerDto
    {
        public string customerCode { get; set; }
        public List<ContactDto> contacts { get; set; } = new List<ContactDto>();
        public List<ReferenceNumberDto> referenceNumbers { get; set; } = new List<ReferenceNumberDto>();
    }
}
