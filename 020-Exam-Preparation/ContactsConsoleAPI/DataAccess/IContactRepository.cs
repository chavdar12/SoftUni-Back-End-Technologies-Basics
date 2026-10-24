using ContactsConsoleAPI.Data.Models;

namespace ContactsConsoleAPI.DataAccess;

public interface IContactRepository
{
    Task AddContactAsync(Contact contact);
    Task<List<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByULIDAsync(string ulid);
    Task<List<Contact>> SearchContactsByFirstNameAsync(string firstName);
    Task<List<Contact>> SearchContactsByLastNameAsync(string lastName);
    Task<bool> UpdateContactAsync(Contact contact);
    Task<bool> DeleteContactAsync(string ulid);
}
