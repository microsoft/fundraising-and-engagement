using System.Collections.Generic;

namespace FundraisingandEngagement.DataverseSynchronization
{
    /// <summary>
	/// Synchronizes selected data (selected tables) from Dataverse to an underlying repository.
	/// </summary>
	public interface IDataverseSynchronizer
    {
        /// <summary>
        /// Synchronizes given tables to an underlying repository.
        /// For each table, if this is the first synchronization, all Dataverse table rows will be downloaded and persisted to the underlying data store;
        /// otherwise a successful execution of this method guarantees that all changes in Dataverse since last synchronization have been reflected, i.e. creations, updates, and deletes are applied.
        ///
        /// Execution is not guaranteed to be atomic, repository is only eventually consistent when the method successfully finishes.
        ///
        /// The order of synchronization is guaranteed to be as follows:
        /// - First all Updates and Creates are applied for each table in the order in which tables are given in <paramref name="tablesToSynchronize"/>.
        /// - Then all Deletes are applied for each table in the reverse order of <paramref name="tablesToSynchronize"/>.
        /// This ordering is preserved to avoid problems with foreign keys when persisting to a database.
        /// </summary>
        /// <param name="tablesToSynchronize">Set of Dataverse tables (formerly entities) to synchronize</param>
        void Synchronize(IEnumerable<string> tablesToSynchronize);
    }
}