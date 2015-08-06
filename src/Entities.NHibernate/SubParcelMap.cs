using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class SubParcelMap : ClassMap<SubParcel>
	{
		public SubParcelMap()
		{
			Schema("cadastre");
			Table("subparcel");

			CompositeId()
				.KeyReference(
					x => x.Parcel,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.KeyProperty(
					x => x.LocalNumber,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));

			References(x => x.State)
				.Access.CamelCaseField()
				.Column("StateCode");
			Map(Reveal.Member<SubParcel>("landPieceNumber"))
				.Column("LandPieceNumber")
				.Nullable();
			HasMany(x => x.Encumbrances)
				.KeyColumns.Add("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber", "SubParcelNumber")
				.Cascade.All()
				.Not.LazyLoad()
				.Fetch.Subselect()
				.AsSet();
		}
	}
}