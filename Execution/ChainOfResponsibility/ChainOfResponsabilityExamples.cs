using System;
using static System.Console;

namespace Execution.ChainOfResponsibility
{
    #region Method Chain
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next;    //linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null)
                next.Add(cm);
            else
                next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreasingDefenseModifier : CreatureModifier
    {
        public IncreasingDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
        }
    }
    #endregion

    #region Broker Chain
    // better way to do the game simulation
    // in this case, we will have a 2 patterns joined: Chain of Responsability and Mediator... 
    // API for the game
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }        
    }

    // it's specifies which creature you have to work
    public class Query
    {
        public string CreatureName;
        public enum Argument
        {
            Attack, Defense
        }
        public Argument WhatToQuery;
        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creature2
    {
        private Game game;
        public string Name;
        private int attack, defense;

        public Creature2(Game game, string name, int attack, int defense)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.attack = attack;
            this.defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q); // q.Value
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, defense);
                game.PerformQuery(this, q); // q.Value
                return q.Value;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public abstract class CreateModifier2 : IDisposable
    {
        protected Game game;
        protected Creature2 creature;

        protected CreateModifier2(Game game, Creature2 creature)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier2 : CreateModifier2
    {
        public DoubleAttackModifier2(Game game, Creature2 creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            // checking if is the right creature...
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Attack)
            {
                q.Value *= 2;
            }
        }
    }

    public class IncreasingDefenseModifier2 : CreateModifier2
    {
        public IncreasingDefenseModifier2(Game game, Creature2 creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Defense)
            {
                q.Value += 2;
            }
        }
    }
    #endregion

    public static class ChainOfResponsabilityExamples
    {
        // this way is not good and very painfull to implement
        // not a GoF good way
        public static void MethodChain()
        {
            var goblin = new Creature("Goblin", 2, 2);
            WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            root.Add(new NoBonusesModifier(goblin));

            WriteLine("Let's doubling the goblin attack");
            root.Add(new DoubleAttackModifier(goblin));

            WriteLine("Let's increasing the goblin defense");
            root.Add(new IncreasingDefenseModifier(goblin));

            root.Handle();
            WriteLine(goblin);
        }

        // Better game implementation
        public static void BrokerChain()
        {
            var game = new Game();
            var goblin = new Creature2(game, "Strong goblin", 3, 3);    // in real example you can specify game in DI
            WriteLine(goblin);

            using (new DoubleAttackModifier2(game, goblin))
            {
                WriteLine(goblin);
                using (new IncreasingDefenseModifier2(game, goblin))
                {
                    WriteLine(goblin);
                }
            }

            WriteLine(goblin);

            // console output:
            //Name: Strong goblin, Attack: 3, Defense: 3
            //Name: Strong goblin, Attack: 6, Defense: 3
            //Name: Strong goblin, Attack: 6, Defense: 5
            //Name: Strong goblin, Attack: 3, Defense: 3    // beacause of Dispose
        }
    }
}
