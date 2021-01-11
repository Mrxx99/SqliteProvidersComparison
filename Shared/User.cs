namespace Shared
{
    public class User
    {
        public string Name { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
