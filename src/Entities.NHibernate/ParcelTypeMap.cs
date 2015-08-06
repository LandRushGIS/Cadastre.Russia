using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelTypeMap : ClassMap<ParcelType>
	{
		public ParcelTypeMap()
		{
			Schema("cadastre");
			Table("parceltype");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(512);
		}
	}
}