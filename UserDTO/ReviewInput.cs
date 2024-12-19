namespace E_CommerceSystem.UserDTO
{
    public class ReviewInput
    {
        public int RId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } 
        public int ProductId { get; set; }
       
        public DateTime ReviewDate { get; set; }
    }
}
