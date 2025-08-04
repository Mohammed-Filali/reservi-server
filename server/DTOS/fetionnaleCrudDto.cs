namespace server.DTOS
{
    public class fetionnaleCrudDto
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string Business_name { get; set; }
        public string Description { get; set; }
        public string address { get; set; }
        public string City { get; set; }
        public int category_id { get; set; }
        public IFormFile? ProfileImage { get; set; } // Required for image upload
    }
}
