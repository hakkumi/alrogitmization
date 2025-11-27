using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Scissors : Choice
    {
        public override string Name => "Scissors";

        protected override string BattleAgainst(Choice other)
        {
            return other.Fight(this);
        }

        public override string Fight(Rock r) => "Rock wins";
        public override string Fight(Paper p) => "Scissors win";
        public override string Fight(Scissors s) => "Draw";
        public override string Fight(Lizard l) => "Scissors win";
        public override string Fight(Spock s) => "Spock wins";
    }

}
