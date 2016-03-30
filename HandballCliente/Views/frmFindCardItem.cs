using HandballCliente.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HandballCliente.Controllers;

namespace HandballCliente
{
    public partial class frmFindCardItem : Form
    {
        private int action { get; set; }
        private Form1 source;

        public frmFindCardItem()
        {
            InitializeComponent();
        }

        public frmFindCardItem(Form1 f, int action = 1, string id = "1")
        {
            InitializeComponent();
            GameShowController.fillComboFindCardItemTypes(this.cmbItemType);
            AppController.getInstance().fillCombobox(this.cmbItemPicture);
            this.source = f;
            this.action = action;
            this.nudNumero.Value = int.Parse(id);
            if (action == 2)
            {
                FindCardItem aux = HandballMatch.getInstance().gameshowFindCardItems.Find(element => element.id == int.Parse(id));
                this.nudNumero.Value = aux.id;
                this.cmbItemType.Text = aux.type.ToString();
                this.cmbItemPicture.Text = aux.picture;
            }

        }

        private void FindCardItem_Load(object sender, EventArgs e)
        {
            nudNumero.Enabled = false;
            switch (action)
            {
                case 1:
                    this.Text = "Agregar Item";
                    break;
                case 2:
                    this.Text = "Modificar Item";
                    break;
                default:
                    break;
            }
        }

        private bool isCompleted()
        {
            bool aux = true;

            aux = (cmbItemPicture.Text != "");
            aux = (cmbItemType.Text != "");

            return aux;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (isCompleted())
            {
                FindCardItem aux = new FindCardItem();
                aux.id = (int) nudNumero.Value;
                aux.type = (int)cmbItemType.SelectedValue;
                aux.picture = cmbItemPicture.Text;

                if (action == 1)
                {
                    GameShowController.addFindCardItem(aux);
                }
                else
                {
                    GameShowController.updateFindCardItem(aux);
                }

                source.updateGameShowFindCardItems();
                this.Close();
            }
        }
    }
}
