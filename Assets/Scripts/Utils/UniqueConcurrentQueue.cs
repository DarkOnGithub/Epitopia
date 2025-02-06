using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Utils
{
    public class UniqueConcurrentQueue<T>
    {


        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private readonly ConcurrentDictionary<T, bool> set = new ConcurrentDictionary<T, bool>();

        public bool Enqueue(T item)
        {
            if (set.TryAdd(item, true)) 
            {
                queue.Enqueue(item);
                return true; 
            }
            return false; 
        }

        public bool TryDequeue(out T item)
        {
            if (queue.TryDequeue(out item))
            {
                set.TryRemove(item, out _);
                return true;
            }
            return false;
        }

        public bool Contains(T item) => set.ContainsKey(item);

        public int Count => queue.Count;

        public void Clear()
        {
            while (queue.TryDequeue(out _)) { }
            set.Clear();
        }
    }
}