using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matching_Game.Classes
{
    class Player
    {
        public String Nick;
        public Int32 scores;

        public Player(String nazwa, Int32 punkty)
        {
            this.Nick = nazwa;
            this.scores = punkty;

        }
        public string NickName
        {
            get
            {
                return Nick;
            }

            set
            {
                Nick = value;
            }
        }

        public int ScoresPoints
        {
            get
            {
                return scores;
            }

            set
            {
                scores = value;
            }
        }
    }
}
