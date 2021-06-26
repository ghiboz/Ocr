using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghiboz.Utils
{
    public class IOperation
    {
        public int key { get; set; } = -1;
        public bool enabled { get; set; } = false;
        public string menuTitle { get; set; }
        public string welcomeMessage { get; set; }

        /// <summary>
        /// stampa il menu iniziale
        /// </summary>
        /// <returns></returns>
        public virtual string Menu()
        {
            if (key == -1)
            {
                return "-----------------------------------------";
            }
            return $"{key}: {menuTitle}";
        }

        public void Disable()
        {
            enabled = false;
        }

        public virtual string Welcome(string value)
        {
            try
            {
                var command = Convert.ToInt32(value);
                if (command == key)
                {
                    enabled = true;
                    return $"{this.GetType().Name}: {welcomeMessage}\r\npremi \\ per tornare al menu";
                }
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        /// <summary>
        /// esegue l'operazione
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string Operate(string input)
        {
            throw new Exception("Operation not implemented");
        }
    }
}
