using System;
using System.Collections.Generic;

namespace Shared
{
    public interface ISqliteExecuter : IDisposable
    {
        void CreateTables();
        void Initilize();
        void InsertEntries(IEnumerable<User> users);
        IEnumerable<User> ReadEntries();
    }
}