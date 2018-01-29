namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Округ
	/// </summary>
	public class Region
	{
		private int localNumber;
		private string name;

		public Region(int localNumber, string name)
		{
			this.localNumber = localNumber;
			this.name = name;
		}

		protected Region() : this(0, "") { }

		public virtual RegionNumber Number =>
			new RegionNumber(this.localNumber);

		public virtual int LocalNumber => this.localNumber;

		public virtual string Name => this.name;
	}
}
