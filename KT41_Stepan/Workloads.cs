using System;
using System.Collections.Generic;

public static class Workloads
{
    public static void ShortLived()
    {
        for (int i = 0; i < 300_000; i++)
        {
            var b = new byte[200];
        }
    }

    public static void LongLived()
    {
        var list = new List<byte[]>();
        for (int i = 0; i < 5_000; i++)
        {
            list.Add(new byte[512]);
        }
        GC.KeepAlive(list);
    }

    public static void HeavyLoh()
    {
        var list = new List<byte[]>();
        for (int i = 0; i < 20; i++)
        {
            list.Add(new byte[100_000]);
        }
        GC.KeepAlive(list);
    }
}