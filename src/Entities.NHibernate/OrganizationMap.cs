using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class OrganizationMap : ClassMap<Organization>
	{
		public OrganizationMap()
		{
			Schema("cadastre");
			Table("organization");

			Id(x => x.Id)
				.Access.CamelCaseField();
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Length(500);
			Map(x => x.TIN)
				.Access.LowerCaseField()
				.Length(10)
				.Nullable();
			Map(x => x.Address)
				.Access.CamelCaseField()
				.Length(4000)
				.Nullable();
			HasMany(x => x.PhoneNumbers)
				.Schema("cadastre")
				.Table("organizationphonenumber")
				.KeyColumn("organizationid")
				.Element("phonenumber")
				.Cascade.AllDeleteOrphan()
				.LazyLoad()
				.Fetch.Subselect()
				.AsSet();
		}
	}
}
