using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public abstract class Choice
    {
        public abstract string Name { get; }

        public string Battle(Choice other)
        {
            return BattleAgainst(other);
        }

        protected abstract string BattleAgainst(Choice other);

        // Методы двойной диспетчеризации
        public abstract string Fight(Rock r);
        public abstract string Fight(Paper p);
        public abstract string Fight(Scissors s);
        public abstract string Fight(Lizard l);
        public abstract string Fight(Spock s);
    }

}
