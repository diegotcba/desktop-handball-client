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
        private List<String> images;
        private List<String> audios;
        private List<ComboBox> comboTemplates;
        private CasparCG casparCgServer;

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
            videos = new List<string>();
            images = new List<string>();
            audios = new List<string>();
            comboTemplates = new List<ComboBox>();
            casparCgServer = new CasparCG();
        }

        public void addComboBoxTemplate(ComboBox combo)
        {
            comboTemplates.Add(combo);
        }

        public void getServerTemplates()
        {
            if (casparCgServer.Connected)
            {
                templates = casparCgServer.GetTemplateNames();
            }
        }

        public void getServerImages()
        {
            if (casparCgServer.Connected)
            {
                images = casparCgServer.GetMediaClipsNames(CasparCG.MediaTypes.Still);
            }
        }

        public void getServerVideos()
        {
            if (casparCgServer.Connected)
            {
                videos = casparCgServer.GetMediaClipsNames(CasparCG.MediaTypes.Movie);
            }
        }

        public void getServerAudios()
        {
            if (casparCgServer.Connected)
            {
                audios = casparCgServer.GetMediaClipsNames(CasparCG.MediaTypes.Audio);
            }
        }

        public List<String> getVideos()
        {
            return videos;
        }

        public List<String> getImages()
        {
            return images;
        }

        public List<String> getAudios()
        {
            return audios;
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

        public bool isConnectedToCasparCgServer()
        {
            return casparCgServer.Connected;
        }

        public void setCasparCgServer(String address, int port)
        {
            casparCgServer = new CasparCG();
            casparCgServer.ServerAdress = address;
            casparCgServer.Port = port;
        }

        public void connectCasparCgServer()
        {
            casparCgServer.Connect();
        }

        public void disconnectCasparCgServer()
        {
            casparCgServer.Disconnect();
        }

        public String getCasparCgServerAddress()
        {
            return casparCgServer.ServerAdress;
        }

        public int getCasparCgServerPort()
        {
            return casparCgServer.Port;
        }

        public CasparCG.Paths getCasparCgServerPaths()
        {
            return casparCgServer.ServerPaths;
        }

        public ReturnInfo executeCasparCgServer(String command)
        {
            return casparCgServer.Execute(command);
        }
    }
}
