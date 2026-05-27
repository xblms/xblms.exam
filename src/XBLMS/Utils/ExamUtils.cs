using System.Collections.Generic;

namespace XBLMS.Utils
{
    public static class ExamUtils
    {
        public static bool IsAnswerAllTrue(string answer, string myAnswer, List<string> answerList)
        {
            if (StringUtils.ContainsIgnoreCase(answer, ";"))
            {
                answerList = ListUtils.GetStringList(answer, ";");
                answer = ListUtils.ToString(answerList);
            }
            if (StringUtils.ContainsIgnoreCase(answer, "；"))
            {
                answerList = ListUtils.GetStringList(answer, "；");
                answer = ListUtils.ToString(answerList);
            }
            if (StringUtils.ContainsIgnoreCase(answer, ","))
            {
                answerList = ListUtils.GetStringList(answer, ",");
                answer = ListUtils.ToString(answerList);
            }
            if (StringUtils.ContainsIgnoreCase(answer, "，"))
            {
                answerList = ListUtils.GetStringList(answer, "，");
                answer = ListUtils.ToString(answerList);
            }
            return StringUtils.EqualsIgnoreCase(answer, myAnswer);
        }
    }
}
