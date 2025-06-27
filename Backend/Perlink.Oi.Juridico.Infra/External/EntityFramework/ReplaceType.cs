using System;

namespace Perlink.Oi.Juridico.Infra.External
{
    public class ReplaceType
    {
        internal ReplaceType(Type entityType, Type typeFrom, Type typeTo, string propertyName, string typeMemberName)
        {
            EntityType = entityType;
            TypeFrom = typeFrom;
            TypeTo = typeTo;
            PropertyName = propertyName;
            TypeMemberName = typeMemberName;
        }

        public Type EntityType { get; }

        public Type TypeFrom { get; }

        public Type TypeTo { get; }

        public string PropertyName { get; }

        public string TypeMemberName { get; }
    }
}