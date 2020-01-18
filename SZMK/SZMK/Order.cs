using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Order
    {
        private Int64 _ID;
        private String _DataMatrix;
        private DateTime _DateCreate;
        private String _Number;
        private String _Executor;
        private Int64 _List;
        private String _Mark;
        private Double _Lenght;
        private Double _Weight;
        private Status _Status;
        private User _User;
        private BlankOrder _BlankOrder;


        public Order(Int64 ID, String DataMatrix, DateTime DateCreate, String Number, String Executor, Int64 List, String Mark, Double Lenght, Double Weight, Status Status,User User, BlankOrder BlankOrder)
        {
            if (ID >= 0)
            {
                _ID = ID;
            }

            if (!String.IsNullOrEmpty(DataMatrix))
            {
                _DataMatrix = DataMatrix;
            }
            else
            {
                throw new Exception("Пустое значение DataMatrix");
            }

            if (DateCreate != null)
            {
                _DateCreate = DateCreate;
            }
            else
            {
                throw new Exception("Пустое значение Даты добавления");
            }

            if (!String.IsNullOrEmpty(Number))
            {
                _Number = Number;
            }
            else
            {
                throw new Exception("Пустое значение Номера заказа");
            }

            if (!String.IsNullOrEmpty(Executor))
            {
                _Executor = Executor;
            }
            else
            {
                throw new Exception("Пустое значение Исполнителя");
            }

            if (List >= 0)
            {
                _List = List;
            }
            else
            {
                throw new Exception("Значение листа меньше 0");
            }

            if (!String.IsNullOrEmpty(Mark))
            {
                _Mark = Mark;
            }
            else
            {
                throw new Exception("Пустое значение Марки");
            }

            if (Lenght >= 0)
            {
                _Lenght = Lenght;
            }
            else
            {
                throw new Exception("Значение длинны меньше 0");
            }

            if (Weight >= 0)
            {
                _Weight = Weight;
            }
            else
            {
                throw new Exception("Значение веса меньше 0");
            }
            if (Status!=null)
            {
                _Status = Status;
            }
            else
            {
                throw new Exception("Пустое значение статуса");
            }
            if (User != null)
            {
                _User = User;
            }
            else
            {
                throw new Exception("Пустое значение бланка заказа");
            }
            if (BlankOrder!=null)
            {
                _BlankOrder = BlankOrder;
            }
            else
            {
                throw new Exception("Пустое значение бланка заказа");
            }
        }

        public Order() : this(-1, "Нет DataMatrix", DateTime.Now, "Нет номера заказа", "Нет исполнителя", -1, "Нет марки", -1, -1, null,null,null) { }

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

        public String DataMatrix
        {
            get
            {
                return _DataMatrix;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _DataMatrix = value;
                }
            }
        }

        public DateTime DateCreate
        {
            get
            {
                return _DateCreate;
            }
            set
            {
                if (value != null)
                {
                    _DateCreate = value;
                }
            }
        }

        public String Executor
        {
            get
            {
                return _Executor;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Executor = value;
                }
            }
        }

        public String Number
        {
            get
            {
                return _Number;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
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

        public String Mark
        {
            get
            {
                return _Mark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Mark = value;
                }
            }
        }

        public Double Lenght
        {
            get
            {
                return _Lenght;
            }
            set
            {
                if (value >= 0)
                {
                    _Lenght = value;
                }
            }
        }

        public Double Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                if (value >= 0)
                {
                    _Weight = value;
                }
            }
        }
        public Status Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (value!=null)
                {
                    _Status = value;
                }
            }
        }
        public String StatusView
        {
            get
            {
                return _Status.ToString();
            }
        }
        public User User
        {
            get
            {
                return _User;
            }
            set
            {
                if (value != null)
                {
                    _User = value;
                }
            }
        }
        public String BlankOrderView
        {
            get
            {
                if (_BlankOrder.QR.Split('_').Length >= 3)
                {
                    return _BlankOrder.QR.Split('_')[1];
                }
                else
                {
                    return _BlankOrder.QR;
                }
            }
        }
        public BlankOrder BlankOrder
        {
            get
            {
                return _BlankOrder;
            }
            set
            {
                if (value != null)
                {
                    _BlankOrder = value;
                }
            }
        }
        public String SearchString() => $"{_DataMatrix}_{_Status.Name}_{_BlankOrder}_{_DateCreate.ToShortDateString()}_{_User.Name}_{_User.MiddleName}_{_User.Surname}";
    }
}
