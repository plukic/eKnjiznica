﻿using eKnjiznica.CORE.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKnjiznica.CORE.Services.Admin
{
    public interface IAdminService
    {
        void AddAdminAccount(AdminAccount adminAccount,string password);
        AdminAccount FindByUsername(string username);
        AdminAccount FindByUsernameOrEmail(string username, string email);

        void ToggleAdminAccountStatus(string id);
        AdminAccount FindById(string id);
        AdminAccount UpdateAdminAccount(AdminAccount adminAccount);

        IList<AdminAccount> GetAdminAccountList(string username);

    }
}
