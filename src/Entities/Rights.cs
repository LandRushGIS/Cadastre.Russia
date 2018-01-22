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
		private Parcel parcel;
		private int number;
		private LandRightType landRightType;
		private string name;
		private Landholder landholder;
		private short shareNumerator;
		private short shareDenominator;
		private string shareText;
		private string description;

		protected ParcelRight() { }

		public ParcelRight(
			Parcel parcel,
			int number,
			LandRightType landRightType,
			string name,
			Landholder landholder,
			short shareNumerator,
			short shareDenominator)
		{
			this.parcel = parcel;
			this.number = number;
			this.landRightType = landRightType;
			this.name = name;
			this.landholder = landholder;
			this.shareNumerator = shareNumerator;
			this.shareDenominator = shareDenominator;
		}

		public virtual Parcel Parcel => this.parcel;

		public virtual int Number => this.number;

		public virtual LandRightType LandRightType => this.landRightType;

		public virtual string Name => this.name;

		public virtual Landholder Landholder
		{
			get => this.landholder;
			set => this.landholder = value;
		}

		public virtual short ShareNumerator
		{
			get => this.shareNumerator;
			set => this.shareNumerator = value;
		}

		public virtual short ShareDenominator
		{
			get => this.shareDenominator;
			set => this.shareDenominator = value;
		}

		public virtual string ShareText
		{
			get => this.shareText;
			set => this.shareText = value;
		}

		public virtual string Description
		{
			get => this.description;
			set => this.description = value;
		}

		public override bool Equals(object obj) =>
			obj is ParcelRight other ?
				this.parcel == other.parcel &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.parcel.GetHashCode() ^
			this.number.GetHashCode();

		public virtual int CompareTo(ParcelRight other) =>
			this.number.CompareTo(other.number);

		// !!
		public override string ToString() =>
			$"{this.name} ({this.landRightType.Description})";
	}
}
