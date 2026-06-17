using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.PostDTOs;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public Category Category { get; set; }
    public LicenceType LicenceType { get; set; }
    public decimal AnnualCost { get; set; }

}