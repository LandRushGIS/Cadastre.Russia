using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class MulticontourParcelContourMap : ClassMap<MulticontourParcelContour>
	{
		public MulticontourParcelContourMap()
		{
			Schema("cadastre");
			Table("multicontourparcelcontour");

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
		}
	}
}
