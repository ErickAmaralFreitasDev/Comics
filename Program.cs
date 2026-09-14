using System.Collections.Generic;
using System.Linq;
using Comics;

namespace JimmyLinq
{
    static class ComicAnalyzer
    {
        private static PriceRange CalculatePriceRange(Comic comic)
        {
            if (Comic.Prices[comic.Issue] < 100M)
            {
                return PriceRange.Cheap;
            }
            else
            {
                return PriceRange.Expensive;
            }
        }

        public static IEnumerable<IGrouping<PriceRange, Comic>> GroupComicsByPrice(IEnumerable<Comic> comics, IReadOnlyDictionary<int, decimal> prices)
        {
            IEnumerable<IGrouping<PriceRange, Comic>> grouped =
                from comic in comics
                orderby prices[comic.Issue] ascending
                group comic by CalculatePriceRange(comic) into priceGroup
                select priceGroup;
            return grouped;
        }

        public static IEnumerable<string> GetReviews(IEnumerable<Comic> comics, IEnumerable<Review> reviews)
        {
            var join =
                from comic in comics
                orderby comic.Issue 
                join review in reviews on comic.Issue equals review.Issue
                select $"{review.Critic} rated #{comic.Issue} '{comic.Name}' with score {review.Score}";
            return join;
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            var done = false;
            while (!done)
            {
                Console.WriteLine("\nPress G to group comics by price, R to get reviews, any other key to exit.\n");
                switch (Console.ReadKey(true).KeyChar.ToString().ToUpper())
                {
                    case "G":
                        done = GroupComicsByPrice();
                        break;
                    case "R":
                        done = GetReviews();
                        break;
                    default:
                        done = true;
                        break;
                }
            }
        }

        private static bool GroupComicsByPrice()
        {
            var groups = ComicAnalyzer.GroupComicsByPrice(Comic.Catalog, Comic.Prices);
                foreach (var group in groups)
                {
                    Console.WriteLine($"{group.Key} comics:");
                    foreach (var comic in group)
                    {
                        Console.WriteLine($"#{comic.Issue} '{comic.Name}' costs {Comic.Prices[comic.Issue]:C}");
                    }
                }
                return false;
        }

        private static bool GetReviews()
        {
            var reviews = ComicAnalyzer.GetReviews(Comic.Catalog, Comic.Reviews);
            foreach (var review in reviews)
            {
                Console.WriteLine(review);
            }
            return false;
        }
    }
}

