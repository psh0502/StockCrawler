using System;
using System.Collections;

namespace TomTang.Collections
{
	/// <summary>
	/// SizeFixiedQueue ªººK­n´y­z¡C
	/// </summary>
	public class SizeFixiedQueue : Queue
	{
		private int mnMaxSize;
		private bool mbAutoDequeueWhenFill = false;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="MaxSize">The maximum count of object this queue can contain.</param>
		public SizeFixiedQueue(int MaxSize) : base(MaxSize)
		{
			mnMaxSize = MaxSize;
		}
		/// <summary>
		/// The maximum count of object this queue can contain.
		/// </summary>
		public int Size
		{
			get { return mnMaxSize; }
			set { mnMaxSize = value; }
		}
		/// <summary>
		/// Push object into the queue.
		/// </summary>
		/// <param name="obj">The object you want to pushed.</param>
		/// <exception cref="OverflowException">Queue was filled and removing surplus objects are disabled.</exception>
		public override void Enqueue(object obj)
		{
			if (Count >= mnMaxSize)
			{
				if (mbAutoDequeueWhenFill)
				{
					// Drop surplus queued data
					while(Count >= mnMaxSize)
					{
						Dequeue();
					}
				} 
				else
				{
					throw new OverflowException("This queue was filled! Nothing can be queued anymore.");
				}
			}
			base.Enqueue(obj);
		}
		/// <summary>
		/// Set it to be true if you want to remove first queued object automatically when it was filled.
		/// </summary>
		public bool AutoDequeue
		{
			get { return mbAutoDequeueWhenFill; }
			set { mbAutoDequeueWhenFill = value; }
		}
		/// <summary>
		/// Make size limitation as current count of queued object.
		/// </summary>
		public override void TrimToSize()
		{
			base.TrimToSize ();
			mnMaxSize = Count;
		}
	}
}
