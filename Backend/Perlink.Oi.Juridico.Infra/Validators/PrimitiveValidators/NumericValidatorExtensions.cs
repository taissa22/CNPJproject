namespace Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators {
    public static class NumericValidatorExtensions {
        public static bool HasMaxLength(this int value, int length) {
            return length >= value.ToString().Length;
        }
    }
}
