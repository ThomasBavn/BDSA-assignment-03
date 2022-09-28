namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{

    readonly KanbanContext context;
    //private int index;
    public TagRepository(KanbanContext context)
    {
        this.context = context;
        //index = context.Tags != null && context.Tags.Any() ? context.Tags.Max(t => t.Id) + 1 : 0;
    }

    //select fra KanbanContext get Tags
    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var tags = context.Tags;

        //Trying to create a tag which exists already should return Conflict.
        var checkIfExists = (from t in tags
                             where t.Name.Equals(t.Name)
                             select t).ToList();
        if (checkIfExists.Count > 0)
        {
            return (Response.Conflict, checkIfExists[0].Id);
        }

        //Create new Tag and add to database
        var newTag = new Tag();
        //newTag.Id = index++;
        newTag.Name = tag.Name;
        context.Add(newTag);
        context.SaveChanges();
        return (Response.Created, newTag.Id);
    }

    public IReadOnlyCollection<TagDTO> Read()
    {
        var tags = context.Tags;

        var read = (from t in tags
                    select new TagDTO(
                        t.Id,
                        t.Name
                    )).ToList().AsReadOnly();

        return read;
    }

    public TagDTO Find(int tagId)
    {
        var tags = context.Tags;

        var findTag = (from t in tags
                       where t.Id == tagId
                       select new TagDTO(
                           t.Id,
                           t.Name
                           )).ToList();

        var foundTag = findTag[0];

        return foundTag;
    }

    public Response Update(TagUpdateDTO tag)
    {
        var tags = context.Tags;

        var findTag = (from t in tags
                       where t.Id == tag.Id
                       select t).ToList();

        if (findTag.Count == 0)
        {
            return Response.NotFound;
        }

        var foundTag = findTag[0];

        foundTag.Name = tag.Name;
        context.SaveChanges();

        return Response.Updated;
    }

    public Response Delete(int tagId, bool force = false)
    {
        //Tags which are assigned to a workItem may only be deleted using the force.
        //Trying to delete a tag in use without the force should return Conflict.
        var tags = context.Tags;

        var findTag = (from t in tags
                       where t.Id == tagId
                       select t).ToList();

        var foundTag = findTag[0];

        if (foundTag.Tasks.Count == 0 || force == true)
        {
            context.Remove(foundTag);
            context.SaveChanges();
            return Response.Deleted;
        }
        else
        {
            return Response.Conflict;
        }
    }
}