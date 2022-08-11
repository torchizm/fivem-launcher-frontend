using LaunchwaresCore;
using System;
using System.Linq;

namespace LaunchwaresTestApplication
{
    class Program
    {
        internal static Models.Server server = new Models.Server();
        internal static Database client { get; set; }

        static void Main(string[] args)
        {
            bool debug = args.Where(x => x == "-debug" || x == "-d" || x == "--debug").Select(x => x).FirstOrDefault() != null ? true : false;
            client = new Database("MtHZEQ1gvK8jQ6gwscLOJFX8bpqgaCTi", debug);
            client.OnTokenSync += Client_OnTokenSync;
            client.OnError += Client_OnError;

            Console.ReadKey();
        }

        //private static async void saas()
        //{
        //    Console.WriteLine($"{a.Message}");
        //}

        private static void Client_OnError(Models.ErrorMessage source, ErrorCode errorCode)
        {
            if (source.Message != "") Console.WriteLine($"Error: {source.Message} - Code: {(int)Error.FromMessage(source.Message)}");
        }

        private static void Client_OnTokenSync(Models.Token source)
        {
            Console.WriteLine("TOKEN SYNCED\n----------------------");
        }
    }
}
