using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Тип ограничения (обременения) права на землю
	/// </summary>
	public class LandEncumbranceType : DomainValue
	{
		protected LandEncumbranceType() { }
		public LandEncumbranceType(string code, string description) : base(code, description) { }
	}

	public class CadastralUnitEncumbrance : IComparable<CadastralUnitEncumbrance>
	{
		protected CadastralUnitEncumbrance() { }

		public CadastralUnitEncumbrance(int number, LandEncumbranceType landEncumbranceType, string name)
		{
			this.number = number;
			this.landEncumbranceType = landEncumbranceType;
			this.name = name;
		}

		protected int number;

		private LandEncumbranceType landEncumbranceType;
		public virtual LandEncumbranceType LandEncumbranceType =>
			this.landEncumbranceType;

		private string name;
		public virtual string Name =>
			this.name;

		public virtual int CompareTo(CadastralUnitEncumbrance other) =>
			this.number.CompareTo(other.number);

		// !!
		public override string ToString() =>
			$"{this.name} ({this.landEncumbranceType.Description})";
	}

	/// <summary>
	/// Ограничение (обременение) права на участок
	/// </summary>
	public class ParcelEncumbrance : CadastralUnitEncumbrance
	{
		protected ParcelEncumbrance() : base() { }

		public ParcelEncumbrance(Parcel parcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name) =>
			this.parcel = parcel;

		private Parcel parcel;
		public virtual Parcel Parcel =>
			this.parcel;

		public override bool Equals(object obj) =>
			obj is ParcelEncumbrance other ?
				this.parcel == other.parcel &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.parcel.GetHashCode() ^
			this.number.GetHashCode();
	}

	/// <summary>
	/// Ограничение (обременение) права на часть участка
	/// </summary>
	public class SubParcelEncumbrance : CadastralUnitEncumbrance
	{
		protected SubParcelEncumbrance() : base() { }

		public SubParcelEncumbrance(SubParcel subParcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name) =>
			this.subParcel = subParcel;

		private SubParcel subParcel;
		public virtual SubParcel SubParcel =>
			this.subParcel;

		public override bool Equals(object obj) =>
			obj is SubParcelEncumbrance other ?
				this.subParcel == other.subParcel &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.subParcel.GetHashCode() ^
			this.number.GetHashCode();
	}
}
