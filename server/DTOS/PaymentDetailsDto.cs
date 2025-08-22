namespace server.DTOS
{
    public class PaymentDetailsDto
    {
        // For Credit Card
        
        public string CardLast4 { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;

        // For Bank Transfer
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } =  string.Empty;
        public string SwiftCode { get; set; } = string.Empty;

        // For PayPal
        public string PaypalEmail { get; set; } = string.Empty;

        // For Cash
        public string ReceiptNumber { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }

}
