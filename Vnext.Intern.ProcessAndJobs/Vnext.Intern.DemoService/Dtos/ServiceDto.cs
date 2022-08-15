using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class ServiceDto
    {
        public List<LocationDto> locations { get; set; } = new List<LocationDto>();
        public List<OrderItemDto> items { get; set; } = new List<OrderItemDto>();
    }
}
