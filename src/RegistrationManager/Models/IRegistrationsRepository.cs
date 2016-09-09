using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public interface IRegistrationsRepository
    {
        Task<Boolean> CreateEntry(Credential newCredentials);
    }
}