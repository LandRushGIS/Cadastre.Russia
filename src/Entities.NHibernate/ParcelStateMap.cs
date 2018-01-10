using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelStateMap : ClassMap<ParcelState>
	{
		public ParcelStateMap()
		{
			Schema("cadastre");
			Table("parcelstate");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(512);
		}
	}
}
