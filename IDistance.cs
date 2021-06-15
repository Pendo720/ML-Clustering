namespace KmsLibrary
{
    /// <summary>
    ///     The contract for enabling the calculation of distances between features
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IDistance<T>
    {
        float distanceTo(T other);
    }
}