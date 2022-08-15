using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class ContactDto
    {
        public string name { get; set; }
        public string type { get; set; }
        public string companyName { get; set; }
        public List<ContactMethodDto> contactMethods { get; set; } = new List<ContactMethodDto>();
    }
}
