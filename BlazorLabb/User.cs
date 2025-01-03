﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BlazorLabb
{
	public class UserTodoHandler
	{
		public User User { get; set; } = new();
		public List<ToDo> ToDos { get; set; } = new();


	}
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[RegularExpression(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,3}$", ErrorMessage = "Must contain a valid email")]
		public string? Email { get; set; }
		public string? Username { get; set; }
		public Address Address { get; set; } = new Address();
		public Company Company { get; set; } = new Company();
		public string? Phone { get; set; }
		public string? Website { get; set; }
	}


	public class Address
	{
		[Required(ErrorMessage = "Address is required")]
		public string? Street { get; set; }
		[Required(ErrorMessage = "City is required")]
		public string? City { get; set; }
		[Required(ErrorMessage = "Zip is required")]
		[StringLength(5, ErrorMessage = "Zip can't be longer than 5 characters")]
		public string? ZipCode { get; set; }
		public string? Suite { get; set; }
		public GeoLocation? Geo { get; set; } = new GeoLocation();
		
	}
	public class GeoLocation
	{
		public string? Lat { get; set; }
		public string? Lng { get; set; }
	}

	public class Company
	{
		[Required(ErrorMessage = "Company Name is required")]
		public string? Name { get; set; }
		public string? CatchPhrase { get; set; }
		public string? Bs { get; set; }
	}
	public class ToDo
	{
		public int UserId { get; set; }
		public int Id { get; set; }
		public string? Title { get; set; }
		public bool Completed { get; set; }
	}





}
