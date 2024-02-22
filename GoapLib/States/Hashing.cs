using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GoapLib.States;

public class Hashing
{
    private static readonly MD5 Md5 = new MD5CryptoServiceProvider();
    
    private static byte[] ComputeHash(byte[] bytes)
    {
        return Md5.ComputeHash(bytes);
    }

    public static string GetHash<TK, TV>(Dictionary<TK, TV> dictionary)
    {
        var sb = new StringBuilder();
        foreach (var kv in dictionary)
        {
            sb.Append(kv.Key + ":" + kv.Value + "|");
        }

        var s = sb.ToString();
        var bytes = Encoding.UTF8.GetBytes(s);
        var encoded = ComputeHash(bytes);

        return BitConverter.ToString(encoded);
    }
}