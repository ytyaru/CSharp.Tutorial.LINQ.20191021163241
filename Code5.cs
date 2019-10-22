using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Tutorial_Linq
{
    class Code5
    {
        // トランプのカードをファローシャッフルする。
        public void Run()
        {
            var startingDeck = Create();
            FallowShuffle(startingDeck);
            ShuffleTimes(startingDeck);// 52回と表示されるはずだが長時間かかる
        }
        private System.Collections.Generic.IEnumerable<dynamic> Create() {
//            var startingDeck = from s in Suits()
//                               from r in Ranks()
//                               select new { Suit = s, Rank = r };
            var startingDeck = (from s in Suits().LogQuery("Suit Generation")
                                from r in Ranks().LogQuery("Rank Generation")
                                select new { Suit = s, Rank = r }).LogQuery("Starting Deck");
            Console.WriteLine(startingDeck.GetType());
//          var startingDeck = Suits().SelectMany(suit => Ranks().Select(rank => new { Suit = suit, Rank = rank }));
            foreach (var card in startingDeck) { Console.WriteLine(card); }
            return startingDeck;
        }
        private void FallowShuffle(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== ファローシャッフル =====");
            var top = startingDeck.Take(26);
            var bottom = startingDeck.Skip(26);
            var shuffle = top.InterleaveSequenceWith5(bottom);
            foreach (var c in shuffle) { Console.WriteLine(c); }
        }
        private void ShuffleTimes(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== 何度ファローシャッフルすれば元に戻るか =====");
            var times = 0;
            var shuffle = startingDeck;
            do
            {
//                shuffle = shuffle.Take(26).InterleaveSequenceWith5(shuffle.Skip(26)); // アウトシャッフル
//                shuffle = shuffle.Skip(26).InterleaveSequenceWith5(shuffle.Take(26)); // インシャッフル
                // Out shuffle
                /*
                shuffle = shuffle.Take(26)
                    .LogQuery5("Top Half")
                    .InterleaveSequenceWith(shuffle.Skip(26)
                    .LogQuery5("Bottom Half"))
                    .LogQuery5("Shuffle");
                */
                // In shuffle
                shuffle = shuffle.Skip(26).LogQuery5("Bottom Half")
                        .InterleaveSequenceWith(shuffle.Take(26).LogQuery5("Top Half"))
                        .LogQuery5("Shuffle");

                foreach (var card in shuffle) { Console.WriteLine(card); }
                Console.WriteLine();
                times++;
                Console.WriteLine(times);
            } while (!startingDeck.SequenceEquals5(shuffle));
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
    public static class Extensions5
    {
        public static IEnumerable<T> InterleaveSequenceWith5<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }
        public static bool SequenceEquals5<T>
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
        public static IEnumerable<T> LogQuery5<T>
            (this IEnumerable<T> sequence, string tag)
        {
            // File.AppendText creates a new file if the file doesn't exist.
            using (var writer = File.AppendText("debug.log"))
            {
                writer.WriteLine($"Executing Query {tag}");
            }

            return sequence;
        }
    }
}
