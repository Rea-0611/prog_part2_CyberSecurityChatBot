using System.Collections.Generic;

namespace prog_part2_CyberSecurityChatBot
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
        }
    }
}