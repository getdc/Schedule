using System;
using System.Collections.Generic;
using System.Threading;

namespace ZOS
{
    public class LRUDemo
    {
        public void Run()
        {
            double sget;
            LRUCache<double, string> lRUCache = new LRUCache<double, string>();
            for (int i = 1; i <= 5; i++)
            {
                lRUCache.Set(i, "User" + i.ToString() + " - Init Message");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine("请输入进程Id(例如 1)，小于等于0结束进程:");
                    sget = Convert.ToDouble(Console.ReadLine());
                    if (sget <= 0) break;
                    lRUCache.Set(sget, DateTime.Now.ToString());
                    Console.WriteLine("最近访问Task如下：" + lRUCache.GetLinkedKey());
                }
                catch
                {
                    Console.WriteLine("输入错误");
                }
            }
        }
    }

    public class LRUCache<TKey, TValue>
    {
        const int DEFAULT_CAPACITY = 255;

        int _capacity;
        ReaderWriterLockSlim _locker;
        IDictionary<TKey, TValue> _dictionary;
        LinkedList<TKey> _linkedList;

        public LRUCache() : this(DEFAULT_CAPACITY) { }

        public LRUCache(int capacity)
        {
            _locker = new ReaderWriterLockSlim();
            _capacity = capacity > 0 ? capacity : DEFAULT_CAPACITY;
            _dictionary = new Dictionary<TKey, TValue>();
            _linkedList = new LinkedList<TKey>();
        }

        public void Set(TKey key, TValue value)
        {
            _locker.EnterWriteLock();
            try
            {
                _dictionary[key] = value;
                _linkedList.Remove(key);
                _linkedList.AddFirst(key);
                if (_linkedList.Count > _capacity)
                {
                    _dictionary.Remove(_linkedList.Last.Value);
                    _linkedList.RemoveLast();
                }
            }
            finally { _locker.ExitWriteLock(); }
        }

        public string GetLinkedKey()
        {
            string sList = "";
            foreach(TKey key in _linkedList)
            {
                sList += (key +  ", ");
            }
            return sList;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            _locker.EnterUpgradeableReadLock();
            try
            {
                bool b = _dictionary.TryGetValue(key, out value);
                if (b)
                {
                    _locker.EnterWriteLock();
                    try
                    {
                        _linkedList.Remove(key);
                        _linkedList.AddFirst(key);
                    }
                    finally { _locker.ExitWriteLock(); }
                }
                return b;
            }
            catch { throw; }
            finally { _locker.ExitUpgradeableReadLock(); }
        }

        public bool ContainsKey(TKey key)
        {
            _locker.EnterReadLock();
            try
            {
                return _dictionary.ContainsKey(key);
            }
            finally { _locker.ExitReadLock(); }
        }

        public int Count
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Count;
                }
                finally { _locker.ExitReadLock(); }
            }
        }

        public int Capacity
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _capacity;
                }
                finally { _locker.ExitReadLock(); }
            }
            set
            {
                _locker.EnterUpgradeableReadLock();
                try
                {
                    if (value > 0 && _capacity != value)
                    {
                        _locker.EnterWriteLock();
                        try
                        {
                            _capacity = value;
                            while (_linkedList.Count > _capacity)
                            {
                                _linkedList.RemoveLast();
                            }
                        }
                        finally { _locker.ExitWriteLock(); }
                    }
                }
                finally { _locker.ExitUpgradeableReadLock(); }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Keys;
                }
                finally { _locker.ExitReadLock(); }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Values;
                }
                finally { _locker.ExitReadLock(); }
            }
        }
    }

}
