namespace ShoppingCart.Helper
{
    public class AuditInfo
    {
        public int UserId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RequestURL { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
    }
}
