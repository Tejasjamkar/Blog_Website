using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace Bloggie.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags=await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new Blogpost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublisheddDate = addBlogPostRequest.PublisheddDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible
            };
            //Map Tags from Selected Tags
            var selectedTags=new List<Tag>();
            foreach(var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid=Guid.Parse(selectedTagId);
                var existingTag=await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            //Mapping tags back to domain model
            blogPost.Tags = selectedTags;
          await  blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }
        //[HttpPost]
        //[ActionName("Addlist")]
        //public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        //{
        //  var blogPost = new Blogpost
        //  {
        //      Heading = addBlogPostRequest.Heading,
        //      PageTitle = addBlogPostRequest.PageTitle,
        //      Content = addBlogPostRequest.Content,
        //      ShortDescription = addBlogPostRequest.ShortDescription,
        //      FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
        //      UrlHandle = addBlogPostRequest.UrlHandle,
        //      PublisheddDate = addBlogPostRequest.PublisheddDate,
        //      Author = addBlogPostRequest.Author,
        //      Visible = addBlogPostRequest.Visible
        //  };
        //bloggiedbcontext.blogposts.Add(blogPost);
        //await bloggiedbcontext.SaveChangesAsync();
        //     return RedirectToAction("Add");
        //}
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPost = await blogPostRepository.GetAllAsync();
            return View(blogPost);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //retrieve the result from the repository
            var editData = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();
            //map the domain model into the view model
            if (editData != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = editData.Id,
                    Heading = editData.Heading,
                    PageTitle = editData.PageTitle,
                    Content = editData.Content,
                    Author = editData.Author,
                    FeaturedImageUrl = editData.FeaturedImageUrl,
                    UrlHandle = editData.UrlHandle,
                    ShortDescription = editData.ShortDescription,
                    PublisheddDate = editData.PublisheddDate,
                    Visible = editData.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = editData.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
  
                return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //Map view model back to domian model
            var blogpostdomainmodel = new Blogpost
            {
                Id=editBlogPostRequest.Id,
                Heading=editBlogPostRequest.Heading,
                PageTitle =editBlogPostRequest.PageTitle,
                Content =editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublisheddDate = editBlogPostRequest.PublisheddDate,
                Visible = editBlogPostRequest.Visible,
            };
            //map tags into domain model
            var selectedtag=new List<Tag>();
            foreach(var selectedtags in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedtags, out var tag))
                {
                  var foundtag= await tagRepository.GetAsync(tag);
                    if(foundtag!=null)
                    {
                        selectedtag.Add(foundtag);
                    }
                }
            }
            blogpostdomainmodel.Tags = selectedtag;
            //submit information to repositpry to update
            //redirect to get
           var updatedBlog=  await blogPostRepository.UpdateAsync(blogpostdomainmodel);
            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if(blogPostRepository!=null)
            {
                return RedirectToAction("List");

            }
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id});
        }
    }
}
