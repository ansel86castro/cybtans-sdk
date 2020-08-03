namespace Cybtans.Entities
{
    public enum ServiceType
    {
        /// <summary>
        /// Service interface with full class are generated
        /// </summary>
        Default,        
        /// <summary>
        /// Service interface only is generated
        /// </summary>
        Interface,   
        /// <summary>
        /// Service interface with partiaL class are generated
        /// </summary>
        Partial,
        /// <summary>
        /// Service is not generated
        /// </summary>
        None,
    }

}
