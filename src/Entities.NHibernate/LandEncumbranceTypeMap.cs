using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandEncumbranceTypeMap : ClassMap<LandEncumbranceType>
	{
		public LandEncumbranceTypeMap()
		{
			Schema("cadastre");
			Table("landencumbrancetype");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(100);
		}
	}
}