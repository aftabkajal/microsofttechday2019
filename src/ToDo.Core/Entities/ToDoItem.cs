using ToDo.Core.SharedKernel;

namespace ToDo.Core.Entities
{
    public class ToDoItem : IEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

    }
}
