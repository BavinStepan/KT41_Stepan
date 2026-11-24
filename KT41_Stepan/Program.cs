using System;
using System.Diagnostics;

class Program
{

    const int ShortLivedCalls = 1;
    const int LongLivedCalls = 12;
    const int HeavyLohCalls = 12;

    static void Main()
    {
        Console.WriteLine("Паттерн         | Вызовов | Время, мс | Gen0 | Gen1 | Gen2 | Выделено, МБ");

        MeasureWorkload("ShortLived", Workloads.ShortLived, ShortLivedCalls);
        MeasureWorkload("LongLived", Workloads.LongLived, LongLivedCalls);
        MeasureWorkload("HeavyLoh", Workloads.HeavyLoh, HeavyLohCalls);

        Console.WriteLine();
        Console.WriteLine(" Принудительный блокирующий GC");

        double liveMb = GC.GetTotalMemory(false) / 1048576.0;
        Console.WriteLine($"Живая память перед сборкой: {liveMb:F1} МБ");

        var sw = Stopwatch.StartNew();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        sw.Stop();

        Console.WriteLine($"Блокирующая сборка Gen2 заняла: {sw.Elapsed.TotalMilliseconds:F2} мс");

        GC.KeepAlive(Workloads.KeptAlive);
        GC.KeepAlive(Workloads.LohKept);

        Console.WriteLine();
        Console.WriteLine("Готово. Нажмите Enter для выхода.");
        Console.ReadLine();
    }

    static void MeasureWorkload(string name, Action workload, int iterations)
    {
        int gen0Before = GC.CollectionCount(0);
        int gen1Before = GC.CollectionCount(1);
        int gen2Before = GC.CollectionCount(2);
        long allocatedBefore = GC.GetAllocatedBytesForCurrentThread();

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            workload();
        }
        sw.Stop();

        int gen0Diff = GC.CollectionCount(0) - gen0Before;
        int gen1Diff = GC.CollectionCount(1) - gen1Before;
        int gen2Diff = GC.CollectionCount(2) - gen2Before;
        double allocatedMb = (GC.GetAllocatedBytesForCurrentThread() - allocatedBefore) / 1048576.0;

        Console.WriteLine($"{name,-15} | {iterations,7} | {sw.Elapsed.TotalMilliseconds,9:F1} | {gen0Diff,4} | {gen1Diff,4} | {gen2Diff,4} | {allocatedMb,12:F1}");
    }
}
