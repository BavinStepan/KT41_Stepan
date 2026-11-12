using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Паттерн         | Время | Gen0 | Gen1 | Gen2");

        MeasureWorkload("ShortLived", Workloads.ShortLived);
        MeasureWorkload("LongLived", Workloads.LongLived);
        MeasureWorkload("HeavyLoh", Workloads.HeavyLoh);

        Console.WriteLine("\n Принудительный блокирующий GC");
        var sw = Stopwatch.StartNew();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        sw.Stop();
        Console.WriteLine($"Блокирующая сборка Gen2 заняла: {sw.ElapsedMilliseconds} мс");
    }

    static void MeasureWorkload(string name, Action workload)
    {
        int gen0Before = GC.CollectionCount(0);
        int gen1Before = GC.CollectionCount(1);
        int gen2Before = GC.CollectionCount(2);

        var sw = Stopwatch.StartNew();
        workload();
        sw.Stop();

        int gen0Diff = GC.CollectionCount(0) - gen0Before;
        int gen1Diff = GC.CollectionCount(1) - gen1Before;
        int gen2Diff = GC.CollectionCount(2) - gen2Before;

        Console.WriteLine($"{name,-15} | {sw.ElapsedMilliseconds,5} | {gen0Diff,4} | {gen1Diff,4} | {gen2Diff,4}");
    }
}