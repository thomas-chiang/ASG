using System.ComponentModel;

namespace ASG.Domain.Gaia1001Forms.Enums;

public enum Gaia1001FormStatus
{
    // 未完成
    [Description("NC")] NotComplete,

    // 已儲存
    [Description("SA")] Save,

    // 待簽核
    [Description("WA")] WaitingApprove,

    // 簽核中
    [Description("UA")] UnderApproving,

    // 退回 (簽核人拒簽)
    [Description("RJ")] Rejected,

    // 簽核完成
    [Description("AP")] Approved,

    // 撤回
    [Description("RC")] Recall,

    // 已刪除
    [Description("DE")] Deleted
}