using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Тип ограничения (обременения) права на землю
	/// </summary>
	public class LandEncumbranceType : DomainValue
	{
		public LandEncumbranceType(string code, string description) : base(code, description) { }
		protected LandEncumbranceType() { }
	}

	public class CadastralUnitEncumbrance : IComparable<CadastralUnitEncumbrance>
	{
		protected int number;
		private LandEncumbranceType landEncumbranceType;
		private string name;

		public CadastralUnitEncumbrance(int number, LandEncumbranceType landEncumbranceType, string name)
		{
			this.number = number;
			this.landEncumbranceType = landEncumbranceType;
			this.name = name;
		}

		protected CadastralUnitEncumbrance() { }

		public virtual LandEncumbranceType LandEncumbranceType =>
			this.landEncumbranceType;

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
		private Parcel parcel;

		public ParcelEncumbrance(Parcel parcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name) =>
			this.parcel = parcel;

		protected ParcelEncumbrance() : base() { }

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
		private SubParcel subParcel;

		public SubParcelEncumbrance(SubParcel subParcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name) =>
			this.subParcel = subParcel;

		protected SubParcelEncumbrance() : base() { }

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
