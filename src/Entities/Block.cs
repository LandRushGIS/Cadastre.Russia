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

		public virtual District District
		{
			get
			{
				return district;
			}
		}

		public virtual int LocalNumber
		{
			get
			{
				return localNumber;
			}
		}

		public virtual BlockNumber Number
		{
			get
			{
				return new BlockNumber(district.Number, localNumber);
			}
		}

		// Название
		private string name;
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
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

		// Документированная площадь квартала
		private double? documentedArea;
		public virtual bool HasDocumentedArea
		{
			get
			{
				return documentedArea.HasValue;
			}
		}

		public virtual double DocumentedArea
		{
			get
			{
				return documentedArea.Value;
			}
			set
			{
				documentedArea = value;
			}
		}

		// Геометрия квартала
		// TODO: use specific geometry type
		private IGeometry geometry;
		public virtual IGeometry Geometry
		{
			get
			{
				return geometry;
			}
			set
			{
				geometry = value;
			}
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

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is Block)) return false;
			else return ((obj as Block).district == this.district) && ((obj as Block).localNumber == this.localNumber);
		}

		public override int GetHashCode()
		{
			return this.district.GetHashCode() ^ (int)this.localNumber.GetHashCode();
		}

		public override string ToString()
		{
			return (this.name != null ? this.name : "<без имени>") + " (" + this.Number.ToString() + ")";
		}
	}
}
