namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException(string name, object key) : base($"Order with name {name} and key {key} not found.")
        {
        }
    }
}
