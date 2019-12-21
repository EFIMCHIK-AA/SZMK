using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class User : IPositionToUser
    {
        private Int64 _ID;
        private String _Name;
        private String _MiddleName;
        private String _Surname;
        private DateTime _DateCreate;
        private DateTime _DateOfBirth;
        private List<Mail> _Mails;
        private Position _Position;


        public User (Int64 ID, String Name, String MiddleName, String Surname, DateTime DateCreate, DateTime DateOfBirth, Int64 IDPosition, List<Int64> ListMails)
        {
            _ID = ID;

            if(!String.IsNullOrEmpty(Name))
            {
                _Name = Name;
            }
            else
            {
                throw new Exception("Получено пустое значение имени пользователя");
            }

            if (!String.IsNullOrEmpty(MiddleName))
            {
                _MiddleName = MiddleName;
            }
            else
            {
                throw new Exception("Получено пустое значение отчетства пользователя");
            }

            if (!String.IsNullOrEmpty(Surname))
            {
                _Surname = Surname;
            }
            else
            {
                throw new Exception("Получено пустое значение фамилии пользователя");
            }

            if(DateCreate != null)
            {
                _DateCreate = DateCreate;
            }

            if(DateOfBirth != null)
            {
                _DateOfBirth = DateOfBirth;
            }

            _Mails = new List<Mail>();

            SetMails(ListMails);

            if(!SetPosition(IDPosition))
            {
                throw new Exception("Получено пустое значение должности пользователя");
            }
        }

        public User() : this(-1,"Нет имени", "Нет отчества", "Нет фамилии", DateTime.Now, DateTime.Now, -1, null) { }

        public Position GetPosition()
        {
            if(_Position != null)
            {
                return _Position;
            }
            else
            {
                throw new Exception("Пользвоателю не присвоена должность");
            }
        }

        private bool SetPosition(Int64 IDPosition)
        {
            foreach(Position Temp in SystemArgs.Positions)
            {
                if(Temp.ID == IDPosition)
                {
                    _Position = Temp;
                    return true;
                }
            }

            return false;
        }

        private void SetMails(List<Int64> ListMails)
        {
            for(Int32 i = 0; i < SystemArgs.Mails.Count; i++)
            {
                for(Int32 j = 0; j < ListMails.Count; j++)
                {
                    if(SystemArgs.Mails[i].ID == ListMails[j])
                    {
                        _Mails.Add(SystemArgs.Mails[i]);
                    }
                }
            }
        }

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

        public String Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    _Name = value;
                }
            }
        }

        public String Surname
        {
            get
            {
                return _Surname;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Surname = value;
                }
            }
        }

        public String MiddleName
        {
            get
            {
                return _MiddleName;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _MiddleName = value;
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
                if(value != null)
                {
                    _DateCreate = value;
                }
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }

            set
            {
                if (value != null)
                {
                    _DateOfBirth = value;
                }
            }
        }

        public Mail this[int index]
        {
            get
            {
                return _Mails[index];
            }

            set
            {
                _Mails[index] = value;
            }
        }

        public Int64 MailsCount
        {
            get
            {
                return _Mails.Count;
            }
        }
    }
}
