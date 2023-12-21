namespace AIIntegrator
{
    public class VocabularyChecker
    {
        public HashSet<string> vocabulary = new HashSet<string>
    {
        "这里是需要过滤的敏感词",

    };

        public bool IsInVocabulary(string content)
        {
            bool result = false;
            foreach (var item in vocabulary)
            {
                if (content.Contains(item))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }

}
