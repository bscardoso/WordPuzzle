using System.Collections.Generic;

namespace WordPuzzle
{
    public class Word
    {
        public string WordString { get; set; }
        public List<string> NextWords { get; set; }
    }
}
