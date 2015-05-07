using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Тип права на землю
	/// </summary>
	public class LandRightType : DomainValue
	{
		protected LandRightType() { }
		public LandRightType(string code, string description) : base(code, description) { }
	}

	/// <summary>
	/// Право на земельный участок
	/// </summary>
	public class ParcelRight : IComparable<ParcelRight>
	{
		protected ParcelRight() { }

		public ParcelRight(Parcel parcel, int number, LandRightType landRightType, string name)
		{
			this.parcel = parcel;
			this.number = number;
			this.landRightType = landRightType;
			this.name = name;
		}

		private int number;

		private LandRightType landRightType;
		public virtual LandRightType LandRightType
		{
			get
			{
				return landRightType;
			}
		}

		private string name;
		public virtual string Name
		{
			get
			{
				return name;
			}
		}

		private Parcel parcel;
		public virtual Parcel Parcel
		{
			get
			{
				return parcel;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is ParcelRight)) return false;
			else return ((obj as ParcelRight).parcel == this.parcel) && ((obj as ParcelRight).number == this.number);
		}

		public override int GetHashCode()
		{
			return this.parcel.GetHashCode() ^ (int)this.number.GetHashCode();
		}

		public virtual int CompareTo(ParcelRight other)
		{
			return this.number.CompareTo(other.number);
		}

		// !!
		public override string ToString()
		{
			return name + " (" + landRightType.Description + ")";
		}
	}
}