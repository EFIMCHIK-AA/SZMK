using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public abstract class BasePosition
    {
        abstract public Int32 ID { get; set; }

        public BasePosition(Int32 ID)
        {
            this.ID = ID;
        }

        public BasePosition() : this(-1) { }
    }
}
