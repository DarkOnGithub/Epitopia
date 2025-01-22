using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace World.WorldGeneration.Biomes
{
    public class RTree<T>
    {
        public const int CHILDREN_PER_NODE = 10;
        private readonly RNode<T> root;
        private RLeaf<T> lastLeaf;

        public RTree(List<Tuple<ParamPoint, Func<T>>> points)
        {
            if (points.Count == 0)
                throw new ArgumentException("At least one point is required to build search tree");
            var nodes = points.Select(p => new RLeaf<T>(p.Item1, p.Item2)).Cast<RNode<T>>().ToList();
            root = Build(nodes);
        }

        private static RNode<T> Build(List<RNode<T>> nodes)
        {
            if (nodes.Count == 1)
                return nodes[0];

            if (nodes.Count <= CHILDREN_PER_NODE)
            {
                nodes = nodes.OrderBy(n => GetKey(n, Climate.ParameterSpace)).ToList();
                return new RSubTree<T>(nodes);
            }

            var n3 = -1;
            var f = float.PositiveInfinity;

            for (var n2 = 0; n2 < Climate.ParameterSpace; n2++)
            {
                nodes = Sort(nodes, n2, false);
                var result1 = Bucketize(nodes);

                var f2 = result1.Sum(subTree => Area(subTree.Space));
                if (f > f2)
                {
                    f = f2;
                    n3 = n2;
                }
            }

            if (n3 == -1)
                throw new InvalidOperationException("No suitable parameter found for building tree");

            nodes = Sort(nodes, n3, false);
            var result = Bucketize(nodes);
            result = SortSubTrees(result, n3, true);
            return new RSubTree<T>(result.Select(subTree => Build(subTree.Children)).ToList());
        }

        private static float GetKey(RNode<T> node, int parameterSpace)
        {
            float key = 0;
            Debug.Log(parameterSpace);
            Debug.Log(node.Space.Count);
            for (var i = 0; i < parameterSpace; i++) 
                key += Math.Abs((node.Space[i].Min + node.Space[i].Max) / 2.0f);
            return key;
        }

        private static List<RNode<T>> Sort(List<RNode<T>> nodes, int i, bool abs)
        {
            Func<RNode<T>, float> keySelector = node =>
            {
                var f = (node.Space[i].Min + node.Space[i].Max) / 2;
                return abs ? Math.Abs(f) : f;
            };

            return nodes.OrderBy(keySelector).ToList();
        }

        private static List<RSubTree<T>> Bucketize(List<RNode<T>> nodes)
        {
            var arrayList = new List<RSubTree<T>>();
            var arrayList2 = new List<RNode<T>>();
            var n = (int)Math.Pow(10.0, Math.Floor(Math.Log10(nodes.Count - 0.01)));

            foreach (var node in nodes)
            {
                arrayList2.Add(node);
                if (arrayList2.Count < n)
                    continue;

                arrayList.Add(new RSubTree<T>(arrayList2));
                arrayList2 = new List<RNode<T>>();
            }

            if (arrayList2.Count != 0)
                arrayList.Add(new RSubTree<T>(arrayList2));

            return arrayList;
        }

        private static List<RSubTree<T>> SortSubTrees(List<RSubTree<T>> subTrees, int i, bool abs)
        {
            Func<RSubTree<T>, float> keySelector = subTree =>
            {
                var f = (subTree.Space[i].Min + subTree.Space[i].Max) / 2;
                return abs ? Math.Abs(f) : f;
            };

            return subTrees.OrderBy(keySelector).ToList();
        }

        private static float Area(List<Param> paramsList)
        {
            float f = 0;
            foreach (var param in paramsList)
                f += Math.Abs(param.Max - param.Min);
            return f;
        }

        public T Search(TargetPoint target)
        {
            var values = target.ToArray();
            var leaf = root.Search(values, lastLeaf, (node, vals) => node.Distance(vals));
            lastLeaf = leaf;
            return leaf.Thing();
        }
    }

    public abstract class RNode<T>
    {
        public List<Param> Space { get; }

        public RNode(List<Param> space)
        {
            Space = space;
        }

        public virtual RLeaf<T> Search(List<float> values, RLeaf<T> closestLeaf,
            Func<RNode<T>, List<float>, float> distance)
        {
            throw new NotImplementedException();
        }

        public float Distance(List<float> values)
        {
            float result = 0;
            for (var i = 0; i < Climate.ParameterSpace; i++)
            {
                var d = Space[i].Distance(values[i]);
                result += d * d;
            }

            return result;
        }
    }

    public class RSubTree<T> : RNode<T>
    {
        public List<RNode<T>> Children { get; }

        public RSubTree(List<RNode<T>> children) : base(BuildSpace(children))
        {
            Children = children;
        }

        private static List<Param> BuildSpace(List<RNode<T>> nodes)
        {
            var space = new List<Param>();
            for (var i = 0; i < Climate.ParameterSpace; i++)
                space.Add(new Param(float.PositiveInfinity, float.NegativeInfinity));

            foreach (var node in nodes)
                for (var i = 0; i < Climate.ParameterSpace; i++)
                    space[i] = space[i].Union(node.Space[i]);

            return space;
        }

        public override RLeaf<T> Search(List<float> values, RLeaf<T> closestLeaf,
            Func<RNode<T>, List<float>, float> distance)
        {
            var dist = closestLeaf != null ? distance(closestLeaf, values) : float.PositiveInfinity;
            var leaf = closestLeaf;

            foreach (var node in Children)
            {
                var d1 = distance(node, values);
                if (dist <= d1)
                    continue;

                var leaf2 = node.Search(values, leaf, distance);
                if (leaf2 == null)
                    continue;

                var d2 = node == leaf2 ? d1 : distance(leaf2, values);
                if (d2 == 0)
                    return leaf2;
                if (dist <= d2)
                    continue;

                dist = d2;
                leaf = leaf2;
            }

            return leaf ?? throw new InvalidOperationException("No leaf found");
        }
    }

    public class RLeaf<T> : RNode<T>
    {
        public Func<T> Thing { get; }

        public RLeaf(ParamPoint point, Func<T> thing) : base(point.Space())
        {
            Thing = thing;
        }

        public override RLeaf<T> Search(List<float> values, RLeaf<T> closestLeaf,
            Func<RNode<T>, List<float>, float> distance)
        {
            return this;
        }
    }
}