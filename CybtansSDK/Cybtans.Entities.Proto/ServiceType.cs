namespace Cybtans.Entities
{
    public enum ServiceType
    {
        /// <summary>
        /// Service contract and implementation
        /// </summary>
        Default,        
        /// <summary>
        /// Service contract
        /// </summary>
        Interface,   
        /// <summary>
        /// Service contract and partial implementation
        /// </summary>
        Partial,
        /// <summary>
        /// No service, generate message only
        /// </summary>
        None,
        /// <summary>
        /// Service contract and implementation for read operations
        /// </summary>
        ReadOnly,
    }

}
