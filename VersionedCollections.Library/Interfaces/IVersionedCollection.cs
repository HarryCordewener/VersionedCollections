namespace VersionedCollections.Library.Interfaces;

public interface IVersionedCollection<T> : IVersionedEnumerable<T>, IVersionedEnumerable
{
	/// <summary>
	/// Gets the number of elements contained in the System.Collections.Generic.ICollection`1.
	/// </summary>
	int[] CountV { get; }

	/// <summary>
	/// Adds an item to the System.Collections.Generic.ICollection`1.
	/// </summary>
	/// <param name="item">The object to add to the System.Collections.Generic.ICollection`1.</param>
	/// <returns>The version of the collection after the add operation.</returns>
	/// <exception cref="System.NotSupportedException">The System.Collections.Generic.ICollection`1 is read-only.</exception>
	uint AddV(T item);

	/// <summary>
	/// Removes all items from the System.Collections.Generic.ICollection`1.
	/// </summary>
	/// <returns>The version of the collection after the clear operation.</returns>
	/// <exception cref="System.NotSupportedException">The System.Collections.Generic.ICollection`1 is read-only.</exception>
	uint ClearV();

	/// <summary>
	/// Determines whether the System.Collections.Generic.ICollection`1 contains a specific value.
	/// </summary>
	/// <param name="version">The version of the collection.</param>
	/// <param name="item">The object to locate in the System.Collections.Generic.ICollection`1.</param>
	/// <returns>true if item is found in the System.Collections.Generic.ICollection`1; otherwise, false.</returns>
	bool Contains(uint version, T item);

	/// <summary>
	/// Copies the elements of the System.Collections.Generic.ICollection`1 to an System.Array, starting at a particular System.Array index.
	/// </summary>
	/// <param name="version">The version of the collection.</param>
	/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.Generic.ICollection`1. The System.Array must have zero-based indexing.</param>
	/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
	/// <exception cref="System.ArgumentNullException">array is null.</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
	/// <exception cref="System.ArgumentException">The number of elements in the source System.Collections.Generic.ICollection`1 is greater than the available space from arrayIndex to the end of the destination array.</exception>
	void CopyTo(uint version, T[] array, int arrayIndex);

	/// <summary>
	/// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection`1.
	/// </summary>
	/// <param name="item">The object to remove from the System.Collections.Generic.ICollection`1.</param>
	/// <returns>Success: true if item was successfully removed from the System.Collections.Generic.ICollection`1; otherwise, false. This method also returns false if item is not found in the original System.Collections.Generic.ICollection`1.</returns>
	/// <returns>Version: The version of the collection after the remove operation.</returns>
	/// <exception cref="System.NotSupportedException">The System.Collections.Generic.ICollection`1 is read-only.</exception>
	(bool Success, uint Version) RemoveV(T item);
}