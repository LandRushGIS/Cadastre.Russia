using FluentMigrator;

namespace LandRush.Cadastre.Russia.Database.Migrations
{
	using static Naming;

	[Migration(1)]
	public class Initialize : AutoReversingMigration
	{
		private void CreateTableRegion()
		{
			string name = "region";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("number").AsInt16().NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("name").AsString(100).Nullable().WithDefaultValue(null);
		}

		private void CreateTableDistrict()
		{
			string name = "district";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt16().NotNullable().PrimaryKey(pkName);
			Create.ForeignKey(FkName(name, "region"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumn("regionnumber")
				.ToTable("region").InSchema("cadastre")
					.PrimaryColumn("number");
		}

		private void CreateTableBlock()
		{
			string name = "block";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("name").AsString(100).Nullable().WithDefaultValue(null)
				.WithColumn("note").AsString().Nullable()
				.WithColumn("documentedarea").AsDecimal(20, 2).Nullable().WithDefaultValue(null)
				.WithColumn("geometry").AsCustom("geometry").Nullable();
			Create.ForeignKey(FkName(name, "district"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber")
				.ToTable("district").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "number");
		}

		private void CreateTableLandCategory()
		{
			string name = "landcategory";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(12).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(1000).NotNullable();
		}

		private void CreateTableCadastralValueFactor()
		{
			string name = "cadastralvaluefactor";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("landcategorycode").AsFixedLengthString(12).NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("factorvalue").AsDouble().Nullable();
			Create.ForeignKey(FkName(name, "block"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber")
				.ToTable("block").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "number");
			Create.ForeignKey(FkName(name, "landcategory"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumn("landcategorycode")
				.ToTable("landcategory").InSchema("cadastre")
					.PrimaryColumn("code");
		}

		private void CreateTableLandEncumbranceType()
		{
			string name = "landencumbrancetype";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(12).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(1000).NotNullable();
		}

		private void CreateTableLandholder()
		{
			string name = "landholder";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("id").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("name").AsString(500).NotNullable();

			string seqName = SeqName(name, "id");
			Create.Sequence(seqName).InSchema("cadastre");
			Execute.Sql($"alter table cadastre.{name} alter column id set default nextval('cadastre.{seqName}')");
		}

		private void CreateTableLandRightType()
		{
			string name = "landrighttype";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(12).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(200).Nullable().WithDefaultValue(null);
		}

		private void CreateTableLandUtilizationKind()
		{
			string name = "landutilizationkind";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(12).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(512).NotNullable();
		}

		private void CreateTableParcelState()
		{
			string name = "parcelstate";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(2).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(200).NotNullable();
		}

		private void CreateTableParcelType()
		{
			string name = "parceltype";
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("code").AsFixedLengthString(2).NotNullable().PrimaryKey(PkName(name))
				// non-key columns
				.WithColumn("description").AsString(200).NotNullable();
		}

		private void CreateTableParcel()
		{
			string name = "parcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("typecode").AsFixedLengthString(2).NotNullable()
					.ForeignKey(FkName(name, "type"), "cadastre", "parceltype", "code")
				.WithColumn("statecode").AsFixedLengthString(2).NotNullable()
					.ForeignKey(FkName(name, "state"), "cadastre", "parcelstate", "code")
				.WithColumn("creationdate").AsDate().Nullable()
				.WithColumn("removingdate").AsDate().Nullable()
				.WithColumn("landcategorycode").AsFixedLengthString(12).NotNullable()
					.ForeignKey(FkName(name, "landcategory"), "cadastre", "landcategory", "code")
				.WithColumn("landutilizationkindcode").AsFixedLengthString(12).Nullable().WithDefaultValue(null)
					.ForeignKey(FkName(name, "landutilizationkind"), "cadastre", "landutilizationkind", "code")
				.WithColumn("landutilizationdescription").AsString(4000).Nullable().WithDefaultValue(null)
				.WithColumn("documentedarea").AsDouble().NotNullable();
			Create.ForeignKey(FkName(name, "block"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber")
				.ToTable("block").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "number");
		}

		private void CreateTableParcelEncumbrance()
		{
			string name = "parcelencumbrance";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("landencumbrancetypecode").AsFixedLengthString(12).Nullable().WithDefaultValue(null)
					.ForeignKey(FkName(name, "type"), "cadastre", "landencumbrancetype", "code")
				.WithColumn("name").AsString(4000).Nullable().WithDefaultValue(null);
			Create.ForeignKey(FkName(name, "parcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number");
		}

		private void CreateTableParcelRight()
		{
			string name = "parcelright";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("landrighttypecode").AsFixedLengthString(12).Nullable().WithDefaultValue(null)
					.ForeignKey(FkName(name, "type"), "cadastre", "landrighttype", "code")
				.WithColumn("name").AsString(255).Nullable().WithDefaultValue(null)
				.WithColumn("landholderid").AsInt32().Nullable()
					.ForeignKey(FkName(name, "landholder"), "cadastre", "landholder", "id")
				.WithColumn("share").AsDecimal(20, 10).Nullable();
			Create.ForeignKey(FkName(name, "parcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number");
		}

		private void CreateTableParcelRightsCertificate()
		{
			string name = "parcelrightscertificate";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("series").AsFixedLengthString(4).NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsFixedLengthString(6).NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("date").AsDate().Nullable()
				.WithColumn("registrationrecordnumber").AsString(50).Nullable().WithDefaultValue(null)
				.WithColumn("regionnumber").AsInt16().NotNullable()
				.WithColumn("districtnumber").AsInt16().NotNullable()
				.WithColumn("blocknumber").AsInt32().NotNullable()
				.WithColumn("parcelnumber").AsInt32().NotNullable()
				.WithColumn("rightnumber").AsInt32().NotNullable();
			Create.ForeignKey(FkName(name, "parcelright"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "rightnumber")
				.ToTable("parcelright").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "number");
		}

		private void CreateTableParcelLandPiece()
		{
			string name = "parcellandpiece";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("geometry").AsCustom("geometry").NotNullable()
				.WithColumn("address").AsString(4000).Nullable().WithDefaultValue(null)
				.WithColumn("assessedvalue").AsDecimal(19, 4).Nullable().WithDefaultValue(null)
				.WithColumn("note").AsString().Nullable();
			Create.ForeignKey(FkName(name, "parcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number");
		}

		private void CreateTableSimpleParcel()
		{
			string name = "simpleparcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName);
			Create.ForeignKey(FkName(name))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.OnDelete(System.Data.Rule.Cascade);
		}

		private void CreateTableMulticontourParcel()
		{
			string name = "multicontourparcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName);
			Create.ForeignKey(FkName(name))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.OnDelete(System.Data.Rule.Cascade);
		}

		private void CreateTableMulticontourParcelContour()
		{
			string name = "multicontourparcelcontour";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName);
			Create.ForeignKey(FkName(name, "parcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber")
				.ToTable("multicontourparcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number");
		}

		private void CreateTableSubParcel()
		{
			string name = "subparcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("statecode").AsFixedLengthString(2).Nullable().WithDefaultValue(null)
					.ForeignKey(FkName(name, "state"), "cadastre", "parcelstate", "code")
				.WithColumn("landpiecenumber").AsInt32().Nullable();
			Create.ForeignKey(FkName(name, "parcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number");
			Create.ForeignKey(FkName(name, "landpiece"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "landpiecenumber")
				.ToTable("parcellandpiece").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "number");
		}

		private void CreateTableSubParcelEncumbrance()
		{
			string name = "subparcelencumbrance";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("parcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("subparcelnumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("landencumbrancetypecode").AsFixedLengthString(12).Nullable().WithDefaultValue(null)
					.ForeignKey(FkName(name, "type"), "cadastre", "landencumbrancetype", "code")
				.WithColumn("name").AsString(4000).Nullable().WithDefaultValue(null);
			Create.ForeignKey(FkName(name, "subparcel"))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "subparcelnumber")
				.ToTable("subparcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "parcelnumber", "number");
		}

		private void CreateTableUnifiedLandUseChildParcel()
		{
			string name = "unifiedlandusechildparcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName)
				// non-key columns
				.WithColumn("anotherparentblock").AsBoolean().NotNullable()
				.WithColumn("parentparcelnumber").AsInt32().NotNullable();
			Create.ForeignKey(FkName(name))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.OnDelete(System.Data.Rule.Cascade);
		}

		private void CreateTableUnifiedLandUseParcel()
		{
			string name = "unifiedlanduseparcel";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("regionnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("districtnumber").AsInt16().NotNullable().PrimaryKey(pkName)
				.WithColumn("blocknumber").AsInt32().NotNullable().PrimaryKey(pkName)
				.WithColumn("number").AsInt32().NotNullable().PrimaryKey(pkName);
			Create.ForeignKey(FkName(name))
				.FromTable(name).InSchema("cadastre")
					.ForeignColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.ToTable("parcel").InSchema("cadastre")
					.PrimaryColumns("regionnumber", "districtnumber", "blocknumber", "number")
				.OnDelete(System.Data.Rule.Cascade);
		}

		public override void Up()
		{
			CreateTableRegion();
			CreateTableDistrict();
			CreateTableBlock();
			CreateTableLandCategory();
			CreateTableCadastralValueFactor();
			CreateTableLandEncumbranceType();
			CreateTableLandholder();
			CreateTableLandRightType();
			CreateTableLandUtilizationKind();
			CreateTableParcelState();
			CreateTableParcelType();
			CreateTableParcel();
			CreateTableParcelEncumbrance();
			CreateTableParcelRight();
			CreateTableParcelRightsCertificate();
			CreateTableParcelLandPiece();
			CreateTableSimpleParcel();
			CreateTableMulticontourParcel();
			CreateTableMulticontourParcelContour();
			CreateTableSubParcel();
			CreateTableSubParcelEncumbrance();
			CreateTableUnifiedLandUseChildParcel();
			CreateTableUnifiedLandUseParcel();
		}
	}
}
