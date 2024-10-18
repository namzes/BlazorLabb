using Microsoft.AspNetCore.Components;

namespace BlazorLabb.Components.Pages;

public partial class Users
{
	private List<User> userList = new();
	private LocalDataRepository localUserServices = new();
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
	private string sortBtnMessage = "Sort by Company Name";
    private bool jsonNotLoaded = false;
    private string jsonFileErrorMsg = "File does not exist!";
	private IEnumerable<User> filteredUsers => userList.FilterDataRepo(searchTerm);

	protected override async Task OnInitializedAsync()
	{
		userList = await localUserServices.GetUsers();
		SortUsersByName();
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
		userList = userList.SortDataRepoByName();
	}

	private void SortUsersByCompanyName()
	{
		userList = userList.SortDataRepoByCompanyName();
	}

	private void SortUsersById()
	{
		userList = userList.SortDataRepoById();
	}

	private void SortUserList(ChangeEventArgs e)
	{
		sortUserChoice = e?.Value?.ToString() ?? string.Empty;
		if (int.TryParse(sortUserChoice, out var selectedSort))
		{
			if (selectedSort == 1)
			{
				userList = userList.SortDataRepoByName();
			}
			else if (selectedSort == 2)
			{
				userList = userList.SortDataRepoByCompanyName();
			}
			else if (selectedSort == 3)
			{
				userList = userList.SortDataRepoById();
			}
		}
		StateHasChanged();
	}

	private async Task ChangeUserRepo(ChangeEventArgs e)
	{
		dataRepoChoice = e?.Value?.ToString() ?? string.Empty;
		loadingNotDone = true;
		_ = SimulateLoading();
		if (int.TryParse(dataRepoChoice, out var selectedId))
		{
			if (selectedId == 1)
			{
				userList = await localUserServices.GetUsers();
                jsonNotLoaded = false;
            }
			else if (selectedId == 2)
			{
				var apiDataRep = new ApiDataRepository(new HttpClient());
				userList = await apiDataRep.GetUsers();
                jsonNotLoaded = false;
            }
			else if (selectedId == 3)
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
			}
		}


		StateHasChanged();
	}

	private void ToggleListView()
	{
		if (!allUsers)
		{
			allUsers = true;
			btnMessage = "Toggle Small List View";
		}
		else if (allUsers)
		{
			allUsers = false;
			btnMessage = "Toggle Full List View";
		}
	}
	private void RedirectToDoList(int userId)
	{
		NavigationManager?.NavigateTo($"/todo/{userId}");
	}
	
}