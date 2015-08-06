namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Округ
	/// </summary>
	public class Region
	{
		protected Region() : this(0, "") { }

		public Region(int localNumber, string name)
		{
			this.localNumber = localNumber;
			this.name = name;
		}

		public virtual RegionNumber Number
		{
			get
			{
				return new RegionNumber(localNumber);
			}
		}

		private int localNumber;
		public virtual int LocalNumber
		{
			get
			{
				return localNumber;
			}
		}

		private string name;
		public virtual string Name
		{
			get
			{
				return name;
			}
		}
	}
}