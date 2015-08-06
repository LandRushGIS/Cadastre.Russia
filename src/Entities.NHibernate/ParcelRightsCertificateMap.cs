using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelRightsCertificateMap : ClassMap<ParcelRightsCertificate>
	{
		public ParcelRightsCertificateMap()
		{
			Schema("cadastre");
			Table("parcelrightscertificate");

			CompositeId()
				.KeyProperty(
					x => x.Series,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Series"))
				.KeyProperty(
					x => x.Number,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));
			Map(x => x.Date)
				.Nullable();
			Map(x => x.RegistrationRecordNumber);
			References(x => x.Parcel)
				.Access.CamelCaseField()
				.Columns("RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber");
			References(x => x.Landholder)
				.Column("LandholderId");
		}
	}
}