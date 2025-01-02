using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorLabb.Components.Pages;

public partial class Users
{
	private List<User> userList = new();
	private LocalDataRepository localUserServices = new();
	private ApiDataRepository apiDataRep = new(new HttpClient());
	private JsonDataRepository jsonRep = new();
	[Inject]
	private NavigationManager? NavigationManager { get; set; }
	private string loadingMessage = "Loading";
	private string searchTerm = string.Empty;
	private bool loadingNotDone = true;
	private string dataRepoChoice = "1";
	private bool allUsers;
	private string btnMessage = "Toggle Full List";
	//private string sortUserChoice = "1";
    private bool jsonNotLoaded;
    private string jsonFileErrorMsg = "File does not exist!";
	private IEnumerable<User> filteredUsers => userList.SearchByNameOrCompany(searchTerm);

	protected override async Task OnInitializedAsync()
	{
		userList = await localUserServices.GetUsersAsync();
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
		loadingMessage = "Loading";
		StateHasChanged();
	}

	//private void SortUserList(ChangeEventArgs e)
	//{
	//	sortUserChoice = e.Value?.ToString() ?? string.Empty;
	//	if (int.TryParse(sortUserChoice, out var selectedSort))
	//	{
	//		switch (selectedSort)
	//		{
	//			case 1:
	//				{
	//					userList = userList.SortUsers(user => user.Id);
	//					break;
	//				}
	//			case 2:
	//				{
	//					userList = userList.SortUsers(user => user.Name ?? string.Empty);
	//					break;
	//				}
	//			case 3:
	//				{
	//					userList = userList.SortUsers(user => user.Company.Name ?? string.Empty);
	//					break;
	//				}
	//			case 4:
	//				{
	//					userList = userList.SortUsers(user => user.Email ?? string.Empty);
	//					break;
	//				}
	//			case 5:
	//				{
	//					userList = userList.SortUsers(user => user.Address.Street ?? string.Empty);
	//					break;
	//				}
	//			case 6:
	//				{
	//					userList = userList.SortUsers(user => user.Address.City ?? string.Empty);
	//					break;
	//				}
	//			default:
	//				{
	//					throw new ArgumentOutOfRangeException();
	//				}
	//		}

	//	}
	//	StateHasChanged();
	//}

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
					userList = await localUserServices.GetUsersAsync();
					jsonNotLoaded = false;
					break;
				}
				case 2:
				{
					userList = await apiDataRep.GetUsersAsync();
					jsonNotLoaded = false;
					break;
				}
				case 3:
				{
					if (File.Exists(jsonRep.GetFilePath()))
					{
						var jsonDataRep = new JsonDataRepository();
						userList = await jsonDataRep.GetUsersAsync();
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
		btnMessage = allUsers ? "Toggle Small List" : "Toggle Full List";
	}
	private void RedirectToDoList(int userId)
	{
		NavigationManager?.NavigateTo($"/todo/{userId}");
	}
	
}