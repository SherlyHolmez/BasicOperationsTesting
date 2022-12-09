using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleData;
using Alachisoft.NCache.Runtime.Caching;
using System.Collections;

namespace BasicOperationsTesting
{
    internal class BasicOperations
    {

        private static ICache _cache;
        private static string cacheName = "MirroredNet";

        public static void run()
        {
            Console.WriteLine("Starting Run.");
            InitializeCache(); 

            Customer customer = CreateNewCustomer();

            string key;

            //_cache.Clear();

            for (int j = 0; j <= 1000; j++)
            {
                key = j.ToString();
                //PrintCustomerDetails(customer);
                //AddObjectToCache(key, customer);
                //GetObjectFromCache(key);
                //Console.Write(iteratecache());
                
            }




            Console.WriteLine("\nSuccessfully Exited Run.");
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

        private static void AddObjectToCache(string key, Customer customer)
        {
            TimeSpan expirationInterval = new TimeSpan(0, 1, 0);

            Expiration expiration = new Expiration(ExpirationType.Absolute);
            expiration.ExpireAfter = expirationInterval;

            //Populating cache item
            CacheItem item = new CacheItem(customer);
            item.Expiration = expiration;

            // Adding cacheitem to cache with an absolute expiration of 1 minute
            _cache.Add(key, item);

            // Print output on console
            Console.WriteLine("\nObject is added to cache.");
        }

        private static void AddObjectToCache_Async(string key, Customer customer)
        {
            TimeSpan expirationInterval = new TimeSpan(0, 1, 0);

            Expiration expiration = new Expiration(ExpirationType.Absolute);
            expiration.ExpireAfter = expirationInterval;

            //Populating cache item
            CacheItem item = new CacheItem(customer);
            item.Expiration = expiration;

            // Adding cacheitem to cache with an absolute expiration of 1 minute
            _cache.AddAsync(key, item);

            // Print output on console
            Console.WriteLine("\nObject is added to cache.");
        }

        private static Customer GetObjectFromCache(string key)
        {
            Customer cachedCustomer = _cache.Get<Customer>(key);

            Console.WriteLine("\n Object found and being fetched from cache");

            PrintCustomerDetails(cachedCustomer);

            return cachedCustomer;
        }

        private static void PrintCustomerDetails(Customer customer)
        {
            if (customer == null) return;

            Console.WriteLine();
            Console.WriteLine("CustomerID: " + customer.CustomerID);
            Console.WriteLine("Customer Details are as follows: ");
            Console.WriteLine("ContactName: " + customer.ContactName);
            Console.WriteLine("CompanyName: " + customer.CompanyName);
            Console.WriteLine("Contact No: " + customer.ContactNo);
            Console.WriteLine("Address: " + customer.Address);
            Console.WriteLine();
        }

        private static string GetKey(Customer customer)
        {
            return string.Format("Customer:{0}", customer.CustomerID);
        }

        private static void UpdateObjectInCache(string key, Customer customer)
        {
            // Update item with a sliding expiration of 30 seconds
            customer.CompanyName = "Gourmet Lanchonetes";

            TimeSpan expirationInterval = new TimeSpan(0, 0, 30);

            Expiration expiration = new Expiration(ExpirationType.Sliding);
            expiration.ExpireAfter = expirationInterval;

            CacheItem item = new CacheItem(customer);
            item.Expiration = expiration;

            _cache.Insert(key, customer);

            // Print output on console
            Console.WriteLine("\nObject is updated in cache.");
        }

        private static void RemoveObjectFromCache(string key)
        {
            // Remove the existing customer
            _cache.Remove(key);

            // Print output on console
            Console.WriteLine("\nObject is removed from cache.");
        }

        private static Customer CreateNewCustomer()
        {
            return new Customer
            {
                //CustomerID = "SHAHZ",
                ContactName = "Shahzeb Nasir",
                CompanyName = "Alachisoft",
                ContactNo = "12345-6789",
                Address = "Silicon Valley, Santa Clara, California",
            };
        }

        private static string[] iteratecache()
        {
            List<string> keys = new List<string>();
            IEnumerator x = _cache.GetEnumerator();
            int i = 0;
            while (x.MoveNext())
            {
                DictionaryEntry item = (DictionaryEntry)x.Current;
                keys.Add(item.Key.ToString());
                i++;
            }
            string[] keyarr = keys.ToArray();
            return keyarr;
        }
    }
}
