using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelEncumbranceMap : ClassMap<ParcelEncumbrance>
	{
		public ParcelEncumbranceMap()
		{
			Schema("cadastre");
			Table("parcelencumbrance");

			CompositeId()
				.KeyReference(
					x => x.Parcel,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.KeyProperty(
					Reveal.Member<ParcelEncumbrance>("number"),
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));

			References(x => x.LandEncumbranceType)
				.Access.CamelCaseField()
				.Column("LandEncumbranceTypeCode")
				.LazyLoad(Laziness.Proxy)
				.Fetch.Select();
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Length(4000);
		}
	}
}