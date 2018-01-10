using System;
using IGeometry = GeoAPI.Geometries.IGeometry;

namespace LandRush.Cadastre.Russia
{
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
