using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tutorial_Linq
{
    class Code2
    {
        // トランプのカードをファローシャッフルする。
        public void Run()
        {
            var startingDeck = Create();
            FallowShuffle(startingDeck);
            ShuffleTimes(startingDeck);
        }
        private System.Collections.Generic.IEnumerable<dynamic> Create() {
            var startingDeck = from s in Suits()
                               from r in Ranks()
                               select new { Suit = s, Rank = r };
            Console.WriteLine(startingDeck.GetType());
//          var startingDeck = Suits().SelectMany(suit => Ranks().Select(rank => new { Suit = suit, Rank = rank }));
            foreach (var card in startingDeck) { Console.WriteLine(card); }
            return startingDeck;
        }
        private void FallowShuffle(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== ファローシャッフル =====");
            var top = startingDeck.Take(26);
            var bottom = startingDeck.Skip(26);
            var shuffle = top.InterleaveSequenceWith2(bottom);
            foreach (var c in shuffle) { Console.WriteLine(c); }
        }
        private void ShuffleTimes(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== 何度ファローシャッフルすれば元に戻るか =====");
            var times = 0;
            var shuffle = startingDeck;
            do
            {
                shuffle = shuffle.Take(26).InterleaveSequenceWith2(shuffle.Skip(26));
                foreach (var card in shuffle) { Console.WriteLine(card); }
                Console.WriteLine();
                times++;
            } while (!startingDeck.SequenceEquals2(shuffle));
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
    public static class Extensions2
    {
        public static IEnumerable<T> InterleaveSequenceWith2<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }
        public static bool SequenceEquals2<T>
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
    }
}
