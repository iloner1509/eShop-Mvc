namespace eShop_Mvc.SharedKernel
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }

        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}