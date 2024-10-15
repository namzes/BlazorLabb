using System.Text.Json;

namespace BlazorLabb
{
	public interface IDataRepository
	{

		Task<List<User>> GetUsers();
		Task<User> GetUser(int id);
	}

	public class ApiDataRepository : IDataRepository
	{
		private readonly HttpClient _httpClient;

		public ApiDataRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<User>> GetUsers()
		{
			var responseCall = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
			if (responseCall.IsSuccessStatusCode)
			{
				var response = await responseCall.Content.ReadAsStringAsync();
				var users = JsonSerializer.Deserialize<List<User>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return users ?? new List<User>();
			}
			return new List<User>();
		}

		public async Task<User> GetUser(int id)
		{
			var responseCall = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
			if (responseCall.IsSuccessStatusCode)
			{
				var response = await responseCall.Content.ReadAsStringAsync();
				var user = JsonSerializer.Deserialize<User>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return user ?? new User();
			}
			return new User();
		}

	}
	public class LocalDataRepository : IDataRepository
	{
		public async Task<List<User>> GetUsers()
		{
			return await Task.Run(() =>
			{
				var users = new List<User>
				{
					new User
					{
						Id = 1,
						Name = "Anna Svensson",
						Email = "anna.svensson@example.com",
						Address = new Address
						{
							Street = "Storgatan 10",
							City = "Stockholm",
							ZipCode = "111 22"
						},
						Company = new Company
						{
							Name = "Svensson AB",
							CatchPhrase = "Quality in every detail."
						}
					},
					new User
					{
						Id = 2,
						Name = "Johan Karlsson",
						Email = "johan.karlsson@example.com",
						Address = new Address
						{
							Street = "Industrivägen 5",
							City = "Göteborg",
							ZipCode = "411 01"
						},
						Company = new Company
						{
							Name = "Karlsson Industries",
							CatchPhrase = "Innovation for a better tomorrow."
						}
					},
					new User
					{
						Id = 3,
						Name = "Maria Nilsson",
						Email = "maria.nilsson@example.com",
						Address = new Address
						{
							Street = "Björkgatan 3",
							City = "Malmö",
							ZipCode = "211 23"
						},
						Company = new Company
						{
							Name = "Nilsson Consulting",
							CatchPhrase = "Your success is our mission."
						}
					},
					new User
					{
						Id = 4,
						Name = "Erik Lindgren",
						Email = "erik.lindgren@example.com",
						Address = new Address
						{
							Street = "Östra Storgatan 8",
							City = "Uppsala",
							ZipCode = "753 23"
						},
						Company = new Company
						{
							Name = "Lindgren Solutions",
							CatchPhrase = "Crafting solutions together."
						}
					},
					new User
					{
						Id = 5,
						Name = "Sara Olsson",
						Email = "sara.olsson@example.com",
						Address = new Address
						{
							Street = "Norrtullsgatan 15",
							City = "Stockholm",
							ZipCode = "113 28"
						},
						Company = new Company
						{
							Name = "Olsson & Co.",
							CatchPhrase = "Building relationships."
						}
					},
					new User
					{
						Id = 6,
						Name = "David Persson",
						Email = "david.persson@example.com",
						Address = new Address
						{
							Street = "Stenbrottsvägen 2",
							City = "Helsingborg",
							ZipCode = "252 32"
						},
						Company = new Company
						{
							Name = "Persson Enterprises",
							CatchPhrase = "Excellence in every project."
						}
					},
					new User
					{
						Id = 7,
						Name = "Linda Eriksson",
						Email = "linda.eriksson@example.com",
						Address = new Address
						{
							Street = "Köpmangatan 12",
							City = "Linköping",
							ZipCode = "582 23"
						},
						Company = new Company
						{
							Name = "Eriksson Innovations",
							CatchPhrase = "Innovation and beyond."
						}
					},
					new User
					{
						Id = 8,
						Name = "Oskar Håkansson",
						Email = "oskar.hakansson@example.com",
						Address = new Address
						{
							Street = "Västra Gatan 4",
							City = "Norrköping",
							ZipCode = "602 24"
						},
						Company = new Company
						{
							Name = "Håkansson Group",
							CatchPhrase = "Forward thinking solutions."
						}
					},
					new User
					{
						Id = 9,
						Name = "Emma Sjöberg",
						Email = "emma.sjoberg@example.com",
						Address = new Address
						{
							Street = "Södra Vägen 7",
							City = "Västerås",
							ZipCode = "721 30"
						},
						Company = new Company
						{
							Name = "Sjöberg Consulting",
							CatchPhrase = "Your partner in progress."
						}
					},
					new User
					{
						Id = 10,
						Name = "Lars Gustafsson",
						Email = "lars.gustafsson@example.com",
						Address = new Address
						{
							Street = "Skogsgatan 1",
							City = "Örebro",
							ZipCode = "703 60"
						},
						Company = new Company
						{
							Name = "Gustafsson Holdings",
							CatchPhrase = "Investing in the future."
						}
					}
				};
				return users;
			});

			

		}
		

		public async Task<User> GetUser(int id)
		{

			return await Task.FromResult(new User());
		}

	}

	public class JsonDataRepository : IDataRepository
	{
		private string filePath = "C:\\Users\\nicke\\source\\repos\\BlazorLabb\\usersjson.json";
		public JsonDataRepository() { }
		public async Task<List<User>> GetUsers()
		{
			var users = new List<User>();
			var jsonString = await File.ReadAllTextAsync(filePath);
			users = JsonSerializer.Deserialize<List<User>>(jsonString);
			return users ?? new List<User>();
		}
		public async Task<User> GetUser(int id)
		{
			try
			{
				var jsonString = await File.ReadAllTextAsync(filePath);
				var users = JsonSerializer.Deserialize<List<User>>(jsonString);
				return users?.FirstOrDefault(user => user.Id == id) ?? new User();
			}
			catch (FileNotFoundException)
			{
				return new User();
			}
			catch (JsonException ex)
			{
				Console.WriteLine(ex); //Fixa
				return new User();
			}
			
		}
		public void SaveUserToJson(User user)
		{
			List<User> users = new List<User>();
			if (File.Exists(filePath))
			{
				var existingJsonString = File.ReadAllText(filePath);
				users = JsonSerializer.Deserialize<List<User>>(existingJsonString) ?? new List<User>();
			}
			users.Add(user);
			string newJsonString = JsonSerializer.Serialize(users);
			File.WriteAllText(filePath, newJsonString);
		}

		public string GetFilePath()
		{
			return filePath;
		}

	}
	public static class DataRepoExtensions
	{
		public static List<User> SortDataRepoByName(this List<User> userList)
		{
			return userList.OrderBy(user => user.Name).ToList();
		}
		public static List<User> SortDataRepoByCompanyName(this List<User> userList)
		{
			return userList.OrderBy(user => user.Company.Name).ToList();
		}
		public static List<User> FilterDataRepo(this List<User> userList, string search)
		{
			return userList.Where(user => user.Name != null && user.Name.Contains(search, StringComparison.OrdinalIgnoreCase) 
			                              || user.Company.Name != null && user.Company.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
		}

	}


}
