using Bloggie.Models.Domain;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(); // The name should be anything but it is understandable 
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> UpdateAsync(Tag tag);
        Task<Tag?> AddAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);

    }
}
