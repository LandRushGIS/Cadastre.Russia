using FluentMigrator;

namespace LandRush.Cadastre.Russia.Database.Migrations
{
	using System;
	using static Naming;

	[Migration(2)]
	public class IntroduceLandholderClassification : Migration
	{
		private void CreateTablePerson()
		{
			string name = "person";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("id").AsInt32().NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("familyname").AsString(100).NotNullable()
				.WithColumn("firstname").AsString(100).NotNullable()
				.WithColumn("patronymic").AsString(100).Nullable().WithDefaultValue(null);

			string seqName = SeqName(name, "id");
			Create.Sequence(seqName).InSchema("cadastre");
			Execute.Sql($"alter table cadastre.{name} alter column id set default nextval('cadastre.{seqName}')");
		}

		private void CreateTablePassport()
		{
			string name = "passport";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("series").AsFixedLengthString(4).NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsFixedLengthString(6).NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("issuedate").AsDate().NotNullable()
				.WithColumn("issuer").AsString(200).NotNullable()
				.WithColumn("personid").AsInt32().NotNullable()
					.ForeignKey(FkName(name, "person"), "cadastre", "person", "id");
		}

		private void CreateTablePersonLandholder()
		{
			string name = "personlandholder";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("id").AsInt32().NotNullable().PrimaryKey(PkName(name))
					.ForeignKey(FkName(name), "cadastre", "landholder", "id")
				// non-key columns
				.WithColumn("personid").AsInt32().NotNullable()
					.ForeignKey(FkName(name, "person"), "cadastre", "person", "id");
		}

		private void CreateTableOrganizationLandholder()
		{
			string name = "organizationlandholder";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("id").AsInt32().NotNullable().PrimaryKey(PkName(name))
					.ForeignKey(FkName(name), "cadastre", "landholder", "id")
				// non-key columns
				.WithColumn("name").AsString(500).NotNullable();
		}

		private void PopulateTablePerson()
		{
			string name = "person";
			string seqName = SeqName(name, "id");
			// FIXME: PostgreSQL-specific
			Execute.Sql(@"
				insert into cadastre." + name + @"
				select
					id,
					split_part(trim(name), ' ', 1) as familyname,
					split_part(trim(name), ' ', 2) as firstname,
					split_part(trim(name), ' ', 3) as patronymic
				from cadastre.landholder
				where array_length(regexp_split_to_array(trim(name), E'\\s+'), 1) = 3
			");
			Execute.Sql($"select setval('cadastre.{seqName}', (select greatest(max(id) + 1, nextval('cadastre.{seqName}')) from cadastre.{name}))");
		}

		private void PopulateTablePersonLandholder()
		{
			Execute.Sql(@"
				insert into cadastre.personlandholder
				select
					id,
					id as personid
				from cadastre.person
			");
		}

		private void PopulateTableOrganizationLandholder()
		{
			// FIXME: PostgreSQL-specific
			Execute.Sql(@"
				insert into cadastre.organizationlandholder
				select id, trim(name) as name
				from cadastre.landholder
				where array_length(regexp_split_to_array(trim(name), E'\\s+'), 1) <> 3
			");
		}

		private void UpdateTableLandholder()
		{
			string name = "landholder";
			Alter.Table(name).InSchema("cadastre")
				// 0 - person landholder
				// 1 - organization landholder
				//
				// set to person by default
				.AddColumn("type").AsInt32().SetExistingRowsTo(0);

			// change to 1 for organizations
			Execute.Sql(@"
				update cadastre.landholder
				set type = 1
				where id in (select id from cadastre.organizationlandholder)
			");

			Delete.Column("name").FromTable(name).InSchema("cadastre");
		}

		private void UpdateTableParcelRight()
		{
			string name = "parcelright";
			Alter.Table(name).InSchema("cadastre")
				.AddColumn("sharenumerator").AsInt16()
				.AddColumn("sharedenominator").AsInt16()
				.AddColumn("sharetext").AsString(255)
				.AddColumn("description").AsString(255);
			// TODO: migrate share values
			Delete.Column("share").FromTable(name).InSchema("cadastre");
		}

		public override void Up()
		{
			CreateTablePerson();
			CreateTablePassport();
			CreateTablePersonLandholder();
			CreateTableOrganizationLandholder();
			PopulateTablePerson();
			PopulateTablePersonLandholder();
			PopulateTableOrganizationLandholder();
			UpdateTableLandholder();
			UpdateTableParcelRight();
		}

		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}
