using System.Threading.Tasks;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IDbInitializer
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        Task SeedData();
    }
}