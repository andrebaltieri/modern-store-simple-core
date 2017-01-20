namespace ModernStore.Domain.Commands
{
    public class RegisterCustomerCommand : ICommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}