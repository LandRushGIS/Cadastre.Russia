using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelRightMap : ClassMap<ParcelRight>
	{
		public ParcelRightMap()
		{
			Schema("cadastre");
			Table("parcelright");

			CompositeId()
				.KeyReference(
					x => x.Parcel,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.KeyProperty(
					Reveal.Member<ParcelRight>("number"),
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));
			References(x => x.LandRightType)
				.Access.CamelCaseField()
				.Column("LandRightTypeCode")
				.LazyLoad(Laziness.Proxy)
				.Fetch.Select();
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Column("Name")
				.Length(255);
		}
	}
}