using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace HandballCliente
{
    public partial class Form1 : Form
    {
        //-----------------------------CONSTANTS FOR TOPMOST FEATURE -------------------------------------------------------------
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        //------------------------------------------------------------------------------------------------------------------------

        private CasparCG casparServer = new CasparCG();
        private Color ColorFondo;
        private String fileName = "";


        private const int layerPresentation = 5;
        private const int layerTeams = 10;
        private const int layerScoreboard = 15;
        private const int layerResult = 20;
        private const int layerLowerThird = 25;
        private const int layerPositions = 30;
        private const int layerTwitter = 35;

        private const int layerVideo = 1;
        private const int layerLogo = 99;
        private const int layerImageScrolling = 90;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.NewMatch();
            ColorFondo = stsEstado.BackColor;
        }

        private void NewMatch()
        {
            Limpiar();
            HandballMatch.getInstance().NewMatch();
            fileName = "";
            this.Text = String.Format("Handball Cliente - [{0}]", "sin titulo");
            setDefaults();
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            dtpFecha.Value = DateTime.Now;
            clearParameters();
            clearPresentation();
            clearTeams();
            clearScoreboard();
            clearMultimedia();
        }

        #region ClearData
        private void clearParameters()
        {
            txtDireccionServidor.Text = "";
            txtPuertoServidor.Text = "";
            cmbTemplateEquipos.Items.Clear();
            cmbTemplateLowerThird.Items.Clear();
            cmbTemplatePresentacion.Items.Clear();
            cmbTemplateResultado.Items.Clear();
            cmbTemplateScoreboard.Items.Clear();
            cmbTemplatePositions.Items.Clear();
            cmbTemplateTwitter.Items.Clear();
        }

        private void clearPresentation()
        {

        }

        private void clearTeams()
        {
            lvwEquipoLocal.Items.Clear();
            lvwEquipoVisitante.Items.Clear();
        }

        private void clearScoreboard()
        {

        }

        private void clearMultimedia()
        {

        }
        #endregion

        private void setDefaults()
        {
            txtDireccionServidor.Text = casparServer.ServerAdress;
            txtPuertoServidor.Text = casparServer.Port.ToString();
            btnConexion.Text = casparServer.Connected ? "Desconectar" : "Conectar";
            btnClearChannel.Enabled = casparServer.Connected;
            btnMostrarOcultarPresentacion.Tag = "0";
            btnMostrarEquipoLocal.Tag = "0";
            btnMostrarEquipoVisitante.Tag = "0";
            btnMostrarEquipoLocalVisitante.Tag = "0";
            btnMostrarResultado.Tag = "0";
            lockUnlockTemplates();
            radTeam1List.Checked = true;
            trkVolume.Value = 10;
            trkImageScrollingSpeed.Value = 1;

            fillCombosTeamTextStyle();
        }

        private void fillCombosTeamTextStyle()
        {
            cmbPlayersFontSize.Items.Clear();
            cmbPlayersFontSize.Items.Add("10");
            cmbPlayersFontSize.Items.Add("12");
            cmbPlayersFontSize.Items.Add("14");
            cmbPlayersFontSize.Items.Add("16");
            cmbPlayersFontSize.Items.Add("18");
            cmbPlayersFontSize.Items.Add("20");

            cmbPlayersFontLineSpacing.Items.Clear();
            cmbPlayersFontLineSpacing.Items.Add("-20");
            cmbPlayersFontLineSpacing.Items.Add("-10");
            cmbPlayersFontLineSpacing.Items.Add("-5");
            cmbPlayersFontLineSpacing.Items.Add("0");
            cmbPlayersFontLineSpacing.Items.Add("5");
            cmbPlayersFontLineSpacing.Items.Add("10");
            cmbPlayersFontLineSpacing.Items.Add("20");

            cmbPlayersFontLetterSpacing.Items.Clear();
            cmbPlayersFontLetterSpacing.Items.Add("-20");
            cmbPlayersFontLetterSpacing.Items.Add("-10");
            cmbPlayersFontLetterSpacing.Items.Add("-5");
            cmbPlayersFontLetterSpacing.Items.Add("0");
            cmbPlayersFontLetterSpacing.Items.Add("5");
            cmbPlayersFontLetterSpacing.Items.Add("10");
            cmbPlayersFontLetterSpacing.Items.Add("20");
        }

        private void setTopMost(bool stateTopMost)
        {
            SetWindowPos(this.Handle, (stateTopMost) ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void connectToCasparCGServer()
        {
            try
            {
                if (!casparServer.Connected)
                {
                    casparServer.Connect();
                    getServerTemplates();
                    getServerImageFiles();
                    getServerVideoFiles();
                }
                else
                {
                    casparServer.Disconnect();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                btnConexion.Text = casparServer.Connected ? "Desconectar" : "Conectar";
                btnClearChannel.Enabled = casparServer.Connected;
                lockUnlockTemplates();
                //this.BackColor = casparServer.Connected ? Color.ForestGreen : ColorFondo;
                stsEstado.BackColor = casparServer.Connected ? Color.ForestGreen : ColorFondo;
            }
        }

        public void clearChannel()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute("CLEAR 1");
            }
        }

        private void getServerTemplates()
        {
            if (casparServer.Connected)
            {
                List<String> templates = casparServer.GetTemplateNames();
                fillCombosTemplate(cmbTemplateEquipos, templates);
                fillCombosTemplate(cmbTemplatePresentacion, templates);
                fillCombosTemplate(cmbTemplateResultado, templates);
                fillCombosTemplate(cmbTemplateScoreboard, templates);
                fillCombosTemplate(cmbTemplateLowerThird, templates);
                fillCombosTemplate(cmbTemplatePositions, templates);
                fillCombosTemplate(cmbTemplateTwitter, templates);
            }
        }

        private void getServerImageFiles()
        {
            if (casparServer.Connected)
            {
                List<String> medias = casparServer.GetMediaClipsNames(CasparCG.MediaTypes.Still);
                fillCombosTemplate(cmbLogoLocal, medias);
                fillCombosTemplate(cmbLogoVisitante, medias);
                fillCombosTemplate(cmbLogoFederacion, medias);
                fillCombosTemplate(cmbLogoLocal, medias);
                fillCombosTemplate(cmbLogoVisitante, medias);
                fillCombosTemplate(cmbLogoTransmision, medias);
                fillCombosTemplate(cmbImageScrolling, medias);
            }
        }

        private void getServerVideoFiles()
        {
            if (casparServer.Connected)
            {
                List<String> medias = casparServer.GetMediaClipsNames(CasparCG.MediaTypes.Movie);
                lvwVideoFiles.Items.Clear();
                foreach (String item in medias)
                {
                    lvwVideoFiles.Items.Add(item);
                }
            }
        }

        private void getServerAudioFiles()
        {
            if (casparServer.Connected)
            {
                List<String> medias = casparServer.GetMediaClipsNames(CasparCG.MediaTypes.Audio);
                lvwVideoFiles.Items.Clear();
                foreach (String item in medias)
                {
                    lvwVideoFiles.Items.Add(item);
                }
            }
        }

        private void fillCombosTemplate(ComboBox cbo, List<String> templates)
        {
            cbo.Items.Clear();
            foreach (String t in templates)
            {
                cbo.Items.Add(t);
            }
        }

        private void lockUnlockTemplates()
        {
            cmbTemplateEquipos.Enabled = !cmbTemplateEquipos.Enabled;
            cmbTemplateLowerThird.Enabled = !cmbTemplateLowerThird.Enabled;
            cmbTemplatePresentacion.Enabled = !cmbTemplatePresentacion.Enabled;
            cmbTemplateResultado.Enabled = !cmbTemplateResultado.Enabled;
            cmbTemplateScoreboard.Enabled = !cmbTemplateScoreboard.Enabled;
            cmbTemplatePositions.Enabled = !cmbTemplatePositions.Enabled;
            cmbTemplateTwitter.Enabled = !cmbTemplateTwitter.Enabled;
        }

        private bool checkDataScoreboard()
        {
            bool aux = false;

            aux = (cmbTemplateScoreboard.Text.Length > 0);

            aux = (txtNombreScoreLocal.Text.Length > 0);

            aux = (txtNombreScoreVisitante.Text.Length > 0);

            aux = (cmbTiempo.Text.Length > 0);

            return aux;
        }

        private void startScoreboard()
        {
            if (checkDataScoreboard())
            {
                if (casparServer.Connected)
                {
                    Template templateScoreboard = new Template();
                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoFederacion.Text.ToLower() + ".png");

                    templateScoreboard.Fields.Add(new TemplateField("team1Name", txtNombreScoreLocal.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team2Name", txtNombreScoreVisitante.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team1Score", nudScoreLocal.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("team2Score", nudScoreVisitante.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("gameTime", nudMinutosReloj.Value.ToString() + ":" + nudSegundosReloj.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("halfNum", cmbTiempo.Text));
                    templateScoreboard.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), cmbTemplateScoreboard.Text, layerScoreboard.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnMostrarOcultarPresentacion.Tag = "1";
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template, local/visitante)", this.Text);
            }
        }

        private void stopScoreboard()
        {
            casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerScoreboard.ToString()));
        }

        private void showHideScoreboard()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "clockShowHide", layerScoreboard.ToString()));
            }
        }

        private void startScoreboardClock()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layerScoreboard.ToString()));
            }
        }

        private void stopScoreboardClock()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layerScoreboard.ToString()));
            }
        }

        private void resetScoreboardClock()
        {
            Template templateUpdateScore = new Template();

            templateUpdateScore.Fields.Add(new TemplateField("gameTime", nudMinutosReloj.Value.ToString() + ":" + nudSegundosReloj.Value.ToString()));

            //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
            ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
            System.Diagnostics.Debug.WriteLine(ri.Message);
        }

        public void addOneScoreTeam1()
        {
            nudScoreLocal.UpButton();
        }

        public void addOneScoreTeam2()
        {
            nudScoreVisitante.UpButton();
        }

        private void updateTeamsScore()
        {
            if (casparServer.Connected)
            {
                Template templateUpdateScore = new Template();

                templateUpdateScore.Fields.Add(new TemplateField("team1Score", nudScoreLocal.Value.ToString()));
                templateUpdateScore.Fields.Add(new TemplateField("team2Score", nudScoreVisitante.Value.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void showHideIntro()
        {
            if (casparServer.Connected)
            {
                if (!btnMostrarOcultarPresentacion.Tag.ToString().Equals("1"))
                {
                    Template templateIntro = new Template();

                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoLocal.Text.ToLower() + ".png");
                    Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoVisitante.Text.ToLower() + ".png");

                    templateIntro.Fields.Add(new TemplateField("team1Name", txtNombreLocal.Text));
                    templateIntro.Fields.Add(new TemplateField("team2Name", txtNombreVisitante.Text));
                    templateIntro.Fields.Add(new TemplateField("infoLeague", txtCampeonato.Text));
                    templateIntro.Fields.Add(new TemplateField("infoDate", txtTitulo.Text));
                    templateIntro.Fields.Add(new TemplateField("infoLocation", txtLugar.Text));
                    templateIntro.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                    templateIntro.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text, layerPresentation.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnMostrarOcultarPresentacion.Tag = "1";
                    if (chkAutoOcultarPresentacion.Checked)
                    {
                        tmrIntro.Interval = ((int)nudSegundosAutoOcultarPresentacion.Value) * 1000;
                        tmrIntro.Enabled = true;
                        tmrIntro.Start();
                    }
                }
                else
                {
                    casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
                    btnMostrarOcultarPresentacion.Tag = "0";
                    if (chkAutoOcultarPresentacion.Checked)
                    {
                        tmrIntro.Stop();
                        tmrIntro.Enabled = false;
                    }
                }
            }
        }

        public String getPlayerList(ListView lvw)
        {
            String aux = "";

            foreach (ListViewItem item in lvw.Items)
            {
                aux += int.Parse(item.SubItems[0].Text).ToString("00") + "   " + item.SubItems[1].Text + "\n";
            }

            return aux;
        }

        private void showHideTeamsheet(Button btnSource, TextBox txtTeam, ListView lvwPlayers, TextBox txtCoach, ComboBox cmbLogo, CheckBox chkAutoHide, NumericUpDown nudSeconds, Timer timer)
        {
            if (casparServer.Connected)
            {
                if (!btnSource.Tag.ToString().Equals("1"))
                {
                    String players;
                    players = getPlayerList(lvwPlayers);

                    Template templateIntro = new Template();

                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogo.Text.ToLower() + ".png");

                    templateIntro.Fields.Add(new TemplateField("teamName", txtTeam.Text));
                    templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                    templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtCoach.Text));
                    templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));
                    templateIntro.Fields.Add(new TemplateField("fontSizePlayers", cmbPlayersFontSize.Text));
                    templateIntro.Fields.Add(new TemplateField("fontLineSpacingPlayers", cmbPlayersFontLineSpacing.Text));
                    templateIntro.Fields.Add(new TemplateField("fontLetterSpacingPlayers", cmbPlayersFontLetterSpacing.Text));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateEquipos.Text, layerTeams.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnSource.Tag = "1";
                    if (chkAutoHide.Checked)
                    {
                        timer.Interval = ((int)nudSeconds.Value) * 1000;
                        timer.Enabled = true;
                        timer.Start();
                    }
                }
                else
                {
                    casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));
                    btnSource.Tag = "0";
                    if (chkAutoHide.Checked)
                    {
                        timer.Stop();
                        timer.Enabled = false;
                    }
                }
            }
        }

        private void autoShowHideTeams()
        {
            if (casparServer.Connected)
            {
                switch (btnMostrarEquipoLocalVisitante.Tag.ToString())
                {
                    case "0":
                        btnMostrarEquipoLocalVisitante.Tag = "1";
                        String players;
                        players = getPlayerList(lvwEquipoLocal);

                        Template templateIntro = new Template();

                        Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoLocal.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtNombreLocal.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtEntrenadorLocal.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateEquipos.Text, layerTeams.ToString()));

                        tmrTeams.Interval = ((int)nudSegundosOcultarEquipoLocalVisitante.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "1":
                        btnMostrarEquipoLocalVisitante.Tag = "2";
                        casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));

                        tmrTeams.Stop();
                        tmrTeams.Enabled = false;

                        tmrInBetweenTeams.Interval = ((int)nudSegundosSeparacionEquipoLocalVisitante.Value) * 1000;
                        tmrInBetweenTeams.Enabled = true;
                        tmrInBetweenTeams.Start();
                        break;
                    case "2":
                        btnMostrarEquipoLocalVisitante.Tag = "3";
                        players = getPlayerList(lvwEquipoVisitante);

                        templateIntro = new Template();

                        logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoVisitante.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtNombreVisitante.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtEntrenadorVisitante.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateEquipos.Text, layerTeams.ToString()));

                        tmrInBetweenTeams.Stop();
                        tmrInBetweenTeams.Enabled = false;

                        tmrTeams.Interval = ((int)nudSegundosOcultarEquipoLocalVisitante.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "3":
                        btnMostrarEquipoLocalVisitante.Tag = "0";
                        casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));

                        tmrTeams.Stop();
                        tmrTeams.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void showHideResult()
        {
            if (casparServer.Connected)
            {
                if (!btnMostrarResultado.Tag.ToString().Equals("1"))
                {
                    Template templateResult = new Template();

                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoLocal.Text.ToLower() + ".png");
                    Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoVisitante.Text.ToLower() + ".png");

                    templateResult.Fields.Add(new TemplateField("team1Name", txtNombreLocal.Text));
                    templateResult.Fields.Add(new TemplateField("team2Name", txtNombreVisitante.Text));
                    templateResult.Fields.Add(new TemplateField("team1Score", nudScoreLocal.Value.ToString()));
                    templateResult.Fields.Add(new TemplateField("team2Score", nudScoreVisitante.Value.ToString()));
                    templateResult.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                    templateResult.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));
                    templateResult.Fields.Add(new TemplateField("timeResult", ""));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateResult.TemplateDataText(), cmbTemplateResultado.Text, layerResult.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnMostrarResultado.Tag = "1";
                    if (chkAutoHideResult.Checked)
                    {
                        tmrResult.Interval = ((int)nudSeconsAutoHideResult.Value) * 1000;
                        tmrResult.Enabled = true;
                        tmrResult.Start();
                    }
                }
                else
                {
                    casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerResult.ToString()));
                    btnMostrarResultado.Tag = "0";
                    if (chkAutoHideResult.Checked)
                    {
                        tmrResult.Stop();
                        tmrResult.Enabled = false;
                    }
                }
            }
        }

        private void startLowerThird()
        {
            if (cmbTemplateLowerThird.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templateLowerThird = new Template();

                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + ((radTeam1List.Checked) ? cmbLogoLocal.Text.ToLower() : cmbLogoVisitante.Text.ToLower()) + ".png");

                    templateLowerThird.Fields.Add(new TemplateField("teamPlayer", txtLTTitulo.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamName", txtLTSubtitulo.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamLogo", logoPath.AbsoluteUri));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateLowerThird.TemplateDataText(), cmbTemplateLowerThird.Text, layerLowerThird.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnMostrarOcultarPresentacion.Tag = "1";

                    if (chkAutoOcultarLT.Checked)
                    {
                        tmrLT.Interval = ((int)nudSegundosAutoOcultarLT.Value) * 1000;
                        tmrLT.Enabled = true;
                        tmrLT.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private void stopLowerThird()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerLowerThird.ToString()));
                if (chkAutoOcultarLT.Checked)
                {
                    tmrLT.Stop();
                    tmrLT.Enabled = false;
                }
            }
        }

        private void loadPlayerLT()
        {
            string[] arr = new string[2];
            if (radTeam1List.Checked)
            {
                lvwJugadores.Items.Clear();
                foreach (ListViewItem item in lvwEquipoLocal.Items)
                {
                    arr[0] = item.SubItems[0].Text;
                    arr[1] = item.SubItems[1].Text;
                    lvwJugadores.Items.Add(new ListViewItem(arr));
                }
            }
            else
            {
                lvwJugadores.Items.Clear();
                foreach (ListViewItem item in lvwEquipoVisitante.Items)
                {
                    arr[0] = item.SubItems[0].Text;
                    arr[1] = item.SubItems[1].Text;
                    lvwJugadores.Items.Add(new ListViewItem(arr));
                }
            }
        }

        private void getPlayerInfoToLT()
        {
            if (lvwJugadores.SelectedItems != null)
            {
                txtLTTitulo.Text = lvwJugadores.SelectedItems[0].SubItems[1].Text;
                txtLTSubtitulo.Text = radTeam1List.Checked ? txtNombreLocal.Text : txtNombreVisitante.Text;
            }
        }

        private void startPositions()
        {
            if (cmbTemplatePositions.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templatePositions = new Template();

                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbLogoFederacion.Text.ToLower() + ".png");

                    templatePositions.Fields.Add(new TemplateField("logo", logoPath.AbsoluteUri));
                    templatePositions.Fields.Add(new TemplateField("title", txtPositionsTitle.Text));
                    templatePositions.Fields.Add(new TemplateField("subtitle", txtPositionsSubtitle.Text));
                    templatePositions.Fields.Add(new TemplateField("line01", "#;Equipo;Pts;PJ;PG;PE;PP;GF;GC;Dif"));
                    templatePositions.Fields.Add(new TemplateField("line02", getPositionCVS(1)));
                    templatePositions.Fields.Add(new TemplateField("line03", getPositionCVS(2)));
                    templatePositions.Fields.Add(new TemplateField("line04", getPositionCVS(3)));
                    templatePositions.Fields.Add(new TemplateField("line05", getPositionCVS(4)));
                    templatePositions.Fields.Add(new TemplateField("line06", getPositionCVS(5)));
                    templatePositions.Fields.Add(new TemplateField("line07", getPositionCVS(6)));
                    templatePositions.Fields.Add(new TemplateField("line08", getPositionCVS(7)));
                    templatePositions.Fields.Add(new TemplateField("line09", getPositionCVS(8)));
                    templatePositions.Fields.Add(new TemplateField("line10", getPositionCVS(9)));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templatePositions.TemplateDataText(), cmbTemplatePositions.Text, layerPositions.ToString()));

                    if (chkAutoHidePositions.Checked)
                    {
                        tmrPositions.Interval = ((int)nudAutoHidePositionsSeconds.Value) * 1000;
                        tmrPositions.Enabled = true;
                        tmrPositions.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private String getPositionCVS(int index)
        {
            String aux = "";
            if (HandballMatch.getInstance().positions.Count>=(index))
            {
                aux = index + ";" + HandballMatch.getInstance().positions.ElementAt(index-1).getCVS();
            }
            return aux;
        }

        private void stopPositions()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerPositions.ToString()));
                if (chkAutoHidePositions.Checked)
                {
                    tmrPositions.Stop();
                    tmrPositions.Enabled = false;
                }
            }
        }

        private void startTwitter()
        {
            if (cmbTemplateTwitter.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templateTwitter = new Template();

                    templateTwitter.Fields.Add(new TemplateField("hashtag", txtTwitterHashtag.Text));
                    templateTwitter.Fields.Add(new TemplateField("fullname", txtTwitterFullName.Text));
                    templateTwitter.Fields.Add(new TemplateField("username", txtTwitterUserName.Text));
                    templateTwitter.Fields.Add(new TemplateField("message", txtTwitterMessage.Text));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), cmbTemplateTwitter.Text, layerTwitter.ToString()));

                    if (chkAutoHideTwitter.Checked)
                    {
                        tmrTwitter.Interval = ((int)nudAutoHideTwitterSeconds.Value) * 1000;
                        tmrTwitter.Enabled = true;
                        tmrTwitter.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private void stopTwitter()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerTwitter.ToString()));
                if (chkAutoHideTwitter.Checked)
                {
                    tmrTwitter.Stop();
                    tmrTwitter.Enabled = false;
                }
            }
        }

        private void startRecording()
        {
            if (casparServer.Connected)
            {
                if (!txtArchivoGrabacion.Text.Equals(""))
                {
                    //String aux=String.Format("ADD 1 FILE {0}-{1}-{2}.mov -vcodec libx264 [-preset ultrafast -tune fastdecode -crf 5]","REC",txtArchivoGrabacion.Text,DateTime.Now.ToString("ddMM-HHmm"));
                    String aux = String.Format("ADD 1 FILE {0}-{1}-{2}.mp4 -vcodec libx264 -preset ultrafast -tune fastdecode -crf {3}", "REC", txtArchivoGrabacion.Text, DateTime.Now.ToString("ddMMHHmm"), HandballMatch.getInstance().recordingCRF);
                    casparServer.Execute(aux);
                }
            }
        }

        private void stopRecording()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute("REMOVE 1 FILE");
            }
        }

        private void showLogo()
        {
            if (casparServer.Connected)
            {
                if (!cmbLogoTransmision.Text.Equals(""))
                {
                    casparServer.Execute(String.Format("PLAY 1-{2} {0}{1}{0} MIX 15", (char)0x22, cmbLogoTransmision.Text, layerLogo.ToString()));
                }
            }
        }

        private void hideLogo()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("STOP 1-{0}", layerLogo.ToString()));
            }
        }

        private void playMedia()
        {
            if (casparServer.Connected)
            {
                if (lvwVideoFiles.SelectedItems != null)
                {
                    casparServer.Execute(String.Format("PLAY 1-{3} {0}{1}{0} {2}", (char)0x22, lvwVideoFiles.SelectedItems[0].SubItems[0].Text, (chkLoopVideoFile.Checked ? "LOOP" : ""), layerVideo.ToString()));
                }
            }
        }

        private void stopMedia()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("STOP 1-{0}", layerVideo.ToString()));
            }
        }

        private void pauseMedia()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("PAUSE 1-{0}", layerVideo.ToString()));
            }
        }

        private void setVolume()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("MIXER 1 MASTERVOLUME {0}", (trkVolume.Value * 10)));
            }
        }

        private void muteVolume()
        {
            if (casparServer.Connected)
            {
                trkVolume.Value = 0;
                setVolume();
            }
        }

        private void startImageScrolling()
        {
            if (casparServer.Connected)
            {
                if (!cmbImageScrolling.Text.Equals(""))
                {
                    casparServer.Execute(String.Format("PLAY 1-{2} {0}{1}{0} BLUR 0 SPEED {3}", (char)0x22, cmbImageScrolling.Text, layerImageScrolling.ToString(), trkImageScrollingSpeed.Value));
                }
            }
        }

        private void stopImageScrolling()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("STOP 1-{0}", layerImageScrolling.ToString()));
            }
        }

        private void openConfigRecording()
        {
            ConfigRecording f1 = new ConfigRecording();
            f1.ShowDialog(this);
        }

        public void Jugador(int action, int nro, string nombrecompleto)
        {
            switch (action)
            {
                case 1:
                    AgregarJugador(lvwEquipoLocal, nro, nombrecompleto);
                    break;
                case 2:
                    ModificarJugador(HandballMatch.getInstance().team1Players, lvwEquipoLocal, nro, nombrecompleto);
                    break;
                case 3:
                    AgregarJugador(lvwEquipoVisitante, nro, nombrecompleto);
                    break;
                case 4:
                    ModificarJugador(HandballMatch.getInstance().team2Players, lvwEquipoVisitante, nro, nombrecompleto);
                    break;
                default:
                    break;
            }
        }

        public void AgregarJugador(ListView lvw, int nro, string nombrecompleto)
        {
            if (lvw.SelectedItems != null)
            {
                Player aux = new Player(nro, "", "", "");
                string[] arr = nombrecompleto.Split(',');
                aux.lastName = arr[0].Trim();
                aux.firstName = arr[1].Trim();

                if (lvw.Name.Contains("Local"))
                {
                    AddPlayerToTeam(HandballMatch.getInstance().team1Players, aux, lvw);
                }
                else
                {
                    AddPlayerToTeam(HandballMatch.getInstance().team2Players, aux, lvw);
                }
            }
        }

        public void AddPlayerToTeam(List<Player> team, Player player, ListView lvw)
        {
            if (!team.Exists(element => element.number == player.number))
            {
                team.Add(player);
                FillTeamPlayers(lvw, team);
            }
        }

        public void FillTeamPlayers(ListView lvw, List<Player> players)
        {
            string[] arr;
            lvw.Items.Clear();
            foreach (Player item in players)
            {
                arr = new string[2];
                arr[0] = item.number.ToString();
                arr[1] = item.getFullName();
                lvw.Items.Add(new ListViewItem(arr));
            }
        }

        public void ModificarJugador(List<Player> players, ListView lvw, int nro, string nombrecompleto)
        {
            string[] arr = nombrecompleto.Split(',');
            players.Find(element => element.number == nro).number = nro;
            players.Find(element => element.number == nro).lastName = arr[0];
            players.Find(element => element.number == nro).firstName = arr[1];

            FillTeamPlayers(lvw, players);
        }

        private void clearTeam1Players()
        {
            if (MessageBox.Show("¿Esta seguro de limpiar el listado jugadores locales?", "Jugadores Locales", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HandballMatch.getInstance().team1Players.Clear();
                FillTeamPlayers(lvwEquipoLocal, HandballMatch.getInstance().team1Players);
            }
        }

        private void clearTeam2Players()
        {
            if (MessageBox.Show("¿Esta seguro de limpiar el listado jugadores visitantes?", "Jugadores Visitantes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HandballMatch.getInstance().team2Players.Clear();
                FillTeamPlayers(lvwEquipoVisitante, HandballMatch.getInstance().team2Players);
            }
        }

        private void openFile()
        {
            System.Windows.Forms.OpenFileDialog fdlg = new System.Windows.Forms.OpenFileDialog();
            fdlg.DefaultExt = "xml";
            fdlg.Filter = "Datos-XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*||";
            fdlg.FilterIndex = 0;
            //fdlg.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments

            if (fdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HandballMatch.getInstance().readXML(fdlg.FileName);
                fileName = fdlg.FileName;
                this.Text = String.Format("Handball Cliente - [{0}]", fdlg.FileName);
                getMatchValues();
            }
        }

        private void setMatchValues()
        {
            HandballMatch.getInstance().name = txtNombre.Text;
            HandballMatch.getInstance().description = txtDescripcion.Text;
            HandballMatch.getInstance().fecha = dtpFecha.Value;
            HandballMatch.getInstance().team1Name = txtNombreLocal.Text;
            HandballMatch.getInstance().team2Name = txtNombreVisitante.Text;
            HandballMatch.getInstance().leagueName = txtCampeonato.Text;
            HandballMatch.getInstance().location = txtLugar.Text;
            HandballMatch.getInstance().eventTitle = txtTitulo.Text;
            HandballMatch.getInstance().team1ScoreName = txtNombreScoreLocal.Text;
            HandballMatch.getInstance().team2ScoreName = txtNombreScoreVisitante.Text;

            HandballMatch.getInstance().serverAddress = txtDireccionServidor.Text;
            HandballMatch.getInstance().serverPort = txtPuertoServidor.Text;

            HandballMatch.getInstance().templatePresentation = cmbTemplatePresentacion.Text;
            HandballMatch.getInstance().templateTeam = cmbTemplateEquipos.Text;
            HandballMatch.getInstance().templateScoreboard = cmbTemplateScoreboard.Text;
            HandballMatch.getInstance().templateResult = cmbTemplateResultado.Text;
            HandballMatch.getInstance().templateLowerThird = cmbTemplateLowerThird.Text;
            HandballMatch.getInstance().templatePositions = cmbTemplatePositions.Text;
            HandballMatch.getInstance().templateTwitter = cmbTemplateTwitter.Text;

            HandballMatch.getInstance().imageLogoBroadcast = cmbLogoTransmision.Text;
            HandballMatch.getInstance().imageCredits = cmbImageScrolling.Text;
            HandballMatch.getInstance().speedImageCredits = trkImageScrollingSpeed.Value;

            HandballMatch.getInstance().team1Logo = cmbLogoLocal.Text;
            HandballMatch.getInstance().team2Logo = cmbLogoVisitante.Text;
            HandballMatch.getInstance().leagueLogo = cmbLogoFederacion.Text;
            HandballMatch.getInstance().team1Coach = txtEntrenadorLocal.Text;
            HandballMatch.getInstance().team2Coach = txtEntrenadorVisitante.Text;
            HandballMatch.getInstance().recordingFileName = txtArchivoGrabacion.Text;
            HandballMatch.getInstance().positionsTitle = txtPositionsTitle.Text;
            HandballMatch.getInstance().positionsSubtitle = txtPositionsSubtitle.Text;

            HandballMatch.getInstance().autoHideIntro = chkAutoOcultarPresentacion.Checked;
            HandballMatch.getInstance().autoHideIntroSeconds = ((int)nudSegundosAutoOcultarPresentacion.Value);
            HandballMatch.getInstance().autoHideTeam1 = chkAutoOcultarPresentacion.Checked;
            HandballMatch.getInstance().autoHideTeam1Seconds = ((int)nudSegundosAutoOcultarPresentacion.Value);
            HandballMatch.getInstance().autoHideTeam2 = chkAutoOcultarPresentacion.Checked;
            HandballMatch.getInstance().autoHideTeam2Seconds = ((int)nudSegundosAutoOcultarPresentacion.Value);

            HandballMatch.getInstance().autoShowHideTeamsSeconds = ((int)nudSegundosOcultarEquipoLocalVisitante.Value);
            HandballMatch.getInstance().autoShowHideTeamsInBetweenSeconds = ((int)nudSegundosSeparacionEquipoLocalVisitante.Value);

            HandballMatch.getInstance().autoHideResult = chkAutoHideResult.Checked;
            HandballMatch.getInstance().autoHideResultSeconds = ((int)nudSeconsAutoHideResult.Value);
            HandballMatch.getInstance().autoHideLowerThird = chkAutoOcultarLT.Checked;
            HandballMatch.getInstance().autoHideLowerThirdSeconds = ((int)nudSegundosAutoOcultarLT.Value);

            HandballMatch.getInstance().autoHidePositions = chkAutoHidePositions.Checked;
            HandballMatch.getInstance().autoHidePositionsSeconds = ((int)nudAutoHidePositionsSeconds.Value);

            HandballMatch.getInstance().autoHideTwitter = chkAutoHideTwitter.Checked;
            HandballMatch.getInstance().autoHideTwitterSeconds = ((int)nudAutoHideTwitterSeconds.Value);
        }

        private void getMatchValues()
        {
            txtNombre.Text = HandballMatch.getInstance().name;
            txtDescripcion.Text = HandballMatch.getInstance().description;
            dtpFecha.Value = HandballMatch.getInstance().fecha;
            txtNombreLocal.Text = HandballMatch.getInstance().team1Name;
            txtNombreVisitante.Text = HandballMatch.getInstance().team2Name;
            txtCampeonato.Text = HandballMatch.getInstance().leagueName;
            txtLugar.Text = HandballMatch.getInstance().location;
            txtTitulo.Text = HandballMatch.getInstance().eventTitle;
            txtNombreScoreLocal.Text = HandballMatch.getInstance().team1ScoreName;
            txtNombreScoreVisitante.Text = HandballMatch.getInstance().team2ScoreName;

            txtDireccionServidor.Text = HandballMatch.getInstance().serverAddress;
            txtPuertoServidor.Text = HandballMatch.getInstance().serverPort;

            cmbTemplatePresentacion.Text = HandballMatch.getInstance().templatePresentation;
            cmbTemplateEquipos.Text = HandballMatch.getInstance().templateTeam;
            cmbTemplateScoreboard.Text = HandballMatch.getInstance().templateScoreboard;
            cmbTemplateResultado.Text = HandballMatch.getInstance().templateResult;
            cmbTemplateLowerThird.Text = HandballMatch.getInstance().templateLowerThird;
            cmbTemplatePositions.Text = HandballMatch.getInstance().templatePositions;
            cmbTemplateTwitter.Text = HandballMatch.getInstance().templateTwitter;

            cmbLogoTransmision.Text = HandballMatch.getInstance().imageLogoBroadcast;
            cmbImageScrolling.Text = HandballMatch.getInstance().imageCredits;
            trkImageScrollingSpeed.Value = HandballMatch.getInstance().speedImageCredits;

            cmbLogoLocal.Text = HandballMatch.getInstance().team1Logo;
            cmbLogoVisitante.Text = HandballMatch.getInstance().team2Logo;
            cmbLogoFederacion.Text = HandballMatch.getInstance().leagueLogo;
            txtEntrenadorLocal.Text = HandballMatch.getInstance().team1Coach;
            txtEntrenadorVisitante.Text = HandballMatch.getInstance().team2Coach;
            txtArchivoGrabacion.Text = HandballMatch.getInstance().recordingFileName;
            txtPositionsTitle.Text = HandballMatch.getInstance().positionsTitle;
            txtPositionsSubtitle.Text = HandballMatch.getInstance().positionsSubtitle;

            FillTeamPlayers(lvwEquipoLocal, HandballMatch.getInstance().team1Players);
            FillTeamPlayers(lvwEquipoVisitante, HandballMatch.getInstance().team2Players);
            FillPositionTable();
            
            chkAutoOcultarPresentacion.Checked = HandballMatch.getInstance().autoHideIntro;
            nudSegundosAutoOcultarPresentacion.Value = HandballMatch.getInstance().autoHideIntroSeconds;
            chkAutoOcultarEquipoLocal.Checked = HandballMatch.getInstance().autoHideTeam1;
            nudSegundosOcultarEquipoLocal.Value = HandballMatch.getInstance().autoHideTeam1Seconds;
            chkAutoOcultarEquipoVisitante.Checked = HandballMatch.getInstance().autoHideTeam2;
            nudSegundosOcultarEquipoVisitante.Value = HandballMatch.getInstance().autoHideTeam2Seconds;

            nudSegundosOcultarEquipoLocalVisitante.Value = HandballMatch.getInstance().autoShowHideTeamsSeconds;
            nudSegundosSeparacionEquipoLocalVisitante.Value = HandballMatch.getInstance().autoShowHideTeamsInBetweenSeconds;

            chkAutoHideResult.Checked = HandballMatch.getInstance().autoHideResult;
            nudSeconsAutoHideResult.Value = HandballMatch.getInstance().autoHideResultSeconds;
            chkAutoOcultarLT.Checked = HandballMatch.getInstance().autoHideLowerThird;
            nudSegundosAutoOcultarLT.Value = HandballMatch.getInstance().autoHideLowerThirdSeconds;

            chkAutoHidePositions.Checked = HandballMatch.getInstance().autoHidePositions;
            nudAutoHidePositionsSeconds.Value = HandballMatch.getInstance().autoHidePositionsSeconds;

            chkAutoHideTwitter.Checked = HandballMatch.getInstance().autoHideTwitter;
            nudAutoHideTwitterSeconds.Value = HandballMatch.getInstance().autoHideTwitterSeconds;
        }

        private void saveFile()
        {
            setMatchValues();
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    HandballMatch.getInstance().saveXML(fileName);
                }
                else
                {
                    saveAsFile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text);
            }
        }

        private void saveAsFile()
        {
            System.Windows.Forms.SaveFileDialog fdlg = new System.Windows.Forms.SaveFileDialog();
            fdlg.DefaultExt = "xml";
            fdlg.Filter = "Datos-XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*||";
            fdlg.FilterIndex = 0;
            //fdlg.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments;

            if (fdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HandballMatch.getInstance().saveXML(fdlg.FileName);
                fileName = fdlg.FileName;
                this.Text = String.Format("Handball Cliente - [{0}]", fdlg.FileName);
            }
        }

        private void removePlayerTeam1()
        {
            if (lvwEquipoLocal.SelectedItems != null)
            {
                HandballMatch.getInstance().team1Players.Remove(HandballMatch.getInstance().team1Players.Find(element => element.number == int.Parse(lvwEquipoLocal.SelectedItems[0].Text)));
                FillTeamPlayers(lvwEquipoLocal, HandballMatch.getInstance().team1Players);
            }
        }

        private void removePlayerTeam2()
        {
            if (lvwEquipoVisitante.SelectedItems != null)
            {
                HandballMatch.getInstance().team2Players.Remove(HandballMatch.getInstance().team2Players.Find(element => element.number == int.Parse(lvwEquipoVisitante.SelectedItems[0].Text)));
                FillTeamPlayers(lvwEquipoVisitante, HandballMatch.getInstance().team2Players);
            }
        }

        private void editPlayerTeam1()
        {
            if (lvwEquipoLocal.SelectedItems != null)
            {
                Player aux = HandballMatch.getInstance().team1Players.Find(element => element.number == int.Parse(lvwEquipoLocal.SelectedItems[0].Text));
                Jugador f1 = new Jugador(this, 2, aux.number.ToString(), aux.getFullName());
                f1.ShowDialog(this);
            }
        }

        private void editPlayerTeam2()
        {
            if (lvwEquipoVisitante.SelectedItems != null)
            {
                Player aux = HandballMatch.getInstance().team2Players.Find(element => element.number == int.Parse(lvwEquipoVisitante.SelectedItems[0].Text));
                Jugador f1 = new Jugador(this, 4, aux.number.ToString(), aux.getFullName());
                f1.ShowDialog(this);
            }
        }

        private void loadPlayersTeam1()
        {
            System.Windows.Forms.SaveFileDialog fdlg = new System.Windows.Forms.SaveFileDialog();
            fdlg.FilterIndex = 0;
            fdlg.Title = "Cargar Archivo de Jugadores Locales";
            fdlg.Filter = "Equipo (*.team)|*.team|Todos los archivos (*.*)|*.*";
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                //
            }
        }

        private void loadPlayersTeam2()
        {

        }

        private void addPlayerTeam1()
        {
            Jugador f1 = new Jugador(this, 1, getNextPlayerNumber(HandballMatch.getInstance().team1Players).ToString());
            f1.ShowDialog(this);
        }

        private void addPlayerTeam2()
        {
            Jugador f1 = new Jugador(this, 3, getNextPlayerNumber(HandballMatch.getInstance().team2Players).ToString());
            f1.ShowDialog(this);
        }

        public int getNextPlayerNumber(List<Player> team)
        {
            int aux=1;
            if (team.Count > 0)
            {
                aux = team.Last().number + aux;
            }
            return aux;
        }

        public void AddPositionToTable(Position pos)
        {
            if (!HandballMatch.getInstance().positions.Exists(element => element.team == pos.team))
            {
                HandballMatch.getInstance().positions.Add(pos);
                FillPositionTable();
            }
        }

        public void FillPositionTable()
        {
            SortPositionsTable();
            string[] arr;
            int i = 1;
            lvwPositions.Items.Clear();
            foreach (Position item in HandballMatch.getInstance().positions)
            {
                arr = new string[10];
                arr[0] = i.ToString();
                arr[1] = item.team.ToString();
                arr[2] = item.points.ToString();
                arr[3] = item.played.ToString();
                arr[4] = item.won.ToString();
                arr[5] = item.drawn.ToString();
                arr[6] = item.lost.ToString();
                arr[7] = item.goalsFor.ToString();
                arr[8] = item.goalsAgainst.ToString();
                arr[9] = item.goalDifference.ToString();
                lvwPositions.Items.Add(new ListViewItem(arr));
                i++;
            }
        }

        private void SortPositionsTable()
        {
            var sorted=HandballMatch.getInstance().positions.OrderByDescending(a => a.points);
            HandballMatch.getInstance().positions.Sort(delegate(Position x, Position y) 
            { 
                int a = y.points.CompareTo(x.points);
                if (a == 0)
                {
                    a = y.goalDifference.CompareTo(x.goalDifference);
                }
                return a;
            });
        }

        public void EditPositionTable(Position pos)
        {
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).team = pos.team;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).points = pos.points;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).played = pos.played;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).won = pos.won;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).drawn = pos.drawn;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).lost = pos.lost;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).goalsFor = pos.goalsFor;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).goalsAgainst = pos.goalsAgainst;
            HandballMatch.getInstance().positions.Find(element => element.team == pos.team).goalDifference = pos.goalDifference;
            FillPositionTable();
        }

        private void clearPositionTable()
        {
            if (MessageBox.Show("¿Esta seguro de limpiar la tabla de Posiciones?", "Tabla Posiciones", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HandballMatch.getInstance().positions = new List<Position>();
                FillPositionTable();
            }
        }

        private void removePosition()
        {
            if (lvwPositions.SelectedItems != null)
            {
                HandballMatch.getInstance().positions.Remove(HandballMatch.getInstance().positions.Find(element => element.team == lvwPositions.SelectedItems[0].SubItems[1].Text));
                FillPositionTable();
            }
        }

        private void editPosition()
        {
            if (lvwEquipoLocal.SelectedItems != null)
            {
                Position aux = HandballMatch.getInstance().positions.Find(element => element.team == lvwPositions.SelectedItems[0].SubItems[1].Text);
                Posicion f1 = new Posicion(this, 2, aux);
                f1.ShowDialog(this);
            }
        }

        private void addPosition()
        {
            Posicion f1 = new Posicion(this, 1);
            f1.ShowDialog(this);
        }

        # region eventos
        private void mnuSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiarEquipoLocal_Click(object sender, EventArgs e)
        {
            clearTeam1Players();
        }

        private void btnLimpiarEquipoVisitante_Click(object sender, EventArgs e)
        {
            clearTeam2Players();
        }


        private void btnAgregarJugadorLocal_Click(object sender, EventArgs e)
        {
            addPlayerTeam1();
        }

        private void btnAgregarJugadorVisitante_Click(object sender, EventArgs e)
        {
            addPlayerTeam2();
        }

        private void btnModificarJugadorLocal_Click(object sender, EventArgs e)
        {
            editPlayerTeam1();
        }

        private void btnModificarJugadorVisitante_Click(object sender, EventArgs e)
        {
            editPlayerTeam2();
        }

        private void btnCargarJugadoresLocales_Click(object sender, EventArgs e)
        {
            loadPlayersTeam1();
        }

        private void btnQuitarJugadorLocal_Click(object sender, EventArgs e)
        {
            removePlayerTeam1();
        }

        private void btnQuitarJugadorVisitante_Click(object sender, EventArgs e)
        {
            removePlayerTeam2();
        }

        # endregion

        private void btnConexion_Click(object sender, EventArgs e)
        {
            connectToCasparCGServer();
        }

        private void btnUpdateTemplates_Click(object sender, EventArgs e)
        {
            getServerTemplates();
        }

        private void btnParaGraficoScoreboard_Click(object sender, EventArgs e)
        {
            stopScoreboard();
        }

        private void btnMostrarOcultarPresentacion_Click(object sender, EventArgs e)
        {
            showHideIntro();
        }

        private void btnClearChannel_Click(object sender, EventArgs e)
        {
            clearChannel();
        }

        private void btnLockUnlock_Click(object sender, EventArgs e)
        {
            lockUnlockTemplates();
        }

        private void btnIniciarGraficoScoreboard_Click(object sender, EventArgs e)
        {
            startScoreboard();
        }

        private void btnMostrarOcultarScoreboard_Click(object sender, EventArgs e)
        {
            showHideScoreboard();
        }

        private void btnIniciarReloj_Click(object sender, EventArgs e)
        {
            startScoreboardClock();
        }

        private void btnPausarReloj_Click(object sender, EventArgs e)
        {
            startScoreboardClock();
        }

        private void btn1GolLocal_Click(object sender, EventArgs e)
        {
            addOneScoreTeam1();
        }

        private void btn1GolVisitante_Click(object sender, EventArgs e)
        {
            addOneScoreTeam2();
        }

        private void btnIniciarLT_Click(object sender, EventArgs e)
        {
            startLowerThird();
        }

        private void btnPararLT_Click(object sender, EventArgs e)
        {
            stopLowerThird();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                this.tabControl1.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.F3)
            {
                this.tabControl1.SelectedIndex = 1;
            }
            else if (e.KeyCode == Keys.F4)
            {
                this.tabControl1.SelectedIndex = 2;
            }
            else if (e.KeyCode == Keys.F5)
            {
                this.tabControl1.SelectedIndex = 3;
            }
            else if (e.KeyCode == Keys.F6)
            {
                this.tabControl1.SelectedIndex = 4;
            }
            else if (e.KeyCode == Keys.F7)
            {
                this.tabControl1.SelectedIndex = 5;
            }
            else if (e.KeyCode == Keys.F8)
            {
                this.tabControl1.SelectedIndex = 6;
            }
            else if (e.KeyCode == Keys.F9)
            {
                this.tabControl1.SelectedIndex = 7;
            }
        }

        private void tmrLT_Tick(object sender, EventArgs e)
        {
            stopLowerThird();
        }

        private void btnGetVideoFiles_Click(object sender, EventArgs e)
        {
            getServerVideoFiles();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            startRecording();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            stopRecording();
        }

        private void btnStartLogo_Click(object sender, EventArgs e)
        {
            showLogo();
        }

        private void btnStopLogo_Click(object sender, EventArgs e)
        {
            hideLogo();
        }

        private void btnPlayVideo_Click(object sender, EventArgs e)
        {
            playMedia();
        }

        private void btnPauseVideo_Click(object sender, EventArgs e)
        {
            pauseMedia();
        }

        private void btnStopVideo_Click(object sender, EventArgs e)
        {
            stopMedia();
        }

        private void btnGetLogoFiles_Click(object sender, EventArgs e)
        {
            getServerImageFiles();
        }

        private void tmrIntro_Tick(object sender, EventArgs e)
        {
            showHideIntro();
        }

        private void radTeam1List_CheckedChanged(object sender, EventArgs e)
        {
            loadPlayerLT();
        }

        private void radTeam2List_CheckedChanged(object sender, EventArgs e)
        {
            loadPlayerLT();
        }

        private void lvwJugadores_DoubleClick(object sender, EventArgs e)
        {
            getPlayerInfoToLT();
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            muteVolume();
        }

        private void trkVolume_Scroll(object sender, EventArgs e)
        {

        }

        private void trkVolume_ValueChanged(object sender, EventArgs e)
        {
            setVolume();
        }

        private void mnuSiempreVisible_CheckedChanged(object sender, EventArgs e)
        {
            setTopMost(mnuSiempreVisible.Checked);
        }

        private void btnRefreshImageScrolling_Click(object sender, EventArgs e)
        {
            getServerImageFiles();
        }

        private void btnStartImageScrolling_Click(object sender, EventArgs e)
        {
            startImageScrolling();
        }

        private void btnStopImageScrolling_Click(object sender, EventArgs e)
        {
            stopImageScrolling();
        }

        private void btnConfigRecording_Click(object sender, EventArgs e)
        {
            openConfigRecording();
        }

        private void mnuGuardar_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void mnuGuardarComo_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void mnuNuevo_Click(object sender, EventArgs e)
        {
            NewMatch();
        }

        private void mnuAbrir_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void btnMostrarEquipoVisitante_Click(object sender, EventArgs e)
        {
            showHideTeamsheet(btnMostrarEquipoVisitante, txtNombreVisitante, lvwEquipoVisitante, txtEntrenadorVisitante, cmbLogoVisitante, chkAutoOcultarEquipoVisitante, nudSegundosOcultarEquipoVisitante, tmrTeam2);
        }

        private void btnMostrarEquipoLocal_Click(object sender, EventArgs e)
        {
            showHideTeamsheet(btnMostrarEquipoLocal, txtNombreLocal, lvwEquipoLocal, txtEntrenadorLocal, cmbLogoLocal, chkAutoOcultarEquipoLocal, nudSegundosOcultarEquipoLocal, tmrTeam1);
        }

        private void btnMostrarResultado_Click(object sender, EventArgs e)
        {
            showHideResult();
        }

        private void btnReiniciarReloj_Click(object sender, EventArgs e)
        {
            resetScoreboardClock();
        }

        private void nudScoreLocal_ValueChanged(object sender, EventArgs e)
        {
            updateTeamsScore();
        }

        private void nudScoreVisitante_ValueChanged(object sender, EventArgs e)
        {
            updateTeamsScore();
        }

        private void tmrTeam1_Tick(object sender, EventArgs e)
        {
            showHideTeamsheet(btnMostrarEquipoLocal, txtNombreLocal, lvwEquipoLocal, txtEntrenadorLocal, cmbLogoLocal, chkAutoOcultarEquipoLocal, nudSegundosOcultarEquipoLocal, tmrTeam1);
        }

        private void tmrTeam2_Tick(object sender, EventArgs e)
        {
            showHideTeamsheet(btnMostrarEquipoVisitante, txtNombreVisitante, lvwEquipoVisitante, txtEntrenadorVisitante, cmbLogoVisitante, chkAutoOcultarEquipoVisitante, nudSegundosOcultarEquipoVisitante, tmrTeam2);
        }

        private void tmrResult_Tick(object sender, EventArgs e)
        {
            showHideResult();
        }

        private void btnMostrarEquipoLocalVisitante_Click(object sender, EventArgs e)
        {
            autoShowHideTeams();
        }

        private void tmrTeams_Tick(object sender, EventArgs e)
        {
            autoShowHideTeams();
        }

        private void tmrInBetweenTeams_Tick(object sender, EventArgs e)
        {
            autoShowHideTeams();
        }

        private void btnShowPositions_Click(object sender, EventArgs e)
        {
            startPositions();
        }

        private void btnHidePositions_Click(object sender, EventArgs e)
        {
            stopPositions();
        }

        private void tmrPositions_Tick(object sender, EventArgs e)
        {
            stopPositions();
        }

        private void btnAddPosition_Click(object sender, EventArgs e)
        {
            addPosition();
        }

        private void btnRemovePosition_Click(object sender, EventArgs e)
        {
            removePosition();
        }

        private void btnEditPosition_Click(object sender, EventArgs e)
        {
            editPosition();
        }

        private void btnClearTable_Click(object sender, EventArgs e)
        {
            clearPositionTable();
        }

        private void tmrTwitter_Tick(object sender, EventArgs e)
        {
            stopTwitter();
        }

        private void btnStartTwitter_Click(object sender, EventArgs e)
        {
            startTwitter();
        }

        private void btnStopTwitter_Click(object sender, EventArgs e)
        {
            stopTwitter();
        }
    }
}
