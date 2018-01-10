using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class UnifiedLandUseChildParcelMap : SubclassMap<UnifiedLandUseChildParcel>
	{
		public UnifiedLandUseChildParcelMap()
		{
			Schema("cadastre");
			Table("unifiedlandusechildparcel");

			KeyColumn("RegionNumber");
			KeyColumn("DistrictNumber");
			KeyColumn("BlockNumber");
			KeyColumn("Number");

			Map(x => x.HasAnotherParentBlock)
				.Column("AnotherParentBlock");
			Map(x => x.ParentParcelLocalNumber)
				.Column("ParentParcelNumber");
		}
	}
}
