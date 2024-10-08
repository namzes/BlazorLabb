namespace BlazorLabb
{
	
	public class DataRepoUser : IUser
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public DataRepoAddress DataRepoAddress { get; set; } = new DataRepoAddress();
		public Address? Address
		{
			get => new Address
			{
				Street = DataRepoAddress.Street,
				City = DataRepoAddress.City,
				Zip = DataRepoAddress.Zipcode 
			};
		}
		public string? Phone { get; set; }
		public string? Website { get; set; }
		public DataRepoCompany DataRepoCompany { get; set; } = new DataRepoCompany();
		public Company? Company
		{
			get => new Company
			{
				Name = DataRepoCompany.Name,
				CatchPhrase = DataRepoCompany.CatchPhrase
			};
		}
	}
	
	public class DataRepoAddress
	{
		public string? Street { get; set; }
		public string? Suite { get; set; }
		public string? City { get; set; }
		public string? Zipcode { get; set; }
		public DataRepoGeo? Geo { get; set; } = new DataRepoGeo();
		public class DataRepoGeo
		{
			public string? Lat { get; set; }
			public string? Lng { get; set; }
		}
		
		
	}
	public class DataRepoCompany
	{
		public string? Name { get; set; }
		public string? CatchPhrase { get; set; }
		public string? Bs { get; set; }
	}
}
