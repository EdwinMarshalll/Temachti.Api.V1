namespace Temachti.Api.DTOs;

public class DTOUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Biography { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DTORole Role { get; set; }
}