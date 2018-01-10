using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class OrganizationLandholderMap : SubclassMap<OrganizationLandholder>
	{
		public OrganizationLandholderMap()
		{
			Schema("cadastre");
			Table("organizationlandholder");

			KeyColumn("id");
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Length(500);
		}
	}
}
