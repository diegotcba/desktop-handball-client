using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public class AppController
    {
        public static AppController instance = null;

        private List<String> templates;
        private List<String> videos;
        private List<ComboBox> comboTemplates;

        public static AppController getInstance()
        {
            if (instance == null)
            {
                instance = new AppController();
            }
            return instance;
        }

        private AppController()
        {
            templates = new List<string>();
            comboTemplates = new List<ComboBox>();
        }

        public void addComboBoxTemplate(ComboBox combo)
        {
            comboTemplates.Add(combo);
        }

        public void getServerTemplates(CasparCG casparServer)
        {
            if (casparServer.Connected)
            {
                templates = casparServer.GetTemplateNames();
            }
        }

        public void fillCombosTemplates()
        {
            foreach (ComboBox item in comboTemplates)
            {
                fillCombobox(item);
            }
        }

        public void fillCombobox(ComboBox cbo)
        {
            cbo.Items.Clear();
            foreach (String t in templates)
            {
                cbo.Items.Add(t);
            }
        }

        private void fillListView(ListView lvw, List<string> answers)
        {
            lvw.Items.Clear();
            foreach (string item in answers)
            {
                lvw.Items.Add(new ListViewItem(item));
            }
        }

    }
}
