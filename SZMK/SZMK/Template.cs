using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Template
    {
        public Template()
        {
            try 
            {
                CheckFiles();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void CheckFiles()
        {
            if (!File.Exists(SystemArgs.Path.TemplateActUniquePath))
            {
                throw new Exception("Не найден шаблон акта уникальных чертежей");
            }
            if (!File.Exists(SystemArgs.Path.TemplateActNoUniquePath))
            {
                throw new Exception("Не найден шаблон акта не уникальных чертежей");
            }
            if (!File.Exists(SystemArgs.Path.TemplateRegistry))
            {
                throw new Exception("Не найден шаблон реестра чертежей");
            }
        }
    }
}
