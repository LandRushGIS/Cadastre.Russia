namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Категория земель
	/// </summary>
	public class LandCategory : DomainValue
	{
		public LandCategory(string code, string description) : base(code, description) { }
		protected LandCategory() { }
	}
}
