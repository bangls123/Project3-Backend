using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class LocationDto
    {
        public string customerLocationId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public AddressDto address { get; set; }

        public List<ReferenceNumberDto> referenceNumbers { get; set; } = new List<ReferenceNumberDto>();
    }
}
