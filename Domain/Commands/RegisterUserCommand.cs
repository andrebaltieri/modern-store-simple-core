namespace ModernStore.Domain.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}