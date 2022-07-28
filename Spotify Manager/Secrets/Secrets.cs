using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Secrets
{
    public class Secrets
    {
        public string RefreshToken { get; }
        public string Base64 { get; }

        public Secrets()
        {
            RefreshToken = "AQD9-t2_V0WfqCeC4Hg743ebnwYw7Ic4U-TwM0GNoNz2BGYAuZkwdMOEIApPDk2bvcpUPyJKQW18StWSlIzQ6bKXGq6CoJBvfNIXFo5NBKwU5j-48pPr5xJiZUQ0cAmEkNw";
            Base64 = "ZmM1MDFiOWI0YmFkNDNmNGJlOTEyYTZlZGI3M2I2ZGE6NDQ1MGNkMDc1M2E4NDZlNmIxZDU4ZjYzOTNkOTdhMTk=";
        }
    }
}
