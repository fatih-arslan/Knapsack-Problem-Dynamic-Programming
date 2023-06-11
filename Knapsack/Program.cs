using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class Item
{
    public int Value { get; set; }
    public int Weight { get; set; }
}

public class KnapsackSolver
{
    public static Tuple<int, List<Item>> Solve(List<Item> items, int capacity)
    {
        int n = items.Count;
        int[] table = new int[capacity + 1];

        for (int i = 0; i < n; i++)
        {
            int value = items[i].Value;
            int weight = items[i].Weight;

            for (int j = capacity; j >= weight; j--)
            {
                table[j] = Math.Max(table[j], table[j - weight] + value);
            }
        }

        List<Item> includedItems = new List<Item>();
        int remainingCapacity = capacity;

        for (int i = n - 1; i >= 0; i--)
        {
            if (remainingCapacity >= items[i].Weight && table[remainingCapacity] == table[remainingCapacity - items[i].Weight] + items[i].Value)
            {
                includedItems.Add(items[i]);
                remainingCapacity -= items[i].Weight;
            }
        }

        return Tuple.Create(table[capacity], includedItems);
    }
}

public class KnapSack
{    
    public static void Main(string[] args)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string relativePath = @"..\..\..\inputFiles\ks_4_0.txt";

        string filePath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
        
        List<string> inputLines = new List<string>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while (!String.IsNullOrEmpty((line = reader.ReadLine())))
            {
                inputLines.Add(line);
            }
        }

        
        string[] itemAndCapacity = inputLines[0].Split();          
        int item_count = int.Parse(itemAndCapacity[0]);
        int capacity = int.Parse(itemAndCapacity[1]);

        List<Item> items = new List<Item>();
        for (int i = 1; i < inputLines.Count; i++)
        {
            string[] itemData = inputLines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int value = int.Parse(itemData[0]);
            int weight = int.Parse(itemData[1]);
            items.Add(new Item { Value = value, Weight = weight });
        }       

        Tuple<int, List<Item>> result = KnapsackSolver.Solve(items, capacity);

        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;

        Console.WriteLine(result.Item1);
        foreach (Item item in items)
        {
            if (result.Item2.Contains(item))
            {
                Console.Write(1 + " ");
            }
            else
            {
                Console.Write(0 + " ");                   
            }
        } 
        Console.WriteLine();
        /*foreach(Item item in result.Item2)
        {
            Console.Write("Value: " + item.Value + " ");
            Console.Write("Weight: " + item.Weight + " ");
            Console.WriteLine() ;
        }*/
        Console.WriteLine("\nTime: " + elapsedTime.TotalMilliseconds + " milliseconds");
    }
}
