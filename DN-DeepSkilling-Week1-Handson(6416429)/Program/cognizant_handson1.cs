using System;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonPatternExample
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        
        private Logger()
        {
            Console.WriteLine("Logger instance created at: " + DateTime.Now);
        }
        
        public static Logger Instance => _instance.Value;
        
        public void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
        
        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
        
        public void LogWarning(string message)
        {
            Console.WriteLine($"[WARNING] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
        
        public string GetInstanceId()
        {
            return this.GetHashCode().ToString();
        }
    }
    
    public class SingletonTest
    {
        public static void TestSingletonImplementation()
        {
            Console.WriteLine("=== Testing Singleton Pattern Implementation ===\n");
            
            Console.WriteLine("Test 1: Basic Singleton Behavior");
            Logger logger1 = Logger.Instance;
            Logger logger2 = Logger.Instance;
            
            Console.WriteLine($"Logger1 Instance ID: {logger1.GetInstanceId()}");
            Console.WriteLine($"Logger2 Instance ID: {logger2.GetInstanceId()}");
            Console.WriteLine($"Are both instances the same? {ReferenceEquals(logger1, logger2)}");
            
            Console.WriteLine("\nTest 2: Logger Functionality");
            logger1.LogInfo("This is an info message from logger1");
            logger2.LogError("This is an error message from logger2");
            logger1.LogWarning("This is a warning message from logger1");
            
            Console.WriteLine("\nTest 3: Thread Safety Test");
            TestThreadSafety();
            
            Console.WriteLine("\n=== All Tests Completed ===");
        }
        
        private static void TestThreadSafety()
        {
            const int numberOfThreads = 5;
            Logger[] loggerInstances = new Logger[numberOfThreads];
            Task[] tasks = new Task[numberOfThreads];
            
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadIndex = i;
                tasks[i] = Task.Run(() =>
                {
                    Thread.Sleep(new Random().Next(1, 100));
                    loggerInstances[threadIndex] = Logger.Instance;
                    Console.WriteLine($"Thread {threadIndex + 1} - Logger Instance ID: {loggerInstances[threadIndex].GetInstanceId()}");
                });
            }
            
            Task.WaitAll(tasks);
            
            bool allInstancesAreSame = true;
            for (int i = 1; i < numberOfThreads; i++)
            {
                if (!ReferenceEquals(loggerInstances[0], loggerInstances[i]))
                {
                    allInstancesAreSame = false;
                    break;
                }
            }
            
            Console.WriteLine($"Thread safety test result: {(allInstancesAreSame ? "PASSED" : "FAILED")}");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Singleton Pattern Example - Logger Implementation");
            Console.WriteLine("================================================\n");
            
            SingletonTest.TestSingletonImplementation();
            
            Console.WriteLine("\n=== Practical Usage Example ===");
            
            Logger appLogger = Logger.Instance;
            appLogger.LogInfo("Application started");
            appLogger.LogInfo("Processing user request");
            appLogger.LogWarning("Low memory warning");
            appLogger.LogError("Database connection failed");
            appLogger.LogInfo("Application shutting down");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}