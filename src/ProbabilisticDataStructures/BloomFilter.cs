namespace ProbabilisticDataStructures;

public class BloomFilter<T>
{
    private readonly bool[] _bitArray;
    private readonly List<Func<T, int>> _hashFunction;
    
    public int Capacity {get; init;}

    internal BloomFilter(int size, List<Func<T, int>> hashFunctions)
    {
        Capacity = size;
        _bitArray = new bool[size];
        _hashFunction = hashFunctions;
    }

    public bool TryAdd(T item)
    {
        var positionsArray = new int[_hashFunction.Count];
        var i = 0;

        // Apply has functions and store them in a temp list
        foreach (var hashFunction in _hashFunction)
        {
            var position = hashFunction(item);
            positionsArray[i] = position;
            i++;
        }

        if(positionsArray.Length > Capacity)
            return false;
        
        // Set postions since we have done all checks and are ready to commit
        foreach(var pos in positionsArray)
        {
            _bitArray[pos] = true;
        }

        return true;
    }

    public bool MayContain(T item)
    {
        foreach (var hashFunction in _hashFunction)
        {
            int probablePosition = hashFunction(item);
            if (!_bitArray[probablePosition])
                return false;
        }
        return true;
    }
}
