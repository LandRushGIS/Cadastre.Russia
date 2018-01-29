using System;

namespace LandRush.Cadastre.Russia
{
	// !!!!
	public class DomainValue : IComparable<DomainValue>
	{
		private string code;
		private string description;

		public DomainValue(string code, string description) // !
		{
			this.code = code;
			this.description = description;
		}

		protected DomainValue() { }

		public virtual string Code
		{
			get => this.code;
			protected set => this.code = value;
		}

		public virtual string Description
		{
			get => this.description;
			protected set => this.description = value;
		}

		public virtual int CompareTo(DomainValue other) =>
			this.code.CompareTo(other.code);

		public override string ToString() =>
			this.description; // !
	}
}
