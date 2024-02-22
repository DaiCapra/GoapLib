using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace GoapLib.States;

public class State<TK, TV> : IEquatable<State<TK, TV>>
{
    public readonly Dictionary<TK, TV> map;
    private int _hashCode;

    public State()
    {
        map = new();
    }

    public TV this[TK key]
    {
        get => map[key];
        set
        {
            map[key] = value;
            UpdateHashCode();
        }
    }

    public bool Equals(State<TK, TV> other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _hashCode == other._hashCode;
    }

    public void Apply(State<TK, TV> other)
    {
        foreach (var kv in other.map)
        {
            map[kv.Key] = kv.Value;
        }

        UpdateHashCode();
    }

    public void UpdateHashCode()
    {
        // Todo: Implement a faster hash function
        var json = JsonConvert.SerializeObject(map);

        using var sha256 = SHA256.Create();
        var encoded = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        _hashCode = BitConverter.ToInt32(encoded, 0) % 1000000;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((State<TK, TV>)obj);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}