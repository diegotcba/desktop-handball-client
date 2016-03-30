using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class UtilHelper
    {
        public static void populateComboboxNumberRange(ComboBox combo, int from, int to)
        {
            combo.Items.Clear();
            for (int i = from; i <= to; i++)
            {
                combo.Items.Add(i);
            }
        }
    }
}
