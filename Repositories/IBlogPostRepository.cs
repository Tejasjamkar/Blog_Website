using Bloggie.Models.Domain;

namespace Bloggie.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<Blogpost>> GetAllAsync();
        Task<Blogpost> GetAsync(Guid id);
        Task<Blogpost?> GetByUrlhandleAsync(string urlhandle);
        Task<Blogpost> UpdateAsync(Blogpost blogPost);
        Task<Blogpost?> AddAsync(Blogpost blogPost);
        Task<Blogpost?> DeleteAsync(Guid id);

    }
}
