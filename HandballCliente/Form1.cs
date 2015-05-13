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
        private Color ColorRecording;


        private const int layerPresentation = 5;
        private const int layerTeams = 10;
        private const int layerScoreboard = 15;
        private const int layerResult = 20;
        private const int layerLowerThird = 25;
        private const int layerPositions = 30;
        private const int layerTwitter = 35;

        private const int layerVolleyScoreboard = 40;
        private const int layerVolleyResult = 45;

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
            ColorFondo = stsStatus.BackColor;
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
            txtEventName.Text = "";
            txtEventDescription.Text = "";
            dtpEventDate.Value = DateTime.Now;
            clearParameters();
            clearPresentation();
            clearTeams();
            clearScoreboard();
            clearMultimedia();
        }

        #region ClearData
        private void clearParameters()
        {
            txtServerAddress.Text = "";
            txtServerPort.Text = "";
            cmbTemplateTeam.Items.Clear();
            cmbTemplateLowerThird.Items.Clear();
            cmbTemplateIntro.Items.Clear();
            cmbTemplateResult.Items.Clear();
            cmbTemplateScoreboard.Items.Clear();
            cmbTemplatePositions.Items.Clear();
            cmbTemplateTwitter.Items.Clear();
            cmbTemplateVolleyScoreboard.Items.Clear();
        }

        private void clearPresentation()
        {

        }

        private void clearTeams()
        {
            lvwHomeTeamPlayers.Items.Clear();
            lvwGuestTeamPlayers.Items.Clear();
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
            txtServerAddress.Text = casparServer.ServerAdress;
            txtServerPort.Text = casparServer.Port.ToString();
            btnConnectDisconnect.Text = casparServer.Connected ? "Desconectar" : "Conectar";
            btnClearChannel.Enabled = casparServer.Connected;
            btnShowHideIntro.Tag = "0";
            btnShowHideHomeTeam.Tag = "0";
            btnShowHideGuestTeam.Tag = "0";
            btnShowHomeAndGuestTeam.Tag = "0";
            btnShowHideResult.Tag = "0";
            lockUnlockTemplates();
            radHomeTeamPlayers.Checked = true;
            trkVolume.Value = 10;
            trkImageScrollingSpeed.Value = 1;

            fillCombosTeamTextStyle();
            fillComboWebcam();

            nudVolleySetsPerMatch.Value = 5;
            nudVolleyPointsPerSet.Value = 11;
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

            cmbVolleyScoreboardFontSize.Items.Clear();
            cmbVolleyScoreboardFontSize.Items.Add("12");
            cmbVolleyScoreboardFontSize.Items.Add("14");
            cmbVolleyScoreboardFontSize.Items.Add("16");
            cmbVolleyScoreboardFontSize.Items.Add("18");
            cmbVolleyScoreboardFontSize.Items.Add("20");
            cmbVolleyScoreboardFontSize.Items.Add("22");
            cmbVolleyScoreboardFontSize.Items.Add("24");
        }

        private void fillComboWebcam()
        {
            cmbWebcam.Items.Clear();
            cmbWebcam.Items.Add("Logitech HD Webcam C270");
            cmbWebcam.Items.Add("WebCam SC-0311139N");

            cmbWebcamResolution.Items.Clear();
            cmbWebcamResolution.Items.Add("640x480");
            cmbWebcamResolution.Items.Add("800x600");
            cmbWebcamResolution.Items.Add("1024x576");
            cmbWebcamResolution.Items.Add("1280x720");

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
                    getServerAudioFiles();
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
                btnConnectDisconnect.Text = casparServer.Connected ? "Desconectar" : "Conectar";
                btnClearChannel.Enabled = casparServer.Connected;
                lockUnlockTemplates();
                //this.BackColor = casparServer.Connected ? Color.ForestGreen : ColorFondo;
                stsStatus.BackColor = casparServer.Connected ? Color.ForestGreen : ColorFondo;
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
                fillCombosTemplate(cmbTemplateTeam, templates);
                fillCombosTemplate(cmbTemplateIntro, templates);
                fillCombosTemplate(cmbTemplateResult, templates);
                fillCombosTemplate(cmbTemplateScoreboard, templates);
                fillCombosTemplate(cmbTemplateLowerThird, templates);
                fillCombosTemplate(cmbTemplatePositions, templates);
                fillCombosTemplate(cmbTemplateTwitter, templates);
                fillCombosTemplate(cmbTemplateVolleyScoreboard, templates);
                fillCombosTemplate(cmbTemplateVolleyResult,templates);
            }
        }

        private void getServerImageFiles()
        {
            if (casparServer.Connected)
            {
                List<String> medias = casparServer.GetMediaClipsNames(CasparCG.MediaTypes.Still);
                fillCombosTemplate(cmbHomeTeamLogo, medias);
                fillCombosTemplate(cmbGuestTeamLogo, medias);
                fillCombosTemplate(cmbFederationLogo, medias);
                fillCombosTemplate(cmbHomeTeamLogo, medias);
                fillCombosTemplate(cmbGuestTeamLogo, medias);
                fillCombosTemplate(cmbBroadcastLogo, medias);
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
                fillCombosTemplate(cmbAudioFiles, medias);
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
            cmbTemplateTeam.Enabled = !cmbTemplateTeam.Enabled;
            cmbTemplateLowerThird.Enabled = !cmbTemplateLowerThird.Enabled;
            cmbTemplateIntro.Enabled = !cmbTemplateIntro.Enabled;
            cmbTemplateResult.Enabled = !cmbTemplateResult.Enabled;
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

            aux = (cmbHalf.Text.Length > 0);

            return aux;
        }

        private void startScoreboard()
        {
            if (checkDataScoreboard())
            {
                if (casparServer.Connected)
                {
                    Template templateScoreboard = new Template();
                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbFederationLogo.Text.ToLower() + ".png");

                    templateScoreboard.Fields.Add(new TemplateField("team1Name", txtNombreScoreLocal.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team2Name", txtNombreScoreVisitante.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team1Score", nudHomeTeamScore.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("team2Score", nudGuestTeamScore.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("gameTime", nudClockMinutes.Value.ToString() + ":" + nudClockSeconds.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("halfNum", cmbHalf.Text));
                    templateScoreboard.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), cmbTemplateScoreboard.Text, layerScoreboard.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnShowHideIntro.Tag = "1";
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
                if (chkAutoShowOnClockStart.Checked)
                {
                    showHideScoreboard();
                }

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

            templateUpdateScore.Fields.Add(new TemplateField("gameTime", nudClockMinutes.Value.ToString() + ":" + nudClockSeconds.Value.ToString()));

            //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
            ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
            System.Diagnostics.Debug.WriteLine(ri.Message);
        }

        public void addOneScoreTeam1()
        {
            nudHomeTeamScore.UpButton();
        }

        public void addOneScoreTeam2()
        {
            nudGuestTeamScore.UpButton();
        }

        private void updateTeamsScore()
        {
            if (casparServer.Connected)
            {
                Template templateUpdateScore = new Template();

                templateUpdateScore.Fields.Add(new TemplateField("team1Score", nudHomeTeamScore.Value.ToString()));
                templateUpdateScore.Fields.Add(new TemplateField("team2Score", nudGuestTeamScore.Value.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void showHideIntro()
        {
            if (casparServer.Connected)
            {
                if (!btnShowHideIntro.Tag.ToString().Equals("1"))
                {
                    Template templateIntro = new Template();

                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");

                    templateIntro.Fields.Add(new TemplateField("team1Name", txtHomeTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtHomeTeamName.Text.Split(',')[1]));
                    templateIntro.Fields.Add(new TemplateField("team2Name", txtGuestTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtGuestTeamName.Text.Split(',')[1]));
                    templateIntro.Fields.Add(new TemplateField("infoLeague", txtEventLeague.Text));
                    templateIntro.Fields.Add(new TemplateField("infoDate", txtIntroTitle.Text));
                    templateIntro.Fields.Add(new TemplateField("infoLocation", txtEventLocation.Text));
                    templateIntro.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                    templateIntro.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));
                    templateIntro.Fields.Add(new TemplateField("website", txtVolleyWebsite.Text));


                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateIntro.Text, layerPresentation.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnShowHideIntro.Tag = "1";
                    if (chkAutoHideIntro.Checked)
                    {
                        tmrIntro.Interval = ((int)nudAutoHideIntroSeconds.Value) * 1000;
                        tmrIntro.Enabled = true;
                        tmrIntro.Start();
                    }
                }
                else
                {
                    casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
                    btnShowHideIntro.Tag = "0";
                    if (chkAutoHideIntro.Checked)
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

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateTeam.Text, layerTeams.ToString()));
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
                switch (btnShowHomeAndGuestTeam.Tag.ToString())
                {
                    case "0":
                        btnShowHomeAndGuestTeam.Tag = "1";
                        String players;
                        players = getPlayerList(lvwHomeTeamPlayers);

                        Template templateIntro = new Template();

                        Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtHomeTeamName.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtHomeTeamCoach.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateTeam.Text, layerTeams.ToString()));

                        tmrTeams.Interval = ((int)nudTeamsShowSeconds.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "1":
                        btnShowHomeAndGuestTeam.Tag = "2";
                        casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));

                        tmrTeams.Stop();
                        tmrTeams.Enabled = false;

                        tmrInBetweenTeams.Interval = ((int)nudSeparationTeamsSeconds.Value) * 1000;
                        tmrInBetweenTeams.Enabled = true;
                        tmrInBetweenTeams.Start();
                        break;
                    case "2":
                        btnShowHomeAndGuestTeam.Tag = "3";
                        players = getPlayerList(lvwGuestTeamPlayers);

                        templateIntro = new Template();

                        logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtGuestTeamName.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtGuestTeamPlayers.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateTeam.Text, layerTeams.ToString()));

                        tmrInBetweenTeams.Stop();
                        tmrInBetweenTeams.Enabled = false;

                        tmrTeams.Interval = ((int)nudTeamsShowSeconds.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "3":
                        btnShowHomeAndGuestTeam.Tag = "0";
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
                if (!btnShowHideResult.Tag.ToString().Equals("1"))
                {
                    Template templateResult = new Template();

                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");

                    templateResult.Fields.Add(new TemplateField("team1Name", txtHomeTeamName.Text));
                    templateResult.Fields.Add(new TemplateField("team2Name", txtGuestTeamName.Text));
                    templateResult.Fields.Add(new TemplateField("team1Score", nudHomeTeamScore.Value.ToString()));
                    templateResult.Fields.Add(new TemplateField("team2Score", nudGuestTeamScore.Value.ToString()));
                    templateResult.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                    templateResult.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));
                    templateResult.Fields.Add(new TemplateField("timeResult", ""));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateResult.TemplateDataText(), cmbTemplateResult.Text, layerResult.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnShowHideResult.Tag = "1";
                    if (chkAutoHideResult.Checked)
                    {
                        tmrResult.Interval = ((int)nudAutoHideResultSeconds.Value) * 1000;
                        tmrResult.Enabled = true;
                        tmrResult.Start();
                    }
                }
                else
                {
                    casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerResult.ToString()));
                    btnShowHideResult.Tag = "0";
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

                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + ((radHomeTeamPlayers.Checked) ? cmbHomeTeamLogo.Text.ToLower() : cmbGuestTeamLogo.Text.ToLower()) + ".png");

                    templateLowerThird.Fields.Add(new TemplateField("teamPlayer", txtLTTitle.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamName", txtLTSubtitle.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamLogo", logoPath.AbsoluteUri));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateLowerThird.TemplateDataText(), cmbTemplateLowerThird.Text, layerLowerThird.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                    btnShowHideIntro.Tag = "1";

                    if (chkAutoHideLT.Checked)
                    {
                        tmrLT.Interval = ((int)nudAutoHideLTSeconds.Value) * 1000;
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
                if (chkAutoHideLT.Checked)
                {
                    tmrLT.Stop();
                    tmrLT.Enabled = false;
                }
            }
        }

        private void loadPlayerLT()
        {
            string[] arr = new string[2];
            if (radHomeTeamPlayers.Checked)
            {
                lvwTeamPlayers.Items.Clear();
                foreach (ListViewItem item in lvwHomeTeamPlayers.Items)
                {
                    arr[0] = item.SubItems[0].Text;
                    arr[1] = item.SubItems[1].Text;
                    lvwTeamPlayers.Items.Add(new ListViewItem(arr));
                }
            }
            else
            {
                lvwTeamPlayers.Items.Clear();
                foreach (ListViewItem item in lvwGuestTeamPlayers.Items)
                {
                    arr[0] = item.SubItems[0].Text;
                    arr[1] = item.SubItems[1].Text;
                    lvwTeamPlayers.Items.Add(new ListViewItem(arr));
                }
            }
        }

        private void getPlayerInfoToLT()
        {
            if (lvwTeamPlayers.SelectedItems != null)
            {
                txtLTTitle.Text = lvwTeamPlayers.SelectedItems[0].SubItems[1].Text;
                txtLTSubtitle.Text = radHomeTeamPlayers.Checked ? txtHomeTeamName.Text : txtGuestTeamName.Text;
            }
        }

        private void startPositions()
        {
            if (cmbTemplatePositions.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templatePositions = new Template();

                    Uri logoPath = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbFederationLogo.Text.ToLower() + ".png");

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

        private void startVolleyScoreboard()
        {
            if (cmbTemplateVolleyScoreboard.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templateVolleyScoreboard = new Template();
                    //Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    //Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");
                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbFederationLogo.Text.ToLower() + ".png");

                    templateVolleyScoreboard.Fields.Add(new TemplateField("team1", txtVolleyHomeTeam.Text));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("team2", txtVolleyGuestTeam.Text));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f2", nudVolleyHomeSets.Value.ToString()));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f3", nudVolleyGuestSets.Value.ToString()));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f4", nudVolleyHome1SetPoints.Value.ToString()));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f5", nudVolleyGuest1SetPoints.Value.ToString()));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f6", txtVolleyTitle.Text));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("f7", txtVolleyWebsite.Text));
                    if (chkVolleyShowService.Checked)
                    {
                        templateVolleyScoreboard.Fields.Add(new TemplateField("pass", (radVolleyHomeServe.Checked) ? "1" : "2"));
                    }
                    else
                    {
                        templateVolleyScoreboard.Fields.Add(new TemplateField("pass", "0"));
                    }
                    templateVolleyScoreboard.Fields.Add(new TemplateField("flag1", logo1Path.ToString()));
                    //templateVolleyScoreboard.Fields.Add(new TemplateField("flag2", logo2Path.ToString()));
                    templateVolleyScoreboard.Fields.Add(new TemplateField("fontsize", cmbVolleyScoreboardFontSize.Text));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateVolleyScoreboard.TemplateDataText(), cmbTemplateVolleyScoreboard.Text, layerVolleyScoreboard.ToString()));

                    //if (chkAutoHideVolleyScoreboard.Checked)
                    //{
                    //    tmrTwitter.Interval = ((int)nudAutoHideVolleyScoreboardSeconds.Value) * 1000;
                    //    tmrTwitter.Enabled = true;
                    //    tmrTwitter.Start();
                    //}
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private void stopVolleyScoreboard()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerVolleyScoreboard.ToString()));
                //if (chkAutoHideVolleyScoreboard.Checked)
                //{
                //    tmrTwitter.Stop();
                //    tmrTwitter.Enabled = false;
                //}
            }
        }

        private void updateVolleyScoreboard()
        {
            if (casparServer.Connected)
            {
                Template templateVolleyScoreboard = new Template();
                //Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");
                //Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");
                Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbFederationLogo.Text.ToLower() + ".png");


                templateVolleyScoreboard.Fields.Add(new TemplateField("team1", txtVolleyHomeTeam.Text));
                templateVolleyScoreboard.Fields.Add(new TemplateField("team2", txtVolleyGuestTeam.Text));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f2", nudVolleyHomeSets.Value.ToString()));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f3", nudVolleyGuestSets.Value.ToString()));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f4", getVolleySetScore(1).Value.ToString()));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f5", getVolleySetScore(2).Value.ToString()));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f6", txtVolleyTitle.Text));
                templateVolleyScoreboard.Fields.Add(new TemplateField("f7", txtVolleyWebsite.Text));
                if (chkVolleyShowService.Checked)
                {
                    templateVolleyScoreboard.Fields.Add(new TemplateField("pass", (radVolleyHomeServe.Checked) ? "1" : "2"));
                }
                else
                {
                    templateVolleyScoreboard.Fields.Add(new TemplateField("pass", "0"));
                }
                templateVolleyScoreboard.Fields.Add(new TemplateField("flag1", logo1Path.ToString()));
                //templateVolleyScoreboard.Fields.Add(new TemplateField("flag2", logo2Path.ToString()));
                templateVolleyScoreboard.Fields.Add(new TemplateField("fontsize", cmbVolleyScoreboardFontSize.Text));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateVolleyScoreboard.TemplateDataText(), layerVolleyScoreboard.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void addVolleyGamePoint(Button SourceButton)
        {
            getVolleySetScore((SourceButton.Name.Contains("Home") ? 1 : 2)).Value++;

            if (SourceButton.Name.Contains("Home"))
            {
                radVolleyHomeServe.Checked = true;
            }
            else
            {
                radVolleyGuestServe.Checked = true;
            }

            evaluateVolleySetEnd();

            watchVolleyGame();
        }

        private void watchVolleyGame()
        {
            if (chkAutoUpdateVolleyScoreboard.Checked)
            {
                updateVolleyScoreboard();
            }
        }

        private void evaluateVolleySetEnd()
        {
            NumericUpDown scoreHome = getVolleySetScore(1);
            NumericUpDown scoreGuest = getVolleySetScore(2);
            NumericUpDown aux=new NumericUpDown();
            int pointsSet = (int)nudVolleyPointsPerSet.Value;

            if (scoreHome.Value >= pointsSet && (scoreHome.Value - scoreGuest.Value) >= 2)
            {
                nudVolleyHomeSets.Value++;
                evaluateVolleyMatchEnd();
            }
            else if (scoreGuest.Value >= pointsSet && (scoreGuest.Value - scoreHome.Value) >= 2)
            {
                nudVolleyGuestSets.Value++;
                evaluateVolleyMatchEnd();
            }
        }

        private void evaluateVolleyMatchEnd()
        {
            int currentSet = int.Parse(getVolleyCurrentSet());
            int setsMatch = (int)nudVolleySetsPerMatch.Value;

            if (currentSet <= setsMatch)
            {
                ((RadioButton)groupBox21.Controls.Find("radVolley" + (currentSet + 1).ToString() + "Set", false)[0]).Checked = true;
            }
        }

        private int getVolleySetPoints(int team)
        {
            return (int)getVolleySetScore(team).Value;
        }

        private NumericUpDown getVolleySetScore(int team)
        {
            String teamText = (team==1) ? "Home" : "Guest";
            return (NumericUpDown)groupBox21.Controls.Find("nudVolley" + teamText + getVolleyCurrentSet() + "SetPoints", false)[0];
        }

        private String getVolleyCurrentSet()
        {
            String aux = getVolleyCurrentSetRadioButton().Name;
            return aux.Substring(9, 1);
        }

        private RadioButton getVolleyCurrentSetRadioButton()
        {
            RadioButton aux=new RadioButton();

            foreach (Control item in groupBox21.Controls)
	        {
                if (item.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)item).Checked && item.Name.Contains("radVolley") && item.Name.Contains("Set"))
                    {
                        aux = (RadioButton)item;
                    }
                }
	        }

            return aux;
        }

        private void startVolleyResult()
        {
            if (cmbTemplateVolleyResult.Text != "")
            {
                if (casparServer.Connected)
                {
                    Template templateVolleyResult = new Template();
                    //Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    //Uri logo2Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbGuestTeamLogo.Text.ToLower() + ".png");
                    Uri logo1Path = new Uri(casparServer.ServerPaths.InitialPath + casparServer.ServerPaths.MediaPath + cmbFederationLogo.Text.ToLower() + ".png");

                    templateVolleyResult.Fields.Add(new TemplateField("f0", txtVolleyHomeTeam.Text));
                    templateVolleyResult.Fields.Add(new TemplateField("f1", txtVolleyGuestTeam.Text));
                    templateVolleyResult.Fields.Add(new TemplateField("f2", nudVolleyHome1SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f3", nudVolleyGuest1SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f4", nudVolleyHome2SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f5", nudVolleyGuest2SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f6", nudVolleyHome3SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f7", nudVolleyGuest3SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f8", nudVolleyHome4SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f9", nudVolleyGuest4SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f10", nudVolleyHome5SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f11", nudVolleyGuest5SetPoints.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f12", nudVolleyHomeSets.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f13", nudVolleyGuestSets.Value.ToString()));
                    templateVolleyResult.Fields.Add(new TemplateField("f14", txtVolleyTitle.Text));
                    templateVolleyResult.Fields.Add(new TemplateField("f15", txtVolleyWebsite.Text));
                    templateVolleyResult.Fields.Add(new TemplateField("flag1", logo1Path.ToString()));
                    //templateVolleyResult.Fields.Add(new TemplateField("flag2", logo2Path.ToString()));

                    ReturnInfo ri = casparServer.Execute(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateVolleyResult.TemplateDataText(), cmbTemplateVolleyResult.Text, layerVolleyResult.ToString()));

                    if (chkAutoHideVolleyResult.Checked)
                    {
                        tmrVolleyResult.Interval = ((int)nudAutoHideVolleyResultSeconds.Value) * 1000;
                        tmrVolleyResult.Enabled = true;
                        tmrVolleyResult.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private void stopVolleyResult()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("CG 1-{0} STOP 0", layerVolleyResult.ToString()));
                if (chkAutoHideVolleyResult.Checked)
                {
                    tmrVolleyResult.Stop();
                    tmrVolleyResult.Enabled = false;
                }
            }
        }

        private void startRecording()
        {
            if (casparServer.Connected)
            {
                if (!txtRecordingFileName.Text.Equals(""))
                {
                    //String aux=String.Format("ADD 1 FILE {0}-{1}-{2}.mov -vcodec libx264 [-preset ultrafast -tune fastdecode -crf 5]","REC",txtArchivoGrabacion.Text,DateTime.Now.ToString("ddMM-HHmm"));
                    String aux = String.Format("ADD 1 FILE {0}-{1}-{2}.mp4 -vcodec libx264 -preset ultrafast -tune fastdecode -crf {3}", "REC", txtRecordingFileName.Text, DateTime.Now.ToString("ddMMHHmm"), HandballMatch.getInstance().recordingCRF);
                    casparServer.Execute(aux);
                    ColorRecording = btnStartRecording.BackColor;
                    btnStartRecording.BackColor = Color.Red;
                }
            }
        }

        private void stopRecording()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute("REMOVE 1 FILE");
                btnStartRecording.BackColor = ColorRecording;
            }
        }

        private void showLogo()
        {
            if (casparServer.Connected)
            {
                if (!cmbBroadcastLogo.Text.Equals(""))
                {
                    casparServer.Execute(String.Format("PLAY 1-{2} {0}{1}{0} MIX 15", (char)0x22, cmbBroadcastLogo.Text, layerLogo.ToString()));
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
                if (lvwVideoFiles.SelectedItems.Count != 0)
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

        private void startAudio()
        {
            if (casparServer.Connected)
            {
                if (cmbAudioFiles.Text != "")
                {
                    casparServer.Execute(String.Format("PLAY 1-0 {0}{1}{0} CHANNEL_LAYOUT STEREO", (char)0x22, cmbAudioFiles.Text, (chkLoopAudioFile.Checked ? "LOOP" : "")));
                }
            }
        }

        private void stopAudio()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("STOP 1-0"));
            }
        }

        private void pauseAudio()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("PAUSE 1-0"));
            }
        }

        private void startWebcam()
        {
            if (casparServer.Connected)
            {
                if (cmbWebcam.Text != "" && cmbWebcamResolution.Text != "")
                {
                    casparServer.Execute(String.Format("PLAY 1-{2} {0}dshow://video={1}{0} {0}-video_size {3} -framerate 30{0}", (char)0x22, cmbWebcam.Text, layerVideo.ToString(),cmbWebcamResolution.Text));
                }
            }
        }

        private void stopWebcam()
        {
            if (casparServer.Connected)
            {
                casparServer.Execute(String.Format("STOP 1-{0}", layerVideo.ToString()));
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
                    AgregarJugador(lvwHomeTeamPlayers, nro, nombrecompleto);
                    break;
                case 2:
                    ModificarJugador(HandballMatch.getInstance().team1Players, lvwHomeTeamPlayers, nro, nombrecompleto);
                    break;
                case 3:
                    AgregarJugador(lvwGuestTeamPlayers, nro, nombrecompleto);
                    break;
                case 4:
                    ModificarJugador(HandballMatch.getInstance().team2Players, lvwGuestTeamPlayers, nro, nombrecompleto);
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
                FillTeamPlayers(lvwHomeTeamPlayers, HandballMatch.getInstance().team1Players);
            }
        }

        private void clearTeam2Players()
        {
            if (MessageBox.Show("¿Esta seguro de limpiar el listado jugadores visitantes?", "Jugadores Visitantes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HandballMatch.getInstance().team2Players.Clear();
                FillTeamPlayers(lvwGuestTeamPlayers, HandballMatch.getInstance().team2Players);
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
            HandballMatch.getInstance().name = txtEventName.Text;
            HandballMatch.getInstance().description = txtEventDescription.Text;
            HandballMatch.getInstance().fecha = dtpEventDate.Value;
            HandballMatch.getInstance().team1Name = txtHomeTeamName.Text;
            HandballMatch.getInstance().team2Name = txtGuestTeamName.Text;
            HandballMatch.getInstance().leagueName = txtEventLeague.Text;
            HandballMatch.getInstance().location = txtEventLocation.Text;
            HandballMatch.getInstance().eventTitle = txtIntroTitle.Text;
            //HandballMatch.getInstance().team1ScoreName = txtNombreScoreLocal.Text;
            //HandballMatch.getInstance().team2ScoreName = txtNombreScoreVisitante.Text;
            HandballMatch.getInstance().team1ScoreName = txtVolleyHomeTeam.Text;
            HandballMatch.getInstance().team2ScoreName = txtVolleyGuestTeam.Text;

            HandballMatch.getInstance().serverAddress = txtServerAddress.Text;
            HandballMatch.getInstance().serverPort = txtServerPort.Text;

            HandballMatch.getInstance().templatePresentation = cmbTemplateIntro.Text;
            HandballMatch.getInstance().templateTeam = cmbTemplateTeam.Text;
            HandballMatch.getInstance().templateScoreboard = cmbTemplateScoreboard.Text;
            HandballMatch.getInstance().templateResult = cmbTemplateResult.Text;
            HandballMatch.getInstance().templateLowerThird = cmbTemplateLowerThird.Text;
            HandballMatch.getInstance().templatePositions = cmbTemplatePositions.Text;
            HandballMatch.getInstance().templateTwitter = cmbTemplateTwitter.Text;
            HandballMatch.getInstance().templateVolleyScoreboard = cmbTemplateVolleyScoreboard.Text;
            HandballMatch.getInstance().templateVolleyResult = cmbTemplateVolleyResult.Text;

            HandballMatch.getInstance().imageLogoBroadcast = cmbBroadcastLogo.Text;
            HandballMatch.getInstance().imageCredits = cmbImageScrolling.Text;
            HandballMatch.getInstance().speedImageCredits = trkImageScrollingSpeed.Value;

            HandballMatch.getInstance().team1Logo = cmbHomeTeamLogo.Text;
            HandballMatch.getInstance().team2Logo = cmbGuestTeamLogo.Text;
            HandballMatch.getInstance().leagueLogo = cmbFederationLogo.Text;
            HandballMatch.getInstance().team1Coach = txtHomeTeamCoach.Text;
            HandballMatch.getInstance().team2Coach = txtGuestTeamPlayers.Text;
            HandballMatch.getInstance().recordingFileName = txtRecordingFileName.Text;
            HandballMatch.getInstance().positionsTitle = txtPositionsTitle.Text;
            HandballMatch.getInstance().positionsSubtitle = txtPositionsSubtitle.Text;
            HandballMatch.getInstance().twitterHashtag = txtTwitterHashtag.Text;
            HandballMatch.getInstance().twitterUsername = txtTwitterUserName.Text;
            HandballMatch.getInstance().twitterFullname = txtTwitterFullName.Text;
            HandballMatch.getInstance().twitterMessage = txtTwitterMessage.Text;

            HandballMatch.getInstance().scoreHalf = cmbHalf.Text;
            HandballMatch.getInstance().scoreClockMinutes = (int)nudClockMinutes.Value;
            HandballMatch.getInstance().scoreClockSeconds = (int)nudClockSeconds.Value;
            HandballMatch.getInstance().scoreClockMatchMinutes = (int)nudClockLengthMinutes.Value;
            HandballMatch.getInstance().scoreClockMatchSeconds = (int)nudClockLengthSeconds.Value;
            HandballMatch.getInstance().scoreAutoHideOnFinished = chkAutoHideOnClockEnd.Checked;
            HandballMatch.getInstance().scoreClockExclutionMinutes = (int)nudExclutionLengthMinutes.Value;
            HandballMatch.getInstance().scoreClockExclutionSeconds = (int)nudExclutionLengthSeconds.Value;

            HandballMatch.getInstance().titleVolleyScoreboard = txtVolleyTitle.Text;
            HandballMatch.getInstance().websiteVolleyScoreboardResult = txtVolleyWebsite.Text;
            HandballMatch.getInstance().showVolleyService = chkVolleyShowService.Checked;

            HandballMatch.getInstance().autoHideIntro = chkAutoHideIntro.Checked;
            HandballMatch.getInstance().autoHideIntroSeconds = ((int)nudAutoHideIntroSeconds.Value);
            HandballMatch.getInstance().autoHideTeam1 = chkAutoHideIntro.Checked;
            HandballMatch.getInstance().autoHideTeam1Seconds = ((int)nudAutoHideIntroSeconds.Value);
            HandballMatch.getInstance().autoHideTeam2 = chkAutoHideIntro.Checked;
            HandballMatch.getInstance().autoHideTeam2Seconds = ((int)nudAutoHideIntroSeconds.Value);

            HandballMatch.getInstance().autoShowHideTeamsSeconds = ((int)nudTeamsShowSeconds.Value);
            HandballMatch.getInstance().autoShowHideTeamsInBetweenSeconds = ((int)nudSeparationTeamsSeconds.Value);

            HandballMatch.getInstance().autoHideResult = chkAutoHideResult.Checked;
            HandballMatch.getInstance().autoHideResultSeconds = ((int)nudAutoHideResultSeconds.Value);
            HandballMatch.getInstance().autoHideLowerThird = chkAutoHideLT.Checked;
            HandballMatch.getInstance().autoHideLowerThirdSeconds = ((int)nudAutoHideLTSeconds.Value);

            HandballMatch.getInstance().autoHidePositions = chkAutoHidePositions.Checked;
            HandballMatch.getInstance().autoHidePositionsSeconds = ((int)nudAutoHidePositionsSeconds.Value);

            HandballMatch.getInstance().autoHideTwitter = chkAutoHideTwitter.Checked;
            HandballMatch.getInstance().autoHideTwitterSeconds = ((int)nudAutoHideTwitterSeconds.Value);

            HandballMatch.getInstance().autoUpdateVolleyScoreboard = chkAutoUpdateVolleyScoreboard.Checked;
            HandballMatch.getInstance().autoHideVolleyResult = chkAutoHideVolleyResult.Checked;
            HandballMatch.getInstance().autoHideVolleyResultSeconds = ((int)nudAutoHideVolleyResultSeconds.Value);
        }

        private void getMatchValues()
        {
            txtEventName.Text = HandballMatch.getInstance().name;
            txtEventDescription.Text = HandballMatch.getInstance().description;
            dtpEventDate.Value = HandballMatch.getInstance().fecha;
            txtHomeTeamName.Text = HandballMatch.getInstance().team1Name;
            txtGuestTeamName.Text = HandballMatch.getInstance().team2Name;
            txtEventLeague.Text = HandballMatch.getInstance().leagueName;
            txtEventLocation.Text = HandballMatch.getInstance().location;
            txtIntroTitle.Text = HandballMatch.getInstance().eventTitle;
            txtNombreScoreLocal.Text = HandballMatch.getInstance().team1ScoreName;
            txtNombreScoreVisitante.Text = HandballMatch.getInstance().team2ScoreName;
            txtVolleyHomeTeam.Text = HandballMatch.getInstance().team1ScoreName;
            txtVolleyGuestTeam.Text = HandballMatch.getInstance().team2ScoreName;

            txtServerAddress.Text = HandballMatch.getInstance().serverAddress;
            txtServerPort.Text = HandballMatch.getInstance().serverPort;

            cmbTemplateIntro.Text = HandballMatch.getInstance().templatePresentation;
            cmbTemplateTeam.Text = HandballMatch.getInstance().templateTeam;
            cmbTemplateScoreboard.Text = HandballMatch.getInstance().templateScoreboard;
            cmbTemplateResult.Text = HandballMatch.getInstance().templateResult;
            cmbTemplateLowerThird.Text = HandballMatch.getInstance().templateLowerThird;
            cmbTemplatePositions.Text = HandballMatch.getInstance().templatePositions;
            cmbTemplateTwitter.Text = HandballMatch.getInstance().templateTwitter;
            cmbTemplateVolleyScoreboard.Text = HandballMatch.getInstance().templateVolleyScoreboard;
            cmbTemplateVolleyResult.Text = HandballMatch.getInstance().templateVolleyResult;

            cmbBroadcastLogo.Text = HandballMatch.getInstance().imageLogoBroadcast;
            cmbImageScrolling.Text = HandballMatch.getInstance().imageCredits;
            trkImageScrollingSpeed.Value = HandballMatch.getInstance().speedImageCredits;

            cmbHomeTeamLogo.Text = HandballMatch.getInstance().team1Logo;
            cmbGuestTeamLogo.Text = HandballMatch.getInstance().team2Logo;
            cmbFederationLogo.Text = HandballMatch.getInstance().leagueLogo;
            txtHomeTeamCoach.Text = HandballMatch.getInstance().team1Coach;
            txtGuestTeamPlayers.Text = HandballMatch.getInstance().team2Coach;
            txtRecordingFileName.Text = HandballMatch.getInstance().recordingFileName;
            txtPositionsTitle.Text = HandballMatch.getInstance().positionsTitle;
            txtPositionsSubtitle.Text = HandballMatch.getInstance().positionsSubtitle;
            txtTwitterHashtag.Text = HandballMatch.getInstance().twitterHashtag;
            txtTwitterUserName.Text = HandballMatch.getInstance().twitterUsername;
            txtTwitterFullName.Text = HandballMatch.getInstance().twitterFullname;
            txtTwitterMessage.Text = HandballMatch.getInstance().twitterMessage;
            txtVolleyHomeTeam.Text = HandballMatch.getInstance().team1Name;
            txtVolleyGuestTeam.Text = HandballMatch.getInstance().team2Name;

            cmbHalf.Text = HandballMatch.getInstance().scoreHalf;
            nudClockMinutes.Value = HandballMatch.getInstance().scoreClockMinutes;
            nudClockSeconds.Value = HandballMatch.getInstance().scoreClockSeconds;
            chkAutoHideOnClockEnd.Checked = HandballMatch.getInstance().scoreAutoHideOnFinished;
            nudClockLengthMinutes.Value = HandballMatch.getInstance().scoreClockMatchMinutes;
            nudClockLengthSeconds.Value = HandballMatch.getInstance().scoreClockMatchSeconds;
            nudExclutionLengthMinutes.Value = HandballMatch.getInstance().scoreClockExclutionMinutes;
            nudExclutionLengthSeconds.Value = HandballMatch.getInstance().scoreClockExclutionSeconds;

            txtVolleyTitle.Text = HandballMatch.getInstance().titleVolleyScoreboard;
            txtVolleyWebsite.Text = HandballMatch.getInstance().websiteVolleyScoreboardResult;
            chkVolleyShowService.Checked = HandballMatch.getInstance().showVolleyService;

            FillTeamPlayers(lvwHomeTeamPlayers, HandballMatch.getInstance().team1Players);
            FillTeamPlayers(lvwGuestTeamPlayers, HandballMatch.getInstance().team2Players);
            FillPositionTable();
            
            chkAutoHideIntro.Checked = HandballMatch.getInstance().autoHideIntro;
            nudAutoHideIntroSeconds.Value = HandballMatch.getInstance().autoHideIntroSeconds;
            chkAutoHideHomeTeam.Checked = HandballMatch.getInstance().autoHideTeam1;
            nudHideHomeTeamSeconds.Value = HandballMatch.getInstance().autoHideTeam1Seconds;
            chkAutoHideGuestTeam.Checked = HandballMatch.getInstance().autoHideTeam2;
            nudHideGuestTeamSeconds.Value = HandballMatch.getInstance().autoHideTeam2Seconds;

            nudTeamsShowSeconds.Value = HandballMatch.getInstance().autoShowHideTeamsSeconds;
            nudSeparationTeamsSeconds.Value = HandballMatch.getInstance().autoShowHideTeamsInBetweenSeconds;

            chkAutoHideResult.Checked = HandballMatch.getInstance().autoHideResult;
            nudAutoHideResultSeconds.Value = HandballMatch.getInstance().autoHideResultSeconds;
            chkAutoHideLT.Checked = HandballMatch.getInstance().autoHideLowerThird;
            nudAutoHideLTSeconds.Value = HandballMatch.getInstance().autoHideLowerThirdSeconds;

            chkAutoHidePositions.Checked = HandballMatch.getInstance().autoHidePositions;
            nudAutoHidePositionsSeconds.Value = HandballMatch.getInstance().autoHidePositionsSeconds;

            chkAutoHideTwitter.Checked = HandballMatch.getInstance().autoHideTwitter;
            nudAutoHideTwitterSeconds.Value = HandballMatch.getInstance().autoHideTwitterSeconds;

            chkAutoUpdateVolleyScoreboard.Checked = HandballMatch.getInstance().autoUpdateVolleyScoreboard;
            chkAutoHideVolleyResult.Checked = HandballMatch.getInstance().autoHideVolleyResult;
            nudAutoHideVolleyResultSeconds.Value = HandballMatch.getInstance().autoHideVolleyResultSeconds;
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
            if (lvwHomeTeamPlayers.SelectedItems != null)
            {
                HandballMatch.getInstance().team1Players.Remove(HandballMatch.getInstance().team1Players.Find(element => element.number == int.Parse(lvwHomeTeamPlayers.SelectedItems[0].Text)));
                FillTeamPlayers(lvwHomeTeamPlayers, HandballMatch.getInstance().team1Players);
            }
        }

        private void removePlayerTeam2()
        {
            if (lvwGuestTeamPlayers.SelectedItems != null)
            {
                HandballMatch.getInstance().team2Players.Remove(HandballMatch.getInstance().team2Players.Find(element => element.number == int.Parse(lvwGuestTeamPlayers.SelectedItems[0].Text)));
                FillTeamPlayers(lvwGuestTeamPlayers, HandballMatch.getInstance().team2Players);
            }
        }

        private void editPlayerTeam1()
        {
            if (lvwHomeTeamPlayers.SelectedItems != null)
            {
                Player aux = HandballMatch.getInstance().team1Players.Find(element => element.number == int.Parse(lvwHomeTeamPlayers.SelectedItems[0].Text));
                Jugador f1 = new Jugador(this, 2, aux.number.ToString(), aux.getFullName());
                f1.ShowDialog(this);
            }
        }

        private void editPlayerTeam2()
        {
            if (lvwGuestTeamPlayers.SelectedItems != null)
            {
                Player aux = HandballMatch.getInstance().team2Players.Find(element => element.number == int.Parse(lvwGuestTeamPlayers.SelectedItems[0].Text));
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
            if (lvwHomeTeamPlayers.SelectedItems != null)
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
            showHideTeamsheet(btnShowHideGuestTeam, txtGuestTeamName, lvwGuestTeamPlayers, txtGuestTeamPlayers, cmbGuestTeamLogo, chkAutoHideGuestTeam, nudHideGuestTeamSeconds, tmrTeam2);
        }

        private void btnMostrarEquipoLocal_Click(object sender, EventArgs e)
        {
            showHideTeamsheet(btnShowHideHomeTeam, txtHomeTeamName, lvwHomeTeamPlayers, txtHomeTeamCoach, cmbHomeTeamLogo, chkAutoHideHomeTeam, nudHideHomeTeamSeconds, tmrTeam1);
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
            showHideTeamsheet(btnShowHideHomeTeam, txtHomeTeamName, lvwHomeTeamPlayers, txtHomeTeamCoach, cmbHomeTeamLogo, chkAutoHideHomeTeam, nudHideHomeTeamSeconds, tmrTeam1);
        }

        private void tmrTeam2_Tick(object sender, EventArgs e)
        {
            showHideTeamsheet(btnShowHideGuestTeam, txtGuestTeamName, lvwGuestTeamPlayers, txtGuestTeamPlayers, cmbGuestTeamLogo, chkAutoHideGuestTeam, nudHideGuestTeamSeconds, tmrTeam2);
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

        private void btnStartVolleyScoreboard_Click(object sender, EventArgs e)
        {
            startVolleyScoreboard();
        }

        private void btnStopVolleyScoreboard_Click(object sender, EventArgs e)
        {
            stopVolleyScoreboard();
        }

        private void btnUpdateVolleyScoreboard_Click(object sender, EventArgs e)
        {
            updateVolleyScoreboard();
        }

        private void nudVolleyHome1SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void btnVolleyHomeAddPoint_Click(object sender, EventArgs e)
        {
            addVolleyGamePoint(btnVolleyHomeAddPoint);
        }

        private void btnVolleyGuestAddPoint_Click(object sender, EventArgs e)
        {
            addVolleyGamePoint(btnVolleyGuestAddPoint);
        }

        private void btnStartVolleyResult_Click(object sender, EventArgs e)
        {
            startVolleyResult();
        }

        private void btnStopVolleyResult_Click(object sender, EventArgs e)
        {
            stopVolleyResult();
        }

        private void tmrVolleyResult_Tick(object sender, EventArgs e)
        {
            stopVolleyResult();
        }

        private void btnPlayAudio_Click(object sender, EventArgs e)
        {
            startAudio();
        }

        private void btnPauseAudio_Click(object sender, EventArgs e)
        {
            pauseAudio();
        }

        private void btnStopAudio_Click(object sender, EventArgs e)
        {
            stopAudio();
        }

        private void btnRefreshAudioFiles_Click(object sender, EventArgs e)
        {
            getServerAudioFiles();
        }

        private void btnStartWebcam_Click(object sender, EventArgs e)
        {
            startWebcam();
        }

        private void btnStopWebcam_Click(object sender, EventArgs e)
        {
            stopWebcam();
        }

        private void nudVolleyGuest1SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyHome2SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyGuest2SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyHome3SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyGuest3SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyHome4SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyGuest4SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyHome5SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyGuest5SetPoints_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyHomeSets_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void nudVolleyGuestSets_ValueChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }
    }
}
