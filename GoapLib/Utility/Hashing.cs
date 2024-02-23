using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GoapLib;

public class Hashing
{
    private static readonly ConcurrentStack<MD5> Stack = new();

    public static string GetHash<TK, TV>(Dictionary<TK, TV> dictionary)
    {
        var keys = dictionary.Keys.OrderBy(t => t);
        var sb = new StringBuilder();
        
        foreach (var key in keys)
        {
            sb.Append(key + ":" + dictionary[key] + "|");
        }

        var s = sb.ToString();
        var bytes = Encoding.UTF8.GetBytes(s);

        if (!Stack.TryPop(out var md5))
        {
            md5 = MD5.Create();
        }

        var encoded = md5.ComputeHash(bytes);

        var hash = BitConverter.ToString(encoded);
        Stack.Push(md5);

        return hash;
    }
}