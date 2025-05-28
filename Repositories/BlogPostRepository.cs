using Bloggie.Data;
using Bloggie.Models.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly Bloggiedbcontext bloggiedbcontext;

        public BlogPostRepository(Bloggiedbcontext bloggiedbcontext)
        {
            this.bloggiedbcontext = bloggiedbcontext;
        }

        public async Task<Blogpost?> AddAsync(Blogpost blogPost)
        {
            await bloggiedbcontext.AddAsync(blogPost);
            await bloggiedbcontext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<Blogpost?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggiedbcontext.blogposts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggiedbcontext.blogposts.Remove(existingBlog);
                await bloggiedbcontext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<Blogpost>> GetAllAsync()
        {
            return await bloggiedbcontext.blogposts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<Blogpost?> GetAsync(Guid id)
        {
            return await bloggiedbcontext.blogposts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Blogpost?> GetByUrlhandleAsync(string urlhandle)
        {
          return await bloggiedbcontext.blogposts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.UrlHandle== urlhandle);
        }

        public async Task<Blogpost> UpdateAsync(Blogpost blogPost)
        {
            var existingBlog = await bloggiedbcontext.blogposts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Content = blogPost.Content;
                existingBlog.PublisheddDate = blogPost.PublisheddDate;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Tags = blogPost.Tags;
                await bloggiedbcontext.SaveChangesAsync();
                return existingBlog;
            }
            return null;

        }
    }
}
