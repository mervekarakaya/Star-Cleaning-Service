using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarCleaningService_Identity.Model
{
    public class CalculateRate
    {
        public string userId { get; set; }
        public string avgRating { get; set; }
        public static double GetRating(List<int> rates)
        {
            int star5 = rates.Count(x => x == 5);
            int star4 = rates.Count(x => x == 4);
            int star3 = rates.Count(x => x == 3);
            int star2 = rates.Count(x => x == 2);
            int star1 = rates.Count(x => x == 1);

            //calculation for average rating
            double rating = (double)(5 * star5 + 4 * star4 + 3 * star3 + 2 * star2 + 1 * star1) / (star1 + star2 + star3 + star4 + star5);

            rating = Math.Round(rating, 1);

            return rating;
        }
    }
}
