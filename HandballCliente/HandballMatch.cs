using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Xml;
using System.Xml.Serialization;
using System.Data;


namespace HandballCliente
{
    [Serializable()]
    public class HandballMatch
    {
        private static HandballMatch instance = null;
        public String name { get; set; }
        public String description { get; set; }
        public DateTime fecha { get; set; }

        public String serverAddress { get; set; }
        public String serverPort { get; set; }

        public int recordingCRF { get; set; }
        public String team1Name { get; set; }
        public String team2Name { get; set; }
        public String eventTitle { get; set; }
        public String leagueName { get; set; }
        public String location { get; set; }
        public String team1ScoreName { get; set; }
        public String team2ScoreName { get; set; }
        public String templatePresentation { get; set; }
        public String templateTeam { get; set; }
        public String templateScoreboard { get; set; }
        public String templateResult { get; set; }
        public String templateLowerThird { get; set; }
        public String templatePositions { get; set; }

        public String imageLogoBroadcast { get; set; }
        public String imageCredits { get; set; }
        public int speedImageCredits { get; set; }

        public String team1Logo { get; set; }
        public String team2Logo { get; set; }
        public String leagueLogo { get; set; }
        public String team1Coach { get; set; }
        public String team2Coach { get; set; }
        public List<Player> team1Players { get; set; }
        public List<Player> team2Players { get; set; }
        public String recordingFileName { get; set; }
        public List<Position> positions { get; set; }

        public bool autoHideIntro { get; set; }
        public int autoHideIntroSeconds { get; set; }
        public bool autoHideTeam1 { get; set; }
        public int autoHideTeam1Seconds { get; set; }
        public bool autoHideTeam2 { get; set; }
        public int autoHideTeam2Seconds { get; set; }
        
        public int autoShowHideTeamsSeconds { get; set; }
        public int autoShowHideTeamsInBetweenSeconds { get; set; }

        public bool autoHideResult { get; set; }
        public int autoHideResultSeconds { get; set; }
        public bool autoHideLowerThird { get; set; }
        public int autoHideLowerThirdSeconds { get; set; }

        public bool autoHidePositions { get; set; }
        public int autoHidePositionsSeconds { get; set; }


        private HandballMatch()
        {
            NewMatch();
        }

        public void NewMatch()
        {
            name="";
            recordingCRF = 23;
            speedImageCredits = 3;
            team1Players = new List<Player>();
            team2Players = new List<Player>();
            positions = new List<Position>();

            autoHideIntro = false;
            autoHideIntroSeconds = 1;
            autoHideTeam1 = false;
            autoHideTeam1Seconds = 1;
            autoHideTeam2 = false;
            autoHideTeam2Seconds = 1;
            autoShowHideTeamsSeconds = 1;
            autoShowHideTeamsInBetweenSeconds = 1;
            autoHideResult = false;
            autoHideResultSeconds = 1;
            autoHideLowerThird = false;
            autoHideLowerThirdSeconds = 1;
            autoHidePositions = false;
            autoHidePositionsSeconds = 1;
        }

        public static HandballMatch getInstance()
        {
            if (instance == null)
            {
                instance = new HandballMatch();
            }
            return instance;
        }

        public void saveXML(String XMLFile)
        {
            StreamWriter wr=null;

            try 
	        {	        
                 XmlSerializer ser  = new XmlSerializer(this.GetType());
                 wr = new StreamWriter(XMLFile);
                 ser.Serialize(wr, this);
	        }
	        catch (Exception ex)
	        {
		
		        throw (new Exception("HanballMatch.WriteToXML: " + ex.Message, ex));
	        }
            finally
            {
                wr.Close();
            }
        }

        public void readXML(String XMLFile)
        {
            FileStream fs=null;
            instance=null;

            try 
	        {	        
                 XmlSerializer ser = new XmlSerializer(this.GetType());
                 fs = new FileStream(XMLFile, FileMode.Open);
                 instance = (HandballMatch)ser.Deserialize(fs);
	        }
	        catch (Exception ex)
	        {
                throw (new Exception("HanballMatch.ReadFromXML: " + ex.Message, ex));
	        }
            finally
            {
                fs.Close();
            }
        }

    }
}
