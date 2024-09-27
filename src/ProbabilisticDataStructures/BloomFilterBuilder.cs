namespace ProbabilisticDataStructures;

public class BloomFilterBuilder<T>
{
    private int _bloomFilterSize = 10;
    private readonly List<Func<T,int>> _hashFunctions = [];

    public BloomFilterBuilder<T> WithCapacity(int size)
    {
        if(size == 0 || size < 0)
            throw new InvalidOperationException("Size cannot be zero or negetive");

        _bloomFilterSize = size;
        return this;
    }

    public BloomFilterBuilder<T> WithHashFunction(Func<T, int> hashFunction)
    {
        _hashFunctions.Add(hashFunction);
        return this;
    }

    public BloomFilter<T> Build()
    {
        if(_hashFunctions.Count == 0) 
            throw new InvalidOperationException("Atleast one hash function is required");

        if(_hashFunctions.Count > _bloomFilterSize) 
            throw new InvalidOperationException("Number of hash functions cannot be greater than assigned capacity");

        return new BloomFilter<T>(_bloomFilterSize,_hashFunctions);
    }
}