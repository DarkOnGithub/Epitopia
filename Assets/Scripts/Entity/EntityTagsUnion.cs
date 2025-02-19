using System;
using System.Collections.Generic;
using Entities.Entity;

namespace Entities
{
    
public class EntityTagsUnion
{
    private readonly Dictionary<EntityTags, EntityTags> _parent = new(); 
    private readonly Dictionary<EntityTags, int> _rank = new(); 
    private readonly Dictionary<EntityTags, HashSet<AbstractEntity>> _entityGroups = new(); 
    private readonly Dictionary<AbstractEntity, HashSet<EntityTags>> _entityTagMapping = new(); 

    public EntityTagsUnion(IEnumerable<EntityTags> tags)
    {
        foreach (var tag in tags)
        {
            _parent[tag] = tag; 
            _rank[tag] = 1;
            _entityGroups[tag] = new HashSet<AbstractEntity>();
        }
    }

    
    public EntityTags Find(EntityTags tag)
    {
        if (_parent[tag] != tag)
            _parent[tag] = Find(_parent[tag]); 
        return _parent[tag];
    }

    
    public void Union(EntityTags tag1, EntityTags tag2)
    {
        EntityTags root1 = Find(tag1);
        EntityTags root2 = Find(tag2);

        if (root1 == root2) return; 

        if (_rank[root1] > _rank[root2])
        {
            _parent[root2] = root1;
            MergeEntities(root1, root2);
        }
        else if (_rank[root1] < _rank[root2])
        {
            _parent[root1] = root2;
            MergeEntities(root2, root1);
        }
        else
        {
            _parent[root2] = root1;
            MergeEntities(root1, root2);
            _rank[root1]++;
        }
    }

    
    private void MergeEntities(EntityTags target, EntityTags source)
    {
        if (!_entityGroups.ContainsKey(target))
            _entityGroups[target] = new HashSet<AbstractEntity>();

        foreach (var entity in _entityGroups[source])
        {
            _entityGroups[target].Add(entity);
            _entityTagMapping[entity].Add(target);
        }

        _entityGroups.Remove(source);
    }

    
    
    public void AddEntity(AbstractEntity entity)
    {
        if (!_entityTagMapping.ContainsKey(entity))
            _entityTagMapping[entity] = new HashSet<EntityTags>();
        var tags = entity.Tags;
        foreach (var tag in tags)
        {
            EntityTags root = Find(tag); 

            if (!_entityGroups.ContainsKey(root))
                _entityGroups[root] = new HashSet<AbstractEntity>();

            _entityGroups[root].Add(entity);
            _entityTagMapping[entity].Add(root); 
        }
    }


    
    public HashSet<AbstractEntity> GetEntities(EntityTags tag)
    {
        EntityTags root = Find(tag);
        return _entityGroups.ContainsKey(root) ? _entityGroups[root] : new HashSet<AbstractEntity>();
    }

    
    public HashSet<AbstractEntity> GetUnionOfTags(IEnumerable<EntityTags> tags)
    {
        HashSet<AbstractEntity> result = new();

        foreach (var tag in tags)
        {
            EntityTags root = Find(tag);
            if (_entityGroups.ContainsKey(root))
                result.UnionWith(_entityGroups[root]);
        }

        return result;
    }
}

}