using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Tutorial_Linq
{
    class Code6
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
            var startingDeck = (from s in Suits().LogQuery6("Suit Generation")
                                from r in Ranks().LogQuery6("Rank Generation")
                                select new { Suit = s, Rank = r }).LogQuery6("Starting Deck");
            Console.WriteLine(startingDeck.GetType());
//          var startingDeck = Suits().SelectMany(suit => Ranks().Select(rank => new { Suit = suit, Rank = rank }));
            foreach (var card in startingDeck) { Console.WriteLine(card); }
            return startingDeck;
        }
        private void FallowShuffle(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== ファローシャッフル =====");
            var top = startingDeck.Take(26);
            var bottom = startingDeck.Skip(26);
            var shuffle = top.InterleaveSequenceWith6(bottom);
            foreach (var c in shuffle) { Console.WriteLine(c); }
        }
        private void ShuffleTimes(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== 何度ファローシャッフルすれば元に戻るか =====");
            var times = 0;
            var shuffle = startingDeck;
            do
            {
//                shuffle = shuffle.Take(26).InterleaveSequenceWith6(shuffle.Skip(26)); // アウトシャッフル
//                shuffle = shuffle.Skip(26).InterleaveSequenceWith6(shuffle.Take(26)); // インシャッフル
                // Out shuffle
                /*
                shuffle = shuffle.Take(26)
                    .LogQuery6("Top Half")
                    .InterleaveSequenceWith(shuffle.Skip(26)
                    .LogQuery6("Bottom Half"))
                    .LogQuery6("Shuffle");
                */
                // In shuffle
                shuffle = shuffle.Skip(26).LogQuery6("Bottom Half")
                        .InterleaveSequenceWith6(shuffle.Take(26).LogQuery6("Top Half"))
                        .LogQuery6("Shuffle");

                foreach (var card in shuffle) { Console.WriteLine(card); }
                Console.WriteLine();
                times++;
                Console.WriteLine(times);
            } while (!startingDeck.SequenceEquals6(shuffle));
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
    public static class Extensions6
    {
        public static IEnumerable<T> InterleaveSequenceWith6<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }
        public static bool SequenceEquals6<T>
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
        public static IEnumerable<T> LogQuery6<T>
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
