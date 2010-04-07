
using System;
using System.Collections.Generic;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
   public static class ValidationManagerFactory
    {

       private static Dictionary<InfoDescriptor, List<Rule>> GetDictionary<TInfoDescriptor>(AutoKeyDictionary<string, TInfoDescriptor> properties, string ruleSet) where TInfoDescriptor : InfoDescriptor
       {
           var dictionary = new Dictionary<InfoDescriptor, List<Rule>>();
               foreach (var descriptor in properties)
               {
                   var rules = new List<Rule>();
                   foreach (var rule in descriptor.Rules.GetRulesForRuleSet(ruleSet))
                   {
                       rules.Add(rule);
                   }
                   dictionary.Add(descriptor, rules);
               }
           return dictionary;
       }

       public static MemberValidationManager GetPropertyValidationManager(object target)
       {
           return GetPropertyValidationManager(target, null);
       }

       public static MemberValidationManager GetPropertyValidationManager(object target, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(target.GetType().TypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { Target = target };
       }

       public static MemberValidationManager GetPropertyValidationManager<T>()
       {
       return GetPropertyValidationManager<T>(null);
       }

       public static MemberValidationManager GetPropertyValidationManager<T>(string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(typeof(T).TypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { TargetHandle = typeof(T).TypeHandle };
       }


       public static MemberValidationManager GetPropertyValidationManager(RuntimeTypeHandle runtimeTypeHandle)
       {
           return GetPropertyValidationManager(runtimeTypeHandle, null);
       }

       public static MemberValidationManager GetPropertyValidationManager(RuntimeTypeHandle runtimeTypeHandle, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(runtimeTypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { TargetHandle = runtimeTypeHandle };
       }

       public static MemberValidationManager GetFieldValidationManager<T>()
       {
           return GetFieldValidationManager<T>(null);
       }

       public static MemberValidationManager GetFieldValidationManager<T>(string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(typeof(T).TypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { TargetHandle = typeof(T).TypeHandle };
       }

       public static MemberValidationManager GetFieldValidationManager(object target)
       {
           return GetFieldValidationManager(target, null);
       }

       public static MemberValidationManager GetFieldValidationManager(object target, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(target.GetType().TypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { Target = target };
       }

       public static MemberValidationManager GetFieldValidationManager(RuntimeTypeHandle runtimeTypeHandle)
       {
           return GetFieldValidationManager(runtimeTypeHandle, null);
       }
       
       public static MemberValidationManager GetFieldValidationManager(RuntimeTypeHandle runtimeTypeHandle, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(runtimeTypeHandle);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { TargetHandle = runtimeTypeHandle };
       }
    }
}
