namespace Revenue_Recognition_System.PostDTOs;

public class CreateContractDto
{
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int AdditionalYears { get; set; }
    public DateTime? EndDate { get; set; }
}