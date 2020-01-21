using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Specific
    {
        private String _Number;
        private Int64 _List;
        private Int64 _NumberSpecific;
        private Boolean _Finded;
        public Specific(String Number, Int64 List, Int64 NumberSpecific, Boolean Finded)
        {
            if (!String.IsNullOrEmpty(Number))
            {
                _Number = Number;
            }
            else
            {
                throw new Exception("Не задан номер заказа");
            }
            if (List >= 0)
            {
                _List = List;
            }
            else
            {
                throw new Exception("Номер листа заказа меньше 0");
            }
            if (NumberSpecific >= 0)
            {
                _NumberSpecific = NumberSpecific;
            }
            else
            {
                throw new Exception("Номер детали меньше 0");
            }
            _Finded = Finded;
        }
        public String Number
        {
            get
            {
                return _Number;
            }
            set
            {
                if (!String.IsNullOrEmpty(Number))
                {
                    _Number = value;
                }
            }
        }
        public Int64 List
        {
            get
            {
                return _List;
            }
            set
            {
                if (value >= 0)
                {
                    _List = value;
                }
            }
        }
        public Int64 NumberSpecific
        {
            get
            {
                return _NumberSpecific;
            }
            set
            {
                if (value >= 0)
                {
                    _NumberSpecific = value;
                }
            }
        }
        public String FindedView
        {
            get
            {
                if (_Finded)
                {
                    return "Найдено";
                }
                else
                {
                    return "Не найдено";
                }
            }
        }
        public Boolean Finded
        {
            get
            {
                return _Finded;
            }
            set
            {
                _Finded = value;
            }
        }
    }
}
