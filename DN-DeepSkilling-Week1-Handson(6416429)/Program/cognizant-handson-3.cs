using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EcommerceSearchAlgorithms
{
    
    public class Product : IComparable<Product>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public int StockQuantity { get; set; }

        public Product(int productId, string productName, string category, decimal price, string brand, int stockQuantity)
        {
            ProductId = productId;
            ProductName = productName;
            Category = category;
            Price = price;
            Brand = brand;
            StockQuantity = stockQuantity;
        }

        // Implement IComparable for sorting (by ProductId)
        public int CompareTo(Product? other)
        {
            if (other == null) return 1;
            return ProductId.CompareTo(other.ProductId);
        }

        public override string ToString()
        {
            return $"ID: {ProductId}, Name: {ProductName}, Category: {Category}, Price: ${Price:F2}, Brand: {Brand}, Stock: {StockQuantity}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Product product)
                return ProductId == product.ProductId;
            return false;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }

    
    public class SearchResult
    {
        public Product? Product { get; set; }
        public int Index { get; set; }
        public long ElapsedTicks { get; set; }
        public int ComparisonsCount { get; set; }
        public bool Found { get; set; }

        public SearchResult(Product? product, int index, long elapsedTicks, int comparisons, bool found)
        {
            Product = product;
            Index = index;
            ElapsedTicks = elapsedTicks;
            ComparisonsCount = comparisons;
            Found = found;
        }
    }

    
    public static class AlgorithmComplexityExplainer
    {
        public static void ExplainBigONotation()
        {
            Console.WriteLine("=== BIG O NOTATION EXPLANATION ===");
            Console.WriteLine();
            Console.WriteLine("Big O notation describes the upper bound of algorithm performance,");
            Console.WriteLine("focusing on how execution time grows relative to input size (n).");
            Console.WriteLine();
            Console.WriteLine("Common Big O Complexities (from best to worst):");
            Console.WriteLine("• O(1)      - Constant time (best)");
            Console.WriteLine("• O(log n)  - Logarithmic time (very good)");
            Console.WriteLine("• O(n)      - Linear time (acceptable)");
            Console.WriteLine("• O(n log n)- Linearithmic time (fair)");
            Console.WriteLine("• O(n²)     - Quadratic time (poor)");
            Console.WriteLine("• O(2^n)    - Exponential time (very poor)");
            Console.WriteLine();
            Console.WriteLine("For Search Algorithms:");
            Console.WriteLine("• Linear Search:  O(n) - Must check each element sequentially");
            Console.WriteLine("• Binary Search:  O(log n) - Eliminates half the data each step");
            Console.WriteLine();
        }

        public static void ExplainSearchScenarios()
        {
            Console.WriteLine("=== SEARCH ALGORITHM SCENARIOS ===");
            Console.WriteLine();
            Console.WriteLine("LINEAR SEARCH:");
            Console.WriteLine("• Best Case:    O(1) - Element is at the first position");
            Console.WriteLine("• Average Case: O(n/2) ≈ O(n) - Element is in the middle");
            Console.WriteLine("• Worst Case:   O(n) - Element is at the last position or not found");
            Console.WriteLine();
            Console.WriteLine("BINARY SEARCH:");
            Console.WriteLine("• Best Case:    O(1) - Element is at the middle position");
            Console.WriteLine("• Average Case: O(log n) - Standard logarithmic performance");
            Console.WriteLine("• Worst Case:   O(log n) - Element is at leaf level or not found");
            Console.WriteLine();
            Console.WriteLine("KEY REQUIREMENTS:");
            Console.WriteLine("• Linear Search: Works on unsorted arrays");
            Console.WriteLine("• Binary Search: Requires sorted array");
            Console.WriteLine();
        }
    }

    
    public class EcommerceSearchEngine
    {
        private Product[] products;
        private Product[] sortedProducts;

        public EcommerceSearchEngine(Product[] productArray)
        {
            products = productArray;
            sortedProducts = new Product[productArray.Length];
            Array.Copy(productArray, sortedProducts, productArray.Length);
            Array.Sort(sortedProducts); 
        }

       
        public SearchResult LinearSearchById(int productId)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int comparisons = 0;

            for (int i = 0; i < products.Length; i++)
            {
                comparisons++;
                if (products[i].ProductId == productId)
                {
                    stopwatch.Stop();
                    return new SearchResult(products[i], i, stopwatch.ElapsedTicks, comparisons, true);
                }
            }

            stopwatch.Stop();
            return new SearchResult(null, -1, stopwatch.ElapsedTicks, comparisons, false);
        }

        
        public SearchResult BinarySearchById(int productId)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int comparisons = 0;
            int left = 0;
            int right = sortedProducts.Length - 1;

            while (left <= right)
            {
                comparisons++;
                int mid = left + (right - left) / 2;

                if (sortedProducts[mid].ProductId == productId)
                {
                    stopwatch.Stop();
                    return new SearchResult(sortedProducts[mid], mid, stopwatch.ElapsedTicks, comparisons, true);
                }

                if (sortedProducts[mid].ProductId < productId)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            stopwatch.Stop();
            return new SearchResult(null, -1, stopwatch.ElapsedTicks, comparisons, false);
        }

        
        public List<SearchResult> LinearSearchByName(string productName)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<SearchResult> results = new List<SearchResult>();
            int comparisons = 0;

            for (int i = 0; i < products.Length; i++)
            {
                comparisons++;
                if (products[i].ProductName.IndexOf(productName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(new SearchResult(products[i], i, stopwatch.ElapsedTicks, comparisons, true));
                }
            }

            stopwatch.Stop();
            
            
            foreach (var result in results)
            {
                result.ElapsedTicks = stopwatch.ElapsedTicks;
            }

            if (results.Count == 0)
            {
                results.Add(new SearchResult(null, -1, stopwatch.ElapsedTicks, comparisons, false));
            }

            return results;
        }

       
        public List<SearchResult> LinearSearchByCategory(string category)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<SearchResult> results = new List<SearchResult>();
            int comparisons = 0;

            for (int i = 0; i < products.Length; i++)
            {
                comparisons++;
                if (string.Equals(products[i].Category, category, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new SearchResult(products[i], i, stopwatch.ElapsedTicks, comparisons, true));
                }
            }

            stopwatch.Stop();
            
            foreach (var result in results)
            {
                result.ElapsedTicks = stopwatch.ElapsedTicks;
            }

            if (results.Count == 0)
            {
                results.Add(new SearchResult(null, -1, stopwatch.ElapsedTicks, comparisons, false));
            }

            return results;
        }

        public int GetProductCount() => products.Length;
        public Product[] GetAllProducts() => products;
        public Product[] GetSortedProducts() => sortedProducts;
    }

    
    public class PerformanceAnalyzer
    {
        public static void CompareSearchAlgorithms(EcommerceSearchEngine searchEngine, int[] testProductIds)
        {
            Console.WriteLine("=== PERFORMANCE COMPARISON ===");
            Console.WriteLine();
            Console.WriteLine($"Dataset Size: {searchEngine.GetProductCount()} products");
            Console.WriteLine($"Test Cases: {testProductIds.Length} searches");
            Console.WriteLine();

            long totalLinearTicks = 0;
            long totalBinaryTicks = 0;
            int totalLinearComparisons = 0;
            int totalBinaryComparisons = 0;
            int linearFoundCount = 0;
            int binaryFoundCount = 0;

            Console.WriteLine("Individual Search Results:");
            Console.WriteLine("Product ID | Linear Search | Binary Search | Difference");
            Console.WriteLine(new string('-', 65));

            foreach (int productId in testProductIds)
            {
                SearchResult linearResult = searchEngine.LinearSearchById(productId);
                SearchResult binaryResult = searchEngine.BinarySearchById(productId);

                totalLinearTicks += linearResult.ElapsedTicks;
                totalBinaryTicks += binaryResult.ElapsedTicks;
                totalLinearComparisons += linearResult.ComparisonsCount;
                totalBinaryComparisons += binaryResult.ComparisonsCount;

                if (linearResult.Found) linearFoundCount++;
                if (binaryResult.Found) binaryFoundCount++;

                double timeDifference = linearResult.ElapsedTicks > 0 ? 
                    (double)linearResult.ElapsedTicks / binaryResult.ElapsedTicks : 1.0;

                Console.WriteLine($"{productId,10} | {linearResult.ComparisonsCount,8} ops | {binaryResult.ComparisonsCount,8} ops | {timeDifference:F2}x");
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine();
            Console.WriteLine("SUMMARY STATISTICS:");
            Console.WriteLine($"Linear Search  - Total Operations: {totalLinearComparisons}, Found: {linearFoundCount}");
            Console.WriteLine($"Binary Search  - Total Operations: {totalBinaryComparisons}, Found: {binaryFoundCount}");
            Console.WriteLine($"Efficiency Gain: {(double)totalLinearComparisons / totalBinaryComparisons:F2}x fewer operations with binary search");
            Console.WriteLine();
        }

        public static void AnalyzeScalability()
        {
            Console.WriteLine("=== SCALABILITY ANALYSIS ===");
            Console.WriteLine();
            Console.WriteLine("Theoretical Operations Needed (Worst Case):");
            Console.WriteLine("Dataset Size | Linear Search | Binary Search | Ratio");
            Console.WriteLine(new string('-', 55));

            int[] dataSizes = { 100, 1000, 10000, 100000, 1000000 };

            foreach (int size in dataSizes)
            {
                int linearOps = size;
                int binaryOps = (int)Math.Ceiling(Math.Log2(size));
                double ratio = (double)linearOps / binaryOps;

                Console.WriteLine($"{size,11} | {linearOps,13} | {binaryOps,13} | {ratio,6:F1}x");
            }

            Console.WriteLine();
            Console.WriteLine("Key Insights:");
            Console.WriteLine("• As dataset grows, binary search advantage increases exponentially");
            Console.WriteLine("• For 1 million products: Binary search is ~50,000x more efficient");
            Console.WriteLine("• Binary search scales logarithmically - very predictable performance");
            Console.WriteLine();
        }
    }

    
    public class SampleDataGenerator
    {
        private static readonly string[] Categories = 
        {
            "Electronics", "Clothing", "Books", "Home & Garden", "Sports", 
            "Beauty", "Automotive", "Toys", "Health", "Food"
        };

        private static readonly string[] Brands = 
        {
            "Samsung", "Apple", "Nike", "Adidas", "Sony", "LG", "Dell", 
            "HP", "Canon", "Microsoft", "Google", "Amazon"
        };

        private static readonly string[] ProductNames = 
        {
            "Smartphone", "Laptop", "Headphones", "T-Shirt", "Jeans", "Novel", 
            "Cookbook", "Garden Tool", "Soccer Ball", "Lipstick", "Car Battery", 
            "Toy Car", "Vitamins", "Coffee", "Monitor", "Keyboard", "Mouse"
        };

        public static Product[] GenerateProducts(int count)
        {
            Random random = new Random(42); 
            Product[] products = new Product[count];

            for (int i = 0; i < count; i++)
            {
                int productId = i + 1;
                string category = Categories[random.Next(Categories.Length)];
                string brand = Brands[random.Next(Brands.Length)];
                string productName = $"{brand} {ProductNames[random.Next(ProductNames.Length)]} {random.Next(1000, 9999)}";
                decimal price = (decimal)(random.NextDouble() * 1000 + 10);
                int stock = random.Next(0, 100);

                products[i] = new Product(productId, productName, category, price, brand, stock);
            }

            
            for (int i = products.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Product temp = products[i];
                products[i] = products[j];
                products[j] = temp;
            }

            return products;
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("E-COMMERCE PLATFORM SEARCH FUNCTION ANALYSIS");
            Console.WriteLine("============================================");
            Console.WriteLine();

            try
            {
                // Step 1: Explain algorithmic concepts
                AlgorithmComplexityExplainer.ExplainBigONotation();
                Console.WriteLine();
                AlgorithmComplexityExplainer.ExplainSearchScenarios();
                Console.WriteLine();

                // Step 2: Generate sample data
                Console.WriteLine("=== GENERATING SAMPLE DATA ===");
                Product[] products = SampleDataGenerator.GenerateProducts(10000);
                Console.WriteLine($"Generated {products.Length} sample products");
                Console.WriteLine();

                // Step 3: Initialize search engine
                EcommerceSearchEngine searchEngine = new EcommerceSearchEngine(products);

                // Step 4: Display sample products
                Console.WriteLine("=== SAMPLE PRODUCTS ===");
                for (int i = 0; i < Math.Min(5, products.Length); i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i]}");
                }
                Console.WriteLine($"... and {products.Length - 5} more products");
                Console.WriteLine();

                // Step 5: Performance testing
                int[] testIds = { 1, 500, 2500, 5000, 7500, 9999, 15000 }; // Include non-existent ID
                PerformanceAnalyzer.CompareSearchAlgorithms(searchEngine, testIds);

                // Step 6: Scalability analysis
                PerformanceAnalyzer.AnalyzeScalability();

                // Step 7: Demonstrate different search types
                DemonstrateSearchTypes(searchEngine);

                // Step 8: Recommendations
                Console.WriteLine("=== RECOMMENDATIONS FOR E-COMMERCE PLATFORM ===");
                Console.WriteLine();
                Console.WriteLine("1. PRIMARY KEY SEARCHES (Product ID):");
                Console.WriteLine("   • Use Binary Search on sorted arrays or Hash Tables O(1)");
                Console.WriteLine("   • Maintain sorted indices for fast lookups");
                Console.WriteLine();
                Console.WriteLine("2. TEXT SEARCHES (Product Name, Description):");
                Console.WriteLine("   • Use Full-Text Search engines (Elasticsearch, Solr)");
                Console.WriteLine("   • Implement Trie data structures for autocomplete");
                Console.WriteLine();
                Console.WriteLine("3. CATEGORY/FILTER SEARCHES:");
                Console.WriteLine("   • Use Database indices and B-trees");
                Console.WriteLine("   • Consider inverted indices for multiple filters");
                Console.WriteLine();
                Console.WriteLine("4. HYBRID APPROACH:");
                Console.WriteLine("   • Combine multiple search strategies based on query type");
                Console.WriteLine("   • Cache frequently searched items");
                Console.WriteLine("   • Use asynchronous search for better user experience");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void DemonstrateSearchTypes(EcommerceSearchEngine searchEngine)
        {
            Console.WriteLine("=== DIFFERENT SEARCH TYPES DEMONSTRATION ===");
            Console.WriteLine();

            // Search by ID
            Console.WriteLine("1. Search by Product ID (Binary Search):");
            SearchResult idResult = searchEngine.BinarySearchById(2500);
            if (idResult.Found)
            {
                Console.WriteLine($"   Found: {idResult.Product}");
                Console.WriteLine($"   Operations: {idResult.ComparisonsCount}");
            }
            else
            {
                Console.WriteLine("   Product not found");
            }
            Console.WriteLine();

            // Search by name
            Console.WriteLine("2. Search by Product Name (Linear Search):");
            List<SearchResult> nameResults = searchEngine.LinearSearchByName("Samsung");
            Console.WriteLine($"   Found {nameResults.Count(r => r.Found)} products containing 'Samsung':");
            foreach (var result in nameResults.Where(r => r.Found).Take(3))
            {
                if (result.Product != null)
                {
                    Console.WriteLine($"   • {result.Product.ProductName}");
                }
            }
            if (nameResults.Count(r => r.Found) > 3)
            {
                Console.WriteLine($"   ... and {nameResults.Count(r => r.Found) - 3} more");
            }
            Console.WriteLine();

            // Search by category
            Console.WriteLine("3. Search by Category (Linear Search):");
            List<SearchResult> categoryResults = searchEngine.LinearSearchByCategory("Electronics");
            Console.WriteLine($"   Found {categoryResults.Count(r => r.Found)} products in 'Electronics' category");
            if (categoryResults.Any(r => r.Found))
            {
                Console.WriteLine($"   Total comparisons needed: {categoryResults.First().ComparisonsCount}");
            }
            Console.WriteLine();
        }
    }
}