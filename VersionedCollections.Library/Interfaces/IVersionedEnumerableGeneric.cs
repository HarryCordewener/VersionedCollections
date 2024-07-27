namespace VersionedCollections.Library.Interfaces;

public interface IVersionedEnumerable<out T> : IVersionedEnumerable
{
	/// <summary>
	/// Returns an enumerator that iterates through the collection at the version indicated.
	/// </summary>
	/// <param name="version">The version at which to get the Enumerator.</param>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	new IEnumerator<T> GetEnumerator(uint version);
}