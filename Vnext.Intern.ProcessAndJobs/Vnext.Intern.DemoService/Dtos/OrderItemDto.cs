using System.Collections.Generic;

namespace CHRobinsonCreateOrderService.Dtos
{
    public class OrderItemDto
    {
        public string packagingType { get; set; }
        public decimal insuranceValue { get; set; }
        public string freightClass { get; set; }
        public int quantity { get; set; }
        public decimal weight { get; set; }
        public string weightUnitOfMeasure { get; set; }

        public List<ReferenceNumberDto> referenceNumbers { get; set; } = new List<ReferenceNumberDto>();
    }
}
