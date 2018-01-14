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
		public virtual Parcel Parcel =>
			this.parcel;

		private int number;
		public virtual int Number =>
			this.number;

		// Геометрия части земли
		private IGeometry geometry;
		public virtual IGeometry Geometry =>
			this.geometry;

		// Фактическая площадь части земли участка
		public virtual double Area =>
			this.geometry.Area;

		// Адрес
		private string address;
		public virtual string Address
		{
			get => this.address;
			set => this.address = value;
		}

		// Оценочная стоимость
		private double? assessedValue;
		public virtual bool HasAssessedValue =>
			this.assessedValue.HasValue;

		public virtual void UnsetAssessedValue()
		{
			this.assessedValue = null;
		}

		public virtual double AssessedValue
		{
			get => this.assessedValue.Value;
			set => this.assessedValue = value;
		}

		// Заметка (Note)
		private string note;
		public virtual string Note
		{
			get => this.note;
			set => this.note = value;
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
