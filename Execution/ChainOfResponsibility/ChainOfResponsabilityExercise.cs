using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.ChainOfResponsibility
{
    /*
     * You are given a game scenario with classes Goblin and GoblinKing. Please implement the following rules:
     * - A goblin has basd 1 attack / 1 defense (1/1), a goblin king 3/3
     * - When the goblin king is in the play, every other goblin gets +1 attack
     * - Goblins get +1 to defense for every other goblin in play (a GoblinKing is a Goblin!)
     * 
     * Example:
     * - Suppose you have 3 ordinary goblins in play. Each one is a 1/3 (1/1 + 0/2 defense bonus)
     * A goblin king comes into play. Now every goblin is 2/4 (1/1 + 0/3 defense bonus from each other + 1/0 from goblin king)
     * 
     * The state of all the goblins has to be consistent as goblin are added and removed from the game.
     * 
     * Here is an example of the kind of test that you will be run on the system:
     * 
     * [Test]
     * public void Test()
     * {
     *      var game = new Game();
     *      var goblin = new Goblin(game);
     *      game.Creatures.Add(goblin);
     *      Assert.That(goblin.Attack, Is.Equal(1));
     *      Assert.That(goblin.Defense, Is.Equal(1));
     * }
     * 
    */

    //Comentei pra poder compilar....
    //public abstract class Creature
    //{
    //    public int Attack { get; set; }
    //    public int Defense { get; set; }

    //}

    //public class Goblin : Creature
    //{
    //    public Goblin(Game game) { }
    //}

    //public class GoblinKing : Goblin
    //{
    //    public GoblinKing(Game game) { }
    //}

    //public class Game
    //{
    //    public IList<Creature> Creatures;
    //}

    // ---------------------------------------------------------------
    // ---------------------------------------------------------------
    // MY SOLUTION (based on txt explanation...)
    // ---------------------------------------------------------------
    // ---------------------------------------------------------------
    //public enum Movement { Attack, Defense }
    //public class MovementScore
    //{
    //    public Movement Move;
    //    public int Score;
    //}

    //public abstract class Creature
    //{
    //    protected Game game;
    //    protected readonly int attack, defense;

    //    protected Creature(Game game, int attack, int defense)
    //    {
    //        this.game = game;
    //        this.attack = attack;
    //        this.defense = defense;
    //    }

    //    public virtual int Attack { get; set; }
    //    public virtual int Defense { get; set; }
    //    public abstract void ApplyMovement(object source, MovementScore movementScore);
    //}

    //public class Goblin : Creature
    //{
    //    public Goblin(Game game) : this(game, 1, 1) { }
    //    public Goblin(Game game, int attack, int defense) : base(game, attack, defense) { }

    //    public override void ApplyMovement(object source, MovementScore movementScore)
    //    {
    //        if (ReferenceEquals(source, this))
    //        {
    //            switch (movementScore.Move)
    //            {
    //                case Movement.Attack:
    //                    movementScore.Score += attack;
    //                    break;
    //                case Movement.Defense:
    //                    movementScore.Score += defense;
    //                    break;
    //                default:
    //                    throw new ArgumentOutOfRangeException();
    //            }
    //        }
    //        else
    //        {
    //            // in case of different goblin get in
    //            // we add only the defense...
    //            if (movementScore.Move == Movement.Defense)
    //            {
    //                movementScore.Score++;
    //            }
    //        }
    //    }

    //    public override int Attack
    //    {
    //        get
    //        {
    //            var movementScore = new MovementScore { Move = Movement.Attack };
    //            foreach (var c in game.Creatures)
    //            {
    //                c.ApplyMovement(this, movementScore);
    //            }
    //            return movementScore.Score;
    //        }
    //    }

    //    public override int Defense
    //    {
    //        get
    //        {
    //            var movementScore = new MovementScore { Move = Movement.Defense };
    //            foreach (var c in game.Creatures)
    //            {
    //                c.ApplyMovement(this, movementScore);
    //            }
    //            return movementScore.Score;
    //        }
    //    }
    //}

    //public class GoblinKing : Goblin
    //{
    //    public GoblinKing(Game game) : base(game, 3, 3) { }

    //    public override void ApplyMovement(object source, MovementScore movementScore)
    //    {
    //        if (!ReferenceEquals(source, this) && (movementScore.Move == Movement.Attack))
    //        {
    //            movementScore.Score++;  // every goblin gets +1 attack
    //        }
    //        else
    //        {
    //            base.ApplyMovement(source, movementScore);
    //        }
    //    }
    //}

    //public class Game
    //{
    //    public IList<Creature> Creatures = new List<Creature>();
    //}

    class ChainOfResponsabilityExercise
    {
    }
}
