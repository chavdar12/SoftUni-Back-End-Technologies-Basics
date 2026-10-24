using System.ComponentModel.DataAnnotations;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;

namespace ContactsConsoleAPI.Business;

public class ContactManager(IContactRepository repository)
{
    public async Task AddAsync(Contact contact)
    {
        Validate(contact);
        if (await repository.GetContactByULIDAsync(contact.ULID) != null)
        {
            throw new InvalidOperationException($"A contact with ULID {contact.ULID} already exists.");
        }
        await repository.AddContactAsync(contact);
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        var contacts = await repository.GetAllContactsAsync();
        return contacts.Count > 0 ? contacts : throw new KeyNotFoundException("No contact found.");
    }

    public async Task<Contact> GetSpecificAsync(string ulid)
    {
        RequireText(ulid, "ULID cannot be empty.");
        return await repository.GetContactByULIDAsync(ulid)
            ?? throw new KeyNotFoundException($"No contact found with ULID: {ulid}");
    }

    public async Task<IEnumerable<Contact>> SearchByFirstNameAsync(string firstName)
    {
        RequireText(firstName, "First name cannot be empty.");
        var contacts = await repository.SearchContactsByFirstNameAsync(firstName);
        return contacts.Count > 0 ? contacts : throw new KeyNotFoundException("No contact found with the given first name.");
    }

    public async Task<IEnumerable<Contact>> SearchByLastNameAsync(string lastName)
    {
        RequireText(lastName, "Last name cannot be empty.");
        var contacts = await repository.SearchContactsByLastNameAsync(lastName);
        return contacts.Count > 0 ? contacts : throw new KeyNotFoundException("No contact found with the given last name.");
    }

    public async Task UpdateAsync(Contact contact)
    {
        Validate(contact);
        if (!await repository.UpdateContactAsync(contact))
        {
            throw new KeyNotFoundException($"No contact found with ULID: {contact.ULID}");
        }
    }

    public async Task DeleteAsync(string ulid)
    {
        RequireText(ulid, "ULID cannot be empty.");
        if (!await repository.DeleteContactAsync(ulid))
        {
            throw new KeyNotFoundException($"No contact found with ULID: {ulid}");
        }
    }

    private static void Validate(Contact? contact)
    {
        if (contact == null)
        {
            throw new ValidationException("Invalid contact!");
        }
        Validator.ValidateObject(contact, new ValidationContext(contact), validateAllProperties: true);
    }

    private static void RequireText(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message);
        }
    }
}
