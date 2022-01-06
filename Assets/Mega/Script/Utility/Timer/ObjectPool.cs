using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
namespace Mega
{
	public class ObjectPool : IObjectPool
	{
		private Type destType;
		private object[] ctorArgs;
		private int minObjCount;
		private int maxObjCount;
		private int shrinkPoint;
		private Hashtable hashTableObjs = new Hashtable();
		private Hashtable hashTableStatus = new Hashtable();
		private QuickList<int> keyList = new QuickList<int>();
		private bool supportReset;
		public static bool notUsePool;

		public event CallBackObjPool PoolShrinked;
		public event CallBackObjPool MemoryUseOut;

		public int MinObjCount
		{
			get
			{
				return this.minObjCount;
			}
		}

		public int MaxObjCount
		{
			get
			{
				return this.maxObjCount;
			}
		}

		public int CurObjCount
		{
			get
			{
				return this.keyList.size;
			}
		}

		public int IdleObjCount
		{
			get
			{
				Monitor.Enter(this);
				int idleObjCount;
				try
				{
					idleObjCount = this.GetIdleObjCount();
				}
				finally
				{
					Monitor.Exit(this);
				}
				return idleObjCount;
			}
		}

		public bool Initialize(Type objType, object[] cArgs, int minNum, int maxNum)
		{
			if (minNum < 1)
			{
				minNum = 1;
			}
			if (maxNum < 2)
			{
				maxNum = 2;
			}
			this.destType = objType;
			this.ctorArgs = cArgs;
			this.minObjCount = minNum;
			this.maxObjCount = maxNum;
			double num = 1.0 - (double)minNum / (double)maxNum;
			this.shrinkPoint = (int)(num * (double)minNum);
			Type typeFromHandle = typeof(IPooledObjSupporter);
			if (typeFromHandle.IsAssignableFrom(objType))
			{
				this.supportReset = true;
			}
			if (ObjectPool.notUsePool)
			{
			}
			return true;
		}

		private void InstanceObjects()
		{
			for (int i = 0; i < this.minObjCount; i++)
			{
				this.CreateOneObject();
			}
		}

		private int CreateOneObject()
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(this.destType, this.ctorArgs);
			}
			catch (Exception ex)
			{
				this.PrintPoolStatus();
				Debuger.LogError("ObjectPool " + this.destType.ToString() + " used out!!!!!" + ex.ToString());
				this.maxObjCount = this.CurObjCount;
				if (this.minObjCount > this.CurObjCount)
				{
					this.minObjCount = this.CurObjCount;
				}
				if (this.MemoryUseOut != null)
				{
					this.MemoryUseOut();
				}
				return -1;
			}
			int hashCode = obj.GetHashCode();
			this.hashTableObjs.Add(hashCode, obj);
			this.hashTableStatus.Add(hashCode, true);
			this.keyList.Add(hashCode);
			return hashCode;
		}

		private void DistroyOneObject(int key)
		{
			object obj = this.hashTableObjs[key];
			IDisposable disposable = obj as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			this.hashTableObjs.Remove(key);
			this.hashTableStatus.Remove(key);
			this.keyList.Remove(key);
		}

		public object RentObject()
		{
			Monitor.Enter(this);
			object result;
			try
			{
				if (ObjectPool.notUsePool)
				{
					int num = this.CreateOneObject();
					if (num != -1)
					{
						this.hashTableStatus[num] = false;
						result = this.hashTableObjs[num];
					}
					else
					{
						result = null;
					}
				}
				else
				{
					object obj = null;
					for (int i = 0; i < this.keyList.size; i++)
					{
						int num = this.keyList[i];
						if ((bool)this.hashTableStatus[num])
						{
							this.hashTableStatus[num] = false;
							obj = this.hashTableObjs[num];
							break;
						}
					}
					if (obj == null && this.keyList.size < this.maxObjCount)
					{
						int num = this.CreateOneObject();
						if (num != -1)
						{
							this.hashTableStatus[num] = false;
							obj = this.hashTableObjs[num];
						}
					}
					result = obj;
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
			return result;
		}

		public void GiveBackObject(int objHashCode)
		{
			if (this.hashTableStatus[objHashCode] == null)
			{
				return;
			}
			Monitor.Enter(this);
			try
			{
				if (ObjectPool.notUsePool)
				{
					this.DistroyOneObject(objHashCode);
				}
				else
				{
					this.hashTableStatus[objHashCode] = true;
					if (this.supportReset)
					{
						IPooledObjSupporter pooledObjSupporter = (IPooledObjSupporter)this.hashTableObjs[objHashCode];
						pooledObjSupporter.Reset();
					}
					if (this.CanShrink())
					{
						this.Shrink();
					}
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}

		private bool CanShrink()
		{
			int idleObjCount = this.GetIdleObjCount();
			int num = this.CurObjCount - idleObjCount;
			return num < this.shrinkPoint && this.CurObjCount > this.minObjCount + (this.maxObjCount - this.minObjCount) / 2;
		}

		private void Shrink()
		{
			while (this.CurObjCount > this.minObjCount)
			{
				int num = -1;
				for (int i = 0; i < this.keyList.size; i++)
				{
					int num2 = this.keyList[i];
					if ((bool)this.hashTableStatus[num2])
					{
						num = num2;
						break;
					}
				}
				if (num == -1)
				{
					break;
				}
				this.DistroyOneObject(num);
			}
			if (this.PoolShrinked != null)
			{
				this.PoolShrinked();
			}
		}

		public bool CheckObjectStatus(int hashCode)
		{
			return this.hashTableStatus[hashCode] != null && (bool)this.hashTableStatus[hashCode];
		}

		public void Dispose()
		{
			Type typeFromHandle = typeof(IDisposable);
			if (typeFromHandle.IsAssignableFrom(this.destType))
			{
				int num = this.keyList.size - 1;
				for (int i = num; i >= 0; i--)
				{
					this.DistroyOneObject(this.keyList[i]);
				}
			}
			this.hashTableStatus.Clear();
			this.hashTableObjs.Clear();
			this.keyList.Clear();
		}

		private int GetIdleObjCount()
		{
			int num = 0;
			for (int i = 0; i < this.keyList.size; i++)
			{
				int num2 = this.keyList[i];
				if ((bool)this.hashTableStatus[num2])
				{
					num++;
				}
			}
			return num;
		}

		public string PrintPoolStatus()
		{
			return string.Format("Object Type:{0} -> (Current:{1}/Idle:{2}/Max:{3})", new object[]
			{
			this.destType.ToString (),
			this.CurObjCount,
			this.IdleObjCount,
			this.maxObjCount
			});
		}

		public void SeeEveryNode()
		{
			for (int i = 0; i < this.keyList.size; i++)
			{
				if ((bool)this.hashTableStatus[this.keyList[i]])
				{
					object obj = this.hashTableObjs[this.keyList[i]];
					Debuger.Log(obj.ToString());
				}
				else
				{
					object obj2 = this.hashTableObjs[this.keyList[i]];
					Debuger.Log(obj2.ToString());
				}
			}
		}

		public bool isInPool(int hashCode)
		{
			bool result = false;
			if (this.keyList == null)
			{
				return false;
			}
			for (int i = 0; i < this.keyList.Count; i++)
			{
				if (this.keyList[i] == hashCode)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool isIdle(int hashCode)
		{
			if (!this.isInPool(hashCode))
			{
				Debuger.LogError("Found Object not in pool when you want to check this object is idle in pool >_<");
				return false;
			}
			return (bool)this.hashTableStatus[hashCode];
		}
	}
}
