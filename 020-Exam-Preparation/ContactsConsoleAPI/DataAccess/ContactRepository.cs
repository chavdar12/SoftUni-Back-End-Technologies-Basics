using ContactsConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsConsoleAPI.DataAccess;

public class ContactRepository(ContactsDbContext context) : IContactRepository
{
    public async Task AddContactAsync(Contact contact)
    {
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();
    }

    public Task<List<Contact>> GetAllContactsAsync() =>
        context.Contacts.AsNoTracking().OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();

    public Task<Contact?> GetContactByULIDAsync(string ulid) =>
        context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.ULID == ulid);

    public Task<List<Contact>> SearchContactsByFirstNameAsync(string firstName) =>
        context.Contacts.AsNoTracking().Where(c => c.FirstName.ToLower() == firstName.ToLower()).ToListAsync();

    public Task<List<Contact>> SearchContactsByLastNameAsync(string lastName) =>
        context.Contacts.AsNoTracking().Where(c => c.LastName.ToLower() == lastName.ToLower()).ToListAsync();

    public async Task<bool> UpdateContactAsync(Contact contact)
    {
        var existing = await context.Contacts.FirstOrDefaultAsync(c => c.ULID == contact.ULID);
        if (existing == null)
        {
            return false;
        }

        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.Address = contact.Address;
        existing.Phone = contact.Phone;
        existing.Email = contact.Email;
        existing.Gender = contact.Gender;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteContactAsync(string ulid) =>
        await context.Contacts.Where(c => c.ULID == ulid).ExecuteDeleteAsync() > 0;
}
