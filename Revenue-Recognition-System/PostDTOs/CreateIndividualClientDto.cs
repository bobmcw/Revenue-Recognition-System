namespace Revenue_Recognition_System.PostDTOs;

public class CreateIndividualClientDto : CreateBaseClientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Pesel { get; set; } = string.Empty;
}