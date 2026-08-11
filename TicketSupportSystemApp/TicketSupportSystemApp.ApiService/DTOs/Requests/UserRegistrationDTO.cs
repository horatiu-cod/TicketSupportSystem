namespace TicketSupportSystemApp.ApiService.DTOs.Requests;
public record UserRegistrationDTO(string Name, string Surname, string Email, string UserName, string PhoneNumber, string Password);
