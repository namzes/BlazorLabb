using System.Text.Json;

namespace BlazorLabb
{
	public interface IDataRepository
	{

		Task<List<DataRepoUser>> GetUsers();
		Task<DataRepoUser> GetUser(int id);
	}

	public class APIDataRepository : IDataRepository
	{
		private readonly HttpClient _httpClient;

		public APIDataRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<DataRepoUser>> GetUsers()
		{
			var responseCall = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
			if (responseCall.IsSuccessStatusCode)
			{
				var response = await responseCall.Content.ReadAsStringAsync();
				var users = JsonSerializer.Deserialize<List<DataRepoUser>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return users ?? new List<DataRepoUser>();
			}
			return new List<DataRepoUser>();
		}

		public async Task<DataRepoUser> GetUser(int id)
		{
			var responseCall = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
			if (responseCall.IsSuccessStatusCode)
			{
				var response = await responseCall.Content.ReadAsStringAsync();
				var user = JsonSerializer.Deserialize<DataRepoUser>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return user ?? new DataRepoUser();
			}
			return new DataRepoUser();
		}

	}

	
}
