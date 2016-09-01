using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public interface IRegistrationsRepository
    {
        IEnumerable<Credential> GetAllCredentials();

        Task<Boolean> CreateEntry(Credential newCredentials);
    }
}