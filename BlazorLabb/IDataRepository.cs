using System.Text.Json;

namespace BlazorLabb
{
	public interface IDataRepository
	{

		Task<List<User>> GetUsers();
		Task<User> GetUser(int id);
	}

	public class APIDataRepository : IDataRepository
	{
		private readonly HttpClient _httpClient;

		public APIDataRepository(HttpClient httpClient)
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

	
}
