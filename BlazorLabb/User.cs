using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorLabb
{
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Must contain a valid email")]
		public string? Email { get; set; }
		public Address Address { get; set; } = new Address();
		public Company Company { get; set; } = new Company();

		public User()
		{
			Random random = new Random();
			Id = random.Next(1000, 10000);
		}

		
	}


	public class Address
	{
		[Required(ErrorMessage = "Address is required")]
		public string? Street { get; set; }
		[Required(ErrorMessage = "City is required")]
		public string? City { get; set; }
		[Required(ErrorMessage = "Zip is required")]
		[StringLength(5, ErrorMessage = "Zip can't be longer than 5 characters")]
		public string? Zip { get; set; }
	}

	public class Company
	{
		public string? Name { get; set; }
		public string? CatchPhrase { get; set; }
	}

	



}
