using System;

namespace Perlink.Oi.Juridico.Infra.External
{
    public class ReplaceTypeBuilder<TEntity, TypeFrom, TypeTo>
    {
        internal ReplaceTypeBuilder()
        {
        }

        private string PropertyName { get; set; } = string.Empty;

        private string TypeMemberName { get; set; } = string.Empty;

        public ReplaceTypeBuilder<TEntity, TypeFrom, TypeTo> UsePropertyName(string propertyName)
        {
            PropertyName = propertyName;
            return this;
        }

        public ReplaceTypeBuilder<TEntity, TypeFrom, TypeTo> WithTypeMemberName(string typeMemberName)
        {
            TypeMemberName = typeMemberName;
            return this;
        }

        public ReplaceType Build()
        {
            if (string.IsNullOrEmpty(PropertyName))
            {
                throw new ArgumentNullException(nameof(PropertyName));
            }
            if (string.IsNullOrEmpty(TypeMemberName))
            {
                throw new ArgumentNullException(nameof(TypeMemberName));
            }
            return new ReplaceType(typeof(TEntity), typeof(TypeFrom), typeof(TypeTo), PropertyName, TypeMemberName);
        }
    }
}