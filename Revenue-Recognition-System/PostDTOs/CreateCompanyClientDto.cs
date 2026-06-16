namespace Revenue_Recognition_System.PostDTOs;

public class CreateCompanyClientDto : CreateBaseClientDto
{
    public string Name { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public string Krs { get; set; } = string.Empty;
}