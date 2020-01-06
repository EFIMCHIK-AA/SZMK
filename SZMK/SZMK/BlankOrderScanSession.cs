using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class BlankOrderScanSession
    {
        public String _QR;
        public bool _Unique;
        public String _QRBlankOrder;
        public BlankOrderScanSession(String QR, Boolean Unique,String QRBlankOrder)
        {
            if (!String.IsNullOrEmpty(QR))
            {
                _QR = QR;
            }
            else
            {
                throw new Exception("Пустое значение DataMatrix чертежа");
            }
            _Unique = Unique;
            if (!String.IsNullOrEmpty(QRBlankOrder))
            {
                _QRBlankOrder = QRBlankOrder;
            }
            else
            {
                throw new Exception("Пустое значение QR бланка заказа чертежа");
            }
        }
        public BlankOrderScanSession() : this("Нет QR", false,"Нет QR бланка заказа") { }
        public String QR
        {
            get
            {
                return _QR;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _QR = value;
                }
            }
        }
        public Boolean Unique
        {
            get
            {
                return _Unique;
            }
            set
            {
                _Unique = value;
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
