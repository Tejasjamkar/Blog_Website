using Azure;
using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]

        public async Task<IActionResult> Add(AddTagRequests addTagRequests)
        {
            var tag = new Tag
            {
                Name = addTagRequests.Name,
                DisplayName = addTagRequests.DisplayName
            };
           await tagRepository.AddAsync(tag);
            return RedirectToAction("list");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tagslist = await tagRepository.GetAllAsync();
            return View(tagslist);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tags = await tagRepository.GetAsync(id);
            if (tags != null)
            {
                var editTagRequest = new EditTagRequests
                {
                    Id = tags.Id,
                    Name = tags.Name,
                    DisplayName = tags.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequests editTagRequests)
        {
            var tag = new Tag
            {
                Id = editTagRequests.Id,
                Name = editTagRequests.Name,
                DisplayName = editTagRequests.DisplayName
            };
            var updatedtag =await tagRepository.UpdateAsync(tag);
            if (updatedtag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequests.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequests editTagRequests)
        {
            var deletedtag = await tagRepository.DeleteAsync(editTagRequests.Id);
            if (deletedtag != null)
            {
                return RedirectToAction("List");
            }
            //showing that delete is not occur and stay on that page only
            return RedirectToAction("Edit", new { id = editTagRequests.Id });

        }
    }
}
