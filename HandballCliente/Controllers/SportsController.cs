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

            ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateResult.TemplateDataText(), templateName, layer.ToString()));
            System.Diagnostics.Debug.WriteLine(ri.Message);
        }

        public static void stopResult(int layer)
        {
            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{0} STOP 0", layer.ToString()));
        }

        public static void startTemplatePositions(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templatePositions = buildTemplateObject(fieldsAndValues);

            ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templatePositions.TemplateDataText(), templateName, layer.ToString()));
        }

        public static void stopTemplatePositions(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }

        public static void startScoreboard(string templateName, int layer, Dictionary<string, string> fieldsAndValues)
        {
            Template templateScoreboard = buildTemplateObject(fieldsAndValues);

            ReturnInfo ri = AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateScoreboard.TemplateDataText(), templateName, layer.ToString()));
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

            AppController.getInstance().executeCasparCgServer(String.Format("CG 1-{3} ADD 0 {0}{2}{0} 1 {0}{1}{0}", (char)0x22, templateTeamsheet.TemplateDataText(), templateName, layer.ToString()));
        }

        public static void stopTeamsheet(int layer)
        {
            AppController.getInstance().cgStop(layer);
        }
    }
}
