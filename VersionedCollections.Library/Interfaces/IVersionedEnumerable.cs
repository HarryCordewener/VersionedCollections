using System.Collections;

namespace VersionedCollections.Library.Interfaces;

public interface IVersionedEnumerable
{
	/// <summary>
	/// Returns an enumerator that iterates through a collection at the version indicated.
	/// </summary>
	/// <param name="version">The version at which to give the Enumerator for.</param>
	/// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
	IEnumerator GetEnumerator(uint version);
}