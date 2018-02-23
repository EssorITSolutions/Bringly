namespace Bringly.DomainLogic.Settings
{
    public static class SystemSettings
    {
        public static string Error404PageUrl
        {
            get
            {
                return "~/Error/Error404";
            }
        }
        public static int DefaultPageSize
        {
            get
            {
                return 5;
            }
        }
    }
}
