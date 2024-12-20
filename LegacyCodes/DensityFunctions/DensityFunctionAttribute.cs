using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using World.WorldGeneration.WorldDataParser;

namespace World.WorldGeneration.DensityFunctions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DensityFunctionAttribute : Attribute
    {
    }

    public static class DensityFunctionManager
    {
        public static List<MemberInfo> GetMembersWithDensityFunctionAttribute()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                           .SelectMany(t => t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Instance | BindingFlags.Static))
                           .Where(m => m.GetCustomAttributes(typeof(DensityFunctionAttribute), false).Length > 0)
                           .ToList();
        }


        public static void LoadDensityFunctions()
        {
            var members = GetMembersWithDensityFunctionAttribute();
            foreach (var member in members)
                TypeBinding.TypeBindings.Add(DensityFunctionHelper.CleanString(member.Name),
                                             (jObject) => DensityFunctionHelper.ParseStruct(jObject, (Type)member));
        }
    }
}