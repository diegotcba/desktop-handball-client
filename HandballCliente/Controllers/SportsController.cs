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
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \\\"{0}\\\"", "clockShowHide", layer.ToString()));
        }

        public static void startScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \\\"{0}\\\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void stopScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \\\"{0}\\\"", "gameTimeStartStop", layer.ToString()));
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


        public static void startRugbyLineup(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {

            String xml = "<templateData><componentData id=\\\"teamName\\\"><data id=\\\"text\\\" value=\\\"Tala Rugby Club\\\"/></componentData><componentData id=\\\"headCoachName\\\"><data id=\\\"text\\\" value=\\\"Julio Garcia\\\"/></componentData><componentData id=\\\"leagueLogo\\\"><data id=\\\"text\\\" value=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/logo-rojo-blanco.png\\\"/></componentData><componentData id=\\\"teamLogo\\\"><data id=\\\"text\\\" value=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/logo-rojo-blanco.png\\\"/></componentData><componentData id=\\\"player-1\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-2\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-3\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-4\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-5\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-6\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-7\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-8\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-9\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-10\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-11\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-12\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-13\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-14\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-15\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData></templateData>"
            + "<componentData id=\\\"player-1\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-2\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-3\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-4\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-5\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-6\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-7\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-8\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-9\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-10\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-11\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-12\\\"><data id=\\\"text\\\" value=\\\"Escuti\\\" number=\\\"2\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-13\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData><componentData id=\\\"player-14\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P02.png\\\" /></componentData><componentData id=\\\"player-15\\\"><data id=\\\"text\\\" value=\\\"Brada\\\" number=\\\"1\\\" photoPath=\\\"file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/P01.png\\\" /></componentData>"
            + "</templateData>";

            //xml = xml.Replace("\\\\\"", "\\\\" + Microsoft.VisualBasic.Strings.ChrW(0x22));

            System.Diagnostics.Debug.WriteLine(xml);

            xml = getRugbyLineupTemplateDataText(createLineup());
            System.Diagnostics.Debug.WriteLine(xml);

            AppController.getInstance().cgAdd(templateName, 30, xml);
        }

        private static Dictionary<string, Tuple<string, string, string>> createLineup()
        {
            //Uri logo1Path = new Uri("logo.png");
            //Uri logo2Path = new Uri("logo-rojo-blanco.png.png");

            String baseUrl = "file:///C:/CasparCG Server 2.0.6/Server/templates/Rugby/";

            Dictionary<string, Tuple<string, string, string>> templateResultValues = new Dictionary<string, Tuple<string, string, string>>();

            templateResultValues = new Dictionary<string, Tuple<string, string, string>>(){
                {"teamName",Tuple.Create("Tala Rugby Club","","")},{"headCoachName",Tuple.Create("Julio Garcia","","")},
                {"leagueLogo",Tuple.Create(baseUrl + "logo-rojo-blanco.png","","")},
                {"teamLogo",Tuple.Create(baseUrl + "VMH-Logo.png","","")},
                {"player-1",Tuple.Create("Brarda","1",baseUrl + "P01.png")},
                {"player-2",Tuple.Create("Escuti","2",baseUrl + "P02.png")},
                {"player-3",Tuple.Create("Cetti","3",baseUrl + "P03.png")},
                {"player-4",Tuple.Create("Lobato","4",baseUrl + "P04.png")},
                {"player-5",Tuple.Create("Vergara","5",baseUrl + "P01.png")},
                {"player-6",Tuple.Create("Marioli","6",baseUrl + "P02.png")},
                {"player-7",Tuple.Create("Basile","7",baseUrl + "P03.png")},
                {"player-8",Tuple.Create("Simondi","8",baseUrl + "P04.png")},
                {"player-9",Tuple.Create("Casutti","9",baseUrl + "P01.png")},
                {"player-10",Tuple.Create("Nacassian","10",baseUrl + "P02.png")},
                {"player-11",Tuple.Create("Cuaranta","11",baseUrl + "P03.png")},
                {"player-12",Tuple.Create("Ambrosio","20",baseUrl + "P04.png")},
                {"player-13",Tuple.Create("Cantarutti","13",baseUrl + "P02.png")},
                {"player-14",Tuple.Create("Schulz","14",baseUrl + "P03.png")},
                {"player-15",Tuple.Create("Ruiz","15",baseUrl + "P04.png")}
            };

            return templateResultValues;
        }

        private static String getRugbyLineupTemplateDataText(Dictionary<string, Tuple<string, string, string>> lineup)
        {
            string af = "\\" + (char)0x22;
            StringBuilder sb = new StringBuilder();

            sb.Append("<templateData>");

            foreach (KeyValuePair<string, Tuple<string, string, string>> tf in lineup)
            {
                sb.AppendFormat("<componentData id={0}{1}{0}>", af, tf.Key);
                sb.AppendFormat("<data id={0}text{0} value={0}{1}{0} number={0}{2}{0} photoPath={0}{3}{0} />", af, tf.Value.Item1, tf.Value.Item2, tf.Value.Item3);
                sb.Append("</componentData>");
            }

            sb.Append("</templateData>");

            return sb.ToString();
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
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \\\"{0}\\\"", "gameTimeStartStop", layer.ToString()));
        }

        public static void stopRugbyScoreboardClock(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{1} INVOKE 0 \\\"{0}\\\"", "gameTimeStartStop", layer.ToString()));
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
