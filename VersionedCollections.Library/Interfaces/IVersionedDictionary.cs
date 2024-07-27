using System.Diagnostics.CodeAnalysis;

namespace VersionedCollections.Library.Interfaces;

public interface IVersionedDictionary<TKey, TValue> :
						IDictionary<TKey, TValue>,
						IVersionedCollection<KeyValuePair<TKey, TValue>>,
						IVersionedEnumerable<KeyValuePair<TKey, TValue>>,
						IVersionedEnumerable
{
	/// <summary>
	/// Gets the element with the specified key at that version.
	/// </summary>
	/// <param name="version">The version of the dictionary to check.</param>
	/// <param name="key">The key of the element to get or set.</param>
	/// <returns>The element with the specified key.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
	/// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
	TValue this[uint version, TKey key] { get; }

	/// <summary>
	/// Gets an ICollection containing the keys of the IDictionary.
	/// </summary>
	/// <param name="version">The version of the dictionary to check.</param>
	/// <returns>An ICollection containing the keys of the object at that version that implements IDictionary.</returns>
	ICollection<TKey> KeysV(uint version);

	/// <summary>
	/// Gets an ICollection containing all historic keys of the object, and what versions they were in that implements IDictionary.
	/// </summary>
	/// <returns>An ICollection containing all historic keys of the object, and what versions they were in that implements IDictionary.</returns>
	ICollection<(TKey Key, uint[] Versions)> AllKeys();

	/// <summary>
	/// Gets an ICollection containing the values in the IDictionary.
	/// </summary>
	/// <param name="version">The version of the dictionary to check.</param>
	/// <returns>An ICollection containing the values in the object that implements IDictionary.</returns>
	ICollection<TValue> ValuesV(uint version);

	/// <summary>
	/// Determines whether the IDictionary contains an element with the specified key.
	/// </summary>
	/// <param name="version">The version of the dictionary to check.</param>
	/// <param name="key">The key to locate in the IDictionary.</param>
	/// <returns>true if the IDictionary contains an element with the key in that version; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
	bool ContainsKey(uint version, TKey key);

	/// <summary>
	/// Gets the value associated with the specified key.
	/// </summary>
	/// <param name="version">The version of the dictionary to check.</param>
	/// <param name="key">The key whose value to get.</param>
	/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
	/// <returns>true if the object that implements IDictionary contains an element with the specified key; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
	bool TryGetValue(uint version, TKey key, [MaybeNullWhen(false)] out TValue value);

	/// <summary>
	/// Gets the dictionary values at the specific version as a standard dictionary.
	/// </summary>
	/// <param name="version">The version of the dictionary.</param>
	/// <returns>A plain dictionary.</returns>
	IDictionary<TKey, TValue> GetDictionary(uint version);

	/// <summary>
	/// Gets the latest dictionary values.
	/// </summary>
	/// <returns>A plain dictionary.</returns>
	IDictionary<TKey, TValue> GetDictionary();
}