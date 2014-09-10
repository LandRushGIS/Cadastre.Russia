using System;
using IGeometry = GeoAPI.Geometries.IGeometry;

namespace LandRush.Cadastre
{
	/// <summary>
	/// Вид земель (вид угодий)
	/// </summary>
	public class LandKind : IComparable<LandKind>
	{
		private int id;
		public virtual int Id
		{
			get
			{
				return id;
			}
			protected set
			{
				id = value;
			}
		}

		private string description;
		public virtual string Description
		{
			get
			{
				return description;
			}
			protected set
			{
				description = value;
			}
		}

		int IComparable<LandKind>.CompareTo(LandKind other)
		{
			return this.id.CompareTo(other.id);
		}
	}

	/// <summary>
	/// Права землепользования
	/// </summary>
	public class LandUserRightType : IComparable<LandUserRightType>
	{
		protected LandUserRightType() : this(0, "") { }

		public LandUserRightType(int id, string description)
		{
			this.id = id;
			this.description = description;
		}

		private int id;
		public virtual int Id
		{
			get
			{
				return id;
			}
			protected set
			{
				id = value;
			}
		}

		private string description;
		public virtual string Description
		{
			get
			{
				return description;
			}
			protected set
			{
				description = value;
			}
		}

		int IComparable<LandUserRightType>.CompareTo(LandUserRightType other)
		{
			return this.id.CompareTo(other.id);
		}
	}

	/// <summary>
	/// Часть земли участка
	/// </summary>
	public class ParcelLandPiece
	{
		protected ParcelLandPiece() : this(null, 0, null) { }

		public ParcelLandPiece(Parcel parcel, int number, IGeometry geometry)
		{
			this.parcel = parcel;
			this.number = number;
			this.geometry = geometry;
		}

		private Parcel parcel;
		public virtual Parcel Parcel
		{
			get
			{
				return parcel;
			}
		}

		private int number;
		public virtual int Number
		{
			get
			{
				return number;
			}
		}

		// Геометрия части земли
		private IGeometry geometry;
		public virtual IGeometry Geometry
		{
			get
			{
				return geometry;
			}
			//set
			//{
			//	geometry = value;
			//}
		}

		// Фактическая площадь части земли участка
		public virtual double Area
		{
			get
			{
				return geometry.Area;
			}
		}

		// Адрес
		private string address;
		public virtual string Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		// Оценочная стоимость
		private double? assessedValue;
		public virtual bool HasAssessedValue
		{
			get
			{
				return assessedValue.HasValue;
			}
		}

		public virtual void UnsetAssessedValue()
		{
			assessedValue = null;
		}

		public virtual double AssessedValue
		{
			get
			{
				return assessedValue.Value;
			}
			set
			{
				assessedValue = value;
			}
		}

		// Балансовая стоимость
		private double? bookValue;
		public virtual bool HasBookValue
		{
			get
			{
				return bookValue.HasValue;
			}
		}

		public virtual void UnsetBookValue()
		{
			bookValue = null;
		}

		public virtual double BookValue
		{
			get
			{
				return bookValue.Value;
			}
			set
			{
				bookValue = value;
			}
		}

		// Вид угодий
		private LandKind landKind;
		public virtual LandKind LandKind
		{
			get
			{
				return landKind;
			}
			set
			{
				landKind = value;
			}
		}

		// Право землепользователя
		private LandUserRightType landUserRightType;
		public virtual LandUserRightType LandUserRightType
		{
			get
			{
				return landUserRightType;
			}
			set
			{
				landUserRightType = value;
			}
		}

		// Дата начала срока аренды
		private DateTime? leaseStartDate;
		public virtual bool HasLeaseStartDate
		{
			get
			{
				return leaseStartDate.HasValue;
			}
		}

		public virtual void UnsetLeaseStartDate()
		{
			leaseStartDate = null;
		}

		public virtual DateTime LeaseStartDate
		{
			get
			{
				return leaseStartDate.Value;
			}
			set
			{
				leaseStartDate = value;
			}
		}

		// Дата конца срока аренды
		private DateTime? leaseEndDate;
		public virtual bool HasLeaseEndDate
		{
			get
			{
				return leaseEndDate.HasValue;
			}
		}

		public virtual void UnsetLeaseEndDate()
		{
			leaseEndDate = null;
		}

		public virtual DateTime LeaseEndDate
		{
			get
			{
				return leaseEndDate.Value;
			}
			set
			{
				leaseEndDate = value;
			}
		}

		// Заметка (Note)
		private string note;
		public virtual string Note
		{
			get
			{
				return note;
			}
			set
			{
				note = value;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is ParcelLandPiece)) return false;
			else return ((obj as ParcelLandPiece).parcel == this.parcel) && ((obj as ParcelLandPiece).number == this.number);
		}

		public override int GetHashCode()
		{
			return this.parcel.GetHashCode() ^ (int)this.number.GetHashCode();
		}
	}
}