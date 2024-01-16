public interface IShuffler
{
    void Shuffle<Type>(Type[] values);
}

public class RandomShuffler : IShuffler
{
    public void Shuffle<Type>(Type[] values)
    {
        Random.Shared.Shuffle(values);
    }
}

public class DoNothingShuffler : IShuffler
{
    public void Shuffle<Type>(Type[] values)
    {
    }
}