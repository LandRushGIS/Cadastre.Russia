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
						.Access.CamelCaseField().Not.Lazy(),
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
			References(x => x.Landholder)
				.Column("LandholderId");
			Map(x => x.ShareNumerator)
				.Access.CamelCaseField()
				.Column("ShareNumerator");
			Map(x => x.ShareDenominator)
				.Access.CamelCaseField()
				.Column("ShareDenominator");
			Map(x => x.ShareText)
				.Access.CamelCaseField()
				.Column("ShareText");
			Map(x => x.Description)
				.Access.CamelCaseField()
				.Column("Description");
		}
	}
}