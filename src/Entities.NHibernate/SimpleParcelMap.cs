using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class SimpleParcelMap : SubclassMap<SimpleParcel>
	{
		public SimpleParcelMap()
		{
			Schema("cadastre");
			Table("simpleparcel");

			KeyColumn("RegionNumber");
			KeyColumn("DistrictNumber");
			KeyColumn("BlockNumber");
			KeyColumn("Number");
		}
	}
}