using UnityEngine;

namespace DataLayer
{
    public static class Data
    {
        public static SessionData Session { get; private set; }

        static Data()
        {
            Session = new SessionData();
        }
    }
}
