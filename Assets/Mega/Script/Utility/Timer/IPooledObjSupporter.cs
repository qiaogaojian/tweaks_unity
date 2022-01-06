using System;
namespace Mega
{
	public interface IPooledObjSupporter : IDisposable
	{
		void Reset();
	}
}

