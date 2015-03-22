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
    public partial class Posicion : Form
    {
        private int action { get; set; }
        private Form1 source;
        private Position position;

        public Posicion()
        {
            InitializeComponent();
        }

        public Posicion(Form1 f, int a = 1, Position pos=null)
        {
            InitializeComponent();
            source = f;
            action = a;
            position = pos;
            SetValues();
            if (position!=null)
            {
                txtTeam.Text = position.team;
                nudPoints.Value = position.points;
                nudPlayed.Value = position.played;
                nudWon.Value = position.won;
                nudDrawn.Value = position.drawn;
                nudLost.Value = position.lost;
                nudGoalsFor.Value = position.goalsFor;
                nudGoalsAgainst.Value = position.goalsAgainst;
                nudGoalDiff.Value = position.goalDifference;
            }
        }

        private void SetValues()
        {
            SetNUDProperties(nudPoints, 0, 99, 0);
            SetNUDProperties(nudPlayed, 0, 99, 0);
            SetNUDProperties(nudWon, 0, 99, 0);
            SetNUDProperties(nudDrawn, 0, 99, 0);
            SetNUDProperties(nudLost, 0, 99, 0);
            SetNUDProperties(nudGoalDiff, -999, 999, 0);
            SetNUDProperties(nudGoalsFor, 0, 999, 0);
            SetNUDProperties(nudGoalsAgainst, 0, 999, 0);
        }

        private void Posicion_Load(object sender, EventArgs e)
        {
            switch (action)
            {
                case 1:
                    this.Text = "Agregar Posicion";
                    break;
                case 2:
                    this.Text = "Modificar Posicion";
                    break;
                default:
                    break;
            }
        }

        private void SetNUDProperties(NumericUpDown nud, decimal min, decimal max, decimal val)
        {
            nud.Minimum = min;
            nud.Maximum = max;
            nud.Value = val;
        }

        private void AutoSetPJ()
        {
            nudPlayed.Value = nudWon.Value + nudDrawn.Value + nudLost.Value;
            AutoSetPoints();
        }

        private void AutoSetGDiff()
        {
            nudGoalDiff.Value = (nudGoalsFor.Value - nudGoalsAgainst.Value);
        }

        private void AutoSetPoints()
        {
            nudPoints.Value=(nudWon.Value*3)+(nudDrawn.Value);
        }

        private void AssignData()
        {
            position = new Position();
            position.team = txtTeam.Text;
            position.points = (int) nudPoints.Value;
            position.played = (int)nudPlayed.Value;
            position.won = (int)nudWon.Value;
            position.drawn = (int)nudDrawn.Value;
            position.lost = (int)nudLost.Value;
            position.goalsFor = (int)nudGoalsFor.Value;
            position.goalsAgainst = (int)nudGoalsAgainst.Value;
            position.goalDifference = (int)nudGoalDiff.Value;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtTeam.Text == "")
                return;
            AssignData();
            if (action == 1)
            {
                source.AddPositionToTable(position);
            }
            else
            {
                source.EditPositionTable(position);
            }
            this.Close();
        }

        private void nudWon_ValueChanged(object sender, EventArgs e)
        {
            AutoSetPJ();
        }

        private void nudDrawn_ValueChanged(object sender, EventArgs e)
        {
            AutoSetPJ();
        }

        private void nudLost_ValueChanged(object sender, EventArgs e)
        {
            AutoSetPJ();
        }

        private void nudGoalsFor_ValueChanged(object sender, EventArgs e)
        {
            AutoSetGDiff();
        }

        private void nudGoalsAgainst_ValueChanged(object sender, EventArgs e)
        {
            AutoSetGDiff();
        }
    }
}
