using System.Text.Json.Serialization;

namespace Revenue_Recognition_System.PostDTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "clientType")]
[JsonDerivedType(typeof(CreateIndividualClientDto), typeDiscriminator: "Individual")]
[JsonDerivedType(typeof(CreateCompanyClientDto), typeDiscriminator: "Company")]
public abstract class CreateBaseClientDto
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}