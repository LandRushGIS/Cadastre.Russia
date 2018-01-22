namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Район
	/// </summary>
	public class District
	{
		protected District() : this(null, 0) { }

		public District(Region region, int localNumber)
		{
			this.region = region;
			this.localNumber = localNumber;
		}

		private Region region;
		public virtual Region Region =>
			this.region;

		private int localNumber;
		public virtual int LocalNumber =>
			this.localNumber;

		public override bool Equals(object obj) =>
			obj is District other ?
				this.region == other.region &&
				this.localNumber == other.localNumber :
				false;

		public override int GetHashCode() =>
			this.region.GetHashCode() ^
			this.localNumber;

		public virtual DistrictNumber Number =>
			new DistrictNumber(this.region.Number, this.localNumber);
	}
}
