using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.Interfaces;

public interface IClient
{
   public int Id { get; set; }
   public ClientTypeId ClientTypeId { get; set; }
   
   //contact info
   public string Email { get; set; }
   public string PhoneNumber { get; set; }
}