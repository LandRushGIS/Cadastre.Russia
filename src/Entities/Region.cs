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

		public virtual RegionNumber Number =>
			new RegionNumber(this.localNumber);

		private int localNumber;
		public virtual int LocalNumber => this.localNumber;

		private string name;
		public virtual string Name => this.name;
	}
}
