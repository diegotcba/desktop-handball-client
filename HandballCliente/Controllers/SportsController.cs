using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class SportsController
    {
        private static String[] sportList = {"General", "Volleyball", "Boxeo", "Basket", "Rugby"};

        public static Object[] getSportList()
        {
            return sportList;
        }
    }
}
