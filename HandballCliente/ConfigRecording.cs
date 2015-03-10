using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente
{
    public partial class ConfigRecording : Form
    {
        public ConfigRecording()
        {
            InitializeComponent();
        }

        private void ConfigRecording_Load(object sender, EventArgs e)
        {
            cmbCRFRecording.Items.Clear();
            cmbCRFRecording.Items.Add(13);
            cmbCRFRecording.Items.Add(18);
            cmbCRFRecording.Items.Add(23);
            cmbCRFRecording.Items.Add(28);
            cmbCRFRecording.Items.Add(30);
            cmbCRFRecording.Items.Add(33);

            cmbCRFRecording.Text = HandballMatch.getInstance().recordingCRF.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!cmbCRFRecording.Text.Equals(""))
            {
                HandballMatch.getInstance().recordingCRF = int.Parse(cmbCRFRecording.Text);
                this.Close();
            }
        }
    }
}
