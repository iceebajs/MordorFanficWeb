using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MordorFanficWeb.Common
{
    public static class RegistrationPasswordValidator
    {
        public static bool Validation(string password)
        {
            Regex reg = new Regex(@"^[\P{L}\p{IsBasicLatin}]+$");
            Match result = reg.Match(password);
            return result.Success;
        }

    }
}
