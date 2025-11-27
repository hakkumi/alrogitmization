using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Paper : Choice
    {
        public override string Name => "Paper";

        protected override string BattleAgainst(Choice other)
        {
            return other.Fight(this);
        }

        public override string Fight(Rock r) => "Paper wins";
        public override string Fight(Paper p) => "Draw";
        public override string Fight(Scissors s) => "Scissors win";
        public override string Fight(Lizard l) => "Lizard wins";
        public override string Fight(Spock s) => "Paper wins";
    }

}
