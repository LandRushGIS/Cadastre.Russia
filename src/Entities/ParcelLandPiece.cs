using System;
using IGeometry = GeoAPI.Geometries.IGeometry;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Часть земли участка
	/// </summary>
	public class ParcelLandPiece
	{
		private Parcel parcel;
		private int number;
		private IGeometry geometry;
		private string address;
		private double? assessedValue;
		private string note;

		public ParcelLandPiece(Parcel parcel, int number, IGeometry geometry)
		{
			this.parcel = parcel;
			this.number = number;
			this.geometry = geometry;
		}

		protected ParcelLandPiece() : this(null, 0, null) { }

		public virtual Parcel Parcel =>
			this.parcel;

		public virtual int Number =>
			this.number;

		// Геометрия части земли
		public virtual IGeometry Geometry =>
			this.geometry;

		// Фактическая площадь части земли участка
		public virtual double Area =>
			this.geometry.Area;

		// Адрес
		public virtual string Address
		{
			get => this.address;
			set => this.address = value;
		}

		// Оценочная стоимость
		public virtual bool HasAssessedValue =>
			this.assessedValue.HasValue;

		public virtual void UnsetAssessedValue() =>
			this.assessedValue = null;

		public virtual double AssessedValue
		{
			get => this.assessedValue.Value;
			set => this.assessedValue = value;
		}

		// Заметка (Note)
		public virtual string Note
		{
			get => this.note;
			set => this.note = value;
		}

		public override bool Equals(object obj) =>
			obj is ParcelLandPiece other ?
				this.parcel == other.parcel &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.parcel.GetHashCode() ^
			this.number.GetHashCode();
	}
}
