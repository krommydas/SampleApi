using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Storage
{
    public class DuplicateException : System.Exception { }

    public class PersistenceException : System.Exception { }

    public class NotFoundException : System.Exception { }
}
