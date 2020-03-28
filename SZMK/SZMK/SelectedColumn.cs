using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SZMK
{
    public class SelectedColumn
    {
        Boolean[] _Visible;
        Int32[] _DisplayIndex;
        float[] _FillWeight;
        public SelectedColumn()
        {
            if (CheckFile())
            {
                if (!GetParametrColumn())
                {
                    throw new Exception("Ошибка при чтении настроек столбцов");
                }
            }
            else
            {
                throw new Exception("Файл настроек столбцов не найден");
            }
        }

        public Boolean this[int Index]
        {
            get
            {
                return _Visible[Index];
            }
            set
            {
                _Visible[Index] = value;
            }
        }

        public Boolean[] GetVisibels()
        {
            return _Visible;
        }
        public Int32[] GetDisplayIndex()
        {
            return _DisplayIndex;
        }
        public float[] GetFillWeight()
        {
            return _FillWeight;
        }
        public bool SetParametrColumnFillWeight()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.VisualColumnsPath))
                {
                    throw new Exception();
                }

                XDocument xdoc = XDocument.Load(SystemArgs.Path.VisualColumnsPath);

                int i = 0;

                foreach (XElement ColumnVisible in xdoc.Element("Columns").Elements("Column"))
                {
                    ColumnVisible.Element("FillWeight").SetValue(_FillWeight[i]);
                    i++;
                }

                xdoc.Save(SystemArgs.Path.VisualColumnsPath);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetParametrColumnDisplayIndex()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.VisualColumnsPath))
                {
                    throw new Exception();
                }

                XDocument xdoc = XDocument.Load(SystemArgs.Path.VisualColumnsPath);

                int i = 0;

                foreach (XElement ColumnVisible in xdoc.Element("Columns").Elements("Column"))
                {
                    ColumnVisible.Element("DisplayIndex").SetValue(_DisplayIndex[i]);
                    i++;
                }

                xdoc.Save(SystemArgs.Path.VisualColumnsPath);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetParametrColumnVisible()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.VisualColumnsPath))
                {
                    throw new Exception();
                }

                XDocument xdoc = XDocument.Load(SystemArgs.Path.VisualColumnsPath);

                int i = 0;

                foreach (XElement ColumnVisible in xdoc.Element("Columns").Elements("Column"))
                {
                    if (_Visible[i])
                    {
                        ColumnVisible.Element("Visible").SetValue("true");
                    }
                    else
                    {
                        ColumnVisible.Element("Visible").SetValue("false");
                    }
                    i++;
                }

                xdoc.Save(SystemArgs.Path.VisualColumnsPath);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetParametrColumn()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.VisualColumnsPath))
                {
                    throw new Exception();
                }

                XDocument xdoc = XDocument.Load(SystemArgs.Path.VisualColumnsPath);

                _Visible = new Boolean[xdoc.Element("Columns").Elements("Column").Count()];
                _DisplayIndex = new Int32[xdoc.Element("Columns").Elements("Column").Count()];
                _FillWeight = new float[xdoc.Element("Columns").Elements("Column").Count()];
                int i = 0;
                foreach (XElement ColumnVisible in xdoc.Element("Columns").Elements("Column"))
                {
                    if (ColumnVisible.Element("Visible").Value == "true")
                    {
                        _Visible[i] = true;
                    }
                    else
                    {
                        _Visible[i] = false;
                    }
                    _DisplayIndex[i] = Convert.ToInt32(ColumnVisible.Element("DisplayIndex").Value);
                    _FillWeight[i] = float.Parse(ColumnVisible.Element("FillWeight").Value, CultureInfo.InvariantCulture.NumberFormat);
                    i++;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckFile()
        {
            if (!File.Exists(SystemArgs.Path.VisualColumnsPath))
            {
                return false;
            }

            return true;
        }
    }
}
