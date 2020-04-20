using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Analysis;
using Tekla.Structures.Model;
using tsd = Tekla.Structures.Drawing;

namespace SZMK
{
    class Tekla
    {
        public void GetData()
        {
            var CurrentDrawingHandler = new tsd.DrawingHandler(); //получаем доступ к чертежам

            var drawingsEnum = CurrentDrawingHandler.GetDrawingSelector().GetSelected(); //получение чертежей, выбранных пользователем

            while (drawingsEnum.MoveNext()) //перебираем перебираем полученый список
            {
                var _drawing = drawingsEnum.Current as tsd.Drawing;  //получили доступ к конкретному чертежу.
                string some_string = string.Empty;
                _drawing.GetUserProperty("Default", ref some_string);//сделаем с ним что нибудь, например получим UDA.

                MessageBox.Show(some_string); //И выведем его, чтобы убедится что все работает.
            }
        }
    }
}
