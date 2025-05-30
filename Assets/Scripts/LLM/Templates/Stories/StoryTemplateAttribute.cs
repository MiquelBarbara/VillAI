using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using LLM.Utilities;

namespace LLM.Templates.Stories
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StoryAttribute : Attribute
    {
        public StoryAttribute(string story)
        {
            Story = story;
        }

        private string Story { get; }

        // Simple thread-safe cache for reflection lookups.
        private static readonly ConcurrentDictionary<(Type, string), MemberInfo> _memberCache = new();

        /// <summary>
        /// Generates a story by replacing placeholders (e.g. [PropertyName]) in the template with the corresponding property or field values.
        /// </summary>
        public string GenerateStory(object target)
        {
            if (target == null || string.IsNullOrEmpty(Story))
                return string.Empty;
            
            var type = target.GetType();

            // Allow any characters inside the brackets.
            return Regex.Replace(Story, @"\[([^\]]+)\]", match =>
            {
                var member = GetMember(match, type);

                switch (member)
                {
                    case FieldInfo field:
                    {
                        var value = field.GetValue(target);
                        return JsonParser.ToJson(value);
                    }
                    case PropertyInfo property:
                    {
                        var value = property.GetValue(target, null);
                        return JsonParser.ToJson(value);
                    }
                    default:
                        return match.Value;
                }
            });
        }

        private MemberInfo GetMember(Match match, Type type)
        {
            var memberName = match.Groups[1].Value;
            var key = (type, memberName);
            if (_memberCache.TryGetValue(key, out MemberInfo member)) return member;
            
            member = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     ?? (MemberInfo)type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _memberCache[key] = member;
            return member;
        }
    }
}
