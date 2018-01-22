using IGeometry = GeoAPI.Geometries.IGeometry;

namespace LandRush.Cadastre.Russia
{
	// Квартал
	public class Block
	{
		protected Block() { }

		public Block(District district, int localNumber)
		{
			this.district = district;
			this.localNumber = localNumber;
		}

		private District district;
		private int localNumber;

		public virtual District District =>
			this.district;

		public virtual int LocalNumber =>
			this.localNumber;

		public virtual BlockNumber Number =>
			new BlockNumber(this.district.Number, this.localNumber);

		// Название
		private string name;
		public virtual string Name
		{
			get => this.name;
			set => this.name = value;
		}

		// Заметка (Note)
		private string note;
		public virtual string Note
		{
			get => this.note;
			set => this.note = value;
		}

		// Документированная площадь квартала
		private double? documentedArea;
		public virtual bool HasDocumentedArea =>
			this.documentedArea.HasValue;

		public virtual double DocumentedArea
		{
			get => this.documentedArea.Value;
			set => this.documentedArea = value;
		}

		// Геометрия квартала
		// TODO: use specific geometry type
		private IGeometry geometry;
		public virtual IGeometry Geometry
		{
			get => this.geometry;
			set => this.geometry = value;
		}

		////// Коэффициенты кадастровой стоимости для участков в данном квартале (по категориям земель)
		////private IDictionary<LandCategory, double> cadastralValueFactors;
		////public virtual IDictionary<LandCategory, double> CadastralValueFactors
		////{
		////	get
		////	{
		////		return cadastralValueFactors;
		////	}
		////}

		public override bool Equals(object obj) =>
			obj is Block other ?
				this.district == other.district &&
				this.localNumber == other.localNumber :
				false;

		public override int GetHashCode() =>
			this.district.GetHashCode() ^
			this.localNumber.GetHashCode();

		public override string ToString() =>
			$"{this.name ?? "<без имени>"} ({this.Number})";
	}
}
