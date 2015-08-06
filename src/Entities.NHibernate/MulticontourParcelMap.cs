using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class MulticontourParcelMap : SubclassMap<MulticontourParcel>
	{
		public MulticontourParcelMap()
		{
			Schema("cadastre");
			Table("multicontourparcel");

			KeyColumn("RegionNumber");
			KeyColumn("DistrictNumber");
			KeyColumn("BlockNumber");
			KeyColumn("Number");

			HasMany(x => x.NumberedContours)
				.Access.CamelCaseField()
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.Cascade.All()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.AsMap("Number");
		}
	}
}