using System;

namespace TestAsync
{
    public class MyAction
    {
        public string Source { get; set; }

        public MyAction(string source)
        {
            Source = source;
        }

        public string DoAction()
        {
            Thread.Sleep(1000); //better task.Delay
            return Source;
        }
    }

    class TestTask
    {
        static async Task Main(string[] args)
        {
            DateTime a, b;

            //sync - blocking the main thread
            Console.WriteLine("Do");
            a = DateTime.Now;

            Console.WriteLine(DoActionTask("task 1a").Result);
            Console.WriteLine(DoActionTask("task 1b").Result);

            b = DateTime.Now;
            Console.WriteLine("duration: " + (b - a));


            //async w.r.t. the main thread but not concurrent
            Console.WriteLine("DoAsyncNotConcurrent");
            a = DateTime.Now;

            string sa = await DoActionTaskAsync("task 2a");
            Console.WriteLine(sa);
            string sb = await DoActionTaskAsync("task 2b");
            Console.WriteLine(sb);

            b = DateTime.Now;
            Console.WriteLine("duration: " + (b - a));


            //async w.r.t. the main thread &  concurrent
            Console.WriteLine("DoAsyncConcurrent");
            a = DateTime.Now;

            Task<string> tra = DoActionTaskAsync("task 2a");
            Task<string> trb = DoActionTaskAsync("task 2b");

            await tra;
            Console.WriteLine(tra.Result);
            await trb;
            Console.WriteLine(trb.Result);

            b = DateTime.Now;
            Console.WriteLine("duration: " + (b - a));
        }

        public static Task<string> DoActionTask(string what)
        {
            MyAction action = new MyAction(what);
            return Task.Run(action.DoAction);// Task.Run(() => action.DoAction());
        }

        public async static Task<string> DoActionTaskAsync(string what)
        {
            MyAction action = new MyAction(what);
            return await Task.Run(action.DoAction);
        }


    }

}

