namespace BlazorLabb.Components.Pages;

public partial class Users
{
	private List<User> userList = new();
	private LocalDataRepository localUserServices = new();
	private string loadingMessage = "Loading";
	private string searchTerm = string.Empty;
	private bool loadingNotDone = true;
	private bool localDataBase = true;
	private string changeUserRepo = "Fetch Users from API";
	private bool allUsers;
	private string btnMessage = "Toggle Full List View";
	private bool sortCompanyName = false;
	private string sortBtnMessage = "Sort by Company Name";
	private IEnumerable<User> filteredUsers => userList.FilterDataRepo(searchTerm);

	protected override async void OnInitialized()
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

	private void OnSortButtonClick()
	{
		if (!sortCompanyName)
		{
			SortUsersByCompanyName();
			sortBtnMessage = "Sort by Name";
		}
		else
		{
			SortUsersByName();
			sortBtnMessage = "Sort by Company name";
		}

		sortCompanyName = !sortCompanyName;
		StateHasChanged();
	}

	private async void ChangeUserRepo()
	{
		loadingNotDone = true;
		_ = SimulateLoading();

		if (!localDataBase)
		{
			userList = await localUserServices.GetUsers();
			changeUserRepo = "Fetch Users from API";
			localDataBase = true;
		}
		else if (localDataBase)
		{
			var apiDataRep = new APIDataRepository(new HttpClient());
			userList = await apiDataRep.GetUsers();
			changeUserRepo = "Fetch Local Users";
			localDataBase = false;
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
}