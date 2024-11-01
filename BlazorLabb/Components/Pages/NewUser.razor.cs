namespace BlazorLabb.Components.Pages;

public partial class NewUser
{
	private User user = new();
	private JsonDataRepository jsonRep = new();
	private List<User> users = new();
	private int step = 1;
	private bool formIsDone;
	private bool userSaved = false;
	private bool userDeleted = false;

	protected override async Task OnInitializedAsync()
	{
		step = 1;
		if (File.Exists(jsonRep.GetFilePath()))
		{
			users = await jsonRep.GetUsers();
		}
	}

	private void NextPage()
	{
		userSaved = false;
		step++;
		StateHasChanged();
	}

	private void FinalSubmit()
	{
		formIsDone = true;
		step = 0;
	}

	private void ResetForm()
	{
		user = new();
		step = 1;
		formIsDone = false;
	}

	private void DeleteUser()
	{
		ResetForm();
		userDeleted = true;
	}

	private void SaveUser()
	{
		jsonRep.SaveUserToJson(user);
		ResetForm();
		userSaved = true;
	}
}