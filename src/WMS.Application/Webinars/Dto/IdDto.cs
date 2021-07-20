using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Webinars.Dto
{
    public class IdDto<T>
    {
        public IdDto(){}
        public IdDto(T id) {
            this.Id = id;
        }
        public T Id { get; set; }
    }
}
