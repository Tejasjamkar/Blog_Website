using Bloggie.Models.Domain;

namespace Bloggie.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Blogpost> Blogposts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
