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
        public Int64 GetIDOrder(String DataMatrix)
        {
            return SystemArgs.Orders.Where(j => j.DataMatrix == DataMatrix).Select(j => j.ID).Single();
        }
        public String GetDataMatrixIsNumberAndList(String Number,String List)
        {
            return SystemArgs.Orders.Where(p => p.Number == Number && p.List == List).Select(p => p.DataMatrix).Single();
        }
        public bool CheckedOrderAndStatus(String Number, String List)
        {
            try
            {
                if (SystemArgs.Orders.Where(p => p.Number == Number && p.List == List).Count() == 1 && CheckedStatusOrderDB(SystemArgs.User.IDStatus,GetDataMatrixIsNumberAndList(Number,List)) == 1)
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
        public Int32 CheckedStatusOrderDB(Int64 IDStatus, String DataMatrix)
        {
            try
            {
                if(SystemArgs.StatusOfOrders.Where(p => p.IDOrder == GetIDOrder(DataMatrix)).Max(p => p.IDStatus) == IDStatus - 1)
                {
                    return 1;
                }
                else if(SystemArgs.StatusOfOrders.Where(p => p.IDOrder == GetIDOrder(DataMatrix)).Max(p => p.IDStatus) >= IDStatus)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }
        public bool CheckedOrderAndStatusForUpdate(String Number, String List)
        {
            try
            {
                if (CheckedStatusExistingOrder(GetDataMatrixIsNumberAndList(Number,List)))
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
        public bool CheckedStatusExistingOrder(String DataMatrix)
        {
            try
            {
                if (SystemArgs.StatusOfOrders.Where(p => p.IDOrder == GetIDOrder(DataMatrix)).Max(p => p.IDStatus) > SystemArgs.User.IDStatus - 1)
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
        public bool CheckedNumberAndMark(String Number, String Mark)
        {
            try
            {
                if (SystemArgs.Orders.Where(p => p.Number == Number && p.Mark == Mark).Count() == 0)
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
        public bool CompareBlankOrder(List<Order> Orders, String QR)
        {
            try
            {
                if (GetIDBlankOrder(QR)!=-1)
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
                        if (GetIDBlankOrder(QR) != -1)
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
        public Int64 GetIDBlankOrder(String QR)
        {
            try
            {

                return SystemArgs.BlankOrders.Where(p => p.QR == QR).Select(p => p.ID).Single();
            }
            catch
            {
                return -1;
            }
        }
        public bool FindedOrdersInAddBlankOrder(String QR, String Number, String List)
        {
            try
            {
                if (SystemArgs.BlankOrderOfOrders.Where(p => p.IDBlankOrder == GetIDBlankOrder(QR) && p.IDOrder == GetIDOrder(GetDataMatrixIsNumberAndList(Number, List))).Count() == 1)
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
                    if (SystemArgs.BlankOrderOfOrders.Where(p => p.IDOrder == order.ID).Count() == 1)
                    {
                        return SystemArgs.BlankOrderOfOrders.Where(p => p.IDOrder == order.ID).Select(p => p.IDBlankOrder).Single();
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
