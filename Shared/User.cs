namespace Shared
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
