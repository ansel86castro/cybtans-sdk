namespace Cybtans.Automation
{
    /// <summary>
    /// Use a Driver Context ,WaitForBarrier is set to true by default.
    /// </summary>
    public class UseDriverAttribute : DriverContextAttribute
    {
        public UseDriverAttribute(string driverName) : base(driverName)
        {
            WaitForBarrier = true;
        }
    }
}
