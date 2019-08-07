using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

// Command pattern definition
// An object which represents an instruction to perform a perticular action.
// Contains all the information necessarly for the action to be taken.
namespace Execution.Command
{
    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {balance}.");
        }

        // to the Undo operation owrk, we need to change this ethod to return a boolean value
        //public void WithDraw(int amount)
        //{
        //    if (balance - amount >= overdraftLimit)
        //    {
        //        balance -= +amount;
        //        WriteLine($"Withdrew ${amount}, balance is now {balance}.");
        //    }
        //}
        public bool WithDraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= +amount;
                WriteLine($"Withdrew ${amount}, balance is now {balance}.");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}, {nameof(overdraftLimit)}: {overdraftLimit}";
        }
    }

    public interface ICommand
    {
        void Call();

        //undo operations...
        void Undo();
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount account;
        private bool succeeded;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
            this.action = action;
            this.amount = amount;
        }

        public void Call()
        {
            if (!succeeded) return;

            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    succeeded = true;
                    break;
                case Action.Withdraw:
                    succeeded = account.WithDraw(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            if (!succeeded) return;

            switch (action)
            {
                case Action.Deposit:
                    succeeded = account.WithDraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    succeeded = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class CommandExamples
    {
        public static void Demo1()
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 50)
            };

            WriteLine(ba);

            foreach (var cmd in commands)
                cmd.Call();

            WriteLine(ba);
        }

        public static void UndoOperations()
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 600)
            };

            WriteLine(ba);

            foreach (var cmd in commands)
                cmd.Call();

            WriteLine(ba);

            foreach (var cmd in Enumerable.Reverse(commands))
                cmd.Undo();

            WriteLine(ba);
        }

        public static void Exercise()
        {
            var ac = new Account();
            var commands = new List<Command>
            {
                new Command { TheAction = Command.Action.Deposit, Amount = 100},
                new Command { TheAction = Command.Action.Deposit, Amount = 200},
                new Command { TheAction = Command.Action.Withdraw, Amount = 50},
                new Command { TheAction = Command.Action.Withdraw, Amount = 1000}
            };

            foreach (var cmd in commands)
                ac.Process(cmd);

            WriteLine(ac);
        }
    }

    #region exercise
    public class Command
    {
        public enum Action
        {
            Deposit,
            Withdraw
        }

        public Action TheAction;
        public int Amount;
        public bool Success;
    }

    public class Account
    {
        public int Balance { get; set; }

        public void Process(Command c)
        {
            // todo
            switch (c.TheAction)
            {
                case Command.Action.Deposit:
                    Balance += c.Amount;
                    c.Success = true;
                    break;
                case Command.Action.Withdraw:
                    c.Success = (Balance - c.Amount >= 0);

                    if (c.Success)
                        Balance -= c.Amount;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            WriteLine($"{Enum.GetName(typeof(Command.Action), c.TheAction)} of ${c.Amount} {(c.Success ? "occured" : "not occured")}. Balance is now ${Balance}");
        }

        public override string ToString()
        {
            return $"Account balance is ${Balance}";
        }
    }
    #endregion
}
