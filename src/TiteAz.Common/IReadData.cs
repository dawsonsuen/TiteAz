using System;
using NEvilES.Pipeline;

namespace TiteAz.Common
{
   public interface IReadData
    {
        T Get<T>(Guid id) where T : IHaveIdentity;
    }
}
