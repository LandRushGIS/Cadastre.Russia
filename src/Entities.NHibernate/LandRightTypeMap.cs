using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandRightTypeMap : ClassMap<LandRightType>
	{
		public LandRightTypeMap()
		{
			Schema("cadastre");
			Table("landrighttype");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(100);
		}
	}
}