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
using RestSharp;
using HandballCliente.Models;
using HandballCliente.Controllers;
using RestSharp.Deserializers;
using System.Net;


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

        //private CasparCG casparServer = new CasparCG();
        private Color ColorFondo;
        private String fileName = "";
        private Color ColorRecording;
        private bool gameplayMouseDown;

        private const int layerPresentation = 5;
        private const int layerTeams = 10;
        private const int layerScoreboard = 15;
        private const int layerResult = 20;
        private const int layerLowerThird = 25;
        private const int layerPositions = 30;
        private const int layerTwitter = 35;

        private const int layerVolleyScoreboard = 40;
        private const int layerVolleyResult = 45;
        private const int layerDynamicInfo = 50;
        private const int layerTwitterCounter = 55;

        private const int layerBackground = 0;
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
            picSolidColor.BackColor = Color.Empty;
        }

        private void NewMatch()
        {
            Limpiar();
            HandballMatch.getInstance().NewMatch();
            fileName = "";
            this.Text = String.Format("nViVo CG Client - [{0}]", "sin titulo");
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
            cmbTemplateVolleyResult.Items.Clear();
            cmbTemplateGameshowCountdown.Items.Clear();
            cmbTemplateGameshowQuestions.Items.Clear();
            cmbTemplateDynamicLogo.Items.Clear();
            cmbTemplateElectionsTop3.Items.Clear();
            cmbLogoFile.Items.Clear();
            cmbTemplateWeatherForecast.Items.Clear();
            cmbRugbyIntroTemplate.Items.Clear();
            cmbTemplateRugbyLineup.Items.Clear();
            cmbTemplateRugbyResult.Items.Clear();
            cmbTemplateRugbyScoreboard.Items.Clear();
            cmbTemplateRugbyOfficials.Items.Clear();
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
            txtServerAddress.Text = AppController.getInstance().getCasparCgServerAddress();
            txtServerPort.Text = AppController.getInstance().getCasparCgServerPort().ToString();
            btnConnectDisconnect.Text = AppController.getInstance().isConnectedToCasparCgServer() ? "Desconectar" : "Conectar";
            btnClearChannel.Enabled = AppController.getInstance().isConnectedToCasparCgServer();

            btnShowHideIntro.Tag = "0";
            btnShowHideHomeTeam.Tag = "0";
            btnShowHideGuestTeam.Tag = "0";
            btnShowHomeAndGuestTeam.Tag = "0";
            btnShowHideResult.Tag = "0";
            btnStartStopRugbyIntro.Tag = "0";
            btnStartStopRugbyLineup.Tag = "0";
            btnStartStopRugbyResult.Tag = "0";
            btnStartStopRugbyOfficials.Tag = "0";


            lockUnlockTemplates();
            radHomeTeamPlayers.Checked = true;

            trkVolume.Value = 10;
            trkImageScrollingSpeed.Value = 1;

            fillCombosTeamTextStyle();
            fillComboWebcam();
            fillListSports();
            setSportsTabPositions();

            radVolleyHomeServe.Checked = true;
            radVolleyHomeServe.Tag = 1;
            radVolleyGuestServe.Tag = 0;
            cmbVolleyScoreboardFontSize.Text = "14";
            nudVolleySetsPerMatch.Value = 5;
            nudVolleyPointsPerSet.Value = 11;
            nudVolleyServicesPerPlayer.Value = 2;

            txtWeatherForecastWS.Text = "http://localhost:3000";
            txtElectionsTop3WS.Text = "http://localhost:3000";
            txtTwitterWS.Text = "http://localhost:3000";
            txtTwitterCounterWS.Text = "http://localhost:3000";
            txtDynamicInfoWS.Text = "http://localhost:3000";
            txtDynamicNewsTickerWS.Text = "http://localhost:3000";
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


            cmbGameshowCorrectAnswer.Items.Clear();
            UtilHelper.populateComboboxNumberRange(cmbGameshowCorrectAnswer, 1, 4);

            cmbGameshowPlayerAnswer.Items.Clear();
            UtilHelper.populateComboboxNumberRange(cmbGameshowPlayerAnswer, 1, 4);

            UtilHelper.populateComboboxNumberRange(cmbGameShowFindCardNumberTries, 1, 3);
        }

        private void fillListQuestions()
        {
            List<Question> tmpquestions = new List<Question>();
            Question question = new Question();
            question.id = 1;
            question.question = "¿De que color era el caballo blanco de San Martin?";
            List<Answer> tmpanswers = new List<Answer>();
            Answer answer = new Answer();
            answer.answer = "Rojo";
            tmpanswers.Add(answer);
            answer = new Answer();
            answer.answer = "Marron";
            tmpanswers.Add(answer);
            answer = new Answer();
            answer.answer = "Blanco";
            tmpanswers.Add(answer);
            answer = new Answer();
            answer.answer = "Negro";
            tmpanswers.Add(answer);
            question.answers = tmpanswers;
            question.correctAnswer = 3;
            tmpquestions.Add(question);

            HandballMatch.getInstance().gameshowQuestions = tmpquestions;
            FillGameshowQuestions(lvwGameshowQuestions, HandballMatch.getInstance().gameshowQuestions);
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

        private void fillListSports()
        {
            lstSports.Items.Clear();
            lstSports.Items.AddRange(SportsController.getSportList());
        }

        private void setSportsTabPositions()
        {
            setTabPosition(tabSports);
            setTabPosition(tabVolleyball);
            setTabPosition(tabBasket);
            setTabPosition(tabRugby);

            showSportTab(tabSports);
            lstSports.SelectedIndex = -1;
            hideAllSportTabs();
        }

        private void selectSportTab()
        {
            switch (lstSports.SelectedIndex)
            {
                case 0: 
                    showSportTab(tabSports);
                    break;
                case 1: 
                    showSportTab(tabVolleyball);
                    break;
                case 3:
                    showSportTab(tabBasket);
                    break;
                case 4:
                    showSportTab(tabRugby);
                    break;
                default:
                    hideAllSportTabs();
                    break;
            }
        }

        private void setTabPosition(TabControl tab)
        {
            setTabPosition(tab, 161, 0);
        }

        private void setTabPosition(TabControl tab, int left, int top)
        {
            tab.Left = left;
            tab.Top = top;
        }

        private void showSportTab(System.Windows.Forms.TabControl tab)
        {
            hideAllSportTabs();
            tab.Visible = true;
        }

        private void hideAllSportTabs()
        {
            tabSports.Visible = false;
            tabVolleyball.Visible = false;
            tabBasket.Visible = false;
            tabRugby.Visible = false;
        }

        private void setTopMost(bool stateTopMost)
        {
            SetWindowPos(this.Handle, (stateTopMost) ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void connectToCasparCGServer()
        {
            try
            {
                if (!AppController.getInstance().isConnectedToCasparCgServer())
                {
                    AppController.getInstance().setCasparCgServer(txtServerAddress.Text, Convert.ToInt32(txtServerPort.Text));
                    AppController.getInstance().connectCasparCgServer();
                    getServerTemplates();
                    getServerImageFiles();
                    getServerVideoFiles();
                    getServerAudioFiles();
                    getGameplayVideoFiles();
                }
                else
                {
                    AppController.getInstance().disconnectCasparCgServer();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                btnConnectDisconnect.Text = AppController.getInstance().isConnectedToCasparCgServer() ? "Desconectar" : "Conectar";
                btnClearChannel.Enabled = AppController.getInstance().isConnectedToCasparCgServer();
                lockUnlockTemplates();
                //this.BackColor = AppController.getInstance().isConnectedToCasparCgServer() ? Color.ForestGreen : ColorFondo;
                stsStatus.BackColor = AppController.getInstance().isConnectedToCasparCgServer() ? Color.ForestGreen : ColorFondo;
            }
        }

        public void clearChannel()
        {
                AppController.getInstance().clearChannel();
                if (chkWithSolidColor.Checked && !picSolidColor.BackColor.IsEmpty)
                {
                    AppController.getInstance().setBackgroundColor(picSolidColor.BackColor, layerBackground);
                }
        }

        private void getServerTemplates()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().getServerTemplates();
                setCombosTemplates();
                AppController.getInstance().fillCombosTemplates();
            }
        }

        private void getServerImageFiles()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().getServerImages();
                List<String> medias = AppController.getInstance().getImages();
                fillCombosTemplate(cmbHomeTeamLogo, medias);
                fillCombosTemplate(cmbGuestTeamLogo, medias);
                fillCombosTemplate(cmbFederationLogo, medias);
                fillCombosTemplate(cmbHomeTeamLogo, medias);
                fillCombosTemplate(cmbGuestTeamLogo, medias);
                fillCombosTemplate(cmbBroadcastLogo, medias);
                fillCombosTemplate(cmbImageScrolling, medias);
                fillCombosTemplate(cmbLogoFile, medias);
                fillCombosTemplate(cmbElectionsC1Picture, medias);
            }
        }

        private void getServerVideoFiles()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().getServerVideos();
                List<String> medias = AppController.getInstance().getVideos();
                lvwVideoFiles.Items.Clear();
                foreach (String item in medias)
                {
                    lvwVideoFiles.Items.Add(item);
                }
                fillCombosTemplate(cmbTwitterPlaylistBGVideo, medias);
            }
        }

        private void getServerAudioFiles()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                List<String> medias = AppController.getInstance().getAudios();
                fillCombosTemplate(cmbAudioFiles, medias);
            }
        }

        private void getGameplayVideoFiles()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().getServerVideos();
                List<String> medias = AppController.getInstance().getVideos();
                List<String> playlist = medias.FindAll(m => m.StartsWith("GAMEPLAY/"));
                lvwGameplayPlaylist.Items.Clear();
                playlist.ForEach(i => lvwGameplayPlaylist.Items.Add(i));
            }
        }

        private void setCombosTemplates()
        {
            AppController.getInstance().addComboBoxTemplate(cmbTemplateTeam);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateIntro);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateResult);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateScoreboard);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateLowerThird);
            AppController.getInstance().addComboBoxTemplate(cmbTemplatePositions);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateTwitter);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateVolleyScoreboard);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateVolleyResult);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateGameshowCountdown);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateDynamicLogo);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateGameshowQuestions);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateElectionsTop3);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateWeatherForecast);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateTwitterCounter);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateDynamicInfo);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateAnimatedLogo);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateDynamicNewsTicker);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateGameplay);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateTwitterPoll);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateBasketScoreboard);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateGameshowFindCard);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateTwitterPlaylist);
            AppController.getInstance().addComboBoxTemplate(cmbRugbyIntroTemplate);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateRugbyLineup);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateRugbyOfficials);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateRugbyResult);
            AppController.getInstance().addComboBoxTemplate(cmbTemplateRugbyScoreboard);
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
                Template templateScoreboard = new Template();
                Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                Dictionary<string, string> fieldsValues = new Dictionary<string, string>()
                {
                    {"team1Name", txtNombreScoreLocal.Text},{"team2Name", txtNombreScoreVisitante.Text},
                    {"team1Score", nudHomeTeamScore.Value.ToString()},{"team2Score", nudGuestTeamScore.Value.ToString()},
                    {"gameTime", nudClockMinutes.Value.ToString() + ":" + nudClockSeconds.Value.ToString()},
                    {"halfNum", cmbHalf.Text},{"logoScoreboard", logoPath.ToString()}
                };

                SportsController.startScoreboard(cmbTemplateScoreboard.Text, layerScoreboard, fieldsValues);
                btnShowHideIntro.Tag = "1";
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template, local/visitante)", this.Text);
            }
        }

        private void stopScoreboard()
        {
            SportsController.stopScoreboard(layerScoreboard);
        }

        private void showHideScoreboard()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                SportsController.showHideScoreboard(layerScoreboard);
            }
        }

        private void startScoreboardClock()
        {
            SportsController.startScoreboardClock(layerScoreboard);
            if (chkAutoShowOnClockStart.Checked)
            {
                showHideScoreboard();
            }
        }

        private void stopScoreboardClock()
        {
            SportsController.stopScoreboardClock(layerScoreboard);
        }

        private void resetScoreboardClock()
        {
            Dictionary<string, string> fieldsValues = new Dictionary<string, string>()
            {
                {"gameTime", nudClockMinutes.Value.ToString() + ":" + nudClockSeconds.Value.ToString()}
            };

            SportsController.resetScoreboardClock(layerScoreboard, fieldsValues);
        }

        private void updateTeamsScore()
        {
            Dictionary<string, string> fieldsValues = new Dictionary<string, string>()
            {
                {"team1Score", nudHomeTeamScore.Value.ToString()},{"team2Score", nudGuestTeamScore.Value.ToString()}
            };

            SportsController.updateScoreboardScores(layerScoreboard, fieldsValues);

        }

        public void addOneScoreTeam1()
        {
            nudHomeTeamScore.UpButton();
        }

        public void addOneScoreTeam2()
        {
            nudGuestTeamScore.UpButton();
        }

        private void startDynamicLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateDynamicLogo = new Template();
                Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                templateDynamicLogo.Fields.Add(new TemplateField("positionX", nudDynamicLogoPosX.Value.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("positionY", nudDynamicLogoPosY.Value.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("logoWidth", nudDynamicLogoWidth.Value.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("logoHeight", nudDynamicLogoHeight.Value.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateDynamicLogo.TemplateDataText(), cmbTemplateDynamicLogo.Text, layerLogo.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopDynamicLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerLogo.ToString()));
            }
        }

        private void startDynamicInfo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateDynamicInfo = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                if (chkDynamicInfoPosition.Checked)
                {
                    templateDynamicInfo.Fields.Add(new TemplateField("positionX", nudDynamicInfoPosX.Value.ToString()));
                    templateDynamicInfo.Fields.Add(new TemplateField("positionY", nudDynamicInfoPosY.Value.ToString()));
                }
                if (chkDynamicInfoSize.Checked)
                {
                    templateDynamicInfo.Fields.Add(new TemplateField("width", nudDynamicInfoWidth.Value.ToString()));
                    templateDynamicInfo.Fields.Add(new TemplateField("height", nudDynamicInfoHeight.Value.ToString()));
                }
                templateDynamicInfo.Fields.Add(new TemplateField("header", txtDynamicInfoHeader.Text));
                templateDynamicInfo.Fields.Add(new TemplateField("title", txtDynamicInfoTitle.Text));
                templateDynamicInfo.Fields.Add(new TemplateField("info", txtDynamicInfoText.Text));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateDynamicInfo.TemplateDataText(), cmbTemplateDynamicInfo.Text, layerDynamicInfo.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopDynamicInfo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerDynamicInfo.ToString()));
            }
        }

        private void startTop3()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateDynamicLogo = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbElectionsC1Picture.Text.ToLower() + ".png");
                Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbElectionsC1Picture.Text.ToLower() + ".png");

                templateDynamicLogo.Fields.Add(new TemplateField("info", "TOTAL DE MESAS ESCRUTADAS:"));
                templateDynamicLogo.Fields.Add(new TemplateField("total", "66"));

                templateDynamicLogo.Fields.Add(new TemplateField("c1Position", "1"));
                templateDynamicLogo.Fields.Add(new TemplateField("c1Name", HandballMatch.getInstance().electionResults[0].name));
                templateDynamicLogo.Fields.Add(new TemplateField("c1Percentage", HandballMatch.getInstance().electionResults[0].percentage.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("c1PictureBase64", HandballMatch.getInstance().electionResults[0].pictureBase64));
                //templateDynamicLogo.Fields.Add(new TemplateField("c1Picture", logoPath.ToString()));

                templateDynamicLogo.Fields.Add(new TemplateField("c2Position", "2"));
                templateDynamicLogo.Fields.Add(new TemplateField("c2Name", HandballMatch.getInstance().electionResults[1].name));
                templateDynamicLogo.Fields.Add(new TemplateField("c2Percentage", HandballMatch.getInstance().electionResults[1].percentage.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("c2PictureBase64", HandballMatch.getInstance().electionResults[1].pictureBase64));
                //templateDynamicLogo.Fields.Add(new TemplateField("c2Picture", logoPath.ToString()));

                templateDynamicLogo.Fields.Add(new TemplateField("c3Position", "3"));
                templateDynamicLogo.Fields.Add(new TemplateField("c3Name", HandballMatch.getInstance().electionResults[2].name));
                templateDynamicLogo.Fields.Add(new TemplateField("c3Percentage", HandballMatch.getInstance().electionResults[2].percentage.ToString()));
                templateDynamicLogo.Fields.Add(new TemplateField("c3PictureBase64", HandballMatch.getInstance().electionResults[2].pictureBase64));
                //templateDynamicLogo.Fields.Add(new TemplateField("c3Picture", logoPath.ToString()));


                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateDynamicLogo.TemplateDataText(), cmbTemplateElectionsTop3.Text, layerLogo.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopTop3()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerLogo.ToString()));
            }
        }

        private void showCountdown()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateCountdown = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                templateCountdown.Fields.Add(new TemplateField("countdownNumber", nudGameshowCounterSeconds.Value.ToString()));
                templateCountdown.Fields.Add(new TemplateField("countdownMilisecondsRefresh", nudGameshowRefreshMiliseconds.Value.ToString()));
                //templateCountdown.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateCountdown.TemplateDataText(), cmbTemplateGameshowCountdown.Text, layerScoreboard.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void hideCountdown()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerScoreboard.ToString()));
            }
        }

        private void startCountdown()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "countdownStartStop", layerScoreboard.ToString()));
            }
        }

        private void stopCountdown()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "countdownStartStop", layerScoreboard.ToString()));
            }
        }

        private void showAnimatedLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateCountdown = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                templateCountdown.Fields.Add(new TemplateField("countdownNumber", nudGameshowCounterSeconds.Value.ToString()));
                //templateCountdown.Fields.Add(new TemplateField("countdownMilisecondsRefresh", nudGameshowRefreshMiliseconds.Value.ToString()));
                //templateCountdown.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateCountdown.TemplateDataText(), cmbTemplateAnimatedLogo.Text, layerLogo.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void hideAnimatedLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerLogo.ToString()));
            }
        }

        private void startAnimatedLogoOut()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "animationOut", layerLogo.ToString()));
            }
        }

        private void startAnimatedLogoIn()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "animationIn", layerLogo.ToString()));
            }
        }
        
        private void startGameshowQuestions()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateCountdown = new Template();
                    //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                    templateCountdown.Fields.Add(new TemplateField("questionText", txtGameshowQuestion.Text));
                    templateCountdown.Fields.Add(new TemplateField("answer1Text", lvwGameshowQuestionAnswers.Items[0].SubItems[1].Text));
                    templateCountdown.Fields.Add(new TemplateField("answer2Text", lvwGameshowQuestionAnswers.Items[1].SubItems[1].Text));
                    templateCountdown.Fields.Add(new TemplateField("answer3Text", lvwGameshowQuestionAnswers.Items[2].SubItems[1].Text));
                    templateCountdown.Fields.Add(new TemplateField("answer4Text", lvwGameshowQuestionAnswers.Items[3].SubItems[1].Text));
                    templateCountdown.Fields.Add(new TemplateField("correctAnswer", cmbGameshowCorrectAnswer.Text));
                    templateCountdown.Fields.Add(new TemplateField("playerAnswer", cmbGameshowPlayerAnswer.Text));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateCountdown.TemplateDataText(), cmbTemplateGameshowQuestions.Text, layerPresentation.ToString()));
                    txtLogMessages.Text += "\n" + ri.Message;
                    //System.Diagnostics.Debug.WriteLine(ri.Message);
                }
            }
        }

        private void stopGameshowQuestions()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
            }
        }

        private void showGameshowPlayerAnswer()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "showPlayerAnswer", layerPresentation.ToString()));
            }
        }

        private void showGameshowCorrectAnswer()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "showCorrectAnswer", layerPresentation.ToString()));
            }
        }

        private void startGameshowFindCard()
        {
            if (cmbTemplateGameshowFindCard.SelectedIndex == -1) return;
            if (cmbGameShowFindCardMatches.SelectedIndex != -1)
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateFindCard = new Template();
                    String basePath = AppController.getInstance().getFullMediaPath();
                    Uri frontPicPath = new Uri(basePath + GameShowController.getFindCardItemPictureByType(CGClientConstants.FindCardItemType.FRONT_CARD) + ".png");
                    Uri backPicPath = new Uri(basePath + GameShowController.getFindCardItemPictureByType(CGClientConstants.FindCardItemType.LOOSER_CARD) + ".png");
                    Uri winPicPath = new Uri(basePath + GameShowController.getFindCardItemPictureByType(CGClientConstants.FindCardItemType.WINNER_CARD) + ".png");
                    long winPos = GameShowController.getFindCardWinnerPositionById(long.Parse(cmbGameShowFindCardMatches.Text));

                    templateFindCard.Fields.Add(new TemplateField("frontPicture", frontPicPath.ToString()));
                    templateFindCard.Fields.Add(new TemplateField("backPicture", backPicPath.ToString()));
                    templateFindCard.Fields.Add(new TemplateField("winnerPicture", winPicPath.ToString()));
                    templateFindCard.Fields.Add(new TemplateField("winnerCardPosition", winPos.ToString()));
                    templateFindCard.Fields.Add(new TemplateField("rotationSecondsDuration", cmbGameShowFindCardRotationSecondsDuration.Text));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateFindCard.TemplateDataText(), cmbTemplateGameshowFindCard.Text, layerPresentation.ToString()));
                    txtLogMessages.Text += "\n" + ri.Message;
                    //System.Diagnostics.Debug.WriteLine(ri.Message);
                }
            }
        }

        private void updateGameshowFindCard(int position)
        {
            if (cmbTemplateGameshowFindCard.SelectedIndex == -1) return;
            if (cmbGameShowFindCardMatches.SelectedIndex == -1) return;
            if (!chkGameShowFindCardAutoUpdate.Checked) return;
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateFindCard = new Template();

                templateFindCard.Fields.Add(new TemplateField("rotateCardPosition", position.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateFindCard.TemplateDataText(), layerPresentation.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopGameshowFindCard()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
            }
        }

        private void showGameshowFindCardShowAllCards()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                chkGameShowFindCardAutoUpdate.Checked = false;
                settingFindCardsCheck(true);
                chkGameShowFindCardAutoUpdate.Checked = true;
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "showAllCards", layerPresentation.ToString()));
            }
        }

        private void showGameshowFindCardHideAllCards()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                chkGameShowFindCardAutoUpdate.Checked = false;
                settingFindCardsCheck(false);
                chkGameShowFindCardAutoUpdate.Checked = true;
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "hideAllCards", layerPresentation.ToString()));
            }
        }

        private void settingFindCardsCheck(Boolean state)
        {
            foreach (CheckBox item in tlpGameShowFindCardsItems.Controls)
	        {
                item.Checked = state;
	        } 
        }

        private void startWeatherForecast()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateCountdown = new Template();
                    //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                    if (WeatherController.getWeatherForecasts().Count >= 1)
                    {
                        String xmlCity1 = Utils.Base64Encode(Utils.ConvertXML(WeatherController.getWeatherForecasts()[0]));
                        String xmlCity2 = Utils.Base64Encode(Utils.ConvertXML(WeatherController.getWeatherForecasts()[1]));
                        String xmlCity3 = Utils.Base64Encode(Utils.ConvertXML(WeatherController.getWeatherForecasts()[2]));

                        //templateCountdown.Fields.Add(new TemplateField("questionText", txtGameshowQuestion.Text));
                        templateCountdown.Fields.Add(new TemplateField("city1", xmlCity1));
                        templateCountdown.Fields.Add(new TemplateField("city2", xmlCity2));
                        templateCountdown.Fields.Add(new TemplateField("city3", xmlCity3));
                        templateCountdown.Fields.Add(new TemplateField("startDelay", nudWeatherForecastStartDelaySeconds.Value.ToString()));
                        templateCountdown.Fields.Add(new TemplateField("pauseLength", nudWeatherForecastPauseSeconds.Value.ToString()));
                    }

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateCountdown.TemplateDataText(), cmbTemplateWeatherForecast.Text, layerPresentation.ToString()));
                    txtLogMessages.Text += "\n" + ri.Message;
                    //System.Diagnostics.Debug.WriteLine(ri.Message);
                }
            }
        }

        private void stopWeatherForecast()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
            }
        }

        private void showHideIntro()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (!btnShowHideIntro.Tag.ToString().Equals("1"))
                {
                    Template templateIntro = new Template();

                    Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

                    string t1 = (txtHomeTeamName.Text.Contains(",") ? txtHomeTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtHomeTeamName.Text.Split(',')[1] : txtHomeTeamName.Text);
                    string t2 = (txtGuestTeamName.Text.Contains(",") ? txtGuestTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtGuestTeamName.Text.Split(',')[1] : txtGuestTeamName.Text);

                    templateIntro.Fields.Add(new TemplateField("team1Name", t1));
                    templateIntro.Fields.Add(new TemplateField("team2Name", t2));
                    templateIntro.Fields.Add(new TemplateField("infoLeague", txtEventLeague.Text));
                    templateIntro.Fields.Add(new TemplateField("infoDate", txtIntroTitle.Text));
                    templateIntro.Fields.Add(new TemplateField("infoLocation", txtEventLocation.Text));
                    templateIntro.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                    templateIntro.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));
                    templateIntro.Fields.Add(new TemplateField("website", txtVolleyWebsite.Text));


                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateIntro.Text, layerPresentation.ToString()));
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
                    AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerPresentation.ToString()));
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
            if (!btnSource.Tag.ToString().Equals("1"))
            {
                String players;
                players = getPlayerList(lvwPlayers);

                Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogo.Text.ToLower() + ".png");

                Dictionary<string, string> teamValues = new Dictionary<string, string>()
                {
                    {"teamName", txtTeam.Text},{"teamPlayers", players},{"teamCoach", "Entrenador: " + txtCoach.Text},{"teamLogo", logoPath.ToString()},
                    {"fontSizePlayers", cmbPlayersFontSize.Text},{"fontLineSpacingPlayers", cmbPlayersFontLineSpacing.Text},{"fontLetterSpacingPlayers", cmbPlayersFontLetterSpacing.Text}
                };


                SportsController.startTeamsheet(cmbTemplateTeam.Text, layerTeams, teamValues);
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
                SportsController.stopTeamsheet(layerTeams);
                btnSource.Tag = "0";
                if (chkAutoHide.Checked)
                {
                    timer.Stop();
                    timer.Enabled = false;
                }
            }
        }

        private void autoShowHideTeams()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                switch (btnShowHomeAndGuestTeam.Tag.ToString())
                {
                    case "0":
                        btnShowHomeAndGuestTeam.Tag = "1";
                        String players;
                        players = getPlayerList(lvwHomeTeamPlayers);

                        Template templateIntro = new Template();

                        Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtHomeTeamName.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtHomeTeamCoach.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateTeam.Text, layerTeams.ToString()));

                        tmrTeams.Interval = ((int)nudTeamsShowSeconds.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "1":
                        btnShowHomeAndGuestTeam.Tag = "2";
                        AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));

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

                        logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

                        templateIntro.Fields.Add(new TemplateField("teamName", txtGuestTeamName.Text));
                        templateIntro.Fields.Add(new TemplateField("teamPlayers", players));
                        templateIntro.Fields.Add(new TemplateField("teamCoach", "Entrenador: " + txtGuestTeamPlayers.Text));
                        templateIntro.Fields.Add(new TemplateField("teamLogo", logoPath.ToString()));

                        ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateTeam.Text, layerTeams.ToString()));

                        tmrInBetweenTeams.Stop();
                        tmrInBetweenTeams.Enabled = false;

                        tmrTeams.Interval = ((int)nudTeamsShowSeconds.Value) * 1000;
                        tmrTeams.Enabled = true;
                        tmrTeams.Start();
                        break;
                    case "3":
                        btnShowHomeAndGuestTeam.Tag = "0";
                        AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTeams.ToString()));

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
            if (!btnShowHideResult.Tag.ToString().Equals("1"))
            {
                startTmplResult();

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
                stopTmplResult();

                btnShowHideResult.Tag = "0";
                if (chkAutoHideResult.Checked)
                {
                    tmrResult.Stop();
                    tmrResult.Enabled = false;
                }
            }
        }

        private void startTmplResult()
        {
            Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
            Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

            Dictionary<string, string> templateResultValues = new Dictionary<string, string>();

            templateResultValues = new Dictionary<string, string>(){
                {"team1Name", txtHomeTeamName.Text},{"team2Name", txtGuestTeamName.Text},
                {"team1Score", nudHomeTeamScore.Value.ToString()},{"team2Score", nudGuestTeamScore.Value.ToString()},
                {"team1Logo", logo1Path.ToString()},{"team2Logo", logo2Path.ToString()},
                {"timeResult", String.Empty}
            };

            SportsController.startResult(cmbTemplateResult.Text, layerResult, templateResultValues);
        }

        private void stopTmplResult()
        {
            SportsController.stopResult(layerResult);
        }

        private void startLowerThird()
        {
            if (cmbTemplateLowerThird.Text != "")
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateLowerThird = new Template();

                    Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + ((radHomeTeamPlayers.Checked) ? cmbHomeTeamLogo.Text.ToLower() : cmbGuestTeamLogo.Text.ToLower()) + ".png");

                    templateLowerThird.Fields.Add(new TemplateField("teamPlayer", txtLTTitle.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamName", txtLTSubtitle.Text));
                    templateLowerThird.Fields.Add(new TemplateField("teamLogo", logoPath.AbsoluteUri));

                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateLowerThird.TemplateDataText(), cmbTemplateLowerThird.Text, layerLowerThird.ToString()));
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
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerLowerThird.ToString()));
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
                    Dictionary<string, string> positionsValues;

                    Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                    positionsValues = new Dictionary<string, string>() 
                    {
                        {"logo", logoPath.AbsoluteUri},{"title", txtPositionsTitle.Text},{"subtitle", txtPositionsSubtitle.Text},
                        {"line01", "#;Equipo;Pts;PJ;PG;PE;PP;GF;GC;Dif"},{"line02", getPositionCVS(1)},{"line03", getPositionCVS(2)},
                        {"line04", getPositionCVS(3)},{"line05", getPositionCVS(4)},{"line06", getPositionCVS(5)},
                        {"line07", getPositionCVS(6)},{"line08", getPositionCVS(7)},{"line09", getPositionCVS(8)},
                        {"line10", getPositionCVS(9)}
                    };

                    SportsController.startTemplatePositions(cmbTemplatePositions.Text, layerPositions, positionsValues);
                    if (chkAutoHidePositions.Checked)
                    {
                        tmrPositions.Interval = ((int)nudAutoHidePositionsSeconds.Value) * 1000;
                        tmrPositions.Enabled = true;
                        tmrPositions.Start();
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
            SportsController.stopTemplatePositions(layerPositions);
            if (chkAutoHidePositions.Checked)
            {
                tmrPositions.Stop();
                tmrPositions.Enabled = false;
            }
        }

        private void showHideRugbyIntro()
        {
            if (!btnStartStopRugbyIntro.Tag.ToString().Equals("1"))
            {
                //Template templateIntro = new Template();

                //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

                //string t1 = (txtHomeTeamName.Text.Contains(",") ? txtHomeTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtHomeTeamName.Text.Split(',')[1] : txtHomeTeamName.Text);
                //string t2 = (txtGuestTeamName.Text.Contains(",") ? txtGuestTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtGuestTeamName.Text.Split(',')[1] : txtGuestTeamName.Text);

                //templateIntro.Fields.Add(new TemplateField("team1Name", t1));
                //templateIntro.Fields.Add(new TemplateField("team2Name", t2));
                //templateIntro.Fields.Add(new TemplateField("infoLeague", txtEventLeague.Text));
                //templateIntro.Fields.Add(new TemplateField("infoDate", txtIntroTitle.Text));
                //templateIntro.Fields.Add(new TemplateField("infoLocation", txtEventLocation.Text));
                //templateIntro.Fields.Add(new TemplateField("team1Logo", logo1Path.ToString()));
                //templateIntro.Fields.Add(new TemplateField("team2Logo", logo2Path.ToString()));
                //templateIntro.Fields.Add(new TemplateField("website", txtVolleyWebsite.Text));


                //ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateIntro.TemplateDataText(), cmbTemplateIntro.Text, layerPresentation.ToString()));
                SportsController.startRugbyIntro(cmbRugbyIntroTemplate.Text, layerPresentation, new Dictionary<string, string>());

                btnStartStopRugbyIntro.Tag = "1";
                if (chkRugbyIntroAutoHide.Checked)
                {
                    tmrIntro.Interval = ((int)nudRugbyIntroAutoHideSeconds.Value) * 1000;
                    tmrIntro.Enabled = true;
                    tmrIntro.Start();
                }
            }
            else
            {
                SportsController.stopRugbyIntro(layerPresentation);

                btnStartStopRugbyIntro.Tag = "0";
                if (chkRugbyIntroAutoHide.Checked)
                {
                    tmrIntro.Stop();
                    tmrIntro.Enabled = false;
                }
            }
        }


        private void showHideRugbyLineup()
        {
            if (!btnStartStopRugbyLineup.Tag.ToString().Equals("1"))
            {
                //Template templateIntro = new Template();

                //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

                //string t1 = (txtHomeTeamName.Text.Contains(",") ? txtHomeTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtHomeTeamName.Text.Split(',')[1] : txtHomeTeamName.Text);
                //string t2 = (txtGuestTeamName.Text.Contains(",") ? txtGuestTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtGuestTeamName.Text.Split(',')[1] : txtGuestTeamName.Text);

                //templateIntro.Fields.Add(new TemplateField("team1Name", t1));
                //templateIntro.Fields.Add(new TemplateField("team2Name", t2));

                SportsController.startRugbyLineup(cmbTemplateRugbyLineup.Text, layerPresentation, new Dictionary<string, string>());

                btnStartStopRugbyLineup.Tag = "1";
                if (chkRugbyLineupAutoHide.Checked)
                {
                    tmrIntro.Interval = ((int)nudRugbyLineupAutoHideSeconds.Value) * 1000;
                    tmrIntro.Enabled = true;
                    tmrIntro.Start();
                }
            }
            else
            {
                SportsController.stopRugbyLineup(layerPresentation);

                btnStartStopRugbyLineup.Tag = "0";
                if (chkRugbyLineupAutoHide.Checked)
                {
                    tmrIntro.Stop();
                    tmrIntro.Enabled = false;
                }
            }
        }

        private void showHideRugbyResult()
        {
            if (!btnStartStopRugbyResult.Tag.ToString().Equals("1"))
            {
                //Template templateIntro = new Template();

                //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");

                //string t1 = (txtHomeTeamName.Text.Contains(",") ? txtHomeTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtHomeTeamName.Text.Split(',')[1] : txtHomeTeamName.Text);
                //string t2 = (txtGuestTeamName.Text.Contains(",") ? txtGuestTeamName.Text.Split(',')[0].ToUpper() + "\n" + txtGuestTeamName.Text.Split(',')[1] : txtGuestTeamName.Text);

                //templateIntro.Fields.Add(new TemplateField("team1Name", t1));
                //templateIntro.Fields.Add(new TemplateField("team2Name", t2));

                SportsController.startRugbyResult(cmbTemplateRugbyResult.Text, layerPresentation, new Dictionary<string, string>());

                btnStartStopRugbyResult.Tag = "1";
                if (chkRugbyResultAutoHide.Checked)
                {
                    tmrIntro.Interval = ((int)nudRugbyResultAutoHideSeconds.Value) * 1000;
                    tmrIntro.Enabled = true;
                    tmrIntro.Start();
                }
            }
            else
            {
                SportsController.stopRugbyResult(layerPresentation);

                btnStartStopRugbyResult.Tag = "0";
                if (chkRugbyResultAutoHide.Checked)
                {
                    tmrIntro.Stop();
                    tmrIntro.Enabled = false;
                }
            }
        }

        private void showHideRugbyOfficials()
        {
            if (!btnStartStopRugbyOfficials.Tag.ToString().Equals("1"))
            {
                SportsController.startRugbyOfficials(cmbTemplateRugbyOfficials.Text, layerPresentation, new Dictionary<string, string>());

                btnStartStopRugbyOfficials.Tag = "1";
                if (chkRugbyOfficialsAutoHide.Checked)
                {
                    tmrIntro.Interval = ((int)nudRugbyResultAutoHideSeconds.Value) * 1000;
                    tmrIntro.Enabled = true;
                    tmrIntro.Start();
                }
            }
            else
            {
                SportsController.stopRugbyOfficials(layerPresentation);

                btnStartStopRugbyOfficials.Tag = "0";
                if (chkRugbyOfficialsAutoHide.Checked)
                {
                    tmrIntro.Stop();
                    tmrIntro.Enabled = false;
                }
            }
        }

        private void startTwitter()
        {
            if (cmbTemplateTwitter.Text != "")
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateTwitter = new Template();

                    templateTwitter.Fields.Add(new TemplateField("hashtag", txtTwitterHashtag.Text));
                    templateTwitter.Fields.Add(new TemplateField("fullname", txtTwitterFullName.Text));
                    templateTwitter.Fields.Add(new TemplateField("username", txtTwitterUserName.Text));
                    templateTwitter.Fields.Add(new TemplateField("message", txtTwitterMessage.Text));

                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), cmbTemplateTwitter.Text, layerTwitter.ToString()));

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
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTwitter.ToString()));
                if (chkAutoHideTwitter.Checked)
                {
                    tmrTwitter.Stop();
                    tmrTwitter.Enabled = false;
                }
            }
        }

        private void startTwitterCounter()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateTwitter = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                if (chkTwitterCounterPosition.Checked)
                {
                    templateTwitter.Fields.Add(new TemplateField("positionX", nudTwitterCounterPosX.Value.ToString()));
                    templateTwitter.Fields.Add(new TemplateField("positionY", nudTwitterCounterPosY.Value.ToString()));
                }
                templateTwitter.Fields.Add(new TemplateField("twitterHashtag", txtTwitterCounterHashtag.Text));
                templateTwitter.Fields.Add(new TemplateField("twitterCounter", nudTwitterCounter.Value.ToString()));
                templateTwitter.Fields.Add(new TemplateField("logoAlpha", "100"));
                templateTwitter.Fields.Add(new TemplateField("counterAnimated", (chkTwitterCounterIsAnimated.Checked ? "true" : "false")));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), cmbTemplateTwitterCounter.Text, layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void updateTwitterCounter()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateTwitter = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                if (chkTwitterCounterPosition.Checked)
                {
                    templateTwitter.Fields.Add(new TemplateField("positionX", nudTwitterCounterPosX.Value.ToString()));
                    templateTwitter.Fields.Add(new TemplateField("positionY", nudTwitterCounterPosY.Value.ToString()));
                }
                templateTwitter.Fields.Add(new TemplateField("twitterHashtag", txtTwitterCounterHashtag.Text));
                templateTwitter.Fields.Add(new TemplateField("twitterCounter", nudTwitterCounter.Value.ToString()));
                templateTwitter.Fields.Add(new TemplateField("logoAlpha", "100"));
                templateTwitter.Fields.Add(new TemplateField("counterAnimated", (chkTwitterCounterIsAnimated.Checked?"true":"false")));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopTwitterCounter()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTwitterCounter.ToString()));
            }
        }

        private void enableTwitterCounterPosition()
        {
            nudTwitterCounterPosX.Enabled = chkTwitterCounterPosition.Checked;
            nudTwitterCounterPosY.Enabled = chkTwitterCounterPosition.Checked;
        }

        private void showDynamicNewsTicker()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
        		string af = "\\" + Microsoft.VisualBasic.Strings.ChrW(0x22);

                String xml = "<templateData>";
                xml += "<componentData id=" + af + "sideTitle" + af + ">";
                xml += "<data id=" + af + "text" + af + " value=" + af + cmbDynamicNewsTickerCategory.Text + af + " />";
                xml += "</componentData>";
                xml += "<componentData id=" + af + "crawl" + af + ">";
                xml += "<action type=" + af + "set" + af + ">";

                foreach (ListViewItem item in lvwDynamicNewsTicker.Items)
                {
                    xml += "<data id=" + af + "text" + af + " value=" + af + item.SubItems[2].Text +af + " />";
                }

                xml += "</action>";
                xml += "</componentData>";
                xml += "</templateData>";

                Template templateCountdown = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                templateCountdown.Fields.Add(new TemplateField("sideTitle", nudGameshowCounterSeconds.Value.ToString()));
                templateCountdown.Fields.Add(new TemplateField("crawl", nudGameshowRefreshMiliseconds.Value.ToString()));
                //templateCountdown.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                //ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateCountdown.TemplateDataText(), cmbTemplateDynamicNewsTicker.Text, layerScoreboard.ToString()));
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, xml, cmbTemplateDynamicNewsTicker.Text, layerScoreboard.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void hideDynamicNewsTicker()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerScoreboard.ToString()));
            }
        }

        private void startDynamicNewsTicker()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "countdownStartStop", layerScoreboard.ToString()));
            }
        }

        private void stopDynamicNewsTicker()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "countdownStartStop", layerScoreboard.ToString()));
            }
        }

        private void startTwitterPoll()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateTwitter = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                templateTwitter.Fields.Add(new TemplateField("pollTitle", txtTwitterPollTitle.Text));
                templateTwitter.Fields.Add(new TemplateField("pollText", txtTwitterPollText.Text));
                int index = 0;
                foreach (ListViewItem item in lvwTwitterPollHashtags.Items)
                {
                    index++;
                    templateTwitter.Fields.Add(new TemplateField("hashtag" + index.ToString(), item.SubItems[1].Text));
                    templateTwitter.Fields.Add(new TemplateField("percentage" + index.ToString(), item.SubItems[3].Text.Replace(",", ".")));

                }
                templateTwitter.Fields.Add(new TemplateField("counterAnimated", (chkTwitterPollIsCounterAnimated.Checked ? "true" : "false")));
                templateTwitter.Fields.Add(new TemplateField("percentageAnimated", (chkTwitterPollIsPercentageAnimated.Checked ? "true" : "false")));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), cmbTemplateTwitterPoll.Text, layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void updateTwitterPoll()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateTwitter = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                templateTwitter.Fields.Add(new TemplateField("pollTitle", txtTwitterPollTitle.Text));
                templateTwitter.Fields.Add(new TemplateField("pollText", txtTwitterPollText.Text));
                int index = 0;
                foreach (ListViewItem item in lvwTwitterPollHashtags.Items)
                {
                    index++;
                    templateTwitter.Fields.Add(new TemplateField("hashtag" + index.ToString(), item.SubItems[1].Text));
                    templateTwitter.Fields.Add(new TemplateField("percentage" + index.ToString(), item.SubItems[3].Text.Replace(",",".")));
                    
                }
                templateTwitter.Fields.Add(new TemplateField("counterAnimated", (chkTwitterPollIsCounterAnimated.Checked ? "true" : "false")));
                templateTwitter.Fields.Add(new TemplateField("percentageAnimated", (chkTwitterPollIsPercentageAnimated.Checked ? "true" : "false")));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void stopTwitterPoll()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTwitterCounter.ToString()));
            }
        }

        private void startTwitterPlaylist()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                //if (lvwTwitterPlaylistDetailList.SelectedItems[0] == null) return;

                Playlist aux = HandballMatch.getInstance().playlists.Find(element => element.id == int.Parse(lblTwitterPlaylistId.Text));
                Tweets tweet = aux.tweets.Find(element => element.id == long.Parse(lvwTwitterPlaylistDetailList.SelectedItems[0].Text));

                Template templateTwitter = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                templateTwitter.Fields.Add(new TemplateField("datetime", tweet.dateTime));
                templateTwitter.Fields.Add(new TemplateField("fullname", tweet.fullName));
                templateTwitter.Fields.Add(new TemplateField("username", tweet.userName));
                templateTwitter.Fields.Add(new TemplateField("message", tweet.message));
                templateTwitter.Fields.Add(new TemplateField("picAvatar", tweet.profileImages.Find(pi => pi.profileImageType == "profile").profileImageUrl));

                AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{3} {0}{1}{0} {2}", (char)0x22, cmbTwitterPlaylistBGVideo.Text, "LOOP", layerVideo.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), cmbTemplateTwitterPlaylist.Text, layerTwitter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;

                //if (chkAutoHideTwitter.Checked)
                //{
                //    tmrTwitter.Interval = ((int)nudAutoHideTwitterSeconds.Value) * 1000;
                //    tmrTwitter.Enabled = true;
                //    tmrTwitter.Start();
                //}
            }
        }

        private void updateTwitterPlaylist()
        {
            //if (AppController.getInstance().isConnectedToCasparCgServer())
            //{
            //    Template templateTwitter = new Template();
            //    //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

            //    templateTwitter.Fields.Add(new TemplateField("pollTitle", txtTwitterPollTitle.Text));
            //    templateTwitter.Fields.Add(new TemplateField("pollText", txtTwitterPollText.Text));
            //    int index = 0;
            //    foreach (ListViewItem item in lvwTwitterPollHashtags.Items)
            //    {
            //        index++;
            //        templateTwitter.Fields.Add(new TemplateField("hashtag" + index.ToString(), item.SubItems[1].Text));
            //        templateTwitter.Fields.Add(new TemplateField("percentage" + index.ToString(), item.SubItems[3].Text.Replace(",", ".")));

            //    }
            //    templateTwitter.Fields.Add(new TemplateField("counterAnimated", (chkTwitterPollIsCounterAnimated.Checked ? "true" : "false")));
            //    templateTwitter.Fields.Add(new TemplateField("percentageAnimated", (chkTwitterPollIsPercentageAnimated.Checked ? "true" : "false")));
            //    //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

            //    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateTwitter.TemplateDataText(), layerTwitterCounter.ToString()));
            //    txtLogMessages.Text += "\n" + ri.Message;
            //    //System.Diagnostics.Debug.WriteLine(ri.Message);
            //}
        }

        private void stopTwitterPlaylist()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTwitter.ToString()));
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerVideo.ToString()));

                //if (chkAutoHideTwitter.Checked)
                //{
                //    tmrTwitter.Stop();
                //    tmrTwitter.Enabled = false;
                //}
            }
        }


        private void startGameplay()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                getImageCanvas();

                Template templateGameplay = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                templateGameplay.Fields.Add(new TemplateField("positionX", "0.5"));
                templateGameplay.Fields.Add(new TemplateField("positionY", "0.5"));
                templateGameplay.Fields.Add(new TemplateField("pointColor", "0x00ff00"));
                templateGameplay.Fields.Add(new TemplateField("pointWidth", "10"));
                templateGameplay.Fields.Add(new TemplateField("pointHeight", "10"));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateGameplay.TemplateDataText(), cmbTemplateGameplay.Text, layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                AppController.getInstance().executeCasparCgServer(String.Format("MIXER 1-{0} CHROMA BLUE 0.10 0.24", layerTwitterCounter.ToString()));
                //System.Diagnostics.Debug.WriteLine(ri.Message);

                fillImagesCombo();
            }
        }

        private void getImageCanvas()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("PRINT {0}", layerVideo.ToString()));
                txtLogMessages.Text += "\n" + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void fillImagesCombo()
        {
            AppController.getInstance().getServerImages();
            List<String> medias = AppController.getInstance().getImages();
            List<String> images = medias.FindAll(m => m.StartsWith("2016"));
            fillCombosTemplate(cmbGameplayImages, images);
            cmbGameplayImages.SelectedIndex = cmbGameplayImages.Items.Count - 1;
        }

        private void setImageAsCanvas()
        {
            Uri file = new Uri(AppController.getInstance().getFullMediaPath() + cmbGameplayImages.Text.ToLower() + ".png");

            picGameplayWhiteboard.SizeMode = PictureBoxSizeMode.StretchImage;
            picGameplayWhiteboard.Image = Image.FromFile(file.LocalPath);
        }

        private void updateGameplay(String mouseEvent,double porcX, double porcY)
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateGameplay = new Template();
                //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbLogoFile.Text.ToLower() + ".png");

                    templateGameplay.Fields.Add(new TemplateField("mouseEvent", mouseEvent));
                    templateGameplay.Fields.Add(new TemplateField("positionX", porcX.ToString().Replace(",", ".")));
                    templateGameplay.Fields.Add(new TemplateField("positionY", porcY.ToString().Replace(",",".")));
                    templateGameplay.Fields.Add(new TemplateField("pointColor", "0xff0000"));
                    templateGameplay.Fields.Add(new TemplateField("pointWidth", "10"));
                //templateGameplay.Fields.Add(new TemplateField("pointHeight", "10"));
                //templateDynamicLogo.Fields.Add(new TemplateField("logoFile", logoPath.ToString()));

                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateGameplay.TemplateDataText(), layerTwitterCounter.ToString()));
                txtLogMessages.Text += "\n" + mouseEvent + ": " + ri.Message;
                //System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void callGameplayTool(String toolName)
        {
            String command = ""; ;
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                switch (toolName)
                {
                    case "Pencil":
                        command = "setPencilTool";
                        break;
                    case "Clear":
                        command = "ClearBoard";
                        break;
                    default:
                        break;
                }

                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", command, layerTwitterCounter.ToString()));
            }
        }


        private void stopGameplay()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerTwitterCounter.ToString()));
                AppController.getInstance().executeCasparCgServer(String.Format("MIXER 1-{0} CHROMA NONE", layerTwitterCounter.ToString()));
                cleanImageCanvas();
            }
        }

        private void cleanImageCanvas()
        {
            picGameplayWhiteboard.Image = null;
        }

        //private void playGameplayVideo()
        //{
        //    if (AppController.getInstance().isConnectedToCasparCgServer())
        //    {
        //        AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{0} AMB LENGTH 150", layerVideo.ToString()));
        //        AppController.getInstance().executeCasparCgServer(String.Format("VERSION", layerVideo.ToString()));
        //        AppController.getInstance().executeCasparCgServer(String.Format("LOADBG 1-{0} AMB LOOP SEEK 150 LENGTH 1", layerVideo.ToString()));
        //    }
        //}

        //private void stopGameplayVideo()
        //{
        //    if (AppController.getInstance().isConnectedToCasparCgServer())
        //    {
        //        AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{0} AMB SEEK 151", layerVideo.ToString()));
        //        //AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{0}", layerVideo.ToString()));
        //    }
        //}
        
        private void playGameplayMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (lvwGameplayPlaylist.SelectedItems.Count != 0)
                {
                    muteGameplayVolume();
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{3} {0}{1}{0} {2}", (char)0x22, lvwGameplayPlaylist.SelectedItems[0].SubItems[0].Text, (false ? "LOOP" : ""), layerVideo.ToString()));
                }
            }
        }

        private void stopGameplayMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerVideo.ToString()));
            }
        }

        private void pauseGameplayMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("PAUSE 1-{0}", layerVideo.ToString()));
            }
        }

        private void muteGameplayVolume()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("MIXER 1 MASTERVOLUME {0}", 0));
            }
        }

        private void startVolleyScoreboard()
        {
            if (cmbTemplateVolleyScoreboard.Text != "")
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateVolleyScoreboard = new Template();
                    //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");
                    Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

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

                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateVolleyScoreboard.TemplateDataText(), cmbTemplateVolleyScoreboard.Text, layerVolleyScoreboard.ToString()));
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template)", this.Text);
            }
        }

        private void stopVolleyScoreboard()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerVolleyScoreboard.ToString()));
            }
        }

        private void updateVolleyScoreboard()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateVolleyScoreboard = new Template();
                //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");
                Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");


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
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateVolleyScoreboard.TemplateDataText(), layerVolleyScoreboard.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
            }
        }

        private void addVolleyGamePoint(Button SourceButton)
        {
            getVolleySetScore((SourceButton.Name.Contains("Home") ? 1 : 2)).Value++;

            evaluateVolleyService();

            evaluateVolleySetEnd();

            watchVolleyGame();
        }

        private void evaluateVolleyService()
        {
            RadioButton radioChecked = new RadioButton();
            RadioButton radionNotChecked = new RadioButton();

            if (radVolleyHomeServe.Checked)
            {
                radioChecked = radVolleyHomeServe;
                radionNotChecked = radVolleyGuestServe;
            }
            else
            {
                radioChecked = radVolleyGuestServe;
                radionNotChecked = radVolleyHomeServe;
            }

            if (((int)radioChecked.Tag) >= ((int)nudVolleyServicesPerPlayer.Value))
            {
                radioChecked.Checked = false;
                radioChecked.Tag = 0;
                radioChecked.Text = "Servicio";
                radionNotChecked.Checked = true;
                radionNotChecked.Tag = 1;
                radionNotChecked.Text = "Servicio [" + (int)radionNotChecked.Tag + "]";
            }
            else
            {
                radioChecked.Tag = ((int)radioChecked.Tag) + 1;
                radioChecked.Text = "Servicio [" + (int)radioChecked.Tag + "]";
                radionNotChecked.Tag = 0;
            }

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
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateVolleyResult = new Template();
                    //Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbHomeTeamLogo.Text.ToLower() + ".png");
                    //Uri logo2Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbGuestTeamLogo.Text.ToLower() + ".png");
                    Uri logo1Path = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

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

                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateVolleyResult.TemplateDataText(), cmbTemplateVolleyResult.Text, layerVolleyResult.ToString()));

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
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerVolleyResult.ToString()));
                if (chkAutoHideVolleyResult.Checked)
                {
                    tmrVolleyResult.Stop();
                    tmrVolleyResult.Enabled = false;
                }
            }
        }

        private void startRecording()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (!txtRecordingFileName.Text.Equals(""))
                {
                    //String aux=String.Format("ADD 1 FILE {0}-{1}-{2}.mov -vcodec libx264 [-preset ultrafast -tune fastdecode -crf 5]","REC",txtArchivoGrabacion.Text,DateTime.Now.ToString("ddMM-HHmm"));
                    String aux = String.Format("ADD 1 FILE {0}-{1}-{2}.mp4 -vcodec libx264 -preset ultrafast -tune fastdecode -crf {3}", "REC", txtRecordingFileName.Text, DateTime.Now.ToString("ddMMHHmm"), HandballMatch.getInstance().recordingCRF);
                    AppController.getInstance().executeCasparCgServer(aux);
                    ColorRecording = btnStartRecording.BackColor;
                    btnStartRecording.BackColor = Color.Red;
                }
            }
        }

        private void stopRecording()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer("REMOVE 1 FILE");
                btnStartRecording.BackColor = ColorRecording;
            }
        }

        private void showLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (!cmbBroadcastLogo.Text.Equals(""))
                {
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{2} {0}{1}{0} MIX 15", (char)0x22, cmbBroadcastLogo.Text, layerLogo.ToString()));
                }
            }
        }

        private void hideLogo()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerLogo.ToString()));
            }
        }

        private void playMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (lvwVideoFiles.SelectedItems.Count != 0)
                {
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{3} {0}{1}{0} {2}", (char)0x22, lvwVideoFiles.SelectedItems[0].SubItems[0].Text, (chkLoopVideoFile.Checked ? "LOOP" : ""), layerVideo.ToString()));
                }
            }
        }

        private void stopMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerVideo.ToString()));
            }
        }

        private void pauseMedia()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("PAUSE 1-{0}", layerVideo.ToString()));
            }
        }

        private void setVolume()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("MIXER 1 MASTERVOLUME {0}", (trkVolume.Value * 10)));
            }
        }

        private void muteVolume()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                trkVolume.Value = 0;
                setVolume();
            }
        }

        private void startAudio()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (cmbAudioFiles.Text != "")
                {
                    //AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1 {0}{1}{0} CHANNEL_LAYOUT STEREO", (char)0x22, cmbAudioFiles.Text, (chkLoopAudioFile.Checked ? "LOOP" : "")));
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1 {0}{1}{0} {2}", (char)0x22, cmbAudioFiles.Text, (chkLoopAudioFile.Checked ? "LOOP" : "")));
                }
            }
        }

        private void stopAudio()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1"));
            }
        }

        private void pauseAudio()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("PAUSE 1"));
            }
        }

        private void startWebcam()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (cmbWebcam.Text != "" && cmbWebcamResolution.Text != "")
                {
                    //AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{2} {0}dshow://video={1}{0} {0}-video_size {3} -framerate 30{0}", (char)0x22, cmbWebcam.Text, layerVideo.ToString(),cmbWebcamResolution.Text));
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{2} {0}dshow://video={1}{0} {0}-video_size {3} -framerate 30{0}", (char)0x22, cmbWebcam.Text, layerVideo.ToString(), cmbWebcamResolution.Text));
                }
            }
        }

        private void stopWebcam()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerVideo.ToString()));
            }
        }

        private void startImageScrolling()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                if (!cmbImageScrolling.Text.Equals(""))
                {
                    AppController.getInstance().executeCasparCgServer(String.Format("PLAY 1-{2} {0}{1}{0} BLUR 0 SPEED {3}", (char)0x22, cmbImageScrolling.Text, layerImageScrolling.ToString(), trkImageScrollingSpeed.Value));
                }
            }
        }

        private void stopImageScrolling()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("STOP 1-{0}", layerImageScrolling.ToString()));
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

        public void actionQuestion(int action, Question q)
        {
            if (action == 1)
            {
                addQuestion(q);
            }
            else
            {
                updateQuestion(q);
            }
        }

        private void addQuestion(Question q)
        {
            if (!HandballMatch.getInstance().gameshowQuestions.Exists(element => element.id == q.id))
            {
                HandballMatch.getInstance().gameshowQuestions.Add(q);
                FillGameshowQuestions(lvwGameshowQuestions, HandballMatch.getInstance().gameshowQuestions);
            }
        }

        private void updateQuestion(Question q)
        {
            if (HandballMatch.getInstance().gameshowQuestions.Exists(element => element.id == q.id))
            {
                HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == q.id).id = q.id;
                HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == q.id).question = q.question;
                HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == q.id).answers = q.answers;
                HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == q.id).correctAnswer = q.correctAnswer;
                FillGameshowQuestions(lvwGameshowQuestions, HandballMatch.getInstance().gameshowQuestions);
            }
        }

        private void clearQuestionInfo()
        {
            lvwGameshowQuestionAnswers.Items.Clear();
            txtGameshowQuestion.Text = "";
            cmbGameshowCorrectAnswer.SelectedIndex = -1;
            cmbGameshowPlayerAnswer.SelectedIndex = -1;
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

        private void getWeatherForecastStates()
        {
            String endpoint;
            endpoint = txtWeatherForecastWS.Text;

            if (chkWeatherForecastMockCall.Checked)
            {
                WeatherController.mockForecastStates();
            }
            else
            {
                WeatherController.getForecastStates(endpoint);
            }

            WeatherController.fillForecatStates(cmbWeatherStates);
        }

        private void getWeatherForecastStateCities()
        {
            String endpoint;
            endpoint = txtWeatherForecastWS.Text;

            if (chkWeatherForecastMockCall.Checked)
            {
                WeatherController.mockForecastStateCities();
            }
            else
            {
                WeatherController.getForecastStateCities(endpoint, ((Provincia)cmbWeatherStates.SelectedItem).id.ToString());
            }
            lvwCities.Items.Clear();
            lvwCities.Items.AddRange(WeatherController.fillSelectedStateCities(((Provincia)cmbWeatherStates.SelectedItem).id));

        }

        private void getWeatherForecastCityForecast()
        {
            if(chkWeatherForecastMockCall.Checked) 
            {
                WeatherController.mockForecastCityForecast();
            } 
            else 
            {
                WeatherController.getForecastCityForecast(txtWeatherForecastWS.Text, lvwForecastPlaylist.SelectedItems[0].Text);
            }

            lvwForecastPlaylist.Items.Clear();
            lvwForecastPlaylist.Items.AddRange(WeatherController.fillForecast(long.Parse(lvwCities.SelectedItems[0].Text)));
        }

        private void loadTwitterList()
        {
            TwitterController.mockingTwitterList();
            fillTwitterList();
        }

        private void retrieveTweetsFromWS()
        {
            this.Cursor = Cursors.WaitCursor;
            getTweets();
            this.Cursor = Cursors.Default;
        }

        private void loadTwitterSearchList()
        {
            if (chkTwitterSearchMockCall.Checked)
            {
                TwitterController.mockingTwitterList();
            }
            else
            {
                retrieveTwitterSearchFromWS();
            }
            fillTwitterSearchList();
        }

        private void retrieveTwitterSearchFromWS()
        {
            this.Cursor = Cursors.WaitCursor;
            getTwitterQuery();
            this.Cursor = Cursors.Default;
        }

        private void fillTwitterSearchList()
        {
            lvwTwitterSearchListResult.Items.Clear();
            lvwTwitterSearchListResult.Items.AddRange(TwitterController.fillTwitterSearchList());
        }

        private void fillTwitterList()
        {
            lvwTwitterList.Items.Clear();
            lvwTwitterList.Items.AddRange(TwitterController.fillTwitterList());
        }

        private void getTweets()
        {
            String endpoint;
            endpoint = txtTwitterWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/tweets/", Method.GET);

            var response = client.Execute(request);

            List<Tweets> tweetsSorted = new List<Tweets>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<Tweets> tweets = deserial.Deserialize<List<Tweets>>(response);

                tweetsSorted = tweets.OrderBy(e => e.id).ToList();
            }

            HandballMatch.getInstance().tweets = tweetsSorted;
        }

        private void loadTwitterPlaylistList()
        {
            if (chkTwitterPlaylistMockWSCall.Checked)
            {
                mockingTwitterPlaylist();
            }
            else
            {
                retrieveTwitterPlaylistFromWS();
            }
            fillPlaylistList();
        }

        private void retrieveTwitterPlaylistFromWS()
        {
            this.Cursor = Cursors.WaitCursor;
            getTwitterPlaylistList();
            this.Cursor = Cursors.Default;
        }

        private void getTwitterPlaylistList()
        {
            TwitterController.getTwitterPlaylistList(txtTwitterPlaylistWS.Text);
        }

        private void mockingTwitterPlaylist()
        {
            TwitterController.mockingTwitterPlaylist();
        }

        private void fillPlaylistList()
        {
            lvwTwitterPlaylistResult.Items.Clear();
            TwitterController.fillPlaylistList();
        }

        private void selectPlaylist()
        {
            if (lvwTwitterPlaylistResult.SelectedItems != null)
            {
                Playlist aux = TwitterController.selectPlaylist(long.Parse(lvwTwitterPlaylistResult.SelectedItems[0].Text));
                lblTwitterPlaylistId.Text = aux.id.ToString();
                lblTwitterPlaylistTitle.Text = aux.title;
                lblTwitterPlaylistDescription.Text = aux.description;
                //txtTwitterFullName.Text = aux.fullName;

                lvwTwitterPlaylistDetailList.Items.Clear();
                lvwTwitterPlaylistDetailList.Items.AddRange(TwitterController.fillSelectedPlaylistTweets(aux.id));
            }
        }

        private void getTwitterQuery()
        {
            TwitterController.getTwitterQuery(txtTwitterSearchWS.Text, txtTwitterSearchHashtag.Text);
        }

        private void removeTwitterSearchFromPlaylist()
        {
            if (lvwTwitterSearchPlaylistTweets.SelectedItems != null)
            {
                TwitterController.removeFromPlaylist(long.Parse(lvwTwitterSearchListResult.SelectedItems[0].Text));
                fillTwitterSearchPlaylist();
            }
        }

        private void addTwitterSearchToPlaylist()
        {
            if (lvwTwitterSearchListResult.SelectedItems != null)
            {
                Tweets tweet = HandballMatch.getInstance().tweets.Find(t => t.id == long.Parse(lvwTwitterSearchListResult.SelectedItems[0].Text));
                TwitterController.addToPlaylist(tweet);
                fillTwitterSearchPlaylist();
            }
        }

        private void fillTwitterSearchPlaylist()
        {
            Playlist aux = TwitterController.getPlaylist();
            txtTwitterSearchPlaylistTitle.Text = aux.title;
            txtTwitterSearchPlaylistDescription.Text = aux.description;
            dtpTwitterSearchPlaylistDate.Value = (aux.dateTime == null ? DateTime.Now : DateTime.Parse(aux.dateTime));

            lvwTwitterSearchPlaylistTweets.Items.Clear();
            lvwTwitterSearchPlaylistTweets.Items.AddRange(TwitterController.fillTwitterSearchPlaylist());
        }

        private void clearTwitterSearchPlaylist()
        {
            TwitterController.newPlaylist();
            fillTwitterSearchPlaylist();
        }

        private void saveTwitterSearchPlaylist()
        {
            if (txtTwitterSearchPlaylistWS.Text == "")
            {
                MessageBox.Show("Falta definir la url del WS!!");
                return;
            }

            if (txtTwitterSearchPlaylistTitle.Text == "" || lvwTwitterSearchPlaylistTweets.Items.Count == 0)
            {
                MessageBox.Show("Faltan datos para guardar el playlist");
                return;
            }

            TwitterController.setPlaylist(txtTwitterSearchPlaylistTitle.Text, dtpTwitterSearchPlaylistDate.Value, txtTwitterSearchPlaylistDescription.Text);

            if (chkTwitterSearchPlaylistMockCall.Checked)
            {
                mockSavePlaylist();
            }
            else
            {
                callSaveTwitterPlaylist();
            }
            clearTwitterSearchPlaylist();
        }

        private void mockSavePlaylist()
        {
            TwitterController.mockSavePlaylist();
        }

        private void callSaveTwitterPlaylist()
        {
            TwitterController.callSaveTwitterPlaylist(txtTwitterSearchPlaylistWS.Text);
        }

        private void selectTweet()
        {
            if (lvwTwitterList.SelectedItems != null)
            {
                Tweets aux = HandballMatch.getInstance().tweets.Find(element => element.id == long.Parse(lvwTwitterList.SelectedItems[0].Text));
                lblTwitterId.Text = aux.id.ToString();
                txtTwitterHashtag.Text = aux.hashtag;
                txtTwitterUserName.Text = aux.userName;
                txtTwitterFullName.Text = aux.fullName;
                txtTwitterMessage.Text = aux.message;
            }
        }

        private void fillTwitterCounter()
        {
            if (!chkTwitterCounterSimulateCounter.Checked)
            {
                callGetCountFromTwitterCounterWS();
            }
            else
            {
                simulateGetCount();
            }
        }

        private void simulateGetCount()
        {
            int count = 0;
            int startValue = (int)nudTwitterCounter.Value;

            Random generator = new Random();

            count = generator.Next(0, 100);

            nudTwitterCounter.Value = startValue + count;
        }

        private void callGetCountFromTwitterCounterWS()
        {
            String endpoint;
            endpoint = txtTwitterCounterWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/stream/count/", Method.GET);

            var response = client.Execute(request);

            int count = 0;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                count = int.Parse(response.Content.ToString());
            }

            nudTwitterCounter.Value = count;
        }

        private void callTwitterCounterWS(String action)
        {
            String endpoint;
            endpoint = txtTwitterCounterWS.Text;

            var client = new RestClient(endpoint);

            var url = "/stream/" + action + "/" + (action.Equals("start") ? "sdfadsf" : "");

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("query", txtTwitterCounterHashtag.Text);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                lblTwitterCounterStatus.Text = action + " succefully executed!!!";
            }
            else
            {
                lblTwitterCounterStatus.Text = "ERROR: " + response.ErrorMessage;
            }
        }

        private void callStatusTwitterCounterWS()
        {
            String endpoint;
            endpoint = txtTwitterCounterWS.Text;

            RestClient client = new RestClient(endpoint);

            RestRequest request = new RestRequest("/status/", Method.GET);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                lblTwitterCounterStatus.Text = response.Content.ToString();
            }
            else
            {
                lblTwitterCounterStatus.Text = "ERROR: " + response.ErrorMessage;
            }
        }

        private void loadDynamicInfoList()
        {
            mockingDynamicInfoList();
            fillDynamicInfoList();
        }

        private void mockingDynamicInfoList()
        {
            List<DynamicInfo> list = new List<DynamicInfo>();
            DynamicInfo di = new DynamicInfo();

            di.id = 1;
            di.header = "A CONTINUACION";
            di.title = "MUNDO D";
            di.info = "Todas las noticias";

            list.Add(di);

            di = new DynamicInfo();
            di.id = 2;
            di.header = "NUEVO CAPITULO";
            di.title = "TRIBUNA CELESTE";
            di.info = "";
            list.Add(di);


            HandballMatch.getInstance().dynamicInfo = list;
        }

        private void retrieveDynamicInfoFromWS()
        {
            this.Cursor = Cursors.WaitCursor;
            getDynamicInfo();
            this.Cursor = Cursors.Default;
        }

        private void fillDynamicInfoList()
        {
            lvwDynamicInfoList.Items.Clear();
            List<DynamicInfo> results = HandballMatch.getInstance().dynamicInfo;
            int i = 0;

            foreach (DynamicInfo item in results)
            {
                i++;
                string[] arr = new string[4];
                arr[0] = item.id.ToString();
                arr[1] = item.header;
                arr[2] = item.title;
                arr[3] = item.info;
                lvwDynamicInfoList.Items.Add(new ListViewItem(arr));
            }
        }

        private void getDynamicInfo()
        {
            String endpoint = txtDynamicInfoWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/dynamicInfo/", Method.GET);

            var response = client.Execute(request);

            List<DynamicInfo> tweetsSorted = new List<DynamicInfo>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<DynamicInfo> tweets = deserial.Deserialize<List<DynamicInfo>>(response);

                tweetsSorted = tweets.OrderBy(e => e.id).ToList();
            }

            HandballMatch.getInstance().dynamicInfo = tweetsSorted;
        }

        private void selectDynamicInfo()
        {
            if (lvwDynamicInfoList.SelectedItems != null)
            {
                DynamicInfo aux = HandballMatch.getInstance().dynamicInfo.Find(element => element.id == int.Parse(lvwDynamicInfoList.SelectedItems[0].Text));
                //lblTwitterId.Text = aux.id.ToString();
                txtDynamicInfoHeader.Text = aux.header;
                txtDynamicInfoTitle.Text = aux.title;
                txtDynamicInfoText.Text = aux.info;
                //txtTwitterMessage.Text = aux.mensaje;
            }
        }

        private void fillElectionsResults()
        {
            getElectionsResultsOrdered();

            lvwElectionsTop3Results.Items.Clear();
            List<Election> results = HandballMatch.getInstance().electionResults;
            int i = 0;

            foreach (Election item in results)
            {
                i++;
                string[] arr = new string[3];
                arr[0] = i.ToString();
                arr[1] = item.name;
                arr[2] = item.percentage.ToString() + "%";
                lvwElectionsTop3Results.Items.Add(new ListViewItem(arr));
            }
        }

        private void getElectionsResultsOrdered()
        {
            String endpoint;
            endpoint = txtElectionsTop3WS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/elections/", Method.GET);

            var response = client.Execute(request);

            List<Election> electionsSorted = new List<Election>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<Election> elections = deserial.Deserialize<List<Election>>(response);

                electionsSorted = elections.OrderByDescending(e => e.percentage).ToList();
            }

            HandballMatch.getInstance().electionResults = electionsSorted;
        }

        private void getDynamicNewsCategories()
        {
            String endpoint;
            endpoint = txtDynamicNewsTickerWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/deportes/", Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<Provincia> prov = deserial.Deserialize<List<Provincia>>(response);

                cmbDynamicNewsTickerCategory.Items.Clear();

                cmbDynamicNewsTickerCategory.DisplayMember = "nombre";
                cmbDynamicNewsTickerCategory.ValueMember = "id";
                cmbDynamicNewsTickerCategory.DataSource = prov;
            }
        }

        private void getDynamicNewsWS()
        {
            String endpoint;
            endpoint = txtWeatherForecastWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/deportes/{id}/noticias", Method.GET);

            request.AddUrlSegment("id", ((Provincia)cmbDynamicNewsTickerCategory.SelectedItem).id.ToString());

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<City> cities = deserial.Deserialize<List<City>>(response);

                string category = ((Provincia)cmbDynamicNewsTickerCategory.SelectedItem).nombre;
                lvwDynamicNewsTicker.Items.Clear();
                foreach (var c in cities)
                {
                    string[] arr = new string[3];
                    arr[0] = c.id.ToString();
                    arr[1] = category;
                    arr[2] = c.nombre;
                    lvwDynamicNewsTicker.Items.Add(new ListViewItem(arr));
                }
            }
        }


        private void fillTwitterPoll()
        {
            if (!chkTwitterPollWSSimulate.Checked)
            {
                callGetPollFromTwitterPollWS();
            }
            else
            {
                if (lvwTwitterPollHashtags.Items.Count == 0) return;
                simulateGetPoll();
                if (chkTwitterPollAutoUpdate.Checked)
                {
                    updateTwitterPoll();
                }
            }
        }

        private void simulateGetPoll()
        {
            int count = 0;
            int total = 0;

            Random generator = new Random();
            Random seed = new Random(1357);

            foreach (ListViewItem item in lvwTwitterPollHashtags.Items)
            {
                count = generator.Next(0, seed.Next(10,100));
                int aux = int.Parse(item.SubItems[2].Text) + count;
                item.SubItems[2].Text = aux.ToString();
                total += aux;
            }

            foreach (ListViewItem item in lvwTwitterPollHashtags.Items)
            {
                double aux = Math.Round((int.Parse(item.SubItems[2].Text) / (double)total) * 100, 2);
                item.SubItems[3].Text = aux.ToString();
            }
        }

        private void callGetPollFromTwitterPollWS()
        {
            String endpoint;
            endpoint = txtTwitterPollWS.Text;

            var client = new RestClient(endpoint);

            var request = new RestRequest("/stream/count/", Method.GET);

            var response = client.Execute(request);

            int count = 0;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                count = int.Parse(response.Content.ToString());
            }

            nudTwitterCounter.Value = count;
        }

        private void callTwitterPollWS(String action)
        {
            String endpoint;
            endpoint = txtTwitterPollWS.Text;

            var client = new RestClient(endpoint);

            var url = "/stream/" + action + "/" + (action.Equals("start") ? "sdfadsf" : "");

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("query", txtTwitterCounterHashtag.Text);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                lblTwitterPollStatus.Text = action + " succefully executed!!!";
            }
            else
            {
                lblTwitterPollStatus.Text = "ERROR: " + response.ErrorMessage;
            }
        }

        private void callStatusTwitterPollWS()
        {
            String endpoint;
            endpoint = txtTwitterPollWS.Text;

            RestClient client = new RestClient(endpoint);

            RestRequest request = new RestRequest("/status/", Method.GET);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                lblTwitterPollStatus.Text = response.Content.ToString();
            }
            else
            {
                lblTwitterPollStatus.Text = "ERROR: " + response.ErrorMessage;
            }
        }

        private void generateGameShowFindCardsMatches()
        {
            GameShowController.simulateFindCardMatches();
            GameShowController.simulateFindCardItems();
            fillFindCardsMatches();
            fillFindCardItems();
        }

        private void fillFindCardItems()
        {
            lvwGameShowFindCardItems.Items.Clear();
            lvwGameShowFindCardItems.Items.AddRange(GameShowController.fillListviewGameshowFindCardItems());
        }

        private void fillFindCardsMatches()
        {
            cmbGameShowFindCardMatches.Items.Clear();
            cmbGameShowFindCardMatches.Items.AddRange(GameShowController.fillComboboxGameshowFindCardMatches());
        }

        private void fillSelectedFindCardMatch()
        {
            if (cmbGameShowFindCardMatches.SelectedIndex != -1)
            {
                FindCardMatch aux = GameShowController.getFindCardMatchById(long.Parse(cmbGameShowFindCardMatches.Text));
                fillSelectedFindCardMatchCards(aux.Cards);
            }
        }

        private void fillSelectedFindCardMatchCards(List<Card> cards)
        {
            chkGameShowFindCardMatch1.Text = cards[0].Win ? "Ganador" : "";
            chkGameShowFindCardMatch2.Text = cards[1].Win ? "Ganador" : "";
            chkGameShowFindCardMatch3.Text = cards[2].Win ? "Ganador" : "";
            chkGameShowFindCardMatch4.Text = cards[3].Win ? "Ganador" : "";
            chkGameShowFindCardMatch5.Text = cards[4].Win ? "Ganador" : "";
            chkGameShowFindCardMatch6.Text = cards[5].Win ? "Ganador" : "";
            chkGameShowFindCardMatch7.Text = cards[6].Win ? "Ganador" : "";
            chkGameShowFindCardMatch8.Text = cards[7].Win ? "Ganador" : "";
            chkGameShowFindCardMatch9.Text = cards[8].Win ? "Ganador" : "";
        }

        private bool checkDataBasketScoreboard()
        {
            bool aux = false;

            aux = (cmbTemplateBasketScoreboard.Text.Length > 0);

            aux = (txtBasketScoreboardHomeTeam.Text.Length > 0);

            aux = (txtBasketScoreboardAwayTeam.Text.Length > 0);

            aux = (cmbBasktScoreboardQuarter.Text.Length > 0);

            return aux;
        }

        private void startBasketScoreboard()
        {
            if (checkDataBasketScoreboard())
            {
                if (AppController.getInstance().isConnectedToCasparCgServer())
                {
                    Template templateScoreboard = new Template();
                    //Uri logoPath = new Uri(AppController.getInstance().getFullMediaPath() + cmbFederationLogo.Text.ToLower() + ".png");

                    templateScoreboard.Fields.Add(new TemplateField("team1Name", txtBasketScoreboardHomeTeam.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team2Name", txtBasketScoreboardAwayTeam.Text));
                    templateScoreboard.Fields.Add(new TemplateField("team1Score", nudBasketScoreboardHomeScore.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("team2Score", nudBasketScoreboardAwayScore.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("gameTime", nudBasketScoreboardGameTimeMins.Value.ToString() + ":" + nudBasketScoreboardGameTimeSecs.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("shootTime", nudBasketScoreboardShootTime.Value.ToString()));
                    templateScoreboard.Fields.Add(new TemplateField("quarterNum", cmbBasktScoreboardQuarter.Text));
                    //templateScoreboard.Fields.Add(new TemplateField("logoScoreboard", logoPath.ToString()));

                    //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                    ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), cmbTemplateBasketScoreboard.Text, layerScoreboard.ToString()));
                    System.Diagnostics.Debug.WriteLine(ri.Message);
                }
            }
            else
            {
                MessageBox.Show("Faltan definir algunos datos para iniciar (template, local/visitante)", this.Text);
            }
        }

        private void stopBasketScoreboard()
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layerScoreboard.ToString()));
        }

        private void showHideBasketScoreboardClocks()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "clockShowHide", layerScoreboard.ToString()));
            }
        }

        private void showHideBasketScoreboard()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "scoreboardShowHide", layerScoreboard.ToString()));
            }
        }

        private void startBasketScoreboardClock()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layerScoreboard.ToString()));
                if (chkAutoShowOnClockStart.Checked)
                {
                    showHideBasketScoreboardClocks();
                }
            }
        }

        private void stopBasketScoreboardClock()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layerScoreboard.ToString()));
            }
        }

        private void resetBasketScoreboardClock()
        {
            Template templateUpdateScore = new Template();

            templateUpdateScore.Fields.Add(new TemplateField("gameTime", nudBasketScoreboardGameTimeMins.Value.ToString() + ":" + nudBasketScoreboardGameTimeSecs.Value.ToString()));

            //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
            ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
            System.Diagnostics.Debug.WriteLine(ri.Message);
        }

        //public void addOneScoreTeam1()
        //{
        //    nudHomeTeamScore.UpButton();
        //}

        //public void addOneScoreTeam2()
        //{
        //    nudGuestTeamScore.UpButton();
        //}

        private void addBasketScoreboardPoints(NumericUpDown control, int pointsToAdd)
        {
            control.Value += pointsToAdd;
        }

        private void updateBasketScoreboardTeamsScore()
        {
            if (AppController.getInstance().isConnectedToCasparCgServer())
            {
                Template templateUpdateScore = new Template();

                templateUpdateScore.Fields.Add(new TemplateField("team1Name", txtBasketScoreboardHomeTeam.Text));
                templateUpdateScore.Fields.Add(new TemplateField("team2Name", txtBasketScoreboardAwayTeam.Text));
                templateUpdateScore.Fields.Add(new TemplateField("team1Score", nudBasketScoreboardHomeScore.Value.ToString()));
                templateUpdateScore.Fields.Add(new TemplateField("team2Score", nudBasketScoreboardAwayScore.Value.ToString()));

                //string command = String.Format("CG 1 ADD 0 {0}{2}{0} 1 {0}{1}{0}", "\"", templateIntro.TemplateDataText(), cmbTemplatePresentacion.Text.ToString());
                ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateUpdateScore.TemplateDataText(), layerScoreboard.ToString()));
                System.Diagnostics.Debug.WriteLine(ri.Message);
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

        private void selectColor()
        {
            ColorDialog cdlg = new ColorDialog();
            if (cdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                picSolidColor.BackColor = cdlg.Color;
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
            HandballMatch.getInstance().teamPlayerFontSize = int.Parse(cmbPlayersFontSize.Text);
            HandballMatch.getInstance().teamPlayerLineSpacing = int.Parse(cmbPlayersFontLineSpacing.Text);
            HandballMatch.getInstance().teamPlayerLetterSpacing = int.Parse(cmbPlayersFontLetterSpacing.Text);
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
            HandballMatch.getInstance().scoreAutoHideOnClockFinished = chkAutoHideOnClockEnd.Checked;
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
            cmbPlayersFontSize.Text = HandballMatch.getInstance().teamPlayerFontSize.ToString();
            cmbPlayersFontLineSpacing.Text = HandballMatch.getInstance().teamPlayerLineSpacing.ToString();
            cmbPlayersFontLetterSpacing.Text = HandballMatch.getInstance().teamPlayerLetterSpacing.ToString();
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
            chkAutoHideOnClockEnd.Checked = HandballMatch.getInstance().scoreAutoHideOnClockFinished;
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
            FillGameshowQuestions(lvwGameshowQuestions, HandballMatch.getInstance().gameshowQuestions);
            
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

        private void newGameshowQuestion()
        {
            Pregunta q1 = new Pregunta(this, 1, (HandballMatch.getInstance().gameshowQuestions.Count + 1).ToString());
            q1.ShowDialog(this);
        }

        private void editGameshowQuestion()
        {
            if (lvwGameshowQuestions.SelectedItems != null)
            {
                Question aux = HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == int.Parse(lvwGameshowQuestions.SelectedItems[0].Text));
                Pregunta f1 = new Pregunta(this, 2, aux.id.ToString(), aux.question, aux.answers, aux.correctAnswer);
                f1.ShowDialog(this);
            }
        }

        private void deleteGameshowQuestion()
        {
            if (lvwGameshowQuestions.SelectedItems != null)
            {
                HandballMatch.getInstance().gameshowQuestions.Remove(HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == int.Parse(lvwGameshowQuestions.SelectedItems[0].Text)));
                FillGameshowQuestions(lvwGameshowQuestions, HandballMatch.getInstance().gameshowQuestions);
            }
        }

        private void loadGameshowQuestions()
        {
            mockingGameshowQuestions();
        }

        private void mockingGameshowQuestions()
        {
            HandballMatch.getInstance().gameshowQuestions.Clear();

            Question q = new Question();
            q.id = 1;
            q.question = "¿De que color era el caballo blanco de San Martin?";
            List<Answer> ans = new List<Answer>();
            ans.Add(new Answer("Marron"));
            ans.Add(new Answer("Colorado"));
            ans.Add(new Answer("Blanco"));
            ans.Add(new Answer("Negro"));
            q.answers = ans;
            q.correctAnswer = 3;

            addQuestion(q);
        }

        private void newGameshowFindCardItem()
        {
            frmFindCardItem q1 = new frmFindCardItem(this, 1, (HandballMatch.getInstance().gameshowFindCardItems.Count + 1).ToString());
            q1.ShowDialog(this);
        }

        private void editGameshowFindCardItem()
        {
            if (lvwGameShowFindCardItems.SelectedItems != null)
            {
                frmFindCardItem f1 = new frmFindCardItem(this, 2, lvwGameShowFindCardItems.SelectedItems[0].Text);
                f1.ShowDialog(this);
            }
        }

        private void deleteGameshowFindCardItem()
        {
            if (lvwGameShowFindCardItems.SelectedItems != null)
            {
                GameShowController.deleteGameshowFindCardItem(lvwGameShowFindCardItems.SelectedItems[0].Text);
                updateGameShowFindCardItems();
            }
        }

        public void updateGameShowFindCardItems()
        {
            fillGameShowFindCardItems();
        }

        private void fillGameShowFindCardItems()
        {
            SortPositionsTable();
            string[] arr;
            lvwGameShowFindCardItems.Items.Clear();
            lvwGameShowFindCardItems.Items.AddRange(GameShowController.fillListviewGameshowFindCardItems());
            //foreach (FindCardItem item in HandballMatch.getInstance().gameshowFindCardItems)
            //{
            //    arr = new string[3];
            //    arr[0] = item.id.ToString();
            //    arr[1] = item.type.ToString();
            //    arr[2] = item.picture.ToString();
            //    lvwGameShowFindCardItems.Items.Add(new ListViewItem(arr));
            //}
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

        private void FillGameshowQuestions(ListView lvw, List<Question> questions)
        {
            string[] arr;
            lvw.Items.Clear();
            clearQuestionInfo();
            int j = 1;
            foreach (Question item in questions)
            {
                arr = new string[2];
                arr[0] = item.id.ToString();
                arr[1] = item.question;
                lvw.Items.Add(new ListViewItem(arr));
                j++;
            }
        }

        private void getGameshowQuestion()
        {
            if (lvwGameshowQuestions.SelectedItems != null)
            {
                txtGameshowQuestion.Text = lvwGameshowQuestions.SelectedItems[0].SubItems[1].Text;
                Question aux = HandballMatch.getInstance().gameshowQuestions.Find(element => element.id == int.Parse(lvwGameshowQuestions.SelectedItems[0].Text));
                cmbGameshowCorrectAnswer.Text = aux.correctAnswer.ToString();
                FillGameshowAnswers(lvwGameshowQuestionAnswers, aux.answers);
            }
        }

        private void FillGameshowAnswers(ListView lvw, List<Answer> answers)
        {
            string[] arr;
            lvw.Items.Clear();
            int j = 1;
            foreach (Answer item in answers)
            {
                arr = new string[2];
                arr[0] = j.ToString();
                arr[1] = item.answer;
                lvw.Items.Add(new ListViewItem(arr));
                j++;
            }
        }

        # region eventos
        private void mnuSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConexion_Click(object sender, EventArgs e)
        {
            connectToCasparCGServer();
        }

        private void btnUpdateTemplates_Click(object sender, EventArgs e)
        {
            getServerTemplates();
        }

        private void btnClearChannel_Click(object sender, EventArgs e)
        {
            clearChannel();
        }

        private void btnLockUnlock_Click(object sender, EventArgs e)
        {
            lockUnlockTemplates();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    break;
                case Keys.F10:
                    this.tabControl1.SelectedIndex = 8;
                    break;
                case Keys.F11:
                    this.tabControl1.SelectedIndex = 9;
                    break;
                case Keys.F12:
                    this.tabControl1.SelectedIndex = 10;
                    break;
                case Keys.F13:
                    break;
                case Keys.F14:
                    break;
                case Keys.F15:
                    break;
                case Keys.F16:
                    break;
                case Keys.F17:
                    break;
                case Keys.F18:
                    break;
                case Keys.F19:
                    break;
                case Keys.F2:
                    this.tabControl1.SelectedIndex = 0;
                    break;
                case Keys.F20:
                    break;
                case Keys.F21:
                    break;
                case Keys.F22:
                    break;
                case Keys.F23:
                    break;
                case Keys.F24:
                    break;
                case Keys.F3:
                    this.tabControl1.SelectedIndex = 1;
                    break;
                case Keys.F4:
                    this.tabControl1.SelectedIndex = 2;
                    break;
                case Keys.F5:
                    this.tabControl1.SelectedIndex = 3;
                    break;
                case Keys.F6:
                    this.tabControl1.SelectedIndex = 4;
                    break;
                case Keys.F7:
                    this.tabControl1.SelectedIndex = 5;
                    break;
                case Keys.F8:
                    this.tabControl1.SelectedIndex = 6;
                    break;
                case Keys.F9:
                    this.tabControl1.SelectedIndex = 7;
                    break;
                case Keys.Help:
                    break;
                default:
                    break;
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

        private void radVolleyHomeServe_CheckedChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void radVolleyGuestServe_CheckedChanged(object sender, EventArgs e)
        {
            watchVolleyGame();
        }

        private void btnShowCountdown_Click(object sender, EventArgs e)
        {
            showCountdown();
        }

        private void btnHideCountdown_Click(object sender, EventArgs e)
        {
            hideCountdown();
        }

        private void btnStartCountdown_Click(object sender, EventArgs e)
        {
            startCountdown();
        }

        private void btnStopCountdown_Click(object sender, EventArgs e)
        {
            stopCountdown();
        }

        private void btnStartDynamicLogo_Click(object sender, EventArgs e)
        {
            startDynamicLogo();
        }

        private void btnStopDynamicLogo_Click(object sender, EventArgs e)
        {
            stopDynamicLogo();
        }

        private void btnShowGameshowQuestions_Click(object sender, EventArgs e)
        {
            startGameshowQuestions();
        }

        private void btnHideGameshowQuestions_Click(object sender, EventArgs e)
        {
            stopGameshowQuestions();
        }

        private void btnShowPlayerAnswer_Click(object sender, EventArgs e)
        {
            showGameshowPlayerAnswer();
        }

        private void btnShowCorrectAnswer_Click(object sender, EventArgs e)
        {
            showGameshowCorrectAnswer();
        }

        private void btnLoadGameshowQuestions_Click(object sender, EventArgs e)
        {
            fillListQuestions();
        }

        private void lvwGameshowQuestions_DoubleClick(object sender, EventArgs e)
        {
            getGameshowQuestion();
        }

        private void btnStartTop3_Click(object sender, EventArgs e)
        {
            startTop3();
        }

        private void btnStopTop3_Click(object sender, EventArgs e)
        {
            stopTop3();
        }

        private void btnAddGameshowQuestion_Click(object sender, EventArgs e)
        {
            newGameshowQuestion();
        }

        private void btnEditGameshowQuestion_Click(object sender, EventArgs e)
        {
            editGameshowQuestion();
        }

        private void btnRemoveGameshowQuestion_Click(object sender, EventArgs e)
        {
            deleteGameshowQuestion();
        }

        private void btnWeatherForecastCallWS_Click(object sender, EventArgs e)
        {
            getWeatherForecastStates();
        }

        private void cmbWeatherStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            getWeatherForecastStateCities();
        }

        private void btnElectionsTop3CallWS_Click(object sender, EventArgs e)
        {
            fillElectionsResults();
        }

        private void btnStartWeatherForecast_Click(object sender, EventArgs e)
        {
            startWeatherForecast();
        }

        private void btnStopWeatherForecast_Click(object sender, EventArgs e)
        {
            stopWeatherForecast();
        }

        private void lvwCities_DoubleClick(object sender, EventArgs e)
        {
            getWeatherForecastCityForecast();
        }

        private void lvwVideoFiles_DoubleClick(object sender, EventArgs e)
        {
            playMedia();
        }

        private void btnTwitterWSCall_Click(object sender, EventArgs e)
        {
            fillTwitterList();
        }

        private void lvwTwitterList_DoubleClick(object sender, EventArgs e)
        {
            selectTweet();
        }

        private void btnStartTwitter_Click(object sender, EventArgs e)
        {
            startTwitter();
        }

        private void btnStopTwitter_Click(object sender, EventArgs e)
        {
            stopTwitter();
        }

        private void btnGameplayPlay_Click(object sender, EventArgs e)
        {
            playGameplayMedia();
        }

        private void btnGameplayStop_Click(object sender, EventArgs e)
        {
            stopGameplayMedia();
        }

        private void btnStartTwitterCounter_Click(object sender, EventArgs e)
        {
            startTwitterCounter();
        }

        private void btnStopTwitterCounter_Click(object sender, EventArgs e)
        {
            stopTwitterCounter();
        }

        private void btnUpdateTwitterCounter_Click(object sender, EventArgs e)
        {
            updateTwitterCounter();
        }

        private void nudTwitterCounter_ValueChanged(object sender, EventArgs e)
        {
            if (chkTwitterCounterAutoupdate.Checked)
            {
                updateTwitterCounter();
            }
        }

        private void chkTwitterCounterPosition_Click(object sender, EventArgs e)
        {
            enableTwitterCounterPosition();
        }

        private void btnStartDynamicInfo_Click(object sender, EventArgs e)
        {
            startDynamicInfo();
        }

        private void btnStopDynamicInfo_Click(object sender, EventArgs e)
        {
            stopDynamicInfo();
        }

        private void btnShowHideIntro_Click(object sender, EventArgs e)
        {
            showHideIntro();
        }

        private void btnShowHideHomeTeam_Click(object sender, EventArgs e)
        {
            showHideTeamsheet(btnShowHideHomeTeam, txtHomeTeamName, lvwHomeTeamPlayers, txtHomeTeamCoach, cmbHomeTeamLogo, chkAutoHideHomeTeam, nudHideHomeTeamSeconds, tmrTeam1);
        }

        private void btnShowHideGuestTeam_Click(object sender, EventArgs e)
        {
            showHideTeamsheet(btnShowHideGuestTeam, txtGuestTeamName, lvwGuestTeamPlayers, txtGuestTeamPlayers, cmbGuestTeamLogo, chkAutoHideGuestTeam, nudHideGuestTeamSeconds, tmrTeam2);
        }

        private void btnShowHomeAndGuestTeam_Click(object sender, EventArgs e)
        {
            autoShowHideTeams();
        }

        private void btnStartLT_Click(object sender, EventArgs e)
        {
            startLowerThird();
        }

        private void btnStopLT_Click(object sender, EventArgs e)
        {
            stopLowerThird();
        }

        private void btnStartScoreboard_Click(object sender, EventArgs e)
        {
            startScoreboard();
        }

        private void btnShowHideScoreboard_Click(object sender, EventArgs e)
        {
            showHideScoreboard();
        }

        private void btnStopScoreboard_Click(object sender, EventArgs e)
        {
            stopScoreboard();
        }

        private void btnStartClock_Click(object sender, EventArgs e)
        {
            startScoreboardClock();
        }

        private void btnStopClock_Click(object sender, EventArgs e)
        {
            startScoreboardClock();
        }

        private void btnResetClock_Click(object sender, EventArgs e)
        {
            resetScoreboardClock();
        }

        private void btnShowHideResult_Click(object sender, EventArgs e)
        {
            showHideResult();
        }

        private void btnAddOneToHomeTeamScore_Click(object sender, EventArgs e)
        {
            addOneScoreTeam1();
        }

        private void btnAddOneToGuestTeamScore_Click(object sender, EventArgs e)
        {
            addOneScoreTeam2();
        }

        private void btnClearHomeTeamPlayers_Click(object sender, EventArgs e)
        {
            clearTeam1Players();
        }

        private void btnClearGuestTeamPlayers_Click(object sender, EventArgs e)
        {
            clearTeam2Players();
        }

        private void btnAddHomeTeamPlayer_Click(object sender, EventArgs e)
        {
            addPlayerTeam1();
        }

        private void btnAddGuestTeamPlayer_Click(object sender, EventArgs e)
        {
            addPlayerTeam2();
        }

        private void btnEditHomeTeamPlayer_Click(object sender, EventArgs e)
        {
            editPlayerTeam1();
        }

        private void btnEditGuestTeamPlayer_Click(object sender, EventArgs e)
        {
            editPlayerTeam2();
        }

        private void btnLoadHomeTeamPlayers_Click(object sender, EventArgs e)
        {
            loadPlayersTeam1();
        }

        private void btnRemoveHomeTeamPlayer_Click(object sender, EventArgs e)
        {
            removePlayerTeam1();
        }

        private void btnRemoveGuestTeamPlayer_Click(object sender, EventArgs e)
        {
            removePlayerTeam2();
        }

        private void radHomeTeamPlayers_CheckedChanged(object sender, EventArgs e)
        {
            loadPlayerLT();
        }

        private void radGuestTeamPlayers_CheckedChanged(object sender, EventArgs e)
        {
            loadPlayerLT();
        }

        private void lvwTeamPlayers_DoubleClick(object sender, EventArgs e)
        {
            getPlayerInfoToLT();
        }

        private void btnDynamicInfoWSCall_Click(object sender, EventArgs e)
        {
            retrieveDynamicInfoFromWS();
        }

        private void lvwDynamicInfoList_DoubleClick(object sender, EventArgs e)
        {
            selectDynamicInfo();
        }

        private void btnTwitterCounterWSCall_Click(object sender, EventArgs e)
        {
            fillTwitterCounter();
        }

        private void btnAnimatedLogoStart_Click(object sender, EventArgs e)
        {
            showAnimatedLogo();
        }

        private void btnAnimatedLogoStop_Click(object sender, EventArgs e)
        {
            hideAnimatedLogo();
        }

        private void btnAnimatedLogoOut_Click(object sender, EventArgs e)
        {
            startAnimatedLogoOut();
        }

        private void btnAnimatedLogoIn_Click(object sender, EventArgs e)
        {
            startAnimatedLogoIn();
        }

        private void chkTwitterCounterAutoWSCall_CheckedChanged(object sender, EventArgs e)
        {
            tmrTwitterCounterWSCall.Interval = (int) (nudTwitterCounterAutoWSCallSeg.Value*1000);
            if (chkTwitterCounterAutoWSCall.Checked)
	        {
                tmrTwitterCounterWSCall.Start();		 
	        }
            else
	        {
                tmrTwitterCounterWSCall.Stop();    
	        }
        }

        private void nudTwitterCounterAutoWSCallSeg_ValueChanged(object sender, EventArgs e)
        {
            tmrTwitterCounterWSCall.Interval = (int)(nudTwitterCounterAutoWSCallSeg.Value * 1000);
        }

        private void tmrTwitterCounterWSCall_Tick(object sender, EventArgs e)
        {
            fillTwitterCounter();
        }

        private void btnTwitterCounterStartWS_Click(object sender, EventArgs e)
        {
            callTwitterCounterWS("start");
        }

        private void btnTwitterCounterStopWS_Click(object sender, EventArgs e)
        {
            callTwitterCounterWS("stop");
        }

        private void btnTwitterCounterWSClear_Click(object sender, EventArgs e)
        {
            callTwitterCounterWS("clear");
        }

        private void btnTwitterCounterWSStatus_Click(object sender, EventArgs e)
        {
            callStatusTwitterCounterWS();
        }

        private void btnDynamicNewsTickerShow_Click(object sender, EventArgs e)
        {
            showDynamicNewsTicker();
        }

        private void btnDynamicNewsTickerHide_Click(object sender, EventArgs e)
        {
            hideDynamicNewsTicker();
        }

        private void btnDynamicNewsTickerWSCall_Click(object sender, EventArgs e)
        {
            getDynamicNewsCategories();
        }

        private void cmbDynamicNewsTickerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDynamicNewsWS();
        }

        private void picGameplayWhiteboard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                gameplayMouseDown = true;
                showGameplayPointerLocation("DOWN", e.Location);
            }
        }

        private void picGameplayWhiteboard_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameplayMouseDown)
            {
                showGameplayPointerLocation("MOVE", e.Location);
            }
        }

        private void showGameplayPointerLocation(String mouseEvent, Point mouseLocation)
        {
            int posX, posY;
            double porcX, porcY;

            posX = mouseLocation.X;
            posY = mouseLocation.Y;

            if (posX <= picGameplayWhiteboard.Width && posY <= picGameplayWhiteboard.Height)
            {
                porcX = Math.Round((posX / (double)picGameplayWhiteboard.Width), 2);
                porcY = Math.Round((posY / (double)picGameplayWhiteboard.Height), 2);
                lblGameplayPointerLocation.Text = "X: " + posX + " Y: " + posY + "\n" + "%X : " + porcX + " %Y: " + porcY;
                updateGameplay(mouseEvent, porcX, porcY);
            }
        }

        private void picGameplayWhiteboard_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                gameplayMouseDown = false;
                showGameplayPointerLocation("UP", e.Location);
            }
        }

        private void btnStartGameplay_Click(object sender, EventArgs e)
        {
            startGameplay();
        }

        private void btnStopGameplay_Click(object sender, EventArgs e)
        {
            stopGameplay();
        }

        private void nudTwitterPollWSAutoCallSeg_ValueChanged(object sender, EventArgs e)
        {
            tmrTwitterPollWSCall.Interval = (int)(nudTwitterPollWSAutoCallSeg.Value * 1000);
        }

        private void chkTwitterPollWSAutoCall_CheckedChanged(object sender, EventArgs e)
        {
            tmrTwitterPollWSCall.Interval = (int)(nudTwitterPollWSAutoCallSeg.Value * 1000);
            if (chkTwitterPollWSAutoCall.Checked)
            {
                tmrTwitterPollWSCall.Start();
            }
            else
            {
                tmrTwitterPollWSCall.Stop();
            }
        }

        private void tmrTwitterPollWSCall_Tick(object sender, EventArgs e)
        {
            fillTwitterPoll();
        }

        private void btnStartTwitterPoll_Click(object sender, EventArgs e)
        {
            startTwitterPoll();
        }

        private void btnUpdateTwitterPoll_Click(object sender, EventArgs e)
        {
            updateTwitterPoll();
        }

        private void btnStopTwitterPoll_Click(object sender, EventArgs e)
        {
            stopTwitterPoll();
        }

        private void btnTwitterPollHashtagsAdd_Click(object sender, EventArgs e)
        {
            mockHashtagTwitterPoll();
        }

        private void mockHashtagTwitterPoll()
        {
            string[] arr;
            lvwTwitterPollHashtags.Items.Clear();

            arr = new string[4];
            arr[0] = "1";
            arr[1] = "#PrimerHashtag";
            arr[2] = "0";
            arr[3] = "0";
            lvwTwitterPollHashtags.Items.Add(new ListViewItem(arr));

            arr = new string[4];
            arr[0] = "2";
            arr[1] = "#SegundoHashtag";
            arr[2] = "0";
            arr[3] = "0";
            lvwTwitterPollHashtags.Items.Add(new ListViewItem(arr));

            arr = new string[4];
            arr[0] = "3";
            arr[1] = "#TercerHashtag";
            arr[2] = "0";
            arr[3] = "0";
            lvwTwitterPollHashtags.Items.Add(new ListViewItem(arr));
        }

        private void clearHashtagsTwitterPoll()
        {
            lvwTwitterPollHashtags.Items.Clear();
        }

        private void btnTwitterPollWSCall_Click(object sender, EventArgs e)
        {
            fillTwitterPoll();
        }

        private void btnTwitterPollHashtagsClear_Click(object sender, EventArgs e)
        {
            clearHashtagsTwitterPoll();
        }

        private void btnPencil_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPencil.Checked)
            {
                callGameplayTool("Pencil");
            }
        }

        private void btnToolbarGameplayClear_CheckedChanged(object sender, EventArgs e)
        {
            if (btnToolbarGameplayClear.Checked)
            {
                callGameplayTool("Clear");
            }
        }

        private void lstSports_Click(object sender, EventArgs e)
        {
            selectSportTab();
        }

        private void cmbGameplayImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            setImageAsCanvas();
        }

        private void btnGameplayRefresh_Click(object sender, EventArgs e)
        {
            fillImagesCombo();
        }

        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            connectToCasparCGServer();
        }

        private void btnTwitterLoadTweets_Click(object sender, EventArgs e)
        {
            loadTwitterList();
        }

        private void btnDynamicInfoLoadInfo_Click(object sender, EventArgs e)
        {
            loadDynamicInfoList();
        }

        private void btnSelectSolidColor_Click(object sender, EventArgs e)
        {
            selectColor();
        }

        private void picSolidColor_BackColorChanged(object sender, EventArgs e)
        {
            lblSolidColorHtml.Text = ColorTranslator.ToHtml(Color.FromArgb(Color.Tomato.ToArgb()));
        }

        private void picSolidColor_DoubleClick(object sender, EventArgs e)
        {
            selectColor();
        }

        private void lvwGameplayPlaylist_DoubleClick(object sender, EventArgs e)
        {
            playGameplayMedia();
        }

        private void btnGameplayPause_Click(object sender, EventArgs e)
        {
            pauseGameplayMedia();
        }

        private void btnBasketScoreboardStart_Click(object sender, EventArgs e)
        {
            startBasketScoreboard();
        }

        private void btnBasketScoreboardStop_Click(object sender, EventArgs e)
        {
            stopBasketScoreboard();
        }

        private void btnBasketScoreboardStartGameTime_Click(object sender, EventArgs e)
        {
            startBasketScoreboardClock();
        }

        private void btnBasketScoreboardStopGameTime_Click(object sender, EventArgs e)
        {
            stopBasketScoreboardClock();
        }

        private void btnBasketScoreboardResetGameTime_Click(object sender, EventArgs e)
        {
            resetBasketScoreboardClock();
        }

        private void btnBasketScoreboardShowHideClocks_Click(object sender, EventArgs e)
        {
            showHideBasketScoreboardClocks();
        }

        private void nudBasketScoreboardHomeScore_ValueChanged(object sender, EventArgs e)
        {
            updateBasketScoreboardTeamsScore();
        }

        private void nudBasketScoreboardAwayScore_ValueChanged(object sender, EventArgs e)
        {
            updateBasketScoreboardTeamsScore();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGameShowFindCardSaveMatch_Click(object sender, EventArgs e)
        {
            generateGameShowFindCardsMatches();
        }

        private void cmbGameShowFindCardMatches_Click(object sender, EventArgs e)
        {
            fillSelectedFindCardMatch();
        }

        private void btnBasketScoreboardShowHideScoreboard_Click(object sender, EventArgs e)
        {
            showHideBasketScoreboard();
        }

        private void btnBasketScoreboardHome1PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardHomeScore, 1);
        }

        private void btnBasketScoreboardAway1PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardAwayScore, 1);
        }

        private void btnBasketScoreboardHome2PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardHomeScore, 2);
        }

        private void btnBasketScoreboardAway2PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardAwayScore, 2);
        }

        private void btnBasketScoreboardHome3PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardHomeScore, 3);
        }

        private void btnBasketScoreboardAway3PAdd_Click(object sender, EventArgs e)
        {
            addBasketScoreboardPoints(nudBasketScoreboardAwayScore, 3);
        }

        private void btnGameShowFindCardAddItem_Click(object sender, EventArgs e)
        {
            newGameshowFindCardItem();
        }

        private void btnGameShowFindCardRemoveItem_Click(object sender, EventArgs e)
        {
            deleteGameshowFindCardItem();
        }

        private void btnGameShowFindCardEditItem_Click(object sender, EventArgs e)
        {
            editGameshowFindCardItem();
        }

        private void btnGameShowFindCardShow_Click(object sender, EventArgs e)
        {
            startGameshowFindCard();
        }

        private void btnGameShowFindCardHide_Click(object sender, EventArgs e)
        {
            stopGameshowFindCard();
        }

        private void chkGameShowFindCardMatch1_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(1);
        }

        private void chkGameShowFindCardMatch2_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(2);
        }

        private void chkGameShowFindCardMatch3_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(3);
        }

        private void chkGameShowFindCardMatch4_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(4);
        }

        private void chkGameShowFindCardMatch5_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(5);
        }

        private void chkGameShowFindCardMatch6_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(6);
        }

        private void chkGameShowFindCardMatch7_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(7);
        }

        private void chkGameShowFindCardMatch8_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(8);
        }

        private void chkGameShowFindCardMatch9_CheckedChanged(object sender, EventArgs e)
        {
            updateGameshowFindCard(9);
        }

        private void btnGameShowFindCardShowAll_Click(object sender, EventArgs e)
        {
            showGameshowFindCardShowAllCards();
        }

        private void btnGameShowFindCardHideAll_Click(object sender, EventArgs e)
        {
            showGameshowFindCardHideAllCards();
        }

        private void btnTwitterSearchWSCall_Click(object sender, EventArgs e)
        {
            loadTwitterSearchList();
        }

        private void btnTwitterPlaylistWSCall_Click(object sender, EventArgs e)
        {
            loadTwitterPlaylistList();
        }

        private void lvwTwitterPlaylistResult_DoubleClick(object sender, EventArgs e)
        {
            selectPlaylist();
        }

        private void btnTwitterPlaylistStart_Click(object sender, EventArgs e)
        {
            startTwitterPlaylist();
        }

        private void btnTwitterPlaylistStop_Click(object sender, EventArgs e)
        {
            stopTwitterPlaylist();
        }

        private void btnTwitterSearchAddToPlaylist_Click(object sender, EventArgs e)
        {
            addTwitterSearchToPlaylist();
        }

        private void TwitterSearchRemoveFromPlaylist_Click(object sender, EventArgs e)
        {
            removeTwitterSearchFromPlaylist();
        }

        private void btnTwitterSearchClearPlaylist_Click(object sender, EventArgs e)
        {
            clearTwitterSearchPlaylist();
        }

        private void btnTwitterSearchSavePlaylist_Click(object sender, EventArgs e)
        {
            saveTwitterSearchPlaylist();
        }

        private void nudGuestTeamScore_ValueChanged(object sender, EventArgs e)
        {
            updateTeamsScore();
        }

        private void nudHomeTeamScore_ValueChanged(object sender, EventArgs e)
        {
            updateTeamsScore();
        }

        private void btnStartStopRugbyIntro_Click(object sender, EventArgs e)
        {
            showHideRugbyIntro();
        }

        # endregion

        private void btnStartStopRugbyLineup_Click(object sender, EventArgs e)
        {
            showHideRugbyLineup();
        }

        private void btnStartStopRugbyResult_Click(object sender, EventArgs e)
        {
            showHideRugbyResult();
        }

        private void btnStartStopRugbyOfficials_Click(object sender, EventArgs e)
        {
            showHideRugbyOfficials();
        }
    }
}
