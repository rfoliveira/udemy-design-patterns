using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Flyweight
{
    public class Sentence
    {
        private string plainText;
        private Dictionary<int, WordToken> tokens = new Dictionary<int, WordToken>();

        public Sentence(string plainText)
        {
            // todo
            this.plainText = plainText;
        }

        public WordToken this[int index]
        {
            get
            {
                // todo
                tokens.Add(index, new WordToken());
                return tokens[index];
            }
        }

        public override string ToString()
        {
            // output formatted text here
            var words = plainText.Split(' ');
            var result = new string[words.Length];

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];

                if (tokens.ContainsKey(i) && tokens[i].Capitalize)
                    word = words[i].ToUpper();

                result[i] = word;
            }

            return string.Join(" ", result);
        }

        public class WordToken
        {
            public bool Capitalize;
        }
    }

    public static class FlyweightExercise
    {
        public static void Result()
        {
            var sentence = new Sentence("Rafael Fernandes de Oliveira");
            sentence[1].Capitalize = true;

            Console.WriteLine(sentence);
        }
    }
}
