using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class SportsController
    {
        private static String[] sportList = {"General", "Volleyball", "Boxeo", "Basket", "Rugby"};

        public static Object[] getSportList()
        {
            return sportList;
        }

        private static Template buildTemplateObject(Dictionary<string, string> fieldAndValues)
        {
            Template templateResult = new Template();

            foreach (var item in fieldAndValues)
            {
                templateResult.Fields.Add(new TemplateField(item.Key, item.Value));
            }

            return templateResult;

        }

        public static void startResult(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templateResult = buildTemplateObject(fieldsAndValues);

            AppController.getInstance().cgAdd(templateName, layer, templateResult);
        }

        public static void stopResult(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startTemplatePositions(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templatePositions = buildTemplateObject(fieldsAndValues);

            AppController.getInstance().cgAdd(templateName, layer, templatePositions);
        }

        public static void stopTemplatePositions(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startScoreboard(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templateScoreboard = buildTemplateObject(fieldsAndValues);

            AppController.getInstance().cgAdd(templateName, layer, templateScoreboard);
        }

        public static void stopScoreboard(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void showHideScoreboard(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "clockShowHide", layer.ToString()));
        }

        public static void startScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void stopScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void resetScoreboardClock(int layer, Dictionary<string, string> fieldsToUpdate)
        {
            Template templateScoreboard = buildTemplateObject(fieldsToUpdate);

            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), layer.ToString()));
        }

        public static void updateScoreboardScores(int layer, Dictionary<string, string> fieldsToUpdate)
        {
            Template templateScoreboard = buildTemplateObject(fieldsToUpdate);

            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), layer.ToString()));
        }

        public static void startTeamsheet(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templateTeamsheet = buildTemplateObject(fieldsAndValues);

            AppController.getInstance().cgAdd(templateName, layer, templateTeamsheet);
        }

        public static void stopTeamsheet(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyIntro(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"infoLeague", "2018 super rugby"},{"team1Name","crusaders"},{"team2Name","lions"},
                {"infoDate","final"},{"infoLocation","ami stadium, christchurch"},
                {"team1Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"team2Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"team1Color","ffffff"},{"team2Color","ff0000"},
                {"team1TitleDark","true"},{"team2TitleDark","false"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            AppController.getInstance().cgAdd(templateName, layer, templateRugbyIntro);
        }

        public static void stopRugbyIntro(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }


        public static void startRugbyLineup()
        {
            //string af = "\\" + Microsoft.VisualBasic.Strings.ChrW(0x22);

            //String xml = "<templateData>";
            //xml += "<componentData id=" + af + "sideTitle" + af + ">";
            //xml += "<data id=" + af + "text" + af + " value=" + af + cmbDynamicNewsTickerCategory.Text + af + " />";
            //xml += "</componentData>";
            //xml += "<componentData id=" + af + "crawl" + af + ">";
            //xml += "<action type=" + af + "set" + af + ">";

            //foreach (ListViewItem item in lvwDynamicNewsTicker.Items)
            //{
            //    xml += "<data id=" + af + "text" + af + " value=" + af + item.SubItems[2].Text + af + " />";
            //}

            //xml += "</action>";
            //xml += "</componentData>";
            //xml += "</templateData>";

        }

        public static void stopRugbyLineup(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyLowerthird(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"title", "12. RYAN CROTTY"},{"subtitle","10 Minutes in the sin bin"},
                {"teamLogo","file:///C:/CasparCG Server 2.0.6/Server/media/Logos-Futbol/logoRacing-PNG.png"},
                {"teamColor","ff0000"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            AppController.getInstance().cgAdd(templateName, layer, templateRugbyIntro);
        }

        public static void stopRugbyLowerthird(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyOfficials(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"title", "MATCH OFFICIALS"},{"roles","REF;AR1;AR2;TMO"},
                {"names","angus gardner;glen jackson;nic berry;shane mcdermott"},
                {"leagueLogo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/logo-rojo-blanco.png"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            AppController.getInstance().cgAdd(templateName, layer, templateRugbyIntro);
        }

        public static void stopRugbyOfficials(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyResult(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"team1Name", "Crusaders"},{"team2Name","Lions"},
                {"team1Score","3"},{"team2Score","12"},
                {"team1Info",""},{"team2Info","Penalty goal - jantjie 13'"},{"gameInfo",""},
                {"team1Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"team2Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"team1Color","ffffff"},{"team2Color","ff0000"},
                {"team1TitleDark","true"},{"team2TitleDark","false"},
                {"timeResult","Primer Tiempo"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            AppController.getInstance().cgAdd(templateName, layer, templateRugbyIntro);
        }

        public static void stopRugbyResult(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyScoreboard(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"team1Name", "CRU"},{"team2Name","LIO"},
                {"team1Score","23"},{"team2Score","13"},
                {"team1Info",""},{"team2Info","Penalty goal - jantjie 13'"},{"gameTime","79:40"},{"halfNum","2"},
                {"team1Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"team2Logo","file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/VMH-Logo.png"},
                {"acumulatedTime","0"},{"startDateTime",""},{"team1YellowCards","1"},{"team2YellowCards","0"},
                {"team1Color","ffffff"},{"team2Color","ff0000"},
                {"team1TitleDark","true"},{"team2TitleDark","false"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            AppController.getInstance().cgAdd(templateName, layer, templateRugbyIntro);
        }

        public static void stopRugbyScoreboard(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startRugbyScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void stopRugbyScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \"{0}\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void updateRugbyScoreboardScores(int layer, Dictionary<string, string> fieldsToUpdate)
        {
            Dictionary<string, string> introData = new Dictionary<string, string>()
            {
                {"team1Score","26"},{"team2Score","13"}
            };

            Template templateRugbyIntro = buildTemplateObject(introData);

            Template templateScoreboard = buildTemplateObject(fieldsToUpdate);

            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{2} UPDATE 0 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), layer.ToString()));
        }

    }
}
