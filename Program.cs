using static ASSLINQ.ListGenerators;
namespace ASSLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Customers.xml and dictionary_english.txt must be copied to the output folder
            // (Properties -> Copy to Output Directory -> Copy always)

            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region LINQ - Element Operators

            #region Q1 First product out of stock
            var firstOut = ProductList.First(p => p.UnitsInStock == 0);
            Console.WriteLine(firstOut);
            #endregion

            #region Q2 First product whose price > 1000, or null
            var expensive = ProductList.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine(expensive is null ? "null" : expensive.ToString());
            #endregion

            #region Q3 Second number greater than 5
            Console.WriteLine(arr.Where(n => n > 5).ElementAt(1));
            #endregion

            #endregion

            #region LINQ - Aggregate Operators

            #region Q1 Count of odd numbers
            Console.WriteLine(arr.Count(n => n % 2 != 0));
            #endregion

            #region Q2 Customers and how many orders each has
            var customersOrders = CustomerList.Select(c => new { c.CustomerName, OrdersCount = c.Orders.Length });

            foreach (var item in customersOrders)
                Console.WriteLine(item);
            #endregion

            #region Q3 Categories and how many products each has
            var categoriesCount = ProductList.GroupBy(p => p.Category)
                                             .Select(g => new { Category = g.Key, ProductsCount = g.Count() });

            foreach (var item in categoriesCount)
                Console.WriteLine(item);
            #endregion

            #region Q4 Total of the numbers in an array
            Console.WriteLine(arr.Sum());
            #endregion

            string[] words = File.ReadAllLines("dictionary_english.txt");

            #region Q5 Total number of characters of all words
            Console.WriteLine(words.Sum(w => w.Length));
            #endregion

            #region Q6 Length of the shortest word
            Console.WriteLine(words.Min(w => w.Length));
            #endregion

            #region Q7 Length of the longest word
            Console.WriteLine(words.Max(w => w.Length));
            #endregion

            #region Q8 Average length of the words
            Console.WriteLine(words.Average(w => w.Length));
            #endregion

            #region Q9 Total units in stock for each category
            var unitsPerCategory = ProductList.GroupBy(p => p.Category)
                                              .Select(g => new { Category = g.Key, TotalUnits = g.Sum(p => p.UnitsInStock) });

            foreach (var item in unitsPerCategory)
                Console.WriteLine(item);
            #endregion

            #region Q10 Cheapest price in each category
            var cheapestPrice = ProductList.GroupBy(p => p.Category)
                                           .Select(g => new { Category = g.Key, CheapestPrice = g.Min(p => p.UnitPrice) });

            foreach (var item in cheapestPrice)
                Console.WriteLine(item);
            #endregion

            #region Q11 Products with the lowest price in each category (LET)
            var cheapestProducts = from p in ProductList
                                   group p by p.Category into g
                                   let minPrice = g.Min(p => p.UnitPrice)
                                   select new { Category = g.Key, Products = g.Where(p => p.UnitPrice == minPrice) };

            foreach (var item in cheapestProducts)
            {
                Console.WriteLine(item.Category);
                foreach (var product in item.Products)
                    Console.WriteLine("   " + product);
            }
            #endregion

            #region Q12 Highest price in each category
            var highestPrice = ProductList.GroupBy(p => p.Category)
                                          .Select(g => new { Category = g.Key, HighestPrice = g.Max(p => p.UnitPrice) });

            foreach (var item in highestPrice)
                Console.WriteLine(item);
            #endregion

            #region Q13 Products with the highest price in each category
            var mostExpensiveProducts = from p in ProductList
                                        group p by p.Category into g
                                        let maxPrice = g.Max(p => p.UnitPrice)
                                        select new { Category = g.Key, Products = g.Where(p => p.UnitPrice == maxPrice) };

            foreach (var item in mostExpensiveProducts)
            {
                Console.WriteLine(item.Category);
                foreach (var product in item.Products)
                    Console.WriteLine("   " + product);
            }
            #endregion

            #region Q14 Average price of each category
            var averagePrice = ProductList.GroupBy(p => p.Category)
                                          .Select(g => new { Category = g.Key, AveragePrice = g.Average(p => p.UnitPrice) });

            foreach (var item in averagePrice)
                Console.WriteLine(item);
            #endregion

            #endregion

            #region LINQ - Set Operators

            #region Q1 Unique category names
            var categories = ProductList.Select(p => p.Category).Distinct();

            foreach (var item in categories)
                Console.WriteLine(item);
            #endregion

            var productLetters = ProductList.Select(p => p.ProductName[0]);
            var customerLetters = CustomerList.Select(c => c.CustomerName[0]);

            #region Q2 Unique first letters from both product and customer names
            var allLetters = productLetters.Union(customerLetters);

            foreach (var item in allLetters)
                Console.WriteLine(item);
            #endregion

            #region Q3 Common first letters
            var commonLetters = productLetters.Intersect(customerLetters);

            foreach (var item in commonLetters)
                Console.WriteLine(item);
            #endregion

            #region Q4 First letters of products that are not first letters of customers
            var onlyProductLetters = productLetters.Except(customerLetters);

            foreach (var item in onlyProductLetters)
                Console.WriteLine(item);
            #endregion

            #region Q5 Last three characters of all names (with duplicates)
            var lastThree = ProductList.Select(p => p.ProductName)
                                       .Concat(CustomerList.Select(c => c.CustomerName))
                                       .Select(name => new string(name.TakeLast(3).ToArray()));

            foreach (var item in lastThree)
                Console.WriteLine(item);
            #endregion

            #endregion

            #region LINQ - Partitioning Operators

            #region Q1 First 3 orders from customers in Washington
            var firstThreeOrders = CustomerList.Where(c => c.Region == "WA")
                                               .SelectMany(c => c.Orders)
                                               .Take(3);

            foreach (var item in firstThreeOrders)
                Console.WriteLine(item);
            #endregion

            #region Q2 All but the first 2 orders from customers in Washington
            var skipTwoOrders = CustomerList.Where(c => c.Region == "WA")
                                            .SelectMany(c => c.Orders)
                                            .Skip(2);

            foreach (var item in skipTwoOrders)
                Console.WriteLine(item);
            #endregion

            #region Q3 From the beginning until a number is less than its position
            var untilLess = arr.TakeWhile((n, i) => n >= i);

            foreach (var item in untilLess)
                Console.WriteLine(item);
            #endregion

            #region Q4 Starting from the first element divisible by 3
            var fromDivisibleBy3 = arr.SkipWhile(n => n % 3 != 0);

            foreach (var item in fromDivisibleBy3)
                Console.WriteLine(item);
            #endregion

            #region Q5 Starting from the first element less than its position
            var fromLess = arr.SkipWhile((n, i) => n >= i);

            foreach (var item in fromLess)
                Console.WriteLine(item);
            #endregion

            #endregion

            #region LINQ - Quantifiers

            #region Q1 Any word contains 'ei'
            Console.WriteLine(words.Any(w => w.Contains("ei")));
            #endregion

            #region Q2 Categories that have at least one product out of stock
            var someOut = ProductList.GroupBy(p => p.Category)
                                     .Where(g => g.Any(p => p.UnitsInStock == 0));

            foreach (var grp in someOut)
            {
                Console.WriteLine(grp.Key);
                foreach (var product in grp)
                    Console.WriteLine("   " + product);
            }
            #endregion

            #region Q3 Categories that have all products in stock
            var allIn = ProductList.GroupBy(p => p.Category)
                                   .Where(g => g.All(p => p.UnitsInStock > 0));

            foreach (var grp in allIn)
            {
                Console.WriteLine(grp.Key);
                foreach (var product in grp)
                    Console.WriteLine("   " + product);
            }
            #endregion

            #endregion

            #region LINQ - Grouping Operators

            #region Q1 Group numbers by their remainder when divided by 5
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var byRemainder = numbers.GroupBy(n => n % 5);

            foreach (var grp in byRemainder)
            {
                Console.WriteLine($"Numbers with a remainder of {grp.Key} when divided by 5:");
                foreach (var number in grp)
                    Console.WriteLine(number);
            }
            #endregion

            #region Q2 Group words by their first letter
            // printing all the words will be too long, so we print the letter and how many words it has
            var byFirstLetter = words.Where(w => w != "").GroupBy(w => w[0]);

            foreach (var grp in byFirstLetter)
                Console.WriteLine($"{grp.Key} : {grp.Count()}");
            #endregion

            #region Q3 Group by with custom comparer (same characters together)
            string[] anagrams = { "from", "salt", "earn", " last", "near", "form" };

            var sameChars = anagrams.GroupBy(w => w, new AnagramComparer());

            foreach (var grp in sameChars)
            {
                foreach (var word in grp)
                    Console.WriteLine(word);
                Console.WriteLine("....");
            }
            #endregion

            #endregion
        }
    }
}
