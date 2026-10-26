namespace ASSLINQ
{
    // words with the same characters are equal (from == form)
    internal class AnagramComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            return Sort(x) == Sort(y);
        }

        public int GetHashCode(string obj)
        {
            return Sort(obj).GetHashCode();
        }

        private string Sort(string? word)
        {
            char[] chars = word!.Trim().ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }
}
