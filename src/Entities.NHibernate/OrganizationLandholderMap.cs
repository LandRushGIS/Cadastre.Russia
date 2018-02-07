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
			References(x => x.Organization)
				.Access.CamelCaseField()
				.Column("organizationid");
		}
	}
}
