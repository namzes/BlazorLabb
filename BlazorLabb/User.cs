using System.Reflection;

namespace BlazorLabb
{
	public class User
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public Address Address { get; set; } = new Address();
		public Company Company { get; set; } = new Company();
		public User() { }
		
	}


	public class Address
	{
		public string? Street { get; set; }
		public string? City { get; set; }
		public string? Zip { get; set; }
	}

	public class Company
	{
		public string? Name { get; set; }
		public string? CatchPhrase { get; set; }
	}

	



}
