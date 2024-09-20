using System;
using XBLMS.Enums;

namespace XBLMS.Dto
{
    public class CreateTaskLog
    {
        public CreateTaskLog(CreateType createType, string timeSpan, bool isSuccess, string errorMessage, DateTime addDate)
        {
            CreateType = createType;
            TimeSpan = timeSpan;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            AddDate = addDate;
        }

        public CreateType CreateType { get; set; }

        public string TimeSpan { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime AddDate { get; set; }
    }
}
