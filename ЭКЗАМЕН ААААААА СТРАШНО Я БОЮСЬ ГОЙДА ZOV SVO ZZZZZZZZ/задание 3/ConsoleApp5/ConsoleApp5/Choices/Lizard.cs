using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Lizard : Choice
    {
        public override string Name => "Lizard";

        protected override string BattleAgainst(Choice other)
        {
            return other.Fight(this);
        }

        public override string Fight(Rock r) => "Rock wins";
        public override string Fight(Paper p) => "Lizard wins";
        public override string Fight(Scissors s) => "Scissors win";
        public override string Fight(Lizard l) => "Draw";
        public override string Fight(Spock s) => "Lizard wins";
    }

}
