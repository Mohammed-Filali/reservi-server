namespace server.DTOS
{
    public class PaymentDetailsDto
    {
        // For Credit Card
        public string CardLast4 { get; set; }
        public string CardHolderName { get; set; }

        // For Bank Transfer
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }

        // For PayPal
        public string PaypalEmail { get; set; }

        // For Cash
        public string ReceiptNumber { get; set; }

        public string Notes { get; set; }
    }

}
