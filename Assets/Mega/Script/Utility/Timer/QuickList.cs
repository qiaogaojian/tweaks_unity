using System.Collections.Generic;
using UnityEngine;
namespace Mega
{
	public class QuickList<T>
	{
		public T[] buffer;
		public int size;

		public QuickList()
		{
			this.buffer = new T[8];
		}

		public QuickList(int size)
		{
			this.buffer = new T[size];
		}

		public void Add(T item)
		{
			if ((this.buffer == null) || (this.size == this.buffer.Length))
			{
				this.AllocateMore();
			}
			this.buffer[this.size++] = item;
		}

		private void AllocateMore()
		{
			T[] array = (this.buffer == null) ? new T[0x20] : new T[Mathf.Max(this.buffer.Length << 1, 0x20)];
			if ((this.buffer != null) && (this.size > 0))
			{
				this.buffer.CopyTo(array, 0);
			}
			this.buffer = array;
		}

		public void Clear()
		{
			this.size = 0;
		}

		public void Release()
		{
			this.size = 0;
			this.buffer = null;
		}

		public void Remove(T item)
		{
			if (this.buffer != null)
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;
				for (int i = 0; i < this.size; i++)
				{
					if (comparer.Equals(this.buffer[i], item))
					{
						this.size--;
						this.buffer[i] = default(T);
						for (int j = i; j < this.size; j++)
						{
							this.buffer[j] = this.buffer[j + 1];
						}
						return;
					}
				}
			}
		}

		public void RemoveAt(int index)
		{
			if ((this.buffer != null) && (index < this.size))
			{
				this.size--;
				this.buffer[index] = default(T);
				for (int i = index; i < this.size; i++)
				{
					this.buffer[i] = this.buffer[i + 1];
				}
			}
		}

		public T[] ToArray()
		{
			this.Trim();
			return this.buffer;
		}

		private void Trim()
		{
			if (this.size > 0)
			{
				if (this.size < this.buffer.Length)
				{
					T[] localArray = new T[this.size];
					for (int i = 0; i < this.size; i++)
					{
						localArray[i] = this.buffer[i];
					}
					this.buffer = localArray;
				}
			}
			else
			{
				this.buffer = null;
			}
		}

		public int Count
		{
			get
			{
				return this.size;
			}
		}

		public T this[int i]
		{
			get
			{
				return this.buffer[i];
			}
			set
			{
				this.buffer[i] = value;
			}
		}
	}

}