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
    public partial class Jugador : Form
    {
        private int action { get; set; }
        private Form1 source;

        public Jugador()
        {
            InitializeComponent();
        }

        public Jugador(Form1 f,int a=1,string n="1",string nc="")
        {
            InitializeComponent();
            source = f;
            action = a;
            nudNumero.Value = decimal.Parse(n);
            txtNombreCompleto.Text = nc;
        }

        private void Jugador_Load(object sender, EventArgs e)
        {
            switch (action)
            {
                case 1:
                    this.Text = "Agregar Jugador Local";
                    break;
                case 2:
                    this.Text = "Modificar Jugador Local";
                    break;
                case 3:
                    this.Text = "Agregar Jugador Visitante";
                    break;
                case 4:
                    this.Text = "Modificar Jugador Visitante";
                    break;
                default:
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            source.Jugador(action, (int)nudNumero.Value, txtNombreCompleto.Text);
            this.Close();
        }
    }
}
