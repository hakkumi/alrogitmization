using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Rock : Choice
    {
        public override string Name => "Rock";

        protected override string BattleAgainst(Choice other)
        {
            return other.Fight(this);
        }

        public override string Fight(Rock r) => "Draw";
        public override string Fight(Paper p) => "Paper wins";
        public override string Fight(Scissors s) => "Rock wins";
        public override string Fight(Lizard l) => "Rock wins";
        public override string Fight(Spock s) => "Spock wins";
    }

}
