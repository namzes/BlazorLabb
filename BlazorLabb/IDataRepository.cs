using System.Text.Json;

namespace BlazorLabb
{
	public interface IDataRepository
	{
		Task<List<User>> GetUsersAsync();
		User GetUser(int id);
	}
	public class ApiDataRepository : IDataRepository
	{
		private List<User>? users { get; set; }
		private readonly HttpClient _httpClient;

		public ApiDataRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<User>> GetUsersAsync()
		{
			users = await _httpClient.GetFromJsonAsync<List<User>>("https://jsonplaceholder.typicode.com/users", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return users ?? new List<User>();
		}

		public User GetUser(int id)
		{
			return users?.GetUser(id) ?? new User();
		}
		public async Task<List<ToDo>> GetToDosAsync(int userId)
		{
			var todos = await _httpClient.GetFromJsonAsync<List<ToDo>>($"https://jsonplaceholder.typicode.com/todos?userId={userId}", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return todos ?? new List<ToDo>();
		}

	}
	public class LocalDataRepository : IDataRepository
	{
		private List<User>? users { get; set; }
		public async Task<List<User>> GetUsersAsync()
		{
			return await Task.Run(() =>
			{
				users = new List<User>
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
		public User GetUser(int id)
		{

			return users?.GetUser(id) ?? new User();
		}

	}
	public class JsonDataRepository : IDataRepository
	{
		private string fileName = "usersjson.json";
		private List<User>? users { get; set; }
		public async Task<List<User>> GetUsersAsync()
		{
			users = new List<User>();
			try
			{
				using (var stream = File.OpenRead(GetFilePath()))
				{
					users = await JsonSerializer.DeserializeAsync<List<User>>(stream);
					return users ?? new List<User>();
				}
			}
			catch (FileNotFoundException)
			{
				return new List<User>();
			}
			catch (JsonException ex)
			{
				Console.WriteLine(ex);
				return new List<User>();
			}

		}
		public User GetUser(int id)
		{
			try
			{
				using (var stream = File.OpenRead(GetFilePath()))
				{
					return users?.GetUser(id) ?? new();
				}
			}
			catch (FileNotFoundException)
			{
				return new User();
			}
			catch (JsonException ex)
			{
				Console.WriteLine(ex);
				return new User();
			}
			
		}
		public void SaveUserToJson(User user)
		{
			List<User> users = new List<User>();
			users = users.JsonDeserializeUsersFile();
			users.Add(user);
			string newJsonString = JsonSerializer.Serialize(users);
			File.WriteAllText(GetFilePath(), newJsonString);
		}

		public void DeleteUserFromJson(int id)
		{
			List<User> users = new List<User>();
			users = users.JsonDeserializeUsersFile();
			int index = users.FindIndex(user => id == user.Id);
			users.RemoveAt(index);
			string newJsonString = JsonSerializer.Serialize(users);
			File.WriteAllText(GetFilePath(), newJsonString);
		}

		
	

		public string GetFilePath()
		{
			return fileName;
		}
		public int ApplyRandomUniqueId()
		{
			Random random = new Random();
			if (users == null)
			{
				return random.Next(1000, 10000);
			}
			int id;
			do
			{
				id = random.Next(1000, 10000);
			}
			while (users.Any(user => user.Id == id));
			return id;

		}

	}
	public static class DataRepoExtensions
	{
		public static List<T> SortUsers<T>(this List<T> userList, Func<T, object> sortFunc)
		{
				List<T> userAscending = userList.OrderBy(sortFunc).ToList();
				if (userList.SequenceEqual(userAscending))
				{
					return userList.OrderByDescending(sortFunc).ToList();
				}
				return userList.OrderBy(sortFunc).ToList();
		}
		public static List<User> SearchByNameOrCompany(this List<User> userList, string search)
		{
			return userList.Where(user => user.Name != null && user.Name.Contains(search, StringComparison.OrdinalIgnoreCase) 
			                              || user.Company.Name != null && user.Company.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
		}
		public static User GetUser(this List<User> users, int chosenId)
		{
			return users.FirstOrDefault(user => user.Id == chosenId) ?? new User();
		}
		public static bool ToggleToDoCompletion(this UserTodoHandler userTodoHandler, int id)
		{

			var todo = userTodoHandler.ToDos.FirstOrDefault(todo => todo.Id == id);
			if (todo != null)
			{
				todo.Completed = !todo.Completed;
				return todo.Completed;
			}
			return false;
		}
		public static List<User> JsonDeserializeUsersFile(this List<User> users)
		{
			JsonDataRepository json = new();
			if (File.Exists(json.GetFilePath()))
			{
				var existingJsonString = File.ReadAllText(json.GetFilePath());
				return JsonSerializer.Deserialize<List<User>>(existingJsonString) ?? new();
			}
			return new();

		}
	}


}
