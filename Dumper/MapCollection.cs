using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Dumper
{

    /// <summary>
    /// 可以有键值重复的无序列表
    /// </summary>
    public class MapCollection : IDictionary, IDisposable
    {
        #region Internal Data
        internal Hashtable m_InternalData;
        int m_Count = 0;
        internal int m_Version = 0;
        #endregion

        #region Constructors
        public MapCollection()
        {
            m_InternalData = new Hashtable();
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// 确保 key 对应的容器存在，并返回该容器
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>key 对应的容器</returns>
        protected ArrayList MakeKeyExist(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (!m_InternalData.ContainsKey(key))
            {
                ArrayList containor = new ArrayList();
                m_InternalData.Add(key, containor);
                ++m_Version;
                return containor;
            }
            else
                return (ArrayList)m_InternalData[key];
        }

        /**/
        /// <summary>
        /// 清理部分数据内容
        /// </summary>
        protected void ParticalClear()
        {
            foreach (DictionaryEntry ent in this.m_InternalData)
                ((ArrayList)ent.Value).Clear();
            m_Count = 0;
            ++m_Version;
        }

        /**/
        /// <summary>
        /// 取得实际数据
        /// </summary>
        protected int GetCount()
        {
            int count = 0;
            foreach (DictionaryEntry ent in this.m_InternalData)
                count += ((ArrayList)ent.Value).Count;

            return count;
        }

        #endregion

        /**/
        /// <summary>
        /// 计算key对应的对象个数
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>对象个数</returns>
        public int CountKey(object key)
        {
            if (this.m_InternalData.ContainsKey(key))
                return ((ArrayList)this.m_InternalData[key]).Count;
            else
                return 0;
        }

        public void RemoveAt(object key, int index)
        {
            if (m_InternalData.ContainsKey(key))
            {
                MakeKeyExist(key).RemoveAt(index);
                --m_Count;
                ++m_Version;
            }
            else
                throw new ArgumentOutOfRangeException("index");
        }

        public object this[object key, int index]
        {
            get
            {
                if (this.m_InternalData.ContainsKey(key))
                {
                    return MakeKeyExist(key)[index];
                }
                else
                    throw new ArgumentOutOfRangeException("index");
            }
            set
            {
                MakeKeyExist(key)[index] = value;
            }
        }

        IEnumerator GetEnumerator(object key)
        {
            if (this.m_InternalData.ContainsKey(key))
                return MakeKeyExist(key).GetEnumerator();
            else
                return null;
        }

        #region Internal Enumerator

        private class MapEnumerator : IDictionaryEnumerator
        {
            MapCollection m_Owner;
            IDictionaryEnumerator master;   // master == null 尚未开始
            IEnumerator slave;
            int version;
            object current;     // current == null 已经结束

            public MapEnumerator(MapCollection owner)
            {
                m_Owner = owner;
                master = null;
                slave = null;
                current = null;
                version = owner.m_Version;
            }

            #region IDictionaryEnumerator Members
            public DictionaryEntry Entry
            {
                get
                {
                    if (master == null)
                        throw new InvalidOperationException("迭代尚未开始");
                    if (current == null)
                        throw new InvalidOperationException("迭代已经结束");
                    if (version != m_Owner.m_Version)
                        throw new InvalidOperationException("迭代期间数据已经被改变");
                    return (DictionaryEntry)current;
                }
            }

            public object Key
            {
                get { return Entry.Key; }
            }

            public object Value
            {
                get { return Entry.Value; }
            }


            #endregion

            #region IEnumerator Members
            public object Current
            {
                get { return Entry; }
            }


            public bool MoveNext()
            {
                if (version != m_Owner.m_Version)
                    throw new InvalidOperationException("迭代期间数据已经被改变");

                bool ok = true;
                if (this.master == null)
                {
                    this.master = this.m_Owner.m_InternalData.GetEnumerator();
                    ok = this.master.MoveNext();
                    if (!ok) return false;
                }

                DictionaryEntry master_current;
                while (ok)
                {
                    master_current = (DictionaryEntry)master.Current;
                    if (this.slave == null)
                        slave = m_Owner.GetEnumerator(master_current.Key);
                    if (slave != null && slave.MoveNext())
                    {
                        current = new DictionaryEntry(master_current.Key, slave.Current);
                        return true;
                    }
                    ok = this.master.MoveNext();
                    slave = null;
                }
                current = null;
                return false;
            }

            public void Reset()
            {
                master = null;
                slave = null;
                current = null;
            }

            #endregion
        }

        private class MapValueCollection : ICollection
        {
            MapCollection m_Owner;
            int m_Version;

            public MapValueCollection(MapCollection owner)
            {
                m_Owner = owner;
                m_Version = owner.m_Version;
            }

            #region ICollection Members

            public void CopyTo(Array array, int index)
            {
                foreach (DictionaryEntry ent in this.m_Owner.m_InternalData)
                {
                    ArrayList a = (ArrayList)ent.Value;
                    if (a != null)
                    {
                        a.CopyTo(array, index);
                        index += a.Count;
                    }
                }
            }

            public int Count
            {
                get { return m_Owner.Count; }
            }

            public bool IsSynchronized
            {
                get { return m_Owner.IsSynchronized; }
            }

            public object SyncRoot
            {
                get { return m_Owner.SyncRoot; }
            }


            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new MapValueEnumerator(this.m_Owner);
            }

            #endregion
        }

        private class MapValueEnumerator : IEnumerator
        {
            MapCollection m_Owner;
            IEnumerator iter;
            int version;
            object current;

            public MapValueEnumerator(MapCollection owner)
            {
                m_Owner = owner;
                version = m_Owner.m_Version;
                iter = null;
                current = null;    // 表示尚未开始:和其他枚举的标志不一样，正好相反
            }

            #region IEnumerator Members
            public object Current
            {
                get
                {
                    if (iter == null)
                        throw new InvalidOperationException("迭代尚未开始");
                    if (current == m_Owner)
                        throw new InvalidOperationException("迭代已经结束");
                    if (version != m_Owner.m_Version)
                        throw new InvalidOperationException("迭代期间数据已经被改变");
                    return current;
                }
            }

            public bool MoveNext()
            {
                if (version != m_Owner.m_Version)
                    throw new InvalidOperationException("迭代期间数据已经被改变");
                if (current == m_Owner)
                    throw new InvalidOperationException("迭代已经结束");

                if (iter == null)
                    iter = m_Owner.GetEnumerator();
                if (iter.MoveNext())
                {
                    current = ((DictionaryEntry)iter.Current).Value;
                    return true;
                }
                else
                {
                    current = m_Owner;    // 表示结束
                    return false;
                }
            }

            public void Reset()
            {
                iter = null;
                current = m_Owner;
            }

            #endregion
        }

        #endregion

        #region IDictionary Members

        public void Add(object key, object value)
        {
            MakeKeyExist(key).Add(value);
            ++m_Count;
        }

        public void Clear()
        {
            ParticalClear();
        }

        public bool Contains(object key)
        {
            return this.m_InternalData.ContainsKey(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new MapEnumerator(this);
        }

        public bool IsFixedSize
        {
            get { return this.m_InternalData.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return this.m_InternalData.IsReadOnly; }
        }

        public ICollection Keys
        {
            get { return this.m_InternalData.Keys; }
        }

        public void Remove(object key)
        {
            if (this.m_InternalData.ContainsKey(key))
            {
                ArrayList containor = MakeKeyExist(key);
                m_Count -= containor.Count;
                ++m_Version;
                containor.Clear();
                m_InternalData.Remove(key);
            }
        }

        public ICollection Values
        {
            get { return new MapValueCollection(this); }
        }

        public object this[object key]
        {
            get
            {
                if (this.m_InternalData.ContainsKey(key))
                    return this.MakeKeyExist(key)[0];
                else
                    return null;
            }

            set
            {
                if (this.m_InternalData.ContainsKey(key))
                    this.MakeKeyExist(key)[0] = value;
                else
                    this.MakeKeyExist(key).Add(value);
                ++m_Version;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            int i;
            ArrayList list;
            foreach (DictionaryEntry ent in this.m_InternalData)
            {
                list = (ArrayList)ent.Value;
                for (i = 0; i < list.Count; ++i)
                    array.SetValue(new DictionaryEntry(ent.Key, list[i]), index++);
            }
        }

        public int Count
        {
            get
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(GetCount() == m_Count);
#endif
                return m_Count;
            }
        }

        public bool IsSynchronized
        {
            get { return this.m_InternalData.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return this.m_InternalData.SyncRoot; }
        }


        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MapEnumerator(this);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ParticalClear();
            this.m_InternalData.Clear();
        }

        #endregion
    }
}
