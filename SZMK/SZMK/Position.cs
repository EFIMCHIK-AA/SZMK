using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Position : BasePosition
    {
        private String _Name;
        private Int32 _ID;

        public Position(Int32 ID, String Name) : base(ID)
        {
            if (!String.IsNullOrEmpty(Name))
            {
                _Name = Name;
            }
        }

        public Position() : this(0, "Без наименования позиции") { }

        public override Int32 ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (value >= 0)
                {
                    _ID = value;
                }
            }
        }

        public String Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Name = value;
                }
            }
        }
    }
}
