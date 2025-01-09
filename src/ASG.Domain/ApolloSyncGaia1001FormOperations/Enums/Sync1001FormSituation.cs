using System.ComponentModel;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;

public enum Sync1001FormSituation
{
    [Description("忘打卡_當日已有上班打卡紀錄")] 
    AlreadyHasClockInRecord,
    
    [Description("忘打卡_當日已有下班打卡紀錄")] 
    AlreadyHasClockOutRecord,
    
    [Description("忘打卡_打卡時間已存在打卡紀錄或簽核中忘打卡申請單")] 
    AlreadyHasOtherAttendanceTypeRecordOrIsStillApproving,
    
    [Description("忘打卡_一般拋轉失敗")] 
    NormalFailSync
}