namespace Shared
{
    public class Order
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public Account Account { get; set; }
    }

    public class OrderDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public AccountDto Account { get; set; }
    }

    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Money { get; set; }
    }

    public class AccountDto
    {
        public string Name { get; set; }
        public decimal Money { get; set; }
    }
}