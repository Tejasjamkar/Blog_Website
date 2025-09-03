using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly Bloggiedbcontext _bloggiedbcontext;

        public TagRepository(Bloggiedbcontext bloggiedbcontext)
        {
            this._bloggiedbcontext = bloggiedbcontext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _bloggiedbcontext.Tags.AddAsync(tag);
            await _bloggiedbcontext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)  
        {
            var existingTag = await _bloggiedbcontext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                _bloggiedbcontext.Tags.Remove(existingTag);
                await _bloggiedbcontext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _bloggiedbcontext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            var tags = await _bloggiedbcontext.Tags.FirstOrDefaultAsync(x =>x.Id == id);
            return tags;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            var existingTag = await _bloggiedbcontext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _bloggiedbcontext.SaveChangesAsync();
                return existingTag;
            }

            return null; 
        }

    }
}
