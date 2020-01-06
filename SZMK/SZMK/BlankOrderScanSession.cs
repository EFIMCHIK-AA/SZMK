using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class BlankOrderScanSession
    {
        public struct NumberAndList
        {
            public String _Number;
            public Int64 _List;
            public bool _Finded;

            public NumberAndList(String Number, Int64 List,bool Finded)
            {
                _Number = Number;
                _List = List;
                _Finded = Finded;
            }
        }
        private bool _Added;
        private String _QRBlankOrder;
        public List<NumberAndList> _Order;
        public BlankOrderScanSession(Boolean Added, String QRBlankOrder)
        {
            _Added = Added;
            if (!String.IsNullOrEmpty(QRBlankOrder))
            {
                _QRBlankOrder = QRBlankOrder;
            }
            else
            {
                throw new Exception("Пустое значение QR бланка заказа чертежа");
            }
            _Order = new List<NumberAndList>();
        }
        public BlankOrderScanSession() : this(false,"Нет QR бланка заказа") { }

        public Boolean Added
        {
            get
            {
                return _Added;
            }
            set
            {
                _Added = value;
            }
        }
        public String QRBlankOrder
        {
            get
            {
                return _QRBlankOrder;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _QRBlankOrder = value;
                }
            }
        }
    }
}
