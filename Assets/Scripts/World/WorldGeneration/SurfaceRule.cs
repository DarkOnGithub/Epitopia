using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using World.Blocks;

namespace World.WorldGeneration
{
    public struct SurfaceRuleComponent
    {
        public string Type;
        public string Argument;
        public int? Height;
    }

    public class Rule
    {
        private SurfaceRuleComponent _component;
        private IBlock[] _blocks;
        private Func<IBlock> _getter;
        public Rule(SurfaceRuleComponent component)
        {
            _component = component;
            Parse();
        }

        private void Parse()
        {
            switch (_component.Type)
            {
                case "Constant":
                    _blocks = new[]
                    {
                        BlockRegistry.GetBlock(_component.Argument)                        
                    };
                    _getter = () => _blocks[0];
                    break;
            }
        }
        
        public IBlockState GetBlock()
        {
            return _getter().GetDefaultState();
        }
    }

    public class SurfaceRule
    {
        private List<(int, Rule)> _rules = new();
        private Rule _defaultRule;

        public SurfaceRule(SurfaceRuleComponent[] component)
        {
            var cumsum = 0;
            foreach (var c in component)
            {
                var rule = new Rule(c);
                if (c.Height == null)
                {
                    _defaultRule = rule;
                    continue;
                }

                if (c.Height == 0)
                    continue;
                cumsum += c.Height.Value;
                Debug.Log(cumsum);
                _rules.Add((cumsum, rule));
                _defaultRule = rule; // last rule is default
            }
        }

        public Rule GetRule(int surfaceLevel, int y)
        {
            var height = surfaceLevel - y;
            foreach (var (h, rule) in _rules)
            {
                if (h >= height)
                {
                    Debug.Log(h);
                    Debug.Log(height);
                    return rule;
                }
            }

            return _defaultRule;
        }
    }
}