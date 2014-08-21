﻿namespace LandRush.Cadastre
{
	/// <summary>
	/// Категория земель
	/// </summary>
	public class LandCategory : DomainValue
	{
		protected LandCategory() { }
		public LandCategory(string code, string description) : base(code, description) { }
	}
}