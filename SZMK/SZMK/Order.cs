using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    /*Класс реализует объек чертежа со всеми полями присущими чертежу*/
    public class Order
    {
        private Int64 _ID;
        private String _DataMatrix;
        private DateTime _DateCreate;
        private String _Number;
        private String _Executor;
        private String _List;
        private String _Mark;
        private Double _Lenght;
        private Double _Weight;
        private Status _Status;
        private User _User;
        private BlankOrder _BlankOrder;
        private Boolean _Canceled;
        private DateTime _StatusDate;


        public Order(Int64 ID, String DataMatrix, DateTime DateCreate, String Number, String Executor, String List, String Mark, Double Lenght, Double Weight, Status Status,DateTime StatusDate,User User, BlankOrder BlankOrder, Boolean Canceled)
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

            if (!String.IsNullOrEmpty(List))
            {
                _List = List;
            }
            else
            {
                throw new Exception("Пустое значение листа");
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

            if (Status != null)
            {
                _Status = Status;
            }
            else
            {
                throw new Exception("Пустое значение статуса");
            }

            if (StatusDate != null)
            {
                _StatusDate = StatusDate;
            }
            else
            {
                throw new Exception("Пустое значение даты присвоения статуса");
            }

            if (User != null)
            {
                _User = User;
            }
            else
            {
                throw new Exception("Пустое значение бланка заказа");
            }

            if (BlankOrder != null)
            {
                _BlankOrder = BlankOrder;
            }
            else
            {
                throw new Exception("Пустое значение бланка заказа");
            }

            _Canceled = Canceled;
        }

        public Order() : this(-1, "Нет DataMatrix", DateTime.Now, "Нет номера заказа", "Нет исполнителя", "Нет листа", "Нет марки", -1, -1, null, DateTime.Now, null, null, false) { }

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

        public String List
        {
            get
            {
                return _List;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
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

        public DateTime StatusDate
        {
            get
            {
                return _StatusDate;
            }
            set
            {
                if(value!=null)
                {
                    _StatusDate = value;
                }
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

        public Boolean Canceled
        {
            get
            {
                return _Canceled;
            }
            set
            {
                _Canceled = value;
            }
        }
        public String SearchString() => $"{_DataMatrix}_{_Status.Name}_{_BlankOrder}_{_DateCreate.ToString()}_{_User.Name}_{_User.MiddleName}_{_User.Surname}_{SystemArgs.StatusOfOrders.Where(p => p.IDOrder == _ID && p.IDStatus == _Status.ID).Select(p => p.DateCreate).ToString()}";
    }
}
