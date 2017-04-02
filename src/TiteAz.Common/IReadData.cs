using System;

namespace TiteAz.Common
{
   public interface IReadData
    {
        T Get<T>(Guid id) where T : IHaveIdentity;
    }

    public interface IHaveIdentity
    {
        Guid Id { get; }
    }
}