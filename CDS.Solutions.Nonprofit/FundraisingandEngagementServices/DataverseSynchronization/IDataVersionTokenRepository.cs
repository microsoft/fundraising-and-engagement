using Microsoft.Xrm.Sdk.Messages;

namespace FundraisingandEngagement.DataverseSynchronization
{
    /// <summary>
    /// Persistent repository for Dataverse change tracking API data version tokens.
    /// </summary>
    /// <see cref="RetrieveEntityChangesRequest.DataVersion"/>
    public interface IDataVersionTokenRepository
    {
        /// <summary>
        /// Persist token for given Dataverse table (formerly entity).
        /// </summary>
        void Put(string entityLogicalName, string dataVersionToken);

        /// <summary>
        /// Get persistent token for given Dataverse table (formerly entity) or return null if it hasn't been stored yet.
        /// </summary>
        /// <returns>Data version token or null</returns>
        string Get(string entityLogicalName);
    }
}