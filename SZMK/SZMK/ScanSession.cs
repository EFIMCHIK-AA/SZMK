using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ScanSession
    {
        public String _DateMatrix;
        public bool _Unique;
        public ScanSession(String DataMatrix, Boolean Unique)
        {
            if(!String.IsNullOrEmpty(DataMatrix))
            {
                _DateMatrix = DataMatrix;
            }
            else
            {
                throw new Exception("Пустое значение DataMatrix чертежа");
            }
            _Unique = Unique;
        }
        public ScanSession() : this("Нет DataMatrix", false) { }
        public String DataMatrix
        {
            get
            {
                return _DateMatrix;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _DateMatrix = value;
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

    }
}
