namespace HotelBookingSystemAPI.Interfaces
{
    public interface IRepositoryForCompositeKey<K1,K2,T> where T : class
    {
        public Task<T> Add(T item);
        public Task<T> Delete(K1 key1 , K2 key2 );
        public Task<T> Update(T item);
        public Task<T> Get(K1 key1, K2 key2);
        public Task<IEnumerable<T>> Get();
    }
}
