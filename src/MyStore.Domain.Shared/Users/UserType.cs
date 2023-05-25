using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Users
{
    public enum UserType : byte
    {
        Owner = 0,
        User = 10,
        Vendor = 20,
        Customer = 30
    }
}
