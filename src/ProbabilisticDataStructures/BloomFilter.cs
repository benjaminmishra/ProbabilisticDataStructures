namespace ProbabilisticDataStructures;

public class BloomFilter<T>
{
    private readonly bool[] _bitArray;
    private readonly List<Func<T, int>> _hashFunctions;

    public int Capacity { get; init; }

    public BloomFilter(int capacity)
    {
        Capacity = capacity;
        _bitArray = new bool[capacity];
        _hashFunctions = [];
    }

    public bool TryAdd(T item)
    {
        if (_hashFunctions.Count == 0)
            throw new InvalidOperationException("Atleast one hash function is required");

        var positionsArray = new int[_hashFunctions.Count];
        var i = 0;

        // Apply has functions and store them in a temp list
        foreach (var hashFunction in _hashFunctions)
        {
            var position = hashFunction(item);
            positionsArray[i] = position;
            i++;
        }

        if (positionsArray.Length > Capacity)
            return false;

        // Set postions since we have done all checks and are ready to commit
        foreach (var pos in positionsArray)
        {
            _bitArray[pos] = true;
        }

        return true;
    }

    public bool MayContain(T item)
    {
        if (_hashFunctions.Count == 0)
            throw new InvalidOperationException("Atleast one hash function is required");

        foreach (var hashFunction in _hashFunctions)
        {
            int probablePosition = hashFunction(item);
            if (!_bitArray[probablePosition])
                return false;
        }
        return true;
    }

    public BloomFilter<T> AddHashFunction(Func<T, int> hashFunction)
    {
        if (_hashFunctions.Count > Capacity)
            throw new InvalidOperationException("Number of hash functions cannot be greater than assigned capacity");

        _hashFunctions.Add(hashFunction);
        return this;
    }
}
