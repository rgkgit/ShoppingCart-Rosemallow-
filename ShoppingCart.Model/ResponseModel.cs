namespace ShoppingCart.Model
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public dynamic Data { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
