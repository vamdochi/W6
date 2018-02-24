using System;
using System.Linq.Expressions;

public static class CastTo<T>
{
    public static T From<S>(S s)
    {
        return Cache<S>.caster(s);
    }

    private static class Cache<S>
    {
        private static Func<S, T> Get()
        {
            var p = Expression.Parameter(typeof(S), "s");
            var c = Expression.ConvertChecked(p, typeof(T));
            return Expression.Lambda<Func<S, T>>(c, p).Compile();
        }

        public static readonly Func<S, T> caster = Get();
    }
}

public class EnumArray<TValue>
{
    private TValue[] _data;

    public EnumArray(Enum enumId)
    {
        _data = new TValue[CastTo<int>.From(enumId)];
    }

    public TValue this[Enum enumId]
    {
        get { return _data[CastTo<int>.From(enumId)]; }
        set { _data[CastTo<int>.From(enumId)] = value; }
    }

    public TValue this[int index]
    {
        get { return _data[index]; }
        set { _data[index] = value; }
    }

    public int Length  { get { return _data.Length; } }
}