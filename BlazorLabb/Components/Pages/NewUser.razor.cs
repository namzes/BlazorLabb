namespace BlazorLabb.Components.Pages;

public partial class NewUser
{
	private User user = new();
	private JsonDataRepository jsonRep = new();
	private List<User> users = new();
	private int step = 1;
	private bool formIsDone;
	private bool userSaved;
	private bool userDeleted;

	protected override async Task OnInitializedAsync()
	{
		step = 1;
		if (File.Exists(jsonRep.GetFilePath()))
		{
			users = await jsonRep.GetUsersAsync() ?? new();
		}
		else
		{
			users = new List<User>(); 
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

	private void DiscardUser()
	{
		ResetForm();
		userDeleted = true;
	}

	private void DeleteUser(int id)
	{
		jsonRep.DeleteUserFromJson(id);
		if (users != null)
		{
			var userToDelete = users.FirstOrDefault(user => user.Id == id);
			if (userToDelete != null)
			{
				users.Remove(userToDelete); 
			}
		}
		StateHasChanged();
	}

	private void SaveUser()
	{
		user.Id = jsonRep.ApplyRandomUniqueId();
		jsonRep.SaveUserToJson(user);
		users.Add(user);
		ResetForm();
		userSaved = true;
	}
}