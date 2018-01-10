using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class UnifiedLandUseParcelMap : SubclassMap<UnifiedLandUseParcel>
	{
		public UnifiedLandUseParcelMap()
		{
			Schema("cadastre");
			Table("unifiedlanduseparcel");

			KeyColumn("RegionNumber");
			KeyColumn("DistrictNumber");
			KeyColumn("BlockNumber");
			KeyColumn("Number");
		}
	}
}
