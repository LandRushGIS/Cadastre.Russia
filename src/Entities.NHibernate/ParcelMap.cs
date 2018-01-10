using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelMap : ClassMap<Parcel>
	{
		public ParcelMap()
		{
			Schema("cadastre");
			Table("parcel");

			CompositeId()
				.KeyReference(
					x => x.Block,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber")
				.KeyProperty(
					x => x.LocalNumber,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));
			References(x => x.Type)
				.Access.CamelCaseField()
				.Column("TypeCode");
			References(x => x.State)
				.Access.CamelCaseField()
				.Column("StateCode");
			Map(x => x.CreationDate)
				.Access.CamelCaseField();
			Map(x => x.RemovingDate)
				.Access.CamelCaseField();
			References(x => x.LandCategory)
				.Column("LandCategoryCode");
			Component(x => x.LandUtilization)
				.ColumnPrefix("LandUtilization");
			Map(x => x.DocumentedArea);
			HasMany(x => x.Rights)
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.Inverse()
				.Cascade.AllDeleteOrphan()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.AsSet();
			HasMany(x => x.Encumbrances)
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.Cascade.AllDeleteOrphan()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.AsSet();
			HasMany(x => x.NumberedLandPieces)
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.Cascade.AllDeleteOrphan()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.Inverse()
				.AsMap("Number");
			HasMany(x => x.NumberedSubParcels)
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.Cascade.AllDeleteOrphan()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.Inverse()
				.AsMap("Number");
		}
	}
}
