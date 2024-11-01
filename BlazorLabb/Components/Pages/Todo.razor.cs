using Microsoft.AspNetCore.Components;

namespace BlazorLabb.Components.Pages;

public partial class Todo
{
	[Parameter] public int userId { get; set; }

	private UserTodoHandler userTodoHandler = new UserTodoHandler();
	private ApiDataRepository apiDataRep;
	private Dictionary<int, bool> collapsedState = new();

	public Todo()
	{
		apiDataRep = new ApiDataRepository(new HttpClient());
	}

	protected override async Task OnInitializedAsync()
	{
		userTodoHandler.user = apiDataRep.GetUser(userId);

		userTodoHandler.ToDos = await apiDataRep.GetToDos(userId);
		foreach (var todo in userTodoHandler.ToDos)
		{
			if (todo.Completed)
			{
				collapsedState[todo.Id] = true;
			}
			else
			{
				collapsedState[todo.Id] = false;
			}
		}
	}

	private void UpdateToDo(int id)
	{
		var todo = userTodoHandler.ToDos.FirstOrDefault(todo => todo.Id == id);

		if (todo != null)
		{
			userTodoHandler.ToggleToDoCompletion(id);
			ToggleCollapse(id);
		}
	}

	private void ToggleCollapse(int id)
	{
		if (collapsedState.ContainsKey(id))
		{
			collapsedState[id] = !collapsedState[id];
		}
	}
}