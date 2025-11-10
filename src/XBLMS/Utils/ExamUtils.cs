using System.Collections.Generic;

namespace XBLMS.Utils
{
    public static class ExamUtils
    {
        public static bool IsAnswerAllTrue(string answer, string myAnswer, List<string> answerList)
        {
            var allTrue = true;
            foreach (var answerItem in answerList)
            {
                if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                {
                    allTrue = false;
                }
            }
            if (StringUtils.ContainsIgnoreCase(answer, ";") && !allTrue)
            {
                answerList = ListUtils.GetStringList(answer, ";");
                foreach (var answerItem in answerList)
                {
                    if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                    {
                        allTrue = false;
                    }
                }
            }
            if (StringUtils.ContainsIgnoreCase(answer, "，") && !allTrue)
            {
                answerList = ListUtils.GetStringList(answer, "，");
                foreach (var answerItem in answerList)
                {
                    if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                    {
                        allTrue = false;
                    }
                }
            }
            if (StringUtils.ContainsIgnoreCase(answer, "；") && !allTrue)
            {
                answerList = ListUtils.GetStringList(answer, "；");
                foreach (var answerItem in answerList)
                {
                    if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                    {
                        allTrue = false;
                    }
                }
            }
            return allTrue;
        }
    }
}
