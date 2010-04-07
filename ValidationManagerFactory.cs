
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
           var typeDescriptor = TypeCache.GetType(target.GetType());
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { Target = target };
       }

       public static MemberValidationManager GetPropertyValidationManager<T>()
       {
       return GetPropertyValidationManager<T>(null);
       }

       public static MemberValidationManager GetPropertyValidationManager<T>(string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(typeof(T));
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { TargetType = typeof(T) };
       }


       public static MemberValidationManager GetPropertyValidationManager(Type runtimeType)
       {
           return GetPropertyValidationManager(runtimeType, null);
       }

       public static MemberValidationManager GetPropertyValidationManager(Type runtimeType, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(runtimeType);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Properties, ruleSet)) { TargetType = runtimeType };
       }

       public static MemberValidationManager GetFieldValidationManager<T>()
       {
           return GetFieldValidationManager<T>(null);
       }

       public static MemberValidationManager GetFieldValidationManager<T>(string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(typeof(T));
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { TargetType = typeof(T) };
       }

       public static MemberValidationManager GetFieldValidationManager(object target)
       {
           return GetFieldValidationManager(target, null);
       }

       public static MemberValidationManager GetFieldValidationManager(object target, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(target.GetType());
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { Target = target };
       }

       public static MemberValidationManager GetFieldValidationManager(Type runtimeType)
       {
           return GetFieldValidationManager(runtimeType, null);
       }
       
       public static MemberValidationManager GetFieldValidationManager(Type runtimeType, string ruleSet)
       {
           var typeDescriptor = TypeCache.GetType(runtimeType);
           return new MemberValidationManager(GetDictionary(typeDescriptor.Fields, ruleSet)) { TargetType = runtimeType };
       }
    }
}
