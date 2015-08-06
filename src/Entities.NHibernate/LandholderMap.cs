﻿using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandholderMap : ClassMap<Landholder>
	{
		public LandholderMap()
		{
			Schema("cadastre");
			Table("landholder");

			Id(x => x.Id)
				.Access.CamelCaseField();
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Length(500);
		}
	}
}