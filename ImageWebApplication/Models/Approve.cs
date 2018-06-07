using ImageWebApplication.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebApplication.Models
{

    public class Approve
    {
        public bool m_Connect;
        public Approve()
        {
            WebClient client = WebClient.Instance;
            m_Connect = client.Connected;
        }

    }
}