using Alachisoft.NCache.Client;
using Alachisoft.NCache.Common.Mirroring;
using Alachisoft.NCache.Licensing.DOM;
using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  
using SampleData;

namespace BulkOperations
{
    internal class BulkOperations
    {
        private static ICache _cache;
        private static string cacheName = "MirroredNet";
        public static void run()
        {
            InitializeCache();

        }

        private static void InitializeCache()
        {
            try
            {

                // Connect to cache
                _cache = CacheManager.GetCache(cacheName);
                Console.WriteLine("Successfully Connected to " + cacheName);
            }
            catch (OperationFailedException ex)
            {
                // NCache specific exception
                if (ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                {
                    // Make sure NCache Service and cache is running
                }
                else
                {
                    // Exception can occur due to:
                    // Connection Failures
                    // Operation Timeout
                    // Operation performed during state transfer
                }
            }
            catch (ConfigurationException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.SERVER_INFO_NOT_FOUND)
                {
                    // client.ncconf must have server information
                }
            }
            catch (Exception ex)
            {
                // Any generic exception like ArgumentNullException or ArgumentException
                // Argument exception occurs in case of empty string name
                // Make sure TLS is enabled on both client and server
            }
        }

        private static void AddMultipleObjectsToCache(IDictionary<string, CacheItem> items)
        {
            // Adding Bulk Items
            // Bulk operation returns 
            IDictionary<string, Exception> result = _cache.AddBulk(items);

            if (result.Count == 0)
            {
                // Print output on console
                Console.WriteLine("\nAll items are successfully added to cache.");
            }
            else
            {
                // Print output on console
                Console.WriteLine("\nOne or more items could not be added to the cache. Iterate hashmap for details.");
                // Iterate hashmap
                for (IEnumerator iter = result.Values.GetEnumerator(); iter.MoveNext();)
                {
                    SampleData.Product product = (SampleData.Product)iter;
                    PrintProductDetails(product);
                }
            }
        }

        private static void UpdateMultipleObjectsInCache(string[] keys, IDictionary<string, CacheItem> items)
        {
            // Updating Bulk Items
            // Previous products have their product id, class and category changed.
            items[keys[0]].GetValue<SampleData.Product>().ClassName = "ClassB";
            items[keys[1]].GetValue<SampleData.Product>().ClassName = "ClassC";
            items[keys[2]].GetValue<SampleData.Product>().ClassName = "ClassA";
            items[keys[3]].GetValue<SampleData.Product>().ClassName = "ClassD";

            IDictionary<string, Exception> result = _cache.InsertBulk(items);

            if (result.Count == 0)
            {
                // Print output on console
                Console.WriteLine("\nAll items are successfully updated in cache.");
            }
            else
            {
                // Print output on console
                Console.WriteLine("\nOne or more items could not be added to the cache. Iterate hashmap for details.");
                // Iterate hashmap
                for (IEnumerator iter = result.Values.GetEnumerator(); iter.MoveNext();)
                {
                    SampleData.Product product = (SampleData.Product)iter;
                    PrintProductDetails(product);
                }
            }
        }

        private static void GetMultipleObjectsFromCache(string[] keys)
        {
            // Getting bulk items
            IDictionary<string, SampleData.Product> items = _cache.GetBulk<SampleData.Product>(keys);

            if (items.Count > 0)
            {
                // Print output on console
                Console.WriteLine("\nFollowing items are fetched from cache.");

                for (IEnumerator iter = items.Values.GetEnumerator(); iter.MoveNext();)
                {
                    SampleData.Product product = (SampleData.Product)iter.Current;
                    PrintProductDetails(product);
                }
            }
        }

        private static void RemoveMultipleObjectsFromCache(string[] keys)
        {
            // Remove bulk from cache and return removed objects as out parameter
            _cache.RemoveBulk(keys);

            // Print output on console
            Console.WriteLine("\nItems deleted from cache. ");
        }

        private static void OutRemoveMultipleObjectsFromCache(string[] keys)
        {
            // Remove Bulk operation
            // Remove returns all the items removed from the cache in them form of Hashmap
            IDictionary<string, SampleData.Product> result;

            _cache.RemoveBulk<SampleData.Product>(keys, out result);

            if (result.Count == 0)
            {
                // Print output on console
                Console.WriteLine("\nNo items removed from the cache against the provided keys.");
            }
            else
            {
                // Print output on console
                Console.WriteLine("\nFollowing items have been removed from the cache:");
                // Iterate hashmap
                for (IEnumerator iter = result.Values.GetEnumerator(); iter.MoveNext();)
                {
                    SampleData.Product product = (SampleData.Product)iter.Current;
                    PrintProductDetails(product);
                }
            }
        }

        private static SampleData.Product[] CreateNewProducts()
        {
            SampleData.Product[] products = new SampleData.Product[4];

            products[0] = new SampleData.Product() { Id = 1, Name = "Dairy Milk Cheese", ClassName = "ClassA", Category = "Edibles" };
            products[1] = new SampleData.Product() { Id = 2, Name = "American Butter", ClassName = "ClassA", Category = "Edibles" };
            products[2] = new SampleData.Product() { Id = 3, Name = "Walmart Delicious Cream", ClassName = "ClassA", Category = "Edibles" };
            products[3] = new SampleData.Product() { Id = 4, Name = "Nestle Yogurt", ClassName = "ClassA", Category = "Edibles" };

            return products;
        }

        private static string[] GetKeys(SampleData.Product[] products)
        {
            string[] keys = new string[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                keys[i] = string.Format("Customer:{0}", products[i].Id);
            }

            return keys;
        }

        private static IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, SampleData.Product[] products)
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();
            CacheItem cacheItem = null;

            for (int i = 0; i < products.Length; i++)
            {
                cacheItem = new CacheItem(products[i]);
                items.Add(keys[i], cacheItem);
            }

            return items;
        }

        private static void PrintProductDetails(SampleData.Product product)
        {
            Console.WriteLine("Id:       " + product.Id);
            Console.WriteLine("Name:     " + product.Name);
            Console.WriteLine("Class:    " + product.ClassName);
            Console.WriteLine("Category: " + product.Category);
            Console.WriteLine();
        }



    }
}
