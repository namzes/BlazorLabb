using Microsoft.AspNetCore.Components;

namespace BlazorLabb.Components.Pages;

public partial class Users
{
	private List<User> userList = new();
	private LocalDataRepository localUserServices = new();
	private ApiDataRepository apiDataRep = new ApiDataRepository(new HttpClient());
	private JsonDataRepository jsonRep = new();
	[Inject]
	private NavigationManager? NavigationManager { get; set; }
	private string loadingMessage = "Loading";
	private string searchTerm = string.Empty;
	private bool loadingNotDone = true;
	private string dataRepoChoice = "1";
	private bool allUsers;
	private string btnMessage = "Toggle Full List View";
	private string sortUserChoice = "1";
    private bool jsonNotLoaded;
    private string jsonFileErrorMsg = "File does not exist!";
	private IEnumerable<User> filteredUsers => userList.SearchByNameOrCompany(searchTerm);

	protected override async Task OnInitializedAsync()
	{
		userList = await localUserServices.GetUsers();
		userList.SortByName();
		_ = SimulateLoading();
	}

	private async Task SimulateLoading()
	{
		for (int i = 0; i < 5; i++)
		{
			var colors = new[] { "purple", "blue", "green", "orange", "red" };
			await Task.Delay(500);
			loadingMessage += $"<span style='color:{colors[i]};'>.</span>";
			StateHasChanged();
		}

		loadingNotDone = false;
		StateHasChanged();
	}

	private void SortUsersByName()
	{
		userList = userList.SortByName();
	}

	private void SortUsersByCompanyName()
	{
		userList = userList.SortByCompany();
	}

	private void SortUsersById()
	{
		userList = userList.SortById();
	}

	private void SortUserList(ChangeEventArgs e)
	{
		sortUserChoice = e.Value?.ToString() ?? string.Empty;
		if (int.TryParse(sortUserChoice, out var selectedSort))
		{
			switch (selectedSort)
			{
				case 1:
				{
					SortUsersByName();
					break;
				}
				case 2:
				{
					SortUsersByCompanyName();
					break;
				}
				case 3:
				{
					SortUsersById();
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}
		StateHasChanged();
	}

	private async Task ChangeUserList(ChangeEventArgs e)
	{
		dataRepoChoice = e.Value?.ToString() ?? string.Empty;
		loadingNotDone = true;
		await SimulateLoading();
		if (int.TryParse(dataRepoChoice, out var selectedId))
		{
			switch (selectedId)
			{
				case 1:
				{
					userList = await localUserServices.GetUsers();
					jsonNotLoaded = false;
					break;
				}
				case 2:
				{
					userList = await apiDataRep.GetUsers();
					jsonNotLoaded = false;
					break;
				}
				case 3:
				{
					if (File.Exists(jsonRep.GetFilePath()))
					{
						var jsonDataRep = new JsonDataRepository();
						userList = await jsonDataRep.GetUsers();
					}
					else
					{
						jsonNotLoaded = true;
					}
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}


		StateHasChanged();
	}

	private void ToggleListView()
	{
		allUsers = !allUsers;
		btnMessage = allUsers ? "Toggle Full List View" : "Toggle Small List View";
	}
	private void RedirectToDoList(int userId)
	{
		NavigationManager?.NavigateTo($"/todo/{userId}");
	}
	
}