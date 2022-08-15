using System.Collections.Generic;

namespace Vnext.Intern.Utility.Dtos
{
    public class PageResultDto<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }
    }
}

