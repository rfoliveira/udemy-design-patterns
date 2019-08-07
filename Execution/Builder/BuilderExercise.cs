using System;
using System.Collections.Generic;

namespace Execution.Builder
{
    public class FieldInfo
    {
        public string FieldType { get; set; }
        public string FieldName { get; set; }

        public FieldInfo()
        {
            //
        }

        public FieldInfo(string fieldType, string fieldName)
        {
            FieldType = fieldType ?? throw new ArgumentNullException(paramName: nameof(fieldType));
            FieldName = fieldName ?? throw new ArgumentNullException(paramName: nameof(fieldName));
        }


        public override string ToString()
        {
            return $"public {FieldType} {FieldName};";
        }
    }

    public class ClassInfo
    {
        private const int DEFAULT_INDENT_SIZE = 2;

        public string Name;
        public List<FieldInfo> Fields = new List<FieldInfo>();
        public int IndentSize;

        public ClassInfo(string name, int indentSize = 0)
        {
            Name = name;
            Fields = new List<FieldInfo>();
            IndentSize = indentSize;
        }

        public override string ToString()
        {
            return ToStringInpl();
        }

        private string ToStringInpl()
        {
            var result = $"public class {Name}" +
                "\n{";
            var spaces = new string(' ', IndentSize > 0 ? IndentSize : DEFAULT_INDENT_SIZE);

            foreach (var field in Fields)
            {
                result += "\n" + spaces + field.ToString();
            }

            result += "\n}";
            return result;
        }
    }

    public class CodeBuilder
    {
        private ClassInfo classInfo;

        public CodeBuilder(string className)
        {
            classInfo = new ClassInfo(className);
        }

        public CodeBuilder AddField(string name, string type)
        {
            classInfo.Fields.Add(
                new FieldInfo(name, type)
            );

            return this;
        }

        public override string ToString()
        {
            return classInfo.ToString();
        }
    }
}
