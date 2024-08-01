using DBlog.Entity;

namespace DBlog.Data.Abstract
{

    public interface ITagRepository
    {

        IQueryable<Tag> Tags { get; }
        void CreateTag(Tag tag);
    }
}