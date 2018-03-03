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

public class EnumArray<TValue> where TValue : new()
{
    private TValue[] _data;

    public void Allocate<TEnum>(TEnum T) 
    {
        _data = new TValue[CastTo<int>.From(T)];

        for( int i = 0; i < _data.Length; ++i )
        {
            _data[i] = new TValue();
        }
    }

    public TValue Get<TEnum>( TEnum T)
    {
        return _data[CastTo<int>.From(T)];
    }

    public TValue this[int index]
    {
        get { return _data[index]; }
        set { _data[index] = value; }
    }

    public int Length  { get { if( _data != null ) return _data.Length; return 0; } }
}