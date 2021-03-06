﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Xml;
using System.Xml.Serialization;
using System.Data;
using HandballCliente.Models;


namespace HandballCliente
{
    [Serializable()]
    public class HandballMatch
    {
        private static HandballMatch instance = null;

        public String name { get; set; }
        public String description { get; set; }
        public DateTime fecha { get; set; }

        //PARAMETERS
        public String serverAddress { get; set; }
        public String serverPort { get; set; }
        public String templatePresentation { get; set; }
        public String templateTeam { get; set; }
        public String templateScoreboard { get; set; }
        public String templateResult { get; set; }
        public String templateLowerThird { get; set; }
        public String templatePositions { get; set; }
        public String templateTwitter { get; set; }

        //EVENT
        public String team1Name { get; set; }
        public String team2Name { get; set; }
        public String team1Logo { get; set; }
        public String team2Logo { get; set; }
        public String leagueLogo { get; set; }
        public String eventTitle { get; set; }
        public String leagueName { get; set; }
        public String location { get; set; }
        public bool autoHideIntro { get; set; }
        public int autoHideIntroSeconds { get; set; }

        //TEAMS
        public String team1Coach { get; set; }
        public String team2Coach { get; set; }
        public List<Player> team1Players { get; set; }
        public List<Player> team2Players { get; set; }
        public bool autoHideTeam1 { get; set; }
        public int autoHideTeam1Seconds { get; set; }
        public bool autoHideTeam2 { get; set; }
        public int autoHideTeam2Seconds { get; set; }
        public int autoShowHideTeamsSeconds { get; set; }
        public int autoShowHideTeamsInBetweenSeconds { get; set; }
        public int teamPlayerFontSize { get; set; }
        public int teamPlayerLineSpacing { get; set; }
        public int teamPlayerLetterSpacing { get; set; }

        //SCOREBOARD
        public String scoreHalf { get; set; }
        public int scoreClockMinutes { get; set; }
        public int scoreClockSeconds { get; set; }
        public int scoreClockMatchMinutes { get; set; }
        public int scoreClockMatchSeconds { get; set; }
        public int scoreClockExclutionMinutes { get; set; }
        public int scoreClockExclutionSeconds { get; set; }
        public bool scoreAutoHideOnClockFinished { get; set; }
        public bool scoreAutoShowOnClockStart { get; set; } 
        public bool autoHideResult { get; set; }
        public int autoHideResultSeconds { get; set; }
        public String team1ScoreName { get; set; }
        public String team2ScoreName { get; set; }

        //LOWER THIRD
        public bool autoHideLowerThird { get; set; }
        public int autoHideLowerThirdSeconds { get; set; }

        //POSITIONS
        public List<Position> positions { get; set; }
        public String positionsTitle { get; set; }
        public String positionsSubtitle { get; set; }
        public bool autoHidePositions { get; set; }
        public int autoHidePositionsSeconds { get; set; }
        
        //MULTIMEDIA
        public String recordingFileName { get; set; }
        public int recordingCRF { get; set; }

        //OTHERS
        public String imageLogoBroadcast { get; set; }
        public String imageCredits { get; set; }
        public int speedImageCredits { get; set; }
        public String twitterHashtag { get; set; }
        public String twitterUsername { get; set; }
        public String twitterFullname { get; set; }
        public String twitterMessage { get; set; }
        public bool autoHideTwitter { get; set; }
        public int autoHideTwitterSeconds { get; set; }

        //VOLLEY/TABLETENNIS
        public String titleVolleyScoreboard { get; set; }
        public String websiteVolleyScoreboardResult { get; set; }
        public bool showVolleyService { get; set; }
        public String templateVolleyScoreboard { get; set; }
        public String templateVolleyResult { get; set; }
        public bool autoUpdateVolleyScoreboard { get; set; }
        public bool autoHideVolleyResult { get; set; }
        public int autoHideVolleyResultSeconds { get; set; }

        //GAMESHOW
        public List<Question> gameshowQuestions { get; set; }
        public List<FindCardItem> gameshowFindCardItems { get; set; }
        public List<FindCardMatch> gameshowFindCardMatches { get; set; }

        //ELECTIONS
        public List<Election> electionResults { get; set; }

        //WEATHERFORECAST
        public List<PronosticoCiudad> weatherForecast { get; set; }

        //TWITTER
        public List<Tweets> tweets { get; set; }
        public List<Playlist> playlists { get; set; }

        //DYNAMIC INFO
        public List<DynamicInfo> dynamicInfo { get; set; }


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
            gameshowQuestions = new List<Question>();
            gameshowFindCardItems = new List<FindCardItem>();
            gameshowFindCardMatches = new List<FindCardMatch>();
            weatherForecast = new List<PronosticoCiudad>();
            tweets = new List<Tweets>();
            playlists = new List<Playlist>();

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
            autoHideTwitter = false;
            autoHideTwitterSeconds = 1;
            autoUpdateVolleyScoreboard = false;
            autoHideVolleyResult = false;
            autoHideVolleyResultSeconds = 1;
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
