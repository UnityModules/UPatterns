using System;
using System.Collections.Generic;

namespace UPatterns
{
    public static class ArrayHelper
    {
        public static T Find<T>(this T[] items,Func<T,bool> searchFunction)
        {
            if(searchFunction == null)
                return default;

            for(int i=0;i<items.Length;i++)
                if(searchFunction(items[i]))
                    return items[i];

            return default;
        }

        public static T[] FindAll<T>(this T[] items,Func<T,bool> searchFunction)
        {
            if(searchFunction == null)
                return default;

            List<T> lst = new();

            for(int i=0;i<items.Length;i++)
                if(searchFunction(items[i]))
                    lst.Add(items[i]);

            return lst.ToArray();
        }

        public static ReturnType[] Map<ArrayType,ReturnType>(this ArrayType[] items,Func<ArrayType,ReturnType> action)
        {
            if(action == null)
                return null;

            ReturnType[] newArray = new ReturnType[items.Length];

            for(int i=0;i<items.Length;i++)
                newArray[i] = action(items[i]);

            return newArray;
        }

        public static ReturnType[] Map<ArrayType,ReturnType>(this ArrayType[] items,Func<ArrayType,int,ReturnType> action)
        {
            if(action == null)
                return null;

            ReturnType[] newArray = new ReturnType[items.Length];

            for(int i=0;i<items.Length;i++)
                newArray[i] = action(items[i],i);

            return newArray;
        }

        public static void ForEach<T>(this T[] items,Action<T> action)
        {
            if(action == null)
                return;

            for(int i=0;i<items.Length;i++)
                action?.Invoke(items[i]);
        }

        public static void ForEach<T>(this T[] items,Action<T,int> action)
        {
            if(action == null)
                return;

            for(int i=0;i<items.Length;i++)
                action?.Invoke(items[i],i);
        }

        public static T[] Range<T>(this T[] items,int startIndex,int endIndex)
        {
            if(startIndex < 0 || startIndex > items.Length ||
            endIndex <= startIndex || endIndex > items.Length)
                return null;

            var rangeItems = new T[endIndex - startIndex];

            for(int i=0;i<rangeItems.Length;i++)
                rangeItems[i] = items[startIndex+i];

            return rangeItems;
        }

        public static List<T> ToList<T>(this T[] items)
        {
            var lst = new List<T>();

            for(int i=0;i<items.Length;i++)
                lst.Add(items[i]);

            return lst;
        }
    }
}