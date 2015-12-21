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
    public partial class Pregunta : Form
    {
        private int action { get; set; }
        private Form1 source;

        public Pregunta()
        {
            InitializeComponent();
        }

        public Pregunta(Form1 f, int action = 1, string id = "1", string q = "", List<Answer> a = null, int correct = 1)
        {
            InitializeComponent();
            this.source = f;
            this.action = action;
            this.nudNumero.Value = decimal.Parse(id);
            this.txtQuestion.Text = q;
            if (a != null)
            {
                this.txtAnswer1.Text = a[0].answer;
                this.txtAnswer2.Text = a[1].answer;
                this.txtAnswer3.Text = a[2].answer;
                this.txtAnswer4.Text = a[3].answer;
            }
            this.cmbCorrectAnswer.Text = correct.ToString();
        }

        private void Pregunta_Load(object sender, EventArgs e)
        {
            nudNumero.Enabled = false;
            switch (action)
            {
                case 1:
                    this.Text = "Agregar Pregunta";
                    break;
                case 2:
                    this.Text = "Modificar Pregunta";
                    break;
                default:
                    break;
            }
        }

        private bool isCompleted()
        {
            bool aux = true;

            aux = (txtQuestion.Text != "");
            aux = (txtAnswer1.Text != "");
            aux = (txtAnswer2.Text != "");
            aux = (txtAnswer3.Text != "");
            aux = (txtAnswer4.Text != "");
            aux = (cmbCorrectAnswer.Text != "");

            return aux;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (isCompleted())
            {
                Question q = new Question();
                q.id = (int)nudNumero.Value;
                q.question = txtQuestion.Text.Trim();
                List<Answer> ans = new List<Answer>();
                ans.Add(new Answer(txtAnswer1.Text.Trim()));
                ans.Add(new Answer(txtAnswer2.Text.Trim()));
                ans.Add(new Answer(txtAnswer3.Text.Trim()));
                ans.Add(new Answer(txtAnswer4.Text.Trim()));
                q.answers = ans;
                q.correctAnswer = int.Parse(cmbCorrectAnswer.Text);
                source.actionQuestion(action, q);
                this.Close();
            }
        }
    }
}
