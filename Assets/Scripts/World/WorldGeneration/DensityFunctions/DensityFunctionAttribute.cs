using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using World.WorldGeneration.WorldDataParser;

namespace World.WorldGeneration.DensityFunctions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Struct)]
    public class DensityFunctionAttribute : Attribute
    {
    }
    public static class DensityFunctionManager
    {
        public static List<MemberInfo> GetMembersWithDensityFunctionAttribute() =>
            Assembly.GetExecutingAssembly().GetTypes()
                    .SelectMany(t => t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    .Where(m => m.GetCustomAttributes(typeof(DensityFunctionAttribute), false).Length > 0)
                    .ToList();
        public static bool IsStruct(this MemberInfo memberInfo) => memberInfo is Type { IsValueType: true, IsPrimitive: false };
        

        public static bool IsMethod(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Method;

        public static void LoadDensityFunctions()
        {
        
            var members = GetMembersWithDensityFunctionAttribute();
            foreach (var member in members)
            {
                if (member.IsStruct())
                {
                    TypeBinding.TypeBindings.Add(DensityFunctionHelper.CleanString(member.Name), (jObject) => DensityFunctionHelper.ParseStruct(jObject, (Type)member));    
                }
                else if (member.IsMethod())
                {
                    
                }   
            }
        }
    }
}