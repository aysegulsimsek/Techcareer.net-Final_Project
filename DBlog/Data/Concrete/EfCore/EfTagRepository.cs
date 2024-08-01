using DBlog.Data.Abstract;
using DBlog.Entity;

namespace DBlog.Data.Concrete
{

    public class EfTagRepository : ITagRepository
    {
        private ApplicationDbContext _context;

        public EfTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }
    }
}