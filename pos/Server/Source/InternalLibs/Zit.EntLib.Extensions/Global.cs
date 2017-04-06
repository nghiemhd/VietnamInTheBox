namespace Zit.EntLib.Extensions
{
    public class Global
    {
        public enum Priority
        {
            // The values get mapped to the integer Priority values used by Enterprise Library Logging

            None = -1, // The default value if one is not specified in Enterprise Library Logging is -1. 
            Critical = 25,
            High = 20,
            Medium = 15,
            Low = 10,
            Trace = 5, // The Tracer class in Enterprise Library Logging uses a priority of 5.
            Debug = 4
        }

        public enum Category
        {
            Assert,
            Business,
            DataAccess,
            Debug,
            General,
            Security,
            Service,
            Trace,
            UI
        }
    }
}