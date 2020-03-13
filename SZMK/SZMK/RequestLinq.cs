using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public class RequestLinq
    {
        public bool CompareBlankOrder(List<Order> Orders, String QR)
        {
            try
            {
                if (SystemArgs.Request.GetIDBlankOrder(QR)!=-1)
                {
                    if (SystemArgs.Request.InsertBlankOrderOfOrders(Orders, QR))
                    {
                        return true;
                    }
                }
                else if (GetOldIDBlankOrder(Orders)!=-1)
                {
                    if (SystemArgs.Request.UpdateBlankOrder(QR,Orders))
                    {
                        SystemArgs.BlankOrders.Clear();
                        SystemArgs.Request.GetAllBlankOrder();
                        if (SystemArgs.Request.InsertBlankOrderOfOrders(Orders, QR))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (SystemArgs.Request.InsertBlankOrder(QR))
                    {
                        SystemArgs.BlankOrders.Clear();
                        SystemArgs.Request.GetAllBlankOrder();
                        if (SystemArgs.Request.GetIDBlankOrder(QR) != -1)
                        {
                            if (SystemArgs.Request.InsertBlankOrderOfOrders(Orders, QR))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool SelectOrderInBlankOrder(Int64 IDOrder)
        {
            try
            {
                if (SystemArgs.BlankOrderOfOrders.Where(p => p.IDOrder == IDOrder).Count() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }
        public Int64 GetOldIDBlankOrder(List<Order> Orders)
        {
            try
            {
                foreach (Order order in Orders)
                {
                    var Temp = SystemArgs.BlankOrderOfOrders.Where(p => p.IDOrder == order.ID).ToList();
                    if (Temp.Count() == 1)
                    {
                        return Temp[0].IDBlankOrder;
                    }
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }
        public Int32 CheckedNumberAndList(String Number, String List)
        {
            try
            {
                String[] Temp = List.Split('и');
                if (SystemArgs.Orders.Where(p => p.Number == Number && p.List == List).Count() == 0)
                {
                    if (Temp.Length != 1)
                    {
                        if (SystemArgs.Orders.Where(p => p.Number == Number && (p.List == Temp[0]||p.List.IndexOf(Temp[0]+"и")==0)).Count() == 0)
                        {
                            if (MessageBox.Show("Заменяемый чертеж отсутсвует. Добавить новый?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                return 2;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch 
            {
                return -1;
            }
        }
    }
}
