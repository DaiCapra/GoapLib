using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GoapLib.States;

public class Hashing
{
    private static readonly MD5 Md5 = new MD5CryptoServiceProvider();

    public static byte[] ComputeHash(byte[] bytes)
    {
        return Md5.ComputeHash(bytes);
    }
}

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

    public bool CanApply(State<TK, TV> state)
    {
        foreach (var kv in state.map)
        {
            if (!map.ContainsKey(kv.Key) || !map[kv.Key].Equals(kv.Value))
            {
                return false;
            }
        }

        return true;
    }

    public State<TK, TV> Copy()
    {
        var state = new State<TK, TV>();

        foreach (var kv in map)
        {
            state.Set(kv.Key, kv.Value, updateHashCode: false);
        }

        state._hashCode = _hashCode;
        return state;
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
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return _hashCode;
    }

    public void Remove(TK key, bool updateHashCode = true)
    {
        map.Remove(key);

        if (updateHashCode)
        {
            UpdateHashCode();
        }
    }

    public void Set(TK key, TV value, bool updateHashCode = true)
    {
        map[key] = value;

        if (updateHashCode)
        {
            UpdateHashCode();
        }
    }

    public void UpdateHashCode()
    {
        var json = JsonConvert.SerializeObject(map);

        var bytes = Encoding.UTF8.GetBytes(json);
        var encoded = Hashing.ComputeHash(bytes);

        var hash = Encoding.UTF8.GetString(encoded);
        _hashCode = hash.GetHashCode();
    }
}