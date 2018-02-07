using FluentMigrator;

namespace LandRush.Cadastre.Russia.Database.Migrations
{
	using static Naming;

	[Migration(4)]
	public class ExtractOrganization : ForwardOnlyMigration
	{
		public override void Up()
		{
			CreateTableOrganization();
			CreateTableOrganizationPhoneNumber();
			PopulateTableOrganization();
			UpdateTableOrganizationLandholder();
		}

		private void CreateTableOrganization()
		{
			string name = "organization";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("id").AsInt32().NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("name").AsString(500).NotNullable()
				.WithColumn("tin").AsFixedLengthString(10).Nullable().WithDefaultValue(null)
				.WithColumn("address").AsString(4000).Nullable().WithDefaultValue(null);

			string seqName = SeqName(name, "id");
			Create.Sequence(seqName).InSchema("cadastre");
			Execute.Sql($"alter table cadastre.{name} alter column id set default nextval('cadastre.{seqName}')");
		}

		private void CreateTableOrganizationPhoneNumber()
		{
			string name = "organizationphonenumber";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("organizationid").AsInt32().NotNullable().PrimaryKey(pkName)
					.ForeignKey(FkName(name, "organization"), "cadastre", "organization", "id")
					.Indexed(IdxName(name, "organizationid"))
				.WithColumn("phonenumber").AsString().NotNullable().PrimaryKey(pkName);
		}

		private void PopulateTableOrganization()
		{
			string name = "organization";
			string seqName = SeqName(name, "id");
			// FIXME: PostgreSQL-specific
			Execute.Sql(@"
				insert into cadastre.organization
				select id, name
				from cadastre.organizationlandholder
			");
			Execute.Sql($"select setval('cadastre.{seqName}', (select greatest(max(id) + 1, nextval('cadastre.{seqName}')) from cadastre.{name}))");
		}

		private void UpdateTableOrganizationLandholder()
		{
			string name = "organizationlandholder";
			Alter.Table(name).InSchema("cadastre")
				.AddColumn("organizationid").AsInt32().Nullable().WithDefaultValue(null);
			// FIXME: PostgreSQL-specific
			Execute.Sql(@"
				update cadastre.organizationlandholder
				set organizationid = id
			");
			Alter.Table(name).InSchema("cadastre")
				.AlterColumn("organizationid").AsInt32().NotNullable()
					.ForeignKey(FkName(name, "organization"), "cadastre", "organization", "id");
			Delete.Column("name").FromTable(name).InSchema("cadastre");
		}
	}
}
