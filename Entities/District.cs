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
		public virtual Region Region
		{
			get
			{
				return region;
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

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is District)) return false;
			else return ((obj as District).region == this.region) && ((obj as District).localNumber == this.localNumber);
		}

		public override int GetHashCode()
		{
			return this.region.GetHashCode() ^ (int)this.localNumber;
		}

		public virtual DistrictNumber Number
		{
			get
			{
				return new DistrictNumber(region.Number, localNumber);
			}
		}
	}
}