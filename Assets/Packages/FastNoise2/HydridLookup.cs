using System;

namespace Packages.FastNoise2
{
    [Serializable]
    public class HybridLookup<T1, T2>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly bool _isItem1;

        public HybridLookup(T1 item1)
        {
            _item1 = item1;
            _isItem1 = true;
        }

        public HybridLookup(T2 item2)
        {
            _item2 = item2;
            _isItem1 = false;
        }

        public bool IsItem1 => _isItem1;
        public bool IsItem2 => !_isItem1;

        public T1 Item1
        {
            get
            {
                if (!_isItem1) throw new InvalidOperationException("Item is not of type T1");
                return _item1;
            }
        }

        public T2 Item2
        {
            get
            {
                if (_isItem1) throw new InvalidOperationException("Item is not of type T2");
                return _item2;
            }
        }
    }
}