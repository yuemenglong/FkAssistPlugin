using System.Collections.Generic;

namespace FkAssistPlugin
{
    public static class Fp
    {
//        public delegate void Action<in T>(T obj);
        public delegate TR MapFn<in T, out TR>(T t);

        public static TR[] Map<T, TR>(this T[] arr, MapFn<T, TR> fn)
        {
            var ret = new TR[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ret[i] = fn(arr[i]);
            }
            return ret;
        }

        public delegate TR[] FlatMapFn<in T, out TR>(T t);

        public static TR[] FlatMap<T, TR>(this T[] arr, FlatMapFn<T, TR> fn)
        {
            var ret = new List<TR>();
            for (int i = 0; i < arr.Length; i++)
            {
                fn(arr[i]).Foreach(o => { ret.Add(o); });
            }
            return ret.ToArray();
        }

        public delegate bool FilterFn<in T>(T t);

        public static T[] Filter<T>(this T[] arr, FilterFn<T> fn)
        {
            var ret = new List<T>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (fn(arr[i]))
                {
                    ret.Add(arr[i]);
                }
            }
            return ret.ToArray();
        }

        public delegate void ForeachFn<in T>(T t);

        public static void Foreach<T>(this T[] arr, ForeachFn<T> fn)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                fn(arr[i]);
            }
        }
    }
}