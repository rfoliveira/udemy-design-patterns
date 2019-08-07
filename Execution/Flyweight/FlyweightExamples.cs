using JetBrains.dotMemoryUnit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.Console;

namespace Execution.Flyweight
{
    public class User
    {
        private string fullname;

        public User(string fullname)
        {
            this.fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
        }
    }

    public class User2
    {
        static List<string> strings = new List<string>();
        private int[] names;

        public User2(string fullname)
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullname.Split(' ').Select(getOrAdd).ToArray();
        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));
    }

    // this way expands a lot of memory...
    public class FormattedText
    {
        private readonly string plaintext;
        private bool[] capitalize;
        private Stopwatch watch = new Stopwatch();

        public FormattedText(string plaintext)
        {
            this.plaintext = plaintext ?? throw new ArgumentNullException(nameof(plaintext));
            capitalize = new bool[plaintext.Length];
        }

        public void Capitalize(int start, int end)
        {
            watch.Start();
            for (int i = start; i <= end; i++)
            {
                capitalize[i] = true;
            }
            watch.Stop();

            WriteLine($"FormattedText.Capitalize executed in {watch.ElapsedMilliseconds}ms");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            watch.Start();
            for (int i = 0; i < plaintext.Length; i++)
            {
                var c = plaintext[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }
            watch.Stop();
            WriteLine($"FormattedText.ToString executed in {watch.ElapsedMilliseconds}ms");
            return sb.ToString();
        }
    }

    // same as FormattedText but applying Flyweight pattern
    public class BetterFormattedText
    {
        private string plaintext;
        private List<TextRange> formatting = new List<TextRange>();
        private Stopwatch watch = new Stopwatch();

        public BetterFormattedText(string plaintext)
        {
            this.plaintext = plaintext ?? throw new ArgumentNullException(nameof(plaintext));
        }

        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange
            {
                Start = start,
                End = end
            };
            formatting.Add(range);

            return range;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            watch.Start();
            for (int i = 0; i < plaintext.Length; i++)
            {
                var c = plaintext[i];
                foreach (var range in formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                }
                sb.Append(c);
            }
            watch.Stop();
            WriteLine($"BetterFormattedText.ToString executed in {watch.ElapsedMilliseconds}ms");
            return sb.ToString();
        }       
    }

    public static class FlyweightExamples
    {
        public static void SizeInBytesTest1()   //1655033 (se funcionasse)
        {
            var firstnames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastnames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstname in firstnames)
            {
                foreach (var lastname in lastnames)
                {
                    users.Add(new User($"{firstname} {lastname}"));
                }
            }

            ForceGC();

            // Não funciona, dá o erro abaixo:
            // Exceção Sem Tratamento: DotMemoryUnitException: The test was run without the support for dotMemory Unit. To safely run tests with or without (depending on your needs) the support for dotMemory Unit:
            // -Set 'DotMemoryUnitAttribute.FailIfRunWithoutSupport' to 'False'.In this case, if a test is run without the support for dotMemory Unit, all 'dotMemory.Check' calls will be ignored.
            // If you use the 'dotMemoryApi' class to work with memory, wrap all dotMemoryApi calls that get data for assertions with the 'dotMemoryApi.IsEnabled' check.
            //dotMemory.Check(memory =>
            //{
            //    WriteLine(memory.SizeInBytes);
            //});

            // também não funciona porque  "dotMemoryApi.IsEnabled = false"...
            //if (dotMemoryApi.IsEnabled)
            //{
            //    var snapshot = dotMemoryApi.GetSnapshot();
            //    WriteLine(snapshot.SizeInBytes);
            //}
        }

        public static void SizeInBytesTest2()   //1296991 (se funcionasse)
        {
            var firstnames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastnames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User2>();

            foreach (var firstname in firstnames)
            {
                foreach (var lastname in lastnames)
                {
                    users.Add(new User2($"{firstname} {lastname}"));
                }
            }

            ForceGC();

            // Não funciona, dá o erro abaixo:
            // Exceção Sem Tratamento: DotMemoryUnitException: The test was run without the support for dotMemory Unit. To safely run tests with or without (depending on your needs) the support for dotMemory Unit:
            // -Set 'DotMemoryUnitAttribute.FailIfRunWithoutSupport' to 'False'.In this case, if a test is run without the support for dotMemory Unit, all 'dotMemory.Check' calls will be ignored.
            // If you use the 'dotMemoryApi' class to work with memory, wrap all dotMemoryApi calls that get data for assertions with the 'dotMemoryApi.IsEnabled' check.
            //dotMemory.Check(memory =>
            //{
            //    WriteLine(memory.SizeInBytes);
            //});

            // também não funciona porque  "dotMemoryApi.IsEnabled = false"...
            //if (dotMemoryApi.IsEnabled)
            //{
            //    var snapshot = dotMemoryApi.GetSnapshot();
            //    WriteLine(snapshot.SizeInBytes);
            //}
        }

        private static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private static string RandomString()
        {
            Random rand = new Random();

            return new string(
                Enumerable.Range(0, 10)
                .Select(i => (char)('a' + rand.Next(26)))
                .ToArray()
            );
        }

        public static void TextFormatting()
        {            
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15);
            WriteLine(ft);

            var bft = new BetterFormattedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
            WriteLine(bft);
        }
    }
}
