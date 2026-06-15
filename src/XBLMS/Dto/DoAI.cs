using System.Collections.Generic;
using XBLMS.Models;

namespace XBLMS.Dto
{
    public class DoAI
    {
        public class DoAI_Version_Result
        {
            public string Version { get; set; }
        }
        public class DoAI_RunningModels_Result
        {
            public List<DoAI_RunningModels_Result_Info> Models { get; set; }
            
        }
        public class DoAI_RunningModels_Result_Info
        {
            public string Name { get; set; }
            public long Size { get; set; }
            public DoAI_RunningModels_Result_Details Details { get; set; }
        }
        public class DoAI_RunningModels_Result_Details
        {
            public string Family { get; set; }
        }
        public class DoAI_Tm_Result
        {
            public bool Success { get; set; }
            public ExamTmAi Tm { get; set; }
            public string Msg { get; set; }
        }
        public class DoAI_Tm_AI_Result
        {
            public DoAI_Tm_AI_Result_Message Message { get; set; }
        }
        public class DoAI_Tm_AI_Result_Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }
        //public class DoAI_Tm_AI_Result_Message
        //{
        //    public string Role { get; set; }
        //    public DoAI_Tm_AI_Result_Content Content { get; set; }
        //}
        public class DoAI_Tm_AI_Result_Content
        {
            public string Content { get; set; }
            public string Answer { get; set; }
            public string Analysis { get; set; }
            public List<string> Option { get; set; }
        }
    }
}
