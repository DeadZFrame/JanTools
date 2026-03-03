namespace Jan.Maths
{
    public interface IWeightedRNG<T>
    {
        public WeightedRNGItem<T> RNG { get; }
    }
}