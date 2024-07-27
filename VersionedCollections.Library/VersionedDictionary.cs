using OneOf;
using OneOf.Types;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using VersionedCollections.Library.Interfaces;

namespace VersionedCollections.Library;

public class VersionedDictionary<TKey, TValue> : IVersionedDictionary<TKey, TValue> where TKey : notnull
{
	private readonly Dictionary<TKey, Dictionary<uint, OneOf<TValue, None>>> _dictionary = [];
	private static readonly KeyValuePair<uint, OneOf<TValue, None>> _NOTFOUNDVALUE = new(0, new None());

	public uint Version { get; private set; } = 0;
	public List<int> internalCount = [];

	public VersionedDictionary([NotNull] Dictionary<TKey, TValue> dictionary)
	{
		ArgumentNullException.ThrowIfNull(dictionary);

		_dictionary = dictionary.ToDictionary(kvp => kvp.Key, kvp => new Dictionary<uint, OneOf<TValue, None>>() { { 0, kvp.Value } });
		internalCount.Add(1);
	}

	public VersionedDictionary()
	{
		_dictionary = [];
		internalCount.Add(0);
	}

	public TValue this[TKey key] { get => GetDictionary()[key]; set => _dictionary[key].Add(++Version, value); }

	public TValue this[uint version, TKey key] => GetDictionary(version)[key];

	public ICollection<TKey> Keys => GetDictionary().Keys;

	public ICollection<TValue> Values => GetDictionary().Values;

	public bool IsReadOnly => false;

	int ICollection<KeyValuePair<TKey, TValue>>.Count => internalCount.Last();

	int[] IVersionedCollection<KeyValuePair<TKey, TValue>>.CountV => [.. internalCount];

	public void Add(TKey key, TValue value)
	{
		if (GetDictionary().ContainsKey(key))
		{
			throw new ArgumentException("Key already set.");
		}
		else
		{
			if (_dictionary.TryGetValue(key, out Dictionary<uint, OneOf<TValue, None>>? val))
			{
				val.Add(++Version, value);
				internalCount.Add(internalCount.Last() + 1);
			}
			else
			{
				_dictionary.Add(key, new Dictionary<uint, OneOf<TValue, None>>() { { ++Version, value } });
				internalCount.Add(internalCount.Last() + 1);
			}
		}
	}

	public void Add(KeyValuePair<TKey, TValue> item)
		=> Add(item.Key, item.Value);

	public void Clear()
	{
		Version++;
		internalCount[(int)Version] = 0;

		foreach (var kvp in _dictionary)
		{
			kvp.Value.Add(Version, new None());
		}
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
		=> GetDictionary().Contains(item);

	public bool ContainsKey(TKey key)
		=> GetDictionary().ContainsKey(key);

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		=> GetDictionary().CopyTo(array, arrayIndex);

	public IDictionary<TKey, TValue> GetDictionary(uint version)
	{
		if (version > Version) throw new ArgumentException("That version does not exist.");

		return _dictionary
			.Where(kvp => kvp.Value.TakeWhile(x => x.Key <= version).LastOrDefault(_NOTFOUNDVALUE).Value.IsT0)
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.LastOrDefault(x => x.Key <= version).Value.AsT0);
	}

	public IDictionary<TKey, TValue> GetDictionary() =>
		_dictionary
			.Where(kvp => kvp.Value.LastOrDefault(_NOTFOUNDVALUE).Value.IsT0)
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Last().Value.AsT0);

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
		GetDictionary().GetEnumerator();

	public bool Remove(TKey key)
	{
		if (_dictionary.TryGetValue(key, out Dictionary<uint, OneOf<TValue, None>>? value))
		{
			value.Add(++Version, new None());
			internalCount.Add(internalCount.Last() - 1);
			return true;
		}
		else return false;
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		if (_dictionary.TryGetValue(item.Key, out Dictionary<uint, OneOf<TValue, None>>? value)
			&& value != null
			&& value.Count != 0
			&& value.Last().Value.TryPickT0(out TValue tval, out _)
			&& ((tval == null && item.Value == null) || (tval?.Equals(item.Value) ?? false)))
		{
			value.Add(++Version, new None());
			internalCount.Add(internalCount.Last() - 1);
			return true;
		}
		else return false;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		=> GetDictionary().TryGetValue(key, out value);

	IEnumerator IEnumerable.GetEnumerator()
		=> GetDictionary().GetEnumerator();

	ICollection<TKey> IVersionedDictionary<TKey, TValue>.KeysV(uint version)
		=> GetDictionary(version).Keys;

	public ICollection<(TKey Key, uint[] Versions)> AllKeys()
		// Todo: This needs to use a Range object to show the versions.
		=> _dictionary.Keys.Select(k => (k, _dictionary[k].Keys.ToArray())).ToArray();

	ICollection<TValue> IVersionedDictionary<TKey, TValue>.ValuesV(uint version)
		=> GetDictionary(version).Values;

	public bool ContainsKey(uint version, TKey key)
		=> GetDictionary(version).ContainsKey(key);

	public bool TryGetValue(uint version, TKey key, [MaybeNullWhen(false)] out TValue value)
		=> GetDictionary(version).TryGetValue(key, out value);

	public bool Contains(uint version, KeyValuePair<TKey, TValue> item)
		=> GetDictionary(version).Contains(item);

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator(uint version)
		=> GetEnumerator(version);

	IEnumerator IVersionedEnumerable.GetEnumerator(uint version)
	=> GetDictionary().GetEnumerator();

	public uint AddV(KeyValuePair<TKey, TValue> item)
	{
		Add(item);
		return Version;
	}

	public uint ClearV()
	{
		Clear();
		return Version;
	}

	public (bool Success, uint Version) RemoveV(KeyValuePair<TKey, TValue> item)
		=> (Remove(item), Version);

	public void CopyTo(uint version, KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		=> GetDictionary(version).CopyTo(array, arrayIndex);
}
