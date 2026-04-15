namespace Ordering.API.Controllers
{
    internal class GetOrdersListQuery
    {
        private string userName;

        public GetOrdersListQuery(string userName)
        {
            this.userName = userName;
        }
    }
}