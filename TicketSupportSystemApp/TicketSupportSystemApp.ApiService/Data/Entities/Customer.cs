namespace TicketSupportSystemApp.ApiService.Data.Entities;

public sealed class Customer 
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public CustomerDetails? CustomerDetails { get; private set; }
    public List<Ticket> CreatedTickets { get; private set; } = [];

    // for EF Core
    private Customer()
    {
    }

    public static Customer UpdateCustomerDetails(Customer customer, CustomerDetails customerDetails)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
        }

        customer.CustomerDetails = customerDetails ?? throw new ArgumentNullException(nameof(customerDetails), "Customer details cannot be null.");
        return customer;
    } 
}

public sealed class CustomerDetails
{
}