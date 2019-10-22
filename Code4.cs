using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tutorial_Linq
{
    class Code4
    {
        // トランプのカードをファローシャッフルする。
        public void Run()
        {
            var startingDeck = Create();
            var shuffle = FallowShuffle(startingDeck);
            ShuffleTimes(shuffle);
//            ShuffleTimes(startingDeck);
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
        private System.Collections.Generic.IEnumerable<dynamic> FallowShuffle(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== ファローシャッフル =====");
            /*
            // アウトシャッフル
            var top = startingDeck.Take(26);
            var bottom = startingDeck.Skip(26);
            var shuffle = top.InterleaveSequenceWith4(bottom);
            */
            // インシャッフル
//            var shuffle = startingDeck;
//            shuffle = shuffle.Skip(26).InterleaveSequenceWith4(shuffle.Take(26));
            var shuffle = startingDeck.Skip(26).InterleaveSequenceWith4(startingDeck.Take(26));
            foreach (var c in shuffle) { Console.WriteLine(c); }
            return shuffle;
        }
        private void ShuffleTimes(IEnumerable<dynamic> startingDeck) {
            Console.WriteLine("===== 何度ファローシャッフルすれば元に戻るか =====");
            var times = 0;
            var shuffle = startingDeck;
            do
            {
                shuffle = shuffle.Take(26).InterleaveSequenceWith4(shuffle.Skip(26));
                foreach (var card in shuffle) { Console.WriteLine(card); }
                Console.WriteLine();
                times++;
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
    }
}
