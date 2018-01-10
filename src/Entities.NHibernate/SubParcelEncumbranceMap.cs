using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class SubParcelEncumbranceMap : ClassMap<SubParcelEncumbrance>
	{
		public SubParcelEncumbranceMap()
		{
			Schema("cadastre");
			Table("subparcelencumbrance");

			CompositeId()
				.KeyReference(
					x => x.SubParcel,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber", "SubParcelNumber")
				.KeyProperty(
					Reveal.Member<SubParcelEncumbrance>("number"),
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));

			References(x => x.LandEncumbranceType)
				.Column("LandEncumbranceTypeCode")
				.LazyLoad(Laziness.Proxy)
				.Fetch.Select();
			Map(x => x.Name)
				.Length(4000);
		}
	}
}
