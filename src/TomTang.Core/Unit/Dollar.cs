using System;

namespace TomTang.Unit
{
	/// <summary>
	/// Currency unit.
	/// </summary>
	public class Dollar
	{
		private double _Amount = 0;
		private Nation _Nation;
		public Dollar(double Amout)
		{
			_Amount = Amount;
		}
		public double Amount
		{
			get { return _Amount; }
			set { _Amount = value; }
		}
		public override bool Equals(object obj)
		{
			bool bIsEqual = false;
			if (obj.GetType() == GetType())
			{
				bIsEqual = (_Amount == ((Dollar)obj).Amount);
			}
			else
			{
				try 
				{
					bIsEqual = (_Amount == (double)obj);
				} 
				catch {}
			}
			return bIsEqual;
		}

		public override int GetHashCode()
		{
			return (int)_Amount;
		}

		public Nation Nation
		{
			get { return _Nation; }
			set { _Nation = value; }
		}
		public override string ToString()
		{
			return _Amount.ToString();
		}


		#region Operators

		#region +
		public static Dollar operator + (Dollar dollar1, Dollar dollar2)
		{
			return new Dollar(dollar1.Amount + dollar2.Amount);
		}
		public static Dollar operator + (Dollar dollar1, double amount2)
		{
			return new Dollar(dollar1.Amount + amount2);
		}
		public static Dollar operator + (double amount1, Dollar dollar2)
		{
			return new Dollar(dollar2.Amount + amount1);
		}
		#endregion

		#region -
		public static Dollar operator - (Dollar dollar1, Dollar dollar2)
		{
			return new Dollar(dollar1.Amount - dollar2.Amount);
		}
		public static Dollar operator - (Dollar dollar1, double amount2)
		{
			return new Dollar(dollar1.Amount - amount2);
		}
		public static Dollar operator - (double amount1, Dollar dollar2)
		{
			return new Dollar(dollar2.Amount - amount1);
		}
		#endregion

		#region *
		public static Dollar operator * (Dollar dollar1, Dollar dollar2)
		{
			return new Dollar(dollar1.Amount * dollar2.Amount);
		}
		public static Dollar operator * (Dollar dollar1, double amount2)
		{
			return new Dollar(dollar1.Amount * amount2);
		}
		public static Dollar operator * (double amount1, Dollar dollar2)
		{
			return new Dollar(dollar2.Amount * amount1);
		}
		#endregion

		#region /
		public static Dollar operator / (Dollar dollar1, Dollar dollar2)
		{
			return new Dollar(dollar1.Amount / dollar2.Amount);
		}
		public static Dollar operator / (Dollar dollar1, double amount2)
		{
			return new Dollar(dollar1.Amount / amount2);
		}
		public static Dollar operator / (double amount1, Dollar dollar2)
		{
			return new Dollar(dollar2.Amount / amount1);
		}
		#endregion

		#region %
		public static Dollar operator % (Dollar dollar1, Dollar dollar2)
		{
			return new Dollar(dollar1.Amount % dollar2.Amount);
		}
		public static Dollar operator % (Dollar dollar1, double amount2)
		{
			return new Dollar(dollar1.Amount % amount2);
		}
		public static Dollar operator % (double amount1, Dollar dollar2)
		{
			return new Dollar(dollar2.Amount % amount1);
		}
		#endregion

		#region ==
		public static bool operator == (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount == dollar2.Amount);
		}
		public static bool operator == (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount == amount2);
		}
		public static bool operator == (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount == amount1);
		}
		#endregion

		#region !=
		public static bool operator != (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount != dollar2.Amount);
		}
		public static bool operator != (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount != amount2);
		}
		public static bool operator != (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount != amount1);
		}
		#endregion

		#region >
		public static bool operator > (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount > dollar2.Amount);
		}
		public static bool operator > (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount > amount2);
		}
		public static bool operator > (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount > amount1);
		}
		#endregion

		#region <
		public static bool operator < (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount < dollar2.Amount);
		}
		public static bool operator < (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount < amount2);
		}
		public static bool operator < (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount < amount1);
		}
		#endregion

		#region >=
		public static bool operator >= (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount >= dollar2.Amount);
		}
		public static bool operator >= (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount >= amount2);
		}
		public static bool operator >= (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount >= amount1);
		}
		#endregion

		#region <=
		public static bool operator <= (Dollar dollar1, Dollar dollar2)
		{
			return (dollar1.Amount <= dollar2.Amount);
		}
		public static bool operator <= (Dollar dollar1, double amount2)
		{
			return (dollar1.Amount <= amount2);
		}
		public static bool operator <= (double amount1, Dollar dollar2)
		{
			return (dollar2.Amount <= amount1);
		}
		#endregion

		#endregion
	}
}
