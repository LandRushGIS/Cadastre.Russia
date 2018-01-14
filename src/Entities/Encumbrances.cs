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
		public virtual LandEncumbranceType LandEncumbranceType
		{
			get
			{
				return this.landEncumbranceType;
			}
		}

		private string name;
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		public virtual int CompareTo(CadastralUnitEncumbrance other)
		{
			return this.number.CompareTo(other.number);
		}

		// !!
		public override string ToString()
		{
			return this.name + " (" + this.landEncumbranceType.Description + ")";
		}
	}

	/// <summary>
	/// Ограничение (обременение) права на участок
	/// </summary>
	public class ParcelEncumbrance : CadastralUnitEncumbrance
	{
		protected ParcelEncumbrance() : base() { }

		public ParcelEncumbrance(Parcel parcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name)
		{
			this.parcel = parcel;
		}

		private Parcel parcel;
		public virtual Parcel Parcel
		{
			get
			{
				return this.parcel;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is ParcelEncumbrance)) return false;
			else return ((obj as ParcelEncumbrance).parcel == this.parcel) && ((obj as ParcelEncumbrance).number == this.number);
		}

		public override int GetHashCode()
		{
			return this.parcel.GetHashCode() ^ (int)this.number.GetHashCode();
		}
	}

	/// <summary>
	/// Ограничение (обременение) права на часть участка
	/// </summary>
	public class SubParcelEncumbrance : CadastralUnitEncumbrance
	{
		protected SubParcelEncumbrance() : base() { }

		public SubParcelEncumbrance(SubParcel subParcel, int number, LandEncumbranceType landEncumbranceType, string name)
			: base(number, landEncumbranceType, name)
		{
			this.subParcel = subParcel;
		}

		private SubParcel subParcel;
		public virtual SubParcel SubParcel
		{
			get
			{
				return this.subParcel;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is SubParcelEncumbrance)) return false;
			else return ((obj as SubParcelEncumbrance).subParcel == this.subParcel) && ((obj as SubParcelEncumbrance).number == this.number);
		}

		public override int GetHashCode()
		{
			return this.subParcel.GetHashCode() ^ (int)this.number.GetHashCode();
		}
	}
}
