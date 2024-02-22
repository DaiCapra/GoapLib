using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GoapLib.States;

public class State<TK, TV> : IEquatable<State<TK, TV>>
{
    public readonly Dictionary<TK, TV> map;
    private int _hashCode;

    public TV this[TK key]
    {
        get => map[key];
        set
        {
            map[key] = value;
            UpdateHashCode();
        }
    }

    public State()
    {
        map = new();
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

    public bool Equals(State<TK, TV> other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _hashCode == other._hashCode;
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
        // Todo: Implement a faster hash function
        var json = JsonConvert.SerializeObject(map);

        using var sha256 = SHA256.Create();
        var encoded = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        _hashCode = BitConverter.ToInt32(encoded, 0) % 1000000;
    }
}