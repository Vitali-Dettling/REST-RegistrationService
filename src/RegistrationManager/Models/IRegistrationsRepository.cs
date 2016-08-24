using System.Collections.Generic;

namespace RegistrationManager.Models
{
    public interface IRegistrationsRepository
    {
        IEnumerable<Credential> GetAllCredentials();
    }
}