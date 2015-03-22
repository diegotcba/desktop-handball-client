using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente
{
    [Serializable()]
    public class Position
    {
        public String team { get; set; }
        public int points { get; set; }
        public int played { get; set; }
        public int won { get; set; }
        public int drawn { get; set; }
        public int lost { get; set; }
        public int goalsFor { get; set; }
        public int goalsAgainst { get; set; }
        public int goalDifference { get; set; }

        public Position()
        {

        }

        public Position(String team, int points, int played, int won, int drawn, int lost, int goalsFor, int goalsAgainst, int goalDifference)
        {
            this.team = team;
            this.played = played;
            this.points = points;
            this.won = won;
            this.drawn = drawn;
            this.lost = lost;
            this.goalsFor = goalsFor;
            this.goalsAgainst = goalsAgainst;
            this.goalDifference = goalDifference;
        }

        public String getCVS()
        {
            String aux = "";
            aux = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", team, points, played, won, drawn, lost, goalsFor, goalsAgainst, goalDifference);
            return aux;
        }
    }
}
