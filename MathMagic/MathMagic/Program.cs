using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MathMagic
{
    class Program
    {
        static void Main(string[] args)
        {
            long[] arrayContainingTimeForQuestions = new long[1000];
            string specificFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MathMagic");

            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MathMagic\\MathMagic.dat");

            if (File.Exists(fileName))
            {
                arrayContainingTimeForQuestions = (long[])ConvertByteArrayToObject(File.ReadAllBytes(fileName));
            }

            Console.Write("Questions : ");
            Stopwatch sw = new Stopwatch();

            int numberOfQuestions;
            try
            {
                numberOfQuestions = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("input have to be a number!");
                Console.ReadKey();
                return;
            }

            int counter = numberOfQuestions;
            int rightAnswers = 0;
            int wrongAnswers = 0;
            long penaltyTime = 0;
            Console.WriteLine();
            sw.Start();

            for (int i = 0; i < counter; i++)
            {
                Random seed = new Random();

                int a = seed.Next(1, 9);
                int b = seed.Next(1, 9);

                Console.Write($"{a}x{b}=");

                int res;
                try
                {
                    res = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("input have to be a number!");
                    Console.ReadKey();
                    continue;
                }

                if (res == a * b)
                {
                    rightAnswers++;
                    Console.WriteLine("right");
                }
                else
                {
                    wrongAnswers++;
                    penaltyTime += 2000;
                    Console.WriteLine($"wrong ({a * b})");
                }
            }

            sw.Stop();
            Console.WriteLine();
            if (sw.ElapsedMilliseconds + penaltyTime < arrayContainingTimeForQuestions[numberOfQuestions] || arrayContainingTimeForQuestions[numberOfQuestions] == 0)
            {
                arrayContainingTimeForQuestions[numberOfQuestions] = sw.ElapsedMilliseconds + penaltyTime;
                Console.WriteLine("new record!");
            }

            Console.WriteLine($"correct : {rightAnswers}.");
            Console.WriteLine($"wrong : {wrongAnswers}.");
            Console.WriteLine($"time : {sw.ElapsedMilliseconds + penaltyTime} ms.");

            File.WriteAllBytes(fileName, ConvertObjectToByteArray(arrayContainingTimeForQuestions));
            Console.ReadKey();
        }

        public static byte[] ConvertObjectToByteArray(object ob)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, ob);
            return ms.ToArray();
        }

        public static object ConvertByteArrayToObject(byte[] ba)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new MemoryStream(ba);
            return bf.Deserialize(stream);
        }
    }
}
