namespace LandRush.Cadastre.Russia
{
	/// <summary xml:lang="ru">
	/// Физическое лицо
	/// </summary>
	public class Person
	{
		private readonly int id;
		private string familyName;
		private string firstName;
		private string patronymic;

		protected Person(): this(0, string.Empty, string.Empty, string.Empty) { }

		public Person(int id, string familyName, string firstName, string patronymic)
		{
			this.id = id;
			this.familyName = familyName;
			this.firstName = firstName;
			this.patronymic = patronymic;
		}

		/// <summary xml:lang="ru">
		/// Внутренний идентификатор
		/// </summary>
		public virtual int Id => this.id;

		/// <summary xml:lang="ru">
		/// Фамилия
		/// </summary>
		public virtual string FamilyName
		{
			get => this.familyName;
			set => this.familyName = value;
		}

		/// <summary xml:lang="ru">
		/// Имя
		/// </summary>
		public virtual string FirstName
		{
			get => this.firstName;
			set => this.firstName = value;
		}

		/// <summary xml:lang="ru">
		/// Отчество
		/// </summary>
		public virtual string Patronymic
		{
			get => this.patronymic;
			set => this.patronymic = value;
		}
	}
}
