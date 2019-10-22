using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Tutorial_Linq
{
    class Code4
    {
        // トランプのカードをファローシャッフルする。
        public void Run()
        {
            var startingDeck = (from s in Suits().LogQuery4("Suit Generation")
                                from r in Ranks().LogQuery4("Rank Generation")
                                select new { Suit = s, Rank = r }).LogQuery4("Starting Deck");
            foreach (var c in startingDeck)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine();
            var times = 0;
            var shuffle = startingDeck;
            do
            {
                // Out shuffle
                /*
                shuffle = shuffle.Take(26)
                    .LogQuery4("Top Half")
                    .InterleaveSequenceWith(shuffle.Skip(26)
                    .LogQuery4("Bottom Half"))
                    .LogQuery4("Shuffle");
                */
                // In shuffle
                shuffle = shuffle.Skip(26).LogQuery4("Bottom Half")
                        .InterleaveSequenceWith4(shuffle.Take(26).LogQuery4("Top Half"))
                        .LogQuery4("Shuffle");
                foreach (var c in shuffle)
                {
                    Console.WriteLine(c);
                }
                times++;
                Console.WriteLine(times);
            } while (!startingDeck.SequenceEquals4(shuffle));
            Console.WriteLine(times);
        }
        private IEnumerable<string> Suits()
        {
            yield return "clubs";
            yield return "diamonds";
            yield return "hearts";
            yield return "spades";
        }
        private IEnumerable<string> Ranks()
        {
            yield return "two";
            yield return "three";
            yield return "four";
            yield return "five";
            yield return "six";
            yield return "seven";
            yield return "eight";
            yield return "nine";
            yield return "ten";
            yield return "jack";
            yield return "queen";
            yield return "king";
            yield return "ace";
        }
    }
    public static class Extensions4
    {
        public static IEnumerable<T> InterleaveSequenceWith4<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }
        public static bool SequenceEquals4<T>
            (this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                if (!firstIter.Current.Equals(secondIter.Current))
                {
                    return false;
                }
            }
            return true;
        }
        public static IEnumerable<T> LogQuery4<T>
            (this IEnumerable<T> sequence, string tag)
        {
            using (var writer = File.AppendText("debug.log"))
            {
                writer.WriteLine($"Executing Query {tag}");
            }
            return sequence;
        }
    }
}
