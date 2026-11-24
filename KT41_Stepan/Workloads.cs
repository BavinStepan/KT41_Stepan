using System;
using System.Collections.Generic;

public static class Workloads
{

    public static readonly List<byte[]> KeptAlive = new List<byte[]>();
    public static readonly List<byte[]> LohKept = new List<byte[]>();

    private static long _checksum;

    public static void ShortLived()
    {
        for (int i = 0; i < 300_000; i++)
        {
            var b = new byte[200];
            _checksum += b[0];
        }
    }

    public static void LongLived()
    {
        List<byte[]> list = KeptAlive;
        for (int i = 0; i < 5_000; i++)
        {
            list.Add(new byte[512]);
        }
    }

    public static void HeavyLoh()
    {
        List<byte[]> list = LohKept;
        for (int i = 0; i < 20; i++)
        {
            list.Add(new byte[100_000]);
        }
    }
}
