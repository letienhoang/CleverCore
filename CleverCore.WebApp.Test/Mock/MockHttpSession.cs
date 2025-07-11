using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleverCore.WebApp.Test.Mock
{
    public class MockHttpSession : ISession
    {
        private Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public object this[string key]
        {
            get => sessionStorage[key];
            set => sessionStorage[key] = value;
        }

        bool ISession.IsAvailable => throw new NotImplementedException();

        string ISession.Id => throw new NotImplementedException();

        IEnumerable<string> ISession.Keys => throw new NotImplementedException();

        void ISession.Clear()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        void ISession.Remove(string key)
        {
            throw new NotImplementedException();
        }

        void ISession.Set(string key, byte[] value)
        {
            throw new NotImplementedException();
        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}