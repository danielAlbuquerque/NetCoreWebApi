using Flunt.Notifications;
using Flunt.Validations;

namespace TodoApi.ViewModels.TodoViewModel
{
    public class EditorViewModel : Notifiable, IValidatable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .HasMaxLen(Name, 120, "Name", "O nome nao pode ser maior que 120 caracteres")
                    .HasMinLen(Name, 3, "Name", "O nome nao pode ser menor que 3 caracteres")
            );
        }
    }
}