using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Status
    {
        private Int64 _ID;
        private Int64 _IDPosition;
        private String _Name;
        public Status(Int64 ID,Int64 IDPosition, String Name)
        {
            if (ID >= 0)
            {
                _ID = ID;
            }
            if (IDPosition >= 0)
            {
                _IDPosition = IDPosition;
            }
            else
            {
                throw new Exception("Значение id позиции меньше 0");
            }

            if (!String.IsNullOrEmpty(Name))
            {
                _Name = Name;
            }
            else
            {
                throw new Exception("Пустое значение наименования статуса");
            }
        }
        public Status() : this(-1,-1,"Нет наименования статуса") { }
        public Int64 ID
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
        public Int64 IDPosition
        {
            get
            {
                return _IDPosition;
            }
            set
            {
                if (value >= 0)
                {
                    _IDPosition = value;
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
