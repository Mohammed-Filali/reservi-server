namespace server.DTOS
{
    public class ProfetionnalDTO
    {

        public int id { get; set; }
        public string user_id { get; set; }
        public string Business_name { get; set; }
        public string Description { get; set; }
        public string address { get; set; }
        public string City { get; set; }
        public int category_id { get; set; }
        public string ProfileImage { get; set; } // Required for image upload
        public string? CategoryName{ get; set; }

        // Embedded user data
        public string Status { get; set; } = null!;

        public string? Phone { get; set; }

        public string UserEmail { get; set; } = null!;
            public string UserName { get; set; } = null!;
            public string? UserPhone { get; set; }
        public string TitleService { get; set; } = null!;
        public decimal ServicePrice { get; set; }



    }
}
