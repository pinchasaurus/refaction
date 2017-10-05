﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Collections.ObjectModel;

namespace Refaction.Data.Fakes
{
    /// <summary>
    /// An in-memory DbSet for testing Entity Framework per https://romiller.com/2012/02/14/testing-with-a-fake-dbcontext
    /// </summary>
    public abstract class FakeDbSet<T> : IDbSet<T>
        where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        static readonly IEnumerable<T> _emptyArrayOfT = new T[] { };

        public FakeDbSet()
            : this(_emptyArrayOfT)
        {
        }

        public FakeDbSet(IEnumerable<T> initialEntities)
        {
            _data = new ObservableCollection<T>(initialEntities);
            _query = _data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find()");
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
