using RegistrationManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationManager.Validations
{
    public class Validation : IValidate
    {
        public bool Login(Credential credentials)
        {
            //TODO Validation through the DB
            return true;
        }
    }
}
