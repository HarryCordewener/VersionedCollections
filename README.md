# VersionedCollections
A library for Collections with Versions, keeping track of the full Collection History, allowing for operations to get the state of a Collection during a previous Version.

# Usage

## VersionedDictionary
### Create your dictionary
```csharp
var dictionary = new VersionedDictionary<string, string>();
```

### Add and Remove items
```csharp
dictionary.Add("key", "value");
dictionary.Remove("key");
dictionary.Add("key", "value2");
```

### Get the state of the dictionary at the latest, or a previous version
```csharp
dictionary["key"]; // "value2"
dictionary[1, "key"]; // "value"
``` 