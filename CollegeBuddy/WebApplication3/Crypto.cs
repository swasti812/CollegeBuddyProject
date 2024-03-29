﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication3
{
    public static class Crypto
    {
        public static string Hash(String Value)
        {
            //if (string.IsNullOrEmpty(Value)) return null;
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(Value))
                );
        }
    }
}